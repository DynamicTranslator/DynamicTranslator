namespace DynamicTranslator.Prompt
{
    internal class AdviseParams
    {
        public bool showIntime { get; set; }

        public object adviseText { get; set; }
    }

    internal class D
    {
        public string __type { get; set; }

        public string formSeek { get; set; }

        public string ptsTopic { get; set; }

        public string provider { get; set; }

        public bool isURL { get; set; }

        public bool isWord { get; set; }

        public string ptsDirCode { get; set; }

        public string advise { get; set; }

        public int autoCode { get; set; }

        public string result { get; set; }

        public string resultNoTags { get; set; }

        public string dirNames { get; set; }

        public AdviseParams adviseParams { get; set; }

        public string key4saveEdits { get; set; }

        public string trId { get; set; }

        public string canonForm { get; set; }

        public string fdLink { get; set; }

        public int errCodeInt { get; set; }

        public int errCode { get; set; }

        public object errMessage { get; set; }
    }

    internal class PromptResult
    {
        public D d { get; set; }
    }
}