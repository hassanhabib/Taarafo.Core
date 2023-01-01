// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Taarafo.Core.Models.Groups;

namespace Taarafo.Core.Services.Foundations.Groups
{
	public interface IGroupService
	{
		ValueTask<Group> AddGroupAsync(Group group);
		ValueTask<Group> RetrieveGroupByIdAsync(Guid groupId);
		IQueryable<Group> RetrieveAllGroups();
		ValueTask<Group> ModifyGroupAsync(Group group);
		ValueTask<Group> RemoveGroupByIdAsync(Guid groupId);
	}
}
