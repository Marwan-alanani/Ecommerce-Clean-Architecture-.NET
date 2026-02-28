using AutoMapper;
using ECommerce_Clean_Arch.Domain.Products;
using ECommerce_Clean_Arch.Domain.Products.ValueObjects;

namespace ECommerce_Clean_Arch.Application.Products.Queries.GetById;

public record ProductDto
{
    private ProductDto()
    {
    }

    public ProductDto(
        Guid id,
        string name,
        string? description,
        string currency,
        decimal price,
        DateTime createdAt,
        DateTime updatedAt
    )
    {
        Id = id;
        Name = name;
        Description = description;
        Currency = currency;
        Price = price;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }


    public Guid Id { get; init; }
    public string Name { get; init; } = null!;
    public string? Description { get; init; }
    public decimal Price { get; init; }
    public string Currency { get; init; } = null!;
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }

    private class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<ProductId, Guid>()
                .ConvertUsing(id => id.Value);

            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price.Amount))
                .ForMember(
                    dest => dest.Currency,
                    opt => opt.MapFrom(src => src.Price.Currency.ToString())
                );
        }
    }
}