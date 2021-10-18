using Scare.AI.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static Scare.AI.Components.FOVComponent;

public class CatController : MonoBehaviour
{
    private const string PLAYER_TAG = "Player";

    [Tooltip("The time before the cat loses interest in the player if he wasn't found again via fovcomponent")]
    [SerializeField]
    private float timeToLosePursuitInterest = 1.25f;

    [Tooltip("The speed of the navMeshAgent when following the player.")]
    [SerializeField]
    private float pursuitSpeed = 2.2f;

    [Tooltip("The speed of the navMeshAgent when wandering around.")]
    [SerializeField]
    private float wanderSpeed = 1.6f;

    [Tooltip("The range for which the cat can attack the player")]
    [SerializeField]
    private float playerAttackRange = 2f;

    [Tooltip("The time between cat attacks")]
    [SerializeField]
    private float attackReloadTime = 2f;

    [Tooltip("How strong the pushback of an attack is. 0 <=> no pushback")]
    [SerializeField]
    private float attackPushMagnitude = 2.0f;

    EntityWanderer wanderer;
    FOVComponent fovChecker;

    private Timer followPursuitTimer;
    private Timer attackReloadTimer;

    private Vector3 lastSeenPlayerPos;
    private GameObject playerGameObject;

    private bool isPursuing = false;

    private NavMeshAgent navMeshAgent;

    void Awake()
    {
        wanderer = GetComponent<EntityWanderer>();
        fovChecker = GetComponent<FOVComponent>();
        navMeshAgent = GetComponent<NavMeshAgent>();

        followPursuitTimer = gameObject.AddComponent<Timer>();
        followPursuitTimer.Init(timeToLosePursuitInterest, LoseInterest);
        followPursuitTimer.SetPaused(true);

        attackReloadTimer = gameObject.AddComponent<Timer>();
        attackReloadTimer.Init(attackReloadTime);
        attackReloadTimer.SetPaused(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        fovChecker.foundItemCallBack += OnFoundItemOfInterest;
    }

    // Update is called once per frame
    void Update()
    {
        CheckAttackPlayerOnPursuit();
    }

    private void CheckAttackPlayerOnPursuit()
    {
        if (!isPursuing || attackReloadTimer.IsRunning())
        {
            return;
        }
        if (playerGameObject != null && AIUtil.FlatDistance(transform.position, playerGameObject.transform.position) < playerAttackRange)
        {
            attackReloadTimer.Reset();
            attackReloadTimer.SetPaused(false);
            AttackPlayer(playerGameObject);
        }
    }

    private void AttackPlayer(GameObject playerGO)
    {
        playerGO.GetComponent<PlayerController>().ReduceHealth();

        if(attackPushMagnitude > 0.0f)
        {
            Vector3 pushForce = (playerGameObject.transform.position - transform.position).normalized * attackPushMagnitude;
            pushForce.y = 0.5f;
            playerGO.GetComponent<Rigidbody>().AddForce(pushForce, ForceMode.Impulse);
        }
    }


    private void OnFoundItemOfInterest(Collider closestItemOfInterest)
    {
        if (closestItemOfInterest.tag == PLAYER_TAG)
        {
            GetComponentInChildren<Light>().color = Color.red;
            UpdatePlayerPursuit(closestItemOfInterest.gameObject);
        }
    }

    private void UpdatePlayerPursuit(GameObject playerGO)
    {
        playerGameObject = playerGO;
        lastSeenPlayerPos = playerGO.transform.position;

        NavMeshHit closestNavMeshPosition;
        NavMesh.SamplePosition(lastSeenPlayerPos, out closestNavMeshPosition, 2.0f, NavMesh.AllAreas);
        navMeshAgent.SetDestination(closestNavMeshPosition.position);


        if (!isPursuing)
        {
            wanderer.StopWandering();

            StartPursuingPlayer();
        }
        else
        {
            followPursuitTimer.Reset();
        }
    }

    private void StartPursuingPlayer()
    {
        isPursuing = true;
        followPursuitTimer.Reset();
        followPursuitTimer.SetPaused(false);

        navMeshAgent.isStopped = false;
    }

    private void LoseInterest()
    {
        //stop pursuing
        isPursuing = false;
        navMeshAgent.isStopped = true;
        GetComponentInChildren<Light>().color = Color.yellow;
        followPursuitTimer.SetPaused(true);

        //start wandering
        wanderer.StartWandering(false);
    }
}
