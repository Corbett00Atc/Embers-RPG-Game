using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.Characters;

namespace RPG.CameraAndUi
{
	public class TargetHealthBar : MonoBehaviour
	{
		RawImage healthBarRawImage = null;
		Enemy enemyTarget = null; 


		// Use this for initialization
		void Start()
		{
			healthBarRawImage = GetComponent<RawImage>();
		}

		// Update is called once per frame
		void Update()
		{
			if (enemyTarget != null)
			{
				float xValue = (enemyTarget.healthAsPercentage / 2f) - 0.5f;
				healthBarRawImage.uvRect = new Rect(-xValue, 0f, 0.5f, 1f);
			}
		}

		public void SetTarget(Enemy newTarget)
		{
			enemyTarget = newTarget;   
		}

		public void NoTarget()
		{
			enemyTarget = null;
		}
	}
}