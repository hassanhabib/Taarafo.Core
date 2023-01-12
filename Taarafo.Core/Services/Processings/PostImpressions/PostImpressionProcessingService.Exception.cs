// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Threading.Tasks;
using Taarafo.Core.Models.PostImpressions;

namespace Taarafo.Core.Services.Processings.PostImpressions
{
    public partial class PostImpressionProcessingService
    {
        private delegate ValueTask<PostImpression> ReturningPostImpressionFunction();
        private async ValueTask<PostImpression> TryCatch(ReturningPostImpressionFunction returningPostImpressionFunction)
        {
            try
            {
                return await returningPostImpressionFunction();
            }
            catch (System.Exception)
            {

                throw;
            }
        }
    }
}