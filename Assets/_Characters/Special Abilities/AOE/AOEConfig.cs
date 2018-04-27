using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Characters;

namespace RPG.Combat
{	
	[CreateAssetMenu(menuName=("RPG/Special Ability/AOE"))]
	public class AOEConfig : SpecialAbility 
	{
		[Header("AOE Specific")]
		[SerializeField] float perTargetDamage = 20f;
		[SerializeField] float radius = 5f;
		

		override public void AttachComponentTo(GameObject gameObjectToAttachTo)
		{
			var behaviorComponent = gameObjectToAttachTo.AddComponent<AOEBehavior>();

			behaviorComponent.SetConfig(this);
			behavior = behaviorComponent;
		}

		public float GetPerTargetDamage()
		{
			return perTargetDamage;
		}

		public float GetRadius()
		{
			return radius;
		}
	}
}
