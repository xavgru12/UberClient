// Decompiled with JetBrains decompiler
// Type: MouseOrbit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public class MouseOrbit : MonoBehaviour
{
  private const float zoomSpeedFactor = 15f;
  private const float zoomTouchSpeedFactor = 0.001f;
  private const float flingSpeedFactor = 0.1f;
  private const float orbitSpeedFactor = 3f;
  private const float panSpeedFactor = 0.01f;
  [SerializeField]
  private UnityEngine.Transform target;
  private Vector2 zoomMax = new Vector2(1.3f, 5f);
  private Vector2 panMax = new Vector2(-0.5f, 0.5f);
  private Vector2 panTotalMax = new Vector2(-1f, 1f);
  public Vector3 OrbitConfig;
  public float yPanningOffset;
  private float zoomDistance = 5f;
  public int MaxX;
  private Vector2 mouseAxisSpin;
  private Vector3 mousePos;
  private bool listenToMouseUp;
  private bool isMouseDragging;

  public static MouseOrbit Instance { get; private set; }

  public Vector3 OrbitOffset { get; set; }

  public static bool Disable { get; set; }

  private void Awake()
  {
    MouseOrbit.Instance = this;
    this.enabled = false;
    MouseOrbit.Disable = false;
    if ((UnityEngine.Object) this.target == (UnityEngine.Object) null)
      throw new NullReferenceException("MouseOrbit.target not set");
  }

  private void Start()
  {
    this.mouseAxisSpin = Vector2.zero;
    Vector3 eulerAngles = this.transform.eulerAngles;
    this.OrbitConfig.x = eulerAngles.y;
    this.OrbitConfig.y = eulerAngles.x;
    this.MaxX = Screen.width;
  }

  private void OnEnable()
  {
    this.zoomDistance = this.OrbitConfig.z = Mathf.Clamp(Vector3.Distance(this.transform.position, this.target.position), this.zoomMax[0], this.zoomMax[1]);
    this.OrbitConfig.x = this.transform.rotation.eulerAngles.y;
    this.OrbitConfig.y = this.transform.rotation.eulerAngles.x;
  }

  private void LateUpdate()
  {
    if (!PopupSystem.IsAnyPopupOpen && !PanelManager.IsAnyPanelOpen)
    {
      if (this.camera.pixelRect.Contains(Input.mousePosition) && (double) Input.GetAxis("Mouse ScrollWheel") != 0.0)
        this.OrbitConfig.z = Mathf.Clamp(this.zoomDistance - Input.GetAxis("Mouse ScrollWheel") * 15f, this.zoomMax[0], this.zoomMax[1]);
      if (Input.GetMouseButtonDown(0) && this.camera.pixelRect.Contains(Input.mousePosition))
      {
        this.mouseAxisSpin = Vector2.zero;
        this.listenToMouseUp = true;
        this.isMouseDragging = true;
      }
      if (Input.GetMouseButtonUp(0))
      {
        this.mouseAxisSpin = !this.listenToMouseUp ? Vector2.zero : (Vector2) ((Input.mousePosition - this.mousePos).normalized * Mathf.Clamp((Input.mousePosition - this.mousePos).magnitude, 0.0f, 3f));
        this.listenToMouseUp = false;
      }
      this.mousePos = Input.mousePosition;
      if (this.isMouseDragging && Input.GetMouseButton(0))
      {
        this.OrbitConfig.x += Input.GetAxis("Mouse X") * 3f;
        this.yPanningOffset -= (float) ((double) Input.GetAxis("Mouse Y") * 0.0099999997764825821 * (!MouseOrbit.IsValueWithin(this.yPanningOffset, this.panMax[0], this.panMax[1]) ? 0.30000001192092896 : 1.0));
      }
      else if ((double) this.mouseAxisSpin.magnitude > 0.010000000707805157)
      {
        this.mouseAxisSpin = Vector2.Lerp(this.mouseAxisSpin, Vector2.zero, Time.deltaTime * 2f);
        this.OrbitConfig.x += this.mouseAxisSpin.x * 0.1f;
        this.yPanningOffset -= (float) ((double) this.mouseAxisSpin.y * 0.0099999997764825821 * 0.10000000149011612 * (!MouseOrbit.IsValueWithin(this.yPanningOffset, this.panMax[0], this.panMax[1]) ? 0.30000001192092896 : 1.0));
      }
      else
        this.mouseAxisSpin = Vector2.zero;
      if (!this.isMouseDragging || !Input.GetMouseButton(0))
        this.yPanningOffset = Mathf.Lerp(this.yPanningOffset, Mathf.Clamp(this.yPanningOffset, this.panMax[0], this.panMax[1]), Time.deltaTime * 10f);
      this.yPanningOffset = Mathf.Clamp(this.yPanningOffset, this.panTotalMax[0], this.panTotalMax[1]);
      this.zoomDistance = Mathf.Lerp(this.zoomDistance, Mathf.Clamp(this.OrbitConfig.z, this.zoomMax[0], this.zoomMax[1]), Time.deltaTime * 5f);
      this.Transform(this.transform);
    }
    else
    {
      this.listenToMouseUp = false;
      this.mouseAxisSpin = Vector2.zero;
    }
    if (!this.isMouseDragging || Input.GetMouseButton(0))
      return;
    this.isMouseDragging = false;
  }

  public void Transform(UnityEngine.Transform transform)
  {
    Vector3 position;
    Quaternion rotation2;
    this.Transform(out position, out rotation2);
    transform.position = position;
    transform.rotation = rotation2;
  }

  public void Transform(out Vector3 position, out Quaternion rotation2)
  {
    float num = 1f - Mathf.Clamp01(this.zoomDistance / this.zoomMax[1]);
    Quaternion quaternion = Quaternion.Euler(this.OrbitConfig.y, this.OrbitConfig.x, 0.0f);
    rotation2 = quaternion;
    position = this.target.position + quaternion * new Vector3(0.0f, 0.0f, -this.zoomDistance) + quaternion * (this.OrbitOffset + new Vector3(0.0f, this.yPanningOffset * num, 0.0f)) * this.zoomDistance;
  }

  private static bool IsValueWithin(float value, float min, float max) => (double) value >= (double) min && (double) value <= (double) max;

  public static float ClampAngle(float angle, float min, float max)
  {
    if ((double) angle < -360.0)
      angle += 360f;
    if ((double) angle > 360.0)
      angle -= 360f;
    return Mathf.Clamp(angle, min, max);
  }
}
