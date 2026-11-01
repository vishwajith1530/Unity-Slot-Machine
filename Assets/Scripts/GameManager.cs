
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Game Settings")]
    [SerializeField] private int startingBalance = 1000;
    [SerializeField] private int[] betOptions = { 10, 50, 100 };

    [Header("Reel References")]
    [SerializeField] private ReelController[] reels;

    [Header("Symbol Sprites")]
    [Tooltip("Order: Seven, Cherry, Bell, BAR")]
    [SerializeField] private Sprite[] symbolSprites;

    [Header("Animation Settings")]
    [SerializeField] private float spinDuration = 2.5f;
    [SerializeField] private float reelStopDelay = 0.4f;

    [Header("UI Reference")]
    [SerializeField] private UIManager uiManager;

    private int balance;
    private int currentBetIndex = 0;
    private bool isSpinning;

    private Coroutine spinCoroutine;

    public int Balance => balance;

    public int CurrentBet => betOptions[currentBetIndex];

    private void Start()
    {
        balance = startingBalance;

        ValidateSetup();

        UpdateUI("PLACE YOUR BET AND SPIN!");
    }

    private void ValidateSetup()
    {
        if (reels == null || reels.Length != 3)
        {
            Debug.LogError("GameManager requires exactly 3 reels.");
        }

        if (symbolSprites == null || symbolSprites.Length != 4)
        {
            Debug.LogError("Assign exactly 4 symbol sprites.");
        }

        if (betOptions == null || betOptions.Length == 0)
        {
            Debug.LogError("Bet options are missing.");
        }
    }

    // Select bet using index:
    // 0 = 10, 1 = 50, 2 = 100

    public void SetBet(int betIndex)
    {
        if (isSpinning)
            return;

        if (betIndex < 0 || betIndex >= betOptions.Length)
            return;

        currentBetIndex = betIndex;

        UpdateUI("BET SELECTED!");
    }

    public void ChangeBet(int direction)
    {
        if (isSpinning)
            return;

        currentBetIndex += direction;

        currentBetIndex = Mathf.Clamp(
            currentBetIndex,
            0,
            betOptions.Length - 1
        );

        UpdateUI("BET: " + CurrentBet);
    }

    // Called by Spin button and Lever button.

    public void Spin()
    {
        if (isSpinning)
            return;

        if (reels == null || reels.Length != 3 ||
            symbolSprites == null || symbolSprites.Length != 4)
        {
            UpdateUI("GAME SETUP ERROR!");
            Debug.LogError("Check your reel and symbol assignments.");
            return;
        }

        if (balance < CurrentBet)
        {
            UpdateUI("INSUFFICIENT BALANCE!");
            return;
        }

        balance -= CurrentBet;

        isSpinning = true;

        spinCoroutine = StartCoroutine(SpinSequence());
    }

    private IEnumerator SpinSequence()
    {
        UpdateUI("GOOD LUCK!");

        // Generate three independent random outcomes.
        int[] results = new int[3];

        for (int i = 0; i < results.Length; i++)
        {
            results[i] = Random.Range(0, symbolSprites.Length);
        }

        // Spin and stop reels one after another.
        for (int i = 0; i < reels.Length; i++)
        {
            if (reels[i] == null)
            {
                Debug.LogError("A ReelController is missing.");
                isSpinning = false;
                UpdateUI("REEL SETUP ERROR!");
                yield break;
            }

            yield return StartCoroutine(
                reels[i].SpinTo(
                    results[i],
                    symbolSprites,
                    spinDuration
                )
            );

            if (i < reels.Length - 1)
            {
                yield return new WaitForSeconds(reelStopDelay);
            }
        }

        // All three center symbols must match.
        if (results[0] == results[1] &&
            results[1] == results[2])
        {
            int multiplier = GetPayoutMultiplier(results[0]);

            int winnings = CurrentBet * multiplier;

            balance += winnings;

            UpdateUI("JACKPOT! YOU WON " + winnings + "!");
        }
        else
        {
            UpdateUI("BETTER LUCK NEXT TIME!");
        }

        isSpinning = false;
        spinCoroutine = null;
    }

    // Payout multipliers:
    // Seven = 10x
    // Cherry = 5x
    // Bell = 4x
    // BAR = 3x

    private int GetPayoutMultiplier(int symbolIndex)
    {
        switch (symbolIndex)
        {
            case 0:
                return 10;

            case 1:
                return 5;

            case 2:
                return 4;

            case 3:
                return 3;

            default:
                return 0;
        }
    }

    public void ResetGame()
    {
        if (spinCoroutine != null)
        {
            StopCoroutine(spinCoroutine);
            spinCoroutine = null;
        }

        isSpinning = false;

        balance = startingBalance;
        currentBetIndex = 0;

        UpdateUI("GAME RESET! GOOD LUCK!");
    }

    private void UpdateUI(string message)
    {
        if (uiManager != null)
        {
            uiManager.UpdateDisplay(
                balance,
                CurrentBet,
                message
            );
        }
    }
}
