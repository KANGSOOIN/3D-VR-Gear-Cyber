using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PID;
using PGR;

namespace KSI
{
    public class Melee : MonoBehaviour
    {
		[Header("Settings")]
		[SerializeField] private float checkRate = 0.1f; // ��ġ�� üũ�ϴ� �ֱ�
		[SerializeField] private int maxPositions = 10; // ť�� ������ �ִ� ��ġ ����
		[SerializeField] private float maxDamage = 100; // �ִ� ������ ��
		[SerializeField] private float minDamage = 10; // �ּ� ������ ��
		[SerializeField] private float minDistanceForMaxDamage = 1.0f; // �ִ� �������� �ֱ� ���� �ּ� �Ÿ�
		[SerializeField] private float minDistanceForMinDamage = 0.2f; // �ּ� �������� �ֱ� ���� �ּ� �Ÿ�
		
		[Header("Runtime Data")]
		[SerializeField] private Collider coll; // ������ �ݶ��̴�

		private bool isRightHanded; 		
		[SerializeField] private bool isSwinging = false; // ���Ⱑ �ֵθ��� �ִ��� ����
		bool IsSwinging{ get{ return isSwinging; } set { isSwinging = value; Debug.Log($"{name} is {value}"); } }
		private Queue<Vector3> positionQueue = new Queue<Vector3>(); // ��ġ�� ������ ť		
		private PlayerHandMotion playerHandMotion;

		void Start()
		{
			if (playerHandMotion == null)
				playerHandMotion = GetComponent<PlayerHandMotion>();
		}

		// ��ġ�� �����ϴ� �ڷ�ƾ
		IEnumerator TrackPositionRoutine()
		{
			while (true)
			{
				if (isSwinging)
				{
					if (positionQueue.Count >= maxPositions)
					{
						// ť�� ���� ����, ���� ������ ��ġ ����
						positionQueue.Dequeue(); 
					}
					// ���� ��ġ�� ť�� �߰�
					positionQueue.Enqueue(transform.position);
				}

				// ���� üũ���� ���
				yield return new WaitForSeconds(checkRate); 
			}
		}

		public void StartSwing()
        {
			Debug.Log("StartSwing");

			if (playerHandMotion == null)
                playerHandMotion = GameManager.Data.Player.HandMotion;

            if (isRightHanded == true)
			{
				playerHandMotion.GrabOnCloseWeaponRight(true);
			}
			else
			{
				playerHandMotion.GrabOnCloseWeaponLeft(true);
			}

			StartCoroutine(TrackPositionRoutine());
			IsSwinging = true;
		}

		public void EndSwing()
		{
			Debug.Log("EndSwing");

			if (playerHandMotion == null)
				playerHandMotion = GameManager.Data.Player.HandMotion;

			if (isRightHanded == true)
			{
				playerHandMotion.GrabOnCloseWeaponRight(false);
			}
			else
			{
				playerHandMotion.GrabOnCloseWeaponLeft(false);
			}

			StopAllCoroutines();
			IsSwinging = false;
		}

		private void OnCollisionEnter(Collision other)
		{
			Debug.Log("OnColliderEnter" + other.gameObject.name);

			Debug.Log($"{isSwinging}, {positionQueue.Count}");
			
			if (isSwinging && positionQueue.Count > 0)

			{
				IStrikable iStrikable = other.gameObject.GetComponent<IStrikable>();
				Debug.Log($"{iStrikable}");
				if (iStrikable != null)
				{
					// ���� ������ ��ġ ��������
					Vector3 oldestPosition = positionQueue.Peek();
					// ���� ��ġ ��������
					Vector3 currentPosition = transform.position;
					// �� ��ġ ������ �Ÿ� ���
					float distance = Vector3.Distance(oldestPosition, currentPosition);

					// ������ ���� ���
					float calculateDamageRatio = Mathf.InverseLerp(minDistanceForMinDamage, minDistanceForMaxDamage, distance);
					// ���� ������ �� ���
					int calculateDamage = Mathf.RoundToInt(Mathf.Lerp(minDamage, maxDamage, calculateDamageRatio));

					// ���� ������ ����
					iStrikable?.TakeStrike(transform, calculateDamage, Vector3.zero, Vector3.zero);
					Debug.Log("Calculated damage: " + calculateDamage);
				}
			}
		}
	}
}