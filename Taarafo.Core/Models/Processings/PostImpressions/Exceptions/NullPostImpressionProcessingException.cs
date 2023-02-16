// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace Taarafo.Core.Models.Processings.PostImpressions.Exceptions
{
    public class NullPostImpressionProcessingException : Xeption
    {
        public NullPostImpressionProcessingException()
            : base(message: "PostImpression is null.")
        { }
    }
}
