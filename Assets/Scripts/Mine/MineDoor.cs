using UnityEngine;
using TMPro;

public class MineDoor : MonoBehaviour
{
    public Transform transformToTransport;
    public TMP_Text interactionText;
    private bool isPlayerNear = false;
    private Transform playerTransform;
    private Vector3 originalTextPosition;

    private void Awake()
    {
        if (interactionText != null)
        {
            originalTextPosition = interactionText.transform.localPosition;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNear = true;
            playerTransform = collision.transform;
            if (interactionText != null) interactionText.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNear = false;
            playerTransform = null;
            if (interactionText != null)
            {
                interactionText.gameObject.SetActive(false);
                interactionText.transform.localPosition = originalTextPosition;
            }
        }
    }

    private void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E) && playerTransform != null)
        {
            playerTransform.position = transformToTransport.position;
        }

        if (isPlayerNear && interactionText != null)
        {
            float offset = Mathf.Sin(Time.time * 2f) * 0.2f;
            interactionText.transform.localPosition = new Vector3(originalTextPosition.x, originalTextPosition.y + offset, originalTextPosition.z);
        }
    }
}
