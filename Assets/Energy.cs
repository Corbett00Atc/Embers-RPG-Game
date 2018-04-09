using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Characters
{
	public class Energy : MonoBehaviour 
	{
		[SerializeField] RawImage energyBar;
		[SerializeField] float maxEnergyPoints = 100f;
		[SerializeField] float costPerMeleeSwing = 25f;
		[SerializeField] float energyRegenDelay = 3f;


		float currentEnergyPoints;

		void Start()
		{
			currentEnergyPoints = maxEnergyPoints;
			StartCoroutine(EnergyRegen());
		}

		// moves energy bar based on current energy as percent
		public void UpdateCurrentEnergy()
		{
			currentEnergyPoints = Mathf.Clamp(
				currentEnergyPoints - costPerMeleeSwing,
				0, 
				maxEnergyPoints
			);
		}

		void Update()
		{
			SetEnergyBar();
		}

		public bool HasEnergyToAttack()
		{
			return (currentEnergyPoints - costPerMeleeSwing >= 0) ? true : false;
		}

		void SetEnergyBar()
		{
			energyBar.uvRect = new Rect(
				-((currentEnergyPoints / maxEnergyPoints) / 2f) - 0.5f, 
				0f, 
				0.5f, 
				1f
				);
		}

		IEnumerator EnergyRegen()
		{
			while (true)
			{	
				// ticks 1% of energy at user defines speed
				float energyTick = maxEnergyPoints / 100;
				
				if (currentEnergyPoints < maxEnergyPoints)
				{
					currentEnergyPoints += energyTick;

					if (currentEnergyPoints > maxEnergyPoints)
						currentEnergyPoints = maxEnergyPoints;
				}		

				yield return new WaitForSeconds(energyRegenDelay / 100);
			}
		}
	}
}