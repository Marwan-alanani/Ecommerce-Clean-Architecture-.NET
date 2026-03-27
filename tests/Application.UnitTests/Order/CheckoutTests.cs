using ECommerce_Clean_Arch.Application.Abstractions.Persistence;
using ECommerce_Clean_Arch.Application.Abstractions.Persistence.Repositories;
using ECommerce_Clean_Arch.Application.Abstractions.Services;
using ECommerce_Clean_Arch.Application.Common.Interfaces;
using ECommerce_Clean_Arch.Application.Orders.Commands.Checkout;
using ECommerce_Clean_Arch.Domain.Carts;
using ECommerce_Clean_Arch.Domain.Errors.Orders;
using ECommerce_Clean_Arch.Domain.Errors.Security;
using ECommerce_Clean_Arch.Domain.Orders.ValueObjects;

using FluentAssertions;

using Moq;

using SharedKernel.Errors;

namespace Application.UnitTests.Order;

public class CheckoutTests
{
    private readonly CheckoutCommandHandler _handler;
    private readonly Mock<IUser> _user;
    private readonly Mock<IDateTimeProvider> _dateTime;
    private readonly Mock<ICartRepository> _cartRepository;
    private readonly Mock<ICartKeyResolver> _cartKeyResolver;
    private readonly Mock<IApplicationDbContext> _context;
    private readonly Mock<IPaymentGateway> _paymentGateway;

    public CheckoutTests()
    {
        _user = new();
        _dateTime = new();
        _cartRepository = new();
        _cartKeyResolver = new();
        _context = new();
        _paymentGateway = new();
        _handler = new(
            _user.Object,
            _dateTime.Object,
            _cartRepository.Object,
            _cartKeyResolver.Object,
            _context.Object,
            _paymentGateway.Object);
    }

    [Fact]
    public async Task Handle_Should_ReturnSecurityError_WhenUserIdIsNull()
    {
        var command = new CheckoutCommand() { ShippingAddress = It.IsAny<ShippingAddress>() };
        _user.Setup(u => u.Id).Returns((Guid?)null);
        var result = await _handler.Handle(command, CancellationToken.None);
        result.IsFailure.Should().BeTrue();
        result.Error.Type.Should().Be(ErrorType.Security);
        result.Error.Reasons.Should().Contain(new UserUnauthenticated());
    }

    [Fact]
    public async Task Handle_Should_ReturnValidationError_WhenCartIsNull()
    {
        // Arrange
        _user.Setup(u => u.Id).Returns(It.IsAny<Guid>());
        _cartKeyResolver.Setup(c => c.GetUserKey(It.IsAny<Guid>())).Returns(It.IsAny<string>());
        _cartRepository
            .Setup(c => c.GetCartAsync(It.IsAny<string>()))
            .ReturnsAsync(() => null);
        var command = new CheckoutCommand() { ShippingAddress = It.IsAny<ShippingAddress>() };
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Type.Should().Be(ErrorType.Validation);
        result.Error.Reasons.Should().Contain(new EmptyCart());
    }

    [Fact]
    public async Task Handle_Should_ReturnValidationError_WhenCartIsEmpty()
    {
        // Arrange
        _user.Setup(u => u.Id).Returns(It.IsAny<Guid>());
        _cartKeyResolver.Setup(c => c.GetUserKey(It.IsAny<Guid>())).Returns(It.IsAny<string>());
        _cartRepository
            .Setup(c => c.GetCartAsync(It.IsAny<string>()))
            .ReturnsAsync(Cart.Create);
        var command = new CheckoutCommand() { ShippingAddress = It.IsAny<ShippingAddress>() };
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Type.Should().Be(ErrorType.Validation);
        result.Error.Reasons.Should().Contain(new EmptyCart());
    }
}