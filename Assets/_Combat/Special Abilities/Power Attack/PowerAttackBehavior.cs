using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Characters;

namespace RPG.Combat
{
	public class PowerAttackBehavior : MonoBehaviour, ISpecialAbility
	{
		PowerAttackConfig config;
		PlayerCombatController combatController;
		AudioClip audioClip;
		AudioSource audioSource;

		void Start()
		{
			combatController = GetComponent<PlayerCombatController>();
			audioClip = config.GetAudioClip();
			audioSource = GetComponent<AudioSource>();
		}

		public void SetConfig(PowerAttackConfig configToSet)
		{
			this.config = configToSet;
		}

		public void Use(AbilityParamaters useParams)
		{
			combatController.SetWeaponDamage(useParams.baseDamage + config.GetExtraDamage());
			PlayerParticalEffect();
			PlayAudio();
		}

		private void PlayerParticalEffect()
		{
			var prefab = Instantiate(config.GetParticalPrefab(), this.gameObject.transform);
			var particals = prefab.GetComponent<ParticleSystem>();
			particals.Play();
			Destroy(prefab, particals.main.duration);
		}

		private void PlayAudio()
		{
			audioSource.clip = audioClip;
			audioSource.Play();
		}
	}
}