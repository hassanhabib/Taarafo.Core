using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Taarafo.Core.Tests.Acceptance.Brokers;
using Taarafo.Core.Tests.Acceptance.Models.Posts;
using Tynamix.ObjectFiller;
using Xunit;

namespace Taarafo.Core.Tests.Acceptance.Apis.Posts
{
    [Collection(nameof(ApiTestCollection))]
    public partial class PostsApiTests
    {
        private readonly ApiBroker apiBroker;

        public PostsApiTests(ApiBroker apiBroker)
        {
            this.apiBroker = apiBroker;
        }

        private async ValueTask<Post> PostRandomPostAsync()
        {
            Post randomPost = CreateRandomPost();
            await this.apiBroker.PostPostAsync(randomPost);

            return randomPost;
        }

        private async ValueTask<List<Post>> CreateRandomPostedPostsAsync()
        {
            int randomNumber = GetRandomNumber();
            var randomPosts = new List<Post>();

            for (int i = 0; i < randomNumber; i++)
            {
                randomPosts.Add(await PostRandomPostAsync());
            }
            return randomPosts;
        }

        private Post CreateRandomPost() => CreatePostFiller().Create();

        private int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private Filler<Post> CreatePostFiller()
        {
            var now = DateTimeOffset.UtcNow;
            var filler = new Filler<Post>();

            filler.Setup()
                .OnProperty(post => post.CreatedDate).Use(now)
                .OnProperty(post => post.UpdatedDate).Use(now)
                .OnType<DateTimeOffset>().Use(GetRandomDateTimeOffset());

            return filler;
        }
    }
}
