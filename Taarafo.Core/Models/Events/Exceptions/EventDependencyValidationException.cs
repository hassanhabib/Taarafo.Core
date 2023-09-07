// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Taarafo.Core.Models.Events.Exceptions
{
    public class EventDependencyValidationException : Xeption
    {
        public EventDependencyValidationException(Xeption innerException)
            : base(
                message: "Event dependency validation occurred, please try again.",
                innerException: innerException)
        { }

        public EventDependencyValidationException(string message, Xeption innerException)
            : base(message, innerException)
        { }
    }
}