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
using System.Windows.Forms;
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
        public const int WM_HOTKEY = 0x312;

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, ModifierKeys fsModifuers, int vk);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern short GlobalAddAtom(string lpString);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern short GlobalDeleteAtom(string nAtom);

        bool bPause = false;
        private NotifyIcon notifyIcon;

        public MainWindow()
        {
            InitializeComponent();

            browser.Source = new Uri(text.Text);

            this.notifyIcon = new NotifyIcon();
            this.notifyIcon.Text = "ximalaya-player";
            this.notifyIcon.Icon = System.Drawing.Icon.ExtractAssociatedIcon(System.Windows.Forms.Application.ExecutablePath);
            this.notifyIcon.Visible = true;
            //exit
            System.Windows.Forms.MenuItem exit = new System.Windows.Forms.MenuItem("Exit");
            exit.Click += new EventHandler(Close);
            //menu
            System.Windows.Forms.MenuItem[] childen = new System.Windows.Forms.MenuItem[] { exit };
            notifyIcon.ContextMenu = new System.Windows.Forms.ContextMenu(childen);

            this.notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler((o, e) =>
            {
                if (e.Button == MouseButtons.Left) this.Show(o, e);
            });
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            int atom = GlobalAddAtom("ximalaya-player");

            IntPtr handle = new WindowInteropHelper(this).Handle;
            RegisterHotKey(handle, atom, ModifierKeys.Control | ModifierKeys.Alt, 81);

            HwndSource source = PresentationSource.FromVisual(this) as HwndSource;
            source.AddHook(WndProc);
        }

        IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handle)
        {
            if(msg == WM_HOTKEY)
            {
                playOrPause();
            }
            return IntPtr.Zero;
        }

        private void playOrPause()
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            browser.Source = new Uri(text.Text);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            playOrPause();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            GlobalDeleteAtom("ximalaya-player");
        }

        private void Show(object sender, EventArgs e)
        {
            this.Visibility = System.Windows.Visibility.Visible;
            this.ShowInTaskbar = true;
            this.WindowState = WindowState.Normal;
            this.Activate();
        }

        private void Hide(object sender, EventArgs e)
        {
            this.ShowInTaskbar = false;
            this.Visibility = System.Windows.Visibility.Hidden;
        }

        private void Close(object sender, EventArgs e)
        {
            this.notifyIcon.Visible = false;
            System.Windows.Application.Current.Shutdown();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }
    }
}
