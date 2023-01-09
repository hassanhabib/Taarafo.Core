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

        public ValueTask<PostImpression> RetrievePostImpressionByIdAsync(Guid postId, Guid profileId) =>
            TryCatch(async () =>
            {
                ValidatePostImpressionId(postId, profileId);

                PostImpression maybePostImpression =
                    await this.storageBroker.SelectPostImpressionByIdAsync(postId, profileId);

                ValidateStoragePostImpression(maybePostImpression, postId, profileId);

                return maybePostImpression;
            });

        public ValueTask<PostImpression> ModifyPostImpressionAsync(PostImpression postImpression) =>
            TryCatch(async () =>
            {
                ValidatePostImpressionOnModify(postImpression);

                var maybePostImpression =await this.storageBroker.SelectPostImpressionByIdAsync(
                    postImpression.PostId, postImpression.ProfileId);

                ValidateAginstStoragePostImpressionOnModify( postImpression, maybePostImpression);

                return await this.storageBroker.UpdatePostImpressionAsync(postImpression);
            });

        public ValueTask<PostImpression> RemovePostImpressionAsync(PostImpression postImpression) =>
            TryCatch(async () =>
            {
                ValidatePostImpressionOnRemove(postImpression);

                PostImpression somePostImpression =
                    await this.storageBroker.SelectPostImpressionByIdAsync(postImpression.PostId, postImpression.ProfileId);

                ValidateStoragePostImpression(somePostImpression, postImpression.PostId, postImpression.ProfileId);

                return await this.storageBroker.DeletePostImpressionAsync(somePostImpression);
            });
    }
}