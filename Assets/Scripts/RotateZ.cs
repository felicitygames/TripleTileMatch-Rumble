using UnityEngine;
using System.Collections;

public class RotateZ : MonoBehaviour
{
	Vector3 angle;
	public float speed = 40;
	void Start()
	{
		angle = transform.eulerAngles;
	}

	void Update()
	{
		angle.z += Time.deltaTime * speed;
		transform.eulerAngles = -angle;
	}

}
