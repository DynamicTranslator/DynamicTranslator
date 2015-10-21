namespace Dynamic.Tureng.Translator
{
    #region using

    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Net.Cache;
    using System.Reactive.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Documents;
    using System.Windows.Forms;
    using System.Windows.Interop;
    using System.Windows.Threading;
    using System.Xml;
    using Dynamic.Translator.Core.Config;
    using Dynamic.Translator.Core.Dependency;
    using Dynamic.Translator.Core.ViewModel;
    using Orchestrator;
    using Orchestrator.Observable;
    using Utility;
    using Application = System.Windows.Application;
    using Clipboard = System.Windows.Clipboard;
    using HtmlDocument = HtmlAgilityPack.HtmlDocument;

    #endregion

    public partial class MainWindow
    {
        private readonly IStartupConfiguration _configurations;
        private readonly GrowlNotifiactions _growNotifications;
        private readonly Dictionary<string, string> _languageMap;
        private CancellationToken cancellationToken;
        private CancellationTokenSource cancellationTokenSource;
        private string currentString;
        private IntPtr hWndNextViewer;
        private HwndSource hWndSource;
        private bool isViewing;
        private string previousString;

        public MainWindow()
        {
            this.InitializeComponent();

            this._configurations = IocManager.Instance.Resolve<IStartupConfiguration>();
            this._growNotifications = IocManager.Instance.Resolve<GrowlNotifiactions>();

            this._languageMap = this._configurations.LanguageMap;
            this._growNotifications.Top = SystemParameters.WorkArea.Top + this._configurations.TopOffset;
            this._growNotifications.Left = SystemParameters.WorkArea.Left + SystemParameters.WorkArea.Width - this._configurations.LeftOffset;
            this._growNotifications.OnDispose += ClearAllNotifications;
            Application.Current.DispatcherUnhandledException += this.HandleUnhandledException;
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
            if (this.cancellationToken.CanBeCanceled)
            {
                this.cancellationTokenSource.Cancel();
            }
            this.Close();
            GC.Collect();
            GC.SuppressFinalize(this);
            Application.Current.Shutdown();
        }

        private void InitializeCopyPasteListenerAsync()
        {
            this.cancellationTokenSource = new CancellationTokenSource();
            this.cancellationToken = this.cancellationTokenSource.Token;

            Task.Run(() =>
            {
                while (true)
                {
                    if (this.cancellationToken.IsCancellationRequested)
                    {
                        break;
                    }
                    Thread.Sleep(740);
                    SendKeys.SendWait("^c");
                }
            }, this.cancellationToken);
        }

        private async void btnSwitch_Click(object sender, RoutedEventArgs e)
        {
            var translator = new Translator(this);
            var translatorEvents = Observable
                .FromEventPattern(
                    h => translator.WhenClipboardContainsTextEventHandler += h,
                    h => translator.WhenClipboardContainsTextEventHandler -= h);

            var notifierEvents = Observable
                .FromEventPattern<WhenNotificationAddEventArgs>(
                    h => translator.WhenNotificationAddEventHandler += h,
                    h => translator.WhenNotificationAddEventHandler -= h);

            translatorEvents.Subscribe(new Finder(translator));
            notifierEvents.Subscribe(new Notifier(translator, this._growNotifications));


            if (!this.isViewing)
            {
                this.InitializeCopyPasteListenerAsync();
                await this._growNotifications.AddNotificationAsync(
                    new Notification
                    {
                        Title = Titles.StartingMessage,
                        ImageUrl = ImageUrls.NotificationUrl,
                        Message = "The translator will listen your clipboard and it'll translate everything !"
                    });
                this.InitCbViewer();
                this.BtnSwitch.Content = "Stop Translator";
            }
            else
            {
                if (this.cancellationToken.CanBeCanceled)
                {
                    this.cancellationTokenSource.Cancel();
                }
                this.CloseCbViewer();
                this.BtnSwitch.Content = "Start Translator";
            }
        }

        private async void ButtonClick1(object sender, RoutedEventArgs e)
        {
            await this._growNotifications.AddNotificationAsync(
                new Notification
                {
                    Title = Titles.Message,
                    ImageUrl = ImageUrls.NotificationUrl,
                    Message = "It's a toast notification !!!"
                });
        }

        private async void DrawContent()
        {
            this.RichCurrentText.Document.Blocks.Clear();
            try
            {
                if (!Clipboard.ContainsText()) return;

                this.RichCurrentText.Document.Blocks.Add(new Paragraph(new Run(Clipboard.GetText())));
                this.currentString = Clipboard.GetText();
                if (this.previousString != this.currentString /*&& Regex.IsMatch(currentString, @"^[a-zA-Z0-9]+$")*/)
                {
                    this.previousString = this.currentString;
                    if (this.currentString.Length > this._configurations.SearchableCharacterLimit)
                    {
                        await this._growNotifications.AddNotificationAsync(new Notification
                        {
                            Title = Titles.MaximumLimit,
                            ImageUrl = ImageUrls.NotificationUrl,
                            Message = "You have exceed maximum character limit"
                        });
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(this._configurations.ApiKey))
                        {
                            await this.GetMeanFromTureng();
                        }
                        else
                        {
                            await this._growNotifications.AddNotificationAsync(
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
            turenClient.DownloadStringAsync(new Uri(address1 + this.currentString));
            turenClient.DownloadStringCompleted += this.wcTureng_DownloadStringCompleted;
        }

        private async Task GetMeanFromYandex()
        {
            var address2 = new Uri(
                string.Format(
                    "https://translate.yandex.net/api/v1.5/tr/translate?" +
                    this.GetPostData(this._languageMap[this._configurations.FromLanguage], this._languageMap[this._configurations.ToLanguage], this.currentString)));

            var yandexClient = new WebClient {Encoding = Encoding.UTF8};
            yandexClient.DownloadStringAsync(address2);
            yandexClient.CachePolicy = new HttpRequestCachePolicy(HttpCacheAgeControl.MaxAge, TimeSpan.FromHours(1));
            yandexClient.DownloadStringCompleted += this.wcYandex_DownloadStringCompleted;
        }


        private async Task GetMeanFromSesliSozluk()
        {
            var address =
                new Uri(string.Format("http://api.seslisozluk.com/?key=1234567890abcdef&lang_from={0}&query={1}&callback=?", this._languageMap[this._configurations.FromLanguage],
                    this.currentString));

            var sesliSozlukClient = new WebClient {Encoding = Encoding.UTF8};
            sesliSozlukClient.DownloadStringAsync(address);
            sesliSozlukClient.CachePolicy = new HttpRequestCachePolicy(HttpCacheAgeControl.MaxAge, TimeSpan.FromHours(1));
            sesliSozlukClient.DownloadStringCompleted += this.wcSesliSozluk_DownloadStringCompleted;
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

                await this._growNotifications.AddNotificationAsync(
                    new Notification
                    {
                        Title = this.currentString,
                        ImageUrl = ImageUrls.NotificationUrl,
                        Message = output.ToLower()
                    });
            }
            catch (Exception ex)
            {
                //ingore
            }
        }

        private async void wcSesliSozluk_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                if (e.Result == null) return;

                var doc = new XmlDocument();
                doc.LoadXml(e.Result);
                var node = doc.SelectSingleNode("//Translation/text");
                var output = node?.InnerText ?? e.Error.Message;

                await this._growNotifications.AddNotificationAsync(
                    new Notification
                    {
                        Title = this.currentString,
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
                    await this.GetMeanFromYandex();
                    //await GetMeanFromSesliSozluk();
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
                                var word = cell.InnerHtml.ToString(CultureInfo.CurrentCulture);
                                space = true;
                                i++;
                                if (i <= 1) continue;
                                if (output.ToString().Contains(word))
                                {
                                    space = false;
                                    continue;
                                }
                                output.Append(cell.Id + " " + word);
                            }
                            if (!space) continue;
                            output.AppendLine();
                        }
                        break;
                    }

                    await this._growNotifications.AddNotificationAsync(new Notification
                    {
                        Title = this.currentString,
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
            var strPostData = $"key={this._configurations.ApiKey}&lang={fromLanguage}-{toLanguage}&text={content}";
            return strPostData;
        }

        #region initialize & core

        private void CloseCbViewer()
        {
            Win32.ChangeClipboardChain(this.hWndSource.Handle, this.hWndNextViewer);
            this.hWndNextViewer = IntPtr.Zero;
            this.hWndSource.RemoveHook(this.WinProc);
            this.RichCurrentText.Document.Blocks.Clear();
            this.isViewing = false;
        }

        private void InitCbViewer()
        {
            var wih = new WindowInteropHelper(this);
            this.hWndSource = HwndSource.FromHwnd(wih.Handle);

            var source = this.hWndSource;

            if (source != null)
            {
                source.AddHook(this.WinProc); // start processing window messages
                this.hWndNextViewer = Win32.SetClipboardViewer(source.Handle); // set this window as a viewer
            }

            this.isViewing = true;
        }

        private IntPtr WinProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case Win32.WmChangecbchain:
                    if (wParam == this.hWndNextViewer)
                    {
                        // clipboard viewer chain changed, need to fix it.
                        this.hWndNextViewer = lParam;
                    }
                    else if (this.hWndNextViewer != IntPtr.Zero)
                    {
                        // pass the message to the next viewer.
                        Win32.SendMessage(this.hWndNextViewer, msg, wParam, lParam);
                    }

                    break;
                case Win32.WmDrawclipboard:

                    // clipboard content changed
                    this.DrawContent();

                    // pass the message to the next viewer.
                    Win32.SendMessage(this.hWndNextViewer, msg, wParam, lParam);
                    break;
            }

            return IntPtr.Zero;
        }

        #endregion
    }
}