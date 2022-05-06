// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Taarafo.Core.Models.Groups;
using Taarafo.Core.Models.Groups.Exceptions;

namespace Taarafo.Core.Services.Foundations.Groups
{
    public partial class GroupService
    {
        private void ValidateGroupOnAdd(Group group)
        {
            ValidateGroupIsNotNull(group);
        }

        private static void ValidateGroupIsNotNull(Group group)
        {
            if (group is null)
            {
                throw new NullGroupException();
            }
        }
    }
}