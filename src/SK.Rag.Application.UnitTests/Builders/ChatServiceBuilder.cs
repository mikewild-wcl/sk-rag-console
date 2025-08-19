using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.SemanticKernel;
using SK.Rag.Application.Services;
using SK.Rag.Application.Services.Interfaces;

namespace SK.Rag.Application.UnitTests.Builders;

public class ChatServiceBuilder
{
    private Kernel? _kernel;
    private ISearchService? _searchService;
    private ILogger<ChatService>? _logger;

    public ChatServiceBuilder WithKernel(Kernel kernel)
    {
        _kernel = kernel;
        return this;
    }

    public ChatServiceBuilder WithSearchService(ISearchService searchService)
    {
        _searchService = searchService;
        return this;
    }

    public ChatServiceBuilder WithLogger(ILogger<ChatService> logger)
    {
        _logger = logger;
        return this;
    }

    public ChatService Build() => 
        new(
            _kernel ?? new Kernel(),
            _searchService ?? new SearchServiceBuilder().WithKernel(_kernel ?? new Kernel()).Build(),
            _logger ?? new NullLogger<ChatService>());

    public static ChatService CreateDefault() => new ChatServiceBuilder().Build();
}