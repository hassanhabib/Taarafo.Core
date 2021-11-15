// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------
using System;
using Xeptions;

namespace Taarafo.Core.Models.Comments.Exceptions
{
    public class InvalidCommentReferenceException : Xeption
    {
        public InvalidCommentReferenceException(Exception innerException)
            : base(message: "Invalid comment reference error occurred.", innerException) { }
    }
}