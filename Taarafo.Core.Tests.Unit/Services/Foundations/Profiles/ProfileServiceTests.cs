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
using Xunit;
using Taarafo.Core.Brokers.DateTimes;
using System.Linq;

namespace Taarafo.Core.Tests.Unit.Services.Foundations.Profiles
{
    public partial class ProfileServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly IProfileService profileService;

        public ProfileServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();

            this.profileService = new ProfileService(
                storageBroker: this.storageBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object,
                dateTimeBroker: this.dateTimeBrokerMock.Object);
        }

        private static IQueryable<Profile> CreateRandomProfiles() =>
            CreateProfileFiller(dates: GetRandomDateTime())
                .Create(count:GetRandomNumber())
                    .AsQueryable();

        private static Profile CreateRandomProfile() =>
            CreateProfileFiller(dates: GetRandomDateTime()).Create();

        private static Profile CreateRandomProfile(DateTimeOffset dates) =>
            CreateProfileFiller(dates: dates).Create();

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static SqlException GetSqlException() =>
           (SqlException)FormatterServices.GetUninitializedObject(typeof(SqlException));

        private static int GetRandomNumber() =>
           new IntRange(min: 2, max: 10).GetValue();

        private static int GetRandomNegativeNumber() => 
            -1 * new IntRange(min: 2, max: 10).GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static string GetRandomMessage() =>
            new MnemonicString(wordCount: GetRandomNumber()).GetValue();
        public static TheoryData MinutesBeforeOrAfter()
        {
            int randomNumber = GetRandomNumber();
            int randomNegativeNumber = GetRandomNegativeNumber();

            return new TheoryData<int>
            {
                randomNumber,
                randomNegativeNumber
            };
        }

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expedtedException)
        {
            return actualException =>
                actualException.Message == expedtedException.Message
                && actualException.InnerException.Message == expedtedException.InnerException.Message
                && (actualException.InnerException as Xeption).DataEquals(expedtedException.InnerException.Data);
        }

        private static Filler<Profile> CreateProfileFiller(DateTimeOffset dates)
        {
            var filler = new Filler<Profile>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dates);

            return filler;
        }
    }
}
