namespace Dynamic.Tureng.Translator
{
    #region using

    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Net.Cache;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Documents;
    using System.Windows.Interop;
    using System.Windows.Threading;
    using System.Xml;
    using Dynamic.Translator.Core.Config;
    using Dynamic.Translator.Core.Dependency;
    using Dynamic.Translator.Core.ViewModel;
    using HtmlAgilityPack;
    using Utility;

    #endregion

    public partial class MainWindow
    {
        private readonly IStartupConfiguration _configurations;
        private readonly GrowlNotifiactions _growNotifications;
        private readonly Dictionary<string, string> _languageMap;
        private string currentString;
        private IntPtr hWndNextViewer;
        private HwndSource hWndSource;
        private bool isViewing;
        private string previousString;

        public MainWindow()
        {
            InitializeComponent();

            _configurations = IocManager.Instance.Resolve<IStartupConfiguration>();
            _growNotifications = IocManager.Instance.Resolve<GrowlNotifiactions>();

            _languageMap = _configurations.LanguageMap;
            _growNotifications.Top = SystemParameters.WorkArea.Top + _configurations.TopOffset;
            _growNotifications.Left = SystemParameters.WorkArea.Left + SystemParameters.WorkArea.Width - _configurations.LeftOffset;
            _growNotifications.OnDispose += ClearAllNotifications;
            Application.Current.DispatcherUnhandledException += HandleUnhandledException;
        }

        private static void ClearAllNotifications(object sender, EventArgs args)
        {
            var growl = sender as GrowlNotifiactions;
            if (growl == null) return;
            if (growl.IsDisposed) return;

            growl.Notifications.Clear();
            GC.SuppressFinalize(growl);
            growl.IsDisposed = true;
        }

        private void HandleUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            //e.Dispatcher.Invoke(() => Application.Current.Run());
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Close();
            GC.Collect();
            GC.SuppressFinalize(this);
            Application.Current.Shutdown();
        }

        private async void btnSwitch_Click(object sender, RoutedEventArgs e)
        {
            if (!isViewing)
            {
                await _growNotifications.AddNotificationAsync(
                    new Notification
                    {
                        Title = Titles.StartingMessage,
                        ImageUrl = ImageUrls.NotificationUrl,
                        Message = "The translator will listen your clipboard and it'll translate everything !"
                    });
                InitCbViewer();
                BtnSwitch.Content = "Stop Translator";
            }
            else
            {
                CloseCbViewer();
                BtnSwitch.Content = "Start Translator";
            }
        }

        private async void ButtonClick1(object sender, RoutedEventArgs e)
        {
            await _growNotifications.AddNotificationAsync(
                new Notification
                {
                    Title = Titles.Message,
                    ImageUrl = ImageUrls.NotificationUrl,
                    Message = "It's a toast notification !!!"
                });
        }

