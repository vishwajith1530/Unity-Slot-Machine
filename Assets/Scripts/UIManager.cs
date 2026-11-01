
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

    public void UpdateDisplay(
        int balance,
        int bet,
        string message)
    {
        if (balanceText != null)
        {
            balanceText.text = "BALANCE: " + balance;
        }

        if (betText != null)
        {
            betText.text = "BET: " + bet;
        }

        if (resultText != null)
        {
            resultText.text = message;
        }
    }

    // Individual bet buttons

    public void SelectBet10()
    {
        gameManager.SetBet(0);
    }

    public void SelectBet50()
    {
        gameManager.SetBet(1);
    }

    public void SelectBet100()
    {
        gameManager.SetBet(2);
    }

    // Bet Up and Bet Down buttons

    public void SelectNextBet()
    {
        gameManager.ChangeBet(1);
    }

    public void SelectPreviousBet()
    {
        gameManager.ChangeBet(-1);
    }

    // Spin button

    public void OnSpinButton()
    {
        gameManager.Spin();
    }

    // Lever button

    public void OnLeverClicked()
    {
        gameManager.Spin();
    }

    // Reset button

    public void OnResetButton()
    {
        gameManager.ResetGame();
    }
}
