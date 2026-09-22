using UnityEngine;
using UnityEngine.Events;

public class TemplateInteractable : MonoBehaviour
{
    [Header("Interaction Settings")]
    public bool triggerOnEnter = false;

    [Header("Interaction Events")]
    public UnityEvent onInteract;

    public void Activate()
    {
        onInteract.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (triggerOnEnter && collision.GetComponent<TemplatePlayerMovement>() != null)
        {
            Debug.Log("Object activated on enter!");
            onInteract.Invoke();
        }
    }
}