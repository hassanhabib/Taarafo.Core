// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Taarafo.Core.Models.Groups.Exceptions
{
    public class AlreadyExistsGroupException : Xeption
    {
        public AlreadyExistsGroupException(Exception innerException)
            : base(
                message: "Group with the same id already exists.",
                innerException: innerException)
        { }

        public AlreadyExistsGroupException(string message, Exception innerException)
        : base(message, innerException)
        { }
    }
}