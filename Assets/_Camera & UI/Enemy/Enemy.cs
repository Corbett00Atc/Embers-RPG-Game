using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using RPG.Combat;

namespace RPG.Characters
{
	public class Enemy : MonoBehaviour, IDamageable 
	{
		[SerializeField] bool hasMeleeAttack = false;
		[SerializeField] bool hasRangedAttack = false;
		[SerializeField] bool hasSpellAttack = false;

		[SerializeField] float maxHealthPoints = 100f;
		[SerializeField] float aggroRange = 6f;

		[SerializeField] float spellCastRadius = 6f;
		[SerializeField] float spellDamage = 15f;
		[SerializeField] float spellCastTime = .5f;
		[SerializeField] float spellCD = 3f;

		[SerializeField] float rangedAttackRadius = 6f;
		[SerializeField] float rangedAttackCD = 0f;
		[SerializeField] float rangedAttackTime = .5f;
		[SerializeField] float rangedDamage = 3f;

		[SerializeField] float meleeAttackRadius = 2f;
		[SerializeField] float meleeAttackTime = .5f;
		[SerializeField] float meleeAttackDamage = 3f;

		[SerializeField] GameObject rangedAttackProjectile;
		[SerializeField] GameObject spellCastProjectile;
		[SerializeField] GameObject projectileSocket;

		float currentHealthPoints;
		float timeOfLastAction;
		float lockedTill = 0;
		float spellLockedTill = 0;
		float rangedAttackLockedTill = 0;
		float distanceToPlayer;
		float currentTime;

		AICharacterControl aICharacterControl = null;
		GameObject player = null;

		TargetMarker targetMark;
		Renderer rend;


		void Start()
		{
			player = GameObject.FindGameObjectWithTag("Player");
			aICharacterControl = GetComponent<AICharacterControl>();
			currentHealthPoints = maxHealthPoints;
			targetMark = GetComponentInChildren<TargetMarker>();
			rend = targetMark.GetComponentInChildren<Renderer>();
		}

		void Update()
		{
			distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
			currentTime = Time.time;

			if (distanceToPlayer <= aggroRange)
			{
				MakeMoveAttackDecision();
			}
			else
			{
				// do nothing
				aICharacterControl.SetTarget(transform);
			}
		}

		public float healthAsPercentage
		{
			get { return currentHealthPoints / maxHealthPoints; }
		}

		public void TakeDamage(float damage)
		{
			currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0, maxHealthPoints);

			if (currentHealthPoints == 0)
			{
				Destroy(this.gameObject);
			}
		}

		// basic ai decision making based on range
		void MakeMoveAttackDecision()
		{
			// First priority: cast spells
			if (hasSpellAttack && distanceToPlayer <= spellCastRadius 
					&& currentTime >= spellLockedTill && currentTime >= lockedTill)
			{
				aICharacterControl.SetTarget(transform);
				CastSpell();
				spellLockedTill = currentTime + spellCD;
				lockedTill = currentTime + spellCastTime;
			}
			// second priority: melee attack
			else if (hasMeleeAttack && distanceToPlayer <= meleeAttackRadius 
						&& currentTime >= lockedTill)
			{
				aICharacterControl.SetTarget(transform);
				PerformMeleeAttack();
				lockedTill = currentTime + meleeAttackTime;
			}
			// third priority: ranged attack
			else if (hasRangedAttack && distanceToPlayer <= rangedAttackRadius 
						&& currentTime >= lockedTill && currentTime >= rangedAttackLockedTill)
			{
				aICharacterControl.SetTarget(transform);
				PerformRangedAttack();
				lockedTill = currentTime + rangedAttackTime;
				rangedAttackLockedTill = currentTime + rangedAttackCD;
			}
			// fourth priority: move comfortably within attack range
			else if (distanceToPlayer >= meleeAttackRadius * .5 && currentTime >= lockedTill)
			{
				aICharacterControl.SetTarget(player.transform);
			}
		}

		// will customize further in future
		void PerformRangedAttack()
		{
			GameObject newProjectile = Instantiate(rangedAttackProjectile, projectileSocket.transform.position, Quaternion.identity);
			var projectileComponent = newProjectile.GetComponent<Projectile>();
			projectileComponent.DamageCaused(rangedDamage);
			projectileComponent.SetShooter(gameObject);

			Vector3 hitPoint = player.GetComponentInChildren<HitPoint>().GetHitPoint();
			Vector3 unitVectorToPlayer = (hitPoint - projectileSocket.transform.position).normalized;

			float projectileSpeed = projectileComponent.ProjectileSpeed();
			newProjectile.GetComponent<Rigidbody>().velocity = unitVectorToPlayer * projectileSpeed;
		}

		void PerformMeleeAttack()
		{
			player.GetComponent<IDamageable>().TakeDamage(meleeAttackDamage);
		}

		// will customize further in future
		void CastSpell()
		{
			GameObject newProjectile = Instantiate(spellCastProjectile, projectileSocket.transform.position, Quaternion.identity);
			var projectileComponent = newProjectile.GetComponent<Projectile>();
			projectileComponent.DamageCaused(spellDamage);
			projectileComponent.SetShooter(gameObject);

			Vector3 hitPoint = player.GetComponentInChildren<HitPoint>().GetHitPoint();
			Vector3 unitVectorToPlayer = (hitPoint - projectileSocket.transform.position).normalized;

			float projectileSpeed = projectileComponent.ProjectileSpeed();
			newProjectile.GetComponent<Rigidbody>().velocity = unitVectorToPlayer * projectileSpeed;
		}

		void OnDrawGizmos()
		{
			// draw attack range 
			Gizmos.color = new Color(255f, 0f, 0f, .75f);
			Gizmos.DrawWireSphere(transform.position, meleeAttackRadius);

			// draw aggro range 
			Gizmos.color = new Color(0f, 255f, 0f, .75f);
			Gizmos.DrawWireSphere(transform.position, aggroRange);

			// draw ranged range 
			Gizmos.color = new Color(0f, 0f, 255f, .75f);
			Gizmos.DrawWireSphere(transform.position, rangedAttackRadius);

			// draw spell range 
			Gizmos.color = new Color(0f, 125f, 125f, .75f);
			Gizmos.DrawWireSphere(transform.position, spellCastRadius);
		}

		public void MarkTarget()
		{
			rend.enabled = true;
	
		}

		public void UnMarkTarget()
		{
			rend.enabled = false;
		}
	}
}