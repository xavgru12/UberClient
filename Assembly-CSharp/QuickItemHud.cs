// Decompiled with JetBrains decompiler
// Type: QuickItemHud
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class QuickItemHud
{
  private const float EmptyAlpha = 0.3f;
  private const float BackgroundAlpha = 0.3f;
  private const float CooldownAlpha = 1f;
  private const float RechargeAlpha = 0.5f;
  private const int AngleOffset = 0;
  private const int TotalFillAngle = 360;
  private MeshGUICircle _recharge;
  private MeshGUICircle _cooldown;
  private MeshGUIQuad _cooldownFlash;
  private MeshGUIQuad _background;
  private MeshGUIQuad _selection;
  private MeshGUIQuad _icon;
  private MeshGUIText _countText;
  private MeshGUIText _descriptionText;
  private Animatable2DGroup _quickItemGroup;
  private int _amount;
  private float _cooldownTime;
  private float _cooldownMax;
  private float _rechargeTime;
  private float _rechargeTimeMax;
  private bool _isAnimating;
  private bool _isCooliingDown;
  private Vector2 _expandGroupPos;
  private Vector2 _collapseGroupPos;

  public QuickItemHud(string name)
  {
    this.Name = name;
    this._amount = 0;
    MeshGUICircle meshGuiCircle1 = new MeshGUICircle((Texture) ConsumableHudTextures.CircleBlue);
    meshGuiCircle1.Name = name + nameof (Recharging);
    meshGuiCircle1.Alpha = 0.5f;
    meshGuiCircle1.Depth = 2f;
    this._recharge = meshGuiCircle1;
    this._recharge.CircleMesh.StartAngle = 0.0f;
    MeshGUICircle meshGuiCircle2 = new MeshGUICircle((Texture) ConsumableHudTextures.CircleBlue);
    meshGuiCircle2.Name = name + nameof (Cooldown);
    meshGuiCircle2.Alpha = 1f;
    meshGuiCircle2.Depth = 3f;
    this._cooldown = meshGuiCircle2;
    this._cooldown.CircleMesh.StartAngle = 0.0f;
    MeshGUIQuad meshGuiQuad1 = new MeshGUIQuad((Texture) ConsumableHudTextures.CircleWhite, TextAnchor.MiddleCenter);
    meshGuiQuad1.Name = name + "Flash";
    meshGuiQuad1.Alpha = 0.0f;
    meshGuiQuad1.Depth = 3f;
    this._cooldownFlash = meshGuiQuad1;
    MeshGUIQuad meshGuiQuad2 = new MeshGUIQuad((Texture) ConsumableHudTextures.CircleBlue, TextAnchor.MiddleCenter);
    meshGuiQuad2.Name = name + "CooldownBackground";
    meshGuiQuad2.Alpha = 0.3f;
    meshGuiQuad2.Depth = 6f;
    this._background = meshGuiQuad2;
    MeshGUIText meshGuiText1 = new MeshGUIText(this._amount.ToString(), HudAssets.Instance.HelveticaBitmapFont, TextAnchor.LowerCenter);
    meshGuiText1.NamePrefix = name;
    meshGuiText1.Depth = 7f;
    this._countText = meshGuiText1;
    if (!ApplicationDataManager.IsMobile)
    {
      MeshGUIText meshGuiText2 = new MeshGUIText("Key: 7", HudAssets.Instance.HelveticaBitmapFont, TextAnchor.LowerCenter);
      meshGuiText2.NamePrefix = name;
      meshGuiText2.Depth = 8f;
      this._descriptionText = meshGuiText2;
      MeshGUIQuad meshGuiQuad3 = new MeshGUIQuad((Texture) ConsumableHudTextures.CircleBlue, TextAnchor.MiddleCenter);
      meshGuiQuad3.Name = name + "Selection";
      meshGuiQuad3.Depth = 4f;
      this._selection = meshGuiQuad3;
    }
    MeshGUIQuad meshGuiQuad4 = new MeshGUIQuad((Texture) ConsumableHudTextures.AmmoBlue, TextAnchor.MiddleCenter);
    meshGuiQuad4.Name = name + "Icon";
    meshGuiQuad4.Depth = 1f;
    meshGuiQuad4.Alpha = 0.3f;
    this._icon = meshGuiQuad4;
    this._quickItemGroup = new Animatable2DGroup();
    this._quickItemGroup.Group.Add((IAnimatable2D) this._recharge);
    this._quickItemGroup.Group.Add((IAnimatable2D) this._cooldown);
    this._quickItemGroup.Group.Add((IAnimatable2D) this._cooldownFlash);
    this._quickItemGroup.Group.Add((IAnimatable2D) this._background);
    this._quickItemGroup.Group.Add((IAnimatable2D) this._icon);
    this._quickItemGroup.Group.Add((IAnimatable2D) this._countText);
    if (ApplicationDataManager.IsMobile)
      return;
    this._quickItemGroup.Group.Add((IAnimatable2D) this._descriptionText);
    this._quickItemGroup.Group.Add((IAnimatable2D) this._selection);
  }

  public string Name { get; private set; }

  public bool IsEmpty { get; private set; }

  public int Amount
  {
    get => this._amount;
    set
    {
      bool isDecreasing = value < this._amount;
      this._amount = value <= 0 ? 0 : value;
      this.UpdateSpringCountText(isDecreasing);
    }
  }

  public float Cooldown
  {
    set
    {
      if ((double) this._cooldownTime == (double) value)
        return;
      this._cooldownTime = value;
      this.UpdateCooldownAngle();
    }
  }

  public float CooldownMax
  {
    set
    {
      this._cooldownMax = value;
      this.UpdateCooldownAngle();
    }
  }

  public float Recharging
  {
    set
    {
      if ((double) this._rechargeTime == (double) value)
        return;
      this._rechargeTime = value;
      this.UpdateRechargingAngle();
    }
  }

  public float RechargingMax
  {
    set
    {
      this._rechargeTimeMax = value;
      this.UpdateRechargingAngle();
    }
  }

  public Animatable2DGroup Group => this._quickItemGroup;

  public void SetKeyBinding(string binding)
  {
    if (ApplicationDataManager.IsMobile)
      return;
    this._descriptionText.Text = "Key: " + binding;
  }

  public void SetRechargeBarVisible(bool isVisible) => this._recharge.IsEnabled = isVisible;

  public void Expand(Vector2 destPos, float delay = 0)
  {
    this.ResetHud();
    this._expandGroupPos = destPos;
    Vector2 destPosition = new Vector2(this._background.Size.x / 2f, this._background.Size.y + this._countText.Size.y * 1.5f);
    if (this._quickItemGroup.IsVisible)
    {
      this._quickItemGroup.MoveTo(this._expandGroupPos, 0.3f, EaseType.Berp, delay);
      this._descriptionText.FadeAlphaTo(1f, 0.3f, EaseType.In);
      this._descriptionText.MoveTo(destPosition, 0.3f, EaseType.Berp, delay);
    }
    else
    {
      this._quickItemGroup.Position = this._expandGroupPos;
      this._descriptionText.Alpha = 1f;
      this._descriptionText.Position = destPosition;
    }
    this.IsExpanded = true;
  }

  public void Expand(bool moveNext = true)
  {
    this.ResetHud();
    float x = 40f;
    if (moveNext)
      x = -40f;
    this._quickItemGroup.Position = new Vector2(x, this.CollapsedHeight);
    this._quickItemGroup.MoveTo(new Vector2(0.0f, this.CollapsedHeight), 0.3f, EaseType.Berp, 0.0f);
    this._quickItemGroup.FadeAlphaTo(1f, 0.3f);
    this._cooldown.StopFading();
    this._cooldown.Alpha = 0.0f;
    this._cooldownFlash.StopFading();
    this._cooldownFlash.Alpha = 0.0f;
    this.IsExpanded = true;
  }

  public void Collapse(Vector2 destPos, float delay = 0)
  {
    this.ResetHud();
    this._collapseGroupPos = destPos;
    Vector2 destPosition = new Vector2(this._background.Size.x / 2f, this._background.Size.y + 5f);
    if (this._quickItemGroup.IsVisible)
    {
      this._quickItemGroup.MoveTo(this._collapseGroupPos, 0.3f, EaseType.Berp, delay);
      this._descriptionText.FadeAlphaTo(0.0f, 0.3f, EaseType.Out);
      this._descriptionText.MoveTo(destPosition, 0.3f, EaseType.Berp, delay);
    }
    else
    {
      this._quickItemGroup.Position = this._collapseGroupPos;
      this._descriptionText.Alpha = 0.0f;
      this._descriptionText.Position = destPosition;
    }
    this.IsExpanded = false;
  }

  public void Collapse(bool moveNext = true)
  {
    this.ResetHud();
    float x = -40f;
    if (moveNext)
      x = 40f;
    this._quickItemGroup.Position = new Vector2(0.0f, this.CollapsedHeight);
    this._quickItemGroup.MoveTo(new Vector2(x, this.CollapsedHeight), 0.3f, EaseType.Berp, 0.0f);
    this._quickItemGroup.FadeAlphaTo(0.0f, 0.3f);
    this.IsExpanded = false;
  }

  public void SetSelected(bool selected, bool moveNext = true)
  {
    if (this.IsEmpty)
      selected = false;
    if (ApplicationDataManager.IsMobile)
    {
      if (!selected)
        this.Collapse(moveNext);
      else
        this.Expand(moveNext);
    }
    else
      this._selection.IsEnabled = selected;
  }

  public float ExpandedHeight { get; private set; }

  public float CollapsedHeight { get; private set; }

  public bool IsExpanded { get; private set; }

  public void ConfigureEmptySlot()
  {
    this._quickItemGroup.Hide();
    this.IsEmpty = true;
    this.ResetHud();
  }

  public void ConfigureSlot(
    Color textColor,
    Texture rechargingTexture,
    Texture cooldownTexture,
    Texture backgroundTexture,
    Texture selectionTexture,
    Texture2D icon)
  {
    Singleton<HudStyleUtility>.Instance.SetDefaultStyle(this._countText);
    if (!ApplicationDataManager.IsMobile)
      Singleton<HudStyleUtility>.Instance.SetDefaultStyle(this._descriptionText);
    this._recharge.Texture = rechargingTexture;
    this._cooldown.Texture = cooldownTexture;
    this._cooldown.Alpha = 0.0f;
    this._cooldownFlash.Texture = cooldownTexture;
    this._cooldownFlash.Alpha = 0.0f;
    this._background.Texture = backgroundTexture;
    this._background.Alpha = 0.3f;
    this._icon.Texture = (Texture) icon;
    this._countText.BitmapMeshText.ShadowColor = textColor;
    this._countText.BitmapMeshText.AlphaMin = 0.4f;
    if (!ApplicationDataManager.IsMobile)
    {
      this._selection.Texture = selectionTexture;
      this._descriptionText.BitmapMeshText.ShadowColor = textColor;
      this._descriptionText.BitmapMeshText.AlphaMin = 0.4f;
      this._descriptionText.BitmapMeshText.MainColor = this._descriptionText.BitmapMeshText.MainColor.SetAlpha(0.5f);
    }
    this._quickItemGroup.Show();
    this.IsEmpty = false;
    this.ResetHud();
  }

  public void ResetHud()
  {
    this._quickItemGroup.StopScaling();
    this._quickItemGroup.StopMoving();
    this._cooldownFlash.StopScaling();
    this._cooldownFlash.StopFading();
    this._isAnimating = false;
    this.ResetTransform();
  }

  private void ResetTransform()
  {
    Vector2 vector2 = Vector2.one * 0.8f;
    this._background.Scale = vector2;
    this._icon.Scale = vector2 * 0.9f;
    this._recharge.Scale = vector2 * 0.8f;
    this._cooldown.Scale = vector2;
    this._cooldownFlash.Scale = vector2;
    this._countText.Scale = Vector2.one * 0.25f;
    if (!ApplicationDataManager.IsMobile)
    {
      this._selection.Scale = vector2;
      this._descriptionText.Scale = Vector2.one * 0.25f;
    }
    this._icon.Position = this._background.Size / 2f;
    this._recharge.Position = this._background.Size / 2f;
    this._cooldown.Position = this._background.Size / 2f;
    this._cooldownFlash.Position = this._background.Size / 2f;
    this._background.Position = this._background.Size / 2f;
    this._countText.Position = new Vector2(this._background.Size.x * 0.95f, this._background.Size.y + this._countText.Size.y * 0.5f);
    if (!ApplicationDataManager.IsMobile)
      this._selection.Position = this._selection.Size / 2f;
    this.UpdateExpandedHeight();
  }

  private void UpdateCollapsedHeight()
  {
    if (ApplicationDataManager.IsMobile)
      return;
    Vector2 position = this._descriptionText.Position;
    this._descriptionText.Position = new Vector2(this._background.Size.x / 2f, this._background.Size.y + 5f);
    float height = this.Group.Rect.height;
    this._descriptionText.Position = position;
    this.CollapsedHeight = height;
  }

  private void UpdateExpandedHeight()
  {
    this.UpdateCollapsedHeight();
    this.ExpandedHeight = this.CollapsedHeight + this._countText.Size.y;
  }

  private void UpdateSpringCountText(bool isDecreasing)
  {
    if (this._amount <= 0)
    {
      this._icon.Alpha = 0.3f;
      this._cooldown.Alpha = 0.0f;
    }
    else if (!ApplicationDataManager.IsMobile || this.IsExpanded)
      this._icon.Alpha = 1f;
    else
      this._icon.Alpha = 0.0f;
    this._countText.Text = this._amount.ToString();
    if (!isDecreasing)
      return;
    MonoRoutine.Start(this.OnQuickItemDecrement());
  }

  [DebuggerHidden]
  private IEnumerator OnQuickItemDecrement() => (IEnumerator) new QuickItemHud.\u003COnQuickItemDecrement\u003Ec__Iterator4D()
  {
    \u003C\u003Ef__this = this
  };

  private void UpdateCooldownAngle()
  {
    float num = (double) this._cooldownMax != 0.0 ? (float) (((double) this._cooldownMax - (double) this._cooldownTime) * 360.0) / this._cooldownMax : 360f;
    if ((double) num < 360.0)
    {
      if ((double) this._cooldown.Angle == 360.0)
        this.OnCooldownStart();
      if (this._isCooliingDown && this._amount > 0 && (!ApplicationDataManager.IsMobile || this.IsExpanded))
        this._cooldown.Alpha = 1f;
    }
    else
    {
      if ((double) this._cooldown.Angle < 360.0 && this._amount > 0)
        this.OnCooldownFinish();
      if (!this._isCooliingDown)
        this._cooldown.Alpha = 0.0f;
    }
    this._cooldown.Angle = num;
  }

  private void OnCooldownStart() => this._isCooliingDown = true;

  private void OnCooldownFinish()
  {
    this._isCooliingDown = false;
    MonoRoutine.Start(this.DoFlashAnim());
  }

  private void UpdateRechargingAngle() => this._recharge.Angle = (double) this._rechargeTimeMax != 0.0 ? (float) (((double) this._rechargeTimeMax - (double) this._rechargeTime) * 360.0) / this._rechargeTimeMax : 360f;

  [DebuggerHidden]
  private IEnumerator DoFlashAnim() => (IEnumerator) new QuickItemHud.\u003CDoFlashAnim\u003Ec__Iterator4E()
  {
    \u003C\u003Ef__this = this
  };

  private void ResetCooldownFlashView(float alpha)
  {
    if (ApplicationDataManager.IsMobile && !this.IsExpanded)
      alpha = 0.0f;
    this._cooldownFlash.Alpha = alpha;
    this._cooldownFlash.Scale = Vector2.one * 0.8f;
    this._cooldownFlash.Position = this._background.Size / 2f;
  }
}
