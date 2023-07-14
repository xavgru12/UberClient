// Decompiled with JetBrains decompiler
// Type: TutorialWaypoint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class TutorialWaypoint : MonoBehaviour
{
  [SerializeField]
  private Texture ImgWaypoint;
  private Transform _transform;
  private Sprite2D _imgWaypoint;
  private MeshGUIText _txtDistance;
  private MeshGUIText _txtTitle;
  private Vector2 _imgPos;
  private Vector2 _txtPos;
  private Vector2 _disPos;
  [SerializeField]
  private string _text = string.Empty;
  private bool _canShow;

  public bool CanShow
  {
    get => this._canShow;
    set
    {
      this._canShow = value;
      if (this._canShow)
      {
        this._imgWaypoint.Flicker(0.5f, 0.02f);
        this._imgWaypoint.Scale = Vector2.one;
        this._imgWaypoint.FadeAlphaTo(1f, 0.5f);
        if (this._txtDistance != null)
        {
          this._txtDistance.Show();
          this._txtDistance.Flicker(0.5f, 0.02f);
          this._txtDistance.FadeAlphaTo(1f, 0.5f);
        }
        if (this._txtTitle == null)
          return;
        this._txtTitle.Show();
        this._txtTitle.Flicker(0.5f, 0.02f);
        this._txtTitle.FadeAlphaTo(1f, 0.5f);
      }
      else
      {
        this._imgWaypoint.Flicker(0.5f, 0.02f);
        this._imgWaypoint.FadeAlphaTo(0.0f, 0.5f);
        if (this._txtTitle != null)
        {
          this._txtTitle.Alpha = 0.0f;
          this._txtTitle.Hide();
        }
        if (this._txtDistance == null)
          return;
        this._txtDistance.Alpha = 0.0f;
        this._txtDistance.Hide();
      }
    }
  }

  public string Text
  {
    get => this._text;
    set => this._text = value;
  }

  private void Awake()
  {
    this._transform = this.transform;
    this._imgWaypoint = new Sprite2D(this.ImgWaypoint);
    this._imgWaypoint.Alpha = 0.0f;
  }

  private void OnGUI()
  {
    if (!this.CanShow && (double) this._imgWaypoint.Alpha <= 0.10000000149011612 || !(bool) (Object) LevelCamera.Instance.MainCamera || !GameState.HasCurrentPlayer)
      return;
    this.DrawWaypoint2();
  }

  private void DrawWaypoint2()
  {
    Vector3 screenPoint = LevelCamera.Instance.MainCamera.WorldToScreenPoint(this._transform.position);
    screenPoint.y = (float) Screen.height - screenPoint.y;
    bool toLeft = true;
    if ((double) screenPoint.z > 0.0)
      this.CalcWaypointPos(screenPoint, toLeft, out this._txtPos, out this._imgPos, out this._disPos);
    GUI.depth = 100;
    int num = Mathf.RoundToInt(Vector3.Distance(this._transform.position, GameState.LocalCharacter.Position));
    this._imgWaypoint.Draw(this._imgPos.x, this._imgPos.y);
    if (this._txtDistance == null)
    {
      if (!LevelTutorial.Exists)
        return;
      this._txtDistance = new MeshGUIText(string.Empty, LevelTutorial.Instance.Font);
      this._txtDistance.Scale = new Vector2(0.2f, 0.2f);
      this._txtDistance.Color = Color.white;
      this._txtDistance.Alpha = 0.0f;
      Singleton<HudStyleUtility>.Instance.SetDefaultStyle(this._txtDistance);
      this._txtTitle = new MeshGUIText(this.Text, LevelTutorial.Instance.Font);
      this._txtTitle.Scale = new Vector2(0.2f, 0.2f);
      this._txtTitle.Color = Color.white;
      this._txtTitle.Alpha = 0.0f;
      Singleton<HudStyleUtility>.Instance.SetDefaultStyle(this._txtTitle);
    }
    else
    {
      this._txtDistance.Show();
      this._txtDistance.Position = this._disPos;
      this._txtDistance.Text = num.ToString() + "M";
      this._txtTitle.Show();
      this._txtTitle.Position = this._txtPos;
    }
  }

  private void CalcWaypointPos(
    Vector3 screenPos,
    bool toLeft,
    out Vector2 textPos,
    out Vector2 imgPos,
    out Vector2 distancePos)
  {
    float num1 = screenPos.x - this._imgWaypoint.Size.x / 2f;
    float a = screenPos.x + this._imgWaypoint.Size.x / 2f;
    float y = screenPos.y - this._imgWaypoint.Size.y / 2f;
    float num2 = screenPos.y + this._imgWaypoint.Size.y / 2f;
    float num3;
    if (this._txtTitle != null)
    {
      num1 = Mathf.Min(num1, screenPos.x - this._txtTitle.Size.x / 2f);
      num3 = screenPos.x + Mathf.Max(this._txtTitle.Size.x / 2f, this._txtDistance == null ? this._imgWaypoint.Size.x / 2f : this._txtDistance.Size.x + 20f);
      y -= this._txtTitle.Size.y;
    }
    else
      num3 = Mathf.Max(a, this._txtDistance == null ? this._imgWaypoint.Size.x / 2f : this._txtDistance.Size.x + 50f);
    textPos = new Vector2(num1, y);
    imgPos = new Vector2((float) Mathf.RoundToInt(screenPos.x - this._imgWaypoint.Size.x / 2f), (float) Mathf.RoundToInt(screenPos.y - this._imgWaypoint.Size.y / 2f));
    distancePos = imgPos + new Vector2(48f, 0.0f);
    if ((double) screenPos.z > 0.0)
    {
      if ((double) num1 < 0.0)
      {
        imgPos.x -= num1;
        textPos.x -= num1;
        distancePos.x -= num1;
      }
      if ((double) num3 > (double) Screen.width)
      {
        imgPos.x -= num3 - (float) Screen.width;
        textPos.x -= num3 - (float) Screen.width;
        distancePos.x -= num3 - (float) Screen.width;
      }
    }
    if ((double) y < 0.0)
    {
      imgPos.y -= y;
      textPos.y -= y;
      distancePos.y -= y;
    }
    if ((double) num2 <= (double) Screen.height)
      return;
    imgPos.y -= num2 - (float) Screen.height;
    textPos.y -= num2 - (float) Screen.height;
    distancePos.y -= num2 - (float) Screen.height;
  }

  [DebuggerHidden]
  private IEnumerator StartHideMe(float sec) => (IEnumerator) new TutorialWaypoint.\u003CStartHideMe\u003Ec__Iterator42()
  {
    sec = sec,
    \u003C\u0024\u003Esec = sec,
    \u003C\u003Ef__this = this
  };
}
