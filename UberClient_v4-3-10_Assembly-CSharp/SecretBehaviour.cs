// Decompiled with JetBrains decompiler
// Type: SecretBehaviour
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public class SecretBehaviour : MonoBehaviour
{
  [SerializeField]
  private SecretBehaviour.Door[] _doors;

  private void Awake()
  {
    foreach (SecretBehaviour.Door door in this._doors)
    {
      foreach (SecretTrigger secretTrigger in door.Trigger)
        secretTrigger.SetSecretReciever(this);
    }
  }

  public void SetTriggerActivated(SecretTrigger trigger)
  {
    foreach (SecretBehaviour.Door door in this._doors)
      door.CheckAllTriggers();
  }

  [Serializable]
  public class Door
  {
    public string _description;
    [SerializeField]
    private SecretDoor _door;
    [SerializeField]
    private SecretTrigger[] _trigger;

    public SecretTrigger[] Trigger => this._trigger;

    public void CheckAllTriggers()
    {
      bool flag = true;
      foreach (SecretTrigger secretTrigger in this._trigger)
        flag &= (double) secretTrigger.ActivationTimeOut > (double) Time.time;
      if (!flag)
        return;
      this._door.Open();
    }
  }
}
