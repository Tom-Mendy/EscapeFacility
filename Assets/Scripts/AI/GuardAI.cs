using UnityEngine;
using UnityEngine.AI;

public enum GuardState
{
    Patrol,
    Alerted,
    Chase,
    Search,
    Flee,
    Return
}

[RequireComponent(typeof(NavMeshAgent))]
public class GuardAI : MonoBehaviour
{
    [Header("Refs")]
    public Transform player;
    public VisionCone visionCone;

    [Header("Patrol")]
    public Transform[] waypoints;
    private int waypointIndex = 0;

    [Header("Vision")]
    public float visionRange = 10f;
    public float fov = 60f;

    [Header("Stats")]
    public float health = 100f;
    public float fleeHealthThreshold = 25f;

    [Header("Search")]
    public float searchDuration = 6f;
    public float searchRadius = 4f;
    public float timeBetweenSearchPoints = 2f;

    [Header("Alert")]
    public float alertedInvestigateRadius = 0.5f;
    public float hearingRadiusMultiplier = 1f; // used with noise radius to set priority

    private NavMeshAgent agent;
    public Animator _animator;
    private int _animIDSpeed;
    private int _animIDState;

    [HideInInspector] public GuardState currentState = GuardState.Patrol;
    private Vector3 lastKnownPosition; // last seen or heard pos
    private float searchTimer = 0f;
    private float nextSearchPointTimer = 0f;

    void OnEnable()
    {
        AIManager.OnNoiseRaised += HandleNoise;
    }

    void OnDisable()
    {
        AIManager.OnNoiseRaised -= HandleNoise;
    }


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (visionCone == null)
            visionCone = GetComponent<VisionCone>();

