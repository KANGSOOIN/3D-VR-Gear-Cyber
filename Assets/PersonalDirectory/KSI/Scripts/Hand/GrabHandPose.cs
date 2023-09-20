using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace KSI
{
	public class GrabHandPose : MonoBehaviour
	{
		public HandData rightHandPose;

		private Vector3 startingHandPosition;
		private Vector3 finalHandPosition;
		private Quaternion startingHandRotation;
		private Quaternion finalHandRotation;

		private Quaternion[] startingFingerRotations;
		private Quaternion[] finalFingerRotations;


		private void Start()
		{
			XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();

			grabInteractable.selectEntered.AddListener(SetUpPose); // ��ü�� ������ SetUpPose ȣ��
			grabInteractable.selectExited.AddListener(UnSetPose); // ��ü�� ������ UnSetPose ȣ��

			rightHandPose.gameObject.SetActive(false); // ���۽� ������ ����� ��Ȱ��ȭ
		}

		public void SetUpPose(BaseInteractionEventArgs arg)
		{
			if (arg.interactorObject is XRDirectInteractor)
			{
				HandData handData = arg.interactorObject.transform.GetComponentInChildren<HandData>();
				handData.animator.enabled = false;

				SetHandDataValues(handData, rightHandPose);
				SetHandData(handData, finalHandPosition, finalHandRotation, finalFingerRotations);
			}
		}

		public void UnSetPose(BaseInteractionEventArgs arg)
		{
			if (arg.interactorObject is XRDirectInteractor)
			{
				HandData handData = arg.interactorObject.transform.GetComponentInChildren<HandData>();
				handData.animator.enabled = true;

				SetHandData(handData, startingHandPosition, startingHandRotation, startingFingerRotations);
			}
		}

		public void SetHandDataValues(HandData h1, HandData h2)
		{
			startingHandPosition = new Vector3(h1.root.localPosition.x / h1.root.localPosition.x,
				h1.root.localPosition.y / h1.root.localScale.y, h1.root.localPosition.z / h1.root.localScale.z);
			finalHandPosition = new Vector3(h2.root.localPosition.x / h2.root.localPosition.x,
				h2.root.localPosition.y / h2.root.localScale.y, h2.root.localPosition.z / h2.root.localScale.z);

			startingHandRotation = h1.root.localRotation;
			finalHandRotation = h2.root.localRotation;

			startingFingerRotations = new Quaternion[h1.fingerBones.Length];
			finalFingerRotations = new Quaternion[h1.fingerBones.Length];

			// �հ����� ȸ�� ������ ����
			for (int i = 0; i < h1.fingerBones.Length; i++)
			{
				startingFingerRotations[i] = h1.fingerBones[i].localRotation;
				finalFingerRotations[i] = h2.fingerBones[i].localRotation;
			}
		}

		public void SetHandData(HandData h, Vector3 newPosition, Quaternion newRotaion, Quaternion[] newBoneRotation)
		{
			h.root.localPosition = newPosition;
			h.root.localRotation = newRotaion;

			for (int i = 0; i < newBoneRotation.Length; i++)
			{
				h.fingerBones[i].localRotation = newBoneRotation[i];
			}
		}
	}
}
