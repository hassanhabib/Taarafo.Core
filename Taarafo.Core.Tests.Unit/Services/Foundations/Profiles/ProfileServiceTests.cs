// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Linq.Expressions;
using Microsoft.Data.SqlClient;
using System.Runtime.Serialization;
using Moq;
using Taarafo.Core.Brokers.Loggings;
using Taarafo.Core.Brokers.Storages;
using Taarafo.Core.Models.Profiles;
using Taarafo.Core.Services.Foundations.Profiles;
using Tynamix.ObjectFiller;
using Xeptions;

namespace Taarafo.Core.Tests.Unit.Services.Foundations.Profiles
{
    public partial class ProfileServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IProfileService profileService;

        public ProfileServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.profileService = new ProfileService(
                storageBroker: this.storageBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private static Profile CreateRandomProfile() =>
            CreateProfileFiller().Create();

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static SqlException GetSqlException() =>
           (SqlException)FormatterServices.GetUninitializedObject(typeof(SqlException));

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expedtedException)
        {
            return actualException =>
                actualException.Message == expedtedException.Message
                && actualException.InnerException.Message == expedtedException.InnerException.Message
                && (actualException.InnerException as Xeption).DataEquals(expedtedException.InnerException.Data);
        }

        private static Filler<Profile> CreateProfileFiller()
        {
            var filler = new Filler<Profile>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(GetRandomDateTime());

            return filler;
        }
    }
}
