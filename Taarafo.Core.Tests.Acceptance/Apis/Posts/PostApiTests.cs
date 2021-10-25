using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taarafo.Core.Tests.Acceptance.Brokers;
using Taarafo.Core.Tests.Acceptance.Models.Posts;
using Tynamix.ObjectFiller;
using Xunit;

namespace Taarafo.Core.Tests.Acceptance.Apis.Posts
{
    [Collection(nameof(ApiTestCollection))]
    public partial class PostApiTests
    {
        private readonly ApiBroker apiBroker;
        public PostApiTests(ApiBroker apiBroker)
        {
            this.apiBroker = apiBroker;
        }

        private static int GetRandomNumber() => new IntRange(min: 2, max: 10).GetValue();

        private static IEnumerable<Post> CreateRandomPosts()
        {
            return CreatePostFiller(dates: GetRandomDateTimeOffset())
                .Create(count: GetRandomNumber());                 
        }

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static Filler<Post> CreatePostFiller(DateTimeOffset dates)
        {
            var filler = new Filler<Post>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dates);
            
            return filler;

        }
    }
}
