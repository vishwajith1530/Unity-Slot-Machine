
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ReelController : MonoBehaviour
{
    [Header("Symbol Images")]
    [Tooltip("Assign Top, Middle, Bottom in this order.")]
    [SerializeField] private Image[] symbolImages;

    [Header("Animation")]
    [SerializeField] private float symbolChangeInterval = 0.06f;

    private void Start()
    {
        // Show initial random symbols.
        ShowRandomSymbols();
    }

    public IEnumerator SpinTo(
        int finalSymbolIndex,
        Sprite[] availableSymbols,
        float duration)
    {
        if (symbolImages == null ||
            symbolImages.Length != 3 ||
            availableSymbols == null ||
            availableSymbols.Length == 0)
        {
            Debug.LogError("ReelController is not configured correctly.");
            yield break;
        }

        float elapsed = 0f;

        // Rapidly cycle through random symbols.
        while (elapsed < duration)
        {
            for (int i = 0; i < symbolImages.Length; i++)
            {
                symbolImages[i].sprite =
                    availableSymbols[
                        Random.Range(0, availableSymbols.Length)
                    ];
            }

            yield return new WaitForSeconds(symbolChangeInterval);

            elapsed += symbolChangeInterval;
        }

        // Keep the final winning result in the CENTER position.
        symbolImages[1].sprite =
            availableSymbols[finalSymbolIndex];

        // Randomize the symbols above and below the result.
        symbolImages[0].sprite =
            availableSymbols[
                Random.Range(0, availableSymbols.Length)
            ];

        symbolImages[2].sprite =
            availableSymbols[
                Random.Range(0, availableSymbols.Length)
            ];
    }

    public void ShowRandomSymbols()
    {
        if (symbolImages == null ||
            symbolImages.Length != 3)
            return;

        for (int i = 0; i < symbolImages.Length; i++)
        {
            if (symbolImages[i] != null)
            {
                // Initial symbols are assigned by GameManager
                // when the game is played.
            }
        }
    }
}
