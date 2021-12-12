using GoXLR.Server.Handlers.Attributes;
using GoXLR.Server.Handlers.Interfaces;
using GoXLR.Server.Handlers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace GoXLR.Server.Configuration
{
    public class NotificationHandlerRouter
    {
        private readonly IEnumerable<INotificationHandler> _notifies;

        public NotificationHandlerRouter(
            IEnumerable<INotificationHandler> notifies)
        {
            _notifies = notifies;
        }

        public async Task<bool> RouteToHandler(MessageNotification message, CancellationToken cancellationToken)
        {
            var handeled = false;
            
            foreach (var notify in _notifies)
            {
                var notifyType = notify.GetType();

                //Attribute must exist, and be the right value:
                if (!TryGetAttribute<EventAttribute>(notifyType, out var eventAttribute)
                    || message.Event != eventAttribute.Event)
                    continue;

                //Attribute is optional, but if it exists, it needs to have the right value:
                if (TryGetAttribute<ActionAttribute>(notifyType, out var actionAttribute)
                    && message.Action != actionAttribute.Action)
                    continue;
                
                await notify.Handle(message, cancellationToken);

                handeled = true;
            }

            return handeled;
        }

        private bool TryGetAttribute<TAttribute>(Type type, out TAttribute attribute)
            where TAttribute : Attribute
        {
            var methods = type.GetMethods();

            attribute = methods
                .Select(method => method.GetCustomAttribute<TAttribute>())
                .FirstOrDefault(a => a != null);

            return attribute != null;
        }
    }
}
