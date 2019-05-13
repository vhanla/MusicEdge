using MahApps.Metro.Controls;
using Open.WinKeyboardHook;
using System;
using System.Windows;
using System.Windows.Input;
using System.IO;

namespace GMusic
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private readonly IKeyboardInterceptor _interceptor;

        System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();

        private string MediaKey;

        private string Style;
        
        public MainWindow()
        {
            InitializeComponent();

            _interceptor = new KeyboardInterceptor();
            //_interceptor.KeyPress += (sender, args) => Title += args.KeyChar;
            _interceptor.KeyDown += intercepta;
            _interceptor.StartCapturing();

            timer.Tick += new EventHandler(ftimer);
            timer.Interval = new TimeSpan(0,0,0,0,250);
            timer.Start();
        }

        private void ftimer(object sender, EventArgs e)
        {
            switch (MediaKey)
            {
                case "Play":
                    MediaKey = "";
                    Play();
                    break;
                case "Stop":
                    MediaKey = "";
                    Pause();
                    break;
                case "Previous":
                    MediaKey = "";
                    Previous();
                    break;
                case "Next":
                    MediaKey = "";
                    Next();
                    break;
            }
        }

        private void intercepta(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            var c = e.KeyCode;
            switch (c)
            {
                case System.Windows.Forms.Keys.MediaPlayPause:
                    MediaKey = "Play";
                    break;
                case System.Windows.Forms.Keys.MediaNextTrack:
                    MediaKey = "Next";
                    break;
                case System.Windows.Forms.Keys.MediaPreviousTrack:
                    MediaKey = "Previous";
                    break;
                case System.Windows.Forms.Keys.MediaStop:
                    MediaKey = "Stop";
                    break;                    
            }
        }

        private void webView1_ScriptNotify(object sender, Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT.WebViewControlScriptNotifyEventArgs e)
        {
            MessageBox.Show(e.Value, e.Uri?.ToString() ?? string.Empty);
        }

        private void webView1_NavigationCompleted(object sender, Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT.WebViewControlNavigationCompletedEventArgs e)
        {
            //Title = webView1.DocumentTitle;
            if (!e.IsSuccess)
            {
                MessageBox.Show($"Could not navigate to {e.Uri?.ToString() ?? "NULL"}", $"Error: {e.WebErrorStatus}", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void webView1_NavigationStarting(object sender, Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT.WebViewControlNavigationStartingEventArgs e)
        {
            //Title = $"Navigating {e.Uri?.ToString() ?? string.Empty}";
        }

        private void webView1_PermissionRequested(object sender, Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT.WebViewControlPermissionRequestedEventArgs e)
        {
            if (e.PermissionRequest.State == Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT.WebViewControlPermissionState.Allow) return;

            var msg = $"Allow {e.PermissionRequest.Uri.Host} to access {e.PermissionRequest.PermissionType}?";

            //var response = MessageBox.Show(msg, "Permission Request", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes);

            //if (response == MessageBoxResult.Yes)
            {
                if (e.PermissionRequest.State == Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT.WebViewControlPermissionState.Defer)
                {
                    webView1.GetDeferredPermissionRequestById(e.PermissionRequest.Id)?.Allow();
                }
                else
                {
                    e.PermissionRequest.Allow();
                }
            }
            /*else
            {
                if (e.PermissionRequest.State == Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT.WebViewControlPermissionState.Defer)
                {
                    webView1.GetDeferredPermissionRequestById(e.PermissionRequest.Id)?.Deny();
                }
                else
                {
                    e.PermissionRequest.Deny();
                }
            }*/
        }
        
        private void MetroWindow_Initialized(object sender, EventArgs e)
        {
            webView1.Source = new Uri("https://play.google.com/music");
            string exepath = AppDomain.CurrentDomain.BaseDirectory;
            if (File.Exists(exepath + "style.css"))
            {
                Style = File.ReadAllText(exepath + "style.css");
            }
        }

        private void webView1_DOMContentLoaded(object sender, Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT.WebViewControlDOMContentLoadedEventArgs e)
        {
            string script = "(function(){"+
                "var style=document.getElementById('gmusic_custom_css');"+
                "if(!style){ style = document.createElement('STYLE');" +
                "style.type='text/css';" +
                "style.id='gmusic_custom_css'; " +    
                "style.innerText = \"" + Style + "\";" + 
                "document.getElementsByTagName('HEAD')[0].appendChild(style);" +
                "} } )()";
            webView1.InvokeScriptAsync("eval", new string[] { script });
        }

        private async System.Threading.Tasks.Task ClickAsync()
        {
            var result = await webView1.InvokeScriptAsync("eval", new[] { "document.querySelectorAll('[data-id=\"play-pause\"]')[0].title" });
            MessageBox.Show(result.ToString());
        }

        private void Play()
        {
            webView1.InvokeScriptAsync("eval", new[]
            {
               "if(document.querySelectorAll('[data-id=\"play-pause\"]')[0].title =='Play'){document.querySelectorAll('[data-id=\"play-pause\"]')[0].click();}"
           });
        }

        private void Pause()
        {
            webView1.InvokeScriptAsync("eval", new[]
            {
               "if(document.querySelectorAll('[data-id=\"play-pause\"]')[0].title =='Pause'){document.querySelectorAll('[data-id=\"play-pause\"]')[0].click();}"
           });
        }

        private void Previous()
        {
            webView1.InvokeScriptAsync("eval", new[]
            {
                "if(!document.querySelectorAll('[data-id=\"rewind\"]')[0].disabled){document.querySelectorAll('[data-id=\"rewind\"]')[0].click();}"
            });
        }

        private void Next()
        {
            webView1.InvokeScriptAsync("eval", new[]
            {
                "if(!document.querySelectorAll('[data-id=\"forward\"]')[0].disabled){document.querySelectorAll('[data-id=\"forward\"]')[0].click();}"
            });
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
