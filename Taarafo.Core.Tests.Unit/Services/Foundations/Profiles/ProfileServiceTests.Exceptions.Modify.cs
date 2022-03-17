// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Moq;
using Taarafo.Core.Models.Profiles;
using Taarafo.Core.Models.Profiles.Exceptions;
using Xunit;

namespace Taarafo.Core.Tests.Unit.Services.Foundations.Profiles
{
    public partial class ProfileServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnModifyIfSqlErrorOccursAndLogItAsync()
        {
            // given
            Profile randomProfile = CreateRandomProfile();
            SqlException sqlException = GetSqlException();

            var failedProfileStorageException =
                new FailedProfileStorageException(sqlException);

            var expectedProfileDependencyException =
                new ProfileDependencyException(failedProfileStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(sqlException);

            // when
            ValueTask<Profile> addProfileTask =
                this.profileService.ModifyProfileAsync(randomProfile);

            // then
            await Assert.ThrowsAsync<ProfileDependencyException>(() =>
               addProfileTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProfileByIdAsync(randomProfile.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedProfileDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateProfileAsync(randomProfile),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
