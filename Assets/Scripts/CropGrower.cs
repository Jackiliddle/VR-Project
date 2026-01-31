using UnityEngine;
using System.Collections;

public class CropGrower : MonoBehaviour
{
    [Header("Growth Settings")]
    [SerializeField] private float growDuration = 15f;

    private Vector3 targetScale;

    private void Awake()
    {
        targetScale = transform.localScale;
    }

    public static void StartGrowingCrops()
    {
        // Find all veggies, but only animate the ones that have CropGrower
        foreach (var crop in GameObject.FindGameObjectsWithTag("Veggie"))
        {
            if (crop == null) continue;

            var grower = crop.GetComponent<CropGrower>();
            if (grower == null) continue; // <-- only eggplant (or any non-root using this script)

            crop.transform.localScale = Vector3.zero;
            grower.StartCoroutine(grower.GrowCropCoroutine());
        }
    }

    private IEnumerator GrowCropCoroutine()
    {
        Vector3 startScale = Vector3.zero;
        float elapsed = 0f;

        while (elapsed < growDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / growDuration);

            // Smooth growth
            float eased = Mathf.SmoothStep(0f, 1f, t);

            transform.localScale = Vector3.Lerp(startScale, targetScale, eased);
            yield return null;
        }

        transform.localScale = targetScale;
    }
}
