namespace Dynamic.Translator.Core.ApplicationKernel
{
    #region using

    using System;
    using System.Threading.Tasks;

    #endregion

    public interface IFinder
    {
        Task DrawContent();
        Task GetMeanFromTureng();
        Task GetMeanFromYandex();
        string GetPostData(string fromLanguage, string toLanguage, string content);
        IntPtr WinProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled);
        void InitCbViewer();
        void CloseCbViewer();
    }
}