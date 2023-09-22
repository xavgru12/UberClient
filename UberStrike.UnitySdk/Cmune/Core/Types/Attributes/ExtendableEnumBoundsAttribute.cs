
using System;

namespace Cmune.Core.Types.Attributes
{
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
  public class ExtendableEnumBoundsAttribute : Attribute
  {
    public ExtendableEnumBoundsAttribute(int min, int max)
    {
      this.Min = min;
      this.Max = max;
    }

    public int Min { get; private set; }

    public int Max { get; private set; }
  }
}
