﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chebao.Components;

namespace Chebao.TaskAndJob.Job
{
    public class OrderPastDueJob : IJob
    {
        private static bool isRunning = false;

        public void Execute(System.Xml.XmlNode node)
        {
            try
            {
                if (!isRunning)
                {
                    isRunning = true;
                    Cars.Instance.OrderPastDue();
                    isRunning = false;
                }
            }
            catch { }
        }
    }
}