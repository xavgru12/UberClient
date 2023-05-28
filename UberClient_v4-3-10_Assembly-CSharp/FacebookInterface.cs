// Decompiled with JetBrains decompiler
// Type: FacebookInterface
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class FacebookInterface : AutoMonoBehaviour<FacebookInterface>
{
  public List<string> FacebookFriendUrls { get; private set; }

  private void Awake() => this.FacebookFriendUrls = new List<string>();

  public void GetFbFriends() => Application.ExternalCall("usHelper.getFriends");

  public void PublishFbScore(int score)
  {
    Debug.Log((object) string.Format("usHelper.publishScore('{0}')", (object) score.ToString()));
    Application.ExternalCall("usHelper.publishScore", (object) score.ToString());
  }

  public void PublishFbAchievement(AchievementType achievementType)
  {
    string str1 = "earn";
    string str2 = string.Empty;
    switch (achievementType)
    {
      case AchievementType.MostValuable:
        str2 = "most_valuable";
        break;
      case AchievementType.MostAggressive:
        str2 = "most_aggressive";
        break;
      case AchievementType.SharpestShooter:
        str2 = "sharpest_shooter";
        break;
      case AchievementType.TriggerHappy:
        str2 = "trigger_happy";
        break;
      case AchievementType.HardestHitter:
        str2 = "hardest_hitter";
        break;
      case AchievementType.CostEffective:
        str2 = "cost_effective";
        break;
    }
    Debug.Log((object) string.Format("usHelper.publishAction('{0}','{1}')", (object) str1, (object) str2));
    Application.ExternalCall("usHelper.publishAction", (object) str1, (object) str2);
  }

  public void PublishFbLevelUp(int level)
  {
    Debug.Log((object) string.Format("usHelper.publishLevelUp('{0}')", (object) level.ToString()));
    Application.ExternalCall("usHelper.publishLevelUp", (object) level.ToString());
  }

  public void OpenInviteFbFriends() => Application.ExternalCall("usHelper.inviteFriends");

  public void JsOnFacebookGetFriends(string friendImageUrls)
  {
    try
    {
      this.FacebookFriendUrls = new List<string>((IEnumerable<string>) friendImageUrls.Split(','));
      Debug.Log((object) string.Format("JsOnFacebookGetFriends found {0} friends.", (object) this.FacebookFriendUrls.Count));
    }
    catch (Exception ex)
    {
      Debug.LogWarning((object) (ex.Message + "\n\n" + ex.StackTrace));
    }
  }

  public void JsOnFacebookPayment(string status) => Singleton<BundleManager>.Instance.OnFacebookPayment(status);
}
