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
	
	public abstract class SpecialAbility : ScriptableObject 
	{
		[Header("Special Ability General")]
		[SerializeField] float energyCost = 10;

		protected ISpecialAbility behavior;

		abstract public void AttachComponentTo(GameObject gameObjectToAttachTo);

		public void Use(AbilityParamaters useParams)
		{
			behavior.Use(useParams);
		}

		public float GetEneryCost()
		{
			return energyCost;
		}
	}
}