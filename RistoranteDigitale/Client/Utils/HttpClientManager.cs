using System.Net.Http;

namespace RistoranteDigitaleClient.Utils
{
    public static class HttpClientManager
    {
        static readonly HttpClient client = new();
        public static HttpClient Client { get => client; }
    }
}
