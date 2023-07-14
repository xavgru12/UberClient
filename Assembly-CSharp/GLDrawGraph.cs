// Decompiled with JetBrains decompiler
// Type: GLDrawGraph
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class GLDrawGraph : MonoBehaviour
{
  private const int SAMPLE_COUNT = 200;
  private static Material glMaterial;
  private Rect graph1Rect = new Rect(10f, 10f, 200f, 80f);
  private int xOffset;
  private int yOffset;
  private int xLimit;
  private int yLimit;
  private bool doDrag;
  private Dictionary<int, List<float>> _graphArrays;
  private Dictionary<int, float> _graphArraysMax;
  private static Dictionary<int, string> _captions = new Dictionary<int, string>(10);
  private Dictionary<int, float> _lastSamples;
  private float[] _nullNodes;
  private float[] _accumulatedNodes;
  private int _currentSample;
  public GLDrawGraph.VIEW ViewStyle;
  public Color[] Colors;
  public float SampleFrequency = 0.01f;
  public bool DrawGraph = true;
  public static int GraphId;

  public static GLDrawGraph Instance { get; private set; }

  public static bool Exists => (Object) GLDrawGraph.Instance != (Object) null;

  private void Awake()
  {
    GLDrawGraph.Instance = this;
    this._graphArrays = new Dictionary<int, List<float>>(10);
    this._graphArraysMax = new Dictionary<int, float>(10);
    this._lastSamples = new Dictionary<int, float>(10);
    this._accumulatedNodes = new float[200];
    this._nullNodes = new float[200];
  }

  private void Start()
  {
    this.StartCoroutine(this.startSampleLoop());
    GLDrawGraph.createGLMaterial();
  }

  private void Update()
  {
    if (!this.DrawGraph)
      return;
    if (Input.GetMouseButtonDown(0) && this.graph1Rect.Contains(Input.mousePosition))
    {
      this.xOffset = (int) ((double) Input.mousePosition.x - (double) this.graph1Rect.x);
      this.yOffset = (int) ((double) Input.mousePosition.y - (double) this.graph1Rect.y);
      this.doDrag = true;
    }
    if (this.graph1Rect.Contains(Input.mousePosition))
      this.graph1Rect.height += (float) (int) ((double) Input.GetAxis("Mouse ScrollWheel") * 40.0);
    if (this.doDrag)
      this.graph1Rect = new Rect(Input.mousePosition.x - (float) this.xOffset, Input.mousePosition.y - (float) this.yOffset, this.graph1Rect.width, this.graph1Rect.height);
    if (Input.GetMouseButtonUp(0))
      this.doDrag = false;
    this.yLimit = (int) ((double) Screen.height - (double) this.graph1Rect.height);
    this.xLimit = (int) ((double) Screen.width - (double) this.graph1Rect.width);
    this.graph1Rect = new Rect(Mathf.Clamp(this.graph1Rect.x, 2f, (float) this.xLimit), Mathf.Clamp(this.graph1Rect.y, 2f, (float) this.yLimit), this.graph1Rect.width, Mathf.Clamp(this.graph1Rect.height, 40f, (float) Screen.height));
  }

  [DebuggerHidden]
  private IEnumerator startSampleLoop() => (IEnumerator) new GLDrawGraph.\u003CstartSampleLoop\u003Ec__Iterator83()
  {
    \u003C\u003Ef__this = this
  };

  public static void AddSampleValue(int graph, float v)
  {
    if (!GLDrawGraph.Exists || !GLDrawGraph.Instance.DrawGraph)
      return;
    if (!GLDrawGraph.Instance._graphArrays.ContainsKey(graph))
    {
      GLDrawGraph.Instance._graphArrays[graph] = new List<float>((IEnumerable<float>) GLDrawGraph.Instance._nullNodes);
      GLDrawGraph.Instance._graphArraysMax[graph] = 0.0f;
    }
    GLDrawGraph.Instance._graphArrays[graph][GLDrawGraph.Instance._currentSample] = v;
    GLDrawGraph.Instance._graphArraysMax[graph] = Mathf.Max(GLDrawGraph.Instance._graphArraysMax[graph], Mathf.Abs(v));
    GLDrawGraph.Instance._lastSamples[graph] = v;
  }

  public static void AddCaption(int graph, string caption) => GLDrawGraph._captions[graph] = caption;

  private static void createGLMaterial()
  {
    if ((bool) (Object) GLDrawGraph.glMaterial)
      return;
    GLDrawGraph.glMaterial = new Material("Shader \"Lines/Colored Blended\" {SubShader { Pass {  Blend SrcAlpha OneMinusSrcAlpha  ZWrite Off Cull Off Fog { Mode Off }  BindChannels { Bind \"vertex\", vertex Bind \"color\", color }} } }");
    GLDrawGraph.glMaterial.hideFlags = HideFlags.HideAndDontSave;
    GLDrawGraph.glMaterial.shader.hideFlags = HideFlags.HideAndDontSave;
  }

  private void OnGUI()
  {
    if (!this.DrawGraph || this.ViewStyle != GLDrawGraph.VIEW.SPLIT)
      return;
    GUI.color = Color.black;
    float num1 = this.graph1Rect.height / (float) this._graphArrays.Count;
    int num2 = this._graphArrays.Count - 1;
    foreach (KeyValuePair<int, List<float>> graphArray in this._graphArrays)
    {
      string text = "<no caption> ";
      if (GLDrawGraph._captions.TryGetValue(graphArray.Key, out text))
        GUI.Label(new Rect(this.graph1Rect.x + 5f, (float) ((double) Screen.height - (double) this.graph1Rect.yMax + (double) num2 * (double) num1), 200f, 20f), text);
      float num3;
      if (this._lastSamples.TryGetValue(graphArray.Key, out num3))
        GUI.Label(new Rect(this.graph1Rect.xMax - 50f, (float) ((double) Screen.height - (double) this.graph1Rect.yMax + (double) num2 * (double) num1), 200f, 20f), num3.ToString("f2"));
      --num2;
    }
    GUI.color = Color.white;
  }

  private void OnPostRender()
  {
    if (!this.DrawGraph || this._graphArrays.Count == 0 || this.Colors.Length == 0)
      return;
    GLDrawGraph.glMaterial.SetPass(0);
    GL.PushMatrix();
    GL.LoadPixelMatrix();
    float num1 = this.graph1Rect.height / (float) this._graphArrays.Count;
    switch (this.ViewStyle)
    {
      case GLDrawGraph.VIEW.SPLIT:
        this.draw2DRectOutline(new Rect(this.graph1Rect.x - 1f, this.graph1Rect.y - 1f, this.graph1Rect.width + 1f, this.graph1Rect.height + 1f), new Color(1f, 1f, 1f, 0.5f), new Color(1f, 1f, 1f, 1f), 0);
        int num2 = 0;
        using (Dictionary<int, List<float>>.Enumerator enumerator = this._graphArrays.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            KeyValuePair<int, List<float>> current = enumerator.Current;
            Color color = this.Colors[num2 % this.Colors.Length];
            for (int index1 = 0; index1 < current.Value.Count; ++index1)
            {
              int index2 = (index1 + this._currentSample) % 200;
              float left = this.graph1Rect.x + (float) index1;
              float num3 = current.Value[index2];
              this.draw2DLine(new Rect(left, this.graph1Rect.y + (float) num2 * num1, 1f, num3 * num1), color, 0);
            }
            ++num2;
          }
          break;
        }
      case GLDrawGraph.VIEW.ACCUMULATED:
        this.draw2DRectOutline(new Rect(309f, 9f, this.graph1Rect.width + 1f, this.graph1Rect.height + 1f), new Color(1f, 1f, 1f, 0.5f), new Color(1f, 1f, 1f, 1f), 0);
        for (int index = 0; index < this._accumulatedNodes.Length; ++index)
          this._accumulatedNodes[index] = 0.0f;
        int num4 = 0;
        using (Dictionary<int, List<float>>.ValueCollection.Enumerator enumerator = this._graphArrays.Values.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            List<float> current = enumerator.Current;
            Color color = this.Colors[num4 % this.Colors.Length];
            for (int index3 = 0; index3 < current.Count; ++index3)
            {
              int index4 = (index3 + this._currentSample) % 200;
              this.draw2DLine(new Rect((float) (310 + index3), (float) (10.0 + (double) this._accumulatedNodes[index4] * (double) num1), 1f, current[index4] * num1), color, 0);
              this._accumulatedNodes[index4] += current[index4];
            }
            ++num4;
          }
          break;
        }
    }
    GL.PopMatrix();
  }

  private void draw2DRect(Rect rect, Color color, int depth)
  {
    GL.Begin(7);
    GL.Color(color);
    GL.Vertex3(rect.x, rect.y + rect.height, (float) depth);
    GL.Vertex3(rect.x + rect.width, rect.y + rect.height, (float) depth);
    GL.Vertex3(rect.x + rect.width, rect.y, (float) depth);
    GL.Vertex3(rect.x, rect.y, (float) depth);
    GL.End();
  }

  private void draw2DRectOutline(Rect rect, Color color, Color outlineColor, int depth)
  {
    this.draw2DRect(rect, color, depth);
    this.draw2DOutline(rect, outlineColor, depth);
  }

  private void draw2DOutline(Rect rect, Color color, int depth)
  {
    GL.Begin(1);
    GL.Color(color);
    GL.Vertex3(rect.x, rect.y, (float) depth);
    GL.Vertex3(rect.x + rect.width, rect.y, (float) depth);
    GL.Vertex3(rect.x, rect.y + rect.height, (float) depth);
    GL.Vertex3(rect.x + rect.width, rect.y + rect.height, (float) depth);
    GL.Vertex3(rect.x, rect.y, (float) depth);
    GL.Vertex3(rect.x, rect.y + rect.height, (float) depth);
    GL.Vertex3(rect.x + rect.width, rect.y, (float) depth);
    GL.Vertex3(rect.x + rect.width, rect.y + rect.height, (float) depth);
    GL.End();
  }

  private void draw2DLine(Rect rect, Color color, int depth)
  {
    GL.Begin(1);
    GL.Color(color);
    GL.Vertex3(rect.x, rect.y, (float) depth);
    GL.Vertex3(rect.x, rect.y + rect.height, (float) depth);
    GL.End();
  }

  private void draw2DPoint(Rect rect, Color color, int depth)
  {
    GL.Begin(1);
    GL.Color(color);
    GL.Vertex3(rect.x, rect.y + rect.height, (float) depth);
    GL.Vertex3(rect.x, (float) ((double) rect.y + (double) rect.height + 1.0), (float) depth);
    GL.End();
  }

  public enum VIEW
  {
    SPLIT,
    ACCUMULATED,
  }
}
