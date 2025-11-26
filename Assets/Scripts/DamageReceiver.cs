using UnityEngine;

public class DamageReceiver : MonoBehaviour
{
    [Header("Stats")]
    public int maxHealth = 1;
    private int currentHealth;

    [Header("Drop")]
    public GameObject itemToDrop;
    public Rigidbody2D rb2D;
    public float forceImpulse = 5;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void ApplyDamage(int amount, bool applyForceOrNot, bool applyHitAnimation, Vector2 hitDirection)
    {
        currentHealth -= amount;
        if (applyForceOrNot)
        {
            rb2D.bodyType = RigidbodyType2D.Dynamic;
            rb2D.linearVelocity = Vector2.zero;
            rb2D.AddForce(hitDirection.normalized * forceImpulse, ForceMode2D.Impulse);
            Invoke("ReturnToKinematic", 0.2f);
        }

        if (applyHitAnimation)
        {
            
        }

        if (currentHealth <= 0)
        {
            DropItem();
            Die();
        }
    }

    void ReturnToKinematic()
    {
        rb2D.linearVelocity = Vector2.zero;
        rb2D.bodyType = RigidbodyType2D.Kinematic;
    }

    void DropItem()
    {
        Instantiate(itemToDrop, transform.position + Vector3.up, Quaternion.identity);
    }

    void Die()
    {
        Destroy(gameObject);
    }
}