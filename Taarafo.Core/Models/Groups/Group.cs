// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using Taarafo.Core.Models.GroupMemberships;

namespace Taarafo.Core.Models.Groups
{
    public class Group
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public GroupStatus Status { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }

        public IEnumerable<GroupMembership> GroupMemberships { get; set; }
    }
}
