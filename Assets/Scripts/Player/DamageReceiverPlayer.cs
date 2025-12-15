using UnityEngine;

public class DamageReceiverPlayer : MonoBehaviour
{
    [Header("Stats")]
    public int maxHealth = 5;
    private int currentHealth;

    private Rigidbody2D rb2D;
    private Animator animator;

    public float forceImpulse = 5;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        currentHealth = maxHealth;
        UIManager.instance.UpdateHealth(currentHealth, maxHealth);

    }

    public void ApplyDamage(int amount, bool applyForceOrNot, bool applyHitAnimation, Vector2 hitDirection)
    {
        currentHealth -= amount;
        UIManager.instance.UpdateHealth(currentHealth, maxHealth);

        if (applyForceOrNot)
        {
            GetComponent<Player>().canMove = false;
            rb2D.AddForce(hitDirection.normalized * forceImpulse, ForceMode2D.Impulse);
            Invoke("ResetMovement", 0.1f);
        }

        if (applyHitAnimation)
        {
            animator.SetTrigger("Hit");
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void ResetMovement()
    {
        GetComponent<Player>().canMove = true;
    }

    void Die()
    {
        // Reset Level
    }
}
