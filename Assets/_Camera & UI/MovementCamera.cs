﻿using UnityEngine;
using System.Collections;

namespace RPG.CameraAndUi
{
	public class MovementCamera : MonoBehaviour {

		public GameObject target;
		public float speedMove = 1.0f;

		Vector3 point;

		void Start () {
			point = target.transform.position;
		}

		void Update () {
			transform.RotateAround (point,new Vector3(0.0f,1.0f,0.0f),20 * Time.deltaTime * speedMove);
		}
	}
}