// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Taarafo.Core.Models.Events.Exceptions
{
    public class AlreadyExistsEventException : Xeption
    {
        public AlreadyExistsEventException(Exception innerException)
            : base(message:"Event with the same id already exists.", innerException)
        { }
        
    }
}
