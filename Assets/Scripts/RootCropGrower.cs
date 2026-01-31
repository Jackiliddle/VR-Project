using UnityEngine;
using System.Collections;

public class RootCropGrower : MonoBehaviour
{
    [Header("Timing")]
    [SerializeField] private float baseGrowDuration = 15f;
    [SerializeField] private float staggerStep = 0.35f;
    [SerializeField] private float staggerJitter = 0.25f;

    [Tooltip("Each veggie gets a different duration: duration *= Random.Range(min,max)")]
    [SerializeField] private Vector2 durationMultiplierRange = new Vector2(0.85f, 1.25f);

    [Header("Start State")]
    [Tooltip("Start below final position (e.g. -0.5)")]
    [SerializeField] private float startYOffset = -0.05f;

    [Tooltip("Start scale relative to final scale (0 = invisible, 0.1 = tiny)")]
    [SerializeField] private float startScaleMultiplier = 0.1f;

    [Tooltip("If true, object is placed underground & small as soon as it spawns/enables.")]
    [SerializeField] private bool initializeToStartStateOnEnable = true;

    private Vector3 targetScale;
    private Vector3 targetPos;
    private bool targetsCaptured;

    private void OnEnable()
    {
        if (!targetsCaptured)
            CaptureTargetsFromScene();

        if (initializeToStartStateOnEnable)
            ApplyStartState();
    }

    private void CaptureTargetsFromScene()
    {
        // THIS is the “correct” scene size/position you want to grow to.
        targetScale = transform.localScale;
        targetPos   = transform.position;
        targetsCaptured = true;
    }

    private void ApplyStartState()
    {
        transform.position = new Vector3(targetPos.x, targetPos.y + startYOffset, targetPos.z);
        transform.localScale = targetScale * startScaleMultiplier;
    }

    public static void StartGrowingCrops()
    {
        var veggies = GameObject.FindGameObjectsWithTag("Veggie");
        if (veggies == null || veggies.Length == 0) return;

        System.Array.Sort(veggies, (a, b) => a.name.CompareTo(b.name));

        int rootIndex = 0;

        foreach (var crop in veggies)
        {
            if (crop == null) continue;

            var grower = crop.GetComponent<RootCropGrower>();
            if (grower == null) continue; // only root veggies

            // Ensure targets are captured (scene size), but DO NOT shrink here.
            if (!grower.targetsCaptured)
                grower.CaptureTargetsFromScene();

            float delay = (rootIndex * grower.staggerStep) + Random.Range(0f, grower.staggerJitter);

            float duration = grower.baseGrowDuration * Random.Range(
                grower.durationMultiplierRange.x,
                grower.durationMultiplierRange.y
            );

            rootIndex++;
            grower.StartCoroutine(grower.GrowRoutine(delay, duration));
        }
    }

    private IEnumerator GrowRoutine(float delay, float duration)
    {
        if (delay > 0f) yield return new WaitForSeconds(delay);

        // Start from wherever it currently is (should already be underground & small)
        Vector3 startPos   = transform.position;
        Vector3 startScale = transform.localScale;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float eased = Mathf.SmoothStep(0f, 1f, t);

            transform.position   = Vector3.Lerp(startPos, targetPos, eased);
            transform.localScale = Vector3.Lerp(startScale, targetScale, eased);

            yield return null;
        }

        transform.position = targetPos;
        transform.localScale = targetScale;
    }
}
