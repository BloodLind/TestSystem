using MvvmCross.Platforms.Wpf.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TestSystem.Core.ViewModels;
using TestSystem.WpfUI.Converters;

namespace TestSystem.WpfUI.Views
{
    /// <summary>
    /// Логика взаимодействия для AccountView.xaml
    /// </summary>
    public partial class AccountView : CustomView
    {
        public AccountView()
        {
            InitializeComponent();
        }
        
        private void profileImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void CustomView_Loaded(object sender, RoutedEventArgs e)
        {
           
        }
    }
}
