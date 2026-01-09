using UnityEngine;
using System.Collections.Generic;

public class PlayerResourceCollector : MonoBehaviour
{

    private int money = 0;
    private int meat = 0;
    private int wood = 0;

    private HashSet<int> collectedObjects = new HashSet<int>();

    public AudioSource audioTakeItem;

    void OnTriggerEnter2D(Collider2D collision)
    {
        int objectId = collision.gameObject.GetInstanceID();
        if (collectedObjects.Contains(objectId)) return;

        if (collision.gameObject.CompareTag("MoneyBag"))
        {
            collectedObjects.Add(objectId);
            Destroy(collision.gameObject);
            money++;
            UIManager.instance.UpdateMoney(money);
            audioTakeItem.Play();
        }
        else if (collision.gameObject.CompareTag("Meat"))
        {
            collectedObjects.Add(objectId);
            Destroy(collision.gameObject);
            meat++;
            UIManager.instance.UpdateMeat(meat);
            audioTakeItem.Play();
        }
        else if (collision.gameObject.CompareTag("Wood"))
        {
            collectedObjects.Add(objectId);
            Destroy(collision.gameObject);
            wood++;
            UIManager.instance.UpdateWood(wood);
            audioTakeItem.Play();
        }
    }

    public void UpdateAllResources()
    {
        UIManager.instance.UpdateMoney(money);
        UIManager.instance.UpdateMeat(meat);
        UIManager.instance.UpdateWood(wood);
    }

    public int GetMoney() => money;
    public int GetMeat() => meat;
    public int GetWood() => wood;

    public void SetMoney(int amount) => money = amount;
    public void SetMeat(int amount) => meat = amount;
    public void SetWood(int amount) => wood = amount;
}
