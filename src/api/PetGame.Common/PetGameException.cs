using System;

namespace PetGame.Common
{
    /// <summary>
    /// A generic exception for the entire application to use and extend from.
    /// You must provide an ErrorId to uniquely identify the error.
    /// </summary>
    public abstract class PetGameException : Exception
    {
        /// <summary>
        /// Unique ErrorId to identify the exception.
        /// </summary>
        public ErrorId ErrorId { get; private set; }

        /// <param name="message">Exception message (natural language, human-readable)</param>
        /// <param name="errorId">Unique ErrorId to identify the exception (machine-readable)</param>
        /// <returns></returns>
        public PetGameException(string message, ErrorId errorId) : base(message)
        {
            this.ErrorId = errorId;
        }
    }
}
