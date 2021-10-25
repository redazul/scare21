using Scare.AI.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static Scare.AI.Components.FOVComponent;

public class CatController : MonoBehaviour, ISpawnable
{
    [Tooltip("The time before the cat loses interest in the player if he wasn't found again via fovcomponent")]
    [SerializeField]
    private float timeToLosePursuitInterest = 1.25f;

    [Tooltip("The speed of the navMeshAgent when following the player.")]
    [SerializeField]
    private float pursuitSpeed = 2.2f;

    [Tooltip("How far a pursuit will go until updating the player position")]
    [SerializeField]
    private float pursuitDistance = 5f;

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
    private float attackPushMagnitude = 1.0f;

    [SerializeField]
    private List<AudioClip> catAttackClips;

    private EntityWanderer wanderer;
    private FOVComponent fovChecker;

    private Timer followPursuitTimer;
    private Timer attackReloadTimer;

    private Vector3 lastSeenPlayerPos;
    private GameObject playerGameObject;

    private bool isPursuing = false;

    private NavMeshAgent navMeshAgent;

    private bool wasSpawned = false;

    private AudioSource audioSource;

    void Awake()
    {
        wanderer = GetComponent<EntityWanderer>();
        fovChecker = GetComponent<FOVComponent>();
        navMeshAgent = GetComponent<NavMeshAgent>();

        followPursuitTimer = gameObject.AddComponent<Timer>();
        followPursuitTimer.Init(timeToLosePursuitInterest, LoseInterest);
        followPursuitTimer.SetPaused(true);

        attackReloadTimer = gameObject.AddComponent<Timer>();
        attackReloadTimer.Init(attackReloadTime, ContinueMovement);
        attackReloadTimer.SetPaused(true);

        audioSource = GetComponent<AudioSource>();
        audioSource.loop = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        fovChecker.foundItemCallBack += OnFoundItemOfInterest;
    }

    // Update is called once per frame
    void Update()
    {
        if (References.GetPaused()) return;

        UpdatePursuit();
    }

    private void UpdatePursuit()
    {
        if (!isPursuing || attackReloadTimer.IsRunning())
        {
            return;
        }
        if (playerGameObject != null && AIUtil.FlatDistance(transform.position, playerGameObject.transform.position) < playerAttackRange)
        {
            navMeshAgent.isStopped = true; //movement will continue via attackReloadTimer

            attackReloadTimer.Reset();
            attackReloadTimer.SetPaused(false);
            AttackPlayer();
        }
    }

    private void AttackPlayer()
    {
        /*playerGameObject.GetComponent<PlayerController>().ReduceHealth();

        if(attackPushMagnitude > 0.0f)
        {
            //Vector3 pushForce = (playerGameObject.transform.position - transform.position).normalized * attackPushMagnitude;
            //pushForce.y = 0.5f;
            //playerGameObject.GetComponent<Rigidbody>()?.AddForce(pushForce, ForceMode.Impulse);
        }*/
        audioSource.clip = PlSoundPlayer.GetRandomFromList(catAttackClips);
        audioSource.Play();
        playerGameObject.GetComponent<PlayerController>().GetHit(attackPushMagnitude, transform);

    }

    private void OnFoundItemOfInterest(Collider closestItemOfInterest)
    {
        if (closestItemOfInterest.gameObject.transform.root.CompareTag(PlayerController.PLAYER_TAG))
        {
            if (!playerGameObject)
            {
                playerGameObject = closestItemOfInterest.gameObject.transform.root.gameObject;
            }
            if (!playerGameObject.GetComponent<PlayerController>().IsDead())
            {
                GetComponentInChildren<Light>().color = Color.red;
                UpdatePlayerPursuit();
            }
        }
    }

    private void UpdatePlayerPursuit()
    {
        lastSeenPlayerPos = playerGameObject.transform.position;

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
        navMeshAgent.speed = pursuitSpeed;
        playerGameObject.GetComponent<PlayerController>().StartDangerMode();
    }

    private void LoseInterest()
    {
        //stop pursuing
        isPursuing = false;
        navMeshAgent.isStopped = true;
        GetComponentInChildren<Light>().color = Color.yellow;
        followPursuitTimer.SetPaused(true);
        playerGameObject.GetComponent<PlayerController>().StopDangerMode();

        //start wandering
        navMeshAgent.speed = wanderSpeed;
        wanderer.StartWandering(false);
    }

    public void SetSpawned(bool wasSpawned)
    {
        this.wasSpawned = wasSpawned;
    }

    private void ContinueMovement()
    {
        navMeshAgent.isStopped = false;
    }
}
