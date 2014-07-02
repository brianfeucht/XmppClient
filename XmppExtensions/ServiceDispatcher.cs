using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Matrix;
using Matrix.Xml;
using Matrix.Xmpp.Client;

namespace XmppExtensions
{
    public class ServiceDispatcher : IDisposable
    {
        private readonly IXmppClient client;
        private readonly IMessageProcessor[] supportedProcessors;
        private readonly IDictionary<Type, IList<IMessageProcessor>> typeToMessageProcessorMap;

        public ServiceDispatcher(IXmppClient client, IMessageProcessor[] supportedProcessors)
        {
            this.client = client;
            this.supportedProcessors = supportedProcessors;

            RegisterProcessors(this.supportedProcessors);

            typeToMessageProcessorMap = BuildTypeMap(this.supportedProcessors);

            this.client.OnIq += ClientOnIq;
        }

        private void RegisterProcessors(IEnumerable<IMessageProcessor> supportedProcessors)
        {
            MethodInfo registerMethod = typeof(Matrix.Xml.Factory).GetMethod("RegisterElement", new[] { typeof(string), typeof(string)});
            
            foreach (var process in supportedProcessors)
            {
                foreach (var type in process.SupportedTypes())
                {
                    MethodInfo genericMethod = registerMethod.MakeGenericMethod(type);
                    var parameters = new object[] {type.Namespace, type.Name};
                    genericMethod.Invoke(null, parameters);
                }
            }
        }

        private void ClientOnIq(object sender, IIncomingIqMessage e)
        {
            var messageType = e.Query.GetType();

            if (!typeToMessageProcessorMap.ContainsKey(messageType))
            {
                return;
            }

            foreach (var processor in typeToMessageProcessorMap[messageType])
            {
                processor.ProcessMessage(client, e);
            }
        }

        public void Dispose()
        {
            this.client.OnIq -= ClientOnIq;
        }

        private static IDictionary<Type, IList<IMessageProcessor>> BuildTypeMap(IEnumerable<IMessageProcessor> messageProcessor)
        {
            var map = new Dictionary<Type, IList<IMessageProcessor>>();
            
            foreach (var processor in messageProcessor)
            {
                foreach (var type in processor.SupportedTypes())
                {
                    if (!map.ContainsKey(type))
                    {
                        map.Add(type, new List<IMessageProcessor>());
                    }
                    
                    map[type].Add(processor);
                }
            }

            return map;
        }
    }
}
