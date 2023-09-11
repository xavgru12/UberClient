// Decompiled with JetBrains decompiler
// Type: Cmune.Core.Types.Attributes.ExtendableEnumBoundsAttribute
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

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
