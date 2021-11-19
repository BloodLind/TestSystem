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

namespace TestSystem.WpfUI.Views
{
    /// <summary>
    /// Логика взаимодействия для CreateTestView.xaml
    /// </summary>
    public partial class CreateTestView : CustomView
    {
        public CreateTestView()
        {
            InitializeComponent();
        }

        private void Button_Click1(object sender, RoutedEventArgs e)
        {
            var curItem = ((ListViewItem)listview.ContainerFromElement((Button)sender)).Content;
            listview.SelectedItem = curItem;
            ((CreateTestViewModel)DataContext).RemoveAnswerCommand.Execute(this);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var curItem = ((ListViewItem)listview.ContainerFromElement((Button)sender)).Content;
            if (((Button)sender).Foreground != Brushes.Yellow)
            {
                listview.SelectedItem = curItem;
                ((Button)sender).Foreground = Brushes.Yellow;
                ((CreateTestViewModel)DataContext).SelectAsCorrectCommand.Execute(this);
            }
            else if (((CreateTestViewModel)DataContext).SelectedAnswer != curItem)
            {
                ((Button)sender).Foreground = Brushes.Black;
            }
            
        }
    }
}
