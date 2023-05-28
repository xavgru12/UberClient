// Decompiled with JetBrains decompiler
// Type: PickupNameHud
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class PickupNameHud : Singleton<PickupNameHud>
{
  private float _curScaleFactor;
  private MeshGUIText _pickUpText;
  private PickUpMessageType _lastPickUpType;
  private int _samePickUpCount;
  private TemporaryDisplayAnim _displayAnim;

  private PickupNameHud()
  {
    this._pickUpText = new MeshGUIText(string.Empty, HudAssets.Instance.InterparkBitmapFont, TextAnchor.MiddleCenter);
    this._pickUpText.NamePrefix = "Pickup";
    this._displayAnim = new TemporaryDisplayAnim((IAnimatable2D) this._pickUpText, 2f, 1f);
    this.ResetHud();
    this.Enabled = true;
  }

  public bool Enabled
  {
    get => this._pickUpText.IsVisible;
    set
    {
      if (this._pickUpText.IsVisible == value)
        return;
      if (value)
        this._pickUpText.Show();
      else
        this._pickUpText.Hide();
      this._displayAnim.Stop();
    }
  }

  public void Draw()
  {
  }

  public void Update()
  {
    this._displayAnim.Update();
    this._pickUpText.Draw(0.0f, 0.0f);
    this._pickUpText.ShadowColorAnim.Alpha = 0.0f;
  }

  public void DisplayPickupName(string itemName, PickUpMessageType pickupItem)
  {
    if (!this.IsSupportedPickupType(pickupItem))
      return;
    this.OnPickupItem(itemName, pickupItem);
  }

  private void ResetHud()
  {
    this.ResetStyle();
    this.ResetTransform();
  }

  private void ResetStyle()
  {
    Singleton<HudStyleUtility>.Instance.SetNoShadowStyle(this._pickUpText);
    this._pickUpText.Color = ColorConverter.RgbToColor((float) byte.MaxValue, 248f, 192f);
    this._pickUpText.ShadowColorAnim.Alpha = 0.0f;
  }

  private void ResetTransform()
  {
    this._curScaleFactor = 0.45f;
    this._pickUpText.Scale = new Vector2(this._curScaleFactor, this._curScaleFactor);
    this._pickUpText.Position = new Vector2((float) (Screen.width / 2), (float) Screen.height * 0.6f);
  }

  private bool IsSupportedPickupType(PickUpMessageType selectedItem) => selectedItem != PickUpMessageType.None;

  private void OnPickupItem(string itemName, PickUpMessageType pickupItem)
  {
    if (this.IsComboPickup(pickupItem))
    {
      ++this._samePickUpCount;
      this._pickUpText.Text = this.GetComboPickupString(itemName);
    }
    else
    {
      this._pickUpText.Text = itemName;
      this._samePickUpCount = 1;
    }
    this._lastPickUpType = pickupItem;
    if (this._displayAnim.IsAnimating)
      this._displayAnim.Stop();
    this.ResetStyle();
    this._displayAnim.Start();
  }

  private string GetComboPickupString(string itemName) => string.Format("{0} x {1}", (object) itemName, (object) this._samePickUpCount.ToString());

  private bool IsComboPickup(PickUpMessageType selectedItem) => this._lastPickUpType == selectedItem && this._displayAnim.IsAnimating && this._samePickUpCount > 0;
}
