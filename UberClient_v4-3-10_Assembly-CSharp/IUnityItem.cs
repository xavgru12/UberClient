// Decompiled with JetBrains decompiler
// Type: IUnityItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using UberStrike.Core.Models.Views;
using UberStrike.Core.Types;
using UnityEngine;

public interface IUnityItem
{
  Texture2D Icon { get; set; }

  int ItemId { get; }

  string Name { get; }

  string PrefabName { get; }

  UberstrikeItemType ItemType { get; }

  UberstrikeItemClass ItemClass { get; }

  BaseUberStrikeItemView ItemView { get; }

  MonoBehaviour Prefab { get; }
}
