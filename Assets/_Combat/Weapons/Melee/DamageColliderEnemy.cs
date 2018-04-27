using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Characters;

namespace RPG.Combat
{
	public class DamageColliderEnemy : MonoBehaviour 
	{
		void OnTriggerEnter(Collider other)
		{
			if (other.gameObject.layer == 11)
			{
				other.gameObject.GetComponent<IDamageable>().TakeDamage(
					this.gameObject.GetComponentInParent<Enemy>().GetMeleeDamage()
					);

				this.gameObject.GetComponentInParent<WeaponHook>().CloseDamageColliders();
			}
		}
	}
}