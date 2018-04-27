using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using RPG.CameraAndUi;

namespace RPG.Characters
{
    [RequireComponent(typeof (ThirdPersonCharacter))]
    public class PlayerMovement : MonoBehaviour
    {    
        ThirdPersonCharacter thirdPersonCharacter = null;   // A reference to the ThirdPersonCharacter on the object
        private Vector3 m_Move;
            
        private void Start()
        {
            thirdPersonCharacter = GetComponent<ThirdPersonCharacter>();

        }

        void Update()
        {
            ProcessMovement();
        }

        void ProcessClick(RaycastHit raycastHit, int layerHit)
        {
            // calls when clicked as an observer of CameraRaycaster
        }

        void ProcessMovement()
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            bool sprint = Input.GetButton("Sprint");

            // calculate camera relative direction to move:
            Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
            m_Move = v*cameraForward + h*Camera.main.transform.right;

            thirdPersonCharacter.Move(m_Move, sprint);
        }
    }
}