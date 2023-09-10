// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Taarafo.Core.Models.PostImpressions.Exceptions
{
    public class NullPostImpressionException : Xeption
    {
        public NullPostImpressionException()
            : base(message: "Post impression is null.")
        { }

        public NullPostImpressionException(string message)
            : base(message)
        { }
    }
}