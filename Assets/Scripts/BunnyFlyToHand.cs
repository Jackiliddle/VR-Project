using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable))]
public class BunnyFlyToHand : MonoBehaviour
{
    UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grab;

    void Awake()
    {
        grab = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        grab.selectEntered.AddListener(OnSelectEntered);
    }

    void OnDestroy()
    {
        grab.selectEntered.RemoveListener(OnSelectEntered);
    }

    void OnSelectEntered(SelectEnterEventArgs args)
    {
        // This is the controller object that did the grabbing
        Transform controller = args.interactorObject.transform;

        // Find HandAttach under that controller
        foreach (Transform t in controller.GetComponentsInChildren<Transform>(true))
        {
            if (t.name == "HandAttach")
            {
                grab.attachTransform = t;
                return;
            }
        }

        Debug.LogWarning("HandAttach not found on grabbing controller.");
    }
}
