// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Taarafo.Core.Models.PostImpressions.Exceptions
{
    public class AlreadyExistsPostImpressionException : Xeption
    {
        public AlreadyExistsPostImpressionException(Exception innerException)
            : base(
                message: "Post impression with the same id already exists.",
                innerException: innerException)
        { }

        public AlreadyExistsPostImpressionException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}