// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Taarafo.Core.Models.Events.Exceptions
{
    public class EventDependencyException : Xeption
    {
        public EventDependencyException(Xeption innerException)
            : base(message: "Event dependency validation occurred, please try again.", innerException)
        { }
    }
}
