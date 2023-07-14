// Decompiled with JetBrains decompiler
// Type: LocalShotFeedbackHud
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class LocalShotFeedbackHud : Singleton<LocalShotFeedbackHud>
{
  private Animatable2DGroup _textGroup;

  private LocalShotFeedbackHud() => this._textGroup = new Animatable2DGroup();

  public void Update() => this._textGroup.Draw(0.0f, 0.0f);

  public void DisplayLocalShotFeedback(InGameEventFeedbackType type)
  {
    MeshGUIText meshGuiText = new MeshGUIText(string.Empty, HudAssets.Instance.InterparkBitmapFont, TextAnchor.MiddleCenter);
    AudioClip sound = (AudioClip) null;
    switch (type)
    {
      case InGameEventFeedbackType.HeadShot:
        meshGuiText.Text = "Head Shot";
        sound = GameAudio.HeadShot;
        break;
      case InGameEventFeedbackType.NutShot:
        meshGuiText.Text = "Nut Shot";
        sound = GameAudio.NutShot;
        break;
      case InGameEventFeedbackType.Humiliation:
        meshGuiText.Text = "Smackdown";
        sound = GameAudio.Smackdown;
        break;
    }
    this.ResetTransform(meshGuiText);
    this.ResetStyle(meshGuiText);
    Singleton<InGameFeatHud>.Instance.AnimationScheduler.EnqueueAnim((IAnim) new LocalShotFeedbackAnim(this._textGroup, meshGuiText, 1f, 1f, sound));
  }

  private void ResetTransform(MeshGUIText text)
  {
    float num = Singleton<InGameFeatHud>.Instance.TextHeight / text.TextBounds.y;
    text.Scale = new Vector2(num, num);
    text.Position = Singleton<InGameFeatHud>.Instance.AnchorPoint;
  }

  private void ResetStyle(MeshGUIText text)
  {
    Singleton<HudStyleUtility>.Instance.SetNoShadowStyle(text);
    text.Alpha = 0.0f;
    text.ShadowColorAnim.Alpha = 0.0f;
  }
}
