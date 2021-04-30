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
		transform.position = _target.position + _offset;
	}
}
