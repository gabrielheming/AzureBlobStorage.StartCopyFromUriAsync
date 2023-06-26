// See https://aka.ms/new-console-template for more information

using System.Diagnostics.Tracing;
using Azure.Core.Diagnostics;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using AzureBlobStorage.StartCopyFromUriAsync.Bug;

var configuration = new AzureBlobsStorageConfiguration
{
    DefaultEndpointsProtocol = "https",
    AccountName = "accountname",
    AccountKey = "accountkey",
    EndpointSuffix = "core.windows.net",
    ContainerName = "start-async-copy-from-uri-test",
    DirectoryRoot = null,
};

string connectionString = configuration.ConnectionString;
string containerName = configuration.ContainerName;

// Get a reference to a container named "sample-container" and then create it
var options = new BlobClientOptions(BlobClientOptions.ServiceVersion.V2022_11_02);
BlobContainerClient container = new BlobContainerClient(connectionString, containerName, options);
container.CreateIfNotExists();

var fileToCopyUri = new Uri("https://uri-to-the-file.png");

var newFile = "new-file-2.png";

using AzureEventSourceListener listener = AzureEventSourceListener.CreateConsoleLogger(EventLevel.Verbose);
var blob = container.GetBlobClient(newFile);
CopyFromUriOperation operation = await blob.StartCopyFromUriAsync(fileToCopyUri);
await operation.WaitForCompletionAsync();

Console.WriteLine(operation.GetRawResponse());
