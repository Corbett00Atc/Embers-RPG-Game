using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Characters;

namespace RPG.Combat
{	
	[CreateAssetMenu(menuName=("RPG/Special Ability/Heal"))]
	public class HealConfig : SpecialAbility 
	{
		[Header("Heal Specific")]
		[SerializeField] float healAmount = 50f;

		override public void AttachComponentTo(GameObject gameObjectToAttachTo)
		{
			var behaviorComponent = gameObjectToAttachTo.AddComponent<HealBehavior>();

			behaviorComponent.SetConfig(this);
			behavior = behaviorComponent;
		}

		public float HealAmount()
		{
			return healAmount;
		}
	}
}