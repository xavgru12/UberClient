// Decompiled with JetBrains decompiler
// Type: MeshGUIList
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class MeshGUIList
{
  private float _sizeAttenuationFactor;
  private float _alphaAttenuationFactor;
  private float _gapBetweenItems = 1f;
  private float _animTime = 0.2f;
  private int _destIndex;
  private bool _isCircular;
  private int _maxUnilateralSlotCount;
  private int _currentDisplayIndex;
  private Animatable2DGroup _listItemsGroup;
  private MeshGUIQuad _glowBlur;
  private Animatable2DGroup _entireGroup;
  private Animatable2DGroup _currentVisibleGroup;
  private Action _onUpdate;
  private float _scaleFactor;
  private float _itemHeight;

  public MeshGUIList(Action onDrawMeshGUIList = null)
  {
    this._maxUnilateralSlotCount = 1;
    this._onUpdate = onDrawMeshGUIList;
    this._listItemsGroup = new Animatable2DGroup();
    this._glowBlur = new MeshGUIQuad((Texture) HudTextures.WhiteBlur128);
    this._glowBlur.Name = "MeshGUIList Glow";
    this._glowBlur.Depth = 2f;
    this._glowBlur.Color = HudStyleUtility.GLOW_BLUR_BLUE_COLOR;
    this._entireGroup = new Animatable2DGroup();
    this._entireGroup.Group.Add((IAnimatable2D) this._listItemsGroup);
    this._entireGroup.Group.Add((IAnimatable2D) this._glowBlur);
    this._currentVisibleGroup = new Animatable2DGroup();
    this._currentDisplayIndex = 0;
    this._sizeAttenuationFactor = 0.7f;
    this._alphaAttenuationFactor = 0.5f;
    this._isCircular = true;
    this.ResetHud();
    this.Enabled = false;
    CmuneEventHandler.AddListener<OnSetPlayerTeamEvent>(new Action<OnSetPlayerTeamEvent>(this.OnTeamChange));
    CmuneEventHandler.AddListener<ScreenResolutionEvent>(new Action<ScreenResolutionEvent>(this.OnScreenResolutionChange));
  }

  public bool Enabled
  {
    get => this._entireGroup.IsVisible;
    set
    {
      if (value)
        this._entireGroup.Show();
      else
        this._entireGroup.Hide();
    }
  }

  public void Draw() => this._entireGroup.Draw(0.0f, 0.0f);

  public void Update()
  {
    this._entireGroup.Draw(0.0f, 0.0f);
    if (this._onUpdate == null)
      return;
    this._onUpdate();
  }

  public bool HasItem(int index) => index >= 0 && index < this._listItemsGroup.Group.Count;

  public void SetItemText(int index, string text)
  {
    if (this.HasItem(index))
      (this._listItemsGroup.Group[index] as MeshGUIText).Text = text;
    this.StopAnimToIndexCoroutine();
  }

  public void InsertItem(int index, string text)
  {
    MeshGUIText listItem = this.CreateListItem(text);
    this._listItemsGroup.Group.Insert(index, (IAnimatable2D) listItem);
    this.StopAnimToIndexCoroutine();
  }

  public void AddItem(string text)
  {
    this._listItemsGroup.Group.Add((IAnimatable2D) this.CreateListItem(text));
    this.StopAnimToIndexCoroutine();
  }

  public void RemoveItem(int index)
  {
    this._listItemsGroup.RemoveAndFree(index);
    this.StopAnimToIndexCoroutine();
  }

  public void ClearAllItems()
  {
    this._listItemsGroup.ClearAndFree();
    this.StopAnimToIndexCoroutine();
  }

  public void FadeOut(float time, EaseType easeType) => this._entireGroup.FadeAlphaTo(0.0f, time, easeType);

  public void AnimUpward()
  {
    this.StopAnimToIndexCoroutine();
    if (this._currentDisplayIndex > 0)
    {
      --this._currentDisplayIndex;
      this.UpdateGroupDisplay(this._currentDisplayIndex, this._animTime);
    }
    else
    {
      if (!this._isCircular)
        return;
      this._currentDisplayIndex = this._listItemsGroup.Group.Count - 1;
      this.UpdateGroupDisplay(this._currentDisplayIndex, this._animTime);
    }
  }

  public void AnimDownward()
  {
    this.StopAnimToIndexCoroutine();
    if (this._currentDisplayIndex < this._listItemsGroup.Group.Count - 1)
    {
      ++this._currentDisplayIndex;
      this.UpdateGroupDisplay(this._currentDisplayIndex, this._animTime);
    }
    else
    {
      if (!this._isCircular)
        return;
      this._currentDisplayIndex = 0;
      this.UpdateGroupDisplay(this._currentDisplayIndex, this._animTime);
    }
  }

  public void AnimToIndex(int destIndex, float time)
  {
    this._destIndex = destIndex;
    this._animTime = time;
    this.UpdateGroupDisplay(this._currentDisplayIndex);
    MonoRoutine.Start(this.AnimToIndexCoroutine());
  }

  private void StopAnimToIndexCoroutine()
  {
    Singleton<PreemptiveCoroutineManager>.Instance.IncrementId(new PreemptiveCoroutineManager.CoroutineFunction(this.AnimToIndexCoroutine));
    this.UpdateGroupDisplay(this._currentDisplayIndex);
  }

  [DebuggerHidden]
  private IEnumerator AnimToIndexCoroutine() => (IEnumerator) new MeshGUIList.\u003CAnimToIndexCoroutine\u003Ec__Iterator49()
  {
    \u003C\u003Ef__this = this
  };

  private void ResetHud()
  {
    this.ResetStyle();
    this.ResetTransform();
  }

  private void ResetStyle()
  {
    foreach (IAnimatable2D meshText3D in this._listItemsGroup.Group)
      Singleton<HudStyleUtility>.Instance.SetTeamStyle(meshText3D as MeshGUIText);
  }

  private void OnTeamChange(OnSetPlayerTeamEvent ev)
  {
    this.ResetStyle();
    if (ev.TeamId == TeamID.RED)
      this._glowBlur.Color = HudStyleUtility.GLOW_BLUR_RED_COLOR;
    else
      this._glowBlur.Color = HudStyleUtility.GLOW_BLUR_BLUE_COLOR;
    this.UpdateGroupDisplay(this._currentDisplayIndex);
  }

  private void OnScreenResolutionChange(ScreenResolutionEvent ev) => this.ResetTransform();

  private void ResetTransform()
  {
    this._scaleFactor = 0.45f;
    this._itemHeight = (float) Screen.height * 0.055f;
    this.UpdateGroupDisplay(this._currentDisplayIndex);
    this._entireGroup.Position = new Vector2((float) (Screen.width / 2), (float) Screen.height * 0.74f);
  }

  private MeshGUIText CreateListItem(string text)
  {
    MeshGUIText meshText3D = new MeshGUIText(text, HudAssets.Instance.InterparkBitmapFont);
    Singleton<HudStyleUtility>.Instance.SetTeamStyle(meshText3D);
    meshText3D.Alpha = 0.0f;
    if (!this.Enabled)
      meshText3D.Hide();
    return meshText3D;
  }

  private float GetItemYOffset(float itemHeight, int slotIndex)
  {
    float itemYoffset = 0.0f;
    if (slotIndex == 0)
      itemYoffset = (float) (-(double) itemHeight / 2.0);
    else if (slotIndex > 0)
    {
      itemYoffset = itemHeight / 2f + this._gapBetweenItems;
      for (int index = 2; index < slotIndex + 1; ++index)
      {
        float num = itemHeight * Mathf.Pow(this._sizeAttenuationFactor, (float) (index - 1));
        itemYoffset += (float) ((double) num + (double) this._gapBetweenItems - (double) index * 2.0 * (double) this._scaleFactor);
      }
    }
    else if (slotIndex < 0)
    {
      itemYoffset = (float) (-(double) itemHeight / 2.0);
      for (int p = 1; p < -slotIndex + 1; ++p)
      {
        float num = itemHeight * Mathf.Pow(this._sizeAttenuationFactor, (float) p);
        itemYoffset -= (float) ((double) num + (double) this._gapBetweenItems - (double) p * 2.0 * (double) this._scaleFactor);
      }
    }
    return itemYoffset;
  }

  private void UpdateGroupDisplay(int currentDisplayIndex, float time = 0.0f)
  {
    this.ResetListGroupDisplay(currentDisplayIndex, time);
    this.ResetBlurDisplay();
  }

  private void ResetListGroupDisplay(int currentDisplayIndex, float time = 0.0f)
  {
    List<int> intList = new List<int>();
    for (int index = 0; index < this._listItemsGroup.Group.Count; ++index)
    {
      int num = index - currentDisplayIndex;
      if (this._isCircular)
      {
        if (num < -this._maxUnilateralSlotCount)
          num += this._listItemsGroup.Group.Count;
        else if (num > this._maxUnilateralSlotCount)
          num -= this._listItemsGroup.Group.Count;
      }
      intList.Add(num);
    }
    for (int index = 0; index < this._listItemsGroup.Group.Count; ++index)
    {
      MeshGUIText meshGuiText = (MeshGUIText) this._listItemsGroup.Group[index];
      int slotIndex = intList[index];
      int p = slotIndex <= 0 ? -slotIndex : slotIndex;
      float destAlpha = slotIndex > this._maxUnilateralSlotCount || slotIndex < -this._maxUnilateralSlotCount ? 0.0f : 1f * Mathf.Pow(this._alphaAttenuationFactor, (float) p);
      float num = 1f * Mathf.Pow(this._sizeAttenuationFactor, (float) p) * this._scaleFactor;
      Vector2 destScale = new Vector2(num, num);
      Vector2 destPosition = new Vector2((float) (-(double) meshGuiText.TextBounds.x * (double) num / 2.0), this.GetItemYOffset(this._itemHeight, slotIndex));
      meshGuiText.StopFading();
      meshGuiText.StopMoving();
      meshGuiText.StopScaling();
      meshGuiText.FadeAlphaTo(destAlpha, time, EaseType.Out);
      meshGuiText.MoveTo(destPosition, time, EaseType.Out, 0.0f);
      meshGuiText.ScaleTo(destScale, time, EaseType.Out);
    }
  }

  private void ResetBlurDisplay()
  {
    this._currentVisibleGroup.Group.Clear();
    foreach (IAnimatable2D animatable2D in this._listItemsGroup.Group)
    {
      MeshGUIText meshGuiText = animatable2D as MeshGUIText;
      if ((double) meshGuiText.Alpha > 0.0)
        this._currentVisibleGroup.Group.Add((IAnimatable2D) meshGuiText);
    }
    float num1 = this._currentVisibleGroup.Rect.width * HudStyleUtility.BLUR_WIDTH_SCALE_FACTOR;
    float num2 = this._currentVisibleGroup.Rect.height * HudStyleUtility.BLUR_HEIGHT_SCALE_FACTOR;
    this._glowBlur.Scale = new Vector2(num1 / (float) HudTextures.WhiteBlur128.width, num2 / (float) HudTextures.WhiteBlur128.height);
    this._glowBlur.Position = new Vector2((float) (-(double) num1 / 2.0), (float) (-(double) num2 / 2.0));
    this._glowBlur.StopFading();
    this._glowBlur.Alpha = 1f;
  }

  private int CirculateSpriteIndex(int index)
  {
    if (index < 0)
      index += this._listItemsGroup.Group.Count;
    else if (index >= this._listItemsGroup.Group.Count)
      index -= this._listItemsGroup.Group.Count;
    return index;
  }
}
