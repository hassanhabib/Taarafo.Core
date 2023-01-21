// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Taarafo.Core.Models.Events.Exceptions
{
    public class InvalidEventReferenceException : Xeption
    {
        public InvalidEventReferenceException(Exception innerException)
            : base(message: "Invalid eventt reference error occurred.", innerException)
        { }
    }
}
