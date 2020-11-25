using System.Collections;
using System.Collections.Generic;
using PetGame.Common;

namespace PetGame.Web
{
    /// <summary>
    /// A generic class for returning an Error from the API. Should be used any time the API returns
    /// any kind of 400 response.
    /// </summary>
    public class ApiError : ErrorModel
    {
        /// <summary>
        /// Error collection. This field is purely for serialisation. 
        /// It MUST be of type `object` because otherwise properties for derived
        /// classes will not be serialised. Don't abuse it!
        /// For more details, see: https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-how-to?pivots=dotnet-core-3-1#serialize-properties-of-derived-classes
        /// </summary>
        /// <value></value>
        public List<object> errors { get; private set; }


        // The Great Sea of Constructors
        public ApiError()
        {
            this.errors = new List<object>();
        }

        public ApiError(string message) : this()
        {
            this.Message = message;
        }

        public ApiError(List<ErrorModel> errors) : this()
        {
            this.errors.AddRange(errors);
        }

        public ApiError(params ErrorModel[] errors) : this()
        {
            this.errors.AddRange(errors);
        }

        public ApiError(string message, List<ErrorModel> errors) : this()
        {
            this.Message = message;
            this.errors.AddRange(errors);
        }

        public ApiError(string message, params ErrorModel[] errors) : this()
        {
            this.Message = message;
            this.errors.AddRange(errors);
        }


        public override string GetModelName() => "ApiError";
        public override int GetModelVersionNumber() => 1;
    }
}