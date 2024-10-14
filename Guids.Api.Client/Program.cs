using Guids.Api.Client.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

class Program
{
    private static JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings()
    {
        DateFormatString = "yyyy-MM-ddTHH:mm:ss.fffZ"
    };
    static async Task Main(string[] args)
    {
       string baseUrl = (args == null || args.Length == 0 || string.IsNullOrWhiteSpace(args[0])) ? "http://localhost:5000" : args[0];

       Console.WriteLine($"\nBase url for Guids.Api: {baseUrl}\n");   

       var builder = new HostBuilder()
            .ConfigureServices((hostContext, services) =>
                {
                    services.AddHttpClient();
                    services.AddTransient<IGuidApiClient>(s => new GuidApiClient(baseUrl, s.GetRequiredService<IHttpClientFactory>()));
                }).UseConsoleLifetime();
 
            var host = builder.Build();
            
            try
            {
                var guidService = host.Services.GetRequiredService<IGuidApiClient>();
                bool quitNow = false;
                DisplayHelp();
                while(!quitNow)
                {
                    Console.WriteLine("\nPlease enter a command or type help for list of commands:\n");   
                    string command = Console.ReadLine()?.Trim()?.ToLower();
                    if (command == null)
                    {
                        Console.WriteLine("No command entered. Please try again.");
                        Thread.Sleep(1000);
                        continue;
                    }
                    switch (command)
                    {
                        case "create":
                            await ProcessCreateCommand(guidService);
                            break;

                        case "get":
                            await ProcessGetCommand(guidService);
                            break;

                        case "update":
                            await ProcessUpdateCommand(guidService);
                            break;

                        case "delete":
                            await ProcessDeleteCommand(guidService);
                            break;

                        case "quit":
                            Console.WriteLine("Exiting application.");
                            quitNow = true;
                            break;

                        case "help":
                            DisplayHelp();
                            break;

                        default:
                            Console.WriteLine("Unknown Command. Please try again" + command);
                            break;
                    }
                }
            }
            catch (ApiException ex)
            {
                Console.WriteLine($"Client returned code: {ex.StatusCode}. Error description: " + ex.Message + ex.StackTrace);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred. Error description: " + ex.Message + ex.StackTrace);
            }
    }

    private static async Task ProcessCreateCommand(IGuidApiClient guidService)
    {
        Console.WriteLine("Please enter User.");
        string user = Console.ReadLine();
        Console.WriteLine("Please enter Expiry Date, specify empty for default.");
        string expiration = Console.ReadLine();

        var createGuidRq = new CreateGuidRq()
        {
            User = user
        };
        if (DateTime.TryParse(expiration, out DateTime expirationDate))
        {
            createGuidRq.Expires = expirationDate;
        }
        else
        {
            Console.WriteLine("Invalid or Empty Date. Service will use default expiration date.");
        }

        Console.WriteLine("Creating Guid...");
        
        await ExecuteApiCall(async () =>
        {
            var result = await guidService.GuidPOSTAsync(createGuidRq);
            Console.WriteLine("Create Guid Result: " + Newtonsoft.Json.JsonConvert.SerializeObject(result, Formatting.Indented, _jsonSerializerSettings));
        });
    }

    private static async Task ProcessUpdateCommand(IGuidApiClient guidService)
    {
        Console.WriteLine("Please enter Guid:");
        string guid = Console.ReadLine();
        //Not checking for empty guid,in case we need to test this scenario.
        Console.WriteLine("Please enter User, specify empty line if no update needed.");
        string user = Console.ReadLine();
        Console.WriteLine("Please enter Expiry Date, specify empty line if no update needed.");
        string expiration = Console.ReadLine();

        var updateGuidRq = new UpdateGuidMetadataRq()
        {
            User = user
        };
        if (DateTime.TryParse(expiration, out DateTime expirationDate))
        {
            updateGuidRq.Expires = expirationDate;
        }
        else
        {
            Console.WriteLine("Invalid or Empty Date. Service will use default expiration date.");
        }

        Console.WriteLine("Updating Guid...");
        
        await ExecuteApiCall(async () =>
        {
            var result = await guidService.GuidPUTAsync(guid, updateGuidRq);
            Console.WriteLine("Update Guid Result: " + Newtonsoft.Json.JsonConvert.SerializeObject(result, _jsonSerializerSettings));
        });
    }

    private static async Task ProcessGetCommand(IGuidApiClient guidService)
    {
        Console.WriteLine("Please enter Guid:");
        string guid = Console.ReadLine();
        //Not checking for empty guid,in case we need to test this scenario.
      
        Console.WriteLine("Getting Guid...");
        await ExecuteApiCall(async () =>
        {
            var result = await guidService.GuidGETAsync(guid);
            Console.WriteLine("Get Guid Result: " + Newtonsoft.Json.JsonConvert.SerializeObject(result, _jsonSerializerSettings));
        });
    }

    private static async Task ExecuteApiCall(Func<Task> func)
    {
        try
        {
            await func();
        }
        catch (ApiException ex)
        {
            Console.WriteLine($"Client returned code: {ex.StatusCode}. Response Message: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred. Error description: " + ex.Message + ex.StackTrace);
        }
    }

    private static async Task ProcessDeleteCommand(IGuidApiClient guidService)
    {
        Console.WriteLine("Please enter Guid:");
        string guid = Console.ReadLine();
        //Not checking for empty guid,in case we need to test this scenario.
      
        Console.WriteLine("Deleting Guid...");
        
        await ExecuteApiCall(async () =>
        {
            await guidService.GuidDELETEAsync(guid);
            Console.WriteLine("Guid Delete Complete.");
        });
    }

    private static void DisplayHelp()
    {
        Console.WriteLine("List of available commands: ");   
        Console.WriteLine("---------------------------------");
        Console.WriteLine("help - Display this prompt");
        Console.WriteLine("create - Create Guid");
        Console.WriteLine("get - Get Guid");
        Console.WriteLine("update - Update Guid");
        Console.WriteLine("delete - Delete Guid");
        Console.WriteLine("quit - Quit the application");
        Console.WriteLine("---------------------------------");
    }
}