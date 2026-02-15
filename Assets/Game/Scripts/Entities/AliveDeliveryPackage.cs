using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class AliveDeliveryPackage : Entity
{
    public float WaitTime = 15.0f;
    private NavMeshAgent agent;
    private bool IsAlive = false;
    private Package package;
    private Interaction interaction;
    private Player player;
    private NavMeshObstacle navMeshObstacle;
    private BoxCollider boxCollider;
    private Collider agentCollider;

    private Vector3 target;

    void Start()
    {
        TakingDamageIsAvailable = false;
        interaction = FindFirstObjectByType<Interaction>();
        boxCollider = GetComponent<BoxCollider>();
        agentCollider = GetComponent<Collider>();
        navMeshObstacle = gameObject.AddComponent<NavMeshObstacle>();
        navMeshObstacle.center = boxCollider.center;
        navMeshObstacle.size = boxCollider.size;
        navMeshObstacle.carving = true;
        navMeshObstacle.carveOnlyStationary = false;
        
        if(TryGetComponent(out Package _package))
        {
            package = _package;
            StartCoroutine(WaitForSetAlive());
        }
        package.OnDelivered += StopAllCoroutines;
    }

    private IEnumerator WaitForSetAlive()
    {
        yield return new WaitForSeconds(WaitTime);

        interaction.DropObject();
        SetAlive();
    }

    [ContextMenu("Set Alive")]
    public void SetAlive()
    {
        if (IsAlive) return;
        Destroy(GetComponent<Rigidbody>());
        Destroy(navMeshObstacle);

        if(!TryGetComponent(out NavMeshAgent _agent)) agent = gameObject.AddComponent<NavMeshAgent>();
        else agent = _agent;
        agent.baseOffset = 0;
        agent.height = agentCollider.bounds.size.y/2;
        agent.radius = Mathf.Max(agentCollider.bounds.size.x, agentCollider.bounds.size.z) / 2f;
        agent.angularSpeed = 360;
        // agent.baseOffset = -agent.height;


        IsAlive = true;
        player = FindFirstObjectByType<Player>();
        package.InvokeDestroy();
    }

    private Vector3 GetRandomPointOnNavMesh(float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += transform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, NavMesh.AllAreas))
        {
            return hit.position;
        }

        return transform.position;
    }
    
    void Update()
    {
        if(!IsAlive) return;

        target = player.gameObject.transform.position;
        
        if (Physics.Raycast(transform.position, transform.forward, 2.0f))
        {
            agent.SetDestination(GetRandomPointOnNavMesh(25f));
        }

        if (!agent.hasPath)
        {
            agent.SetDestination(GetRandomPointOnNavMesh(25f));
        }
    }
}
