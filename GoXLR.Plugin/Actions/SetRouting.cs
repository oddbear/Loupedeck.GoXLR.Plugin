using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Loupedeck.GoXLR.Plugin.Config;
using Loupedeck.GoXLR.Plugin.GoXLR;
using Loupedeck.GoXLR.Plugin.GoXLR.Enums;
using Loupedeck.GoXLR.Plugin.GoXLR.EventArgs;
using Loupedeck.GoXLR.Plugin.GoXLR.Extensions;
using Loupedeck.GoXLR.Plugin.GoXLR.Models;
using Loupedeck.GoXLR.Plugin.Helpers;
using EventHandler = Loupedeck.GoXLR.Plugin.GoXLR.EventHandler;

namespace Loupedeck.GoXLR.Plugin.Actions
{
    public class SetRouting : MultistateActionEditorCommand
    {
        private const string InputDevice = "input_device";
        private const string OutputDevice = "output_device";
        private const string Mode = "mode";

        public SetRouting()
        {
            DisplayName = "Routing Table";
            Description = "Edit the Routing Table of your GoXLR-Device";
            GroupName = "";

            AddState("Active", "The routing that is currently active");
            AddState("Not Active", "The routing that isn't currently active");

            ActionEditor.AddControl(new ActionEditorListbox(InputDevice, "Input:").SetRequired());
            ActionEditor.AddControl(new ActionEditorListbox(OutputDevice, "Output:").SetRequired());
            ActionEditor.AddControl(new ActionEditorListbox(Mode, "Mode:").SetRequired());
            ActionEditor.ListboxItemsRequested += OnRoutingChanged;
            ActionEditor.ControlValueChanged += OnControlValueChanged;
        }

        private void OnControlValueChanged(object sender, ActionEditorControlValueChangedEventArgs e)
        {
            if (e.ControlName.EqualsNoCase(InputDevice))
            {
                ActionEditor.ListboxItemsChanged(OutputDevice);
            }
            else if (e.ControlName.EqualsNoCase(OutputDevice))
            {
                ActionEditor.ListboxItemsChanged(InputDevice);
            }
        }

        private void OnRoutingChanged(object sender, ActionEditorListboxItemsRequestedEventArgs e)
        {
            if (e.ControlName.EqualsNoCase(InputDevice))
            {
                var selectedOutput = e.ActionEditorState.GetControlValue(OutputDevice);
                var inputOptions = Routing.GetRoutingInputOptions();

                foreach (var input in inputOptions)
                {
                    var s = !selectedOutput.IsNullOrEmpty();
                    var t = !Routing.IsValid(input, selectedOutput);
                    if (!selectedOutput.IsNullOrEmpty() && !Routing.IsValid(input, selectedOutput)) continue;

                    e.AddItem(input, input, "");
                }
            }
            else if (e.ControlName.EqualsNoCase(OutputDevice))
            {
                var selectedInput = e.ActionEditorState.GetControlValue(InputDevice);
                var outputOptions = Routing.GetRoutingOutputOptions();

                foreach (var output in outputOptions)
                {
                    if (!selectedInput.IsNullOrEmpty() && !Routing.IsValid(selectedInput, output)) continue;

                    e.AddItem(output, output, "");
                }
            }
            else if (e.ControlName.EqualsNoCase(Mode))
            {
                var modeOptions = Routing.GetRoutingModeOptions();

                foreach (var mode in modeOptions)
                {
                    e.AddItem(mode, mode, "");
                }
            }
        }

        /// <summary>
        /// Once loaded subscribe to RoutingChanged events
        /// </summary>
        /// <returns></returns>
        protected override bool OnLoad()
        {
            EventHandler.OnRoutingChanged += RefreshIcons;
            EventHandler.RefreshIcons += RefreshIcons;
            return true;
        }

        /// <summary>
        /// Once loaded subscribe to RoutingChanged events
        /// </summary>
        /// <returns></returns>
        protected override bool OnUnload()
        {
            EventHandler.OnRoutingChanged -= RefreshIcons;
            EventHandler.RefreshIcons -= RefreshIcons;
            return true;
        }

        /// <summary>
        /// Generate an Image for the routing option
        /// </summary>
        /// <param name="actionParameters"></param>
        /// <param name="stateIndex"></param>
        /// <param name="imageWidth"></param>
        /// <param name="imageHeight"></param>
        /// <returns></returns>
        protected override BitmapImage GetCommandImage(ActionEditorActionParameters actionParameters, int stateIndex, int imageWidth,
            int imageHeight)
        {
            if (!Routing.TryParseContext(GetRoutingString(actionParameters), out var routing))
                return base.GetCommandImage(actionParameters, stateIndex, imageWidth, imageHeight);

            using (var bitmapBuilder = new BitmapBuilder(imageWidth == 80 ? PluginImageSize.Width90 : PluginImageSize.Width60))
            {
                GoXlrPlugin.RoutingStates.TryGetValue(routing.ToString(), out var state);

                var background = ImageHelper.GetRoutingImage(
                    imageWidth,
                    routing,
                    state,
                    GoXlrConfig.Routing.Color.Font,
                    GoXlrConfig.Routing.Color.Active,
                    GoXlrConfig.Routing.Color.InActive,
                    GoXlrConfig.Routing.Color.Secondary,
                    GoXlrConfig.Routing.Color.Background,
                    GoXlrConfig.Routing.Color.NotAvailable,
                    GoXlrConfig.Routing.ActiveIconPath,
                    GoXlrConfig.Routing.InActiveIconPath
                );

                bitmapBuilder.SetBackgroundImage(BitmapImage.FromArray(background));

                return bitmapBuilder.ToImage();
            }
        }

        /// <summary>
        /// Only Run when the GoXLR Software is connected and the routing can be parsed
        /// </summary>
        /// <param name="actionParameters"></param>
        protected override bool RunCommand(ActionEditorActionParameters actionParameters)
        {
            return Task.Run(async () =>
            {
                if (Plugin.PluginStatus.Status != PluginStatus.Normal)
                    return false;

                if (!Routing.TryParseContext(GetRoutingString(actionParameters), out var routing))
                    return false;

                if (routing.Mode != RoutingMode.Toggle)
                {
                    var state = GoXlrPlugin.RoutingStates[routing.ToString()];

                    if ((routing.Mode == RoutingMode.Off && state == State.Off)
                        || (routing.Mode == RoutingMode.On && state == State.On))
                        return false;
                }

                await CommandHandler.Send(
                    new GoXLR.Commands.SetRouting(routing),
                    GoXlrPlugin.WsConnection
                );

                return true;
            }).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Invoke that every image has changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void RefreshIcons(object sender, EventArgs eventArgs)
        {
            if (eventArgs.GetType() == typeof(OnRoutingChangedEventArgs))
            {
                var args = (OnRoutingChangedEventArgs) eventArgs;
                SetCurrentState(
                    new ActionEditorActionParameters(
                        new Dictionary<string, string>
                        {
                            [InputDevice] = args.Routing.Input.GetDescription(),
                            [OutputDevice] = args.Routing.Output.GetDescription()
                        }), (int) args.State);
            }

            ActionImageChanged();
        }

        private string GetRoutingString(ActionEditorActionParameters actionParameters)
        {
            return $"{actionParameters.Parameters[InputDevice]}|{actionParameters.Parameters[OutputDevice]}|{actionParameters.Parameters[Mode]}";
        }
    }
}