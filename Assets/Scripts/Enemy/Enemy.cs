using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public float speed = 7;

    private Rigidbody2D rb2D;

    public Transform targetTransform;

    NavMeshAgent navMeshAgent;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
    }

    void Update()
    {
        navMeshAgent.SetDestination(targetTransform.position);
    }
}
