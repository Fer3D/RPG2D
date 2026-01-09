using UnityEngine;

public class TreeRespawn : MonoBehaviour
{
    public GameObject normalTreePrefab;
    public float respawnTime = 60f;

    void Start()
    {
        Invoke("Respawn", respawnTime);
    }

    void Respawn()
    {
        Instantiate(normalTreePrefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}