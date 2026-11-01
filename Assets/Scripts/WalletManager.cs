
using UnityEngine;

public class WalletManager : MonoBehaviour
{
    [SerializeField] private int startingBalance = 1000;

    public int Balance { get; private set; }

    private void Awake()
    {
        Balance = startingBalance;
    }

    public bool TrySpend(int amount)
    {
        if (amount <= 0 || Balance < amount)
        {
            return false;
        }

        Balance -= amount;
        return true;
    }

    public void AddWinnings(int amount)
    {
        if (amount <= 0)
            return;

        Balance += amount;
    }

    public void ResetBalance()
    {
        Balance = startingBalance;
    }
}
