using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(menuName = "RPG/Weapon")]
    public class Weapon : ScriptableObject 
    {
        [SerializeField] GameObject weaponPrefab;
        [SerializeField] AnimationClip attackAnimation;
		[SerializeField] float weaponDamage;
		[SerializeField] float weaponSwingCost;

        public Transform gripTransform;

        public GameObject GetWeaponPrefab()
        { return weaponPrefab; }

        public float GetWeaponDamage()
        { return weaponDamage; }

        public float GetWeaponEnergyCost()
        { return weaponSwingCost; } 

        // prevents asset pack crashes
        private void MakeAnimationEventless()
        { attackAnimation.events = new AnimationEvent[0]; }

        public AnimationClip GetAttackAnimClip()
        {
            //MakeAnimationEventless();
            return attackAnimation;
        }
    }
}