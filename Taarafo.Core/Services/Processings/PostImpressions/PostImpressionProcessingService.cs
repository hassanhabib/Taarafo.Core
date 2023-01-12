// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

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

        public ValueTask<PostImpression> UpsertPostImpressionAsync(PostImpression postImpression) =>
             TryCatch(async () =>
             {
                 this.postImpressionService.RetrieveAllPostImpressions();

                 return await this.postImpressionService.AddPostImpressions(postImpression);
             });
    }
}