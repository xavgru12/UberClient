
namespace Cmune.Realtime.Common
{
  public interface IAttributeID<Type>
  {
    Type ID { get; }

    bool HasID { get; }
  }
}
