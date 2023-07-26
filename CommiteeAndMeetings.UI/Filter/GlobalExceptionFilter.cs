using CommiteeAndMeetings.BLL.Contexts;
using CommiteeAndMeetings.DAL.Domains;
using IHelperServices;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;

namespace CommiteeAndMeetings.UI.Filter
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        ILogger<GlobalExceptionFilter> logger = null;
        ISessionServices sessionServices;
        private MasarContext _DbContext = new MasarContext();
        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> exceptionLogger, ISessionServices _sessionServices)
        {
            logger = exceptionLogger;
            sessionServices = _sessionServices;
        }
        

        public void OnException(ExceptionContext context)
        {
            _DbContext.MasarExceptions.Add(new MasarException
            {
                Message = context.Exception.Message.ToString(),
                StackTrace = context.Exception.StackTrace,
                Time = DateTime.Now,
                UserRoleId = sessionServices.UserRoleId
            });
            _DbContext.SaveChanges();
            // sessionServices.UserRoleId;

            // log the exception
            logger.LogError(0, context.Exception.GetBaseException(), "Exception occurred.");
        }
    }
}
