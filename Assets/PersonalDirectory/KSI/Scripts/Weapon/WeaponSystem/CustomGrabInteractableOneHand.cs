using UnityEngine;
using UnityEngine.XR.Content.Interaction;
using UnityEngine.XR.Interaction.Toolkit;
using PGR;

namespace KSI
{
	[RequireComponent(typeof(RayAttachModifier))]
	public class CustomGrabInteractableOneHand : CustomGrabInteractable
	{
		[Header("")]
		[SerializeField] bool onlyOneHandGrab;

		// �� �� �տ��� ��� �ִ� �߿��� �ٸ� �տ����� ���� ���ϰ� ��
		public override bool IsSelectableBy(IXRSelectInteractor interactor)
		{
			if (!onlyOneHandGrab)
				return base.IsSelectableBy(interactor);

			if (isSelected && !ReferenceEquals(interactor.transform, firstInteractorSelecting.transform))
				return false;
			else
				return base.IsSelectableBy(interactor);
		}
	}
}
