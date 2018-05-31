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
		AudioClip audioClip;
		AudioSource audioSource;

		void Start()
		{
			player = GetComponent<Player>();
			audioClip = config.GetAudioClip();
			audioSource = GetComponent<AudioSource>();
		}

		public void SetConfig(HealConfig configToSet)
		{
			this.config = configToSet;
		}

		public void Use(AbilityParamaters useParams)
		{
			player.ReceiveHealth(config.HealAmount());
			PlayerParticalEffect();
			PlayAudio();
		}

		private void PlayAudio()
		{
			audioSource.clip = audioClip;
			audioSource.Play();
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