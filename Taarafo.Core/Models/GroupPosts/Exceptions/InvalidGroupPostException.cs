// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Taarafo.Core.Models.GroupPosts.Exceptions
{
    public class InvalidGroupPostException : Xeption
    {
        public InvalidGroupPostException()
            : base(message: "Invalid group post. Please correct the errors and try again.")
        { }
        
        public InvalidGroupPostException(string message)
            : base(message)
        { }
    }
}