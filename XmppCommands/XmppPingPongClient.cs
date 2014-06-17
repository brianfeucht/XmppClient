using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using Matrix;
using Matrix.Xml;
using Matrix.Xmpp;
using Matrix.Xmpp.Client;

namespace XmppCommands
{
    public class XmppPingPongClient : IDisposable
    {
        private ILog log = LogManager.GetLogger(typeof(XmppPingPongClient));

        Random random = new Random();

        private AppXmppClient client1;
        private AppXmppClient client2;


        public XmppPingPongClient()
        {
            Factory.RegisterElement<PingPong>("litmus:pingpong", "pingpong");

            client1 = new AppXmppClient("test");
            client2 = new AppXmppClient("test2");

            client1.Client.OnIq += ClientOnOnIq;
            client2.Client.OnIq += ClientOnOnIq;

            client1.OnConnected += ClientOnOnConnected;

        }

        private void ClientOnOnConnected(object sender, JidEventArgs jidEventArgs)
        {
            Task.Run(() =>
            {
                while (true)
                {
                    Thread.Sleep(5000);
                    if (client2.ConnectedUser != null)
                    {
                        var iq = new GenericIq<PingPong>(
                            PingPong.StartNew(jidEventArgs.Jid),
                            client2.ConnectedUser,
                            IqType.get);

                        client1.Client.Send(iq);

                        return;
                    }
                }
            });
        }

        private void ClientOnOnIq(object sender, IqEventArgs e)
        {
            if (e.Iq.Type == IqType.get && e.Iq.Query is PingPong)
            {
                var pingPong = (PingPong) e.Iq.Query;

                log.InfoFormat("{0}: Ping {1}", e.Iq.From.User, pingPong.LastPing);

                Thread.Sleep(random.Next(1000, 10000));

                pingPong.LastPing = DateTime.Now;

                var targetClient = e.Iq.From.User == "test" ? client2 : client1;
                pingPong.RespondTo = e.Iq.From.User == "test" ? client1.ConnectedUser : client2.ConnectedUser;


                targetClient.Client.Send(new GenericIq<PingPong>(pingPong, pingPong.RespondTo, IqType.get));
            }
        }

        public void Start()
        {
            client1.Start();
            client2.Start();
        }

        public void Stop()
        {
            client1.Stop();
            client2.Stop();
        }

        public void Dispose()
        {
            client1.Dispose();
            client2.Dispose();
        }
    }
}
