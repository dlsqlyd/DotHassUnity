using DotNetty.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotHass.Unity.Net.Abstractions
{
    public interface IResponseMessage
    {
        int ContractID { get; }
        byte[] BodyContent { get; set; }
        bool Parse(IByteBuffer byteBuffer);
    }
}
