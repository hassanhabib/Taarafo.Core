// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Taarafo.Core.Models.Events.Exceptions
{
    public class InvalidEventException : Xeption
    {
        public InvalidEventException()
            : base(message: "Invalid event. Please correct the errors and try again.")
        { }

        public InvalidEventException(string message, Exception innerException)
            :base(message, innerException)
        { }
    }
}