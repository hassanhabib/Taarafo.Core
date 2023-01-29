// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Taarafo.Core.Models.PostReports;
using Taarafo.Core.Models.PostReports.Exceptions;

namespace Taarafo.Core.Services.Foundations.PostReports
{
    public partial class PostReportService
    {
        private static void ValidatePostReportNotNull(PostReport postReport)
        {
            if (postReport is null)
            {
                throw new NullPostReportException();
            }
        }
    }
}
