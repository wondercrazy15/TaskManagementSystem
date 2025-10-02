using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace TaskManagementWebApi.Filters.Validations
{
    /// <summary>
    /// Validation Result Model
    /// </summary>
    public class ValidationResultModel
    {
        /// <summary>
        /// Message
        /// </summary>
        public string? Message { get; }

        /// <summary>
        /// Errors
        /// </summary>
        public List<ValidationError> Errors { get; }

        /// <summary>
        /// Validation Result Model
        /// </summary>
        /// <param name="modelState"></param>
        public ValidationResultModel(ModelStateDictionary modelState)
        {
            Message = "Validation Failed";
            Errors = modelState != null && modelState.Keys.Count() > 0 ? modelState.Keys
                    .SelectMany(key => modelState[key].Errors.Select(x => new ValidationError(key, 0, x.ErrorMessage)))
                    .ToList() : new List<ValidationError>();
        }
    }
}







