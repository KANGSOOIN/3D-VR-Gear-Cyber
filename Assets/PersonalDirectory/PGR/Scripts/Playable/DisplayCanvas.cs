using UnityEngine;

namespace PGR
{
    /// <summary>
    /// ȫä ���÷��̸� Ȱ��ȭ���� �� ���̴� World Space UI
    /// ��ġ�� ������ �����ϰ�, �ؽ�Ʈ�� �Է��ϸ� ��
    /// </summary>
    public class DisplayCanvas : SceneUI
    {
        [SerializeField] Camera overayCamera;
        [SerializeField] float maxDistance;
        [SerializeField] bool isVisible;

        void LateUpdate()
        {
            if (overayCamera == null)
                overayCamera = GameManager.Data.Player.IrisSystem;
            if (!overayCamera.isActiveAndEnabled)
                return;
            if(Vector3.SqrMagnitude(overayCamera.transform.position - transform.position) > maxDistance)
            {
                if (isVisible)
                {
                    isVisible = false;
                    images["BG"].gameObject.SetActive(isVisible);
                }
                return;
            }
            else
            {
                if (!isVisible)
                {
                    isVisible = true;
                    images["BG"].gameObject.SetActive(isVisible);
                }

                transform.LookAt(overayCamera.transform);
                transform.Rotate(Vector3.up * 180f);
            }
        }

        public void ChangeMainText(string context)
        {
            texts["MainText"].text = context;
        }

        public void ChangeSubText(string context)
        {
            texts["SubText"].text = context;
        }
    }

}