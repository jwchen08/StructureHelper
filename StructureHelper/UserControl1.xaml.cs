using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace JasonPopupDemo
{
    public partial class UserControl1 : UserControl
    {
        public static double Fn = 150;
        public static double Mx = 300;
        public static double Vx = 200;

        public UserControl1()
        {
            InitializeComponent();
            TextBox1.Text = Fn.ToString();
            TextBox2.Text = Mx.ToString();
            TextBox3.Text = Vx.ToString();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.CloseMeAsPopup();
            bool b1=false;
            bool b2=false;
            bool b3=false;
            b1 = double.TryParse(TextBox1.Text, out Fn);
            b2 = double.TryParse(TextBox2.Text, out Mx);
            b3 = double.TryParse(TextBox3.Text, out Vx);
            if(b1==false||b2==false||b3==false)
            {
                MessageBox.Show("数据输入有误！");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.CloseMeAsPopup();
        }
    }
}
