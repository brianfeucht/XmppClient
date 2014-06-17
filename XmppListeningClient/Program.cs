using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;
using XmppCommands;

namespace XmppListeningClient
{
    class Program
    {
        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();
            //HostFactory.Run(hc => hc.Service<AppXmppClient>(sc =>
            //{
            //    sc.ConstructUsing(() => new AppXmppClient("test", (m) => Console.WriteLine("{0}: {1}", m.From, m.Body)));
            //    sc.WhenStarted(s => s.Start());
            //    sc.WhenStopped(s => s.Stop());
            //}));

            HostFactory.Run(hc => hc.Service<XmppPingPongClient>(sc =>
            {
                sc.ConstructUsing(() => new XmppPingPongClient());
                sc.WhenStarted(s => s.Start());
                sc.WhenStopped(s => s.Stop());
            }));
        }
    }
}
