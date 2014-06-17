using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Matrix.Xml;

namespace XmppCommands
{
    public class PingPong : XmppXElement
    {
        public DateTime TimeStarted
        {
            get { return DateTime.Parse(GetTag("startedOn")); }
            set { SetTag("startedOn", value.ToString()); }
        }

        public DateTime LastPing
        {
            get { return DateTime.Parse(GetTag("lastPing")); }
            set { SetTag("lastPing", value.ToString()); }
        }

        public string RespondTo
        {
            get { return GetTag("respondTo"); }
            set { SetTag("respondTo", value); } 
        }

        public PingPong() : base("litmus:pingpong", "pingpong")
        {

        }

        public static PingPong StartNew(string respondTo)
        {
            return new PingPong()
            {
                TimeStarted = DateTime.Now,
                LastPing = DateTime.Now,
                RespondTo = respondTo
            };
        }
    }
}
