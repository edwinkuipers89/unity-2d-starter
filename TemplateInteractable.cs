using UnityEngine;
using UnityEngine.Events;

public class TemplateInteractable : MonoBehaviour
{
    [Header("Interaction Settings")]
    public bool triggerOnEnter = false;

    [Header("Interaction Events")]
    public UnityEvent onInteract;
    public UnityEvent onExit;

    public void Activate()
    {
        onInteract.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (triggerOnEnter && collision.GetComponent<TemplatePlayerMovement>() != null)
        {
            onInteract.Invoke();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<TemplatePlayerMovement>() != null)
        {
            onExit.Invoke();
        }
    }
}