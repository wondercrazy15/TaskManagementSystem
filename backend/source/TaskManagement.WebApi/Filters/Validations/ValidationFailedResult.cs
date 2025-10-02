using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace TaskManagementWebApi.Filters.Validations
{
    /// <summary>
    /// Validation Failed Result
    /// </summary>
    public class ValidationFailedResult : ObjectResult
    {
        /// <summary>
        /// Validation Failed Result
        /// </summary>
        /// <param name="modelState"></param>
        public ValidationFailedResult(ModelStateDictionary modelState)
            : base(new ValidationResultModel(modelState))
        {
            StatusCode = StatusCodes.Status400BadRequest;
        }
    }
}







