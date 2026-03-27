using ECommerce_Clean_Arch.Application.Abstractions.Persistence;
using ECommerce_Clean_Arch.Application.Categories.Commands.Create;
using ECommerce_Clean_Arch.Domain.Categories;
using ECommerce_Clean_Arch.Domain.Errors.Categories;

using FluentAssertions;

using Moq;
using Moq.EntityFrameworkCore;

using SharedKernel.Errors;

namespace Application.UnitTests.Categories;

public class CreateCategoryCommandHandlerTests
{
    private readonly Mock<IApplicationDbContext> _context;
    private readonly CreateCategoryCommandHandler _handler;

    public CreateCategoryCommandHandlerTests()
    {
        _context = new();
        _handler = new(_context.Object);
    }

    [Fact]
    public async Task Handle_Should_ReturnCategoryId_WhenNameIsUnique()
    {
        // Arrange
        _context
            .Setup(c => c.Categories)
            .ReturnsDbSet(new List<Category>());

        var command = new CreateCategoryCommand("Category");

        // Act
        var result = await _handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
    }

    [Fact]
    public async Task Handle_Should_ReturnFailureResult_WhenNameIsNotUnique()
    {
        // Arrange
        var categories = new List<Category>() { Category.Create("Category") };

        _context
            .Setup(c => c.Categories)
            .ReturnsDbSet(categories);

        var command = new CreateCategoryCommand("Category");

        // Act
        var result = await _handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Type.Should().Be(ErrorType.Conflict);
        result.Error.Reasons.Should().Contain(new CategoryNameAlreadyExists(command.Name));
    }
}