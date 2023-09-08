// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Taarafo.Core.Models.Groups.Exceptions
{
    public class GroupDependencyValidationException : Xeption
    {
        public GroupDependencyValidationException(Xeption innerException)
            : base(
                message: "Group dependency validation occurred, please try again.",
                innerException: innerException)
        { }

        public GroupDependencyValidationException(string message, Xeption innerException)
            : base(message, innerException)
        { }
    }
}