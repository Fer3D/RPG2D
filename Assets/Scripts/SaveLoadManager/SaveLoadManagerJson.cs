using System.IO;
using UnityEngine;

public class SaveLoadManagerJson : MonoBehaviour
{
    private string filePath;
    void Start()
    {
        filePath = Application.persistentDataPath + "/savefile.json";
    }


    public void SaveGame(float speed, int xp, int currentLevel, int attackDamage, string selectedSkin, int health, int maxHealth, Vector2 position, int money, int meat, int wood)
        {
            SaveData data = new SaveData();
            data.speed = speed;
            data.xp = xp;
            data.currentLevel = currentLevel;
            data.attackDamage = attackDamage;
            data.selectedSkin = selectedSkin;
            data.health = health;
            data.maxHealth = maxHealth;
            data.position = position;
            data.money = money;
            data.meat = meat;
            data.wood = wood;

            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(filePath, json);
            Debug.Log("Game Saved to " + filePath);
        }

    public SaveData LoadGame()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            Debug.Log("Game Loaded from " + filePath);
            return data;
        }
        else
        {
            Debug.LogWarning("Save file not found at " + filePath);
            return null;
        }
    }
}