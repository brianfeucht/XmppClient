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
using XmppExtensions;

namespace XmppCommands
{
    public class XmppPingPongClient : IDisposable
    {
        private ILog log = LogManager.GetLogger(typeof(XmppPingPongClient));

        Random random = new Random();

        private readonly MatrixXmppClient client1;
        private readonly MatrixXmppClient client2;
        private readonly ServiceDispatcher dispatcher1;
        private readonly ServiceDispatcher dispatcher2;


        public XmppPingPongClient()
        {
            LicenseManager.SetLicense();

            client1 = new MatrixXmppClient(new XmppClient("test", "brianfeucht693b", "123"));
            client2 = new MatrixXmppClient(new XmppClient("test2", "brianfeucht693b", "123"));

            client1.OnConnected += ClientOnOnConnected;

            dispatcher1 = new ServiceDispatcher(client1, new IMessageProcessor[] {new PingPongMessageProcessor(random)});
            dispatcher2 = new ServiceDispatcher(client2, new IMessageProcessor[] { new PingPongMessageProcessor(random), new PingMessageProcessor() });
        }

        private void ClientOnOnConnected(object sender, System.EventArgs e)
        {
            Task.Run(async () => 
            {
                while (true)
                {
                    Thread.Sleep(5000);
                    if (client2.ConnectedUser != null)
                    {
                        var pingPong = PingPong.StartNew();
                        client1.Send(client2.ConnectedUser, pingPong);

                        var ping = Ping.StartNew();
                        var response = await client1.SendWithResponse<Ping, Ping>(client2.ConnectedUser, ping);
                                
                        log.InfoFormat("Ping sent {0} and response recieved {1}", response.TimeStarted, response.RespondedOn);
                    }

                    return;
                }
            });
        }

        public void Start()
        {
            client1.Connect();
            client2.Connect();
        }

        public void Stop()
        {
            client1.Disconnect();
            client2.Disconnect();
        }

        public void Dispose()
        {
            dispatcher1.Dispose();
            dispatcher2.Dispose();
        }
    }
}
