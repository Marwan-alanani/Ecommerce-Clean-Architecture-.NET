

namespace ECommerce_Clean_Arch.Application.Common.Models;

public static class MoneyMapping
{
    private class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<MoneyFlat, Money>()
                .ConvertUsing(src => new Money(Enum.Parse<Currency>(src.Currency, true), src.Amount));
            CreateMap<Money, MoneyFlat>()
                .ConvertUsing(src => new MoneyFlat(src.Currency.ToString(), src.Amount));
        }
    }
}