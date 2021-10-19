namespace UrlShortener.Domain
{
	public interface IUrlShortenerService
	{
		string ShortenUrl(string urlToShorten, string newUrlScheme, string newUrlHost);
		string GetOriginalUrl(string shortenedUrl);
	}
}
