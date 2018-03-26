using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Utility
{
	public class DestroyAfterDelay : MonoBehaviour
	{
		[SerializeField] float destroyDelay = 1;
		
		void Start() 
		{ Destroy(this, destroyDelay); }
	}
}