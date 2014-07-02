using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Matrix.Xmpp;
using XmppExtensions;
using System.Threading;

namespace XmppCommands
{
    public class PingPongMessageProcessor : IMessageProcessor
    {
        private ILog log = LogManager.GetLogger(typeof(PingPongMessageProcessor));
        private readonly Random random;

        public PingPongMessageProcessor(Random random)
        {
            this.random = random;
        }

        public void ProcessMessage(IXmppClient client, IIncomingIqMessage iq)
        {
            var pingPong = (PingPong)iq.Query;

            log.InfoFormat("{0}: Ping {1}", client.ConnectedUser, pingPong.LastPing);
            Thread.Sleep(random.Next(1000, 10000));

            pingPong.LastPing = DateTime.Now;

            client.Send(iq.From, pingPong);
        }

        public Matrix.Xmpp.Disco.Feature FeatureDefinition
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerable<Type> SupportedTypes()
        {
            yield return typeof (PingPong);
        }
    }
}
