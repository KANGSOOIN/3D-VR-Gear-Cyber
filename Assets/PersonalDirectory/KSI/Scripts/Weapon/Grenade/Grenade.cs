using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KSI
{
	public class Grenade : MonoBehaviour
	{
		[SerializeField] private GameObject grenadeEffect; // ���� ȿ�� ��ƼŬ

		private Transform tr;
		private Rigidbody rb;

		private void Start()
		{
			tr = GetComponent<Transform>();
			rb = GetComponent<Rigidbody>();
		}
	}
}