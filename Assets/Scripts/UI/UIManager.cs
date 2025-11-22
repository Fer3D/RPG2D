using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject inventory;
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
}
