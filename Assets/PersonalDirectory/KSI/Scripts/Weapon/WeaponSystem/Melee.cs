using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
		private bool isSwinging = false; // ���Ⱑ �ֵθ��� �ִ��� ����
		private Queue<Vector3> positionQueue = new Queue<Vector3>(); // ��ġ�� ������ ť

		void Start()
		{
			// �ʱ⿡�� �ݶ��̴��� ��Ȱ��ȭ
			//coll.enabled = false;

			StartCoroutine(TrackPositionRoutine());
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
				else
				{
					// �ֵθ��� ���� ���� ť�� ���
					positionQueue.Clear();
				}

				// ���� üũ���� ���
				yield return new WaitForSeconds(checkRate); 
			}
		}

		public void StartSwing()
		{
			isSwinging = true;
			coll.enabled = true;
		}

		public void EndSwing()
		{
			isSwinging = false;
			coll.enabled = false;
		}

		private void OnTriggerEnter(Collider other)
		{
			if (isSwinging && positionQueue.Count > 0)
			{
				Hitable hitable = other.GetComponent<Hitable>(); 
				if (hitable != null)
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

					// ��ŵ� ������ ����
					StartCoroutine(hitable.Hit(calculateDamage));
				}
			}
		}
	}
}