        if (waypoints != null && waypoints.Length > 0)
        {
            foreach (Transform wp in waypoints)
            {
                if (wp == null || wp.position == null)
                {
                    Debug.LogError("One waypoint is not correctly set");
                    return;
                }
               

            }


        }
        GoToNextWaypoint();
    }

    void Update()
    {
        // visual check each frame
        if (player != null && visionCone != null)
        {
            if (visionCone.CanSee(transform, player))
            {
                // set last known and switch to chase
                lastKnownPosition = player.position;
                currentState = GuardState.Chase;
                AIManager.Instance?.PlayerSpotted(player); // informs global systems
            }
        }

        switch (currentState)
        {
            case GuardState.Patrol:
                Patrol();
                break;
            case GuardState.Alerted:
                Alerted();
                break;
            case GuardState.Chase:
                Chase();
                break;
            case GuardState.Search:
                Search();
                break;
            case GuardState.Flee:
                Flee();
                break;
            case GuardState.Return:
                Return();
                break;
        }

        // global health reaction
        if (health < fleeHealthThreshold && currentState != GuardState.Flee)
        {
            currentState = GuardState.Flee;
        }
        // update animator parameters
        if (_animator != null && agent != null)
        {
            _animator.SetFloat("Speed", agent.velocity.magnitude);
            _animator.SetFloat("MotionSpeed", 1);
        }
    }

    void GoToNextWaypoint()
    {
        if (waypoints.Length == 0)
            return;

        agent.isStopped = false;
        agent.SetDestination(waypoints[waypointIndex].position);
        waypointIndex = (waypointIndex + 1) % waypoints.Length;
    }

    void Patrol()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
            GoToNextWaypoint();
        DetectPlayer();
    }

    void Alerted()
    {
        // move to investigate lastKnownPosition
        agent.isStopped = false;
        if (!agent.pathPending)
        {
            agent.SetDestination(lastKnownPosition);
            // when close enough, switch to Search
            if (Vector3.Distance(transform.position, lastKnownPosition) <= alertedInvestigateRadius)
            {
                EnterSearch();
            }
        }
    }

    void EnterSearch()
    {
        currentState = GuardState.Search;
        searchTimer = searchDuration;
        nextSearchPointTimer = 0f;
    }

    void DetectPlayer()
    {
        Vector3 dir = player.position - transform.position;
        if (Vector3.Angle(transform.forward, dir) < fov / 2 &&
            Vector3.Distance(transform.position, player.position) < visionRange)
            currentState = GuardState.Chase;
    }

    void Search()
    {
        agent.isStopped = false;
        searchTimer -= Time.deltaTime;
        nextSearchPointTimer -= Time.deltaTime;

        // If sees player while searching, go to Chase (vision check at top of Update handles it)
        if (searchTimer <= 0f)
        {
            // end searching -> return to closest waypoint
            currentState = GuardState.Return;
            return;
        }

        // every N seconds choose a random navmesh point near lastKnownPosition
        if (nextSearchPointTimer <= 0f)
        {
            Vector3 p = RandomNavmeshPoint(lastKnownPosition, searchRadius);
            if (p != Vector3.zero) agent.SetDestination(p);
            nextSearchPointTimer = timeBetweenSearchPoints;
        }
    }

    void Chase()
    {
        if (player == null)
        {
            currentState = GuardState.Return;
            return;
        }

        agent.isStopped = false;
        agent.SetDestination(player.position);

        if (!visionCone.CanSee(transform, player))
        {
            // store last seen pos and go search
            currentState = GuardState.Search;
            lastKnownPosition = player.position;
            searchTimer = searchDuration;
            nextSearchPointTimer = 0f;
        }
    }

    void Flee()
    {
        // flee from player towards opposite direction or a safe point
        if (player == null)
        {
            currentState = GuardState.Return;
            return;
        }

        Vector3 dir = (transform.position - player.position).normalized;
        Vector3 fleeTarget = transform.position + dir * 8f;

        Vector3 navTarget = RandomNavmeshPoint(fleeTarget, 2f);
        if (navTarget != Vector3.zero)
        {
            agent.SetDestination(navTarget);
        }

        // simple condition: if far enough from player -> go to Return or Patrol
        if (Vector3.Distance(transform.position, player.position) > 12f)
        {
            currentState = GuardState.Return;
        }
    }

    void Return()
    {
        // get nearest waypoint and go
        if (waypoints == null || waypoints.Length == 0)
        {
            currentState = GuardState.Patrol;
            return;
        }

        // choose closest waypoint
        Transform closest = waypoints[0];
        float min = Vector3.Distance(transform.position, closest.position);
        for (int i = 1; i < waypoints.Length; i++)
        {
            float d = Vector3.Distance(transform.position, waypoints[i].position);
            if (d < min) { min = d; closest = waypoints[i]; }
        }

        agent.SetDestination(closest.position);

        if (!agent.pathPending && agent.remainingDistance < 0.6f)
        {
            currentState = GuardState.Patrol;
        }
    }

    public void HandleNoise(Vector3 pos, float radius)
    {
        // check if within hearing range (radius * multiplier)
        float dist = Vector3.Distance(transform.position, pos);
        if (dist <= radius * hearingRadiusMultiplier)
        {
            // set last known pos and go investigate if not engaged in fleeing or chasing
            lastKnownPosition = pos;
            if (currentState != GuardState.Chase && currentState != GuardState.Flee)
            {
                currentState = GuardState.Alerted;
            }
        }
    }

    public Vector3 RandomNavmeshPoint(Vector3 origin, float dist)
    {
        Vector3 randDir = UnityEngine.Random.insideUnitSphere * dist;
        randDir += origin;
        if (NavMesh.SamplePosition(randDir, out NavMeshHit hit, dist, NavMesh.AllAreas))
        {
            return hit.position;
        }
        return Vector3.zero;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, visionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 2f);
    }

    public void ApplyDamage(float dmg)
    {
        health -= dmg;
        if (health <= 0f)
        {
            gameObject.SetActive(false);
        }
    }
}
