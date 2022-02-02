// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Microsoft.Data.SqlClient;
using Moq;
using Taarafo.Core.Models.Posts.Exceptions;
using Taarafo.Core.Models.Profiles;
using Taarafo.Core.Models.Profiles.Exceptions;
using Xunit;

namespace Taarafo.Core.Tests.Unit.Services.Foundations.Profiles
{
    public partial class ProfileServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnAddIfSqlErrorOccursAndLogItAsync()
        {
            // given
            Profile randomProfile = CreateRandomProfile();
            SqlException sqlException = GetSqlException();

            var failedProfileStorageException =
                new FailedProfileStorageException(sqlException);

            var expectedProfileDependencyException =
                            new ProfileDependencyException(failedProfileStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertProfileAsync(randomProfile))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<Profile> addProfileTask =
                this.profileService.AddProfileAsync(randomProfile);

            // then
            await Assert.ThrowsAsync<ProfileDependencyException>(() =>
                addProfileTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedProfileDependencyException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
