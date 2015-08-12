namespace Dynamic.Translator.Core.ApplicationKernel
{
    #region using

    using Dependency;

    #endregion

    public abstract class ApplicationBase
    {
        protected internal IIocManager IocManager { get; internal set; }

        public virtual void PreInitialize()
        {
        }

        public virtual void Initialize()
        {
        }

        public virtual void PostInitialize()
        {
        }

        public virtual void Shutdown()
        {
        }
    }
}