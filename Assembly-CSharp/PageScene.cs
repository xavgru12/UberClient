// Decompiled with JetBrains decompiler
// Type: PageScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public abstract class PageScene : MonoBehaviour
{
  [SerializeField]
  protected Vector3 _mouseOrbitConfig;
  [SerializeField]
  private Vector3 _mouseOrbitPivot;
  [SerializeField]
  protected Transform _avatarAnchor;
  [SerializeField]
  protected int _guiWidth = -1;
  [SerializeField]
  private bool _haveMouseOrbitCamera;

  public bool HaveMouseOrbitCamera => this._haveMouseOrbitCamera;

  public int GuiWidth => this._guiWidth;

  public Transform AvatarAnchor => this._avatarAnchor;

  public Vector3 MouseOrbitConfig => this._mouseOrbitConfig;

  public Vector3 MouseOrbitPivot => this._mouseOrbitPivot;

  public abstract PageType PageType { get; }

  public bool IsEnabled => this.gameObject.activeSelf;

  private void Awake()
  {
    this._mouseOrbitConfig.x = (float) (((double) this._mouseOrbitConfig.x + 360.0) % 360.0);
    this._mouseOrbitConfig.y = (float) (((double) this._mouseOrbitConfig.y + 360.0) % 360.0);
  }

  public void Load()
  {
    this.gameObject.SetActive(true);
    this.OnLoad();
  }

  public void Unload()
  {
    this.gameObject.SetActive(false);
    this.OnUnload();
  }

  protected virtual void OnLoad()
  {
  }

  protected virtual void OnUnload()
  {
  }
}