        private async void DrawContent()
        {
            RichCurrentText.Document.Blocks.Clear();
            try
            {
                if (!Clipboard.ContainsText()) return;

                RichCurrentText.Document.Blocks.Add(new Paragraph(new Run(Clipboard.GetText())));
                currentString = Clipboard.GetText();
                if (previousString != currentString /*&& Regex.IsMatch(currentString, @"^[a-zA-Z0-9]+$")*/)
                {
                    previousString = currentString;
                    if (currentString.Length > _configurations.SearchableCharacterLimit)
                    {
                        await _growNotifications.AddNotificationAsync(new Notification
                        {
                            Title = Titles.MaximumLimit,
                            ImageUrl = ImageUrls.NotificationUrl,
                            Message = "You have exceed maximum character limit"
                        });
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(_configurations.ApiKey))
                        {
                            await GetMeanFromTureng();
                        }
                        else
                        {
                            await _growNotifications.AddNotificationAsync(
                                new Notification
                                {
                                    Title = Titles.Warning,
                                    ImageUrl = ImageUrls.NotificationUrl,
                                    Message = "The Api Key cannot be NULL !"
                                });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //ingore
            }
        }

        private void WindowLoaded1(object sender, RoutedEventArgs e)
        {
            // this will make minimize restore of notifications too
            //_growNotifications.Owner = GetWindow(this);
        }

        private async Task GetMeanFromTureng()
        {
            var address1 = "http://tureng.com/search/";
            var turenClient = new WebClient
            {
                Headers =
                {
                    {HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/43.0.2357.81 Safari/537.36"}
                },
                Encoding = Encoding.UTF8
            };

            turenClient.Headers.Add(HttpRequestHeader.AcceptLanguage, "en-US,en;q=0.8,tr;q=0.6");
            turenClient.CachePolicy = new HttpRequestCachePolicy(HttpCacheAgeControl.MaxAge, TimeSpan.FromHours(1));
            turenClient.DownloadStringAsync(new Uri(address1 + currentString));
            turenClient.DownloadStringCompleted += wcTureng_DownloadStringCompleted;
        }

        private async Task GetMeanFromYandex()
        {
            var address2 = new Uri(
                string.Format(
                    "https://translate.yandex.net/api/v1.5/tr/translate?" +
                    GetPostData(_languageMap[_configurations.FromLanguage], _languageMap[_configurations.ToLanguage], currentString)));

            var yandexClient = new WebClient {Encoding = Encoding.UTF8};
            yandexClient.DownloadStringAsync(address2);
            yandexClient.CachePolicy = new HttpRequestCachePolicy(HttpCacheAgeControl.MaxAge, TimeSpan.FromHours(1));
            yandexClient.DownloadStringCompleted += wcYandex_DownloadStringCompleted;
        }

        private async void wcYandex_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                if (e.Result == null) return;

                var doc = new XmlDocument();
                doc.LoadXml(e.Result);
                var node = doc.SelectSingleNode("//Translation/text");
                var output = node?.InnerText ?? e.Error.Message;

                await _growNotifications.AddNotificationAsync(
                    new Notification
                    {
                        Title = currentString,
                        ImageUrl = ImageUrls.NotificationUrl,
                        Message = output.ToLower()
                    });
            }
            catch (Exception ex)
            {
                //ingore
            }
        }

        private async void wcTureng_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                if (e.Result == null) return;

                var result = e.Result;
                var output = new StringBuilder();
                var doc = new HtmlDocument();
                var decoded = WebUtility.HtmlDecode(result);
                doc.LoadHtml(decoded);
                if (!result.Contains("table") || doc.DocumentNode.SelectSingleNode("//table") == null)
                {
                    await GetMeanFromYandex();
                }
                else
                {
                    foreach (var table in doc.DocumentNode.SelectNodes("//table"))
                    {
                        foreach (var row in table.SelectNodes("tr").AsParallel())
                        {
                            var space = false;
                            var i = 0;
                            foreach (var cell in row.SelectNodes("th|td").Descendants("a").AsParallel())
                            {
                                space = true;
                                i++;
                                if (i <= 1) continue;
                                output.Append(cell.Id + " " + cell.InnerHtml.ToString(CultureInfo.CurrentCulture));
                            }
                            if (!space) continue;
                            output.AppendLine();
                        }
                        break;
                    }

                    await _growNotifications.AddNotificationAsync(
                        new Notification
                        {
                            Title = currentString,
                            ImageUrl = ImageUrls.NotificationUrl,
                            Message = output.ToString().ToLower()
                        });
                }
            }
            catch (Exception ex)
            {
                //ingore
            }
        }

        private string GetPostData(string fromLanguage, string toLanguage, string content)
        {
            var strPostData = $"key={_configurations.ApiKey}&lang={fromLanguage}-{toLanguage}&text={content}";
            return strPostData;
        }

        #region initialize & core

        private void CloseCbViewer()
        {
            Win32.ChangeClipboardChain(hWndSource.Handle, hWndNextViewer);
            hWndNextViewer = IntPtr.Zero;
            hWndSource.RemoveHook(WinProc);
            RichCurrentText.Document.Blocks.Clear();
            isViewing = false;
        }

        private void InitCbViewer()
        {
            var wih = new WindowInteropHelper(this);
            hWndSource = HwndSource.FromHwnd(wih.Handle);

            var source = hWndSource;

            if (source != null)
            {
                source.AddHook(WinProc); // start processing window messages
                hWndNextViewer = Win32.SetClipboardViewer(source.Handle); // set this window as a viewer
            }

            isViewing = true;
        }

        private IntPtr WinProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case Win32.WmChangecbchain:
                    if (wParam == hWndNextViewer)
                    {
                        // clipboard viewer chain changed, need to fix it.
                        hWndNextViewer = lParam;
                    }
                    else if (hWndNextViewer != IntPtr.Zero)
                    {
                        // pass the message to the next viewer.
                        Win32.SendMessage(hWndNextViewer, msg, wParam, lParam);
                    }

                    break;
                case Win32.WmDrawclipboard:

                    // clipboard content changed
                    DrawContent();

                    // pass the message to the next viewer.
                    Win32.SendMessage(hWndNextViewer, msg, wParam, lParam);
                    break;
            }

            return IntPtr.Zero;
        }

        #endregion
    }
}