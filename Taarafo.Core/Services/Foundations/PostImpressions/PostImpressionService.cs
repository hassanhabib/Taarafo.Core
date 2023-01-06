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

        public ValueTask<PostImpression> ModifyPostImpressionAsync(PostImpression postImpression)=>
            throw new NotImplementedException();

        public ValueTask<PostImpression> RemovePostImpressionAsync(PostImpression postImpression) =>
            TryCatch(async () =>
            {
                ValidatePostImpressionOnRemove(postImpression);

                PostImpression somePostImpression =
                    await this.storageBroker.SelectPostImpressionByIdsAsync(postImpression.PostId, postImpression.ProfileId);

                ValidateStoragePostImpression(somePostImpression,postImpression.PostId, postImpression.ProfileId);

                return await this.storageBroker.DeletePostImpressionAsync(somePostImpression);
            });
    }
}
