using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Characters;

namespace RPG.Combat
{	
	[CreateAssetMenu(menuName=("RPG/Special Ability/Power Attack"))]
	public class PowerAttackConfig : SpecialAbility 
	{
		[Header("Power Attack specific")]
		[SerializeField] float extraDamage = 10f;

		override public void AttachComponentTo(GameObject gameObjectToAttachTo)
		{
			var behaviorComponent = gameObjectToAttachTo.AddComponent<PowerAttackBehavior>();

			behaviorComponent.SetConfig(this);
			behavior = behaviorComponent;
		}

		public float GetExtraDamage()
		{
			return extraDamage;
		}
	}
}
