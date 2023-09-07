﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Taarafo.Core.Models.Events.Exceptions
{
    public class NullEventException : Xeption
    {
        public NullEventException()
            : base(message: "Event is null.")
        { }

        public NullEventException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}