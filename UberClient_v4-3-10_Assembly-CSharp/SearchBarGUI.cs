// Decompiled with JetBrains decompiler
// Type: SearchBarGUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

internal class SearchBarGUI
{
  private string _guiName;

  public SearchBarGUI(string name)
  {
    this._guiName = name;
    this.FilterText = string.Empty;
  }

  public string FilterText { get; private set; }

  public bool IsSearching => !string.IsNullOrEmpty(this.FilterText);

  public void Draw(Rect rect)
  {
    GUI.SetNextControlName(this._guiName);
    int num = 20;
    if (ApplicationDataManager.IsMobile)
      num = 30;
    this.FilterText = GUI.TextField(new Rect(rect.x, rect.y, !this.IsSearching ? rect.width : (float) ((double) rect.width - (double) num - 2.0), rect.height), this.FilterText, BlueStonez.textField);
    if (string.IsNullOrEmpty(this.FilterText) && GUI.GetNameOfFocusedControl() != this._guiName)
    {
      GUI.color = new Color(1f, 1f, 1f, 0.3f);
      GUI.Label(rect, " " + LocalizedStrings.Search, BlueStonez.label_interparkbold_11pt_left);
      GUI.color = Color.white;
    }
    if (!this.IsSearching || !GUITools.Button(new Rect(rect.x + rect.width - (float) num, 8f, (float) num, (float) num), new GUIContent("x"), BlueStonez.buttondark_medium))
      return;
    this.ClearFilter();
    GUIUtility.hotControl = 1;
  }

  public void ClearFilter() => this.FilterText = string.Empty;

  public bool CheckIfPassFilter(string text) => !this.IsSearching || text.ToLower().Contains(this.FilterText.ToLower());
}
