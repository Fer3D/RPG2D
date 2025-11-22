using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject inventory;

    public TMP_Text moneyCountText;
    public TMP_Text woodCountText;
    public TMP_Text meatCountText;
    public TMP_Text healthText;

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

    public void UpdateHealth(int hpValue)
    {
        healthText.text = hpValue.ToString();
    }
}
