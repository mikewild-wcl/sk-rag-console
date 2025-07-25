using HtmlAgilityPack;
using SK.Rag.Application.Services.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace SK.Rag.Application.Services;

[ExcludeFromCodeCoverage(Justification = "This class is a thin wrapper around HtmlAgilityPack and is difficult to unit test.")]
public class HtmlWebProvider : IHtmlWebProvider
{
    private readonly HtmlWeb _htmlWeb;

    public HtmlWebProvider()
    {
        _htmlWeb = new HtmlWeb();
    }

    public async Task<HtmlDocument> LoadFromWebAsync(string downloadPath)
    {
        return await _htmlWeb.LoadFromWebAsync(downloadPath);
    }
}
