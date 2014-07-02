using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmppExtensions
{
    public interface IIncomingIqMessage
    {
        string Id { get; }
        string From { get; }
        object Query { get; }
    }
}
