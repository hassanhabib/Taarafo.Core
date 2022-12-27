// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Taarafo.Core.Models.GroupPosts.Exceptions
{
    public class NullTicketException : Xeption
    {
        public NullTicketException()
            : base(message: "GroupPost is null.")
        { }
    }
}