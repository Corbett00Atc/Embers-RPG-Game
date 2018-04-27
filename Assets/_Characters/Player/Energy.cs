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
		[SerializeField] float energyPointsPerSecond = 10f;

		float currentEnergyPoints;

		void Start()
		{
			currentEnergyPoints = maxEnergyPoints;
		}

		void Update()
		{
			if (currentEnergyPoints < maxEnergyPoints)
			{
				AddEnergyPoints();
				SetEnergyBar();
			}
		}

		void AddEnergyPoints()
		{
			var pointsToAdd = energyPointsPerSecond * Time.deltaTime;
			currentEnergyPoints = Mathf.Clamp(currentEnergyPoints + pointsToAdd, 0, maxEnergyPoints);
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

		// moves energy bar based on current energy as percent
		public void ConsumeEnergy(float amount)
		{
			currentEnergyPoints = Mathf.Clamp(
				currentEnergyPoints - amount,
				0, 
				maxEnergyPoints
			);
		}

		public bool IsEnergyAvailable(float amount)
		{
			return amount < currentEnergyPoints;
		}
	}
}