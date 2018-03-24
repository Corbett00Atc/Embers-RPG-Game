using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.Characters;

namespace RPG.CameraAndUi
{
	public class TargetNameUI : MonoBehaviour 
	{
		Text targetName;

		void Start()
		{
			targetName = GetComponent<Text>();
		}

		public void SetTarget(Enemy newTarget)
		{
			targetName.text = newTarget.name;   
		}

		public void NoTarget()
		{
			targetName.text = "";   
		}
	}
}