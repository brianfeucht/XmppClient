using Matrix.Xml;
using Matrix.Xmpp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Matrix.Xmpp.Disco;

namespace XmppExtensions
{
    public interface IMessageProcessor
    {
        void ProcessMessage(IXmppClient client, IIncomingIqMessage iq);
        IEnumerable<Type> SupportedTypes();
    }
}
