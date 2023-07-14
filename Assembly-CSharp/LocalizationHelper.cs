// Decompiled with JetBrains decompiler
// Type: LocalizationHelper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using UberStrike.Core.Types;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public static class LocalizationHelper
{
  public static bool ValidateMemberName(string name, LocaleType locale = LocaleType.en_US) => locale == LocaleType.ko_KR ? ValidationUtilities.IsValidMemberName(name, "ko-KR") : ValidationUtilities.IsValidMemberName(name);

  public static GUIStyle GetLocalizedStyle(GUIStyle style) => style;
}
