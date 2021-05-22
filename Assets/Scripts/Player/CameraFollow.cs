using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	[SerializeField] Transform _target;
	Vector3 _offset;

	void Start()
	{
		_offset = transform.position - _target.position;
	}

	void LateUpdate()
	{
		if(_target != null)
			transform.position = _target.position + _offset;
	}

	public void SetNewTarget(Transform newTarget)
    {
		_target = newTarget;
		transform.position = _target.position + _offset;
	}
}
