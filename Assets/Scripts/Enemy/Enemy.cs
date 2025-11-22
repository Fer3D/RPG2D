using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 7;

    private Rigidbody2D rb2D;

    public Transform targetTransform;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rb2D.MovePosition(Vector2.MoveTowards(transform.position, targetTransform.position, speed * Time.deltaTime));
    }
}
