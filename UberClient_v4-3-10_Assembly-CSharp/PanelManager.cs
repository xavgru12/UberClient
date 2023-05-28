// Decompiled with JetBrains decompiler
// Type: PanelManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
  private IDictionary<PanelType, IPanelGui> _allPanels;
  private static bool _wasAnyPanelOpen;

  public static PanelManager Instance { get; private set; }

  public static bool Exists => (Object) PanelManager.Instance != (Object) null;

  public LoginPanelGUI LoginPanel => this._allPanels[PanelType.Login] as LoginPanelGUI;

  private void Awake() => PanelManager.Instance = this;

  private void Start()
  {
    this._allPanels = (IDictionary<PanelType, IPanelGui>) new Dictionary<PanelType, IPanelGui>()
    {
      {
        PanelType.Login,
        (IPanelGui) this.GetComponent<LoginPanelGUI>()
      },
      {
        PanelType.Signup,
        (IPanelGui) this.GetComponent<SignupPanelGUI>()
      },
      {
        PanelType.CompleteAccount,
        (IPanelGui) this.GetComponent<CompleteAccountPanelGUI>()
      },
      {
        PanelType.Options,
        (IPanelGui) this.GetComponent<OptionsPanelGUI>()
      },
      {
        PanelType.Help,
        (IPanelGui) this.GetComponent<HelpPanelGUI>()
      },
      {
        PanelType.CreateGame,
        (IPanelGui) this.GetComponent<CreateGamePanelGUI>()
      },
      {
        PanelType.ReportPlayer,
        (IPanelGui) this.GetComponent<ReportPlayerPanelGUI>()
      },
      {
        PanelType.Moderation,
        (IPanelGui) this.GetComponent<ModerationPanelGUI>()
      },
      {
        PanelType.SendMessage,
        (IPanelGui) this.GetComponent<SendMessagePanelGUI>()
      },
      {
        PanelType.FriendRequest,
        (IPanelGui) this.GetComponent<FriendRequestPanelGUI>()
      },
      {
        PanelType.ClanRequest,
        (IPanelGui) this.GetComponent<InviteToClanPanelGUI>()
      },
      {
        PanelType.BuyItem,
        (IPanelGui) this.GetComponent<BuyPanelGUI>()
      },
      {
        PanelType.NameChange,
        (IPanelGui) this.GetComponent<NameChangePanelGUI>()
      }
    };
    foreach (IPanelGui panelGui in (IEnumerable<IPanelGui>) this._allPanels.Values)
    {
      MonoBehaviour monoBehaviour = panelGui as MonoBehaviour;
      if ((bool) (Object) monoBehaviour)
        monoBehaviour.enabled = false;
    }
  }

  private void OnGUI()
  {
    PanelManager.IsAnyPanelOpen = false;
    foreach (IPanelGui panelGui in (IEnumerable<IPanelGui>) this._allPanels.Values)
    {
      if (panelGui.IsEnabled)
      {
        PanelManager.IsAnyPanelOpen = true;
        break;
      }
    }
    if (Event.current.type != UnityEngine.EventType.Layout)
      return;
    if (PanelManager.IsAnyPanelOpen)
    {
      GuiLockController.EnableLock(GuiDepth.Panel);
    }
    else
    {
      GuiLockController.ReleaseLock(GuiDepth.Panel);
      this.enabled = false;
    }
    if (PanelManager._wasAnyPanelOpen == PanelManager.IsAnyPanelOpen)
      return;
    if (PanelManager._wasAnyPanelOpen)
      SfxManager.Play2dAudioClip(GameAudio.ClosePanel);
    else
      SfxManager.Play2dAudioClip(GameAudio.OpenPanel);
    PanelManager._wasAnyPanelOpen = !PanelManager._wasAnyPanelOpen;
  }

  public static bool IsAnyPanelOpen { get; private set; }

  public bool IsPanelOpen(PanelType panel) => this._allPanels[panel].IsEnabled;

  public void CloseAllPanels(PanelType except = PanelType.None)
  {
    foreach (IPanelGui panelGui in (IEnumerable<IPanelGui>) this._allPanels.Values)
    {
      if (panelGui.IsEnabled)
        panelGui.Hide();
    }
  }

  public IPanelGui OpenPanel(PanelType panel)
  {
    foreach (KeyValuePair<PanelType, IPanelGui> allPanel in (IEnumerable<KeyValuePair<PanelType, IPanelGui>>) this._allPanels)
    {
      if (panel == allPanel.Key)
      {
        if (!allPanel.Value.IsEnabled)
          allPanel.Value.Show();
      }
      else if (allPanel.Value.IsEnabled)
        allPanel.Value.Hide();
    }
    this.enabled = true;
    return this._allPanels[panel];
  }

  public void ClosePanel(PanelType panel)
  {
    if (!this._allPanels.ContainsKey(panel) || !this._allPanels[panel].IsEnabled)
      return;
    this._allPanels[panel].Hide();
  }
}
