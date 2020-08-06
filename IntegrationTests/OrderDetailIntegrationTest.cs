using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using kafkaAndDbPairing;
using kafkaAndDbPairing.domain.entity;
using kafkaAndDbPairingTest.Extensions;
using Newtonsoft.Json;
using NUnit.Framework;

namespace kafkaAndDbPairingTest.IntegrationTests
{
    public class OrderDetailIntegrationTest : BaseIntegrationTest<DefaultTestStartup>
    {
        private DataContext _context;

        [SetUp]
        public void SetUp()
        {
            _context = GetRequiredService<DataContext>();
            ClearDataContext(_context);
        }

        [Test]
        public async Task GetOrderDetail_TrueStory()
        {
            // Arrange
            var orderId = CreateObject<long>();

            var orderDetails = CreateObjectBuilder<OrderDetail>()
                .With(x => x.OrderId, orderId)
                .CreateMany()
                .ToList();

            _context.OrderDetails.AddRange(orderDetails);
            await _context.SaveChangesAsync();

            // Act
            var response = await HttpClient.GetAsync($"orders/{orderId}/detail");

            // Verify
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = await response.Content.ReadAsAsync<List<OrderDetail>>();
            result.Count.Should().Be(orderDetails.Count);
        }
    }
}