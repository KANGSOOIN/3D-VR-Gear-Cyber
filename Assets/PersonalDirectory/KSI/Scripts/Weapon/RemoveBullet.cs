using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveBullet : MonoBehaviour
{
	private void OnCollisionEnter(Collision collision)
	{
		if (collision.collider.tag == "Bullet")
		{
			Destroy(collision.gameObject);
			Debug.Log("Bullet hit detected. Destroying bullet.");
		}
		// IEnumerator�� ����Ͽ� �ణ�� �����̸� �� �Ŀ� Ȯ��
		StartCoroutine(CheckIfDestroyed(collision.gameObject));
	}


	IEnumerator CheckIfDestroyed(GameObject obj)
	{
		// 1���� �����̸� �ݴϴ�.
		yield return new WaitForSeconds(1f);

		if (obj == null)
		{
			Debug.Log("Bullet has been destroyed.");
		}
		else
		{
			Debug.Log("Bullet was NOT destroyed.");
		}
	}
}
