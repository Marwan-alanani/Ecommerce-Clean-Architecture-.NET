namespace ECommerce_Clean_Arch.Domain.Orders.Enums;

public enum OrderStatus
{
    Pending, // created, awaiting payment
    Confirmed, // payment succeeded (Stripe webhook)
    Cancelled // admin cancelled or Stripe session expired
}