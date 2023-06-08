using System;
using System.Collections;
using UnityEngine;

public class UnityRuntime : AutoMonoBehaviour<UnityRuntime>
{
	[SerializeField]
	private bool showInvocationList;

	public event Action OnGui;

	public event Action OnUpdate;

	public event Action OnLateUpdate;

	public event Action OnFixedUpdate;

	public event Action OnDrawGizmo;

	public event Action<bool> OnAppFocus;

	private void FixedUpdate()
	{
		if (this.OnFixedUpdate != null)
		{
			this.OnFixedUpdate();
		}
	}

	private void Update()
	{
		if (this.OnUpdate != null)
		{
			this.OnUpdate();
		}
	}

	private void LateUpdate()
	{
		if (this.OnLateUpdate != null)
		{
			this.OnLateUpdate();
		}
	}

	private void OnGUI()
	{
		if (this.OnGui != null)
		{
			this.OnGui();
		}
		if (!showInvocationList)
		{
			return;
		}
		GUILayout.BeginArea(new Rect(10f, 100f, 400f, Screen.height - 200));
		if (this.OnUpdate != null)
		{
			Delegate[] invocationList = this.OnUpdate.GetInvocationList();
			Delegate[] array = invocationList;
			foreach (Delegate @delegate in array)
			{
				GUILayout.Label("Update: " + @delegate.Method.DeclaringType.Name + "." + @delegate.Method.Name);
			}
		}
		if (this.OnFixedUpdate != null)
		{
			Delegate[] invocationList2 = this.OnFixedUpdate.GetInvocationList();
			Delegate[] array2 = invocationList2;
			foreach (Delegate delegate2 in array2)
			{
				GUILayout.Label("FixedUpdate: " + delegate2.Method.DeclaringType.Name + "." + delegate2.Method.Name);
			}
		}
		if (this.OnAppFocus != null)
		{
			Delegate[] invocationList3 = this.OnAppFocus.GetInvocationList();
			Delegate[] array3 = invocationList3;
			foreach (Delegate delegate3 in array3)
			{
				GUILayout.Label("OnApplicationFocus: " + delegate3.Method.DeclaringType.Name + "." + delegate3.Method.Name);
			}
		}
		GUILayout.EndArea();
	}

	private void OnApplicationFocus(bool focus)
	{
		if (this.OnAppFocus != null)
		{
			this.OnAppFocus(focus);
		}
	}

	public static Coroutine StartRoutine(IEnumerator routine)
	{
		if (AutoMonoBehaviour<UnityRuntime>.IsRunning)
		{
			return AutoMonoBehaviour<UnityRuntime>.Instance.StartCoroutine(routine);
		}
		return null;
	}
}
