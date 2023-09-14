using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KSI
{
	public class BulletController : MonoBehaviour
	{
		[SerializeField] private float damage;
		[SerializeField] private float force;
		[SerializeField] private GameObject bulletSparkEffect;

		private Rigidbody rb;

		private void Start()
		{
			rb = GetComponent<Rigidbody>();
			rb.AddForce(transform.forward * force);
			StartCoroutine(DestroyBulltRoutine(5f)); // 5�� �Ŀ� �Ѿ� ����
		}

		private IEnumerator DestroyBulltRoutine(float seconds)
		{
			yield return new WaitForSeconds(seconds);
			Destroy(gameObject);
		}

		private void OnCollisionEnter(Collision coll)
		{
			if (coll.collider.CompareTag("Player") || coll.collider.CompareTag("Gun") || coll.collider.CompareTag("Bullet"))
			{
				return;
			}

			// ù ��° �浹 ���� ���� ����
			ContactPoint contactPoint = coll.GetContact(0);
			// �浹�� �Ѿ��� ���� ���͸� ���ʹϾ� Ÿ������ ��ȯ
			Quaternion rotation = Quaternion.LookRotation(-contactPoint.normal);

			GameObject spark = Instantiate(bulletSparkEffect, contactPoint.point, rotation);
			Destroy(spark, 1f);
			Destroy(gameObject);
		}
	}
}