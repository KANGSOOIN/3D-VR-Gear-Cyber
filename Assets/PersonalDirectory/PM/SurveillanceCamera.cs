using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static UnityEditor.PlayerSettings;

namespace PM
{
    public class SurveillanceCamera : MonoBehaviour, Hitable, Hackingable
    {
        [SerializeField]
        GameObject[] securities;
        private float range;
        [SerializeField] int hp;
        [SerializeField] Transform SpotLight;
        Ray ray;
        private Vector3 lightPosition;
        private float angle;
        private float cos;
        private float sin;

        private void Start()
        {
            lightPosition = SpotLight.transform.position;
            ray = new Ray(lightPosition, SpotLight.forward);
            StartCoroutine(RangeSetting());
            StartCoroutine(Checking());
        }

        IEnumerator RangeSetting()
        {
            angle = SpotLight.GetComponent<Light>().spotAngle;
            cos = Mathf.Cos(angle * 0.5f * Mathf.Deg2Rad);
            sin = Mathf.Sin(angle * 0.5f * Mathf.Deg2Rad);
            Ray ray = new Ray(lightPosition, (SpotLight.forward * cos + SpotLight.up * sin));
            RaycastHit hitData;
            Physics.Raycast(ray, out hitData);
            range = hitData.distance;
            yield return null;
        }
        //private void Update()
        //{
        //    Debug.DrawRay(lightPosition, SpotLight.forward*10, Color.red);
        //    Debug.DrawRay(lightPosition, (SpotLight.forward * cos + SpotLight.right*sin) * 10, Color.red);
        //    Debug.DrawRay(lightPosition, (SpotLight.forward * cos - SpotLight.right * sin) * 10, Color.red);
        //    Debug.DrawRay(lightPosition, (SpotLight.forward * cos + SpotLight.up * sin) * 10, Color.red);
        //    Debug.DrawRay(lightPosition, (SpotLight.forward * cos - SpotLight.up * sin) * 10, Color.red);
        //}
        void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(lightPosition, range);
        }
        IEnumerator Checking()
        {
            while (true)
            {
                Collider[] colliders = Physics.OverlapSphere(lightPosition, range);
                foreach (Collider collider in colliders)
                {
                    // ���� �ݶ��̴��� �÷��̾�� ����
                    if (collider.tag == "Player")
                    {
                        Vector3 dirTarget = (collider.transform.position - lightPosition).normalized;
                        if (Vector3.Dot(transform.forward, dirTarget) < Mathf.Cos(angle * 0.5f * Mathf.Deg2Rad))
                            continue;
                        Debug.Log("player");
                        StartCoroutine(CallSecurity(collider.transform.position));

                    }
                    yield return null;
                }
                yield return new WaitForSeconds(0.3f);
            }
        }

        IEnumerator CallSecurity(Vector3 destination)
        {
            foreach (GameObject guard in securities)
            {
                // �κ��� ��ġ�� �����ϴ� �Լ��� �����ͼ� �÷��̾��� ��ġ�� ������ ���� �Ѱ��� guard.GetComponent<>
                yield return null;
            }
            yield return null;
        }

        public IEnumerator Hit(int damage)
        {
            hp -= damage;
            if (hp <= 0)
                StartCoroutine(Break());
            yield return null;
        }

        public IEnumerator Break()
        {
            SpotLight.gameObject.SetActive(false);
            Destroy(this);
            yield return null;
        }

        // �÷��̾ ��ŷ�� �����ϸ� �Լ��� ȣ�� �����ϸ� true Ʋ���� false�� ȣ��
        // �÷��̾ ��ŷ�� �����ϸ� ���κ����� ȣ��
        public IEnumerator HackingCheck(bool success)
        {
            if (success)
                StartCoroutine(Break());
            else
            {
                RaycastHit hitData;
                Physics.Raycast(ray, out hitData);
                StartCoroutine(CallSecurity(hitData.point));
            }
            yield return null;
        }
    }
}

