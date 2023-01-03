// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Threading.Tasks;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Taarafo.Core.Models.PostReports;

namespace Taarafo.Core.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<PostReport> PostReports { get; set; }

        public async ValueTask<PostReport> InsertPostReportAsync(PostReport postReport) =>
            await InsertAsync(postReport);

        public IQueryable<PostReport> SelectAllPostReports() =>
            SelectAll<PostReport>();

        public async ValueTask<PostReport> UpdatePostReportAsync(PostReport postReport) =>
            await UpdateAsync(postReport);

        public async ValueTask<PostReport> DeletePostReportAsync(PostReport postReport)=>
            await DeleteAsync(postReport);
    }
}