// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using Taarafo.Core.Models.Groups;

namespace Taarafo.Core.Brokers.Storages
{
	public partial interface IStorageBroker
	{
		ValueTask<Group> InsertGroupAsync(Group group);
		IQueryable<Group> SelectAllGroups();
	}
}
