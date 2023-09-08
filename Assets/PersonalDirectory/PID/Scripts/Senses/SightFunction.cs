using PID;
using UnityEngine;
using UnityEngine.Events;

namespace PID
{
    public class SightFunction : MonoBehaviour
    {
        [SerializeField] LayerMask targetMask;
        [SerializeField] LayerMask obstacleMask; 
        [SerializeField] Transform targetEye; 
        Transform playerInSight;
        //Should Be Declared by using Robot
        public UnityAction<Transform> PlayerFound;
        //Should be leading to Assualt State 
        public UnityAction PlayerLost;
        //Shoudl be leading to Tracing State 

        float presenceTimer;
        const float maxTimer = 3f;
        public bool TargetFound
        {
            get;
            private set;
        }
        bool playerPresence = false; 
        float enemyDetectRange;
        float sightAngle;

        [SerializeField] bool debug;
        private void Start()
        {
            targetEye.transform.forward = transform.forward; 
        }

        private void Update()
        {
            FindTarget();
            if (!playerPresence)
            {
                presenceTimer -= Time.deltaTime;
                presenceTimer = Mathf.Clamp(presenceTimer, 0, maxTimer);
            }
            else if (playerPresence)
            {
                presenceTimer += Time.deltaTime;
                if (presenceTimer > maxTimer)
                {
                    TargetFound = true;
                    //Should be replaced by the state changing event; 
                    PlayerFound?.Invoke(playerInSight);
                }
                presenceTimer = Mathf.Clamp(presenceTimer, 0, maxTimer);
            }
        }

        public void SyncSightStat(EnemyStat robotStat)
        {
            this.sightAngle = robotStat.maxSightAngle;
            this.enemyDetectRange = robotStat.maxSightRange; 
        }
        Vector3 dirTarget;
        RaycastHit obstacleHit;
        public void FindTarget()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, enemyDetectRange, targetMask);
            if (colliders.Length == 0)
            {
                playerInSight = null;
                TargetFound = false;
                playerPresence = false;
                PlayerLost?.Invoke();
                return;
            }
            foreach (Collider collider in colliders)
            {
                dirTarget = collider.transform.position - transform.position;
                dirTarget.y = 0f;
                dirTarget.Normalize();
                // IF Player is found on a given Range, 
                if (Vector3.Dot(transform.forward, dirTarget) < Mathf.Cos(sightAngle * 0.5f * Mathf.Deg2Rad))
                {
                    playerInSight = collider.transform;
                    playerPresence = true;
                    continue;
                }
                //Start Coroutine for hiding activities. 
;
                //Vector3 distToTarget = collider.transform.position - transform.position;
                float distance = Vector3.Distance(collider.transform.position, transform.position);//Vector2.SqrMagnitude(distToTarget);
                if (Physics.Raycast(transform.position, dirTarget, out obstacleHit, distance, obstacleMask))
                {
                    break; 
                }
                else
                {
                    playerInSight = collider.transform;
                    PlayerFound?.Invoke(collider.transform);
                    return;
                }
            }
            PlayerLost?.Invoke();
        }
        public bool TargetInValidRange() 
        {
            return false; 
        }

        private void OnDrawGizmos()
        {
            if (!debug) return;
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(targetEye.position, sightAngle);

            Vector3 rightDir = AngleToDir(transform.eulerAngles.y + sightAngle * 0.5f);
            // where .eulerAngle.y returns rotation angle from the y-axis in a Space.World 
            Vector3 leftDir = AngleToDir(transform.eulerAngles.y - sightAngle * 0.5f);
            // 
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(transform.position, rightDir * enemyDetectRange);
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(transform.position, leftDir * enemyDetectRange);
        }

        private Vector3 AngleToDir(float angle)
        {
            float radian = angle * Mathf.Deg2Rad;
            return new Vector3(Mathf.Sin(radian), 0, Mathf.Cos(radian));
        }

        public void UnderPlayerPresence(Transform player)
        {
            presenceTimer += Time.deltaTime;
            Debug.Log(presenceTimer);
            if (presenceTimer > maxTimer)
            {
                TargetFound = true;
                //Should be replaced by the state changing event; 
                PlayerFound?.Invoke(player);
            }
        }
    }
    #region DEPRECATED
    //private void OnTriggerEnter(Collider other)
    //{
    //    //Massive error with this. 
    //    if (!targetMask.Contain(other.gameObject.layer))
    //    {
    //        playerContact = false;
    //        return;
    //    }
    //    else
    //    {
    //        playerContact = true;
    //        presenceTimer += Time.deltaTime;
    //        Debug.Log(presenceTimer);
    //        if (presenceTimer > maxTimer)
    //        {
    //            TargetFound = true;
    //            //Should be replaced by the state changing event; 
    //            PlayerFound?.Invoke(other.transform);
    //            PlayerInSight = other.transform;
    //            presenceTimer = Mathf.Clamp(presenceTimer, 0, maxTimer);
    //            return;
    //        }
    //        return;
    //    }
    //    presenceTimer -= Time.deltaTime;
    //    presenceTimer = Mathf.Clamp(presenceTimer, 0, maxTimer);
    //}

    #endregion
}