// Decompiled with JetBrains decompiler
// Type: PlayerDamageEffect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class PlayerDamageEffect : MonoBehaviour
{
  private const int WIDTH = 313;
  private const int HEIGHT = 43;
  [SerializeField]
  private float _height;
  [SerializeField]
  private float _width;
  [SerializeField]
  private float _duration;
  [SerializeField]
  private MeshRenderer _renderer;
  private Vector2[] FONT_METRICS = new Vector2[10]
  {
    new Vector2(0.0f, 42f),
    new Vector2(42f, 21f),
    new Vector2(63f, 29f),
    new Vector2(92f, 28f),
    new Vector2(120f, 34f),
    new Vector2(154f, 28f),
    new Vector2(182f, 33f),
    new Vector2(215f, 31f),
    new Vector2(246f, 34f),
    new Vector2(280f, 33f)
  };
  public float _speed;
  private float _offset;
  private bool _show;
  private float _time;
  private Vector3 _start;
  private Vector3 _direction;
  private Transform _transform;

  private void Awake()
  {
    this._transform = this.transform;
    this._start = this._transform.position;
  }

  private void Update()
  {
    if (!this._show)
      return;
    float num = this._time * this._speed - this._offset;
    Vector3 vector3 = (this._direction * this._time) with
    {
      y = this._height - num * num * this._width
    };
    this._time += Time.deltaTime;
    this._transform.position = this._start + vector3;
    this.UpdateTransform();
    if ((double) this._time <= (double) this._duration)
      return;
    Color color = this._renderer.material.GetColor("_Color");
    color.a = Mathf.Lerp(color.a, 0.0f, Time.deltaTime * 3f);
    this._renderer.material.SetColor("_Color", color);
    if ((double) color.a >= 0.20000000298023224)
      return;
    Object.Destroy((Object) this.gameObject);
  }

  public void Show(DamageInfo shot)
  {
    if ((double) this._width == 0.0)
      this._width = 1f;
    MeshFilter meshFilter = this.gameObject.AddComponent<MeshFilter>();
    if ((bool) (Object) meshFilter)
      meshFilter.mesh = this.CreateCharacterMesh((int) shot.Damage, this.FONT_METRICS, 313, 43);
    this.UpdateTransform();
    this._show = true;
    this._offset = Mathf.Sqrt(this._height / this._width);
    this._direction = Random.onUnitSphere;
    this._renderer.material = new Material(this._renderer.material);
    this.StartCoroutine(this.StartEnableRenderer());
  }

  [DebuggerHidden]
  private IEnumerator StartEnableRenderer() => (IEnumerator) new PlayerDamageEffect.\u003CStartEnableRenderer\u003Ec__Iterator7F()
  {
    \u003C\u003Ef__this = this
  };

  private void UpdateTransform()
  {
    if (GameState.HasCurrentPlayer)
    {
      float num = (float) (3.0 / 1000.0 + 0.00050000002374872565 * (double) Vector3.Distance(this._transform.position, GameState.LocalCharacter.Position) * (double) LevelCamera.Instance.FOV / 60.0);
      this._transform.localScale = new Vector3(num, num, num);
      this._transform.rotation = GameState.LocalCharacter.Rotation;
    }
    else
    {
      if (!(bool) (Object) Camera.current)
        return;
      Transform transform = Camera.current.transform;
      float num = (float) (3.0 / 1000.0 + 0.00050000002374872565 * (double) Vector3.Distance(this._transform.position, transform.position) * (double) Camera.current.fov / 60.0);
      this._transform.localScale = new Vector3(num, num, num);
      this._transform.rotation = Quaternion.LookRotation(new Vector3(transform.forward.x, 0.0f, transform.forward.z));
    }
  }

  private Mesh CreateCharacterMesh(int number, Vector2[] metrics, int width, int height)
  {
    Mesh characterMesh = new Mesh();
    string str = Mathf.Abs(number).ToString();
    List<Vector3> vector3List1 = new List<Vector3>();
    List<Vector2> vector2List = new List<Vector2>();
    List<int> intList = new List<int>();
    Vector3[] collection1 = new Vector3[4];
    Vector2[] collection2 = new Vector2[4];
    int[] numArray = new int[6]{ 0, 1, 2, 0, 2, 3 };
    float num1 = 0.0f;
    float num2 = 0.0f;
    for (int index1 = 0; index1 < str.Length; ++index1)
    {
      int index2 = (int) str[index1] - 48;
      if (index2 >= 0 && index2 < 10)
      {
        for (int index3 = 0; index3 < 6; ++index3)
          intList.Add(numArray[index3] + vector3List1.Count);
        collection1[0] = new Vector3(metrics[index2].x, 0.0f, 0.0f);
        collection1[1] = new Vector3(metrics[index2].x + metrics[index2].y, 0.0f, 0.0f);
        collection1[2] = new Vector3(metrics[index2].x + metrics[index2].y, (float) height, 0.0f);
        collection1[3] = new Vector3(metrics[index2].x, (float) height, 0.0f);
        collection2[0] = new Vector2(collection1[0].x / (float) width, collection1[0].y / (float) height);
        collection2[1] = new Vector2(collection1[1].x / (float) width, collection1[1].y / (float) height);
        collection2[2] = new Vector2(collection1[2].x / (float) width, collection1[2].y / (float) height);
        collection2[3] = new Vector2(collection1[3].x / (float) width, collection1[3].y / (float) height);
        vector3List1.AddRange((IEnumerable<Vector3>) collection1);
        vector2List.AddRange((IEnumerable<Vector2>) collection2);
        num1 += metrics[index2].y;
      }
    }
    for (int index4 = 0; index4 < vector3List1.Count / 4; ++index4)
    {
      List<Vector3> vector3List2;
      int index5;
      Vector3 vector3_1 = (vector3List2 = vector3List1)[index5 = index4 * 4 + 1] - new Vector3(vector3List1[index4 * 4].x + num1 / 2f - num2, (float) (height / 2));
      vector3List2[index5] = vector3_1;
      List<Vector3> vector3List3;
      int index6;
      Vector3 vector3_2 = (vector3List3 = vector3List1)[index6 = index4 * 4 + 2] - new Vector3(vector3List1[index4 * 4 + 3].x + num1 / 2f - num2, (float) (height / 2));
      vector3List3[index6] = vector3_2;
      List<Vector3> vector3List4;
      int index7;
      Vector3 vector3_3 = (vector3List4 = vector3List1)[index7 = index4 * 4 + 3] - new Vector3(vector3List1[index4 * 4 + 3].x + num1 / 2f - num2, (float) (height / 2));
      vector3List4[index7] = vector3_3;
      List<Vector3> vector3List5;
      int index8;
      Vector3 vector3_4 = (vector3List5 = vector3List1)[index8 = index4 * 4] - new Vector3(vector3List1[index4 * 4].x + num1 / 2f - num2, (float) (height / 2));
      vector3List5[index8] = vector3_4;
      num2 += vector3List1[index4 * 4 + 1].x - vector3List1[index4 * 4].x;
    }
    characterMesh.vertices = vector3List1.ToArray();
    characterMesh.uv = vector2List.ToArray();
    characterMesh.triangles = intList.ToArray();
    return characterMesh;
  }
}
