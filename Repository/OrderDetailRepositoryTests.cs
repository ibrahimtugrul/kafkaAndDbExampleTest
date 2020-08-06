using System.Linq;
using AutoFixture;
using FluentAssertions;
using kafkaAndDbPairing;
using kafkaAndDbPairing.domain.entity;
using kafkaAndDbPairing.domain.repository;
using NUnit.Framework;
using TestProject.Utils;

namespace TestProject.Repository
{
    public class OrderDetailRepositoryTests
    {
        private DataContext _context;
        private Fixture _fixture;
        private IOrderDetailRepository _sut;

        [SetUp]
        public void SetUp()
        {
            _context = DataContextFactory.CreateTestDb();
            _fixture = new Fixture();
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
            _sut = new OrderDetailRepository(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context = null;
        }

        [Test]
        public void GetOrderDetailByOrderId_WhenOrderDetailExists_ShouldReturnOrderDetailsById()
        {
            // Arrange
            var orderId = _fixture.Create<long>();

            var orderDetails = _fixture.Build<OrderDetail>()
                .With(x => x.OrderId, orderId)
                .Create();

            _context.OrderDetails.Add(orderDetails);
            _context.SaveChanges();

            // Act
            var response = _sut.GetOrderDetailByOrderId(orderId);

            // Verify
            response.Count.Should().Be(1);
            response[0].OrderId.Should().Be(orderId);
        }

        [Test]
        public void GetOrderDetailByOrderId_WhenOrderDetailNotExists_ShouldReturnEmptyList()
        {
            // Arrange
            var orderId = _fixture.Create<long>();

            var orderDetails = _fixture.Build<OrderDetail>()
                .With(x => x.OrderId, orderId)
                .Create();


            // Act
            var response = _sut.GetOrderDetailByOrderId(orderId);

            // Verify
            response.Count.Should().Be(0);
            response.Should().BeNullOrEmpty();
        }
    }
}
