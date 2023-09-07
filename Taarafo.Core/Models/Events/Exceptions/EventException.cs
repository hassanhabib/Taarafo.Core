// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Taarafo.Core.Models.Events.Exceptions
{
    public class EventException : Xeption
    {
        public EventException(Exception innerException)
            : base(
                message: "Event service error occurred, please contact support.",
                innerException)
        { }

        public EventException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
