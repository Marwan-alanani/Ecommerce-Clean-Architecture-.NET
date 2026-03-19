using AutoMapper;

using SharedKernel.Models;

namespace ECommerce_Clean_Arch.Application.Common.Models;

public record MoneyDto(string Currency, decimal Amount)
{
    private class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<MoneyDto, Money>()
                .ConvertUsing(src => new Money(Enum.Parse<Currency>(src.Currency, true), src.Amount));
            CreateMap<Money, MoneyDto>()
                .ConvertUsing(src => new MoneyDto(src.Currency.ToString(), src.Amount));
        }
    }
}