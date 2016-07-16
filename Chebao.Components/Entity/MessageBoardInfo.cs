using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chebao.Components
{
    public class MessageBoardInfo
    {
        public int ID { get; set; }

        public int UserID { get; set; }

        public string UserName { get; set; }

        public string LinkName { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string AddTime { get; set; }

        /// <summary>
        /// 回复状态
        /// <para>0：未回复</para>
        /// <para>1：已回复</para>
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 回复内容
        /// </summary>
        public string Reply { get; set; }

        /// <summary>
        /// 回复时间
        /// </summary>
        public string ReplyTime { get; set; }

        /// <summary>
        /// 回复人ID
        /// </summary>
        public int ReplyAdminID { get; set; }

        /// <summary>
        /// 回复人用户名
        /// </summary>
        public string ReplyAdmin { get; set; }

        /// <summary>
        /// 回复人姓名
        /// </summary>
        public string ReplyAdminName { get; set; }
    }
}
