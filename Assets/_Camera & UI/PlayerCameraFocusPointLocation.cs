using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.CameraAndUi
{
	public class PlayerCameraFocusPointLocation : MonoBehaviour 
	{
		[SerializeField] float cameraTilt = 6f;

		public void SetInitialFocusHeight(float height)
		{
			transform.localPosition = new Vector3(0, height, 0);
		}

		public void TiltUpDown(float yAxisTiltMultiplier, float scroll)
		{
			transform.localRotation *= Quaternion.Euler(yAxisTiltMultiplier * scroll, 0, 0);
		}

		public void ResetAngle()
		{
			transform.localRotation = Quaternion.Euler(cameraTilt, 0, 0);
		}
	}
}