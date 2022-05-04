// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Taarafo.Core.Models.Groups.Exceptions
{
    public class GroupValidationException : Xeption
    {
        public GroupValidationException(Xeption innerException)
            : base(message: "Group validation errors occurred, please try again.",
                  innerException)
        { }
    }
}