// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Taarafo.Core.Models.Events.Exceptions
{
    public class EventServiceException : Xeption
    {
        public EventServiceException(Exception innerException)
            : base(message: "Event service error occurred, contact support.", innerException) { }
    }
}
