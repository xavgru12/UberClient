// Decompiled with JetBrains decompiler
// Type: Cmune.Core.Types.MemberInfoMethod`1
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

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
