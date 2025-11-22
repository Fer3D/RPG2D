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
            Debug.Log("Tenemos: " + money + " de dinero.");
        }
        else if (collision.gameObject.CompareTag("Meat"))
        {
            Destroy(collision.gameObject);
            meat ++;
            Debug.Log("Tenemos: " + meat + " de carne.");
        }
        else if (collision.gameObject.CompareTag("Wood"))
        {
            Destroy(collision.gameObject);
            wood ++;
            Debug.Log("Tenemos: " + wood + " de madera.");
        }
    }
}
