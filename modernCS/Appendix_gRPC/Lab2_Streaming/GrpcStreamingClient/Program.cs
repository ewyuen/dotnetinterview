using System;
using System.Threading.Tasks;
using Grpc.Net.Client;
using GrpcStreaming;

namespace GrpcStreamingClient
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            // NOTE: Ensure the server is running on the given URL before running the client.
            using var channel = GrpcChannel.ForAddress("http://localhost:7000");
            var client = new Weather.WeatherClient(channel);

            var request = new WeatherRequest { Location = "Seattle, WA", IntervalSeconds = 1, MaxUpdates = 5 };

            using var call = client.GetWeatherStream(request);

            var responseStream = call.ResponseStream;
            while (await responseStream.MoveNext(System.Threading.CancellationToken.None))
            {
                var update = responseStream.Current;
                Console.WriteLine($"[{update.Timestamp}] Temp={update.Temperature} Summary={update.Summary}");
            }
        }
    }
}