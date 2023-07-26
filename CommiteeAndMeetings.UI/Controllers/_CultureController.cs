using Microsoft.AspNetCore.Mvc;
using System;

namespace CommiteeAndMeetings.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CultureController : ControllerBase
    {
        private readonly IHelperServices.ISessionServices _SessionServices;
        public CultureController(IHelperServices.ISessionServices sessionServices)
        {
            this._SessionServices = sessionServices;
        }
        [HttpPost, Route("UpdateCultureSession")]
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        public void UpdateCultureSession(string key)
        {
            try
            {
                _SessionServices.Culture = !string.IsNullOrEmpty(key) ? key : _SessionServices.Culture;
            }
            catch (Exception ex)
            {
                if (System.IO.File.Exists("C:\\testemail.txt"))
                {
                    using (System.IO.StreamWriter sw = System.IO.File.AppendText("C:\\testemail.txt"))
                    {
                        sw.WriteLine("function= UpdateCultureSession");
                        sw.WriteLine("Mes=" + ex.Message);
                        sw.WriteLine("Stack Trace=" + ex.StackTrace);
                    }
                }
            }
        }
    }
}
