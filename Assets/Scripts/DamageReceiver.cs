using UnityEngine;

public class DamageReceiver : MonoBehaviour
{
    [Header("Stats")]
    public int maxHealth = 1;
    private int currentHealth;

    [Header("Drop")]
    public GameObject[] itemToDrop;
    private Rigidbody2D rb2D;
    private Animator animator;
    public float forceImpulse = 5;

    public int xpOnDeath = 25;
    public static event System.Action<int> OnTargetKilled;

    public AudioSource audioSource;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    public void ApplyDamage(int amount, bool applyForceOrNot, bool applyHitAnimation, Vector2 hitDirection)
    {
        currentHealth -= amount;

        if (audioSource != null)
        {
            audioSource.Play();
        }

        if (applyForceOrNot)
        {
            rb2D.bodyType = RigidbodyType2D.Dynamic;
            rb2D.linearVelocity = Vector2.zero;
            rb2D.AddForce(hitDirection.normalized * forceImpulse, ForceMode2D.Impulse);
            Invoke("ReturnToKinematic", 0.2f);
        }

        if (applyHitAnimation)
        {
            animator.SetTrigger("Hit");
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
        for (int i = 0; i < itemToDrop.Length; i++)
        {
            Instantiate(itemToDrop[i], transform.position, Quaternion.identity);
        }
    }

    void Die()
    {
        OnTargetKilled?.Invoke(xpOnDeath);
        Destroy(gameObject);
    }
}