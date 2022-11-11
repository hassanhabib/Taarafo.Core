// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using ADotNet.Clients;
using ADotNet.Models.Pipelines.GithubPipelines.DotNets;
using ADotNet.Models.Pipelines.GithubPipelines.DotNets.Tasks;
using ADotNet.Models.Pipelines.GithubPipelines.DotNets.Tasks.SetupDotNetTaskV1s;

namespace Taarafo.Core.Infrastructure.Build.Services
{
	public class ScriptGenerationService
	{
		private readonly ADotNetClient adotNetClient;

		public ScriptGenerationService() =>
			this.adotNetClient = new ADotNetClient();

		public void GenerateBuildScript()
		{
			var githubPipeline = new GithubPipeline
			{
				Name = ".Net",

				OnEvents = new Events
				{
					Push = new PushEvent
					{
						Branches = new string[] { "master" }
					},

					PullRequest = new PullRequestEvent
					{
						Branches = new string[] { "master" }
					}
				},

				Jobs = new Jobs
				{
					Build = new BuildJob
					{
						RunsOn = BuildMachines.Windows2019,

						Steps = new List<GithubTask>
					{
						new CheckoutTaskV2
						{
							Name = "Check Out"
						},

						new SetupDotNetTaskV1
						{
							Name = "Setup Dot Net Version",

							TargetDotNetVersion = new TargetDotNetVersion
							{
								DotNetVersion = "7.0.100",
								IncludePrerelease = false
							}
						},

						new RestoreTask
						{
							Name = "Restore"
						},

						new DotNetBuildTask
						{
							Name = "Build"
						},

						new TestTask
						{
							Name = "Test"
						}
					}
					}
				}
			};

			this.adotNetClient.SerializeAndWriteToFile(
				githubPipeline,
				path: "../../../../.github/workflows/dotnet.yml");
		}

		public void GenerateProvisionScript()
		{
			var githubPipeline = new GithubPipeline
			{
				Name = "Provision Taarafo Core",

				OnEvents = new Events
				{
					Push = new PushEvent
					{
						Branches = new string[] { "master" }
					},

					PullRequest = new PullRequestEvent
					{
						Branches = new string[] { "master" }
					}
				},

				Jobs = new Jobs
				{
					Build = new BuildJob
					{
						RunsOn = BuildMachines.WindowsLatest,

						EnvironmentVariables = new Dictionary<string, string>
						{
							{ "AzureClientId", "${{ secrets.AZURECLIENTID }}" },
							{ "AzureTenantId", "${{ secrets.AZURETENANTID }}" },
							{ "AzureClientSecret", "${{ secrets.AZURECLIENTSECRET }}" },
							{ "AzureAdminName", "${{ secrets.AZUREADMINNAME }}" },
							{ "AzureAdminAccess", "${{ secrets.AZUREADMINACCESS }}" }
						},

						Steps = new List<GithubTask>
						{
							new CheckoutTaskV2
							{
								Name = "Check Out"
							},

							new SetupDotNetTaskV1
							{
								Name = "Setup Dot Net Version",

								TargetDotNetVersion = new TargetDotNetVersion
								{
									DotNetVersion = "7.0.100-preview.1.22110.4",
									IncludePrerelease = true
								}
							},

							new RestoreTask
							{
								Name = "Restore"
							},

							new DotNetBuildTask
							{
								Name = "Build"
							},

							new RunTask
							{
								Name = "Provision",
								Run = "dotnet run --project .\\Taarafo.Core.Infrastructure.Provision\\Taarafo.Core.Infrastructure.Provision.csproj"
							}
						}
					}
				}
			};

			this.adotNetClient.SerializeAndWriteToFile(
				githubPipeline,
				path: "../../../../.github/workflows/provision.yml");
		}
	}
}
