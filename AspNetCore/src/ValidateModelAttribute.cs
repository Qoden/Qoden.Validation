using System.Linq;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Qoden.Validation.AspNetCore
{
    /// <summary>
    /// It checks the data model.
    /// </summary>
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Occurs before the request is processed.
        /// </summary>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ModelState.IsValid)
                return;

            var errorList = context.ModelState.Values
                .SelectMany(m => m.Errors)
                .Select(e => string.IsNullOrEmpty(e.ErrorMessage) 
                    ? e.Exception?.ToString() 
                    : e.ErrorMessage)
                .Select(x =>
                {
                    var err =  new Error("ModelState.IsValid", "{Msg}");
                    err.Info["Msg"] = x;
                    return err;
                })
                .ToList();

            throw new MultipleErrorsException(errorList);
        }
    }
}