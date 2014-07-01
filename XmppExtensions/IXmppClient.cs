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
    public interface IXmppClient
    {
        event EventHandler<IqEventArgs> OnIq;
        event EventHandler<JidEventArgs> OnBind;
        void Connect();
        void Disconnect();
        void Send(XmppXElement el);
        Jid ConnectedUser { get; }
    }
}
