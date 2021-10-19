namespace UrlShortener.Domain
{
	public interface IUrlRepository
	{
		void Create(ProcessedUrl processedUrl);
		ProcessedUrl ReadByOriginalUrl(string originalUrl);
		ProcessedUrl ReadByShortenedUrl(string shortenedUrl);
	}
}
