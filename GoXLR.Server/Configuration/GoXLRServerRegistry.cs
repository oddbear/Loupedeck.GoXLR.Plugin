using Fleck;
using GoXLR.Server.Handlers.Commands;
using GoXLR.Server.Handlers.Interfaces;

using StructureMap;

namespace GoXLR.Server.Configuration
{
    public class GoXLRServerRegistry : Registry
    {
        public GoXLRServerRegistry()
        {
            //The server itself is a singleton, but it forks out scopes per connectetion (ex. GoXLRState):
            ForConcreteType<GoXLRServer>().Configure.Singleton();

            //The state is scoped to the connected client:
            ForConcreteType<CommandHandler>().Configure.ContainerScoped();
            
            //Adds all the 
            Scan(scanner =>
            {
                scanner.Assembly(typeof(INotificationHandler).Assembly);
                scanner.WithDefaultConventions().OnAddedPluginTypes(c => c.ContainerScoped());
                scanner.AddAllTypesOf<INotificationHandler>();
            });
        }
    }
}
