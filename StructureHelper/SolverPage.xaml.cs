using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace StructureHelper
{
    public partial class SolverPage : PhoneApplicationPage
    {
        public SolverPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/ConceptPage.xaml", UriKind.RelativeOrAbsolute));
        }
    }
}