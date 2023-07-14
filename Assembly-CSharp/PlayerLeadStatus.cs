// Decompiled with JetBrains decompiler
// Type: PlayerLeadStatus
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

internal class PlayerLeadStatus : Singleton<PlayerLeadStatus>
{
  private PlayerLeadStatus.LeadState _lastLead;

  private PlayerLeadStatus()
  {
  }

  public void ResetPlayerLead() => this._lastLead = PlayerLeadStatus.LeadState.None;

  public void PlayLeadAudio(int myKills, int otherKills, bool isLeading, bool playAudio = true)
  {
    if (myKills == 0 && otherKills == 0)
      this._lastLead = PlayerLeadStatus.LeadState.None;
    else if (isLeading)
    {
      if (playAudio && this._lastLead != PlayerLeadStatus.LeadState.Me && myKills > 0)
        SfxManager.Play2dAudioClip(GameAudio.TakenLead, 0.5f);
      this._lastLead = PlayerLeadStatus.LeadState.Me;
    }
    else if (this._lastLead == PlayerLeadStatus.LeadState.Me)
    {
      this._lastLead = PlayerLeadStatus.LeadState.Tied;
      if (!playAudio || myKills <= 0)
        return;
      SfxManager.Play2dAudioClip(GameAudio.TiedLead, 0.5f);
    }
    else if (this._lastLead == PlayerLeadStatus.LeadState.Tied)
    {
      this._lastLead = PlayerLeadStatus.LeadState.Others;
      if (!playAudio)
        return;
      SfxManager.Play2dAudioClip(GameAudio.LostLead, 0.5f);
    }
    else if (myKills == otherKills && myKills > 0)
    {
      this._lastLead = PlayerLeadStatus.LeadState.Tied;
      if (!playAudio)
        return;
      SfxManager.Play2dAudioClip(GameAudio.TiedLead, 0.5f);
    }
    else
      this._lastLead = PlayerLeadStatus.LeadState.Others;
  }

  public bool IsLeading => this._lastLead == PlayerLeadStatus.LeadState.Me;

  public void OnDeathMatchOver()
  {
    if (this._lastLead == PlayerLeadStatus.LeadState.Me)
    {
      SfxManager.StopAll2dAudio();
      SfxManager.Play2dAudioClip(GameAudio.YouWin, 1f);
    }
    else if (this._lastLead == PlayerLeadStatus.LeadState.Others)
    {
      SfxManager.StopAll2dAudio();
      SfxManager.Play2dAudioClip(GameAudio.GameOver, 1f);
    }
    else
    {
      SfxManager.StopAll2dAudio();
      SfxManager.Play2dAudioClip(GameAudio.Draw, 1f);
    }
  }

  private enum LeadState
  {
    None,
    Me,
    Tied,
    Others,
  }
}
