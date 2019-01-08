using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace player
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        bool bPause = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            browser.Source = new Uri(text.Text);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (bPause)
            {
                browser.InvokeScript("eval", "document.getElementsByClassName('play')[0].click()");
                btn.Content = "PAUSE";
                bPause = false;
            }
            else
            {
                browser.InvokeScript("eval", "document.getElementsByClassName('pause')[0].click()");
                btn.Content = "PLAY";
                bPause = true;
            }
        }
    }
}
