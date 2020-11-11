namespace DynamicTranslator.Core.Prompt
{
    internal class AdviseParams
    {
        public bool ShowIntime { get; set; }

        public object AdviseText { get; set; }
    }

    internal class D
    {
        public string Type { get; set; }

        public string FormSeek { get; set; }

        public string PtsTopic { get; set; }

        public string Provider { get; set; }

        public bool IsUrl { get; set; }

        public bool IsWord { get; set; }

        public string PtsDirCode { get; set; }

        public string Advise { get; set; }

        public int AutoCode { get; set; }

        public string Result { get; set; }

        public string ResultNoTags { get; set; }

        public string DirNames { get; set; }

        public AdviseParams AdviseParams { get; set; }

        public string Key4SaveEdits { get; set; }

        public string TrId { get; set; }

        public string CanonForm { get; set; }

        public string FdLink { get; set; }

        public int ErrCodeInt { get; set; }

        public int ErrCode { get; set; }

        public object ErrMessage { get; set; }
    }

    internal class PromptResult
    {
        public D D { get; set; }
    }
}