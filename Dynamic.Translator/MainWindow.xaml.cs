// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the MainWindow type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Dynamic.Tureng.Translator
{
    #region Using

    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Net;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Windows;
    using System.Windows.Documents;
    using System.Windows.Interop;
    using System.Windows.Threading;
    using System.Xml;

    #endregion

    public partial class MainWindow
    {
        private static readonly string ApiKey = ConfigurationManager.AppSettings["ApiKey"];
        private static readonly double LeftOffset = double.Parse(ConfigurationManager.AppSettings["LeftOffset"]);
        private static readonly double TopOffset = double.Parse(ConfigurationManager.AppSettings["TopOffset"]);

        private static readonly int SearchableCharacterLimit =
            int.Parse(ConfigurationManager.AppSettings["SearchableCharacterLimit"]);

        private static readonly string FromLanguage = ConfigurationManager.AppSettings["FromLanguage"];
        private static readonly string ToLanguage = ConfigurationManager.AppSettings["ToLanguage"];
        private static readonly Dictionary<string, string> LanguageMap = new Dictionary<string, string>();
        private readonly GrowlNotifiactions _growlNotifications = new GrowlNotifiactions();
        private string _currenntString;

        /// <summary>
        ///     Next clipboard viewer window
        /// </summary>
        private IntPtr _hWndNextViewer;

        /// <summary>
        ///     The <see cref="HwndSource" /> for this window.
        /// </summary>
        private HwndSource _hWndSource;

        private bool _isViewing;
        private string _previousString;

        public MainWindow()
        {
            InitializeComponent();
            InitLanguageMap(LanguageMap);
            _growlNotifications.Top = SystemParameters.WorkArea.Top + TopOffset;
            _growlNotifications.Left = SystemParameters.WorkArea.Left + SystemParameters.WorkArea.Width - LeftOffset;
            Application.Current.DispatcherUnhandledException += HandleUnhandledException;
        }

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

            if (!_isViewing)
            {
                _growlNotifications.AddNotification(
                    new Notification
                    {
                        Title = Titles.StartingMessage,
                        ImageUrl = ImageUrls.NotificationUrl,
                        Message =
                            "The translator will listen your clipboard and it'll translate everything !"
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
            _growlNotifications.AddNotification(
                new Notification
                {
                    Title = Titles.Message,
                    ImageUrl = ImageUrls.NotificationUrl,
                    Message = "It's a toast notification !!!"
                });
        }

        private void CloseCbViewer()
        {
            // remove this window from the clipboard viewer chain
            Win32.ChangeClipboardChain(_hWndSource.Handle, _hWndNextViewer);
            _hWndNextViewer = IntPtr.Zero;
            _hWndSource.RemoveHook(WinProc);
            RichCurrentText.Document.Blocks.Clear();
            _isViewing = false;
        }

        private void DrawContent()
        {
            RichCurrentText.Document.Blocks.Clear();
            try
            {
                if (Clipboard.ContainsText())
                {
                    RichCurrentText.Document.Blocks.Add(new Paragraph(new Run(Clipboard.GetText())));
                    _currenntString = Clipboard.GetText();
                    if (_previousString != _currenntString)
                    {
                        _previousString = _currenntString;
                        if (_currenntString.Length > SearchableCharacterLimit)
                        {
                            _growlNotifications.AddNotification(
                                new Notification
                                {
                                    Title = Titles.MaximumLimit,
                                    ImageUrl = ImageUrls.NotificationUrl,
                                    Message = "You have exceed maximum character limit"
                                });
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(ApiKey))
                            {
                                GetMean();
                            }
                            else
                            {
                                _growlNotifications.AddNotification(
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
                _growlNotifications.AddNotification(
                    new Notification
                    {
                        Title = Titles.Exception,
                        ImageUrl = ImageUrls.NotificationUrl,
                        Message = e.Message + " InnerEx: " + e.InnerException
                    });
            }
        }

        private void InitCbViewer()
        {
            var wih = new WindowInteropHelper(this);
            _hWndSource = HwndSource.FromHwnd(wih.Handle);

            var hWndSource = _hWndSource;

            if (hWndSource != null)
            {
                hWndSource.AddHook(WinProc); // start processing window messages
                _hWndNextViewer = Win32.SetClipboardViewer(hWndSource.Handle); // set this window as a viewer
            }

            _isViewing = true;
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

        private IntPtr WinProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case Win32.WM_CHANGECBCHAIN:
                    if (wParam == _hWndNextViewer)
                    {
                        // clipboard viewer chain changed, need to fix it.
                        _hWndNextViewer = lParam;
                    }
                    else if (_hWndNextViewer != IntPtr.Zero)
                    {
                        // pass the message to the next viewer.
                        Win32.SendMessage(_hWndNextViewer, msg, wParam, lParam);
                    }

                    break;

                case Win32.WM_DRAWCLIPBOARD:

                    // clipboard content changed
                    DrawContent();

                    // pass the message to the next viewer.
                    Win32.SendMessage(_hWndNextViewer, msg, wParam, lParam);
                    break;
            }

            return IntPtr.Zero;
        }

        public void GetMean()
        {
            var address =
                new Uri(
                    string.Format(
                        "https://translate.yandex.net/api/v1.5/tr/translate?"
                        + GetPostData(LanguageMap[FromLanguage], LanguageMap[ToLanguage], _currenntString)));

            var wc = new WebClient();
            wc.Encoding = Encoding.UTF8;
            wc.DownloadStringAsync(address);
            wc.DownloadStringCompleted += wc_DownloadStringCompleted;
        }

        private void wc_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                if (e.Result != null)
                {
                    var doc = new XmlDocument();
                    doc.LoadXml(e.Result);
                    var node = doc.SelectSingleNode("//Translation/text");
                    var output = node != null ? node.InnerText : e.Error.Message;
                    _growlNotifications.AddNotification(
                        new Notification
                        {
                            Title = _currenntString,
                            ImageUrl = ImageUrls.NotificationUrl,
                            Message = output.ToLower()
                        });
                }
            }
            catch (Exception ex)
            {
                _growlNotifications.AddNotification(
                    new Notification
                    {
                        Title = Titles.Message,
                        ImageUrl = ImageUrls.NotificationUrl,
                        Message = ex.InnerException.Message
                    });
            }
        }

        private static string GetPostData(string fromLanguage, string toLanguage, string content)
        {
            var strPostData = string.Format("key={0}&lang={1}-{2}&text={3}", ApiKey, fromLanguage, toLanguage, content);
            return strPostData;
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

        private void UpdateConfig(string key, string value)
        {
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            configuration.AppSettings.Settings[key].Value = value;
            configuration.Save();

            ConfigurationManager.RefreshSection("appSettings");
        }
    }
}