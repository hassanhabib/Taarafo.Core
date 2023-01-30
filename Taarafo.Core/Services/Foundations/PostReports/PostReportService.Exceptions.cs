// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Taarafo.Core.Models.PostReports;
using Taarafo.Core.Models.PostReports.Exceptions;
using Xeptions;

namespace Taarafo.Core.Services.Foundations.PostReports
{
    public partial class PostReportService
    {
        private delegate ValueTask<PostReport> ReturningPostReportFunction();

        private async ValueTask<PostReport> TryCatch(ReturningPostReportFunction returningPostReportFunction)
        {
            try
            {
                return await returningPostReportFunction();
            }
            catch (NullPostReportException nullPostReportException)
            {
                throw CreateAndLogValidationException(nullPostReportException);
            }
            catch (InvalidPostReportException invalidPostReportException)
            {
                throw CreateAndLogValidationException(invalidPostReportException);
            }
            catch (SqlException sqlException)
            {
                var failedPostReportStorageException = new FailedPostReportStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedPostReportStorageException);
            }
        }

        private PostReportValidationException CreateAndLogValidationException(Xeption exception)
        {
            var postReportValidationException =
                new PostReportValidationException(exception);

            this.loggingBroker.LogError(postReportValidationException);

            return postReportValidationException;
        }

        private PostReportDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var postReportDependencyException = new PostReportDependencyException(exception);
            this.loggingBroker.LogCritical(postReportDependencyException);

            return postReportDependencyException;
        }
    }
}
