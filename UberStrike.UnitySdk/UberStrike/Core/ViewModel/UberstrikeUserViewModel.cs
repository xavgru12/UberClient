// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.ViewModel.UberstrikeUserViewModel
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using Cmune.DataCenter.Common.Entities;
using System;
using UberStrike.DataCenter.Common.Entities;

namespace UberStrike.Core.ViewModel
{
  [Serializable]
  public class UberstrikeUserViewModel
  {
    public MemberView CmuneMemberView { get; set; }

    public UberstrikeMemberView UberstrikeMemberView { get; set; }
  }
}
