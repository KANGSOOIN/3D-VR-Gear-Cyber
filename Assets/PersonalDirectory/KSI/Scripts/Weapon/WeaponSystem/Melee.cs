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
			StartCoroutine(TrackPositionRoutine());

			isSwinging = true;
		}

		public void EndSwing()
		{
			StopAllCoroutines();

			isSwinging = false;
		}

		private void OnTriggerEnter(Collider other)
		{
			if (isSwinging && positionQueue.Count > 0)
			{
				IHitable hitable = other.GetComponent<IHitable>(); 
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
					hitable.TakeDamage(calculateDamage, Vector3.zero, Vector3.zero);
				}
			}
		}
	}
}