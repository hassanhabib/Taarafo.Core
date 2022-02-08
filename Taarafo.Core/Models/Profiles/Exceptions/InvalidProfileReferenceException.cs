// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Taarafo.Core.Models.Profiles.Exceptions
{
    public class InvalidProfileReferenceException : Xeption
    {
        public InvalidProfileReferenceException(Exception innerException)
            : base(message: "Invalid profile reference error occurred.", innerException) 
        { }
    }
}
