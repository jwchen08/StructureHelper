using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media.Imaging;

namespace StructureHelper
{
    public partial class ConceptPage : PhoneApplicationPage
    {
        public ConceptPage()
        {
            InitializeComponent();
            BuildLocalizedApplicationBar();
        }

        private void BuildLocalizedApplicationBar()
        {
            // 将页面的 ApplicationBar 设置为 ApplicationBar 的新实例。
            ApplicationBar = new ApplicationBar();

            // 创建新按钮并将文本值设置为 AppResources 中的本地化字符串。
            ApplicationBarIconButton appBarButton1 = new ApplicationBarIconButton(new Uri("/Assets/AppBar/Previous.png", UriKind.Relative));
            ApplicationBarIconButton appBarButton2 = new ApplicationBarIconButton(new Uri("/Assets/AppBar/Next.png", UriKind.Relative));
            appBarButton1.Text = "前一张";
            appBarButton2.Text = "后一张";
            appBarButton1.Click += appBarButton1_Click;
            appBarButton2.Click += appBarButton2_Click;
            ApplicationBar.Buttons.Add(appBarButton1);
            ApplicationBar.Buttons.Add(appBarButton2);

        }

        int currentIndex = 1;
        void appBarButton1_Click(object sender, EventArgs e)
        {
            currentIndex++;
            string s = string.Format("/Pictures/Concept{0}.png", currentIndex%2);
            ImageControl.Source = new BitmapImage(new Uri(s, UriKind.Relative));

        }

        void appBarButton2_Click(object sender, EventArgs e)
        {
            currentIndex++;
            string s = string.Format("/Pictures/Concept{0}.png", currentIndex % 2);
            ImageControl.Source = new BitmapImage(new Uri(s, UriKind.Relative));

        }


    }
}