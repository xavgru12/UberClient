// Decompiled with JetBrains decompiler
// Type: DrawArmorPowerIconUtil
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Text;
using UnityEngine;

public static class DrawArmorPowerIconUtil
{
  private static float MaxArmorPoints = 100f;

  public static void DrawArmorPower(Rect rect, int armorPoints, int absortionPoints)
  {
    if (rect.Contains(Event.current.mousePosition))
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine(armorPoints.ToString() + " Armor Points");
      stringBuilder.AppendLine((50 + absortionPoints).ToString("N0") + "% Defense");
      GUI.tooltip = stringBuilder.ToString();
    }
    float num1 = (float) armorPoints / DrawArmorPowerIconUtil.MaxArmorPoints;
    float num2 = (float) (50 + absortionPoints) / 100f;
    GUI.DrawTexture(rect, (Texture) WeaponRange.ArmorIndicatorBackground);
    GUI.color = Color.white;
    GUI.BeginGroup(new Rect(rect.x, rect.y + rect.height * (1f - num1), rect.width / 2f, rect.height * num1));
    GUI.DrawTexture(new Rect(0.0f, (float) (-(double) rect.height * (1.0 - (double) num1)), rect.width, rect.height), (Texture) WeaponRange.ArmorIndicatorForeground);
    GUI.EndGroup();
    GUI.BeginGroup(new Rect(rect.x + rect.width / 2f, rect.y + rect.height * (1f - num2), rect.width / 2f, rect.height * num2));
    GUI.DrawTexture(new Rect((float) (-(double) rect.width / 2.0), (float) (-(double) rect.height * (1.0 - (double) num2)), rect.width, rect.height), (Texture) WeaponRange.ArmorIndicatorForeground);
    GUI.EndGroup();
  }
}
