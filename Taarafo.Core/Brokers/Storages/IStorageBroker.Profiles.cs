// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Taarafo.Core.Models.Profiles;

namespace Taarafo.Core.Brokers.Storages
{
	public partial interface IStorageBroker
	{
		ValueTask<Profile> InsertProfileAsync(Profile profile);
		IQueryable<Profile> SelectAllProfiles();
		ValueTask<Profile> SelectProfileByIdAsync(Guid profileId);
		ValueTask<Profile> UpdateProfileAsync(Profile profile);
		ValueTask<Profile> DeleteProfileAsync(Profile profile);
	}
}
