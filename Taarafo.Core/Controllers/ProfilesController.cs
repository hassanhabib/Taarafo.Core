// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
using Taarafo.Core.Models.Profiles;
using Taarafo.Core.Models.Profiles.Exceptions;
using Taarafo.Core.Services.Foundations.Profiles;

namespace Taarafo.Core.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class ProfilesController : RESTFulController
	{
		private readonly IProfileService profileService;

		public ProfilesController(IProfileService profileService) =>
			this.profileService = profileService;

		[HttpPost]
		public async ValueTask<ActionResult<Profile>> PostProfileAsync(Profile profile)
		{
			try
			{
				Profile addedProfile =
					await this.profileService.AddProfileAsync(profile);

				return Created(addedProfile);
			}
			catch (ProfileValidationException profileValidationException)
			{
				return BadRequest(profileValidationException.InnerException);
			}
			catch (ProfileDependencyValidationException profileDependencyValidationException)
				when (profileDependencyValidationException.InnerException is InvalidProfileReferenceException)
			{
				return FailedDependency(profileDependencyValidationException.InnerException);
			}
			catch (ProfileDependencyValidationException profileDependencyValidationException)
				when (profileDependencyValidationException.InnerException is AlreadyExistsProfileException)
			{
				return Conflict(profileDependencyValidationException.InnerException);
			}
			catch (ProfileDependencyException profileDependencyException)
			{
				return InternalServerError(profileDependencyException);
			}
			catch (ProfileServiceException profileServiceException)
			{
				return InternalServerError(profileServiceException);
			}
		}
	}
}
