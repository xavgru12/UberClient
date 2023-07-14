// Decompiled with JetBrains decompiler
// Type: WeaponCamera
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

[RequireComponent(typeof (Camera))]
public class WeaponCamera : MonoBehaviour
{
  private const float RESET_VELOCITY = 5f;
  private const float LERP_DURATION = 0.1f;
  [SerializeField]
  private float _maxDisplacementDelta = 0.4f;
  [SerializeField]
  private float _maxDisplacement = 0.8f;
  private Transform _transform;
  public Vector2 _currentAngle = Vector2.zero;

  private void Awake() => this._transform = this.transform;

  public void SetCameraEnabled(bool enabled) => this.camera.enabled = enabled;

  private void LateUpdate()
  {
    if (!WeaponFeedbackManager.Exists)
      return;
    if (WeaponFeedbackManager.Instance.IsIronSighted)
    {
      this._currentAngle = Vector2.Lerp(this._currentAngle, Vector2.zero, Time.deltaTime * 5f);
      this.MoveWeapon();
    }
    else
    {
      this.AddDeltaAngle(AutoMonoBehaviour<InputManager>.Instance.GetValue(GameInputKey.HorizontalLook), AutoMonoBehaviour<InputManager>.Instance.GetValue(GameInputKey.VerticalLook));
      this.MoveWeapon();
      this._currentAngle = Vector2.Lerp(this._currentAngle, Vector2.zero, Time.deltaTime * 5f);
    }
  }

  private void AddDeltaAngle(float x, float y) => this._currentAngle = Vector2.ClampMagnitude(this._currentAngle + Vector2.ClampMagnitude(new Vector2(x, y), this._maxDisplacementDelta), this._maxDisplacement);

  private void MoveWeapon() => this._transform.localRotation = Quaternion.AngleAxis(this._currentAngle.x, Vector3.up) * Quaternion.AngleAxis(-this._currentAngle.y, Vector3.right);
}
