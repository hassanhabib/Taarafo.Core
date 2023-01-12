// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Linq;
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

        [HttpGet]
        public ActionResult<IQueryable<Profile>> GetAllProfiles()
        {
            try
            {
                IQueryable<Profile> retrievedProfiles =
                    this.profileService.RetrieveAllProfiles();

                return Ok(retrievedProfiles);
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

        [HttpDelete("profileId")]
        public async ValueTask<ActionResult<Profile>> DeleteProfileByIdAsync(Guid profileId)
        {
            try
            {
                Profile deleteProfile = 
                    await this.profileService.RemoveProfileByIdAsync(profileId);

                return Ok(deleteProfile);
            }
            catch (ProfileValidationException profileValidationException)
                when (profileValidationException.InnerException is NotFoundProfileException)
            {
                return NotFound(profileValidationException.InnerException);
            }
            catch (ProfileValidationException profileValidationException)
            {
                return BadRequest(profileValidationException.InnerException);
            }
            catch (ProfileDependencyValidationException profileDependencyValidationException)
                when (profileDependencyValidationException.InnerException is LockedProfileException)
            {
                return Locked(profileDependencyValidationException.InnerException);
            }
            catch (ProfileDependencyValidationException profileDependencyValidationException)
            {
                return BadRequest(profileDependencyValidationException.InnerException);
            }
            catch (ProfileDependencyException profileDependencyException)
            {
                return InternalServerError(profileDependencyException.InnerException);
            }
            catch (ProfileServiceException profileServiceException)
            {
                return InternalServerError(profileServiceException);
            }
        }
    }
}