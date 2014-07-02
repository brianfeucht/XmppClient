using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Matrix;
using Matrix.Xml;
using Matrix.Xmpp;
using Iq = Matrix.Xmpp.Client.Iq;

namespace XmppExtensions
{
    public class GenericIq<T> : Iq where T : XmppXElement
    {
        public GenericIq()
        {
            GenerateId();
        }

        public GenericIq(T item, Jid to, IqType type)
        {
            Item = item;
            To = to;
            Type = type;
        }

        public T Item
        {
            get { return Element<T>(); }
            set { Replace(value);}
        }
    }
}
