using MvvmCross.Platforms.Wpf.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace TestSystem.WpfUI.Views
{
    public class CustomView : MvxWpfView
    {
        protected void ClickMinimizeWindow(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }
        protected void ClickMaximizeWindow(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.WindowState = (Application.Current.MainWindow.WindowState == WindowState.Normal) ? WindowState.Maximized : WindowState.Normal;
        }
        protected void ClickClose(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Close();
        }

        protected void Card_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
                Application.Current.MainWindow.DragMove();
        }
    }
}
