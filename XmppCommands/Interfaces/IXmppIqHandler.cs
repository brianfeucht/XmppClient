using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Matrix.Xml;

namespace XmppCommands.Interfaces
{
    public interface IXmppIqHandler<T> where T : XmppXElement
    {
        void ProcessMessage(T message);
    }
}
