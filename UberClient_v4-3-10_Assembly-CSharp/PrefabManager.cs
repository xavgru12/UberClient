// Decompiled with JetBrains decompiler
// Type: PrefabManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class PrefabManager : MonoBehaviour
{
  [SerializeField]
  private AvatarDecorator _defaultAvatar;
  [SerializeField]
  private AvatarDecoratorConfig _defaultRagdoll;
  [SerializeField]
  private CharacterConfig _remoteCharacter;
  [SerializeField]
  private CharacterConfig _localCharacter;
  [SerializeField]
  private GameObject _lotteryEffect;
  [SerializeField]
  private Animation _lotteryUIAnimation;

  public static PrefabManager Instance { get; private set; }

  public static bool Exists => (Object) PrefabManager.Instance != (Object) null;

  public AvatarDecorator DefaultAvatar => this._defaultAvatar;

  public AvatarDecoratorConfig DefaultRagdoll => this._defaultRagdoll;

  public CharacterConfig InstantiateLocalCharacter() => Object.Instantiate((Object) this._localCharacter) as CharacterConfig;

  public CharacterConfig InstantiateRemoteCharacter() => Object.Instantiate((Object) this._remoteCharacter) as CharacterConfig;

  public void InstantiateLotteryEffect()
  {
    if (!(bool) (Object) this._lotteryEffect)
      return;
    Object.Instantiate((Object) this._lotteryEffect);
  }

  public Animation GetLotteryUIAnimation()
  {
    Animation lotteryUiAnimation = (Animation) null;
    if ((bool) (Object) this._lotteryUIAnimation)
      lotteryUiAnimation = Object.Instantiate((Object) this._lotteryUIAnimation) as Animation;
    else
      Debug.LogError((object) "The LotteryUIAnimation is not signed in PrefabManger!");
    return lotteryUiAnimation;
  }

  private void Awake() => PrefabManager.Instance = this;
}
