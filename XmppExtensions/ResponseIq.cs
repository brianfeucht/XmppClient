using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Matrix.Xml;
using Matrix.Xmpp;
using Iq = Matrix.Xmpp.Client.Iq;

namespace XmppExtensions
{
    public class ResponseIq<T> : Iq where T : XmppXElement
    {
        public ResponseIq(T response, IIncomingIqMessage originalMessage)
        {
            Item = response;
            To = originalMessage.From;
            Type = IqType.result;
            Id = originalMessage.Id;
        }

        public T Item
        {
            get { return Element<T>(); }
            set { Replace(value);}
        }
    }
}
