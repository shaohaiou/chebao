using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Chebao.Components
{
    public interface IJob
    {
        void Execute(XmlNode node);
    }
}
