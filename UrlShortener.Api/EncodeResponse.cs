namespace UrlShortener.Api
{
	public class EncodeResponse
	{
		public EncodeResponse(string shortenedUrl)
		{
			this.ShortenedUrl = shortenedUrl;
		}

		public string ShortenedUrl { get; set; }
	}
}
