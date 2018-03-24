using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using RPG.CameraAndUi;
using RPG.Combat;

namespace RPG.Characters
{
	public class Player : MonoBehaviour, IDamageable 
	{
		[SerializeField] const int enemyLayerNumber = 9;
		[SerializeField] TargetUI targetUI;
		[SerializeField] float maxHealthPoints = 100f;
		[SerializeField] Weapon weaponInUse; 
		[SerializeField] AnimatorOverrideController animatorOverrideController;

		float currentHealthPoints;

		public float healthAsPercentage 
		{ get { return currentHealthPoints / maxHealthPoints; }	}

		public Weapon GetWeaponInUse()
		{ return weaponInUse;}

		public void TakeDamage(float damage)
		{ currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0, maxHealthPoints); }

		void SetHealthPoints()
		{ currentHealthPoints = maxHealthPoints; }

		void Start()
		{
			PutWeaponInHand();
			SetRuntimeAnimatorController();
			SetHealthPoints();
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
	}
}