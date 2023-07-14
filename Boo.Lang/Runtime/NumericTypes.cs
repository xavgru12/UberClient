// Decompiled with JetBrains decompiler
// Type: Boo.Lang.Runtime.NumericTypes
// Assembly: Boo.Lang, Version=2.0.9.5, Culture=neutral, PublicKeyToken=32c39770e9a21a67
// MVID: D4F47A63-3E02-45E3-BD62-EDD90EF56CC2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Boo.Lang.dll

using System;

namespace Boo.Lang.Runtime
{
  public class NumericTypes
  {
    public static bool IsWideningPromotion(Type paramType, Type argType) => NumericTypes.NumericRangeOrder(paramType) > NumericTypes.NumericRangeOrder(argType);

    public static int NumericRangeOrder(Type type)
    {
      switch (Type.GetTypeCode(type))
      {
        case TypeCode.Boolean:
          return 1;
        case TypeCode.Char:
        case TypeCode.Int16:
        case TypeCode.UInt16:
          return 3;
        case TypeCode.SByte:
        case TypeCode.Byte:
          return 2;
        case TypeCode.Int32:
        case TypeCode.UInt32:
          return 4;
        case TypeCode.Int64:
        case TypeCode.UInt64:
          return 5;
        case TypeCode.Single:
          return 7;
        case TypeCode.Double:
          return 8;
        case TypeCode.Decimal:
          return 6;
        default:
          throw new ArgumentException(type.ToString());
      }
    }
  }
}
