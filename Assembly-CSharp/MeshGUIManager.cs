// Decompiled with JetBrains decompiler
// Type: MeshGUIManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class MeshGUIManager : MonoBehaviour
{
  [SerializeField]
  private GameObject _meshTextObject;
  [SerializeField]
  private GameObject _quadMeshObject;
  [SerializeField]
  private GameObject _circleMeshObject;
  [SerializeField]
  private Camera _guiCamera;
  private ObjectRecycler _meshTextRecycler;
  private ObjectRecycler _quadMeshRecycler;
  private ObjectRecycler _circleMeshRecycler;
  private GameObject _meshTextContainer;
  private GameObject _quadMeshContainer;
  private GameObject _circleMeshContainer;

  public static MeshGUIManager Instance { get; private set; }

  public static bool Exists => (UnityEngine.Object) MeshGUIManager.Instance != (UnityEngine.Object) null;

  private void Awake()
  {
    MeshGUIManager.Instance = this;
    this.CreateMeshGUIContainers();
    this.CreateMeshGUIRecyclers();
    CmuneEventHandler.AddListener<CameraWidthChangeEvent>(new Action<CameraWidthChangeEvent>(this.OnCameraRectChange));
  }

  private void Start() => this._guiCamera.gameObject.SetActive(true);

  private void OnCameraRectChange(CameraWidthChangeEvent ev)
  {
    if (!((UnityEngine.Object) this._guiCamera != (UnityEngine.Object) null))
      return;
    this._guiCamera.rect = new Rect(0.0f, 0.0f, ev.Width, 1f);
  }

  private void CreateMeshGUIContainers()
  {
    this._meshTextContainer = new GameObject("MeshTextContainer");
    this._meshTextContainer.transform.parent = this.gameObject.transform;
    this._quadMeshContainer = new GameObject("QuadMeshContainer");
    this._quadMeshContainer.transform.parent = this.gameObject.transform;
    this._circleMeshContainer = new GameObject("CircleContainer");
    this._circleMeshContainer.transform.parent = this.gameObject.transform;
  }

  private void CreateMeshGUIRecyclers()
  {
    this._meshTextRecycler = new ObjectRecycler(this._meshTextObject, 5, this._meshTextContainer);
    this._quadMeshRecycler = new ObjectRecycler(this._quadMeshObject, 5, this._quadMeshContainer);
    this._circleMeshRecycler = new ObjectRecycler(this._circleMeshObject, 5, this._circleMeshContainer);
  }

  public GameObject CreateMeshText(GameObject parentObject = null)
  {
    GameObject nextFree = this._meshTextRecycler.GetNextFree();
    if ((UnityEngine.Object) parentObject != (UnityEngine.Object) null)
      nextFree.transform.parent = parentObject.transform;
    return nextFree;
  }

  public void FreeMeshText(GameObject meshTextObject)
  {
    meshTextObject.transform.parent = this._meshTextContainer.transform;
    this._meshTextRecycler.FreeObject(meshTextObject);
  }

  public GameObject CreateQuadMesh(GameObject parentObject = null)
  {
    GameObject nextFree = this._quadMeshRecycler.GetNextFree();
    if ((UnityEngine.Object) parentObject != (UnityEngine.Object) null)
      nextFree.transform.parent = parentObject.transform;
    return nextFree;
  }

  public void FreeQuadMesh(GameObject quadMeshObject)
  {
    quadMeshObject.renderer.material.mainTextureOffset = Vector2.zero;
    quadMeshObject.transform.parent = this._quadMeshContainer.transform;
    this._quadMeshRecycler.FreeObject(quadMeshObject);
  }

  public GameObject CreateCircleMesh(GameObject parentObject = null)
  {
    GameObject nextFree = this._circleMeshRecycler.GetNextFree();
    if ((UnityEngine.Object) parentObject != (UnityEngine.Object) null)
      nextFree.transform.parent = parentObject.transform;
    return nextFree;
  }

  public void FreeCircleMesh(GameObject circleMeshObject)
  {
    circleMeshObject.transform.parent = this._circleMeshContainer.transform;
    this._circleMeshRecycler.FreeObject(circleMeshObject);
  }

  public Vector3 TransformPosFromScreenToWorld(Vector2 screenPos)
  {
    float orthographicSize = this._guiCamera.orthographicSize;
    float num = orthographicSize / (float) Screen.height * (float) Screen.width;
    Vector3 zero = Vector3.zero with
    {
      x = (float) ((double) screenPos.x / (double) Screen.width * (double) num * 2.0) - num,
      y = (float) ((double) screenPos.y / (double) Screen.height * (double) orthographicSize * 2.0) - orthographicSize
    };
    zero.y = -zero.y;
    return zero;
  }

  public Vector2 TransformPosFromWorldToScreen(Vector3 worldPos)
  {
    float orthographicSize = this._guiCamera.orthographicSize;
    float num = orthographicSize / (float) Screen.height * (float) Screen.width;
    return Vector2.zero with
    {
      x = (float) (((double) worldPos.x + (double) num) * (double) Screen.width / (double) num / 2.0),
      y = (float) ((-(double) worldPos.y + (double) orthographicSize) * (double) Screen.height / (double) orthographicSize / 2.0)
    };
  }

  public Vector3 TransformSizeFromScreenToWorld(Vector2 screenSize)
  {
    float orthographicSize = this._guiCamera.orthographicSize;
    float num = orthographicSize / (float) Screen.height * (float) Screen.width;
    return Vector3.zero with
    {
      x = (float) ((double) screenSize.x / (double) Screen.width * (double) num * 2.0),
      y = (float) ((double) screenSize.y / (double) Screen.height * (double) orthographicSize * 2.0)
    };
  }

  public Vector2 TransformSizeFromWorldToScreen(Vector3 worldSize)
  {
    float orthographicSize = this._guiCamera.orthographicSize;
    float num = orthographicSize / (float) Screen.height * (float) Screen.width;
    return Vector2.zero with
    {
      x = (float) ((double) worldSize.x / (double) num / 2.0) * (float) Screen.width,
      y = (float) ((double) worldSize.y / (double) orthographicSize / 2.0) * (float) Screen.height
    };
  }
}
