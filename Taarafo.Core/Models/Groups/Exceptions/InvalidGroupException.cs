// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Taarafo.Core.Models.Groups.Exceptions
{
    public class InvalidGroupException : Xeption
    {
        public InvalidGroupException()
            : base(message: "Invalid group. Please correct the errors and try again.")
        { }

        public InvalidGroupException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}