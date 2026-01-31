using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable))]
public class RabbitGrabPause : MonoBehaviour
{
    [Header("Components to pause while held")]
    [SerializeField] private MonoBehaviour rabbitMover;   // Drag your RabbitMover here
    [SerializeField] private MonoBehaviour pestSpawner;   // Optional: drag PestSpawner if you want it paused too

    [Header("Physics behavior while held")]
    [SerializeField] private bool setKinematicWhileHeld = true;
    [SerializeField] private bool disableGravityWhileHeld = true;
    [SerializeField] private bool freezeRotationWhileHeld = false;

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grab;
    private Rigidbody rb;

    private bool originalKinematic;
    private bool originalUseGravity;
    private RigidbodyConstraints originalConstraints;

    private void Awake()
    {
        grab = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        grab.selectEntered.AddListener(OnGrabbed);
        grab.selectExited.AddListener(OnReleased);
    }

    private void OnDisable()
    {
        grab.selectEntered.RemoveListener(OnGrabbed);
        grab.selectExited.RemoveListener(OnReleased);
    }

    private void OnGrabbed(SelectEnterEventArgs args)
    {
        // Stop AI movement
        if (rabbitMover != null) rabbitMover.enabled = false;
        if (pestSpawner != null) pestSpawner.enabled = false;

        if (rb == null) return;

        // Save physics state
        originalKinematic = rb.isKinematic;
        originalUseGravity = rb.useGravity;
        originalConstraints = rb.constraints;

        // Stop current motion so it doesn't keep "running"
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        if (disableGravityWhileHeld)
            rb.useGravity = false;

        // Make VR grabbing stable (optional)
        if (setKinematicWhileHeld)
            rb.isKinematic = true;

        if (freezeRotationWhileHeld)
            rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void OnReleased(SelectExitEventArgs args)
    {
        // Resume AI movement
        if (rabbitMover != null) rabbitMover.enabled = true;
        if (pestSpawner != null) pestSpawner.enabled = true;

        if (rb == null) return;

        // Restore physics state
        rb.isKinematic = originalKinematic;
        rb.useGravity = originalUseGravity;
        rb.constraints = originalConstraints;
    }
}
