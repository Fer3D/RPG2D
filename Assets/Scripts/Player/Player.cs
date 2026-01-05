using UnityEditor.Animations;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 5;

    Rigidbody2D rb2D;
    Vector2 movementInput;

    private Animator animator;

    private bool gameIsPaused = false;

    private bool isAttacking = false;

    [HideInInspector]
    public bool canMove = true;

    Vector2 lastMovementDir = Vector2.right;

    Vector2 attackDir;
    public float attackRange = 1.2f;
    public int attackDamage = 1;
    public LayerMask targetLayer;

    private int xp = 0;
    [HideInInspector]
    public int currentLevel = 1;

    [Header("Skin")]
    public NPCSkin selectedSkin;
    public AnimatorController[] animatorControllers;
    public enum NPCSkin { Blue, Purple, Red, Yellow }

    public AudioSource audioAttack;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        UIManager.instance.UpdatePlayerStats(xp, currentLevel, speed, attackDamage);
        ApplySkin();
    }

    void Update()
    {
        if (movementInput != Vector2.zero)
        {
            lastMovementDir = movementInput;
        }

        movementInput.x = Input.GetAxisRaw("Horizontal");
        movementInput.y = Input.GetAxisRaw("Vertical");

        movementInput = movementInput.normalized;

        animator.SetFloat("Horizontal", Mathf.Abs(movementInput.x));
        animator.SetFloat("Vertical", Mathf.Abs(movementInput.y));

        CheckFlip();

        OpenCloseInventory();
        OpenClosePauseMenu();
        OpenCloseStatsPlayer();
        OpenCloseQuestPanel();

        Attack();
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            rb2D.linearVelocity = movementInput * speed;
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

    void OpenCloseStatsPlayer()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            UIManager.instance.OpenOrCloseStatsPlayer();
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
            int dir = GetDirectionIndex(lastMovementDir);
            attackDir = GetAttackInputDirection();
            int attackDirection = GetDirectionIndex(attackDir);

            animator.SetInteger("AttackDirection", attackDirection);

            int randomIndex = Random.Range(0, 2);
            animator.SetInteger("AttackIndex", randomIndex);
            animator.SetTrigger("DoAttack");

            audioAttack.PlayOneShot(audioAttack.clip);
        }
    }

    public void StartAttack()
    {
        isAttacking = true;
        rb2D.linearVelocity = Vector2.zero;
        canMove = false;
    }

    public void EndAttack()
    {
        isAttacking = false;
        canMove = true;
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
                obj.GetComponent<DamageReceiver>().ApplyDamage(attackDamage, true, false, hitDirection);
            }
            else if (layer == LayerMask.NameToLayer("Sheep"))
            {
                obj.GetComponent<DamageReceiver>().ApplyDamage(attackDamage, true, false, hitDirection);
            }
            else if (layer == LayerMask.NameToLayer("Tree"))
            {
                obj.GetComponent<DamageReceiver>().ApplyDamage(attackDamage, false, true, hitDirection);
            }
        }
    }

    void OnEnable()
    {
        DamageReceiver.OnTargetKilled += AddExp;
    }

    void OnDisable()
    {
        DamageReceiver.OnTargetKilled -= AddExp;
    }

    public void AddExp(int amount)
    {
        xp += amount;

        if (xp > 100)
        {
            xp -= 100;

            LevelUp();
        }

        UIManager.instance.UpdatePlayerStats(xp, currentLevel, speed, attackDamage);
    }

    private void LevelUp()
    {
        currentLevel += 1;

        switch (currentLevel)
        {
            case 2:
                speed += 1;
                attackDamage += 1;
                GetComponent<DamageReceiverPlayer>().GainHealth(1);
                break;
            case 3:
                speed += 1;
                attackDamage += 1;
                GetComponent<DamageReceiverPlayer>().GainHealth(1);
                break;
            case 4:
                speed += 1;
                attackDamage += 1;
                GetComponent<DamageReceiverPlayer>().GainHealth(1);
                break;
            default:
                break;
        }
    }

    void ApplySkin()
    {
        string savedSkinName = PlayerPrefs.GetString("MainPlayerSkin", "Blue");

        if (System.Enum.TryParse(savedSkinName, out NPCSkin skin))
        {
            selectedSkin = skin;
        }
        else
        {
            selectedSkin = NPCSkin.Blue;
        }

        if (animatorControllers != null && animatorControllers.Length > 0)
        {
            int skinIndex = (int)selectedSkin;
            if (animator != null && skinIndex < animatorControllers.Length)
            {
                animator.runtimeAnimatorController = animatorControllers[skinIndex];
            }
        }
    }

    public void OpenCloseQuestPanel()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (UIManager.instance.questPanel.activeSelf)
            {
                UIManager.instance.HideQuestPanel();
            }
            else
            {
                UIManager.instance.ShowQuestPanel();
            }
        }
    }
}