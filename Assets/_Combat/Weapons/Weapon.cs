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
		[SerializeField] float weaponAttackTime;
		[SerializeField] float weaponAttackRange;


        public Transform gripTransform;

        public GameObject GetWeaponPrefab()
        { return weaponPrefab; }

        public float GetWeaponDamage()
        { return weaponDamage; }

        public float GetWeaponAttackTime()
        { return weaponAttackTime; } 

        public float GetWeaponAttackRange()
        { return weaponAttackRange; } 

        // prevents asset pack crashes
        private void MakeAnimationEventless()
        { attackAnimation.events = new AnimationEvent[0]; }

        public AnimationClip GetAttackAnimClip()
        {
            MakeAnimationEventless();
            return attackAnimation;
        }
    }
}