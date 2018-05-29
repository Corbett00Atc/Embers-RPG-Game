using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;

namespace RPG.Characters
{
	public class PlayerCombatController : MonoBehaviour 
	{
		[SerializeField] float dodgeImmuneTime = .5f;
 		[SerializeField] float attackDamage = 15f;

		// temp for development
		// 0 is for "weapon art" TODO: Rename to weapon art
		// 1, 2 is for Heal and spell attack
		[SerializeField] SpecialAbility[] abilities;

		private const string ACTION_LOCKED = "actionLocked";
		private const string PRIMARY_ATTACK = "Primary Attack";
		private const string ALT_ATTACK = "Alt Attack";
		private const string MOVEMENT_ROLL = "Roll";
		private const int PLAYER_LAYER = 11;
		private const int IGNORE_ATTACK_LAYER = 17;

		Player player;
		Enemy targetedEnemy = null;
		Animator anim;
		Weapon playerEquippedWeapon;
		Energy energy;

		private float playerBaseDamage;
		
		void Start()
		{
			player = GetComponent<Player>();
			anim = GetComponent<Animator>();
			energy = GetComponent<Energy>();

			SetWeaponData();
			AttachInitialWeaponAbilities();
		}

		void Update()
		{	
			if (Input.GetButtonDown("Roll"))
				HandleRoll();
			if (Input.GetMouseButtonDown(0))
				StandardMeleeAttack();
			if (Input.GetMouseButtonDown(1))
				AttemptSpecialAbility(0);
			if (Input.GetKeyDown("q"))
				AttemptSpecialAbility(1);
			if (Input.GetKeyDown("e"))
				AttemptSpecialAbility(2);
		}
		
		void SetWeaponData()
		{
 			playerEquippedWeapon = player.GetWeaponInUse();
			attackDamage = playerEquippedWeapon.GetWeaponDamage();
			playerBaseDamage = attackDamage;
		}

		void AttachInitialWeaponAbilities()
		{
			for (int abilityIndex = 0; abilityIndex < abilities.Length; abilityIndex++)
			{
				abilities[abilityIndex].AttachComponentTo(player.gameObject);
			}
		}

		void SendDamageToTarget(float damage)
		{
			targetedEnemy.GetComponent<IDamageable>().TakeDamage(damage);
		}

		void StandardMeleeAttack()
		{
			float energyCost = playerEquippedWeapon.GetWeaponEnergyCost();
			if (CanUseAbility(energyCost))
			{
				attackDamage = playerBaseDamage;
				energy.ConsumeEnergy(energyCost);
				anim.CrossFade(PRIMARY_ATTACK, 0.2f);
			}
		}

		void AttemptSpecialAbility(int abilityIndex)
		{
			float energyCost = abilities[abilityIndex].GetEneryCost();
			if (CanUseAbility(energyCost))
			{
				energy.ConsumeEnergy(energyCost);
				var abilityParams = new AbilityParamaters(playerBaseDamage);
				abilities[abilityIndex].Use(abilityParams);
				anim.CrossFade(ALT_ATTACK, 0.2f);
			}
		}

		private bool CanUseAbility(float energyCost)
		{
			if (anim.GetBool(ACTION_LOCKED))
				return false;

			if (energy.IsEnergyAvailable(energyCost) == false)
				return false;

			return true;
		}

		public float GetWeaponDamage()
		{
			return attackDamage;
		}

		public void SetWeaponDamage(float damage)
		{
			attackDamage = damage;
		}

		void HandleRoll()
		{
			anim.CrossFade(MOVEMENT_ROLL, 0.2f);
			StartCoroutine(Immune());
		}

		// layer switch to avoid collisions with projectils and objects
		IEnumerator Immune()
		{
			this.gameObject.layer = IGNORE_ATTACK_LAYER;
			yield return new WaitForSeconds(dodgeImmuneTime);
			this.gameObject.layer = PLAYER_LAYER;
		}
	}
}