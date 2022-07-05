// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Taarafo.Core.Models.PostImpressions.Exceptions
{
    public class NotFoundPostImpressionException : Xeption
    {
        public NotFoundPostImpressionException(Guid postImpressionId)
            : base(message: $"Couldn't find post impression with id: {postImpressionId}.")
        { }
    }
}
