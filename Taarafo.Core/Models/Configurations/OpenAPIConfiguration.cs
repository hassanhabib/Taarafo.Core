// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

namespace Taarafo.Core.Models.Configurations
{
    public class OpenAPIConfiguration
    {
        public string Version { get; set; }
        public OpenAPIEndpointConfiguration OpenAPIEndpoint { get; set; }
        public OpenAPIDocumentConfiguration Document { get; set; }
    }
}
