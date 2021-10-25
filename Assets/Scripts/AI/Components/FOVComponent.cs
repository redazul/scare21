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

        [SerializeField] [Tooltip("How far the entity can see")]
        private float viewRadius;
        [SerializeField] [Tooltip("FOV angle of character")]
        private float viewAngle;
        [SerializeField] [Tooltip("How much time should pass between checks in our FOV")]
        private float timeBetweenChecks = 5f;

        [SerializeField]
        [Tooltip("from where the ray is cast")]
        private Transform eyeOrigin;

        [SerializeField] [Tooltip("Determines if we are to begin looking on Start")]
        private bool isSearching = false;

        [SerializeField][Tooltip("What we are looking for in our FOV. Like cheese, cats, other rats.")]
        private LayerMask whatToLookFor;
        [SerializeField] [Tooltip("What layers obstruct our FOV. Like walls, chairs, etc.")]
        private LayerMask obstructionMask;

        [Header("Field of View Settings")]
        [SerializeField] [Tooltip("Should begin searching on Start")]
        private bool searchOnStart = true;
        [SerializeField] [Tooltip("When true this will invoke the event even if the current item found is the previous item that has already been called.")]
        private bool recallIfSame = false;
        [SerializeField] [Tooltip("Search Type refers to which object we take in our FOV i.e. Closest, furthest, random")]
        private SearchType searchType;

        private Collider lastFound;

        public delegate void FoundItemOfInterest(Collider closestItemOfInterest);
        public event FoundItemOfInterest foundItemCallBack;

        private void Start()
        {
            if (searchOnStart)
            {
                isSearching = false;
                StartSearching();
            }
        }

        public void StartSearching()
        {
            if (!isSearching)
            {
                isSearching = true;
                StartCoroutine(CheckRoutine());
            }
        }

        public void StopSearching()
        {
            if (isSearching)
            {
                isSearching = false;
                StopCoroutine(CheckRoutine());
            }
        }

        /// <summary>
        /// Coroutine to check new items into out FOV.
        /// </summary>
        private IEnumerator CheckRoutine()
        {

            WaitForSeconds waitTime = new WaitForSeconds(timeBetweenChecks);

            while (isSearching)
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

            Collider relevantItem = null;
            float relevantItemDistance = 0;

            if(searchType == SearchType.CLOSEST)
            {
                relevantItemDistance = float.MaxValue;
            } 
            else if(searchType == SearchType.FURTHEST)
            {
                relevantItemDistance = float.MinValue;
            }

            foreach (Collider item in itemsFound)
            {
                Transform target = item.transform;
                Vector3 directionToTarget = (target.position - transform.position).normalized;

                if (Vector3.Angle(transform.forward, directionToTarget) < viewAngle / 2)
                {
                    float distanceToTarget = Vector3.Distance(transform.position, item.transform.position);
                    if (!Physics.Raycast(eyeOrigin.position, directionToTarget, distanceToTarget, obstructionMask))
                    {

                        if (searchType == SearchType.CLOSEST)
                        {
                            if (distanceToTarget < relevantItemDistance)
                            {
                                relevantItem = item;
                                distanceToTarget = relevantItemDistance;
                            }
                        }
                        else if (searchType == SearchType.FURTHEST)
                        {
                            if (distanceToTarget > relevantItemDistance)
                            {
                                relevantItem = item;
                                distanceToTarget = relevantItemDistance;
                            }
                        }
                        else if (searchType == SearchType.RANDOM)
                        {
                            if ((lastFound == item && !recallIfSame) || item == null)
                                break;

                            foundItemCallBack?.Invoke(item);
                            lastFound = item;
                            break;
                        }

                    }
                }
            }

            if (relevantItem && !recallIfSame)
            {
                if (lastFound == relevantItem || relevantItem == null)
                    return;
                lastFound = relevantItem;
                foundItemCallBack?.Invoke(relevantItem);
            }
            else
            {
                if (relevantItem == null)
                    return;
                lastFound = relevantItem;
                foundItemCallBack?.Invoke(relevantItem);
            }
        }

        /// <summary>
        /// Sorts colliders by closest to parent object.
        /// </summary>
        /// <param name="items">Items of interest found in the FOV</param>
        private void SortColliders(Collider[] items)
        {
            if (searchType == SearchType.CLOSEST)
                items = items.OrderBy(collider => (transform.position - collider.transform.position).sqrMagnitude).ToArray();
            else if (searchType == SearchType.FURTHEST)
                items = items.OrderByDescending(collider => (transform.position - collider.transform.position).sqrMagnitude).ToArray();
        }

        private void OnDisable()
        {
            foundItemCallBack = null;
        }

        [SerializeField]
        [Tooltip("If gizmo is visible for this component")]
        private bool displayGizmo = true;

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (!displayGizmo)
            {
                return;
            }
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

    public enum SearchType { CLOSEST, FURTHEST, RANDOM }
}