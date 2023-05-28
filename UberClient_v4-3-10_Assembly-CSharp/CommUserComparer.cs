// Decompiled with JetBrains decompiler
// Type: CommUserComparer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

public static class CommUserComparer
{
  public static int UserNameCompare(CommUser f1, CommUser f2) => string.Compare(f1.ShortName, f2.ShortName, true);
}
