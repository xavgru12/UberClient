// Decompiled with JetBrains decompiler
// Type: Boo.Lang.QuackFuMember
// Assembly: Boo.Lang, Version=2.0.9.5, Culture=neutral, PublicKeyToken=32c39770e9a21a67
// MVID: D4F47A63-3E02-45E3-BD62-EDD90EF56CC2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Boo.Lang.dll

using System;
using System.Text;

namespace Boo.Lang
{
  public class QuackFuMember : IQuackFuMember
  {
    protected string name;
    protected QuackFuMemberKind kind;
    protected Type returnType;
    protected string[] argumentNames;
    protected Type[] argumentTypes;
    protected string info;

    public string Name
    {
      get => this.name;
      set => this.name = !string.IsNullOrEmpty(value) ? value : throw new ArgumentNullException(nameof (value));
    }

    public QuackFuMemberKind Kind
    {
      get => this.kind;
      set => this.kind = value;
    }

    public Type ReturnType
    {
      get => this.returnType;
      set => this.returnType = value;
    }

    public string[] ArgumentNames
    {
      get => this.argumentNames;
      set => this.argumentNames = value;
    }

    public Type[] ArgumentTypes
    {
      get => this.argumentTypes;
      set => this.argumentTypes = value;
    }

    public string Info
    {
      get => this.info;
      set => this.info = value;
    }

    public override string ToString()
    {
      StringBuilder sb = new StringBuilder();
      sb.Append(this.Name);
      if (this.Kind == QuackFuMemberKind.Method)
      {
        sb.Append("(");
        if (this.ArgumentNames != null)
        {
          for (int index = 0; index < this.ArgumentNames.Length; ++index)
          {
            string argumentName = this.ArgumentNames[index];
            if (index > 0)
              sb.Append(", ");
            sb.Append(argumentName);
            if (this.ArgumentTypes != null && this.ArgumentTypes.Length > index)
            {
              Type argumentType = this.ArgumentTypes[index];
              QuackFuMember.AppendTypeInformation(sb, argumentType);
            }
          }
        }
        sb.Append(")");
      }
      QuackFuMember.AppendTypeInformation(sb, this.ReturnType);
      return sb.ToString();
    }

    private static void AppendTypeInformation(StringBuilder sb, Type type)
    {
      if (type == null)
        return;
      sb.Append(" as ");
      sb.Append(QuackFuMember.GetBooTypeName(type));
    }

    private static string GetBooTypeName(Type type)
    {
      if (type == null)
        throw new ArgumentNullException(nameof (type));
      if (type == typeof (int))
        return "int";
      return type == typeof (string) ? "string" : type.FullName;
    }
  }
}
