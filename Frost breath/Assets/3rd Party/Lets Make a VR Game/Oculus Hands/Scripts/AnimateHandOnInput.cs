using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnimateHandOnInput : MonoBehaviour
{
    public InputActionProperty pinchAnimationAction;   // For pinching (trigger press)
    public InputActionProperty gripAnimationAction;    // For gripping (grip button press)
    public InputActionProperty triggerTouchedAction;   // For detecting trigger touch
    public Animator handAnimator;                     // Animator controlling the hand model

    private void OnEnable()
    {
        // Ensure all Input Actions are enabled
        EnableInputActions(pinchAnimationAction, gripAnimationAction, triggerTouchedAction);

        // Attach event callbacks for triggerTouched
        if (triggerTouchedAction.action != null)
        {
            triggerTouchedAction.action.performed += OnTriggerTouched;
            triggerTouchedAction.action.canceled += OnTriggerReleased;
        }
    }

    private void OnDisable()
    {
        // Remove event callbacks for triggerTouched
        if (triggerTouchedAction.action != null)
        {
            triggerTouchedAction.action.performed -= OnTriggerTouched;
            triggerTouchedAction.action.canceled -= OnTriggerReleased;
        }

        // Disable all Input Actions
        DisableInputActions(pinchAnimationAction, gripAnimationAction, triggerTouchedAction);
    }

    private void Update()
    {
        if (handAnimator == null)
        {
            Debug.LogWarning($"Animator not assigned on {gameObject.name}");
            return;
        }

        // Pinch (trigger press, null-safe)
        if (pinchAnimationAction.action != null)
        {
            float triggerValue = pinchAnimationAction.action.ReadValue<float>();
            handAnimator.SetFloat("Trigger", triggerValue);
        }

        // Grip (grip button press, null-safe)
        if (gripAnimationAction.action != null)
        {
            float gripValue = gripAnimationAction.action.ReadValue<float>();
            handAnimator.SetFloat("Grip", gripValue);
        }
    }

    private void EnableInputActions(params InputActionProperty[] actions)
    {
        foreach (var action in actions)
        {
            if (action.action != null && !action.action.enabled)
            {
                action.action.Enable();
            }
        }
    }

    private void DisableInputActions(params InputActionProperty[] actions)
    {
        foreach (var action in actions)
        {
            if (action.action != null && action.action.enabled)
            {
                action.action.Disable();
            }
        }
    }

    // Event: Trigger touched
    private void OnTriggerTouched(InputAction.CallbackContext context)
    {
        if (handAnimator == null)
        {
            Debug.LogWarning($"Animator not assigned on {gameObject.name}");
            return;
        }

        // Set "Point" parameter for the hand
        handAnimator.SetFloat("Point", 1.0f);
        //Debug.Log($"{gameObject.name}: Trigger Touched - Action: {context.action.name}");
    }

    // Event: Trigger released
    private void OnTriggerReleased(InputAction.CallbackContext context)
    {
        if (handAnimator == null)
        {
            Debug.LogWarning($"Animator not assigned on {gameObject.name}");
            return;
        }

        // Reset "Point" parameter for the hand
        handAnimator.SetFloat("Point", 0.0f);
        //Debug.Log($"{gameObject.name}: Trigger Released - Action: {context.action.name}");
    }
}
