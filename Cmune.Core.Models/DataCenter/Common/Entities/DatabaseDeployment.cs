// Decompiled with JetBrains decompiler
// Type: Cmune.DataCenter.Common.Entities.DatabaseDeployment
// Assembly: Cmune.Core.Models, Version=1.0.2.98, Culture=neutral, PublicKeyToken=null
// MVID: C9B63271-07DC-4C93-BD74-A807803DC1C2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Cmune.Core.Models.dll

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
