﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Taarafo.Core.Brokers.DateTimes;
using Taarafo.Core.Brokers.Loggings;
using Taarafo.Core.Brokers.Storages;
using Taarafo.Core.Models.GroupPosts;

namespace Taarafo.Core.Services.Foundations.GroupPosts
{
    public partial class GroupPostService : IGroupPostService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBrokerMock;
        private readonly ILoggingBroker loggingBroker;

        public GroupPostService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBrokerMock = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<GroupPost> AddGroupPostAsync(GroupPost groupPost) =>
            TryCatch(async () =>
            {
                ValidateGroupPostOnAdd(groupPost);

                return await this.storageBroker.InsertGroupPostAsync(groupPost);
            });

        public ValueTask<GroupPost> RetrieveGroupPostByIdAsync(Guid groupId, Guid postId) =>
            TryCatch(async () =>
            {
                ValidateGroupPostId(groupId, postId);

                GroupPost maybeGroupPost =
                    await this.storageBroker.SelectGroupPostByIdAsync(groupId, postId);

                ValidateStorageGroupPost(maybeGroupPost, groupId, postId);

                return maybeGroupPost;
            });

        public IQueryable<GroupPost> RetrieveAllGroupPosts() =>
            TryCatch(() => this.storageBroker.SelectAllGroupPosts());

        public ValueTask<GroupPost> ModifyGroupPostAsync(GroupPost groupPost) =>
            TryCatch(async () =>
            {
                ValidateGroupPostOnModify(groupPost);

                var maybeGrouPost =
                    await this.storageBroker.SelectGroupPostByIdAsync(
                        groupPost.GroupId, groupPost.PostId);

                ValidateStorageGroupPostExists(maybeGrouPost, groupPost.GroupId, groupPost.PostId);

                return await this.storageBroker.UpdateGroupPostAsync(groupPost);
            });

        public ValueTask<GroupPost> RemoveGroupPostByIdAsync(Guid groupId, Guid postId) =>
            TryCatch(async () =>
            {
                ValidateGroupPostId(groupId, postId);

                GroupPost maybeGroupPost =
                    await this.storageBroker.SelectGroupPostByIdAsync(groupId, postId);

                ValidateStorageGroupPostExists(maybeGroupPost, groupId, postId);

                return await this.storageBroker.DeleteGroupPostAsync(maybeGroupPost);
            });
    }
}
