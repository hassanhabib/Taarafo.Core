// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Taarafo.Core.Models.Profiles.Exceptions
{
    public class ProfileDependencyValidationException : Xeption
    {
        public ProfileDependencyValidationException(Xeption innerException)
            : base(
                message: "Profile dependency validation occurred, please try again.",
                innerException: innerException)
        { }
        
        public ProfileDependencyValidationException(string message, Xeption innerException)
            : base(message, innerException)
        { }
    }
}