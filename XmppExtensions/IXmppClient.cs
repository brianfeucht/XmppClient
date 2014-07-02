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
        string ConnectedUser { get; }
        event EventHandler<IIncomingIqMessage> OnIq;
        event EventHandler OnConnected;
        void Connect();
        void Disconnect();
        void Send<T>(string to, T message) where T : XmppXElement;
        Task<TR> SendWithResponse<T, TR>(string to, T message) where T : XmppXElement where TR : XmppXElement;
        void Respond<T>(IIncomingIqMessage originalMessage, T response) where T : XmppXElement;
    }
}
