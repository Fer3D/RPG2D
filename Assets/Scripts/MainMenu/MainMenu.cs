using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class MainMenu : MonoBehaviour
{
    public GameObject optionsPanel;
    public GameObject buttonsPanel;

    public TMP_Dropdown qualityDropdown;

    public Button loadGameButton;

    public GameObject mainPlayerSkin;
    public Sprite[] spritesSkins;
    public string[] skinNames = { "Blue", "Purple", "Red", "Yellow" };

    private void Start()
    {
        PlayerPrefs.SetString("MainPlayerSkin", skinNames[0]);

        int savedQuality = PlayerPrefs.GetInt("QualityLevel", -1);

        if (savedQuality == -1)
        {
            savedQuality = QualitySettings.GetQualityLevel();
        }

        qualityDropdown.ClearOptions();
        qualityDropdown.AddOptions(new System.Collections.Generic.List<string>(QualitySettings.names));

        qualityDropdown.value = savedQuality;
        qualityDropdown.onValueChanged.AddListener(OnQualityChanged);

        // Check if save file exists and update load button
        string filePath = Application.persistentDataPath + "/savefile.json";
        TMP_Text loadButtonText = loadGameButton.GetComponentInChildren<TMP_Text>();
        if (File.Exists(filePath))
        {
            loadGameButton.interactable = true;
            var colors = loadGameButton.colors;
            colors.normalColor = Color.white; // or default color
            loadGameButton.colors = colors;
            loadButtonText.color = Color.black;
        }
        else
        {
            loadGameButton.interactable = false;
            var colors = loadGameButton.colors;
            colors.normalColor = Color.gray;
            loadGameButton.colors = colors;
            loadButtonText.color = Color.gray;
        }
    }

    public void PlayGame()
    {
        PlayerPrefs.SetInt("LoadOnStart", 0);
        SceneManager.LoadScene("Level1");
    }

    public void LoadGame()
    {
        string filePath = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(filePath))
        {
            PlayerPrefs.SetInt("LoadOnStart", 1);
            SceneManager.LoadScene("Level1");
        }
        // Else do nothing
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OpenOptions()
    {
        buttonsPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }

    public void CloseOptions()
    {
        buttonsPanel.SetActive(true);
        optionsPanel.SetActive(false);
    }

    public void OnQualityChanged(int index)
    {
        QualitySettings.SetQualityLevel(index, true);
        PlayerPrefs.SetInt("QualityLevel", index);
    }

    public void SelectSkin(int index)
    {
        mainPlayerSkin.GetComponent<Image>().sprite = spritesSkins[index];
        PlayerPrefs.SetString("MainPlayerSkin", skinNames[index]);
    }
}
