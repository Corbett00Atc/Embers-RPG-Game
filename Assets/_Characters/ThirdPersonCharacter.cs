using UnityEngine;

namespace RPG.Characters
{
	[RequireComponent(typeof(Rigidbody))]
	[RequireComponent(typeof(CapsuleCollider))]
	[RequireComponent(typeof(Animator))]
	public class ThirdPersonCharacter : MonoBehaviour
	{
		const float k_Half = 0.5f;

		[Range(1f, 4f)][SerializeField] float m_GravityMultiplier = 2f;
		[SerializeField] float m_MovingTurnSpeed = 360;
		[SerializeField] float m_StationaryTurnSpeed = 180;
		[SerializeField] float m_MoveSpeedMultiplier = 1f;
		[SerializeField] float m_AnimSpeedMultiplier = 1f;
		[SerializeField] float m_GroundCheckDistance = 0.1f;
		[SerializeField] float sprintMultiplier = 1.2f;

		Vector3 m_GroundNormal;
		Rigidbody m_Rigidbody;
		Animator m_Animator;

		float m_OrigGroundCheckDistance;
		float m_TurnAmount;
		float m_ForwardAmount;
		float m_MoveSpeedOriginal;
		float m_AnimSpeedOriginal;


		void Start()
		{
			m_Animator = GetComponent<Animator>();
			m_Rigidbody = GetComponent<Rigidbody>();

			m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
			m_OrigGroundCheckDistance = m_GroundCheckDistance;

			m_MoveSpeedOriginal = m_MoveSpeedMultiplier;
			m_AnimSpeedOriginal = m_AnimSpeedMultiplier;
		}


		public void Move(Vector3 move, bool sprint)
		{

			// convert the world relative moveInput vector into a local-relative
			// turn amount and forward amount required to head in the desired
			// direction.
			if (move.magnitude > 1f) move.Normalize();
			move = transform.InverseTransformDirection(move);
			CheckGroundStatus();
			move = Vector3.ProjectOnPlane(move, m_GroundNormal);
			m_TurnAmount = Mathf.Atan2(move.x, move.z);
			m_ForwardAmount = move.z;

			HandleSprint(sprint);

			ApplyExtraTurnRotation();

			// send input and other state parameters to the animator
			UpdateAnimator(move);
		}

		void HandleSprint(bool sprint)
		{
			if (sprint)
			{
				m_AnimSpeedMultiplier = m_AnimSpeedOriginal * sprintMultiplier;
				m_MoveSpeedMultiplier = m_MoveSpeedOriginal * sprintMultiplier;
			}
			else
			{
				m_AnimSpeedMultiplier = m_AnimSpeedOriginal;
				m_MoveSpeedMultiplier = m_MoveSpeedOriginal;
			}
		}

		void UpdateAnimator(Vector3 move)
		{
			// update the animator parameters
			m_Animator.SetFloat("Forward", m_ForwardAmount, 0.1f, Time.deltaTime);
			m_Animator.SetFloat("Turn", m_TurnAmount, 0.1f, Time.deltaTime);
	

			// the anim speed multiplier allows the overall speed of walking/running to be tweaked in the inspector,
			// which affects the movement speed because of the root motion.
			if (move.magnitude > 0)
			{
				m_Animator.speed = m_AnimSpeedMultiplier;
			}
		}


		void HandleAirborneMovement()
		{
			// apply extra gravity from multiplier:
			Vector3 extraGravityForce = (Physics.gravity * m_GravityMultiplier) - Physics.gravity;
			m_Rigidbody.AddForce(extraGravityForce);

			m_GroundCheckDistance = m_Rigidbody.velocity.y < 0 ? m_OrigGroundCheckDistance : 0.01f;
		}

		void ApplyExtraTurnRotation()
		{
			// help the character turn faster (this is in addition to root rotation in the animation)
			float turnSpeed = Mathf.Lerp(m_StationaryTurnSpeed, m_MovingTurnSpeed, m_ForwardAmount);
			transform.Rotate(0, m_TurnAmount * turnSpeed * Time.deltaTime, 0);
		}


		public void OnAnimatorMove()
		{
			// we implement this function to override the default root motion.
			// this allows us to modify the positional speed before it's applied.
			if (Time.deltaTime > 0)
			{
				Vector3 v = (m_Animator.deltaPosition * m_MoveSpeedMultiplier) / Time.deltaTime;

				// we preserve the existing y part of the current velocity.
				v.y = m_Rigidbody.velocity.y;
				m_Rigidbody.velocity = v;
			}
		}


		void CheckGroundStatus()
		{
			RaycastHit hitInfo;
#if UNITY_EDITOR
			// helper to visualise the ground check ray in the scene view
			Debug.DrawLine(transform.position + (Vector3.up * 0.1f), transform.position + (Vector3.up * 0.1f) + (Vector3.down * m_GroundCheckDistance));
#endif
			// 0.1f is a small offset to start the ray from inside the character
			// it is also good to note that the transform position in the sample assets is at the base of the character
			if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, m_GroundCheckDistance))
			{
				m_GroundNormal = hitInfo.normal;
				m_Animator.applyRootMotion = true;
			}
			else
			{
				m_GroundNormal = Vector3.up;
				m_Animator.applyRootMotion = false;
			}
		}
	}
}
