using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace kafkaAndDbPairingTest.Extensions
{
    public static class Extensions
    {
        public static string Message(this string errorResponse)
        {
            return string.Join(Environment.NewLine, "error");
        }
        public static async Task<T> ReadAsAsync<T>(this HttpContent content) =>
            await JsonSerializer.DeserializeAsync<T>(await content.ReadAsStreamAsync());
    }
}
