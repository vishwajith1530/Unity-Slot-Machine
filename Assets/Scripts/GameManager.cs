
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

    [Header("Audio Reference")]
    [SerializeField] private AudioManager audioManager;

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

        // Welcome sound when the game starts.
        if (audioManager != null)
        {
            audioManager.PlayStart();
        }
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

    private bool IsGameSetupValid()
    {
        if (reels == null || reels.Length != 3)
            return false;

        if (symbolSprites == null || symbolSprites.Length != 4)
            return false;

        if (betOptions == null || betOptions.Length == 0)
            return false;

        foreach (ReelController reel in reels)
        {
            if (reel == null)
                return false;
        }

        foreach (Sprite sprite in symbolSprites)
        {
            if (sprite == null)
                return false;
        }

        return true;
    }

    public void SetBet(int betIndex)
    {
        if (isSpinning) return;

        if (betOptions == null || betOptions.Length == 0) return;

        if (betIndex < 0 || betIndex >= betOptions.Length) return;

        currentBetIndex = betIndex;

        UpdateUI("BET SELECTED!");
    }

    public void ChangeBet(int direction)
    {
        if (isSpinning) return;

        if (betOptions == null || betOptions.Length == 0) return;

        currentBetIndex += direction;

        currentBetIndex = Mathf.Clamp(
            currentBetIndex,
            0,
            betOptions.Length - 1
        );

        UpdateUI("BET: " + CurrentBet);
    }

    public void Spin()
    {
        if (isSpinning) return;

        if (!IsGameSetupValid())
        {
            UpdateUI("GAME SETUP ERROR!");
            Debug.LogError("Check your reel and symbol assignments.");

            if (audioManager != null)
                audioManager.PlayWarning();

            return;
        }

        if (balance < CurrentBet)
        {
            UpdateUI("INSUFFICIENT BALANCE!");

            if (audioManager != null)
                audioManager.PlayWarning();

            return;
        }

        // Deduct the bet.
        balance -= CurrentBet;

        isSpinning = true;

        // Start continuous reel spinning sound.
        if (audioManager != null)
        {
            audioManager.StartSpin();
        }

        spinCoroutine = StartCoroutine(SpinSequence());
    }

    private IEnumerator SpinSequence()
    {
        UpdateUI("GOOD LUCK!");

        int[] results = new int[3];

        // Generate final symbols for all three reels.
        for (int i = 0; i < results.Length; i++)
        {
            results[i] = Random.Range(0, symbolSprites.Length);
        }

        // Spin reels one after another.
        for (int i = 0; i < reels.Length; i++)
        {
            if (reels[i] == null)
            {
                Debug.LogError("A ReelController is missing.");

                StopSpinAudio();

                isSpinning = false;
                spinCoroutine = null;

                UpdateUI("REEL SETUP ERROR!");

                if (audioManager != null)
                    audioManager.PlayWarning();

                yield break;
            }

            yield return StartCoroutine(
                reels[i].SpinTo(
                    results[i],
                    symbolSprites,
                    spinDuration
                )
            );

            // Sound when each reel stops.
            if (audioManager != null)
            {
                audioManager.PlayReelStop();
            }

            // Delay before the next reel stops.
            if (i < reels.Length - 1)
            {
                yield return new WaitForSeconds(reelStopDelay);
            }
        }

        // Check whether all three symbols match.
        if (results[0] == results[1] &&
            results[1] == results[2])
        {
            int multiplier = GetPayoutMultiplier(results[0]);

            int winnings = CurrentBet * multiplier;

            balance += winnings;

            UpdateUI("JACKPOT! YOU WON " + winnings + "!");

            if (audioManager != null)
            {
                audioManager.PlayWin();
            }
        }
        else
        {
            UpdateUI("BETTER LUCK NEXT TIME!");

            if (audioManager != null)
            {
                audioManager.PlayLose();
            }
        }

        StopSpinAudio();

        isSpinning = false;
        spinCoroutine = null;
    }

    private int GetPayoutMultiplier(int symbolIndex)
    {
        switch (symbolIndex)
        {
            case 0:
                return 10; // Seven

            case 1:
                return 5;  // Cherry

            case 2:
                return 4;  // Bell

            case 3:
                return 3;  // BAR

            default:
                return 0;
        }
    }

    public void ResetGame()
    {
        // Stop any ongoing spin.
        if (spinCoroutine != null)
        {
            StopCoroutine(spinCoroutine);
            spinCoroutine = null;
        }

        StopSpinAudio();

        isSpinning = false;

        // Restore starting balance and default bet.
        balance = startingBalance;
        currentBetIndex = 0;

        UpdateUI("GAME RESET! GOOD LUCK!");
    }

    private void StopSpinAudio()
    {
        if (audioManager != null)
        {
            audioManager.StopSpin();
        }
    }

    private void UpdateUI(string message)
    {
        if (uiManager == null)
            return;

        if (betOptions == null || betOptions.Length == 0)
            return;

        currentBetIndex = Mathf.Clamp(
            currentBetIndex,
            0,
            betOptions.Length - 1
        );

        uiManager.UpdateDisplay(
            balance,
            CurrentBet,
            message
        );
    }
}
