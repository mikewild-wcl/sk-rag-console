using Azure.AI.OpenAI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using SK.Rag.Application.Configuration;
using SK.Rag.Application.DocumentLoaders;
using SK.Rag.Application.Extensions;
using SK.Rag.Application.Services;
using SK.Rag.Application.Services.Interfaces;
using SK.Rag.CommandLine.ConsoleApp.Commands;
using Spectre.Console;
using System.ClientModel;
using System.CommandLine;
using System.Diagnostics.CodeAnalysis;

namespace SK.Rag.CommandLine.ConsoleApp.Extensions;


[ExcludeFromCodeCoverage]
public static class ConfigurationExtensions
{
    public static IServiceCollection ConfigureOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AzureOpenAIOptions>(configuration.GetSection(nameof(AzureOpenAIOptions)));

        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddSingleton(_ => AnsiConsole.Console);

        services.AddSingleton<IDocumentLoaderFactory, DocumentLoaderFactory>();
        services.AddTransient<DocxDocumentLoader>();
        services.AddTransient<MarkdownDocumentLoader>();
        services.AddTransient<PdfDocumentLoader>();
        services.AddTransient<TextDocumentLoader>();
        services.AddTransient<WebsiteLoader>();

        services.AddTransient<IHtmlWebProvider, HtmlWebProvider>();

        services.AddTransient<IDocumentService, DocumentService>();
        services.AddTransient<IChatService, ChatService>();
        services.AddTransient<ISearchService, SearchService>();

        services.AddTransient<ChatAction>();
        services.AddTransient<DocumentIngestAction>();

        return services;
    }

    public static IServiceCollection AddClients(this IServiceCollection services)
    {
        services.AddSingleton(serviceProvider =>
        {
            var azureOpenAiSettings = serviceProvider.GetAzureOpenAIOptions();

            return new AzureOpenAIClient(
                new Uri(azureOpenAiSettings!.Endpoint),
                new ApiKeyCredential(azureOpenAiSettings.ApiKey));
        });

        return services;    
    }

    public static IServiceCollection AddSemanticKernel(this IServiceCollection services)
    {
        services.AddTransient(serviceProvider =>     
        {
            var azureOpenAiSettings = serviceProvider.GetAzureOpenAIOptions();
            var client = serviceProvider.GetRequiredService<AzureOpenAIClient>();

            var kernelBuilder = Kernel.CreateBuilder();

            var modelId = azureOpenAiSettings.GetModelIdOrDeploymentName();
            var embeddingModelId = azureOpenAiSettings.GetEmbeddingModelIdOrDeploymentName();

#pragma warning disable SKEXP0010 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
            kernelBuilder
                .AddAzureOpenAIChatCompletion(
                    azureOpenAiSettings.DeploymentName,
                    client,
                    modelId: modelId)
                .AddAzureOpenAIEmbeddingGenerator(
                    azureOpenAiSettings.EmbeddingDeploymentName,
                    client,
                    modelId: embeddingModelId,
                    dimensions: 1536);
#pragma warning restore SKEXP0010 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

            kernelBuilder.Services.AddInMemoryVectorStore();

            return kernelBuilder.Build();
        });

        return services;
    }

    public static RootCommand BuildRootCommand(this IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();

        var commandBuilder = new CommandBuilder(
            "",
            ApplicationConstants.ApplicationDescription,
            serviceProvider,
            new RootCommand());

        commandBuilder
            .AddDocumentCommands()
            .AddChatCommand();

        return (commandBuilder.Command as RootCommand) ?? [];
    }

    private static AzureOpenAIOptions? GetAzureOpenAIOptions(this IServiceProvider serviceProvider)
    {
            // https://github.com/dotnet/runtime/issues/79958#issuecomment-2274396492
            /* var aiOptions = serviceProvider.GetRequiredService<IOptions<AzureOpenAiOptions>>().Value; */
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            return configuration.GetSection(AzureOpenAIOptions.SectionName).Get<AzureOpenAIOptions>();
    }
}