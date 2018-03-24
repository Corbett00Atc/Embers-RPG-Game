using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
	public class Projectile : MonoBehaviour 
	{
		[SerializeField] float projectileSpeed; 
		[SerializeField] GameObject shooter; // can inspect when paused

		const float DESTROY_DELAY = 0.01f;
		GameObject target;
		float damageCaused = 10f;

		public void DamageCaused(float damage) { damageCaused = damage;	}
		public void SetShooter(GameObject shooter) { this.shooter = shooter; }
		public float ProjectileSpeed() { return projectileSpeed; }

		void OnCollisionEnter(Collision collision)
		{
			if (collision == null)
				return;

			var layerCollidedWith = collision.gameObject.layer;
			if (shooter != null && layerCollidedWith != shooter.layer)
				DamageOnIdamageable(collision);

			Destroy(this.gameObject, DESTROY_DELAY);
		}

		void DamageOnIdamageable(Collision collision)
		{
			Component damageableComponent = collision.gameObject.GetComponent(typeof(IDamageable));

			if (damageableComponent)
				(damageableComponent as IDamageable).TakeDamage(damageCaused);
		}
	}
}