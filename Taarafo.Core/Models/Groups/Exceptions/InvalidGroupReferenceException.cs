// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Taarafo.Core.Models.Groups.Exceptions
{
    public class InvalidGroupReferenceException : Xeption
    {
        public InvalidGroupReferenceException(Exception innerException)
            : base(
                message: "Invalid group reference error occurred.",
                innerException: innerException)
        { }

        public InvalidGroupReferenceException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}