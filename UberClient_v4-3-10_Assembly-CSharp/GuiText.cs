// Decompiled with JetBrains decompiler
// Type: GuiText
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class GuiText : MonoBehaviour
{
  [SerializeField]
  private Font _font;
  [SerializeField]
  private string _text;
  [SerializeField]
  private Color _color;
  [SerializeField]
  private Vector3 _offset;
  [SerializeField]
  private Transform _target;
  [SerializeField]
  private bool _hasTimeLimit;
  [SerializeField]
  private float _distanceCap = -1f;
  private GUIText _guiText;
  private Transform _transform;
  private Material _material;
  private float _visibleTime;
  private bool _isVisible = true;

  private void Awake() => this._transform = this.transform;

  private void Start()
  {
    this._guiText = this.gameObject.AddComponent(typeof (GUIText)) as GUIText;
    this._guiText.alignment = TextAlignment.Center;
    this._guiText.anchor = TextAnchor.MiddleCenter;
    this._guiText.font = this._font;
    this._guiText.text = this._text;
    this._guiText.material = this._font.material;
    this._material = this._guiText.material;
  }

  private void LateUpdate()
  {
    if (!((Object) Camera.main != (Object) null) || !this._isVisible)
      return;
    Vector3 viewportPoint = Camera.main.WorldToViewportPoint(this._target.localPosition + this._offset);
    this._transform.position = viewportPoint;
    if (this._hasTimeLimit)
    {
      this._visibleTime -= Time.deltaTime;
      if ((double) this._visibleTime > 0.0)
      {
        this._color.a = this._visibleTime;
        this._material.color = this._color;
      }
      else
        this._guiText.enabled = false;
    }
    else
    {
      if ((double) this._distanceCap > 0.0)
        this._color.a = 1f - Mathf.Clamp01(viewportPoint.z / this._distanceCap);
      this._material.color = this._color;
    }
  }

  public void ShowText(int seconds) => this._visibleTime = (float) seconds;

  public void ShowText() => this.ShowText(5);

  public bool IsTextVisible
  {
    get => this._isVisible;
    set
    {
      if (this._isVisible == value)
        return;
      this._isVisible = value;
      this._guiText.enabled = value;
    }
  }
}
