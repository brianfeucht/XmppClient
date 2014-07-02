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
        event EventHandler<IIncomingIqMessage> OnIq;
        event EventHandler OnConnected;
        void Connect();
        void Disconnect();
        void Send<T>(string to, T message) where T : XmppXElement;
        string ConnectedUser { get; }
    }
}
