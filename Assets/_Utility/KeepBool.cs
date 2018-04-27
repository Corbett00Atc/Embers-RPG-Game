using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
	Allows animator to keep a bool in a certain state
 */

public class KeepBool : StateMachineBehaviour 
{
	[SerializeField] string boolName;
	[SerializeField] bool status;
	[SerializeField] bool resetOnExit = true;

	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
	{
		animator.SetBool(boolName, status);
	}

	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
	{
		if (resetOnExit)
			animator.SetBool(boolName, !status);
	}

}
