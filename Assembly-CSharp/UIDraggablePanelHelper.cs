using UnityEngine;

public static class UIDraggablePanelHelper
{
	public static void SpringToSelection(this UIDraggablePanel dragPanel, GameObject selectedObject, float springStrength)
	{
		dragPanel.SpringToPosition(selectedObject.transform.position, springStrength);
	}

	public static void SpringToSelection(this UIDraggablePanel dragPanel, Vector3 selectedPosition, float springStrength)
	{
		dragPanel.SpringToPosition(selectedPosition, springStrength);
	}

	private static void SpringToPosition(this UIDraggablePanel dragPanel, Vector3 positionToSpring, float springStrength)
	{
		Vector4 clipRange = dragPanel.panel.clipRange;
		Transform cachedTransform = dragPanel.panel.cachedTransform;
		Vector3 position = cachedTransform.localPosition;
		position.x += clipRange.x;
		position.y += clipRange.y;
		position = cachedTransform.parent.TransformPoint(position);
		dragPanel.currentMomentum = Vector3.zero;
		Vector3 a = cachedTransform.InverseTransformPoint(positionToSpring);
		Vector3 b = cachedTransform.InverseTransformPoint(position);
		Vector3 b2 = a - b;
		if (dragPanel.scale.x == 0f)
		{
			b2.x = 0f;
		}
		if (dragPanel.scale.y == 0f)
		{
			b2.y = 0f;
		}
		if (dragPanel.scale.z == 0f)
		{
			b2.z = 0f;
		}
		SpringPanel.Begin(dragPanel.gameObject, cachedTransform.localPosition - b2, springStrength);
	}
}
