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
            :base(message:"Event validation errors occured, please try again.")
        { }
    }
}
