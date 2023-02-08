// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Taarafo.Core.Models.PostImpressions;
using Taarafo.Core.Models.PostImpressions.Exceptions;
using Taarafo.Core.Models.Processings.PostImpressions.Exceptions;
using Xeptions;

namespace Taarafo.Core.Services.Processings.PostImpressions
{
    public partial class PostImpressionProcessingService
    {
        private delegate ValueTask<PostImpression> ReturningPostImpressionFunction();
        private delegate IQueryable<PostImpression> ReturningPostImpressionsFunction();

        private async ValueTask<PostImpression> TryCatch(ReturningPostImpressionFunction returningPostImpressionFunction)
        {
            try
            {
                return await returningPostImpressionFunction();
            }
            catch (NullPostImpressionProcessingException nullPostImpressionProcessingException)
            {
                throw CreateAndLogValidationException(nullPostImpressionProcessingException);
            }
            catch (InvalidPostImpressionProcessingException invalidPostImpressionProcessingException)
            {
                throw CreateAndLogValidationException(invalidPostImpressionProcessingException);
            }
        }

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
            catch (PostImpressionServiceException postImpressionServiceException)
            {
                throw CreateAndLogDependencyException(postImpressionServiceException);
            }
            catch (Exception exception)
            {
                var failedPostImpressionProcessingServiceException =
                    new FailedPostImpressionProcessingServiceException(exception);

                throw CreateAndLogServiceException(failedPostImpressionProcessingServiceException);
            }
        }

        private PostImpressionProcessingValidationException CreateAndLogValidationException(Xeption exception)
        {
            var postImpressionProcessingValidationException = 
                new PostImpressionProcessingValidationException(exception);

            this.loggingBroker.LogError(postImpressionProcessingValidationException);

            return postImpressionProcessingValidationException;
        }

        private PostImpressionProcessingDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var postImpressionProcessingDependencyException =
                new PostImpressionProcessingDependencyException(exception.InnerException as Xeption);

            this.loggingBroker.LogError(postImpressionProcessingDependencyException);

            return postImpressionProcessingDependencyException;
        }

        private PostImpressionProcessingServiceException CreateAndLogServiceException(Xeption exception)
        {
            var postImpressionProcessingServiceException =
                new PostImpressionProcessingServiceException(exception);

            this.loggingBroker.LogError(postImpressionProcessingServiceException);

            return postImpressionProcessingServiceException;
        }
    }
}
