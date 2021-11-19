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

namespace TestSystem.WpfUI.Views
{
    /// <summary>
    /// Логика взаимодействия для TestDescriptionView.xaml
    /// </summary>
    public partial class TestDescriptionView : CustomView
    {
        public TestDescriptionView()
        {
            InitializeComponent();
        }

        private void Button_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ((Button)sender).Visibility = ((Button)sender).Visibility == Visibility.Visible ? Visibility.Hidden : Visibility.Visible;
        }
    }
}
