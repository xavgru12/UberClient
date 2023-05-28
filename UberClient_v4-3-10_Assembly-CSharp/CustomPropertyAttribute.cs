// Decompiled with JetBrains decompiler
// Type: CustomPropertyAttribute
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
public class CustomPropertyAttribute : Attribute
{
  public CustomPropertyAttribute(string name) => this.Name = name;

  public string Name { get; private set; }
}
