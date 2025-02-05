using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform player; // Assign the player in the Inspector
    public Transform front;  // Assign an empty GameObject as the front
    public float chaseRange = 10f;
    public float roamRadius = 15f;

    private NavMeshAgent agent;
    private Vector3 roamPosition;
    private bool isChasing = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        SetNewRoamPosition();
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= chaseRange)
        {
            isChasing = true;
            agent.SetDestination(player.position);
        }
        else if (isChasing)
        {
            isChasing = false;
            SetNewRoamPosition();
        }

        if (!isChasing && agent.remainingDistance <= agent.stoppingDistance)
        {
            SetNewRoamPosition();
        }

        RotateFront();
    }

    void SetNewRoamPosition()
    {
        Vector3 randomDirection = Random.insideUnitSphere * roamRadius;
        randomDirection += transform.position;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, roamRadius, NavMesh.AllAreas))
        {
            roamPosition = hit.position;
            agent.SetDestination(roamPosition);
        }
    }

    void RotateFront()
    {
        if (front != null && agent.desiredVelocity.sqrMagnitude > 0.1f)
        {
            // Rotate the front object to always face the movement direction
            front.rotation = Quaternion.LookRotation(agent.desiredVelocity.normalized);
        }
    }
}
