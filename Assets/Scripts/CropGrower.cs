// using UnityEngine;
// using System.Collections;

// public class CropGrower : MonoBehaviour
// {
//     [Header("Growth Settings")]
//     private float finalScale = 1f;
//     private const float GrowDuration = 15f;

//     public static void StartGrowingCrops()
//     {
//         foreach (var crop in GameObject.FindGameObjectsWithTag("Veggie"))
//         {
//             if (crop == null) continue;

//             var grower = crop.GetComponent<CropGrower>();
//             if (grower != null)
//             {
//                 crop.transform.localScale = Vector3.zero;
//                 grower.StartCoroutine(grower.GrowCropCoroutine());
//             }
//         }
//     }

//     private IEnumerator GrowCropCoroutine()
//     {
//         Vector3 startScale = Vector3.zero;
//         Vector3 targetScale = Vector3.one * finalScale;

//         float elapsed = 0f;

//         while (elapsed < GrowDuration)
//         {
//             elapsed += Time.deltaTime;
//             transform.localScale = Vector3.Lerp(startScale, targetScale, elapsed / GrowDuration);
//             yield return null;
//         }

//         transform.localScale = targetScale;
//     }
// }

using UnityEngine;
using System.Collections;

public class CropGrower : MonoBehaviour
{
    [Header("Growth Settings")]
    [SerializeField] private float growDuration = 15f;

    // This will store whatever scale you set on the prefab / instance in the editor
    private Vector3 targetScale;

    private void Awake()
    {
        // Remember the "intended" size (set in prefab or scene)
        targetScale = transform.localScale;
    }

    public static void StartGrowingCrops()
    {
        foreach (var crop in GameObject.FindGameObjectsWithTag("Veggie"))
        {
            if (crop == null) continue;

            var grower = crop.GetComponent<CropGrower>();
            if (grower != null)
            {
                // Make invisible first
                crop.transform.localScale = Vector3.zero;

                // Grow to the stored target scale
                grower.StartCoroutine(grower.GrowCropCoroutine());
            }
        }
    }

    private IEnumerator GrowCropCoroutine()
    {
        Vector3 startScale = Vector3.zero;
        float elapsed = 0f;

        while (elapsed < growDuration)
        {
            elapsed += Time.deltaTime;
            transform.localScale = Vector3.Lerp(startScale, targetScale, elapsed / growDuration);
            yield return null;
        }

        transform.localScale = targetScale;
    }
}
