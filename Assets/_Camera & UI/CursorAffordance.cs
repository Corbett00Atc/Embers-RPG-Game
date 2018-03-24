using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.CameraAndUi
{
	[RequireComponent (typeof(CameraRaycaster))]
	public class CursorAffordance : MonoBehaviour 
	{
		[SerializeField] Texture2D walkCursor = null;
		[SerializeField] Texture2D attackCursor = null;
		[SerializeField] Texture2D errorCursor = null;
		[SerializeField] Vector2 hotspotOffset = new Vector2(0, 0);
		[SerializeField] const int walkableLayerNumber = 8;
		[SerializeField] const int enemyLayerNumber = 9;
		[SerializeField] const int uiLayerNumber = 5;

		CameraRaycaster cameraRaycaster;

		// Use this for initialization
		void Start() 
		{
			cameraRaycaster = GetComponent<CameraRaycaster>();
			cameraRaycaster.notifyLayerChangeObservers += OnLayerChange;
		}
		
		// Update is called once per frame
		void OnLayerChange(int newLayer) 
		{
			switch (newLayer)
			{
				case walkableLayerNumber: // walkable
					Cursor.SetCursor(walkCursor, hotspotOffset, CursorMode.Auto);
					break;
				case uiLayerNumber: // ui
					Cursor.SetCursor(walkCursor, hotspotOffset, CursorMode.Auto);
					break;
				case enemyLayerNumber: // enemy
					Cursor.SetCursor(attackCursor, hotspotOffset, CursorMode.Auto);
					break;
				default:
					Cursor.SetCursor(errorCursor, hotspotOffset, CursorMode.Auto);
					break;
			}
		}
	}
}