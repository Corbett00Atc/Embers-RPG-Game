using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;
using UnityEngine.AI;

namespace RPG.Characters
{
	public class Enemy : MonoBehaviour, IDamageable 
	{
		[SerializeField] bool hasMeleeAttack = false;
		[SerializeField] bool hasRangedAttack = false;
		[SerializeField] bool hasSpellAttack = false;

		[SerializeField] float immuneAfterHitDelay = .1f;

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
		float timeSinceLastHit;

		bool isFacing = false;

		public bool immuneToDamage = true;

		AICharacterControl aICharacterControl = null;
		Player player = null;

		TargetMarker targetMark;
		Renderer rend;
		Animator anim;
		WeaponHook weaponDamageCollider;
		EnemyUI enemyUI;

		void Start()
		{
			player = FindObjectOfType<Player>();
			aICharacterControl = GetComponent<AICharacterControl>();
			targetMark = GetComponentInChildren<TargetMarker>();
			rend = targetMark.GetComponentInChildren<Renderer>();
			weaponDamageCollider = GetComponentInChildren<WeaponHook>();
			anim = GetComponent<Animator>();
			enemyUI = GetComponentInChildren<EnemyUI>();

			currentHealthPoints = maxHealthPoints;
			timeSinceLastHit = immuneAfterHitDelay;		
		}

		void Update()
		{
			distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
			currentTime = Time.time;

			UpdateImmunityGracePeriod();

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

		private void UpdateImmunityGracePeriod()
		{
			if (timeSinceLastHit >= immuneAfterHitDelay)
			{
				immuneToDamage = false;
				return;
			}

			timeSinceLastHit += Time.deltaTime;
		}

		public void TakeDamage(float damage)
		{
			enemyUI.DisplayHealthbar(true);

			if (immuneToDamage)
				return;
			else 
			{	
				timeSinceLastHit = 0;
				immuneToDamage = true;
			}

			currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0, maxHealthPoints);
			anim.CrossFade("Hit", .02f);

			if (currentHealthPoints == 0)
			{
				Destroy(this.gameObject);
			}
		}

		// basic ai decision making based on range
		void MakeMoveAttackDecision()
		{
			if (player.healthAsPercentage <= Mathf.Epsilon)
				return;

			if (anim.GetBool("actionLocked") == true)
				return;

			// First priority: cast spells
			if (hasSpellAttack)
			{
				Vector3 dir = (player.transform.position - transform.position).normalized;
				float dot = Vector3.Dot(dir, transform.forward);

				if (dot > .95f)
					isFacing = true;
				else
					isFacing = false;
			}
			
			if (hasSpellAttack && isFacing && distanceToPlayer <= spellCastRadius && 
					currentTime >= spellLockedTill && currentTime >= lockedTill)
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

			if (distanceToPlayer > aggroRange)
				enemyUI.DisplayHealthbar(false);
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
			anim.CrossFade("Primary Attack", 0.2f);
		}

		void CastSpell()
		{
			StartCoroutine(StartSpellCastSequence());
		}

		void LaunchSpellProjectile(Vector3 hitPoint)
		{
			GameObject newProjectile = Instantiate(spellCastProjectile, projectileSocket.transform.position, Quaternion.identity);
			var projectileComponent = newProjectile.GetComponent<Projectile>();
			projectileComponent.DamageCaused(spellDamage);
			projectileComponent.SetShooter(gameObject);

			Vector3 unitVectorToPlayer = (hitPoint - projectileSocket.transform.position).normalized;

			float projectileSpeed = projectileComponent.ProjectileSpeed();
			newProjectile.GetComponent<Rigidbody>().velocity = unitVectorToPlayer * projectileSpeed;
		}

		IEnumerator StartSpellCastSequence()
		{
			anim.CrossFade("Spell Cast", 0.2f);
			yield return new WaitForSeconds(spellCastTime);
			
			Vector3 hitPoint = player.GetComponentInChildren<HitPoint>().GetHitPoint();
			LaunchSpellProjectile(hitPoint);
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

		public void OpenDamageColliders()
        {
            weaponDamageCollider.OpenDamageColliders();
        }

        public void CloseDamageColliders()
        {
            weaponDamageCollider.CloseDamageColliders();
        }

		public float GetMeleeDamage()
		{
			return meleeAttackDamage;
		}
	}
}