// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Taarafo.Core.Models.Events.Exceptions
{
    public class InvalidEventException : Xeption
    {
        public InvalidEventException()
            : base(message: "Invalid event. Please correct the errors and try again.")
        { }
    }
}
