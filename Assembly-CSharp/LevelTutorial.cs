// Decompiled with JetBrains decompiler
// Type: LevelTutorial
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class LevelTutorial : MonoBehaviour
{
  [SerializeField]
  private Animation _airlockBrigeAnim;
  [SerializeField]
  private Animation _airlockDoorAnim;
  [SerializeField]
  private DoorBehaviour _armoryDoor;
  [SerializeField]
  private AudioSource _bridgeAudio;
  [SerializeField]
  private AudioSource _backgroundMusic;
  [SerializeField]
  private BitmapFont _font;
  [SerializeField]
  private Transform _npcStartPos;
  [SerializeField]
  private BaseWeaponDecorator _npcWeapon;
  [SerializeField]
  private AudioClip _bigDoorClose;
  [SerializeField]
  private AudioClip _waypoint;
  [SerializeField]
  private AudioClip _bigObjComplete;
  [SerializeField]
  private AudioClip _voiceWelcome;
  [SerializeField]
  private AudioClip _voiceToArmory;
  [SerializeField]
  private AudioClip _voicePickupWeapon;
  [SerializeField]
  private AudioClip _voiceShootingRange;
  [SerializeField]
  private AudioClip _voiceShootMore;
  [SerializeField]
  private AudioClip _voiceArena;
  [SerializeField]
  private AudioClip _voiceTutorialComplete;
  [SerializeField]
  private SplineController _airlockSplineController;
  [SerializeField]
  private TutorialAirlockFrontDoor _airlockFrontDoor;
  [SerializeField]
  private TutorialAirlockDoor _airlockBackDoor;
  [SerializeField]
  private TutorialArmoryEnterTrigger _armoryTrigger;
  [SerializeField]
  private Texture _imgMouse;
  [SerializeField]
  private Texture _imgObjTickBackground;
  [SerializeField]
  private Texture _imgObjTickForeground;
  [SerializeField]
  private Texture[] _imgWasdWalkBlack;
  [SerializeField]
  private Texture[] _imgWasdWalkBlue;
  [SerializeField]
  private TutorialWaypoint _armoryWaypoint;
  [SerializeField]
  private TutorialWaypoint _weaponWaypoint;
  [SerializeField]
  private GameObject _shootingTarget;
  [SerializeField]
  private Transform[] _nearRangeTargetPos;
  [SerializeField]
  private Transform[] _farRangeTargetPos;
  [SerializeField]
  private Transform _armoryCameraPathEnd;
  [SerializeField]
  private Transform _finalPlayerPos;
  [SerializeField]
  private TutorialArmoryPickup _pickupWeapon;
  [SerializeField]
  private TutorialWaypoint _ammoWaypoint;

  public static LevelTutorial Instance { get; private set; }

  public static bool Exists => (Object) LevelTutorial.Instance != (Object) null;

  public Animation AirlockBridgeAnim => this._airlockBrigeAnim;

  public Animation AirlockDoorAnim => this._airlockDoorAnim;

  public DoorBehaviour ArmoryDoor => this._armoryDoor;

  public AudioSource BridgeAudio => this._bridgeAudio;

  public AudioSource BackgroundMusic => this._backgroundMusic;

  public BitmapFont Font => this._font;

  public Transform NpcStartPos => this._npcStartPos;

  public int GearBoots => 1272;

  public int GearFace => 1273;

  public int GearGloves => 1274;

  public int GearHead => 1275;

  public int GearLB => 1276;

  public int GearUB => 1277;

  public BaseWeaponDecorator Weapon => this._npcWeapon;

  public AudioClip BigDoorClose => this._bigDoorClose;

  public AudioClip WaypointAppear => this._waypoint;

  public AudioClip BigObjComplete => this._bigObjComplete;

  public AudioClip VoiceWelcome => this._voiceWelcome;

  public AudioClip VoiceToArmory => this._voiceToArmory;

  public AudioClip VoicePickupWeapon => this._voicePickupWeapon;

  public AudioClip VoiceShootingRange => this._voiceShootingRange;

  public AudioClip VoiceShootMore => this._voiceShootMore;

  public AudioClip VoiceArena => this._voiceArena;

  public AudioClip TutorialComplete => this._voiceTutorialComplete;

  public SplineController AirlockSplineController => this._airlockSplineController;

  public TutorialAirlockFrontDoor AirlockFrontDoor => this._airlockFrontDoor;

  public TutorialAirlockDoor AirlockBackDoor => this._airlockBackDoor;

  public TutorialArmoryEnterTrigger ArmoryTrigger => this._armoryTrigger;

  public Texture ImgMouse => this._imgMouse;

  public Texture ImgObjBk => this._imgObjTickBackground;

  public Texture ImgObjTick => this._imgObjTickForeground;

  public Texture[] ImgWasdWalkBlack => this._imgWasdWalkBlack;

  public Texture[] ImgWasdWalkBlue => this._imgWasdWalkBlue;

  public TutorialWaypoint ArmoryWaypoint => this._armoryWaypoint;

  public TutorialWaypoint WeaponWaypoint => this._weaponWaypoint;

  public GameObject ShootingTargetPrefab => this._shootingTarget;

  public Transform[] NearRangeTargetPos => this._nearRangeTargetPos;

  public Transform[] FarRangeTargetPos => this._farRangeTargetPos;

  public Transform ArmoryCameraPathEnd => this._armoryCameraPathEnd;

  public Transform FinalPlayerPos => this._finalPlayerPos;

  public TutorialArmoryPickup PickupWeapon => this._pickupWeapon;

  public TutorialWaypoint AmmoWaypoint => this._ammoWaypoint;

  public Transform NPC { get; set; }

  public bool IsCinematic { get; set; }

  public bool ShowObjectives { get; set; }

  public bool ShowObjPickupMG { get; set; }

  public bool ShowObjShoot3 { get; set; }

  public bool ShowObjShoot6 { get; set; }

  public bool ShowObjComplete { get; set; }

  public HudDrawFlags HudFlags { get; set; }

  private void Awake()
  {
    LevelTutorial.Instance = this;
    this.HudFlags = HudDrawFlags.XpPoints;
  }

  private void Start()
  {
    if ((Object) LevelCamera.Instance != (Object) null)
    {
      this.AirlockSplineController.Target = LevelCamera.Instance.gameObject;
      this.ArmoryTrigger.ArmoryCameraPath.Target = LevelCamera.Instance.gameObject;
    }
    Singleton<GameStateController>.Instance.LoadGameMode(GameMode.Tutorial);
  }
}
