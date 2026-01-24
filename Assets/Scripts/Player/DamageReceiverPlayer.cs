using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Cinemachine;

public class DamageReceiverPlayer : MonoBehaviour
{
    [Header("Stats")]
    public int maxHealth = 5;
    public int currentHealth;

    private Rigidbody2D rb2D;
    private Animator animator;

    public float forceImpulse = 5;
    [Header("Cinemachine")]
    [Range(0f, 3f)]
    [Tooltip("Multiplicate the shake force.")]
    public float shakeForce = 1f;
    [Tooltip("Additional multiplier to scale the Cinemachine impulse signal.")]
    public float shakeMultiplier = 1f;
    [SerializeField]
    [Tooltip("Assign a Cinemachine Impulse Source if you want manual control; it will be added automatically if missing.")]
    private CinemachineImpulseSource impulseSource;

    public AudioSource audioDie;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        impulseSource = GetComponent<CinemachineImpulseSource>();
        if (impulseSource == null)
        {
            impulseSource = GetComponentInChildren<CinemachineImpulseSource>();
        }
        if (impulseSource == null)
        {
            impulseSource = gameObject.AddComponent<CinemachineImpulseSource>();
        }

        currentHealth = maxHealth;
        UIManager.instance.UpdateHealth(currentHealth, maxHealth);

    }

    public void ApplyDamage(int amount, bool applyForceOrNot, bool applyHitAnimation, Vector2 hitDirection)
    {
        currentHealth -= amount;
        UIManager.instance.UpdateHealth(currentHealth, maxHealth);

        if (impulseSource != null)
        {
            impulseSource.GenerateImpulse(hitDirection.normalized * shakeForce * shakeMultiplier);
        }

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

    public void GainHealth(int healthAmount)
    {
        maxHealth += healthAmount;
        currentHealth = maxHealth;
        UIManager.instance.UpdateHealth(currentHealth, maxHealth);
    }

    void Die()
    {
        GetComponent<Player>().enabled = false; // Deshabilitamos el script del Player para que no se pueda mover al morir
        animator.SetTrigger("Die");
        UIManager.instance.DiePanelAnimation();
        audioDie.Play();
        Invoke("ResetLevel", 2);
    }

    public void ResetLevel()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}
