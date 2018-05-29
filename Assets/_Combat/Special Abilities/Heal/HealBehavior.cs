using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Characters;

namespace RPG.Combat
{	
	public class HealBehavior : MonoBehaviour, ISpecialAbility
	{
		Player player;
		HealConfig config;

		void Start()
		{
			player = GetComponent<Player>();
		}

		public void SetConfig(HealConfig configToSet)
		{
			this.config = configToSet;
		}

		public void Use(AbilityParamaters useParams)
		{
			player.TakeDamage(config.HealAmount() * -1);
			PlayerParticalEffect();
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