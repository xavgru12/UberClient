// Decompiled with JetBrains decompiler
// Type: CommUserNameComparer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

public class CommUserNameComparer : Comparer<CommUser>
{
  public override int Compare(CommUser f1, CommUser f2) => CommUserComparer.UserNameCompare(f1, f2);
}
