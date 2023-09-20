using UnityEngine;

namespace PGR
{
    public class FixedPoint : HackingPointBase
    {
        [SerializeField] HackingPuzzle hackingPuzzle;
        [SerializeField] Transform mpTransform, lfpTransform1, lfpTransform2;
        [SerializeField] Renderer rn;
        [SerializeField] Material offMat, onMat;
        [SerializeField] float maximum1, maximum2, compare1, compare2, height;

        void LateUpdate()
        {
            if (hackingPuzzle == null)
                return;

            if (IsInArea())
            {
                rn.material = onMat;
                hackingPuzzle.TurnOn(this);
            }
            else
            {
                rn.material = offMat;
                hackingPuzzle.TurnOff(this);
            }
        }

        public void SetLight(HackingPuzzle _hackingPuzzle, Transform _mpTransform, Transform _lfpTransform1, Transform _lfpTransform2)
        {
            hackingPuzzle = _hackingPuzzle;
            mpTransform = _mpTransform;
            lfpTransform1 = _lfpTransform1;
            lfpTransform2 = _lfpTransform2;
        }

        bool IsInArea()
        {
            // mp, lfp1�� �غ����� �ϰ� ���̰� .05�� �ﰢ���� ���� * 2
            maximum1 = Vector3.Distance(mpTransform.position, lfpTransform1.position) * 0.05f;
            // mp-lfp1�� this ������ �Ÿ�
            height = GetDictance(mpTransform.position, lfpTransform1.position, transform.position);
            // this, lfp1�� �غ����� �ϰ� ���̰� 1�� �ﰢ���� ���� * 2
            compare1 = (Vector3.Distance(mpTransform.position, transform.position) + Vector3.Distance(lfpTransform1.position, transform.position)) * height;
            // ���ڰ� ���� ���϶�� ���� ���� ���� �ִ�
            if (compare1 <= maximum1)
                return true;

            // mp, lfp2�� �غ����� �ϰ� ���̰� .05�� �ﰢ���� ���� * 2
            maximum2 = Vector3.Distance(mpTransform.position, lfpTransform2.position) * 0.05f;
            // mp-lfp2�� this ������ �Ÿ�
            height = GetDictance(mpTransform.position, lfpTransform2.position, transform.position);
            // this, lfp2�� �غ����� �ϰ� ���̰� 1�� �ﰢ���� ���� * 2
            compare2 = (Vector3.Distance(mpTransform.position, transform.position) + Vector3.Distance(lfpTransform2.position, transform.position)) * height;
            // ���ڰ� ���� ���϶�� ���� ���� ���� �ִ�
            if (compare2 <= maximum2)
                return true;

            // �� �� �ƴ϶�� ���� ���� �ۿ� �ִ�
            return false;
        }

        float GetDictance(Vector3 pointA, Vector3 pointB, Vector3 point)
        {
            Vector3 AtoB = pointB - pointA;
            return (Vector3.Cross(point - pointA, AtoB).magnitude / AtoB.magnitude);
        }
    }

}