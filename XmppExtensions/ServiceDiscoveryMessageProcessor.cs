﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Matrix.Xmpp;
using Matrix.Xmpp.Client;
using Matrix.Xmpp.Disco;

namespace XmppExtensions
{
    public class ServiceDiscoveryMessageProcessor : IMessageProcessor
    {
        /// <summary>
        /// Type of client this is.  Defaults to bot
        /// </summary>
        /// <remarks>http://xmpp.org/registrar/disco-categories.html</remarks>
        private string _identityCategory = "bot";
        public string IdentityCategory
        {
            get { return _identityCategory; }
            set { _identityCategory = value; }
        }

        private readonly IMessageProcessor[] _supportedMessageProcessors;

        public ServiceDiscoveryMessageProcessor(IMessageProcessor[] supportedMessageProcessors)
        {
            _supportedMessageProcessors = supportedMessageProcessors;
        }

        public void ProcessMessage(IXmppClient client, IIncomingIqMessage iq)
        {
            var result = new DiscoInfoIq(iq.From, client.ConnectedUser, IqType.result);
            
            result.Add(new Identity("xml", IdentityCategory));

            foreach (var processor in _supportedMessageProcessors)
            {
                result.Add(new Feature(processor.GetType().Name));
            }

            client.Send(iq.From, result);
        }


        public Feature FeatureDefinition
        {
            get
            {
                return new Feature("http://jabber.org/protocol/disco#info");
            }
        }


        public IEnumerable<Type> SupportedTypes()
        {
            yield return typeof (DiscoInfoIq);
        }
    }
}
