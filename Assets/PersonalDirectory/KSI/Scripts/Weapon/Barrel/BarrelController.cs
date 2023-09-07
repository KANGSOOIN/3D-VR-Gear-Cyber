using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KSI
{
	public class BarrelController : MonoBehaviour
	{
		[SerializeField] private GameObject barrelEffect; // ���� ȿ�� ��ƼŬ

		private Transform tr;
		private Rigidbody rb;
		private int shootCount = 0; // �ѿ� ���� Ƚ��

		private void Start() 
		{ 
			tr = GetComponent<Transform>();
			rb = GetComponent<Rigidbody>();
		}

        private void OnCollisionEnter(Collision coll)
		{
			if (coll.collider.CompareTag("Bullet"))
			{
				if (++shootCount == 3)
				{
					BarrelHP();
				}
			}
		}

		private void BarrelHP()
		{
			GameObject barrelHP = Instantiate(barrelEffect, tr.position, Quaternion.identity);

			Destroy(barrelHP, 5.0f);

			// Barrel�� ���Ը� ������ �ؼ� ���� ��������
			rb.mass = 1.0f;
			rb.AddForce(Vector3.up * 1500.0f);

			Destroy(gameObject, 3.0f);
		}
	}
}
