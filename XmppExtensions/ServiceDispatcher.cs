using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Matrix;
using Matrix.Xml;
using Matrix.Xmpp.Client;

namespace XmppExtensions
{
    public class ServiceDispatcher : IDisposable
    {
        private readonly IXmppClient _client;
        private readonly IMessageProcessor[] _supportedProcessors;
        private readonly IMessageProcessor serviceDiscoveryProcessor;

        public ServiceDispatcher(IXmppClient client, IMessageProcessor[] supportedProcessors)
        {
            _client = client;
            _supportedProcessors = supportedProcessors;

            RegisterProcessors(_supportedProcessors);

            _client.OnBind += ClientOnBind;
            _client.OnIq += ClientOnIq;
        }

        private void RegisterProcessors(IMessageProcessor[] supportedProcessors)
        {
            foreach (var process in supportedProcessors)
            {
                foreach(var type in process.SupportedTypes())
                {
                    Factory.RegisterElement(type., type.LocalName);
                }

            }
        }

        private void ClientOnIq(object sender, IqEventArgs e)
        {
            if (e.Iq.Query is DiscoInfoIq)
            {
                serviceDiscoveryProcessor.ProcessMessage(_client, e.Iq);
                return;
            }

            foreach (var processor in _supportedProcessors)
            {
                var continueProcessing = processor.ProcessMessage(_client, e.Iq);

                if (!continueProcessing)
                    break;
            }
        }

        private void ClientOnBind(object sender, JidEventArgs jidEventArgs)
        {
        }

        public void Dispose()
        {
            _client.OnBind -= ClientOnBind;
            _client.OnIq -= ClientOnIq;
        }
    }
}
