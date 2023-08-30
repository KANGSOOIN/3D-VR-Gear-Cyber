using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurveillanceCamera: MonoBehaviour
{
    [SerializeField]
    GameObject[] securities;
    [SerializeField] float range;
    [SerializeField, Range(0, 360)] float angle;
    IEnumerator Checking()
    {
        while(true)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, range);
            foreach(Collider collider in colliders)
            {
                // ���� �ݶ��̴��� �÷��̾�� ����
                //if(collider.GetComponent<>() != null)
                Vector3 dirTarget = (collider.transform.position - transform.position).normalized;
                if (Vector3.Dot(transform.position, dirTarget) < Mathf.Cos(angle * 0.5f * Mathf.Deg2Rad))
                    continue;
                foreach(GameObject guard in securities)
                    // �κ��� ��ġ�� �����ϴ� �Լ��� �����ͼ� �÷��̾��� ��ġ�� ������ ���� �Ѱ��� guard.GetComponent<>
                yield return null;
            }
            yield return null;
        }
    }
}
