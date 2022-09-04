// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using Microsoft.Data.SqlClient;
using Moq;
using Taarafo.Core.Brokers.DateTimes;
using Taarafo.Core.Brokers.Loggings;
using Taarafo.Core.Brokers.Storages;
using Taarafo.Core.Models.Groups;
using Taarafo.Core.Services.Foundations.Groups;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace Taarafo.Core.Tests.Unit.Services.Foundations.Groups
{
    public partial class GroupServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly IGroupService groupService;

        public GroupServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.groupService = new GroupService(
                storageBroker: this.storageBrokerMock.Object,
                dateTimeBroker: this.dateTimeBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private static Group CreateRandomGroup() =>
            CreateGroupFiller(dates: GetRandomDateTime()).Create();

        private static Group CreateRandomGroup(DateTimeOffset dates) =>
            CreateGroupFiller(dates: dates).Create();

        private static IQueryable<Group> CreateRandomGroups()
        {
            return CreateGroupFiller(dates: GetRandomDateTimeOffset())
                .Create(count: GetRandomNumber())
                    .AsQueryable();
        }

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static int GetRandomNegativeNumber() =>
            -1 * new IntRange(min: 2, max: 10).GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static string GetRandomMessage() =>
            new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        private static SqlException GetSqlException() =>
            (SqlException)FormatterServices.GetUninitializedObject(typeof(SqlException));

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

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

        private static Filler<Group> CreateGroupFiller(DateTimeOffset dates)
        {
            var filler = new Filler<Group>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dates);

            return filler;
        }
    }
}
