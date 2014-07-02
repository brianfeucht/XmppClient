using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Matrix.Xml;

namespace XmppCommands
{
    public class Ping : XmppXElement
    {
        public Ping() : base("XmppCommands", "Ping")
        {
        }

        public DateTime TimeStarted
        {
            get { return DateTime.Parse(GetTag("startedOn")); }
            set { SetTag("startedOn", value.ToString()); }
        }

        public DateTime? RespondedOn
        {
            get
            {
                DateTime value;
                if (DateTime.TryParse(GetTag("respondedOn"), out value))
                {
                    return value;
                }

                return null;
            }
            set { SetTag("respondedOn", value.ToString()); }
        }

        public static Ping StartNew()
        {
            return new Ping()
            {
                TimeStarted = DateTime.Now
            };
        }
    }
}
