using System.Text.Json.Serialization;
using PetGame.Common;

namespace PetGame.Web
{
    /// <summary>
    /// A validation error indicating that a field in a request to the API was incorrect.
    /// </summary>
    public class RequestValidationError : ErrorModel
    {
        public override string GetModelName() => "RequestValidationError";
        public override int GetModelVersionNumber() => 1;

        /// <summary>
        /// The name of the field in the request that was not valid
        /// </summary>
        public string Field { get; set; }

        public RequestValidationError(string field)
        {
            this.Field = field;
        }

        public RequestValidationError(string field, string message) : this(field)
        {
            this.Message = message;
        }
    }
}