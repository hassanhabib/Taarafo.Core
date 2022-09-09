// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using Taarafo.Core.Brokers.DateTimes;
using Taarafo.Core.Brokers.Loggings;
using Taarafo.Core.Brokers.Storages;
using Taarafo.Core.Models.PostImpressions;

namespace Taarafo.Core.Services.Foundations.PostImpressions
{
    public partial class PostImpressionService : IPostImpressionService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public PostImpressionService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<PostImpression> AddPostImpressions(PostImpression postImpression) =>
        TryCatch(async () =>
        {
            ValidatePostImpressionOnAdd(postImpression);

            return await this.storageBroker.InsertPostImpressionAsync(postImpression);
        });

        public IQueryable<PostImpression> RetrieveAllPostImpressions() =>
            TryCatch(() => this.storageBroker.SelectAllPostImpressions());
    }
}
