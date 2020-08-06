using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoFixture;
using FluentAssertions;
using kafkaAndDbPairing;
using kafkaAndDbPairing.domain.entity;
using kafkaAndDbPairing.domain.repository;
using kafkaAndDbPairing.domain.service;
using Moq;
using NUnit.Framework;
using TestProject.Utils;

namespace kafkaAndDbPairingTest.Services
{
    class OrderDetailServiceTest
    {
        private DataContext _context;
        private Fixture _fixture;
        private IOrderDetailService _sut;
        private Mock<IOrderDetailRepository> _orderDetailRepository;

        [SetUp]
        public void SetUp()
        {
            _context = DataContextFactory.CreateTestDb();
            _fixture = new Fixture();
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
            _orderDetailRepository = new Mock<IOrderDetailRepository>();
            _sut = new OrderDetailService(_orderDetailRepository.Object);
        }

        [Test]
        public void GetOrderDetailByOrderId_WhenOrderDetailExists_ShouldReturnOrderDetailsById()
        {
            // Arrange
            var orderId = _fixture.Create<long>();

            var orderDetails = _fixture.Build<OrderDetail>()
                .With(x => x.OrderId, orderId)
                .CreateMany()
                .ToList();

            _orderDetailRepository.Setup(x => x.GetOrderDetailByOrderId(orderId)).Returns(orderDetails);

            // Act
            var response = _sut.GetOrderDetailByOrderId(orderId);

            // Verify
            response.Count.Should().Be(orderDetails.Count);
        }

        [Test]
        public void GetOrderDetailByOrderId_WhenOrderDetailNotExists_ShouldThrowException()
        {
            // Arrange
            var orderId = _fixture.Create<long>();

            var orderDetails = _fixture.Build<OrderDetail>()
                .With(x => x.OrderId, orderId)
                .CreateMany()
                .ToList();

            _orderDetailRepository.Setup(x => x.GetOrderDetailByOrderId(orderId)).Returns(new List<OrderDetail>());

            // Act & Verify
            Assert.Throws<Exception>(() => _sut.GetOrderDetailByOrderId(orderId));
        }
    }
}
