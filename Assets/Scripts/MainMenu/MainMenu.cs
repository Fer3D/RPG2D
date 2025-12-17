using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject optionsPanel;
    public GameObject buttonsPanel;

    public TMP_Dropdown qualityDropdown;

    private void Start()
    {
        int savedQuality = PlayerPrefs.GetInt("QualityLevel", -1);

        if (savedQuality == -1)
        {
            savedQuality = QualitySettings.GetQualityLevel();
        }

        qualityDropdown.ClearOptions();
        qualityDropdown.AddOptions(new System.Collections.Generic.List<string>(QualitySettings.names));

        qualityDropdown.value = savedQuality;
        qualityDropdown.onValueChanged.AddListener(OnQualityChanged);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Level1");
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
}
