using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KSI
{
	public class BulletRemove : MonoBehaviour
	{
		[SerializeField] private GameObject bulletSparkEffect;

		private void OnCollisionEnter(Collision coll)
		{
			if (coll.collider.CompareTag("Bullet"))
			{
				// ù ��° �浹 ���� ���� ����
				ContactPoint contactPoint = coll.GetContact(0);
				// �浹�� �Ѿ��� ���� ���͸� ���ʹϾ� Ÿ������ ��ȯ
				Quaternion rotation = Quaternion.LookRotation(-contactPoint.normal);

				GameObject spark = Instantiate(bulletSparkEffect, contactPoint.point, rotation);
				Destroy(spark, 1f);

				Destroy(coll.gameObject);
			}
		}
	}
}
