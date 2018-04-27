using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Characters;

namespace RPG.Combat
{
	public class DamageCollider : MonoBehaviour 
	{
		void OnTriggerEnter(Collider other)
		{
			if (other.gameObject.layer == 9)
			{
				other.gameObject.GetComponent<IDamageable>().TakeDamage(
					this.gameObject.GetComponentInParent<PlayerCombatController>().GetWeaponDamage()
					);

				print(this.gameObject.GetComponentInParent<PlayerCombatController>().GetWeaponDamage());
			}
		}
	}
}