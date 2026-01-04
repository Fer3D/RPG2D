using UnityEngine;

public class PlayerResourceCollector : MonoBehaviour
{

    private int money = 0;
    private int meat = 0;
    private int wood = 0;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("MoneyBag"))
        {
            Destroy(collision.gameObject);
            money ++;
            UIManager.instance.UpdateMoney(money);
        }
        else if (collision.gameObject.CompareTag("Meat"))
        {
            Destroy(collision.gameObject);
            meat ++;
            UIManager.instance.UpdateMeat(meat);
        }
        else if (collision.gameObject.CompareTag("Wood"))
        {
            Destroy(collision.gameObject);
            wood ++;
            UIManager.instance.UpdateWood(wood);
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
