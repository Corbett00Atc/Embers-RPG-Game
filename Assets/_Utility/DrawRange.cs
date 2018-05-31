using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawRange : MonoBehaviour 
{
	[SerializeField] float radius;
	
	void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, radius);
	}
}
