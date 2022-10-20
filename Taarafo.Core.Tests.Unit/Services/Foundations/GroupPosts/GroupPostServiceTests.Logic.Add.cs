// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Taarafo.Core.Models.GroupPosts;
using Xunit;

namespace Taarafo.Core.Tests.Unit.Services.Foundations.GroupPosts
{
    public partial class GroupPostServiceTests
    {
        [Fact]
        public async Task ShouldAddGroupPostAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTimeOffset();
            GroupPost randomGroupPost = CreateRandomGroupPost(dateTime);
            GroupPost inputGroupPost = randomGroupPost;
            GroupPost storageGroupPost = inputGroupPost;
            GroupPost expectedGroupPost = storageGroupPost.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.InsertGroupPostAsync(inputGroupPost))
                    .ReturnsAsync(storageGroupPost);

            // when
            GroupPost actualGroupPost =
                await this.groupPostService.AddGroupPostAsync(inputGroupPost);

            // then
            actualGroupPost.Should().BeEquivalentTo(expectedGroupPost);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertGroupPostAsync(inputGroupPost),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
