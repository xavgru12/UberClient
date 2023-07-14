// Decompiled with JetBrains decompiler
// Type: HudAssets
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class HudAssets : MonoBehaviour
{
  [SerializeField]
  private BitmapFont _interparkBitmapFont;
  [SerializeField]
  private BitmapFont _helveticaBitmapFont;

  public static HudAssets Instance { get; private set; }

  public static bool Exists => (Object) HudAssets.Instance != (Object) null;

  private void Awake() => HudAssets.Instance = this;

  public BitmapFont InterparkBitmapFont => this._interparkBitmapFont;

  public BitmapFont HelveticaBitmapFont => this._helveticaBitmapFont;
}
