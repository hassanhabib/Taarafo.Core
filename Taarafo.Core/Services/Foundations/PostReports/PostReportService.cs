// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Taarafo.Core.Brokers.DateTimes;
using Taarafo.Core.Brokers.Loggings;
using Taarafo.Core.Brokers.Storages;
using Taarafo.Core.Models.PostReports;

namespace Taarafo.Core.Services.Foundations.PostReports
{
    public partial class PostReportService : IPostReportService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public PostReportService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<PostReport> AddPostReportAsync(PostReport postReport) =>
            TryCatch(async () =>
            {
                ValidatePostReport(postReport);

                return await this.storageBroker.InsertPostReportAsync(postReport);
            });

        public async ValueTask<PostReport> RetrievePostReportByIdAsync(Guid postReportId) =>
            await this.storageBroker.SelectPostReportByIdAsync(postReportId);

        public IQueryable<PostReport> RetrieveAllPostReports() =>
            TryCatch(() => this.storageBroker.SelectAllPostReports());
    }
}
