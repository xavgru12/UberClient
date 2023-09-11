
using System.Reflection;

namespace Cmune.Core.Types
{
  public class MemberInfoField<T>
  {
    public T Attribute;
    public FieldInfo Field;

    public MemberInfoField(FieldInfo field, T attribute)
    {
      this.Field = field;
      this.Attribute = attribute;
    }

    public string Name => this.Field.Name;
  }
}
