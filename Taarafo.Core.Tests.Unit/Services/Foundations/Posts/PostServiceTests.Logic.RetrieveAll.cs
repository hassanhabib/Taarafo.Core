// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Linq;
using FluentAssertions;
using Moq;
using Taarafo.Core.Models.Posts;
using Xunit;

namespace Taarafo.Core.Tests.Unit.Services.Foundations.Posts
{
	public partial class PostServiceTests
	{
		[Fact]
		public void ShouldReturnPosts()
		{
			// given
			IQueryable<Post> randomPosts = CreateRandomPosts();
			IQueryable<Post> storagePosts = randomPosts;
			IQueryable<Post> expectedPosts = storagePosts;

			this.storageBrokerMock.Setup(broker =>
				broker.SelectAllPosts())
					.Returns(storagePosts);

			// when
			IQueryable<Post> actualPosts =
				this.postService.RetrieveAllPosts();

			// then
			actualPosts.Should().BeEquivalentTo(expectedPosts);

			this.storageBrokerMock.Verify(broker =>
				broker.SelectAllPosts(),
					Times.Once);

			this.storageBrokerMock.VerifyNoOtherCalls();
			this.loggingBrokerMock.VerifyNoOtherCalls();
		}
	}
}
