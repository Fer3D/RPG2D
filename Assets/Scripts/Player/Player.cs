using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 5;

    Rigidbody2D rb2D;
    Vector2 movementInput;

    private Animator animator;

    private int currentHealth;
    public int maxHealth = 100;

    private bool gameIsPaused = false;

    private bool isAttacking = false;
    private bool canMove = true;

    Vector2 lasMovementDir = Vector2.right;

    Vector2 attackDir;
    public float attackRange = 1.2f;
    public LayerMask targetLayer;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        currentHealth = maxHealth;
        UIManager.instance.UpdateHealth(currentHealth);
    }

    void Update()
    {
        if (isAttacking)
            canMove = false;
        else
            canMove = true;

        if (movementInput != Vector2.zero)
        {
            lasMovementDir = movementInput;
        }

        movementInput.x = Input.GetAxisRaw("Horizontal");
        movementInput.y = Input.GetAxisRaw("Vertical");

        movementInput = movementInput.normalized;

        animator.SetFloat("Horizontal", Mathf.Abs(movementInput.x));
        animator.SetFloat("Vertical", Mathf.Abs(movementInput.y));

        CheckFlip();

        OpenCloseInventory();

        OpenClosePauseMenu();

        Attack();
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            rb2D.linearVelocity = movementInput * speed;
        }
        else
        {
            rb2D.linearVelocity = Vector2.zero;
        }
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

    void OpenClosePauseMenu()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused)
            {
                UIManager.instance.ResumeGame();
                gameIsPaused = false;
            }
            else
            {
                UIManager.instance.PauseGame();
                gameIsPaused = true;
            }
        }
    }

    void Attack()
    {
        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
            int dir = GetDirectionIndex(lasMovementDir);
            attackDir = GetAttackInputDirection();
            int attackDirection = GetDirectionIndex(attackDir);

            animator.SetInteger("AttackDirection", attackDirection);

            int randomIndex = Random.Range(0, 2);
            animator.SetInteger("AttackIndex", randomIndex);
            animator.SetTrigger("DoAttack");
        }
    }

    public void StartAttack()
    {
        isAttacking = true;
    }

    public void EndAttack()
    {
        isAttacking = false;
    }
    Vector2 GetAttackInputDirection()
    {
        Vector2 inputDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        if (inputDir != Vector2.zero)
        {
            return inputDir;
        }
        else
        {
            if (transform.localScale.x > 0)
            {
                return Vector2.right;
            }
            else
            {
                return Vector2.left;
            }
        }
    }

    int GetDirectionIndex(Vector2 dir)
    {
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            return dir.x > 0 ? 0 : 1;
        }
        else
        {
            return dir.y > 0 ? 2 : 3;
        }
    }

    public void DetectAndDamageTargets()
    {
        Vector2 attackPoint = (Vector2)transform.position + attackDir.normalized * attackRange * 0.5f;
        Collider2D[] hitTargets = Physics2D.OverlapCircleAll(attackPoint, attackRange, targetLayer);

        foreach (Collider2D target in hitTargets)
        {
            Vector2 hitDirection = (target.transform.position - transform.position).normalized;

            GameObject obj = target.gameObject;

            int layer = obj.layer;

            if (layer == LayerMask.NameToLayer("Enemy"))
            {
                obj.GetComponent<DamageReceiver>().ApplyDamage(1, true, false, hitDirection);
            }
            else if (layer == LayerMask.NameToLayer("Sheep"))
            {
                obj.GetComponent<DamageReceiver>().ApplyDamage(1, true, false, hitDirection);
            }
            else if (layer == LayerMask.NameToLayer("Tree"))
            {
                obj.GetComponent<DamageReceiver>().ApplyDamage(1, false, true, hitDirection);
            }
        }
    }
}