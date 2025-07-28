using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.Extensions.DependencyInjection;

class LocalLLMTest
{
    static async Task InvokeLocalLLM()
    {
        // Create the kernel builder
        var builder = Kernel.CreateBuilder();

        // Add OpenAI-compatible chat completion (DeepSeek via LM Studio)
        builder.AddOpenAIChatCompletion(
            modelId: "microsoft/phi-4", // This name is arbitrary
            apiKey: "sk-fake-key",    // LM Studio ignores this
            endpoint: new Uri("http://127.0.0.1:1234/v1") // ✅ Fixed: Use Uri, not string
        );

        // Build the kernel
        var kernel = builder.Build();

        // Get the chat completion service
        var chatService = kernel.GetRequiredService<IChatCompletionService>();

        // Start a chat with some initial messages
        var chatHistory = new ChatHistory();
        chatHistory.AddSystemMessage("You are a helpful assistant.");
        chatHistory.AddUserMessage("What is the integral of e raised to x?");

        Console.WriteLine("Assistant: ");

        // Stream the response
        await foreach (var message in chatService.GetStreamingChatMessageContentsAsync(chatHistory))
        {
            Console.Write(message.Content);
        }

        Console.WriteLine(); // End line
    }
}
