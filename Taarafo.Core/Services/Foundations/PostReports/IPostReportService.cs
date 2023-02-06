// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using Taarafo.Core.Models.PostReports;

namespace Taarafo.Core.Services.Foundations.PostReports
{
    public interface IPostReportService
    {
        ValueTask<PostReport> AddPostReportAsync(PostReport postReport);
        IQueryable<PostReport> RetrieveAllPostReports();
    }
}
