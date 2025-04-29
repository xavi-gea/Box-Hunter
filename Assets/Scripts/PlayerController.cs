using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Timeline.DirectorControlPlayable;

public class PlayerController : MonoBehaviour
{
    [Header("Interaction")]
    public GameObject interactor;
    private InputAction interactAction;

    [SerializeField]
    private LayerMask layermask;

    [Header("Movement")]
    public float moveSpeed = 1.5f;

    private Vector2 moveVector;
    private Rigidbody2D rb2D;

    private float xAxis = 0;
    private float yAxis = 0;

    private float lastXAxis = 0;
    private float lastYAxis = 0;

    private Animator animator;

    private InputAction moveAction;

    [Header("Pause")]
    private InputAction pauseAction;
    private InputAction resumeAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Player/Move");
        interactAction = InputSystem.actions.FindAction("Player/Interact");
        pauseAction = InputSystem.actions.FindAction("Player/Pause");
        resumeAction = InputSystem.actions.FindAction("UI/Resume");

        rb2D = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();

        InputManager.Instance.SetInputMap(ActionMap.Player);
    }

    // Update is called once per frame
    void Update()
    {
        moveVector = moveAction.ReadValue<Vector2>().normalized;
        HandleInteraction();
        HandlePause();
    }


    void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (!moveVector.Equals(Vector2.zero))
        {
            if (!moveVector.x.Equals(0) && !moveVector.y.Equals(0))
            {
                xAxis = moveVector.x;
                yAxis = moveVector.y;

                if (!xAxis.Equals(0))
                {
                    switch (xAxis)
                    {
                        case > 0: xAxis = 1; break;
                        case < 0: xAxis = -1; break;
                    }

                    moveVector.Set(xAxis, 0);
                }
                else if (!yAxis.Equals(0))
                {
                    switch (yAxis)
                    {
                        case > 0: yAxis = 1; break;
                        case < 0: yAxis = -1; break;
                    }

                    moveVector.Set(0, yAxis);
                }
            }

            lastXAxis = moveVector.x;
            lastYAxis = moveVector.y;

            AnimateMovement(true);

            rb2D.MovePosition(rb2D.position + (moveSpeed * Time.fixedDeltaTime * moveVector));

            interactor.transform.SetLocalPositionAndRotation(moveVector, Quaternion.identity);

            if (CombatEncounterManager.Instance.isInCombatZone)
            {
                CombatEncounterManager.Instance.IncreaseCombatChance();
            }
        
        }
        else
        {
            AnimateMovement(false);
        }
    }

    private void AnimateMovement(bool isMoving)
    {
        animator.SetBool("isMoving", isMoving);

        if (isMoving)
        {
            animator.SetFloat("xAxis", moveVector.x);
            animator.SetFloat("yAxis", moveVector.y);
        }
        else
        {
            animator.SetFloat("xAxis", lastXAxis);
            animator.SetFloat("yAxis", lastYAxis);
        }
    }
    
    private void HandleInteraction()
    {
        if (interactAction.IsPressed())
        {
            RaycastHit2D hit = Physics2D.Raycast(
                transform.position,
                interactor.transform.position - transform.position,
                Vector2.Distance(transform.position, interactor.transform.position),
                layermask
            );

            if (hit)
            {
                Debug.DrawLine(transform.position, hit.transform.position, Color.green, 5f);
                Debug.Log(hit.collider.name);

                if (hit.collider.gameObject.TryGetComponent(out IInteractable interactableObject))
                {
                    interactableObject.Interact();
                }
            }
        }
    }
    private void HandlePause()
    {
        if (!CombatEncounterManager.Instance.isInCombat)
        {
            if (!ScreenManager.Instance.isGamePaused && pauseAction.IsPressed())
            {
                ScreenManager.Instance.PauseGame();
            }
            else if(resumeAction.IsPressed())
            {
                ScreenManager.Instance.UnPauseGame();
            }
        }
    }
}
