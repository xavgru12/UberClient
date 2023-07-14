// Decompiled with JetBrains decompiler
// Type: LotteryShopGUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using Cmune.DataCenter.Common.Entities;
using System;
using System.Collections.Generic;
using UnityEngine;

public class LotteryShopGUI
{
  private int scrollHeight;
  private Vector2 scroll = Vector2.zero;
  private Dictionary<int, float> _alpha = new Dictionary<int, float>();

  public void Draw(Rect position)
  {
    float height = Mathf.Max(position.height, (float) this.scrollHeight);
    this.scroll = GUITools.BeginScrollView(position, this.scroll, new Rect(0.0f, 0.0f, position.width - 17f, height), useVertical: true);
    int top = 4;
    foreach (int num1 in Enum.GetValues(typeof (BundleCategoryType)))
    {
      BundleCategoryType bundle = (BundleCategoryType) num1;
      if (bundle != BundleCategoryType.None)
      {
        int num2 = 0;
        List<LotteryShopItem> items;
        if (Singleton<LotteryManager>.Instance.TryGetBundle(bundle, out items))
        {
          GUI.Label(new Rect(4f, (float) (top + 4), position.width - 20f, 20f), ((Enum) bundle).ToString(), BlueStonez.label_interparkbold_18pt_left);
          top += 30;
          foreach (LotteryShopItem lotteryShopItem in items)
          {
            this.DrawLotterySlot(new Rect(num2 % 2 != 1 ? 0.0f : 187f, (float) top, 188f, 95f), lotteryShopItem);
            top += num2 % 2 != 1 ? 0 : 94;
            ++num2;
          }
        }
        if (num2 > 0)
        {
          if (num2 % 2 == 1)
            top += 94;
          GUI.Label(new Rect(4f, (float) top, position.width - 8f, 1f), GUIContent.none, BlueStonez.horizontal_line_grey95);
          top += 4;
        }
      }
    }
    this.scrollHeight = top;
    GUITools.EndScrollView();
  }

  private void DrawLotterySlot(Rect position, LotteryShopItem item)
  {
    bool flag = position.Contains(Event.current.mousePosition);
    if (!this._alpha.ContainsKey(item.Id))
      this._alpha[item.Id] = 0.0f;
    this._alpha[item.Id] = Mathf.Lerp(this._alpha[item.Id], !flag ? 0.0f : 1f, Time.deltaTime * (!flag ? 10f : 3f));
    GUI.BeginGroup(position);
    GUI.color = new Color(1f, 1f, 1f, this._alpha[item.Id]);
    if (GUI.Button(new Rect(2f, 2f, position.width - 4f, 79f), GUIContent.none, BlueStonez.gray_background))
      this.UseLotteryItem(item);
    GUI.color = Color.white;
    item.Icon.Draw(new Rect(4f, 4f, 75f, 75f));
    GUI.Label(new Rect(81f, 0.0f, position.width - 80f, 44f), item.Name, BlueStonez.label_interparkbold_13pt_left);
    if (GUI.Button(new Rect(81f, 51f, position.width - 110f, 20f), item.Price.PriceTag(tooltip: item.Description), BlueStonez.buttongold_medium))
      this.UseLotteryItem(item);
    GUI.EndGroup();
  }

  private void UseLotteryItem(LotteryShopItem item) => item.Use();
}
