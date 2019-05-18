using MahApps.Metro.Controls;
using Open.WinKeyboardHook;
using System;
using System.Windows;
using System.Windows.Input;
using System.IO;
using System.Windows.Media.Imaging;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Windows.Interop;

namespace GMusic
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private readonly IKeyboardInterceptor _interceptor;

        private SongInfo songInfo = new SongInfo();
        private string _prevArt = "";
        private Bitmap _coverArt;

        System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();

        private string MediaKey;

        private string Style;

        
        public MainWindow()
        {
            InitializeComponent();
            Loaded += WindowLoaded;

            _interceptor = new KeyboardInterceptor();
            //_interceptor.KeyPress += (sender, args) => Title += args.KeyChar;
            _interceptor.KeyDown += intercepta;
            _interceptor.StartCapturing();

            timer.Tick += new EventHandler(ftimer);
            timer.Interval = new TimeSpan(0,0,0,0,250);
            timer.Start();

        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            HwndSource source = PresentationSource.FromVisual(this) as HwndSource;
            source.AddHook(WndProc);
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            Bitmap bmp = (Bitmap)Bitmap.FromFile("C:\\Users\\vhanla\\Pictures\\Brushes\\WHITE\\WS02.PNG");
            //Bitmap bmp = new Bitmap(_bmp.Width, _bmp.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
            //using (Graphics gr = Graphics.FromImage(_bmp))
            //{
            //    gr.DrawImage(_bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));
            //}
                IntPtr hBitmap = bmp.GetHbitmap();
            IntPtr hwnd = new WindowInteropHelper(Application.Current.MainWindow).Handle;//Process.GetCurrentProcess().MainWindowHandle;
            int attr = (int)NativeMethods.TRUE;
            int hResult = NativeMethods.DwmSetWindowAttribute(hwnd, NativeMethods.DWMWA.FORCE_ICONIC_REPRESENTATION, ref attr, sizeof(int));
            if (hResult != 0)
                throw Marshal.GetExceptionForHR(hResult);
            hResult = NativeMethods.DwmSetWindowAttribute(hwnd, NativeMethods.DWMWA.HAS_ICONIC_BITMAP, ref attr, sizeof(int));
            if (hResult != 0)
                throw Marshal.GetExceptionForHR(hResult);
            //hResult = NativeMethods.DwmSetIconicThumbnail(hwnd, hBitmap, NativeMethods.DWM_SIT.DISPLAYFRAME);
            hResult = NativeMethods.DwmSetIconicThumbnail(hwnd, hBitmap, 0);
            if (hResult != 0)
                throw Marshal.GetExceptionForHR(hResult);

        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == NativeMethods.WM_DWMSENDICONICTHUMBNAIL)
            {
                //Bitmap bmp = new Bitmap(_bmp.Width, _bmp.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
                //using (Graphics gr = Graphics.FromImage(_bmp))
                //{
                //    gr.DrawImage(_bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));
                //}

                IntPtr hBitmap = _coverArt.GetHbitmap();
                //IntPtr hwnd = new WindowInteropHelper(Application.Current.MainWindow).Handle;//Process.GetCurrentProcess().MainWindowHandle;
                int hResult = NativeMethods.DwmSetIconicThumbnail(hwnd, hBitmap, 0);
                if (hResult != 0)
                    throw Marshal.GetExceptionForHR(hResult); 


            }
            else if (msg == NativeMethods.WM_DWMSENDICONICLIVEPREVIEWBITMAP)
            {
                //Bitmap bmp = (Bitmap)Bitmap.FromStream(_cover.StreamSource);
                //Bitmap bmp = new Bitmap(_bmp.Width, _bmp.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
                //using (Graphics gr = Graphics.FromImage(_bmp))
                //{
                //    gr.DrawImage(_bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));
                //}

                IntPtr hBitmap = _coverArt.GetHbitmap();
                //IntPtr hwnd = new WindowInteropHelper(Application.Current.MainWindow).Handle;//Process.GetCurrentProcess().MainWindowHandle;
                int hResult = NativeMethods.DwmSetIconicLivePreviewBitmap(hwnd, hBitmap,(IntPtr)null ,0);
                if (hResult != 0)
                    throw Marshal.GetExceptionForHR(hResult); 



            }
            return IntPtr.Zero;
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

            // Update song info
            UpdateSongInfo();
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

        private async System.Threading.Tasks.Task GetAlbumArt()
        {
            var result = await webView1.InvokeScriptAsync("eval", new[] { "document.getElementById('playerSongInfo').firstChild.firstChild.src" });
            TaskThumb.Overlay = new BitmapImage(new Uri(result, UriKind.Absolute));
        }

        private async System.Threading.Tasks.Task UpdateSongInfo()
        {
            var artist = await webView1.InvokeScriptAsync("eval", new[] { "document.getElementById('player-artist')?document.getElementById('player-artist').innerText:''" });
            songInfo.Artist = artist;

            var title = await webView1.InvokeScriptAsync("eval", new[] { "document.getElementById('currently-playing-title')?document.getElementById('currently-playing-title').innerText:''" });
            songInfo.Title = title;

            var album = await webView1.InvokeScriptAsync("eval", new[] { "document.querySelectorAll('.player-album').length>0?document.querySelectorAll('.player-album')[0].innerText:''" });
            songInfo.Album = album;

            this.Title = (title == "") ? "Google Music Desktop Player": songInfo.Artist + " - " + songInfo.Title;

            var coverArt = await webView1.InvokeScriptAsync("eval", new[] { "document.getElementById('playerSongInfo').firstChild.firstChild.src" });
            songInfo.CoverArt = coverArt;
            if (_prevArt != songInfo.CoverArt)
            {
                _prevArt = songInfo.CoverArt;

                IntPtr hwnd = new WindowInteropHelper(Application.Current.MainWindow).Handle;//Process.GetCurrentProcess().MainWindowHandle;

                //BitmapImage _cover = new BitmapImage(new Uri(songInfo.CoverArt, UriKind.Absolute));
                BitmapImage _cover = new BitmapImage();
                _cover.BeginInit();
                _cover.CacheOption = BitmapCacheOption.OnLoad;
                _cover.UriSource = new Uri(songInfo.CoverArt, UriKind.Absolute);
                _cover.EndInit();
                _cover.DownloadCompleted += (s, e) =>
                {
                    _coverArt = BitmapImage2Bitmap(_cover);
                    NativeMethods.DwmInvalidateIconicBitmaps(hwnd);
                };

                //_coverArt = (Bitmap)Bitmap.FromStream(_cover.StreamSource);
                TaskThumb.Overlay = _cover;
                

            }
        }

        private Bitmap BitmapImage2Bitmap(BitmapImage bitmapImage)
        {
            using(MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                enc.Save(outStream);
                Bitmap bitmap = new Bitmap(outStream);
                return new Bitmap(bitmap);
            }
        }

        private void Play()
        {
            webView1.InvokeScriptAsync("eval", new[]
            {
               "if(document.querySelectorAll('[data-id=\"play-pause\"]')[0].title =='Play'){document.querySelectorAll('[data-id=\"play-pause\"]')[0].click();}"
           });
            //GetAlbumArt();
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

        private void PlayPause()
        {
            webView1.InvokeScriptAsync("eval", new[]
            {
                "document.querySelectorAll('[data-id=\"play-pause\"]')[0].click();"
            });
        }

        private void Next()
        {
            webView1.InvokeScriptAsync("eval", new[]
            {
                "if(!document.querySelectorAll('[data-id=\"forward\"]')[0].disabled){document.querySelectorAll('[data-id=\"forward\"]')[0].click();}"
            });
        }

        /**
         * Taskbar Buttons
         */
        private void TBPlayPause(object sender, EventArgs e)
        {
            PlayPause();
        }

        private void TBNext(object sender, EventArgs e)
        {
            Next();
        }

        private void TBPrev(object sender, EventArgs e)
        {
            Previous();
        }

    }
}
