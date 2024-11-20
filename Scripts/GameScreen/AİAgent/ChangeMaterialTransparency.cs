using System.Collections;
using UnityEngine;

public class ChangeMaterialTransparency : MonoBehaviour
{
    public SkinnedMeshRenderer skinnedMeshRenderer;
    public float transitionDuration = 1.0f;
    private Material[] materials;

    private void Start()
    {
        materials = skinnedMeshRenderer.materials;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TransparencyObject")) // Deðiþtirmek istediðiniz tag ile eþleþtiðinden emin olun
        {
            StopAllCoroutines();
            StartCoroutine(FadeToTransparency(true));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("TransparencyObject")) // Deðiþtirmek istediðiniz tag ile eþleþtiðinden emin olun
        {
            StopAllCoroutines();
            StartCoroutine(FadeToTransparency(false));
        }
    }

    private IEnumerator FadeToTransparency(bool toTransparent)
    {
        float startAlpha = toTransparent ? 1.0f : 0.5f;
        float endAlpha = toTransparent ? 0.5f : 1.0f;

        for (float t = 0; t < transitionDuration; t += Time.deltaTime)
        {
            float normalizedTime = t / transitionDuration;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, normalizedTime);

            foreach (Material mat in materials)
            {
                Color color = mat.color;
                color.a = alpha;
                mat.color = color;
            }

            yield return null;
        }

        // Ensure the final alpha is set
        foreach (Material mat in materials)
        {
            Color color = mat.color;
            color.a = endAlpha;
            mat.color = color;
        }
    }
}
