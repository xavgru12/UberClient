using System;
using UnityEngine;

[Serializable]
public class SpringGrenadeConfiguration : QuickItemConfiguration
{
	[SerializeField]
	private Vector3 _jumpDirection = Vector3.up;

	[CustomProperty("Force")]
	[SerializeField]
	private int _force = 1250;

	[CustomProperty("LifeTime")]
	[SerializeField]
	private int _lifeTime = 15;

	[SerializeField]
	[CustomProperty("Sticky")]
	private bool _isSticky = true;

	[SerializeField]
	private int _speed = 10;

	public Vector3 JumpDirection => _jumpDirection;

	public int Force => _force;

	public int LifeTime => _lifeTime;

	public bool IsSticky => _isSticky;

	public int Speed => _speed;
}
