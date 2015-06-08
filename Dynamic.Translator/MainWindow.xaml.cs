namespace Dynamic.Tureng.Translator
{
    #region using

    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Net.Cache;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Documents;
    using System.Windows.Interop;
    using System.Windows.Threading;
    using System.Xml;
    using Dynamic.Tureng.Translator.Model;
    using Dynamic.Tureng.Translator.Utility;
    using HtmlAgilityPack;

    #endregion


    public partial class MainWindow
    {
        private static readonly string apiKey = ConfigurationManager.AppSettings["ApiKey"];
        private static readonly double leftOffset = double.Parse(ConfigurationManager.AppSettings["LeftOffset"]);
        private static readonly double topOffset = double.Parse(ConfigurationManager.AppSettings["TopOffset"]);
        private static readonly int searchableCharacterLimit = int.Parse(ConfigurationManager.AppSettings["SearchableCharacterLimit"]);
        private static readonly string fromLanguage = ConfigurationManager.AppSettings["FromLanguage"];
        private static readonly string toLanguage = ConfigurationManager.AppSettings["ToLanguage"];
        private static readonly Dictionary<string, string> languageMap = new Dictionary<string, string>();
        private static readonly Lazy<GrowlNotifiactions> pGrowNotifications = new Lazy<GrowlNotifiactions>(() => new GrowlNotifiactions());
        private string currentString;
        private IntPtr hWndNextViewer;
        private HwndSource hWndSource;
        private bool isViewing;
        private string previousString;

        public MainWindow()
        {
            InitializeComponent();
            InitLanguageMap(languageMap);
            GrowlNotifications.Top = SystemParameters.WorkArea.Top + topOffset;
            GrowlNotifications.Left = SystemParameters.WorkArea.Left + SystemParameters.WorkArea.Width - leftOffset;
            Application.Current.DispatcherUnhandledException += HandleUnhandledException;
        }

        private static GrowlNotifiactions GrowlNotifications => pGrowNotifications.Value;

        private void HandleUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            var comException = e.Exception as COMException;

            if (comException != null && comException.ErrorCode == -2147221040)
                e.Handled = true;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Close();
            GC.Collect();
            GC.SuppressFinalize(this);
            Application.Current.Shutdown();
        }

        private void btnSwitch_Click(object sender, RoutedEventArgs e)
        {
            if (!isViewing)
            {
                GrowlNotifications.AddNotification(
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

        private void ButtonClick1(object sender, RoutedEventArgs e)
        {
            GrowlNotifications.AddNotification(
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
                if (Clipboard.ContainsText())
                {
                    RichCurrentText.Document.Blocks.Add(new Paragraph(new Run(Clipboard.GetText())));
                    currentString = Clipboard.GetText();
                    if (previousString != currentString /*&& Regex.IsMatch(currentString, @"^[a-zA-Z0-9]+$")*/)
                    {
                        previousString = currentString;
                        if (currentString.Length > searchableCharacterLimit)
                        {
                            GrowlNotifications.AddNotification(
                                new Notification
                                {
                                    Title = Titles.MaximumLimit,
                                    ImageUrl = ImageUrls.NotificationUrl,
                                    Message = "You have exceed maximum character limit"
                                });
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(apiKey))
                            {
                                await GetMeanFromTureng();
                            }
                            else
                            {
                                GrowlNotifications.AddNotification(
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
            }
            catch (Exception e)
            {
                var exNotification = new GrowlNotifiactions
                {
                    Top = SystemParameters.WorkArea.Top + topOffset,
                    Left = SystemParameters.WorkArea.Left + SystemParameters.WorkArea.Width - leftOffset
                };
                exNotification.AddNotification(new Notification
                {
                    Title = Titles.Message,
                    ImageUrl = ImageUrls.NotificationUrl,
                    Message = ex.InnerException.Message
                });
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            CloseCbViewer();
        }

        private void WindowLoaded1(object sender, RoutedEventArgs e)
        {
            // this will make minimize restore of notifications too
            // growlNotifications.Owner = GetWindow(this);
        }

        public async Task GetMeanFromTureng()
        {
            var address1 = "http://tureng.com/search/";
            var turenClient = new WebClient
            {
                Headers =
                {
                    [HttpRequestHeader.UserAgent] =
                        "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/43.0.2357.81 Safari/537.36"
                },
                Encoding = Encoding.UTF8
            };

            turenClient.Headers.Add(HttpRequestHeader.AcceptLanguage, "en-US,en;q=0.8,tr;q=0.6");
            turenClient.CachePolicy = new HttpRequestCachePolicy(HttpCacheAgeControl.MaxAge, TimeSpan.FromHours(1));
            turenClient.DownloadStringAsync(new Uri(address1 + $"{currentString}"));
            turenClient.DownloadStringCompleted += wcTureng_DownloadStringCompleted;
        }

        public async Task GetMeanFromYandex()
        {
            var address2 =
                new Uri(
                    string.Format("https://translate.yandex.net/api/v1.5/tr/translate?" +
                                  GetPostData(languageMap[fromLanguage], languageMap[toLanguage], currentString)));
            var yandexClient = new WebClient { Encoding = Encoding.UTF8 };
            yandexClient.DownloadStringAsync(address2);
            yandexClient.CachePolicy = new HttpRequestCachePolicy(HttpCacheAgeControl.MaxAge, TimeSpan.FromHours(1));
            yandexClient.DownloadStringCompleted += wcYandex_DownloadStringCompleted;
        }

        private async void wcYandex_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                if (e.Result != null)
                {
                    var doc = new XmlDocument();
                    doc.LoadXml(e.Result);
                    var node = doc.SelectSingleNode("//Translation/text");
                    var output = node != null ? node.InnerText : e.Error.Message;
                    GrowlNotifications.AddNotification(
                        new Notification
                        {
                            Title = currentString,
                            ImageUrl = ImageUrls.NotificationUrl,
                            Message = output.ToLower()
                        });
                }
            }
            catch (Exception ex)
            {
                var exNotification = new GrowlNotifiactions
                {
                    Top = SystemParameters.WorkArea.Top + topOffset,
                    Left = SystemParameters.WorkArea.Left + SystemParameters.WorkArea.Width - leftOffset
                };
                exNotification.AddNotification(new Notification
                {
                    Title = Titles.Message,
                    ImageUrl = ImageUrls.NotificationUrl,
                    Message = ex.InnerException.Message
                });
            }
        }

        private async void wcTureng_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            Contract.Requires(sender != null);
            Contract.Requires(e != null);
            try
            {
                if (e.Result != null)
                {
                    var result = e.Result;
                    var output = new StringBuilder();
                    var doc = new HtmlDocument();
                    var decoded = WebUtility.HtmlDecode(result);
                    doc.LoadHtml(decoded);
                    if (result.Contains("table") && doc.DocumentNode.SelectSingleNode("//table") != null)
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
                                    if (i > 1)
                                    {
                                        output.Append(cell.Id + " " + cell.InnerHtml.ToString(CultureInfo.CurrentCulture));
                                    }
                                }
                                if (space)
                                {
                                    output.AppendLine();
                                }
                            }
                            break;
                        }

                        GrowlNotifications.AddNotification(
                            new Notification
                            {
                                Title = currentString,
                                ImageUrl = ImageUrls.NotificationUrl,
                                Message = output.ToString().ToLower()
                            });
                    }
                    else
                    {
                        await GetMeanFromYandex();
                    }
                }
            }
            catch (Exception ex)
            {
                var exNotification = new GrowlNotifiactions();
                exNotification.Top = SystemParameters.WorkArea.Top + topOffset;
                exNotification.Left = SystemParameters.WorkArea.Left + SystemParameters.WorkArea.Width - leftOffset;
                exNotification.AddNotification(new Notification
                {
                    Title = Titles.Message,
                    ImageUrl = ImageUrls.NotificationUrl,
                    Message = ex.InnerException.Message
                });
            }
        }

        private static string GetPostData(string fromLanguage, string toLanguage, string content)
        {
            var strPostData = string.Format("key={0}&lang={1}-{2}&text={3}", apiKey, fromLanguage, toLanguage, content);
            return strPostData;
        }

        #region initialize & core

        private void CloseCbViewer()
        {
            // remove this window from the clipboard viewer chain
            Win32.ChangeClipboardChain(hWndSource.Handle, hWndNextViewer);
            hWndNextViewer = IntPtr.Zero;
            hWndSource.RemoveHook(WinProc);
            RichCurrentText.Document.Blocks.Clear();
            isViewing = false;
        }

        private void InitCbViewer()
        {
            var wih = new WindowInteropHelper(this);
            this.hWndSource = HwndSource.FromHwnd(wih.Handle);

            var hWndSource = this.hWndSource;

            if (hWndSource != null)
            {
                hWndSource.AddHook(WinProc); // start processing window messages
                hWndNextViewer = Win32.SetClipboardViewer(hWndSource.Handle); // set this window as a viewer
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

        private static void InitLanguageMap(Dictionary<string, string> languageMap)
        {
            languageMap.Add("Afrikaans", "af");
            languageMap.Add("Albanian", "sq");
            languageMap.Add("Arabic", "ar");
            languageMap.Add("Armenian", "hy");
            languageMap.Add("Azerbaijani", "az");
            languageMap.Add("Basque", "eu");
            languageMap.Add("Belarusian", "be");
            languageMap.Add("Bengali", "bn");
            languageMap.Add("Bulgarian", "bg");
            languageMap.Add("Catalan", "ca");
            languageMap.Add("Chinese", "zh-CN");
            languageMap.Add("Croatian", "hr");
            languageMap.Add("Czech", "cs");
            languageMap.Add("Danish", "da");
            languageMap.Add("Dutch", "nl");
            languageMap.Add("English", "en");
            languageMap.Add("Esperanto", "eo");
            languageMap.Add("Estonian", "et");
            languageMap.Add("Filipino", "tl");
            languageMap.Add("Finnish", "fi");
            languageMap.Add("French", "fr");
            languageMap.Add("Galician", "gl");
            languageMap.Add("German", "de");
            languageMap.Add("Georgian", "ka");
            languageMap.Add("Greek", "el");
            languageMap.Add("Haitian Creole", "ht");
            languageMap.Add("Hebrew", "iw");
            languageMap.Add("Hindi", "hi");
            languageMap.Add("Hungarian", "hu");
            languageMap.Add("Icelandic", "is");
            languageMap.Add("Indonesian", "id");
            languageMap.Add("Irish", "ga");
            languageMap.Add("Italian", "it");
            languageMap.Add("Japanese", "ja");
            languageMap.Add("Korean", "ko");
            languageMap.Add("Lao", "lo");
            languageMap.Add("Latin", "la");
            languageMap.Add("Latvian", "lv");
            languageMap.Add("Lithuanian", "lt");
            languageMap.Add("Macedonian", "mk");
            languageMap.Add("Malay", "ms");
            languageMap.Add("Maltese", "mt");
            languageMap.Add("Norwegian", "no");
            languageMap.Add("Persian", "fa");
            languageMap.Add("Polish", "pl");
            languageMap.Add("Portuguese", "pt");
            languageMap.Add("Romanian", "ro");
            languageMap.Add("Russian", "ru");
            languageMap.Add("Serbian", "sr");
            languageMap.Add("Slovak", "sk");
            languageMap.Add("Slovenian", "sl");
            languageMap.Add("Spanish", "es");
            languageMap.Add("Swahili", "sw");
            languageMap.Add("Swedish", "sv");
            languageMap.Add("Tamil", "ta");
            languageMap.Add("Telugu", "te");
            languageMap.Add("Thai", "th");
            languageMap.Add("Turkish", "tr");
            languageMap.Add("Ukrainian", "uk");
            languageMap.Add("Urdu", "ur");
            languageMap.Add("Vietnamese", "vi");
            languageMap.Add("Welsh", "cy");
            languageMap.Add("Yiddish", "yi");
        }

        #endregion
    }
}
