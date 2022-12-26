// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Taarafo.Core.Models.PostImpressions.Exceptions
{
    public class LockedPostImpressionException : Xeption
    {
        public LockedPostImpressionException(Exception innerException)
        : base(message: "Locked post impression record exception, please try again later", innerException)
        { }
    }
}
