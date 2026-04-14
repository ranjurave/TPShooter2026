using UnityEngine;
using UnityEngine.AI;

public class AIEnemyScript : MonoBehaviour
{
    NavMeshAgent agent;
    [SerializeField] float attackRange, sightRange;
    [SerializeField] Transform player;
    [SerializeField] LayerMask playerLayer, groundLayer;
    [SerializeField] float patrolRange;
    [SerializeField] GameObject projectileBullet;
    [SerializeField] Transform firePoint;
    Vector3 walkPoint;

    bool playerInSightRange, playerInAttackRange;
    bool walkPointSet;
    bool alreadyShooting = false;

    private void Awake() {
          agent = GetComponent<NavMeshAgent>();
    }

    void Update() {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, playerLayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayer);

        if (!playerInSightRange && !playerInAttackRange) Patrol();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer(); 
        if (playerInAttackRange && playerInSightRange) AttackPlayer();
    }

    void ChasePlayer() {
        //Debug.Log("Chasing player");
        if (playerInSightRange) {
            agent.SetDestination(player.position);
        }
    }

    void AttackPlayer() {
        // Attack logic here
        //Debug.Log("Attacking player");
        agent.SetDestination(transform.position); // Stop moving to attack
        transform.LookAt(player);

        if(!alreadyShooting) {
            Instantiate(projectileBullet, firePoint.position, firePoint.rotation);

            alreadyShooting = true;
            Invoke(nameof(ResetShoot), 1.0f); 
        }
    }

    void ResetShoot() {
        alreadyShooting = false;
    }

    void Patrol() {
        // Patrol logic here
        //Debug.Log("Patrolling");
        if(!walkPointSet) SearchWalkPoint();

        Vector3 distanceToWalk = transform.position - walkPoint;
        if (distanceToWalk.magnitude < 1.0f) walkPointSet = false;

        if (walkPointSet) {
            agent.SetDestination(walkPoint);
        }
    }

    void SearchWalkPoint() {
        float randomZ = Random.Range(-patrolRange, patrolRange);
        float randomX = Random.Range(-patrolRange, patrolRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.CheckSphere(walkPoint, 2.0f, groundLayer)) {
            walkPointSet = true;
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
