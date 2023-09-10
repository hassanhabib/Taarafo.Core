// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Taarafo.Core.Models.PostImpressions;
using Xunit;

namespace Taarafo.Core.Tests.Unit.Services.Foundations.PostImpressions
{
	public partial class PostImpressionServiceTests
	{
		[Fact]
		private async Task ShouldAddPostImpressionAsync()
		{
			// given
			DateTimeOffset randomDateTime =
				GetRandomDateTimeOffset();

			PostImpression randomPostImpression =
				CreateRandomPostImpression(randomDateTime);

			PostImpression inputPostImpression = randomPostImpression;
			PostImpression storagePostImpression = inputPostImpression;

			PostImpression expectedPostImpression =
				storagePostImpression.DeepClone();

			this.dateTimeBrokerMock.Setup(broker =>
				broker.GetCurrentDateTimeOffset())
					.Returns(randomDateTime);

			this.storageBrokerMock.Setup(broker =>
				broker.InsertPostImpressionAsync(inputPostImpression))
					.ReturnsAsync(storagePostImpression);

			// when
			PostImpression actualPostImpression =
				await this.postImpressionService
				.AddPostImpressions(inputPostImpression);

			// then
			actualPostImpression.Should().BeEquivalentTo(
				expectedPostImpression);

			this.dateTimeBrokerMock.Verify(broker =>
				broker.GetCurrentDateTimeOffset(),
					Times.Once);

			this.storageBrokerMock.Verify(broker =>
				broker.InsertPostImpressionAsync(inputPostImpression),
					Times.Once);

			this.dateTimeBrokerMock.VerifyNoOtherCalls();
			this.storageBrokerMock.VerifyNoOtherCalls();
			this.loggingBrokerMock.VerifyNoOtherCalls();
		}
	}
}