// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Linq;
using Taarafo.Core.Models.PostImpressions;
using Taarafo.Core.Models.PostImpressions.Exceptions;
using Taarafo.Core.Models.Processings.PostImpressions.Exceptions;
using Xeptions;

namespace Taarafo.Core.Services.Processings.PostImpressions
{
    public partial class PostImpressionProcessingService
    {
        private delegate IQueryable<PostImpression> ReturningPostImpressionsFunction();

        private IQueryable<PostImpression> TryCatch(ReturningPostImpressionsFunction returningPostImpressionsFunction)
        {
            try
            {
                return returningPostImpressionsFunction();
            }
            catch (PostImpressionDependencyException postImpressionDependencyException)
            {
                throw CreateAndLogDependencyException(postImpressionDependencyException);
            }
            catch(PostImpressionServiceException postImpressionServiceException)
            {
                throw CreateAndLogDependencyException(postImpressionServiceException);
            }
        }

        private PostImpressionProcessingDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var postImpressionProcessingDependencyException = 
                new PostImpressionProcessingDependencyException(exception.InnerException as Xeption);

            this.loggingBroker.LogError(postImpressionProcessingDependencyException);

            return postImpressionProcessingDependencyException;
        }
    }
}
