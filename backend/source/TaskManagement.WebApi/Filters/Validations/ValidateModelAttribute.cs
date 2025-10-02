using Microsoft.AspNetCore.Mvc.Filters;

namespace TaskManagementWebApi.Filters.Validations
{
    /// <summary>
    /// Validate Model Attribute
    /// </summary>
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// OnActionExecuting
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new ValidationFailedResult(context.ModelState);
            }

            base.OnActionExecuting(context);
        }
    }
}







