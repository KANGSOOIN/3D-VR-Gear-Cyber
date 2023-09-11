using PID;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static UnityEditor.PlayerSettings;

namespace PM
{
    public class SurveillanceCamera : MonoBehaviour, IHitable
    {
        
        [SerializeField]
        private float range;
        [SerializeField] int hp;
        [SerializeField] Transform SpotLight;
        Terminal terminal;
        Ray ray;
        private Vector3 lightPosition;
        private float angle;
        private float cos;
        private float sin;

        private void Start()
        {
            GetTerminal();
            lightPosition = SpotLight.transform.position;
            ray = new Ray(lightPosition, SpotLight.forward);
            StartCoroutine(RangeSetting());
            StartCoroutine(Checking());
        }

        private void GetTerminal()
        {
            terminal = transform.parent.GetComponentInChildren<Terminal>();
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
        //void OnDrawGizmos()
        //{
        //    Gizmos.color = Color.yellow;
        //    Gizmos.DrawWireSphere(lightPosition, range);
        //    Gizmos.color = Color.blue;
        //    //Gizmos.DrawRay(lightPosition, (player.transform.position - lightPosition));
        //}
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
                        // �ٽ� ���̸��� ī�޶�� �÷��̾� ���̿� ��ֹ��� ������ ����
                        Ray ray = new Ray(lightPosition, (collider.transform.position - lightPosition));
                        RaycastHit hitData;
                        Physics.Raycast(ray, out hitData);
                        if (hitData.collider.tag == "Player")
                        {
                            StartCoroutine(CallSecurity());
                        }
                    }
                    yield return null;
                }
                yield return new WaitForSeconds(0.3f);
            }
        }

        public IEnumerator CallSecurity()
        {
            RaycastHit hitData;
            Physics.Raycast(ray, out hitData);
            if (terminal != null)
                terminal.StartCoroutine(terminal.CallSecurity(hitData.point));
            yield return null;
        }

        public IEnumerator Break()
        {
            SpotLight.gameObject.SetActive(false);
            Destroy(this);
            yield return null;
        }

        public void TakeDamage(int damage, Vector3 hitPoint, Vector3 hitNormal)
        {
            hp -= damage;
            if (hp <= 0)
                StartCoroutine(Break());
        }
    }
}

