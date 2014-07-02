using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XmppExtensions;

namespace XmppCommands
{
    public class PingMessageProcessor : IMessageProcessor
    {
        public void ProcessMessage(IXmppClient client, IIncomingIqMessage iq)
        {
            var ping = (Ping)iq.Query;
            ping.RespondedOn = DateTime.Now;

            client.Respond(iq, ping);
        }

        public IEnumerable<Type> SupportedTypes()
        {
            yield return typeof (Ping);
        }
    }
}
