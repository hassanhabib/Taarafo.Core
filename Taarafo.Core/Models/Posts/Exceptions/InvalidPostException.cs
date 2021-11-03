// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Taarafo.Core.Models.Posts.Exceptions
{
    public class InvalidPostException : Xeption
    {
        public InvalidPostException()
            : base(message: "Invalid post. Please correct the errors and try again.")
        { }
    }
}
