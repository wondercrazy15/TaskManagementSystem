using Newtonsoft.Json;

namespace TaskManagementWebApi.Filters.Validations
{
    /// <summary>
    /// Validation Error
    /// </summary>
    public class ValidationError
    {
        /// <summary>
        /// Field
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? Field { get; }

        /// <summary>
        /// Message
        /// </summary>
        public string? Message { get; }

        /// <summary>
        /// ValidationError
        /// </summary>
        /// <param name="field">field</param>
        /// <param name="code">code</param>
        /// <param name="message">message</param>
        public ValidationError(string field, int code, string message)
        {
            Field = field != string.Empty ? field : null;
            Message = message;
        }
    }
}







