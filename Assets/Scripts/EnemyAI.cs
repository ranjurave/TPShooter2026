using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] float sightRange, attackRange, chaseRange;
    [SerializeField] Transform player;
    [SerializeField] LayerMask playerMask, groundMask;
    float timeBetweenAttacks = 0.5f;
    bool playerInSightRange, playerInAttackRange;
    NavMeshAgent agent;

    Vector3 walkpoint;
    bool walkPointSet;
    [SerializeField] float walkPointRange = 100f;
    private bool alreadyAttacked;

    private void Awake() {
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, playerMask);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerMask);

        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInAttackRange && playerInSightRange) AttackPlayer();
    }

    void ChasePlayer() {
        agent.SetDestination(player.position);
    }

    private void SearchWalkPoint() {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        walkpoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        if (Physics.Raycast(walkpoint, -transform.up, 2f, groundMask)) {
            walkPointSet = true;
        }
    }

    void Patroling() {
        Debug.Log("Patroling");
        if (!walkPointSet) { SearchWalkPoint(); }

        if (walkPointSet) {
            //LookAtTarget(); //can be added later
            agent.SetDestination(walkpoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkpoint;
        if (distanceToWalkPoint.magnitude < 1f) {
            walkPointSet = false;
        }
    }
    void AttackPlayer() {
        agent.SetDestination(transform.position);
        transform.LookAt(player);
        if (!alreadyAttacked) {
            // Attack code here
            Debug.Log("Attacking player");
            //
            alreadyAttacked = true;


            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    void ResetAttack() {
        alreadyAttacked = false;
    }


private void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }

}
