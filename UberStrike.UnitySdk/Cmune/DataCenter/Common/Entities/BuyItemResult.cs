
namespace Cmune.DataCenter.Common.Entities
{
  public class BuyItemResult
  {
    public const int Ok = 0;
    public const int DisableInShop = 1;
    public const int DisableForRent = 3;
    public const int DisableForPermanent = 4;
    public const int DurationDisabled = 5;
    public const int PackDisabled = 6;
    public const int IsNotForSale = 7;
    public const int NotEnoughCurrency = 8;
    public const int InvalidMember = 9;
    public const int InvalidExpirationDate = 10;
    public const int AlreadyInInventory = 11;
    public const int InvalidAmount = 12;
    public const int NoStockRemaining = 13;
    public const int InvalidData = 14;
    public const int TooManyUsage = 15;
  }
}
