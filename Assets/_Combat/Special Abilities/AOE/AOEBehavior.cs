using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Characters;

namespace RPG.Combat
{
	public class AOEBehavior : MonoBehaviour, ISpecialAbility
	{
		private const int PLAYER_LAYER = 11;

		AOEConfig config;

		public void SetConfig(AOEConfig configToSet)
		{
			this.config = configToSet;
		}

		public void Use(AbilityParamaters useParams)
		{
			DealRadialDamage(useParams);
			PlayerParticalEffect();
		}

		private void DealRadialDamage(AbilityParamaters useParams)
		{
			RaycastHit[] hits = Physics.SphereCastAll(
				transform.position,
				config.GetRadius(),
				Vector3.forward,
				config.GetRadius()
			);

			foreach (RaycastHit hit in hits)
			{
				var damageable = hit.collider.gameObject.GetComponent<IDamageable>();
				if (damageable != null)
				{
					if (hit.collider.gameObject.layer != PLAYER_LAYER)
					{
						float damageToDeal = config.GetPerTargetDamage() + useParams.baseDamage;
						damageable.TakeDamage(damageToDeal);
					}
				}
			}
		}

		private void PlayerParticalEffect()
		{
			var prefab = Instantiate(config.GetParticalPrefab(), this.gameObject.transform);
			var particals = prefab.GetComponent<ParticleSystem>();
			particals.Play();
			Destroy(prefab, particals.main.duration);
		}
	}
}

