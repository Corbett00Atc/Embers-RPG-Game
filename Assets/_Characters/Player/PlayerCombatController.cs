using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;

namespace RPG.Characters
{
	public class PlayerCombatController : MonoBehaviour 
	{
		Player player;
		GameObject targetedEnemy = null;
		Animator animator;
		Weapon playerEquippedWeapon;
		
		float timeOfLastAction;
		float lockedTill = 0;
		float attackDamage;
		float attackRange;
		float attackSpeed;

		void Start()
		{
			player = GetComponent<Player>();
			animator = GetComponent<Animator>();
			SetWeaponData();
		}

		void Update()
		{		
			if (player.HasCurrentEnemyTarget())
			{
				targetedEnemy = player.CurrentTarget();
				StandardMeleeAttack();
			}
			else
			{
				targetedEnemy = null;
			}
		}

		void SetWeaponData()
		{
			playerEquippedWeapon = player.GetWeaponInUse();
			attackSpeed = playerEquippedWeapon.GetWeaponAttackTime();
			attackDamage = playerEquippedWeapon.GetWeaponDamage();
			attackRange = playerEquippedWeapon.GetWeaponAttackRange();
		}
		
		void TimeTillNextAction(float delay)
		{
			lockedTill = Time.time + delay;
		}

		void SendDamageToTarget(float damage)
		{
			targetedEnemy.GetComponent<IDamageable>().TakeDamage(damage);
		}

		bool IsInRange(float rangeOfAbility)
		{
			float distanceToTarget = (targetedEnemy.transform.position - transform.position).magnitude;
			return rangeOfAbility >= distanceToTarget;
		}

		/*
			All following methods check first for input.
			Then check if the time that has past since the last used action
			is greater than the time it takes to complete the previous action. 
		*/
		void StandardMeleeAttack()
		{
			if (Input.GetKeyDown("1") && Time.time > lockedTill && IsInRange(attackRange))
			{
				animator.SetBool("Attack", true);
				SendDamageToTarget(attackDamage);
				TimeTillNextAction(attackSpeed);
			}
		}

		void TargetCloseFrontEnemy()
		{
			
		}
	}
}