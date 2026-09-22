using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class TemplatePlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private Vector2 moveDirection;
    private Vector2 lastFacingDirection = Vector2.down;

    private TemplateInteractable currentInteractable = null;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
    }

    void OnMove(InputValue value)
    {
        moveDirection = value.Get<Vector2>();

        if (moveDirection.x != 0 || moveDirection.y != 0)
        {
            lastFacingDirection = moveDirection;
        }
    }

    void OnInteract(InputValue value)
        {
            Debug.Log("Interact button pressed");
            
            if (value.isPressed && currentInteractable != null)
            {
                currentInteractable.Activate();
            }
        }

    void Update()
    {
        if (spriteRenderer != null)
        {
            if (moveDirection.x < 0)
            {
                spriteRenderer.transform.localScale = new Vector3(-1, 1, 1);
            }
            else if (moveDirection.x > 0)
            {
                spriteRenderer.transform.localScale = new Vector3(1, 1, 1);
            }
        }

        if (animator != null)
        {
            animator.SetFloat("Speed", moveDirection.sqrMagnitude);

               
            //animator.SetFloat("MoveX", moveDirection.x);
            //animator.SetFloat("MoveY", moveDirection.y);
            //animator.SetFloat("LastMoveX", lastFacingDirection.x);
            //animator.SetFloat("LastMoveY", lastFacingDirection.y);
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = moveDirection * moveSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        TemplateInteractable interactable = collision.GetComponent<TemplateInteractable>();
        if (interactable != null)
        {
            currentInteractable = interactable;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        TemplateInteractable interactable = collision.GetComponent<TemplateInteractable>();
        if (interactable != null && currentInteractable == interactable)
        {
            currentInteractable = null;
        }
    }
}