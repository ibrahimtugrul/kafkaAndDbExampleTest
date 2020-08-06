using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.Dsl;
using kafkaAndDbPairing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace kafkaAndDbPairingTest.IntegrationTests
{
    public abstract class BaseIntegrationTest<T> where T : AbstractTestStartup
    {
        private Fixture _fixture;
        private Generator<int> _numberGenarator;
        private Random _random;
        private IServiceProvider _serviceProvider;
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var webHostBuilder = new WebHostBuilder();
            webHostBuilder.UseStartup<T>();
            var testServer = new TestServer(webHostBuilder);
            _serviceProvider = testServer.Host.Services;
            HttpClient = testServer.CreateClient();
            _fixture = new Fixture();
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            _numberGenarator = _fixture.Create<Generator<int>>();
            _random = _fixture.Create<Random>();
        }
        protected HttpClient HttpClient { get; private set; }
        protected async Task<HttpResponseMessage> GetWithQueryStringAsync(string url, Dictionary<string, string> queryParams)
        {
            var uri = QueryHelpers.AddQueryString(url, queryParams);
            return await HttpClient.GetAsync(uri);
        }
        protected TType CreateObject<TType>()
        {
            return _fixture.Create<TType>();
        }
        protected ICustomizationComposer<TType> CreateObjectBuilder<TType>()
        {
            return _fixture.Build<TType>();
        }
        protected TType GetRequiredService<TType>()
        {
            return _serviceProvider.GetRequiredService<TType>();
        }
        protected void ClearDataContext(DataContext dataContext)
        {
            dataContext.Database.EnsureDeleted();
        }
        protected int GenerateNumber()
        {
            return _numberGenarator.First();
        }
        protected long GenerateRandomNumber()
        {
            return _random.Next();
        }
        protected long GenerateRandomNumber(int maxValue)
        {
            return _random.Next(maxValue);
        }
    }
}
