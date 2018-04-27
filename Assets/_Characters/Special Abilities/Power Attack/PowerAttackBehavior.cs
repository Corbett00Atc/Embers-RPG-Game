using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;

namespace RPG.Characters
{
	public class PowerAttackBehavior : MonoBehaviour, ISpecialAbility
	{
		PowerAttackConfig config;
		PlayerCombatController combatController;

		void Start()
		{
			combatController = GetComponent<PlayerCombatController>();
		}

		public void SetConfig(PowerAttackConfig configToSet)
		{
			this.config = configToSet;
		}

		public void Use(AbilityParamaters useParams)
		{
			combatController.SetWeaponDamage(useParams.baseDamage + config.GetExtraDamage());
		}
	}
}