using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using RPG.CameraAndUi;
using RPG.Combat;
using UnityEngine.SceneManagement;

namespace RPG.Characters
{
	public class Player : MonoBehaviour, IDamageable 
	{
		[SerializeField] const int enemyLayerNumber = 9;
		[SerializeField] TargetUI targetUI;
		[SerializeField] float maxHealthPoints = 100f;
		[SerializeField] Weapon weaponInUse; 
		[SerializeField] AnimatorOverrideController animatorOverrideController;
		[SerializeField] GameObject hitEffect;
		[SerializeField] float deathResetTime = 2;

		float currentHealthPoints;
		HitPoint hitPoint;
		WeaponHook weaponDamageCollider;
		Animator anim;

		public float healthAsPercentage 
		{ get { return currentHealthPoints / maxHealthPoints; }	}
		
		void Start()
		{
			PutWeaponInHand();
			SetRuntimeAnimatorController();
			SetHealthPoints();
			hitPoint = GetComponentInChildren<HitPoint>();
			weaponDamageCollider = GetComponentInChildren<WeaponHook>();
			anim = GetComponent<Animator>();
		} 
		
		public void TakeDamage(float damage)
		{
			ReduceHealth(damage);

			bool playerDies = (currentHealthPoints - damage <= 0);
			if (playerDies)
				StartCoroutine(KillPlayer());
		}

		private void ReduceHealth(float damage)
		{
			CreateHitEffect();
			currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0, maxHealthPoints); 
		}

		public Weapon GetWeaponInUse()
		{ return weaponInUse;}

		void SetHealthPoints()
		{ currentHealthPoints = maxHealthPoints; }

		void CreateHitEffect()
		{
			Instantiate(hitEffect, hitPoint.transform);
		}

		void SetRuntimeAnimatorController()
		{
			var animator = GetComponent<Animator>();
			animator.runtimeAnimatorController = animatorOverrideController;
			animatorOverrideController["Default Attack"] = weaponInUse.GetAttackAnimClip();
		}

		void PutWeaponInHand()
		{
			var weaponPrefab = weaponInUse.GetWeaponPrefab();
			GameObject dominantHand = RequestDominantHand();
			var weapon = Instantiate(weaponPrefab, dominantHand.transform);

			// sets weapons transforme to where the weapon touches weapon socket
			weapon.transform.localPosition = weaponInUse.gripTransform.localPosition;
			weapon.transform.localRotation = weaponInUse.gripTransform.localRotation;
		}

		GameObject RequestDominantHand()
		{
			var dominantHands = GetComponentsInChildren<DominantHand>();
			int numberOfDominantHands = dominantHands.Length;

			Assert.IsFalse(numberOfDominantHands <0 , "No dominant hand found.");
			Assert.IsFalse(numberOfDominantHands > 1, "Multiple dominant hand scripts on player");
			
			return dominantHands[0].gameObject;
		}

		IEnumerator KillPlayer()
		{
			anim.SetTrigger("PlayerDied");
			yield return new WaitForSeconds(deathResetTime);
			SceneManager.LoadScene(0);
		}

		public void OpenDamageColliders()
        {
            weaponDamageCollider.OpenDamageColliders();
        }

        public void CloseDamageColliders()
        {
            weaponDamageCollider.CloseDamageColliders();
        }
	}
}