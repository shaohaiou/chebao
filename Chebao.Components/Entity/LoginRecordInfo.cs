using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chebao.Components
{
    public class LoginRecordInfo
    {
        public int ID { get; set; }

        public int AdminID { get; set; }

        public string AdminName { get; set; }

        public DateTime LoginTime { get; set; }

        public string LoginIP { get; set; }

        public string LoginPosition { get; set; }
    }
}
