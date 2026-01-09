using UnityEditor.Animations;
using UnityEngine;
using System.Collections;

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

    SaveLoadManagerJson saveLoad;

    [Header("Dash")]
    public float dashSpeed = 15f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    private bool isDashing = false;
    private float lastDashTime = -Mathf.Infinity;
    private bool isLoaded = false;

    void Awake()
    {
        if (PlayerPrefs.GetInt("LoadOnStart", 0) == 1)
        {
            PlayerPrefs.SetInt("IsLoadedGame", 1);
            isLoaded = true;
            PlayerPrefs.SetInt("LoadOnStart", 0);
        }
    }

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        saveLoad = FindAnyObjectByType<SaveLoadManagerJson>();

        UIManager.instance.UpdatePlayerStats(xp, currentLevel, speed, attackDamage);
        ApplySkin();

        if (isLoaded)
        {
            LoadGame();
        }
    }

    public void SaveGame()
    {
        int health = GetComponent<DamageReceiverPlayer>().currentHealth;
        int maxHealth = GetComponent<DamageReceiverPlayer>().maxHealth;
        Vector2 position = transform.position;
        var resourceCollector = GetComponent<PlayerResourceCollector>();
        int money = resourceCollector != null ? resourceCollector.GetMoney() : 0;
        int meat = resourceCollector != null ? resourceCollector.GetMeat() : 0;
        int wood = resourceCollector != null ? resourceCollector.GetWood() : 0;
        PlayerPrefs.SetString("MainPlayerSkin", selectedSkin.ToString());
        saveLoad.SaveGame(speed, xp, currentLevel, attackDamage, selectedSkin.ToString(), health, maxHealth, position, money, meat, wood);
    }

    public void LoadGame()
    {
        SaveData loadedData = saveLoad.LoadGame();
        if (loadedData != null)
        {
            speed = loadedData.speed;
            xp = loadedData.xp;
            currentLevel = loadedData.currentLevel;
            attackDamage = loadedData.attackDamage;
            if (!string.IsNullOrEmpty(loadedData.selectedSkin))
            {
                if (System.Enum.TryParse(loadedData.selectedSkin, out NPCSkin skin))
                {
                    selectedSkin = skin;
                    PlayerPrefs.SetString("MainPlayerSkin", loadedData.selectedSkin);
                }
            }
            GetComponent<DamageReceiverPlayer>().currentHealth = loadedData.health;
            GetComponent<DamageReceiverPlayer>().maxHealth = loadedData.maxHealth;
            transform.position = loadedData.position;
            var resourceCollector = GetComponent<PlayerResourceCollector>();
            if (resourceCollector != null)
            {
                resourceCollector.SetMoney(loadedData.money);
                resourceCollector.SetMeat(loadedData.meat);
                resourceCollector.SetWood(loadedData.wood);
                resourceCollector.UpdateAllResources();
            }
            ApplySkin();
            UIManager.instance.UpdatePlayerStats(xp, currentLevel, speed, attackDamage);
            UIManager.instance.UpdateHealth(loadedData.health, loadedData.maxHealth);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            SaveGame();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadGame();
        }


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
        Dash();
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

    void Dash()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isDashing && Time.time >= lastDashTime + dashCooldown)
        {
            Vector2 dashDir = lastMovementDir;
            if (dashDir == Vector2.zero)
            {
                dashDir = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
            }
            StartCoroutine(PerformDash(dashDir));
        }
    }

    private IEnumerator PerformDash(Vector2 direction)
    {
        isDashing = true;
        canMove = false;
        lastDashTime = Time.time;

        rb2D.linearVelocity = direction.normalized * dashSpeed;

        yield return new WaitForSeconds(dashDuration);

        rb2D.linearVelocity = Vector2.zero;
        canMove = true;
        isDashing = false;
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