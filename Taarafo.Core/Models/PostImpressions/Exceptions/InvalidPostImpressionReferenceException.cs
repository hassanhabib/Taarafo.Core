// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Taarafo.Core.Models.PostImpressions.Exceptions
{
    public class InvalidPostImpressionReferenceException : Xeption
    {
        public InvalidPostImpressionReferenceException(Exception innerException)
            : base(message: "Invalid post impression reference error occurred.", innerException)
        { }
    }
}

