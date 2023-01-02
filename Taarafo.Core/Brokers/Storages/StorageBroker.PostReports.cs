// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Taarafo.Core.Models.PostReports;

namespace Taarafo.Core.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<PostReport> PostReports { get; set; }

        public async ValueTask<PostReport> SelectPostReportByIdAsync(Guid postReportId, Guid PostId, Guid ReporterId) =>
            await SelectAsync<PostReport>(postReportId,PostId,ReporterId);
    }
}
