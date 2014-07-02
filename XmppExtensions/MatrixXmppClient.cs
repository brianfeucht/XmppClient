using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Matrix;
using Matrix.Xmpp;
using Matrix.Xmpp.Client;
using Matrix.Xml;
using Matrix.Xmpp.Sasl;
using Matrix.Xmpp.StreamManagement.Ack;

namespace XmppExtensions
{
    public class MatrixXmppClient : IXmppClient, IDisposable
    {
        private ILog log = LogManager.GetLogger(typeof(MatrixXmppClient));
        private ILog messageLog = LogManager.GetLogger(typeof (XmppClient));

        private readonly XmppClient client;

        public event EventHandler<IIncomingIqMessage> OnIq;
        public event EventHandler OnConnected;
        private Jid connectedJid;

        public MatrixXmppClient(XmppClient client)
        {
            this.client = client;
            client.OnBind += ClientOnBind;
            client.OnAuthError += ClientOnAuthError;
            client.OnRegisterError += ClientOnRegisterError;
            client.OnBindError += ClientOnBindError;
            client.OnMessage += ClientOnMessage;
            client.OnIq += ClientOnIq;
        }

        private void ClientOnIq(object sender, IqEventArgs e)
        {
            if (OnIq == null || e.Iq.Query == null)
            {
                return;
            }

            var message = new MatrixIqMessage(e.Iq);
            OnIq.Invoke(this, message);
        }

        private void ClientOnMessage(object sender, MessageEventArgs messageEventArgs)
        {
            messageLog.Debug(messageEventArgs.Message);
        }

        private void ClientOnRegisterError(object sender, IqEventArgs e)
        {
            log.ErrorFormat("Unable to register {0}", e.Iq);
        }

        private void ClientOnAuthError(object sender, SaslEventArgs e)
        {
            log.ErrorFormat("Unable to connect {0}", e.Failure);
        }

        private void ClientOnBindError(object sender, IqEventArgs e)
        {
            log.ErrorFormat("Unable to connect {0}", e.Iq);
        }

        private void ClientOnBind(object sender, JidEventArgs jidEventArgs)
        {
            this.connectedJid = jidEventArgs.Jid;
            log.InfoFormat("Logged in as {0}", jidEventArgs.Jid);

            if (OnConnected != null)
            {
                OnConnected(this, null);
            }
        }

        public void Connect()
        {
            this.client.Open();
        }

        public void Disconnect()
        {
            this.client.Close();
        }

        public void Send<T>(string to, T message) where T : XmppXElement
        {
            var iqMessage = new GenericIq<T>(message, to, IqType.get);
            Send(iqMessage);
        }

        public void Send(Iq message)
        {
            this.client.SendAndAck(message, AcknowledgeSend);
        }

        private void AcknowledgeSend(object sender, AckEventArgs ackEventArgs)
        {
            log.Debug(ackEventArgs.State);
        }

        public string ConnectedUser
        {
            get { return connectedJid ?? null; }
        }

        public void Dispose()
        {
            client.OnBind -= ClientOnBind;
            client.OnAuthError -= ClientOnAuthError;
            client.OnRegisterError -= ClientOnRegisterError;
            client.OnBindError -= ClientOnBindError;

            client.Close();
            client.Dispose();
        }
    }
}
