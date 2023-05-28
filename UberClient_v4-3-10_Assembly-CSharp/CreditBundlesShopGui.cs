// Decompiled with JetBrains decompiler
// Type: CreditBundlesShopGui
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using Cmune.DataCenter.Common.Entities;
using System.Collections.Generic;
using UnityEngine;

public class CreditBundlesShopGui
{
  private Vector2 bundleScroll = Vector2.zero;
  private int scrollHeight;
  private Dictionary<int, float> _alpha = new Dictionary<int, float>();

  public void Draw(Rect position)
  {
    float height = Mathf.Max(position.height, (float) this.scrollHeight);
    this.bundleScroll = GUI.BeginScrollView(position, this.bundleScroll, new Rect(0.0f, 0.0f, position.width - 17f, height), false, true);
    if (Singleton<BundleManager>.Instance.WaitingForBundles)
    {
      WaitingTexture.Draw(new Vector2((float) (((double) position.width - 17.0) / 2.0), position.x + 50f));
    }
    else
    {
      List<BundleUnityView> creditBundles = Singleton<BundleManager>.Instance.GetCreditBundles();
      if (creditBundles.Count == 0)
      {
        GUI.Label(new Rect(4f, 4f, position.width - 20f, 24f), "No credit packs are currently on sale.", BlueStonez.label_interparkbold_16pt);
      }
      else
      {
        int num1 = 4;
        int num2 = 0;
        GUI.Label(new Rect(4f, (float) (num1 + 4), position.width - 20f, 20f), "Credit Packs", BlueStonez.label_interparkbold_18pt_left);
        int top = num1 + 30;
        foreach (BundleUnityView bundleUnityView in creditBundles)
        {
          this.DrawPackSlot(new Rect(num2 % 2 != 1 ? 0.0f : 187f, (float) top, 188f, 95f), bundleUnityView);
          top += num2 % 2 != 1 ? 0 : 94;
          ++num2;
        }
        if (num2 % 2 == 1)
          top += 94;
        GUI.Label(new Rect(4f, (float) top, position.width - 8f, 1f), GUIContent.none, BlueStonez.horizontal_line_grey95);
        this.scrollHeight = top;
      }
    }
    GUI.EndScrollView();
  }

  private void DrawPackSlot(Rect position, BundleUnityView bundleUnityView)
  {
    int id = bundleUnityView.BundleView.Id;
    bool flag = position.Contains(Event.current.mousePosition);
    if (!this._alpha.ContainsKey(id))
      this._alpha[id] = 0.0f;
    this._alpha[id] = Mathf.Lerp(this._alpha[id], !flag ? 0.0f : 1f, Time.deltaTime * (!flag ? 10f : 3f));
    GUI.BeginGroup(position);
    GUI.Label(new Rect(2f, 2f, position.width - 4f, 79f), GUIContent.none, BlueStonez.gray_background);
    bundleUnityView.Icon.Draw(new Rect(4f, 4f, 75f, 75f));
    GUI.Label(new Rect(81f, 0.0f, position.width - 80f, 44f), bundleUnityView.BundleView.Name, BlueStonez.label_interparkbold_13pt_left);
    GUI.enabled = GUITools.SaveClickIn(1f);
    this.BuyButton(position, bundleUnityView);
    GUI.enabled = true;
    GUI.EndGroup();
  }

  private void BuyButton(Rect position, BundleUnityView bundleUnityView)
  {
    ChannelType channel = ApplicationDataManager.Channel;
    switch (channel)
    {
      case ChannelType.MacAppStore:
      case ChannelType.IPad:
      case ChannelType.Android:
        this.BuyStoreKitButton(position, bundleUnityView);
        break;
      default:
        if (channel != ChannelType.WebFacebook)
          break;
        this.BuyFBCButton(position, bundleUnityView);
        break;
    }
  }

  private void BuyFBCButton(Rect position, BundleUnityView bundleUnityView)
  {
    if (!GUI.Button(new Rect(81f, 51f, position.width - 110f, 20f), new GUIContent(bundleUnityView.Price, (Texture) UberstrikeIcons.FacebookCreditsIcon, "Buy the " + bundleUnityView.BundleView.Name + " pack."), BlueStonez.buttongold_medium))
      return;
    GUITools.Clicked();
    if (ScreenResolutionManager.IsFullScreen)
      ScreenResolutionManager.IsFullScreen = false;
    Singleton<BundleManager>.Instance.BuyFacebookBundle(bundleUnityView.BundleView.Id);
  }

  private void BuyStoreKitButton(Rect position, BundleUnityView bundleUnityView)
  {
    if (!GUI.Button(new Rect(81f, 51f, position.width - 110f, 20f), new GUIContent(bundleUnityView.CurrencySymbol + bundleUnityView.Price, "Buy the " + bundleUnityView.BundleView.Name + " pack."), BlueStonez.buttongold_medium))
      return;
    if (Singleton<BundleManager>.Instance.CanMakeMasPayments)
    {
      GUITools.Clicked();
      if (ScreenResolutionManager.IsFullScreen)
        ScreenResolutionManager.IsFullScreen = false;
      Singleton<BundleManager>.Instance.BuyStoreKitItem(bundleUnityView);
    }
    else
      PopupSystem.ShowMessage(LocalizedStrings.Error, "Sorry, it appears you are unable to make In App purchases at this time.", PopupSystem.AlertType.OK);
  }
}
