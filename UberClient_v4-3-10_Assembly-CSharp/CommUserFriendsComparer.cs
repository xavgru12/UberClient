// Decompiled with JetBrains decompiler
// Type: CommUserFriendsComparer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

public class CommUserFriendsComparer : Comparer<CommUser>
{
  public override int Compare(CommUser f1, CommUser f2)
  {
    if ((f1.IsFriend || f1.IsClanMember) && (f2.IsFriend || f2.IsClanMember))
      return CommUserComparer.UserNameCompare(f1, f2);
    if (f2.IsFriend || f2.IsClanMember)
      return 1;
    return f1.IsFriend || f1.IsClanMember ? -1 : CommUserComparer.UserNameCompare(f1, f2);
  }
}
