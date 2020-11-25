using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace PetGame.Web
{
    // @NOTE wow I'm super sorry about this but I just didn't want to have to call `this.____` all the time
    public class ControllerBase : Microsoft.AspNetCore.Mvc.ControllerBase
    {
        /// <summary>
        /// Shorthand for a string:string dictionary, used by `ValidationError()`.
        /// </summary>
        public class ValidationErrors : Dictionary<string, string> { }

        /// <summary>
        /// Return a common ApiError with a set of validation errors, specified as a dictionary.
        /// </summary>
        /// <param name="errors">Dictionary of validation errors. `Key` is the field name. `Value` is the error message.</param>
        /// <returns></returns>
        [NonAction]
        public ActionResult ValidationError(ValidationErrors errors)
        {
            // Map dictionary of messages to RequestValidationError models 
            List<ErrorModel> validationErrors = errors
                .Select(entry =>
                    new RequestValidationError(field: entry.Key, message: entry.Value) as ErrorModel
                )
                .ToList();

            ApiError response = new ApiError(message: validationErrors.Count == 1 ?
                $"There was a validation error in your request" :
                $"There were {validationErrors.Count} validation errors in your request",
                errors: validationErrors);

            return new BadRequestObjectResult(response);
        }
    }
}