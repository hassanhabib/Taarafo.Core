// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Taarafo.Core.Models.PostReports;

namespace Taarafo.Core.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<PostReport> PostReports { get; set; }

        public IQueryable<PostReport> SelectAllPostReports() =>
            SelectAll<PostReport>();

        public async ValueTask<PostReport> SelectPostReportByIdAsync(Guid id) =>
            await SelectAsync<PostReport>(id);
    }
}
