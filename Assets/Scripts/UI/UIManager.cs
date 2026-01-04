using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject inventory;
    public GameObject pauseMenu;
    public GameObject statsPanel;
    public GameObject pauseMenuButton;
    public GameObject questPanel;

    public TMP_Text moneyCountText;
    public TMP_Text woodCountText;
    public TMP_Text meatCountText;

    public TMP_Text healthText;

    public TMP_Text maxHealthText;
    public TMP_Text levelText;
    public TMP_Text attackDamageText;
    public TMP_Text speedText;
    public Slider xpSlider;

    public TMP_Text requireMoneyCountText;
    public TMP_Text requireWoodCountText;
    public TMP_Text requireMeatCountText;
    public TMP_Text questText;


    public static UIManager instance { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void OpenOrCloseInventory()
    {
        inventory.SetActive(!inventory.activeSelf);
    }

    public void OpenOrCloseStatsPlayer()
    {
        if (statsPanel.activeSelf == true)
        {
            statsPanel.GetComponent<Animator>().Play("Close");
        }
        else
        {
            statsPanel.SetActive(true);
            statsPanel.GetComponent<Animator>().Play("Open");
        }
    }

    public void UpdateMoney(int value)
    {
        moneyCountText.text = value.ToString();
    }

    public void UpdateWood(int value)
    {
        woodCountText.text = value.ToString();
    }

    public void UpdateMeat(int value)
    {
        meatCountText.text = value.ToString();
    }

    public void UpdateHealth(int hpValue, int maxHealthValue)
    {
        healthText.text = hpValue.ToString();
        maxHealthText.text = maxHealthValue.ToString();
    }

    public void UpdatePlayerStats(int xpValue, int levelValue, float speedValue, int attackDamageValue)
    {
        xpSlider.value = xpValue;
        levelText.text = levelValue.ToString();
        speedText.text = speedValue.ToString();
        attackDamageText.text = attackDamageValue.ToString();
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        pauseMenuButton.SetActive(false);
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        pauseMenuButton.SetActive(true);
        Time.timeScale = 1;
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    public void ShowQuestPanel()
    {
        questPanel.SetActive(true);
    }

    public void HideQuestPanel()
    {
        questPanel.SetActive(false);
    }

    public void UpdateRequiredResourcesQuestAndName(int moneyAmount, int woodAmount, int meatAmount, string questName)
    {
        requireMoneyCountText.text = moneyAmount.ToString();
        requireWoodCountText.text = woodAmount.ToString();
        requireMeatCountText.text = meatAmount.ToString();
        questText.text = questName.ToString();
    }
}
