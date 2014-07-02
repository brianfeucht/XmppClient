using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Matrix.Xmpp.Base;

namespace XmppExtensions
{
    public class MatrixIqMessage : IIncomingIqMessage
    {
        private readonly Iq message;

        public MatrixIqMessage(Iq message)
        {
            this.message = message;
        }

        public string From
        {
            get { return message.From; }
        }

        public object Query
        {
            get { return message.Query; }
        }
    }
}
