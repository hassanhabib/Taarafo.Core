// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Taarafo.Core.Models.PostImpressions;
using Xunit;

namespace Taarafo.Core.Tests.Unit.Services.Processings.PostImpressions
{
    public partial class PostImpressionProcessingServiceTests
    {
        [Fact]
        public async Task ShouldAddPostImpressionIfNotExistAsync()
        {
            // given
            IQueryable<PostImpression> randomPostImpressions =
                CreateRandomPostImpressions();

            IQueryable<PostImpression> retrievedPostImpressions =
                randomPostImpressions;

            PostImpression randomPostImpression = CreateRandomPostImpression();
            PostImpression inputPostImpression = randomPostImpression;
            PostImpression addedPostImpression = inputPostImpression;
            PostImpression expectedPostImpression = addedPostImpression.DeepClone();

            this.postImpressionServiceMock.Setup(service =>
                service.RetrieveAllPostImpressions())
                    .Returns(retrievedPostImpressions);

            this.postImpressionServiceMock.Setup(service =>
                service.AddPostImpressions(inputPostImpression))
                    .ReturnsAsync(addedPostImpression);

            // when
            PostImpression actualPostImpression = await this.postImpressionProcessingService
                 .UpsertPostImpressionAsync(inputPostImpression);

            // then
            actualPostImpression.Should().BeEquivalentTo(expectedPostImpression);

            this.postImpressionServiceMock.Verify(service =>
                service.RetrieveAllPostImpressions(),
                    Times.Once);

            this.postImpressionServiceMock.Verify(service =>
                service.AddPostImpressions(inputPostImpression),
                    Times.Once);

            this.postImpressionServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}