// Decompiled with JetBrains decompiler
// Type: CommUserPresenceComparer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

public class CommUserPresenceComparer : Comparer<CommUser>
{
  public override int Compare(CommUser f1, CommUser f2)
  {
    if (f1.PresenceIndex == f2.PresenceIndex)
      return CommUserComparer.UserNameCompare(f1, f2);
    if (f1.PresenceIndex == PresenceType.Offline)
      return 1;
    return f2.PresenceIndex == PresenceType.Offline ? -1 : CommUserComparer.UserNameCompare(f1, f2);
  }
}
