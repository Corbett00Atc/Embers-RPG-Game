using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.Characters;

namespace RPG.CameraAndUi
{
	public class TargetUI : MonoBehaviour 
	{
		[SerializeField] TargetNameUI targetName;
		[SerializeField] TargetHealthBar healthBar;

		RawImage[] healthBarImages;
		

		void Start()
		{
			healthBarImages = GetComponentsInChildren<RawImage>();
			DisableTargetUI();
		}

		public void SetUIEnemyTarget(Enemy enemy)
		{
			EnableTargetUI();
			healthBar.SetTarget(enemy);
			targetName.SetTarget(enemy);
		}

		public void ClearTarget()
		{
			DisableTargetUI();
			healthBar.NoTarget();
			targetName.NoTarget();
		}

		void DisableTargetUI()
		{
			foreach(RawImage r in healthBarImages)
			{
				r.enabled = false;
			}
		}

		void EnableTargetUI()
		{
			foreach(RawImage r in healthBarImages)
			{
				r.enabled = true;
			}
		}
	}
}