
using System;
using System.Collections.Generic;
using UberStrike.Core.Models.Views;

namespace UberStrike.Core.ViewModel
{
  [Serializable]
  public class UberstrikeLevelViewModel
  {
    public List<MapView> Maps { get; set; }

    public UberstrikeLevelViewModel() => this.Maps = new List<MapView>();
  }
}
