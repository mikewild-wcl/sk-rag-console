using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.SemanticKernel;
using SK.Rag.Application.Services;

namespace SK.Rag.Application.UnitTests.Builders;

public class SearchServiceBuilder
{
    private Kernel? _kernel;
    private ILogger<SearchService>? _logger;

    public SearchServiceBuilder WithKernel(Kernel kernel)
    {
        _kernel = kernel;
        return this;
    }

    public SearchServiceBuilder WithLogger(ILogger<SearchService> logger)
    {
        _logger = logger;
        return this;
    }

    public SearchService Build() =>
        new(
            _kernel ?? new Kernel(),
            _logger ?? new NullLogger<SearchService>());

    public static SearchService CreateDefault() => new SearchServiceBuilder().Build();
}