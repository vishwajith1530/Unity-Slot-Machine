
using System.Collections;
using UnityEngine;

public class LeverAnimation : MonoBehaviour
{
    [SerializeField] private RectTransform lever;
    [SerializeField] private float pullAngle = 35f;
    [SerializeField] private float animationDuration = 0.2f;

    public void PlayLeverAnimation()
    {
        StopAllCoroutines();
        StartCoroutine(PullLever());
    }

    private IEnumerator PullLever()
    {
        Quaternion originalRotation = lever.localRotation;

        Quaternion pulledRotation =
            originalRotation *
            Quaternion.Euler(0f, 0f, pullAngle);

        float elapsed = 0f;

        while (elapsed < animationDuration)
        {
            elapsed += Time.deltaTime;

            float t = Mathf.Clamp01(
                elapsed / animationDuration
            );

            lever.localRotation = Quaternion.Slerp(
                originalRotation,
                pulledRotation,
                t
            );

            yield return null;
        }

        yield return new WaitForSeconds(0.1f);

        elapsed = 0f;

        while (elapsed < animationDuration)
        {
            elapsed += Time.deltaTime;

            float t = Mathf.Clamp01(
                elapsed / animationDuration
            );

            lever.localRotation = Quaternion.Slerp(
                pulledRotation,
                originalRotation,
                t
            );

            yield return null;
        }

        lever.localRotation = originalRotation;
    }
}
