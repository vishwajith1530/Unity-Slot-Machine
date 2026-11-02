
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Display References")]
    [SerializeField] private TMP_Text balanceText;
    [SerializeField] private TMP_Text betText;
    [SerializeField] private TMP_Text resultText;

    [Header("Game Reference")]
    [SerializeField] private GameManager gameManager;

    [Header("Audio Reference")]
    [SerializeField] private AudioManager audioManager;

    public void UpdateDisplay(int balance, int bet, string message)
    {
        if (balanceText != null)
            balanceText.text = "BALANCE: " + balance;

        if (betText != null)
            betText.text = "BET: " + bet;

        if (resultText != null)
            resultText.text = message;
    }

    public void SelectBet10()
    {
        if (audioManager != null)
            audioManager.PlaySelect();

        if (gameManager != null)
            gameManager.SetBet(0);
    }

    public void SelectBet50()
    {
        if (audioManager != null)
            audioManager.PlaySelect();

        if (gameManager != null)
            gameManager.SetBet(1);
    }

    public void SelectBet100()
    {
        if (audioManager != null)
            audioManager.PlaySelect();

        if (gameManager != null)
            gameManager.SetBet(2);
    }

    public void SelectNextBet()
    {
        if (audioManager != null)
            audioManager.PlayClick();

        if (gameManager != null)
            gameManager.ChangeBet(1);
    }

    public void SelectPreviousBet()
    {
        if (audioManager != null)
            audioManager.PlayClick();

        if (gameManager != null)
            gameManager.ChangeBet(-1);
    }

    public void OnSpinButton()
    {
        if (audioManager != null)
            audioManager.PlayLever();

        if (gameManager != null)
            gameManager.Spin();
    }

    public void OnLeverClicked()
    {
        if (audioManager != null)
            audioManager.PlayLever();

        if (gameManager != null)
            gameManager.Spin();
    }

    public void OnResetButton()
    {
        if (audioManager != null)
            audioManager.PlayReset();

        if (gameManager != null)
            gameManager.ResetGame();
    }
}
