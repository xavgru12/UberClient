// Decompiled with JetBrains decompiler
// Type: MapConfiguration
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class MapConfiguration : MonoBehaviour
{
  [SerializeField]
  private bool _isEnabled = true;
  [SerializeField]
  private int _defaultSpawnPoint;
  [SerializeField]
  private FootStepSoundType _defaultFootStep = FootStepSoundType.Sand;
  [SerializeField]
  private Camera _camera;
  [SerializeField]
  private Transform _defaultViewPoint;
  [SerializeField]
  protected GameObject _staticContentParent;
  [SerializeField]
  private GameObject _spawnPoints;
  [SerializeField]
  private Transform _waterPlane;
  [SerializeField]
  private CombatRangeTier _combatRange;

  public bool IsEnabled => this._isEnabled;

  public int DefaultSpawnPoint => this._defaultSpawnPoint;

  public string SceneName { get; private set; }

  public Camera Camera => this._camera;

  public CombatRangeTier CombatRangeTiers => this._combatRange;

  public FootStepSoundType DefaultFootStep => this._defaultFootStep;

  public Transform DefaultViewPoint => this._defaultViewPoint;

  public GameObject SpawnPoints => this._spawnPoints;

  public bool HasWaterPlane => (Object) this._waterPlane != (Object) null;

  public float WaterPlaneHeight => (bool) (Object) this._waterPlane ? this._waterPlane.position.y : float.MinValue;

  private void Awake()
  {
    if ((Object) this._defaultViewPoint == (Object) null)
      this._defaultViewPoint = this.transform;
    GameState.CurrentSpace = this;
    this.SceneName = Singleton<SceneLoader>.Instance.CurrentScene;
  }

  private void Start() => Singleton<SpawnPointManager>.Instance.ConfigureSpawnPoints(this.SpawnPoints.GetComponentsInChildren<SpawnPoint>(true));
}
