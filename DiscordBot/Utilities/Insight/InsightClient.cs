using NBitcoin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;

namespace CCWallet.DiscordBot.Utilities.Insight
{
    public class InsightClient
    {
        // Endpoint name to use when reading from .env file
        public static string[] EndpointNames { get; } = new string[]{
            "UTXO", "SEND"
        };

        public Uri BaseUri { get; }
        public Dictionary<String, String> Endpoints { get; }

        public InsightClient(string baseuri, Dictionary<String, String> endpoints)
        {
            BaseUri = new Uri(baseuri.TrimEnd('/'), UriKind.Absolute);

            Endpoints = new Dictionary<string, string>();
            foreach(var val in endpoints)
            {
                var uri = val.Value;
                if (!uri.StartsWith('/'))
                {
                    uri = $"/{uri}";
                }
                Endpoints.Add(val.Key, uri);
            }
        }

        public async Task<IEnumerable<UnspentOutput.UnspentCoin>> GetUnspentCoinsAsync(BitcoinAddress address)
        {
            var builder = new UriBuilder(BaseUri);
            String uri = Endpoints.GetValueOrDefault("UTXO", "/addr/{0}/utxo"); ;
            builder.Path += String.Format(uri, address);
            builder.Query = "noCache=1";
            
            return (await FetchAsync<IList<UnspentOutput>>(builder.Uri)).Select(u => u.ToUnspentCoin());
        }

        public async Task BroadcastAsync(Transaction tx)
        {
            var builder = new UriBuilder(BaseUri);
            builder.Path += Endpoints.GetValueOrDefault("SEND", "/tx/send");

            await PostAsync(builder.Uri, Broadcast.ConvertFrom(tx));
        }

        private async Task<T> FetchAsync<T>(Uri uri) where T : class
        {
            var request = WebRequest.Create(uri);
            var response = await request.GetResponseAsync();

            using (var stream = response.GetResponseStream())
            {
                var serializer = new DataContractJsonSerializer(typeof(T));

                return serializer.ReadObject(stream) as T;
            }
        }

        private async Task<WebResponse> PostAsync<T>(Uri uri, T param) where T : class 
        {
            var request = WebRequest.Create(uri);
            request.Method = WebRequestMethods.Http.Post;
            request.ContentType = "application/json";

            using (var stream = request.GetRequestStream())
            {
                new DataContractJsonSerializer(typeof(T)).WriteObject(stream, param);
            }

            return await request.GetResponseAsync();
        }
    }
}
