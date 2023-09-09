// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using Taarafo.Core.Models.GroupPosts;
using Taarafo.Core.Models.GroupPosts.Exceptions;
using Xunit;

namespace Taarafo.Core.Tests.Unit.Services.Foundations.GroupPosts
{
    public partial class GroupPostServiceTests
    {
        [Fact]
        private async Task ShouldThrowCriticalDependencyExceptionOnAddIfSqlErrorOccursAndLogItAsync()
        {
            // given
            GroupPost someGroupPost = CreateRandomGroupPost();
            SqlException sqlException = GetSqlException();

            var failedGroupPostStorageException =
                new FailedGroupPostStorageException(
                    message: "Failed group post storage error occured, contact support.",
                    innerException: sqlException);

            var expectedGroupPostDependencyException =
                new GroupPostDependencyException(
                    message: "Group post dependency validation occurred, please try again.",
                    innerException: failedGroupPostStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertGroupPostAsync(It.IsAny<GroupPost>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<GroupPost> addGroupPostTask =
                this.groupPostService.AddGroupPostAsync(someGroupPost);

            // then
            await Assert.ThrowsAsync<GroupPostDependencyException>(() =>
                addGroupPostTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.InsertGroupPostAsync(It.IsAny<GroupPost>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedGroupPostDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        private async Task ShouldThrowDependencyValidationExceptionOnAddIfGroupPostAlreadyExistsAndLogItAsync()
        {
            // given
            GroupPost someGroupPost = CreateRandomGroupPost();
            string randomMessage = GetRandomMessage();

            var duplicateKeyException =
                new DuplicateKeyException(randomMessage);

            var alreadyExistsGroupPostException =
                new AlreadyExistsGroupPostException(
                    message: "Group post with the same id already exists.",
                    innerException: duplicateKeyException);

            var expectedGroupPostDependencyValidationException =
                new GroupPostDependencyValidationException(
                     message: "Group post dependency validation occurred, please try again.",
                    innerException: alreadyExistsGroupPostException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertGroupPostAsync(It.IsAny<GroupPost>()))
                    .ThrowsAsync(duplicateKeyException);

            // when
            ValueTask<GroupPost> addGroupPostTask =
                this.groupPostService.AddGroupPostAsync(someGroupPost);

            GroupPostDependencyValidationException actualGroupPostDependencyValidationException =
                await Assert.ThrowsAsync<GroupPostDependencyValidationException>(
                    addGroupPostTask.AsTask);

            // then
            actualGroupPostDependencyValidationException.Should().BeEquivalentTo(
                expectedGroupPostDependencyValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertGroupPostAsync(someGroupPost),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGroupPostDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        private async Task ShouldThrowDependencyExceptionOnAddIfDatabaseUpdateErrorOccursAndLogItAsync()
        {
            //given
            GroupPost someGroupPost = CreateRandomGroupPost();

            var databaseUpdateException =
                new DbUpdateException();

            var failedGroupPostStorageException =
                new FailedGroupPostStorageException(
                    message: "Failed group post storage error occured, contact support.",
                    innerException: databaseUpdateException);

            var expectedGroupPostDependencyException =
                new GroupPostDependencyException(
                    message: "Group post dependency validation occurred, please try again.",
                    innerException: failedGroupPostStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertGroupPostAsync(someGroupPost))
                    .Throws(databaseUpdateException);

            //when
            ValueTask<GroupPost> addGroupPostTask =
                this.groupPostService.AddGroupPostAsync(someGroupPost);

            GroupPostDependencyException actualGroupPostDependencyException =
                await Assert.ThrowsAsync<GroupPostDependencyException>(
                    addGroupPostTask.AsTask);

            //then
            actualGroupPostDependencyException.Should().BeEquivalentTo(
                expectedGroupPostDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertGroupPostAsync(It.IsAny<GroupPost>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGroupPostDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        private async void ShouldThrowDependencyValidationExceptionOnAddIfReferenceErrorOccursAndLogItAsync()
        {
            //given
            GroupPost someGroupPost = CreateRandomGroupPost();
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;

            var foreignKeyConstraintConflictException =
                new ForeignKeyConstraintConflictException(exceptionMessage);

            var invalidGroupPostReferenceException =
                new InvalidGroupPostReferenceException(
                    message: "Invalid group post reference error occurred.",
                    innerException: foreignKeyConstraintConflictException);

            var expectedGroupPostDependencyValidationException =
                new GroupPostDependencyValidationException(
                     message: "Group post dependency validation occurred, please try again.",
                    innerException: invalidGroupPostReferenceException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertGroupPostAsync(someGroupPost))
                    .Throws(foreignKeyConstraintConflictException);

            //when
            ValueTask<GroupPost> addGroupPostTask =
                this.groupPostService.AddGroupPostAsync(someGroupPost);

            GroupPostDependencyValidationException actualGroupPostDependencyValidationException =
                await Assert.ThrowsAsync<GroupPostDependencyValidationException>(
                    addGroupPostTask.AsTask);

            //then
            actualGroupPostDependencyValidationException.Should().BeEquivalentTo(
                expectedGroupPostDependencyValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertGroupPostAsync(It.IsAny<GroupPost>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGroupPostDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        private async Task ShouldThrowServiceExceptionOnAddIfServiceErrorOccursAndLogItAsync()
        {
            //given
            GroupPost someGroupPost = CreateRandomGroupPost();
            var serviceException = new Exception();

            var failedGroupPostServiceException =
                new FailedGroupPostServiceException(
                    message: "Failed group post service occurred, please contact support.",
                    innerException: serviceException);

            var expectedGroupPostServiceException =
                new GroupPostServiceException(
                     message: "Group post service error occurred, please contact support.",
                    innerException: failedGroupPostServiceException);

            this.storageBrokerMock.Setup(broker =>
               broker.InsertGroupPostAsync(someGroupPost))
                   .Throws(serviceException);

            //when
            ValueTask<GroupPost> addGroupPostTask =
                this.groupPostService.AddGroupPostAsync(someGroupPost);

            GroupPostServiceException actualGroupPostServiceException =
                await Assert.ThrowsAsync<GroupPostServiceException>(
                    addGroupPostTask.AsTask);

            //then
            actualGroupPostServiceException.Should().BeEquivalentTo(
                expectedGroupPostServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertGroupPostAsync(It.IsAny<GroupPost>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGroupPostServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}