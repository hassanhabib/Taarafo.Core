using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taarafo.Core.Tests.Acceptance.Models.Posts;

namespace Taarafo.Core.Tests.Acceptance.Brokers
{
    public partial class ApiBroker
    {
        private const string PostRelativeUrl = "api/posts";

        public async ValueTask<List<Post>> GetAllPostsAsync() =>
            await this.apiFactoryClient.GetContentAsync<List<Post>>($"{PostRelativeUrl}/");
    }
}
