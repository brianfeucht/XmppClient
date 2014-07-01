using Matrix.Xml;
using Matrix.Xmpp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Matrix.Xmpp.Disco;
using Matrix.Xmpp.XHtmlIM;

namespace XmppExtensions
{
    public interface IMessageProcessor
    {
        void ProcessMessage(IXmppClient client, Iq iq);
        Feature FeatureDefinition { get; }
        IEnumerable<XmppXElement> SupportedTypes();
    }
}
