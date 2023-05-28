// Decompiled with JetBrains decompiler
// Type: TrainingPageGUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class TrainingPageGUI : MonoBehaviour
{
  private const int PageWidth = 700;
  private const int PageHeight = 480;
  private const int MapsPerRow = 4;
  private Vector2 _mapScroll;

  private void OnGUI()
  {
    GUI.depth = 11;
    GUI.skin = BlueStonez.Skin;
    GUI.BeginGroup(new Rect((float) (Screen.width - 700) * 0.5f, (float) (Screen.height - GlobalUIRibbon.Instance.Height() - 480) * 0.5f, 700f, 480f), string.Empty, BlueStonez.window);
    GUI.Label(new Rect(10f, 20f, 670f, 48f), "Explore Maps", BlueStonez.label_interparkbold_48pt);
    GUI.Label(new Rect(30f, 50f, 640f, 120f), LocalizedStrings.TrainingModeDesc, BlueStonez.label_interparkbold_13pt);
    GUI.Box(new Rect(12f, 160f, 670f, 20f), string.Empty, BlueStonez.box_grey50);
    GUI.Label(new Rect(16f, 160f, 120f, 20f), LocalizedStrings.ChooseAMap, BlueStonez.label_interparkbold_18pt_left);
    int height = 280;
    GUI.Box(new Rect(12f, 179f, 670f, (float) height), string.Empty, BlueStonez.window);
    this._mapScroll = GUITools.BeginScrollView(new Rect(0.0f, 179f, 682f, (float) height), this._mapScroll, new Rect(0.0f, 0.0f, 655f, (float) (10 + 80 * Mathf.CeilToInt((float) (Singleton<MapManager>.Instance.Count / 4)))));
    Vector2 v = new Vector2(163f, 80f);
    int num1 = 0;
    foreach (UberstrikeMap allMap in Singleton<MapManager>.Instance.AllMaps)
    {
      if (allMap.IsVisible)
      {
        Color white = Color.white;
        int num2 = num1 / 4;
        Rect rect = new Rect((float) (13.0 + (double) (num1 % 4) * (double) v.Width()), (float) ((double) num2 * (double) v.y + 4.0), v.x, v.y);
        if (GUI.Button(rect, string.Empty, BlueStonez.gray_background) && !GUITools.IsScrolling)
          Singleton<GameStateController>.Instance.CreateGame(allMap, "Training", string.Empty);
        GUI.BeginGroup(rect);
        allMap.Icon.Draw(rect.CenterHorizontally(2f, 100f, 64f));
        Vector2 vector2 = BlueStonez.label_interparkbold_11pt.CalcSize(new GUIContent(allMap.Name));
        GUI.contentColor = white;
        GUI.Label(rect.CenterHorizontally(rect.height - vector2.y, vector2.x, vector2.y), allMap.Name, BlueStonez.label_interparkbold_11pt);
        GUI.contentColor = Color.white;
        GUI.EndGroup();
        ++num1;
      }
    }
    GUITools.EndScrollView();
    GUI.EndGroup();
    GUI.enabled = true;
  }
}
