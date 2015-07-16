using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Management;

namespace Chebao.Browser
{
    public partial class Form1 : Form
    {
        private static string mactxturl = "http://yd.lamda.us/mac.txt";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CheckMac();
        }

        private void CheckMac()
        {
            WebClient wc = new WebClient();
            try
            {
                string macstr = wc.DownloadString(mactxturl);
                string[] macs = macstr.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                string mac = GetLocalMac();
                if (!macs.Contains(mac))
                {
                    MessageBox.Show("请联系软件提供商授权本机");
                    this.Close();
                    Application.Exit();
                }
            }
            catch
            {
                MessageBox.Show("网络异常，请检查网络");
                this.Close();
                Application.Exit();
            }
        }

        public string GetLocalMac()
        {
            string mac = null;
            ManagementObjectSearcher query = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection queryCollection = query.Get();
            foreach (ManagementObject mo in queryCollection)
            {
                if (mo["IPEnabled"].ToString() == "True")
                    mac = mo["MacAddress"].ToString();
            }
            return (mac);
        }
    }
}
