using PetGame.Common;

namespace PetGame.Web
{
    /// <summary>
    /// A generic error that occurred in the API. Usually an attempted action that can't work, or isn't allowed.
    /// </summary>
    public class GenericError : ErrorModel
    {
        public override string GetModelName() => "GenericError";
        public override int GetModelVersionNumber() => 1;

        /// <summary>
        /// Unique ErrorId for client apps to identify this error.
        /// @NOTE That if you ever change the value of this (either by passing a different ErrorId or 
        ///     even by refactoring the name of the enum) you MUST also update any client apps to reflect this. 
        /// IDEALLY this would never change.
        /// </summary>
        public ErrorId Id { get; set; }

        public GenericError(ErrorId id)
        {
            this.Id = id;
        }

        public GenericError(ErrorId id, string message) : this(id)
        {
            this.Message = message;
        }

        public GenericError(PetGameException sourceException)
        {
            this.Id = sourceException.ErrorId;
            this.Message = sourceException.Message;
        }
    }
};
