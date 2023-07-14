// Decompiled with JetBrains decompiler
// Type: DragAndDrop
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using Cmune.DataCenter.Common.Entities;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class DragAndDrop : Singleton<DragAndDrop>
{
  private static int _itemSlotButtonHash = "Button".GetHashCode();
  private float _zoomMultiplier = 1f;
  private float _alphaValue = 1f;
  private bool _isZooming;
  private bool _dragBegin;
  private Rect _draggedControlRect;
  private Vector2 _dragScalePivot;
  private bool _releaseDragItem;

  private DragAndDrop() => AutoMonoBehaviour<UnityRuntime>.Instance.OnGui += new Action(this.OnGui);

  public event Action<IDragSlot> OnDragBegin;

  public int CurrentId { get; private set; }

  public bool IsDragging => this.CurrentId > 0 && this.DraggedItem != null && this.DraggedItem.Item != null;

  public IDragSlot DraggedItem { get; private set; }

  private void OnGui()
  {
    if (this._releaseDragItem)
    {
      this._releaseDragItem = false;
      this.CurrentId = 0;
      this.DraggedItem = (IDragSlot) null;
    }
    if (Event.current.type == UnityEngine.EventType.MouseUp)
      this._releaseDragItem = true;
    if (!this.IsDragging)
      return;
    if (this._dragBegin)
    {
      this._dragBegin = false;
      if (this.OnDragBegin != null)
        this.OnDragBegin(this.DraggedItem);
      MonoRoutine.Start(this.StartDragZoom(0.0f, 1f, 1.25f, 0.1f, 0.8f));
    }
    else
    {
      if (!this._isZooming)
        this._dragScalePivot = GUIUtility.ScreenToGUIPoint(Event.current.mousePosition);
      GUIUtility.ScaleAroundPivot(new Vector2(this._zoomMultiplier, this._zoomMultiplier), this._dragScalePivot);
      GUI.backgroundColor = new Color(1f, 1f, 1f, this._alphaValue);
      GUI.matrix = Matrix4x4.identity;
      GUI.Label(new Rect(this._dragScalePivot.x - 24f, this._dragScalePivot.y - 24f, 48f, 48f), (Texture) this.DraggedItem.Item.Icon, BlueStonez.item_slot_large);
    }
  }

  [DebuggerHidden]
  private IEnumerator StartDragZoom(
    float time,
    float startZoom,
    float endZoom,
    float startAlpha,
    float endAlpha)
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new DragAndDrop.\u003CStartDragZoom\u003Ec__Iterator10()
    {
      time = time,
      startAlpha = startAlpha,
      endAlpha = endAlpha,
      startZoom = startZoom,
      endZoom = endZoom,
      \u003C\u0024\u003Etime = time,
      \u003C\u0024\u003EstartAlpha = startAlpha,
      \u003C\u0024\u003EendAlpha = endAlpha,
      \u003C\u0024\u003EstartZoom = startZoom,
      \u003C\u0024\u003EendZoom = endZoom,
      \u003C\u003Ef__this = this
    };
  }

  public void DrawSlot<T>(
    Rect rect,
    T item,
    Action<int, T> onDropAction = null,
    Color? color = null,
    bool isItemList = false)
    where T : IDragSlot
  {
    int controlId = GUIUtility.GetControlID(DragAndDrop._itemSlotButtonHash, FocusType.Native);
    if ((ApplicationDataManager.Channel == ChannelType.Android || ApplicationDataManager.Channel == ChannelType.IPad || ApplicationDataManager.Channel == ChannelType.IPhone) && Event.current.GetTypeForControl(controlId) == UnityEngine.EventType.MouseDown && isItemList)
      rect.width = 50f;
    switch (Event.current.GetTypeForControl(controlId))
    {
      case UnityEngine.EventType.MouseDown:
        if (Event.current.type == UnityEngine.EventType.Used || !rect.Contains(Event.current.mousePosition))
          break;
        GUIUtility.hotControl = controlId;
        Event.current.Use();
        break;
      case UnityEngine.EventType.MouseUp:
        this.MouseUp<T>(rect, controlId, item.Id, onDropAction);
        break;
      case UnityEngine.EventType.MouseDrag:
        if (GUIUtility.hotControl != controlId)
          break;
        Vector2 screenPoint = GUIUtility.GUIToScreenPoint(new Vector2(rect.x, rect.y));
        this._draggedControlRect = new Rect(screenPoint.x, screenPoint.y, rect.width, rect.height);
        this._dragBegin = true;
        this.DraggedItem = (IDragSlot) item;
        this.CurrentId = GUIUtility.hotControl;
        GUIUtility.hotControl = 0;
        Event.current.Use();
        break;
      case UnityEngine.EventType.Repaint:
        if (!color.HasValue)
          break;
        GUI.color = color.Value;
        BlueStonez.loadoutdropslot_highlight.Draw(rect, GUIContent.none, controlId);
        GUI.color = Color.white;
        break;
    }
  }

  public void DrawSlot<T>(Rect rect, Action<int, T> onDropAction) where T : IDragSlot
  {
    int controlId = GUIUtility.GetControlID(DragAndDrop._itemSlotButtonHash, FocusType.Native);
    if (Event.current.GetTypeForControl(controlId) != UnityEngine.EventType.MouseUp)
      return;
    this.MouseUp<T>(rect, controlId, 0, onDropAction);
  }

  private void MouseUp<T>(Rect rect, int id, int slotId, Action<int, T> onDropAction) where T : IDragSlot
  {
    if (GUIUtility.hotControl == id)
    {
      GUIUtility.hotControl = 0;
      Event.current.Use();
    }
    else
    {
      if (onDropAction == null || this.DraggedItem == null || !rect.Contains(Event.current.mousePosition))
        return;
      onDropAction(slotId, (T) this.DraggedItem);
    }
  }
}
