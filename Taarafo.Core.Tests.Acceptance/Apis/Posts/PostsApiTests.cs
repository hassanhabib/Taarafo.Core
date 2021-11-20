// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
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

        public PostsApiTests(ApiBroker apiBroker) =>
            this.apiBroker = apiBroker;

        private static Post CreateRandomPost() =>
            CreateRandomPostFiller().Create();

        private static Filler<Post> CreateRandomPostFiller()
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            var filler = new Filler<Post>();

            filler.Setup()
                .OnProperty(post => post.CreatedDate).Use(now)
                .OnProperty(post => post.UpdatedDate).Use(now);

            return filler;
        }
    }
}
