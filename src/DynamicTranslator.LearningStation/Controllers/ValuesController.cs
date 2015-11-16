namespace DynamicTranslator.LearningStation.Controllers
{
    #region using

    using System.Web.Http;

    #endregion

    public class ValuesController : ApiController
    {
        public IHttpActionResult GetValues()
        {
            return Ok(new[] {"a", "b", "c"});
        }
    }
}