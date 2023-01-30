// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Linq;
using Microsoft.Data.SqlClient;
using Taarafo.Core.Models.PostReports;
using Taarafo.Core.Models.PostReports.Exceptions;
using Xeptions;

namespace Taarafo.Core.Services.Foundations.PostReports
{
    public partial class PostReportService
    {
        private delegate IQueryable<PostReport> ReturningPostReportsFunction();

        private IQueryable<PostReport> TryCatch(ReturningPostReportsFunction returningPostReportsFunction)
        {
            try
            {
                return returningPostReportsFunction();
            }
            catch (SqlException sqlException)
            {
                var failedPostPeportStorageException =
                    new FailedPostPeportStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedPostPeportStorageException);
            }
        }

        private PostReportDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var postReportDependencyException =
                new PostReportDependencyException(exception);

            this.loggingBroker.LogCritical(postReportDependencyException);

            return postReportDependencyException;
        }
    }
}
