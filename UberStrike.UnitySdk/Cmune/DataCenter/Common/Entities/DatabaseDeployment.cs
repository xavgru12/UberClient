// Decompiled with JetBrains decompiler
// Type: Cmune.DataCenter.Common.Entities.DatabaseDeployment
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using System.Runtime.InteropServices;

namespace Cmune.DataCenter.Common.Entities
{
  [StructLayout(LayoutKind.Sequential, Size = 1)]
  public struct DatabaseDeployment
  {
    public const string Dev = "dev";
    public const string Staging = "staging";
    public const string Prod = "prod";
  }
}
