
using System.Reflection;

namespace Cmune.Core.Types
{
  public class MemberInfoMethod<T>
  {
    public T Attribute;
    public MethodInfo Method;

    public MemberInfoMethod(MethodInfo method, T attribute)
    {
      this.Method = method;
      this.Attribute = attribute;
    }

    public string Name => this.Method.Name;
  }
}
