using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using Coding4Fun.Toolkit.Controls;

namespace StructureHelper
{
    public partial class AboutPage : PhoneApplicationPage
    {
        public AboutPage()
        {
            InitializeComponent();
        }

        private void EmailButton_Click(object sender, EventArgs e)
        {
            EmailComposeTask ect = new EmailComposeTask();
            ect.Subject = "结构小助手v1.5.0版反馈";
            ect.To = "jwchen08@qq.com";
            ect.Show();
        }

        private void CopyButton_Click(object sender, EventArgs e)
        {
            Clipboard.SetText("structurehelper:Home");
            MessageBox.Show("启动协议已经复制到剪贴板，请粘贴到桌面磁贴美化应用中以启动结构小助手！", "提示",MessageBoxButton.OK);
        }
    }
}