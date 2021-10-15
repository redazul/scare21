/*Benton Justice
 * 10/15/2021
 * FOV Script
 */
using UnityEngine;
using System.Collections;
using System.Linq;

namespace Scare.AI.Components
{
    /// <summary>
    /// The FOV Component actively searches for 'items of interest' and returns the closest one 
    /// through its FoundItemOfInterest event.
    /// </summary>
    /// TODO to add to this class: Begin searching on start bool, Return all in field of view, and a check for if previous collider is still in fov.
    public class FOVComponent : MonoBehaviour
    {

        [Header("Field of View Properties")]
        [SerializeField]
        private float viewRadius;
        [SerializeField]
        private float viewAngle;
        [SerializeField]
        private float timeBetweenChecks = 5f;
        [SerializeField]
        private bool shouldBeSearching = true;
        [SerializeField]
        private LayerMask whatToLookFor;
        [SerializeField]
        private LayerMask obstructionMask;

        public delegate void FoundItemOfInterest(Collider closestItemOfInterest);
        public event FoundItemOfInterest foundItemCallBack;

        private void Start()
        {
            StartCoroutine(CheckRoutine());
        }

        /// <summary>
        /// Coroutine to check new items into out FOV.
        /// </summary>
        private IEnumerator CheckRoutine()
        {

            WaitForSeconds waitTime = new WaitForSeconds(timeBetweenChecks);

            while (shouldBeSearching)
            {
                yield return waitTime;
                CheckForItemsOfInterest();
            }
        }

        /// <summary>
        /// Finds the closest item in our field of view and call an event.
        /// </summary>
        private void CheckForItemsOfInterest()
        {
            //Should use OverLapSphereNoAlloc but it is a game jam lol no time to deal with null pointers.
            Collider[] itemsFound = Physics.OverlapSphere(transform.position, viewRadius, whatToLookFor);

            SortColliders(itemsFound);

            foreach (Collider item in itemsFound)
            {
                Transform target = item.transform;
                Vector3 directionToTarget = (target.position - transform.position).normalized;

                if (Vector3.Angle(transform.forward, directionToTarget) < viewAngle / 2)
                {
                    float distanceToTarget = Vector3.Distance(transform.position, item.transform.position);
                    if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                    {
                        foundItemCallBack?.Invoke(item);
                    }
                }
            }
        }

        /// <summary>
        /// Sorts colliders by closest to parent object.
        /// </summary>
        /// <param name="items">Items of interest found in the FOV</param>
        private void SortColliders(Collider[] items)
        {
            items = items.OrderBy(collider => (transform.position - collider.transform.position).sqrMagnitude).ToArray();
        }

        private void OnDisable()
        {
            foundItemCallBack = null;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, viewRadius);

            Vector3 directionFromAngleA = DirectionFromAngle(transform.eulerAngles.y, -viewAngle / 2);
            Vector3 directionFromAngleB = DirectionFromAngle(transform.eulerAngles.y, viewAngle / 2);

            Gizmos.color = Color.cyan;

            Gizmos.DrawLine(transform.position, transform.position + directionFromAngleA * viewRadius);
            Gizmos.DrawLine(transform.position, transform.position + directionFromAngleB * viewRadius);

        }

        private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
        {
            angleInDegrees += eulerY;

            return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));

        }
#endif
    }
}
