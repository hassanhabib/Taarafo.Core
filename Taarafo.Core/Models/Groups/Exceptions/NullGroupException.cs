// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Taarafo.Core.Models.Groups.Exceptions
{
    public class NullGroupException : Xeption
    {
        public NullGroupException()
            : base(message: "Group is null.")
        { }
    }
}

