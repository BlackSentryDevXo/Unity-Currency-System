using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Define currencies
public enum CurrencyType
{
    CurrencyA,
    CurrencyB,
    CurrencyC,
}

public class CurrencySystem : MonoBehaviour
{
    public static CurrencySystem instance;

    // Dictionary to store currency amounts
    private Dictionary<CurrencyType, int> currencyAmounts = new Dictionary<CurrencyType, int>();
    // Events to notify listeners when currency changes
    public event Action<CurrencyType, int> OnCurrencyChanged;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        InitializeCurrencyAmounts();
        LoadCurrency();
        GiveInitialCurrency();
        UpdateUI();
    }

    private void InitializeCurrencyAmounts()
    {
        foreach (CurrencyType currencyType in Enum.GetValues(typeof(CurrencyType)))
        {
            if (!currencyAmounts.ContainsKey(currencyType))
            {
                currencyAmounts.Add(currencyType, 0);
            }
        }
    }

    private void GiveInitialCurrency()
    {
        if (PlayerPrefs.GetInt("initial_reward", 0) == 0)
        {
            SetCurrencyAmount(CurrencyType.CurrencyA, 10);
            SetCurrencyAmount(CurrencyType.CurrencyB, 10);
            SetCurrencyAmount(CurrencyType.CurrencyC, 5);

            PlayerPrefs.SetInt("initial_reward", 1);
            SaveCurrency();
        }
    }

    private void UpdateUI()
    {
        foreach (var currency in currencyAmounts)
        {
            OnCurrencyChanged?.Invoke(currency.Key, currency.Value);
        }
    }

    // Method that can be called externally to charge currency
    public void ChargeCurrency(CurrencyType currencyType, int amountToCharge, Action enoughCurrencyCallback, Action notEnoughCurrencyCallback)
    {
        // Check if there's enough currency to charge
        if (currencyAmounts[currencyType] >= amountToCharge)
        {
            // Execute the enough currency callback
            enoughCurrencyCallback?.Invoke();
            // Deduct the currency
            currencyAmounts[currencyType] -= amountToCharge;
            // Execute currency changed event
            OnCurrencyChanged?.Invoke(currencyType, currencyAmounts[currencyType]);
            // Update UI
            UpdateUI();
            // Save it
            SaveCurrency();
        }
        else
        {
            // Execute the not enough currency callback
            notEnoughCurrencyCallback?.Invoke();
        }
    }

    // Method that can be called externally to charge currency, this function takes into consideration the name of item being bought
    public bool ChargeCurrency(CurrencyType currencyType, int amountToCharge, string itemName, Action<string> enoughCurrencyCallback, Action notEnoughCurrencyCallback)
    {
        // Check if there's enough currency to charge
        if (currencyAmounts[currencyType] >= amountToCharge)
        {
            // Execute the enough currency callback with the item name
            enoughCurrencyCallback?.Invoke(itemName);
            // Deduct the currency
            currencyAmounts[currencyType] -= amountToCharge;
            // Execute currency changed event
            OnCurrencyChanged?.Invoke(currencyType, currencyAmounts[currencyType]);
            // Update UI
            UpdateUI();
            // Save it
            SaveCurrency();
            return true; // Transaction successful
        }
        else
        {
            // Execute the not enough currency callback
            notEnoughCurrencyCallback?.Invoke();
            return false; // Transaction failed due to insufficient funds
        }
    }

    // Method that can be called externally to reward currency
    public void RewardCurrency(CurrencyType currencyType, int amount)
    {
        // Add the currency
        currencyAmounts[currencyType] += amount;
        // Execute currency changed event
        OnCurrencyChanged?.Invoke(currencyType, currencyAmounts[currencyType]);
        // Update UI
        UpdateUI();
        // Save it
        SaveCurrency();
    }

    // Method that can be called externally to reward currency with callback
    public void RewardCurrency(CurrencyType currencyType, int amount, Action increaseCallback)
    {
        // Execute the increase currency callback
        increaseCallback?.Invoke();
        // Add the currency
        currencyAmounts[currencyType] += amount;
        // Execute currency changed event
        OnCurrencyChanged?.Invoke(currencyType, currencyAmounts[currencyType]);
        // Update UI
        UpdateUI();
        // Save it
        SaveCurrency();
    }

    // Method to set currency amount
    public void SetCurrencyAmount(CurrencyType currencyType, int amount)
    {
        currencyAmounts[currencyType] = amount;
        OnCurrencyChanged?.Invoke(currencyType, currencyAmounts[currencyType]);
        UpdateUI();
        SaveCurrency();
    }

    // Method to get currency amount
    public int GetCurrencyAmount(CurrencyType currencyType)
    {
        if (currencyAmounts.ContainsKey(currencyType))
            return currencyAmounts[currencyType];
        return 0; // Default to 0 if currency type not found
    }

    // Save currency amounts to PlayerPrefs
    private void SaveCurrency()
    {
        foreach (var currency in currencyAmounts)
        {
            PlayerPrefs.SetInt(currency.Key.ToString(), currency.Value);
        }
        PlayerPrefs.Save();
    }

    // Load currency amounts from PlayerPrefs
    private void LoadCurrency()
    {
        foreach (CurrencyType currencyType in Enum.GetValues(typeof(CurrencyType)))
        {
            if (PlayerPrefs.HasKey(currencyType.ToString()))
            {
                currencyAmounts[currencyType] = PlayerPrefs.GetInt(currencyType.ToString());
            }
            else
            {
                currencyAmounts[currencyType] = 0;
            }
        }
    }
}