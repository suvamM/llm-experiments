using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using System.ComponentModel;
using Azure.Identity;

namespace trip_planner_agent
{
    class TripPlannerAgent
    {
        public static async Task PlanTrip()
        {
            var builder = Kernel.CreateBuilder();

            // Replace API key authentication with Microsoft Entra ID authentication
            builder.AddOpenAIChatCompletion(
                        modelId: "meta-llama-3-8b-instruct", // This name is arbitrary
                        apiKey: "sk-fake-key",    // LM Studio ignores this
                        endpoint: new Uri("http://127.0.0.1:1234/v1") // ✅ Fixed: Use Uri, not string
                );

            // Add plugins with explicit names
            builder.Plugins.AddFromType<TimeTeller>("TimeTeller");
            builder.Plugins.AddFromType<ElectricCar>("ElectricCar");
            var kernel = builder.Build();

            // Debug: List available functions
            Console.WriteLine("Available Kernel Functions:");
            foreach (var func in kernel.Plugins.GetFunctionsMetadata())
            {
                Console.WriteLine($"- {func.PluginName}.{func.Name}: {func.Description}");
            }

            // Configure function calling
            OpenAIPromptExecutionSettings settings = new()
            {
                ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions,
                Temperature = 0,  // Lower temperature for more deterministic responses
                MaxTokens = 10000
            };

            while (true)
            {
                Console.Write("User > ");
                string userMessage = Console.ReadLine();
                var result = await kernel.InvokePromptAsync(userMessage, new(settings));
                Console.WriteLine(result);
                Console.WriteLine("--------------------------------------------------------------");
            }
        }
    }
    public class TimeTeller
    {
        [Description("Gets the current time in a detailed format")]
        [KernelFunction]
        public string GetCurrentTime() => DateTime.Now.ToString("F");

        [Description("Checks if the current time is during off-peak hours (between 9pm and 7am)")]
        [KernelFunction]
        public bool IsOffPeak() => DateTime.Now.Hour < 7 || DateTime.Now.Hour >= 21;
    }

    public class ElectricCar
    {
        private bool isCarCharging = false;

        [Description("Checks if the electric car is currently charging")]
        [KernelFunction]
        public bool IsCharging() => isCarCharging;

        [Description("Starts charging the electric car if it's not already charging")]
        [KernelFunction]
        public string StartCharging()
        {
            if (isCarCharging)
            {
                return "Car is already charging.";
            }
            isCarCharging = true;
            return "Charging started successfully.";
        }

        [Description("Stops charging the electric car if it's currently charging")]
        [KernelFunction]
        public string StopCharging()
        {
            if (!isCarCharging)
            {
                return "Car is not currently charging.";
            }
            isCarCharging = false;
            return "Charging stopped successfully.";
        }
    }
}


