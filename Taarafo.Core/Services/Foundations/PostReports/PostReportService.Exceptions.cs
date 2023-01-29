// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Threading.Tasks;
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
        }

        private PostReportValidationException CreateAndLogValidationException(Xeption exception)
        {
            var postReportValidationException =
                new PostReportValidationException(exception);

            this.loggingBroker.LogError(postReportValidationException);

            return postReportValidationException;
        }
    }
}
