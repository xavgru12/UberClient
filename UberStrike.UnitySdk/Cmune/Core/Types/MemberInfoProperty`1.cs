
using System.Reflection;

namespace Cmune.Core.Types
{
  public class MemberInfoProperty<T>
  {
    public T Attribute;
    public PropertyInfo Property;

    public MemberInfoProperty(PropertyInfo field, T attribute)
    {
      this.Property = field;
      this.Attribute = attribute;
    }

    public string Name => this.Property.Name;
  }
}
