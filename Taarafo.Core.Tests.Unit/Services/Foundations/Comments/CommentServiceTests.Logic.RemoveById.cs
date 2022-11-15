// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Taarafo.Core.Models.Comments;
using Xunit;

namespace Taarafo.Core.Tests.Unit.Services.Foundations.Comments
{
	public partial class CommentServiceTests
	{
		[Fact]
		public async Task ShouldRemoveCommentByIdAsync()
		{
			// given
			Guid randomId = Guid.NewGuid();
			Guid inputCommentId = randomId;
			Comment randomComment = CreateRandomComment();
			Comment storageComment = randomComment;
			Comment expectedInputComment = storageComment;
			Comment deletedComment = expectedInputComment;
			Comment expectedComment = deletedComment.DeepClone();

			this.storageBrokerMock.Setup(broker =>
				broker.SelectCommentByIdAsync(inputCommentId))
					.ReturnsAsync(storageComment);

			this.storageBrokerMock.Setup(broker =>
				broker.DeleteCommentAsync(expectedInputComment))
					.ReturnsAsync(deletedComment);

			// when
			Comment actualComment = await this.commentService
				.RemoveCommentByIdAsync(inputCommentId);

			// then
			actualComment.Should().BeEquivalentTo(expectedComment);

			this.storageBrokerMock.Verify(broker =>
				broker.SelectCommentByIdAsync(inputCommentId),
					Times.Once);

			this.storageBrokerMock.Verify(broker =>
				broker.DeleteCommentAsync(expectedInputComment),
					Times.Once);

			this.storageBrokerMock.VerifyNoOtherCalls();
			this.loggingBrokerMock.VerifyNoOtherCalls();
			this.dateTimeBrokerMock.VerifyNoOtherCalls();
		}
	}
}
