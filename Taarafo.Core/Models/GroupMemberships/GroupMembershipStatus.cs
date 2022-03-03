using System;
using Taarafo.Core.Models.GroupMemberships;

public class GroupMembershipStatus
{
	// move this guy in it's own file
    public enum GroupMembershipStatuses
    {
        Allowed,
        Muted,
        Banned
    }
}
