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
            //given
            IQueryable<PostImpression> randomPostImpressions = CreateRandomPostImpressions();
            IQueryable<PostImpression> retrievedPostImpression = randomPostImpressions;

            PostImpression randomPostImpression = CreateRandomPostImpression();
            PostImpression inputPostImpression = randomPostImpression;
            PostImpression addedPostImpression = inputPostImpression;
            PostImpression expectedPostImpression = addedPostImpression.DeepClone();

            this.postImpressionServiceMock.Setup(service =>
                service.RetrieveAllPostImpressions()).Returns(retrievedPostImpression);

            this.postImpressionServiceMock.Setup(service =>
                service.AddPostImpressions(inputPostImpression))
                    .ReturnsAsync(addedPostImpression);

            //when
            PostImpression actualPostImpression = await this.postImpressionProcessingService
                .UpsertPostImpressionAsync(inputPostImpression);

            //then
            actualPostImpression.Should().BeEquivalentTo(expectedPostImpression);

            this.postImpressionServiceMock.Verify(service =>
                service.RetrieveAllPostImpressions(), Times.Once);

            this.postImpressionServiceMock.Verify(service =>
                service.AddPostImpressions(inputPostImpression), Times.Once);

            this.postImpressionServiceMock.Verify(service =>
                service.ModifyPostImpressionAsync(It.IsAny<PostImpression>()), Times.Never);

            this.postImpressionServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldModifyPostImpressionIfCountryExistAsync()
        {
            //given
            PostImpression randomPostImpression = CreateRandomPostImpression();
            PostImpression inputPostImpression = randomPostImpression;
            PostImpression modifiedPostImpression = inputPostImpression;
            PostImpression expectedPostImpression = modifiedPostImpression.DeepClone();

            IQueryable<PostImpression> randomPostImpressions =
                CreateRandomPostImpressions(inputPostImpression);

            IQueryable<PostImpression> retrievePostImpression = randomPostImpressions;

            this.postImpressionServiceMock.Setup(service =>
                service.RetrieveAllPostImpressions()).Returns(retrievePostImpression);

            this.postImpressionServiceMock.Setup(service =>
                service.ModifyPostImpressionAsync(modifiedPostImpression))
                    .ReturnsAsync(inputPostImpression);

            //when
            PostImpression actualPostImpression = await this.postImpressionProcessingService
                    .UpsertPostImpressionAsync(inputPostImpression);

            //then
            actualPostImpression.Should().BeEquivalentTo(expectedPostImpression);

            this.postImpressionServiceMock.Verify(service =>
                service.RetrieveAllPostImpressions(), Times.Once);

            this.postImpressionServiceMock.Verify(service =>
                service.ModifyPostImpressionAsync(modifiedPostImpression), Times.Once);

            this.postImpressionServiceMock.Verify(service =>
                service.AddPostImpressions(It.IsAny<PostImpression>()), Times.Never);

            this.postImpressionServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
