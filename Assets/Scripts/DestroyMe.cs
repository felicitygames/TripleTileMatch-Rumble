using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyMe : MonoBehaviour
{
    public float destroyTime;

	void Start () {
        Invoke("Destroy", destroyTime);	
	}
	
	void Destroy()
    {
        Destroy(this.gameObject);
    }
}
