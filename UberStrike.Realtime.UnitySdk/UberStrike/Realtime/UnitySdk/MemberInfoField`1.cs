// Decompiled with JetBrains decompiler
// Type: UberStrike.Realtime.UnitySdk.MemberInfoField`1
// Assembly: UberStrike.Realtime.UnitySdk, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: AA73603F-9C04-49D4-BBD8-49F06040C777
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Realtime.UnitySdk.dll

using System.Reflection;

namespace UberStrike.Realtime.UnitySdk
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
