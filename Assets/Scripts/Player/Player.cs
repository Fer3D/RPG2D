using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 5;

    Rigidbody2D rb2D;
    Vector2 movementInput;

    private Animator animator;

    private int currentHealth;
    public int maxHealth = 100;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        currentHealth = maxHealth;
        UIManager.instance.UpdateHealth(currentHealth);
    }

    void Update()
    {
        movementInput.x = Input.GetAxisRaw("Horizontal");
        movementInput.y = Input.GetAxisRaw("Vertical");

        movementInput = movementInput.normalized;

        animator.SetFloat("Horizontal", Mathf.Abs(movementInput.x));
        animator.SetFloat("Vertical", Mathf.Abs(movementInput.y));

        CheckFlip();

        OpenCloseInventory();
    }

    private void FixedUpdate()
    {
        rb2D.linearVelocity = movementInput * speed;
    }

    void CheckFlip()
    {
        if (movementInput.x > 0 && transform.localScale.x < 0 || movementInput.x < 0 && transform.localScale.x > 0)
        {
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }
    }

    void OpenCloseInventory()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            UIManager.instance.OpenOrCloseInventory();
        }
    }

}