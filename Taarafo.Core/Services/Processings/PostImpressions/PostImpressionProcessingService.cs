// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Taarafo.Core.Brokers.Loggings;
using Taarafo.Core.Models.PostImpressions;
using Taarafo.Core.Services.Foundations.PostImpressions;

namespace Taarafo.Core.Services.Processings.PostImpressions
{
    public partial class PostImpressionProcessingService : IPostImpressionProcessingService
    {
        private readonly IPostImpressionService postImpressionService;
        private readonly ILoggingBroker loggingBroker;

        public PostImpressionProcessingService(
            IPostImpressionService postImpressionService,
            ILoggingBroker loggingBroker)
        {
            this.postImpressionService = postImpressionService;
            this.loggingBroker = loggingBroker;
        }

        public async ValueTask<PostImpression> UpsertPostImpressionAsync(PostImpression postImpression)
        {
            PostImpression maybePostImpression = RetrieveMatchingPostImpression(postImpression);

            return maybePostImpression switch
            {
                null => await this.postImpressionService.AddPostImpressions(postImpression),
                _ => await this.postImpressionService.ModifyPostImpressionAsync(postImpression)
            };
        }

        private PostImpression RetrieveMatchingPostImpression(PostImpression postImpression)
        {
            IQueryable<PostImpression> postImpressions =
                this.postImpressionService.RetrieveAllPostImpressions();

            return postImpressions.FirstOrDefault(SamePostImpressionAs(postImpression));
        }

        private static Expression<Func<PostImpression, bool>> SamePostImpressionAs(PostImpression postImpression) =>
            retrievePostImpression => (retrievePostImpression.PostId == postImpression.PostId)
                || (retrievePostImpression.ProfileId == postImpression.ProfileId);

        public IQueryable<PostImpression> RetrieveAllPostImpressions() =>
            TryCatch(() => this.postImpressionService.RetrieveAllPostImpressions());
    }
}
