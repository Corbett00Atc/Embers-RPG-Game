using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
	Camera movement:
		- Dark Souls III style
*/

namespace RPG.CameraAndUi
{
	public class CameraFollow : MonoBehaviour 
	{
		[SerializeField] float cameraFocusHeight = .70f; 
		[SerializeField] int lookIntensity = 10;
		[SerializeField] float zAxisZoomMultiplier = 2.5f;
		[SerializeField] float yAxisTiltMultiplier = 2.5f;

		
		private Vector3 freeViewLook = new Vector3(0, 0, 0);
		private Vector3 cameraStartPosition;
		private Vector3 cameraAdjustedPosition;
		private Quaternion cameraStartRotation;
		private PlayerCameraFocusPointLocation cameraFocusPoint;
		private GameObject player;	
		private Camera gameCamera;

		float vertical = 0;
		float horizontal = 0;

		void Start()
		{
			player = GameObject.FindGameObjectWithTag("Player");

			// access starting parameters for camera, and prepare adjuster variables
			gameCamera = GetComponentInChildren<Camera>();
			cameraStartPosition = gameCamera.transform.localPosition;
			gameCamera.transform.localPosition = cameraStartPosition;
			cameraAdjustedPosition = cameraStartPosition;
			cameraStartRotation = gameCamera.transform.localRotation;

			// sets focus object location 
			cameraFocusPoint = GetComponentInChildren<PlayerCameraFocusPointLocation>();
			cameraFocusPoint.SetInitialFocusHeight(cameraFocusHeight);
		}
		
		void LateUpdate()
		{
			// attach camera arm to player
			transform.localPosition = player.transform.position;

			FreeLook();
		}

		void FreeLook()
		{
			MouseLock();

			// gets values based on mouse axis
			vertical += lookIntensity * Input.GetAxis("Mouse Y");
			horizontal += lookIntensity * Input.GetAxis("Mouse X");

			ClampValues();

			freeViewLook = new Vector3(-vertical, horizontal, 0f);

			// Set orientation:
			transform.eulerAngles = freeViewLook;
		}

		// locks the mouse location while in free look
		void MouseLock()
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}

		// unlock while not in free look, allows to move mouse off game screen
		void MouseUnlock()
		{
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}

		void ClampValues()
		{
			// Clamp vertical:
			vertical = Mathf.Clamp(vertical, -45, 20);
		
			// Wrap horizontal:
			while (horizontal < 0f) 
			{
				horizontal += 360f;
			}
			while (horizontal >= 360f) 
			{
				horizontal -= 360f;
			}
		}

		void ResetFreeLook()
		{
			horizontal = player.transform.eulerAngles.y;
			vertical = player.transform.eulerAngles.x;
		}

		void Zooming(float scroll)
		{
			cameraAdjustedPosition = new Vector3(
				gameCamera.transform.localPosition.x,
				gameCamera.transform.localPosition.y - MouseWheelAxis(),
				gameCamera.transform.localPosition.z + zAxisZoomMultiplier * MouseWheelAxis()
				);
			gameCamera.transform.localPosition = cameraAdjustedPosition;
		}

		void TiltCamera(float scroll)
		{
			cameraFocusPoint.TiltUpDown(yAxisTiltMultiplier, scroll);
		}

		float MouseWheelAxis()
		{
			return Input.GetAxis("Mouse ScrollWheel");
		}
	}
}