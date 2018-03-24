﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;

namespace RPG.Characters
{
	public class PlayerCombatController : MonoBehaviour 
	{
		Player player;
		Enemy targetedEnemy = null;
		Animator animator;
		Weapon playerEquippedWeapon;
		PlayerTargeting targeting;
		
		float timeOfLastAction;
		float lockedTill = 0;
		float attackDamage;
		float attackRange;
		float attackSpeed;

		void Start()
		{
			player = GetComponent<Player>();
			animator = GetComponent<Animator>();
			targeting = GetComponentInChildren<PlayerTargeting>();

			SetWeaponData();
		}

		void Update()
		{	
			GetTargetedEnemy();

			if (Input.GetMouseButtonDown(0))
				StandardMeleeAttack();
		}

		void GetTargetedEnemy()
		{
			targetedEnemy = targeting.GetCurrentTarget();
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
			if (Time.time > lockedTill && targetedEnemy != null)
			{
				if (IsInRange(attackRange))
				{
					animator.SetBool("Attack", true);
					SendDamageToTarget(attackDamage);
					TimeTillNextAction(attackSpeed);
				}
				else
				{
					print("Not in range for StandardMeleeAttack()");
				}
			}
		}
	}
}