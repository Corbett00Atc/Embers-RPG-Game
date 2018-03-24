using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
	public class HitPoint : MonoBehaviour 
	{

		public Vector3 GetHitPoint()
		{
			return transform.position;
		}
	}
}