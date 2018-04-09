using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPG.Characters
{
	public class PlayerTargeting : MonoBehaviour 
	{
		List<Enemy> targetsInRange = new List<Enemy>();
		Enemy currentTarget = null;
		int targetIndex = 0;

		public Enemy GetCurrentTarget()
		{
			return currentTarget;
		}

		void Update()
		{
			if (Input.GetKeyDown("q"))
				TargetNewEnemy();
		}

		void OnTriggerEnter(Collider other)
		{	
			if (other.gameObject.GetComponent<Enemy>() != null)
				targetsInRange.Add(other.gameObject.GetComponent<Enemy>());
			
			return;
		}

		void OnTriggerExit(Collider other)
		{
			if (other.gameObject.GetComponent<Enemy>() != null)
			{
				Enemy e = other.gameObject.GetComponent<Enemy>();
				targetsInRange.Remove(e);	

				if (e == currentTarget)
				{
					UnMarkTarget();
					currentTarget = null;
				}
			}
			
			return;
		}

		public void TargetDied(Enemy e)
		{
			targetsInRange.Remove(e);	

			if (e == currentTarget)
				currentTarget = null;
		}

		void SortByRange()
		{
			targetsInRange = targetsInRange.OrderBy(
					x => Vector3.Distance(this.transform.position,x.transform.position)
				).ToList();
		}

		void TargetNewEnemy()
		{
			SortByRange();
			
			if (currentTarget)
				if (targetsInRange.Count - 1 > targetIndex)
				{
					targetIndex += 1;
					UnMarkTarget();
					currentTarget = targetsInRange[targetIndex];
					MarkTarget();
				}
				else
				{
					targetIndex = 0;
					UnMarkTarget();
					currentTarget = targetsInRange[targetIndex];
					MarkTarget();
				}
			else if (targetsInRange.Count >= 1)
			{
				targetIndex = 0;
				UnMarkTarget();
				currentTarget = targetsInRange[targetIndex];
				MarkTarget();
			}
			else
			{
				targetIndex = 0;
				UnMarkTarget();
				currentTarget = null;
			}
		}

		void UnMarkTarget()
		{
			if (currentTarget)
				currentTarget.UnMarkTarget();
		}

		void MarkTarget()
		{
			currentTarget.MarkTarget();
		}
	}
}