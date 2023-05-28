// Decompiled with JetBrains decompiler
// Type: HudStyleUtility
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

internal class HudStyleUtility : Singleton<HudStyleUtility>
{
  public static Color DEFAULT_BLUE_COLOR = new Color(0.1882353f, 0.5764706f, 0.7254902f);
  public static Color DEFAULT_RED_COLOR = new Color(0.670588255f, 0.180392161f, 0.137254909f);
  public static float BLUR_WIDTH_SCALE_FACTOR = 3f;
  public static float BLUR_HEIGHT_SCALE_FACTOR = 3.2f;
  public static float XP_BAR_WIDTH_PROPORTION_IN_SCREEN = 0.15f;
  public static float ACRONYM_TEXT_SCALE = 0.25f;
  public static float BIGGER_DIGITS_SCALE = 1f;
  public static float SMALLER_DIGITS_SCALE = 0.7f;
  public static float GAP_BETWEEN_TEXT = 3f;
  public static Color GLOW_BLUR_BLUE_COLOR = new Color(0.0f, 0.623529434f, 1f);
  public static Color GLOW_BLUR_RED_COLOR = new Color(0.721568644f, 0.196078435f, 0.168627456f);

  private HudStyleUtility() => CmuneEventHandler.AddListener<OnSetPlayerTeamEvent>(new Action<OnSetPlayerTeamEvent>(this.OnTeamChange));

  public Color TeamTextColor { get; private set; }

  public Color TeamGlowColor { get; private set; }

  public void SetTeamStyle(MeshGUIText meshText3D)
  {
    this.SetDefaultStyle(meshText3D);
    meshText3D.BitmapMeshText.ShadowColor = this.TeamTextColor;
  }

  public void SetDefaultStyle(MeshGUIText meshText3D)
  {
    meshText3D.Color = Color.white;
    meshText3D.BitmapMeshText.ShadowColor = HudStyleUtility.DEFAULT_BLUE_COLOR;
    meshText3D.BitmapMeshText.AlphaMin = 0.45f;
    meshText3D.BitmapMeshText.AlphaMax = 0.62f;
    meshText3D.BitmapMeshText.ShadowAlphaMin = 0.2f;
    meshText3D.BitmapMeshText.ShadowAlphaMax = 0.45f;
    meshText3D.BitmapMeshText.ShadowOffsetU = 0.0f;
    meshText3D.BitmapMeshText.ShadowOffsetV = 0.0f;
  }

  public void SetSamllTextStyle(MeshGUIText meshText3D)
  {
    meshText3D.Color = Color.white;
    meshText3D.BitmapMeshText.ShadowColor = HudStyleUtility.DEFAULT_BLUE_COLOR;
    meshText3D.BitmapMeshText.AlphaMin = 0.4f;
    meshText3D.BitmapMeshText.AlphaMax = 0.62f;
    meshText3D.BitmapMeshText.ShadowAlphaMin = 0.2f;
    meshText3D.BitmapMeshText.ShadowAlphaMax = 0.45f;
    meshText3D.BitmapMeshText.ShadowOffsetU = 0.0f;
    meshText3D.BitmapMeshText.ShadowOffsetV = 0.0f;
  }

  public void SetBlueStyle(MeshGUIText meshText3D) => this.SetDefaultStyle(meshText3D);

  public void SetRedStyle(MeshGUIText meshText3D)
  {
    this.SetDefaultStyle(meshText3D);
    meshText3D.BitmapMeshText.ShadowColor = HudStyleUtility.DEFAULT_RED_COLOR;
  }

  public void SetNoShadowStyle(MeshGUIText meshText3D)
  {
    meshText3D.Color = Color.white;
    meshText3D.BitmapMeshText.ShadowColor = new Color(1f, 1f, 1f, 0.0f);
    meshText3D.BitmapMeshText.AlphaMin = 0.18f;
    meshText3D.BitmapMeshText.AlphaMax = 0.62f;
    meshText3D.BitmapMeshText.ShadowAlphaMin = 0.18f;
    meshText3D.BitmapMeshText.ShadowAlphaMax = 0.39f;
    meshText3D.BitmapMeshText.ShadowOffsetU = 0.0f;
    meshText3D.BitmapMeshText.ShadowOffsetV = 0.0f;
  }

  public void SetBlackStyle(MeshGUIText meshText3D)
  {
    meshText3D.Color = Color.white;
    meshText3D.BitmapMeshText.ShadowColor = Color.black;
    meshText3D.BitmapMeshText.AlphaMin = 0.45f;
    meshText3D.BitmapMeshText.AlphaMax = 0.62f;
    meshText3D.BitmapMeshText.ShadowAlphaMin = 0.2f;
    meshText3D.BitmapMeshText.ShadowAlphaMax = 0.45f;
    meshText3D.BitmapMeshText.ShadowOffsetU = 0.0f;
    meshText3D.BitmapMeshText.ShadowOffsetV = 0.0f;
  }

  public void OnTeamChange(OnSetPlayerTeamEvent ev)
  {
    this.TeamTextColor = Color.white;
    switch (ev.TeamId)
    {
      case TeamID.NONE:
      case TeamID.BLUE:
        this.TeamTextColor = HudStyleUtility.DEFAULT_BLUE_COLOR;
        this.TeamGlowColor = HudStyleUtility.GLOW_BLUR_BLUE_COLOR;
        break;
      case TeamID.RED:
        this.TeamTextColor = HudStyleUtility.DEFAULT_RED_COLOR;
        this.TeamGlowColor = HudStyleUtility.GLOW_BLUR_RED_COLOR;
        break;
    }
  }

  public void ResetOverlayBoxTransform(Sprite2DGUI boxOverlay, Rect rect, Vector2 scaleFactor)
  {
    float num1 = rect.width * scaleFactor.x;
    float num2 = rect.height * scaleFactor.y;
    Vector2 vector2 = new Vector2(num1 / boxOverlay.GUIBounds.x, num2 / boxOverlay.GUIBounds.y);
    boxOverlay.Scale = vector2;
    boxOverlay.Position = new Vector2(rect.x - num1 / 2f, rect.y - num2 / 2f);
  }
}
