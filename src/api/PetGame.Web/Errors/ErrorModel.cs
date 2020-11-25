namespace PetGame.Web
{
    /// <summary>
    /// Base class for API errors.
    /// </summary>
    public abstract class ErrorModel
    {
        /// <summary>
        /// Unique name for this model. Used in conjunction with `GetModelVersionNumber`
        ///     by client apps to bind the payload to a concrete type / know what fields to expect
        /// @NOTE That if this value ever changes, client code will need to update too.
        /// </summary>
        public string Model => this.GetModelName();
        /// <summary>
        /// Version number for the fields in this model. 
        /// @NOTE DO NOT CHANGE THE PROPERTIES ON THIS MODEL WITHOUT INCREASING THE VERSION NUMBER.
        /// Used by client apps in conjunction with `GetModelName` to bind the payload
        ///     to a concrete type / know what fields to expect
        /// </summary>
        public int ModelVersion => this.GetModelVersionNumber();
        /// <summary>
        /// Optional human-readable message to include in the error payload, describing what the issue is.
        /// This message may (or may not) be displayed in client apps.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Unique name for this model. Used in conjunction with `GetModelVersionNumber`
        ///     by client apps to bind the payload to a concrete type / know what fields to expect
        /// @NOTE That if this value ever changes, client code will need to update too.
        /// </summary>
        public abstract string GetModelName();
        /// <summary>
        /// Version number for the fields in this model. 
        /// @NOTE DO NOT CHANGE THE PROPERTIES ON THIS MODEL WITHOUT INCREASING THE VERSION NUMBER.
        /// Used by client apps in conjunction with `GetModelName` to bind the payload
        ///     to a concrete type / know what fields to expect
        /// </summary>
        public abstract int GetModelVersionNumber();
    }
}