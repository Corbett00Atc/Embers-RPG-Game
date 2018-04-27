using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
	public class WeaponHook : MonoBehaviour 
	{
		[SerializeField] GameObject[] damageCollider;

		public void OpenDamageColliders()
		{
			for (int i = 0; i < damageCollider.Length; i++)
			{
				damageCollider[i].SetActive(true);
			}
		}

		public void CloseDamageColliders()
		{
			for (int i = 0; i < damageCollider.Length; i++)
			{
				damageCollider[i].SetActive(false);
			}
		}

		void Start()
		{
			CloseDamageColliders();
		}
	}
}