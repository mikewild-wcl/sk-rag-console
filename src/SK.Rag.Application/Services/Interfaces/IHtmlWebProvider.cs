using HtmlAgilityPack;

namespace SK.Rag.Application.Services.Interfaces;

public interface IHtmlWebProvider
{
    Task<HtmlDocument> LoadFromWebAsync(string downloadPath);
}