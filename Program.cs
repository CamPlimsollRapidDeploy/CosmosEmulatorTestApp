using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using Microsoft.Azure.Cosmos;

namespace CosmosGettingStartedDotnetCoreTutorial
{
    public class Program
    {
        private static readonly string EndpointUri = "AccountEndpoint=https://localhost:8081/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
        // The primary key for the Azure Cosmos account.
        private static readonly string PrimaryKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";

        // The Cosmos client instance
        private CosmosClient cosmosClient;

        // The database we will create
        private Database database;

        // The container we will create.
        private Container container;

        // The name of the database and container we will create
        private string databaseId = "RapidDeployDatabase";
        private string containerId = "RapidDeployContainer";

        public static async Task Main(string[] args)
        {
            try
            {

                Console.WriteLine("Beginning operations...\n");
                Program p = new Program();
                await p.Connect();
            }
            catch (CosmosException de)
            {
                Exception baseException = de.GetBaseException();
                Console.WriteLine("{0} error occurred: {1}\n", de.StatusCode, de);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}\n", e);
                throw;
            }
            finally
            {
                Console.WriteLine("End of demo, press any key to exit.");
                Thread.Sleep(TimeSpan.FromMinutes(10));
            }
        }

        public async Task Connect()
        {
            // Latest version of SDK uses HttpClientHandler attempt use it but never seems to be called.
            var httpClientHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback =
                    HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };
            CosmosClientOptions cosmosClientOptions = new CosmosClientOptions
            {
                HttpClientFactory = () =>
                {
                    return new HttpClient(httpClientHandler);
                },
                ConnectionMode = ConnectionMode.Direct
            };

            this.cosmosClient = new CosmosClient(Program.EndpointUri, cosmosClientOptions);

            await this.CreateDatabaseAsync();
            await this.CreateContainerAsync();
        }

      
        private async Task CreateDatabaseAsync()
        {
            // Create a new database
            this.database = await this.cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId);
            Console.WriteLine("Created Database: {0}\n", this.database.Id);
        }

     
        private async Task CreateContainerAsync()
        {
            // Create a new container
            this.container = await this.database.CreateContainerIfNotExistsAsync(containerId, "/LastName");
            Console.WriteLine("Created Container: {0}\n", this.container.Id);
        }

    }
}
