using System.Collections;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.AI;

public class NeutralNPC : MonoBehaviour
{
    private Rigidbody2D rb2D;
    NavMeshAgent navMeshAgent;

    Animator animator;

    [Header("Skin")]
    public AnimatorController[] animatorControllers;

    public NPCSkin selectedSkin;
    public enum NPCSkin
    {
        Blue,
        Purple,
        Red,
        Yellow
    }

    [Header("Movement Type")]
    public MovementType movementType;

    public enum MovementType
    {
        Path,
        RandomMovement
    }

    [Header("Path")]
    public Transform[] pathPoints;
    public float waitTimeInPoint = 3;
    private int indexPath = 0;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;

        ApplySkin();

        if (movementType == MovementType.Path)
        {
            StartCoroutine(FollowPath());
        }
    }

    void Update()
    {
        AdjustAnimationsAndRotation();
    }

    public void AdjustAnimationsAndRotation()
    {
        bool isMoving = navMeshAgent.velocity.sqrMagnitude > 0.01f;
        animator.SetBool("isRunning", isMoving);

        if (navMeshAgent.desiredVelocity.x > 0.01f)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (navMeshAgent.desiredVelocity.x < -0.01f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

    }

    public void ApplySkin()
    {
        int skinIndex = (int)selectedSkin;

        animator.runtimeAnimatorController = animatorControllers[skinIndex];
    }

    IEnumerator FollowPath()
    {
        while (true)
        {
            if (pathPoints.Length > 0)
            {
                navMeshAgent.SetDestination(pathPoints[indexPath].position);

                while (!navMeshAgent.pathPending && navMeshAgent.remainingDistance > 0.1f)
                {
                    yield return null;
                }
                yield return new WaitForSeconds(waitTimeInPoint);

                indexPath = (indexPath + 1) % pathPoints.Length;
            }
            yield return null;
        }
    }
}
