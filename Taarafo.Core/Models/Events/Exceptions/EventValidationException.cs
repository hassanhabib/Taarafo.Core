// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Taarafo.Core.Models.Events.Exceptions
{
    public class EventValidationException : Xeption
    {
        public EventValidationException(Xeption innerException)
            : base(message: "Event validation error occurred, please try again.",
                  innerException)
        { }
    }
}
