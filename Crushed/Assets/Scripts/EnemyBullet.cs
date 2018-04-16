using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour {

	public float damage = 10f;

	public Vector3 originPoint;

	public float maxDistance;

	public static float maxBullets = 1f;

	public static float usedBullets = 0f;

	void Start ()
	{
		usedBullets++;
	}

	public float GetDamage ()
	{
		return damage;
	}

	public void Hit ()
	{
		Destroy(this.gameObject);
	}

	void Update ()
	{
		if (Vector3.Distance (originPoint, transform.position) >= maxDistance) {
			Hit();

		}
	}

	void OnDestroy ()
	{
		usedBullets--;
		Mathf.Clamp(usedBullets, 0f, maxBullets);
	}
}
