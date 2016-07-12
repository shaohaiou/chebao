using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Threading;

namespace Chebao.Tools
{
    public static class AsyncHelp
    {
        public delegate void DelayRunFunc(object args);

        /// <summary>
        /// 延时执行
        /// </summary>
        /// <param name="delay"></param>
        /// <param name="?"></param>
        public static void DelayRun(int delay, DelayRunFunc func,object args)
        {
            BackgroundWorker b = new BackgroundWorker();
            b.RunWorkerCompleted += delegate(object sender, RunWorkerCompletedEventArgs e)
            {
                func(args);
            };
            b.DoWork += delegate(object sender, DoWorkEventArgs e)
            {
                Thread.Sleep(delay);
            };
            b.RunWorkerAsync();
        }
    }
}
