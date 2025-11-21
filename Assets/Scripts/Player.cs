using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 5;

    Rigidbody2D rb2D;
    Vector2 movementInput;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        movementInput.x = Input.GetAxisRaw("Horizontal");
        movementInput.y = Input.GetAxisRaw("Vertical");

        movementInput = movementInput.normalized;
    }

    private void FixedUpdate()
    {
        rb2D.linearVelocity = movementInput * speed;
    }
}