using MaterialDesignThemes.Wpf;
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

namespace TestSystem.WpfUI.Views
{
    /// <summary>
    /// Логика взаимодействия для CatalogView.xaml
    /// </summary>
    public partial class CatalogView : CustomView
    {
        public CatalogView()
        {
            InitializeComponent();
        }

        private void Chip_DeleteClick(object sender, RoutedEventArgs e)
        {
            var curItem = ((ListViewItem)listViewFilters.ContainerFromElement((Chip)sender)).Content;
            listViewFilters.SelectedItem = curItem;
            ((CatalogViewModel)DataContext).RemoveFilterCommand.Execute(this);
        }

        private void Button_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (((Button)sender).Visibility == Visibility.Hidden)
                ((Button)sender).Visibility = Visibility.Visible;
            else
                ((Button)sender).Visibility = Visibility.Hidden;
        }

        private void PopupBox_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (((PopupBox)sender).Visibility == Visibility.Hidden)
                ((PopupBox)sender).Visibility = Visibility.Visible;
            else
                ((PopupBox)sender).Visibility = Visibility.Hidden;
        }
    }
}
