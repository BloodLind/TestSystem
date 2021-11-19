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

namespace TestSystem.WpfUI
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MvxWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            System.Threading.Thread.CurrentThread.CurrentUICulture =
              System.Globalization.CultureInfo.GetCultureInfo(Properties.Settings.Default.Language);
        }

        public static void ChangeLanguage(string type)
        {
            Properties.Settings.Default.Language = type;
            Properties.Settings.Default.Save();
            App.Restart();
        }

        private void ClickMinimizeWindow(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }
        private void ClickMaximizeWindow(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.WindowState = (Application.Current.MainWindow.WindowState == WindowState.Normal) ? WindowState.Maximized : WindowState.Normal;
        }
        private void ClickClose(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Close();
        }

        private void Card_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
                Application.Current.MainWindow.DragMove();
        }
    }
}
