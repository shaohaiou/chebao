using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Chebao.Components
{
    public enum ProductType
    {
        [Description("前刹车片")]
        前刹车片,
        [Description("后刹车片")]
        后刹车片,
        [Description("外配报警线(前)")]
        外配报警线_前,
        [Description("外配报警线(后)")]
        外配报警线_后,
        [Description("附件")]
        附件,
    }
}
