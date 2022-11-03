// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Taarafo.Core.Models.Groups;

namespace Taarafo.Core.Brokers.Storages
{
	public partial interface IStorageBroker
	{
		ValueTask<Group> InsertGroupAsync(Group group);
		IQueryable<Group> SelectAllGroups();
		ValueTask<Group> SelectGroupByIdAsync(Guid groupId);
		ValueTask<Group> UpdateGroupAsync(Group group);
		ValueTask<Group> DeleteGroupAsync(Group group);
	}
}
