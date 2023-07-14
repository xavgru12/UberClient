// Decompiled with JetBrains decompiler
// Type: SceneGuiController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

internal class SceneGuiController : MonoBehaviour
{
  [SerializeField]
  private string _title;
  [SerializeField]
  private PageGUI[] _guiPages;
  [SerializeField]
  private float _width;
  private Rect _rect;
  private GUIContent[] _guiPageTabs;
  private float _offset;
  private int _currentGuiPageIndex;
  private FloatAnim _guiPageAnim;

  private void Awake()
  {
    this._guiPageAnim = new FloatAnim((FloatAnim.OnValueChange) ((oldValue, newValue) =>
    {
      this._offset = newValue;
      Singleton<CameraRectController>.Instance.Width = ((float) Screen.width - this._rect.width + this._offset) / (float) Screen.width;
    }));
    this._guiPageTabs = new GUIContent[this._guiPages.Length];
    for (int index = 0; index < this._guiPages.Length; ++index)
      this._guiPageTabs[index] = new GUIContent(this._guiPages[index].Title);
  }

  private void OnEnable()
  {
    if (this._guiPages.Length > 0)
    {
      this.SetCurrentPage(0);
      this._guiPageAnim.Value = this._rect.width;
      this._guiPageAnim.AnimTo(0.0f, 0.5f, EaseType.In);
    }
    GameState.IsReadyForNextGame = false;
    CmuneEventHandler.AddListener<ScreenResolutionEvent>(new Action<ScreenResolutionEvent>(this.OnScreenResolutionChange));
  }

  private void OnDisable()
  {
    if ((UnityEngine.Object) this._guiPages[this._currentGuiPageIndex] != (UnityEngine.Object) null)
      this._guiPages[this._currentGuiPageIndex].enabled = false;
    this._guiPageAnim.Value = this._rect.width;
    this._currentGuiPageIndex = -1;
    CmuneEventHandler.RemoveListener<ScreenResolutionEvent>(new Action<ScreenResolutionEvent>(this.OnScreenResolutionChange));
  }

  public void Update()
  {
    if (!this._guiPageAnim.IsAnimating)
      return;
    this._guiPageAnim.Update();
  }

  private void OnGUI()
  {
    GUI.depth = 11;
    this._rect.x = (float) Screen.width - this._width + this._offset;
    this._rect.y = (float) GlobalUIRibbon.Instance.Height();
    this._rect.width = this._width;
    this._rect.height = (float) Screen.height - this._rect.y;
    GUI.skin = BlueStonez.Skin;
    GUI.BeginGroup(this._rect, GUIContent.none, BlueStonez.window_standard_grey38);
    GUI.Label(new Rect(0.0f, 0.0f, this._rect.width, 56f), this._title, BlueStonez.tab_strip);
    GUI.changed = false;
    this._currentGuiPageIndex = UnityGUI.Toolbar(new Rect(0.0f, 34f, (float) (140 * this._guiPageTabs.Length), 22f), this._currentGuiPageIndex, this._guiPageTabs, this._guiPageTabs.Length, BlueStonez.tab_medium);
    if (GUI.changed)
    {
      this.SetCurrentPage(this._currentGuiPageIndex);
    }
    else
    {
      GUI.EndGroup();
      this._guiPages[this._currentGuiPageIndex].DrawGUI(new Rect(this._rect.x, this._rect.y + 57f, this._rect.width, this._rect.height - 56f));
      GuiManager.DrawTooltip();
    }
  }

  private void SetCurrentPage(int index)
  {
    for (int index1 = 0; index1 < this._guiPages.Length; ++index1)
    {
      this._guiPages[index1].IsOnGUIEnabled = false;
      this._guiPages[index1].enabled = false;
    }
    if (index < 0 || index >= this._guiPages.Length)
      return;
    this._currentGuiPageIndex = index;
    this._guiPages[this._currentGuiPageIndex].enabled = true;
  }

  private void OnScreenResolutionChange(ScreenResolutionEvent ev) => Singleton<CameraRectController>.Instance.Width = ((float) Screen.width - this._rect.width + this._offset) / (float) Screen.width;

  public void SetShopArea()
  {
    for (int index = 0; index < this._guiPages.Length; ++index)
    {
      if (this._guiPages[index] is ShopPageGUI)
      {
        this.SetCurrentPage(index);
        break;
      }
    }
  }
}
