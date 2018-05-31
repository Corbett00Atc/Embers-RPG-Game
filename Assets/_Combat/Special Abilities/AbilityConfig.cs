using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
	public interface ISpecialAbility
	{
		void Use(AbilityParamaters useParams);
	}

	public struct AbilityParamaters
	{
		public float baseDamage;

		public AbilityParamaters(float baseDamage)
		{
			this.baseDamage = baseDamage;
		}
	}
	
	public abstract class AbilityConfig : ScriptableObject 
	{
		[Header("Special Ability General")]
		[SerializeField] float energyCost = 10;
		[SerializeField] GameObject particalPrefab = null;
		[SerializeField] AudioClip audioClip = null;
		[SerializeField] AnimationClip anim = null;

		protected ISpecialAbility behavior;

		public float GetEneryCost() { return energyCost; }
		public GameObject GetParticalPrefab(){ return particalPrefab; }
		public AudioClip GetAudioClip() { return audioClip; }
		public AnimationClip GetAnimation() { return anim; }

		abstract public void AttachComponentTo(GameObject gameObjectToAttachTo);

		public void Use(AbilityParamaters useParams) 
		{ 
			behavior.Use(useParams); 
		}
	}
}