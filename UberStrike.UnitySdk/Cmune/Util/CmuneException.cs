// Decompiled with JetBrains decompiler
// Type: Cmune.Util.CmuneException
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using System;

namespace Cmune.Util
{
  public class CmuneException : Exception
  {
    public CmuneException(string str, params object[] args) => CmuneDebug.LogError("EXCEPTION: " + string.Format(str, args), new object[0]);
  }
}
