using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Matrix;
using Matrix.Xmpp;
using Matrix.Xmpp.Chatstates;
using Matrix.Xmpp.Client;

namespace XmppCommands
{
    public class AppXmppClient : IDisposable
    {
        private ILog log = LogManager.GetLogger(typeof(AppXmppClient));
        private ILog xmppClientLog = LogManager.GetLogger(typeof (XmppClient));

        internal XmppClient Client;

        public Jid ConnectedUser { get; private set; }
        public event EventHandler<XmppMessage> OnMessageReceived;
        public event EventHandler<JidEventArgs> OnConnected;

        public AppXmppClient(string user)
        {
            Client = new XmppClient(user, "brianfeucht693b", "123");
            Client.OnMessage += ClientOnMessage;
            Client.OnBindError += ClientOnBindError;
            Client.OnBind += ClientOnOnBind;
            
            Client.OnReceiveXml += (sender, args) => xmppClientLog.Debug(args.Text);
            Client.OnSendXml += (sender, args) => xmppClientLog.Debug(args.Text);

            LicenseManager.SetLicense();
        }

        private void ClientOnOnBind(object sender, JidEventArgs jidEventArgs)
        {
            ConnectedUser = jidEventArgs.Jid;

            log.InfoFormat("Logged in as {0}", jidEventArgs.Jid);

            if (OnConnected != null)
            {
                OnConnected(this, jidEventArgs);
            }
        }

        public AppXmppClient(string user, Action<XmppMessage> onMessageRecieved) : this(user)
        {
            OnMessageReceived += (sender, message) => onMessageRecieved(message);
        }

        private void ClientOnMessage(object sender, MessageEventArgs messageEventArgs)
        {
            if (OnMessageReceived != null && !string.IsNullOrEmpty(messageEventArgs.Message.Body))
            {
                var xmppMessage = new XmppMessage()
                {
                    From = messageEventArgs.Message.From.User,
                    Body = messageEventArgs.Message.Body
                };

                OnMessageReceived(this, xmppMessage);
            }
        }

        private void ClientOnBindError(object sender, IqEventArgs e)
        {
            log.ErrorFormat("Unable to connect {0}", e.Iq);
        }

        public void Start()
        {
            Client.Open();
            Client.Show = Show.chat;
        }

        public void Stop()
        {
            Client.Close();
        }

        public void Dispose()
        {
            Client.Close();
            Client.Dispose();
        }
    }
}
