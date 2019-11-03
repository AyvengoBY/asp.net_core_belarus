using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace asp.net_core_belarus.Filters
{
    public class LogginActionFilter : IActionFilter
    {
        private readonly IConfiguration _config;
        private ILogger<LogginActionFilter> _logger;

        public LogginActionFilter(IConfiguration config, ILogger<LogginActionFilter> log)
        {
            _config = config;
            _logger = log;
        }
        public  void OnActionExecuting(ActionExecutingContext context)
        {
            var actionName = ((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor).ActionName;
            var controllerName = ((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor).ControllerName;

            if (Convert.ToBoolean(_config.GetSection("Logging")["ActionLogging"]))
            {
                _logger.LogInformation(Environment.NewLine + $"INFO :  ACTION  : {controllerName}.{actionName} : Start");
            }
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            var actionName = ((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor).ActionName;
            var controllerName = ((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor).ControllerName;

            if (Convert.ToBoolean(_config.GetSection("Logging")["ActionLogging"]))
            {
                _logger.LogInformation(Environment.NewLine + $"INFO :  ACTION  : {controllerName}.{actionName} : End");
            }
        }

    }
}
