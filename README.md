# Sentry's Currency System Documentation

## Overview

The Currency System in this project is designed to manage different types of in-game currencies. It handles saving, loading, and modifying currency amounts using Unity's `PlayerPrefs` for persistence. This system includes features to charge and reward currency, update the UI, and notify listeners when currency values change.

## Features

- **Multiple Currency Types**: Manage various in-game currencies such as Spirit Points, Energy Points, Shield Points, Headstarts, and Score Multipliers.
- **Persistent Storage**: Save and load currency amounts using `PlayerPrefs`.
- **Currency Operations**: Charge and reward currency with callbacks for success and failure conditions.
- **Event Notification**: Notify listeners when currency values change.

- Define the types of currencies you want to use in the CurrencyType Enum

## Currency Types

public enum CurrencyType
{
    CurrencyA,
    CurrencyB,
    CurrencyC,
}

## Setting up

- Ensure the CurrencySystem script is attached to a GameObject in your scene. This will initialize the currency system when the game starts.

### Initialize and Load Currencie

- When the game starts, the system initializes all currency types and loads their values from PlayerPrefs.

```c#
private void Start()
{
    InitializeCurrencyAmounts();
    LoadCurrency();
    GiveInitialCurrency();
    UpdateUI();
}
```

### Setting Initial Currency

- The GiveInitialCurrency method sets initial values for currencies if they have not been set before. This method is called once during the first run.

```c#
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
```

#### Rewarding Currency

- Reward currency for achievements or other in-game events.

```c#
CurrencySystem.instance.RewardCurrency(CurrencyType.CurrencyA, 10);
```

#### Setting Currency Amount

- Directly set the amount of a specific currency.

```c#
CurrencySystem.instance.SetCurrencyAmount(CurrencyType.CurrencyA, 100);
```

#### Charging Currency

- Charge currency for in-game purchases or actions. Use callbacks to handle success and failure conditions. Here are some examples

- **Charging currency with callbacks 'Type A'**

```c#
CurrencySystem.instance.ChargeCurrency(CurrencyType.CurrencyA, 5, OnEnoughCurrency, OnNotEnoughCurrency);

void OnEnoughCurrency()
{
    Debug.Log("Currency charged successfully.");
}

void OnNotEnoughCurrency()
{
    Debug.Log("Not enough currency.");
}
```

- **Charging currency with callbacks 'Type B'**

```c#
bool wasTransactionSuccessful = CurrencySystem.instance.ChargeCurrency
(
    CurrencyType.CurrencyA,
    10,
    "Name of item being purchased",
    OnEnoughCurrency,
    OnNotEnoughCurrency
);

if (wasTransactionSuccessful)
{
    /**
        You can execute specific functions here and use 
        OnEnoughCurrency and OnNotEnoughCurrency for more generic functions
    **/
    Debug.Log("transaction was successful");
}

// callback implementations
void OnEnoughCurrency(string itemName)
{
    Debug.Log($"Purchased item: {itemName}");

}

void OnNotEnoughCurrency()
{
    Debug.Log("Not enough currency.");
}
```

- **Charging currency without callbacks 'Type A'**

```c#
CurrencySystem.instance.ChargeCurrency(CurrencyType.CurrencyA, 5, null, null);
```

- **Charging currency without callbacks 'Type B'**

```c#
bool wasTransactionSuccessful = CurrencySystem.instance.ChargeCurrency
(
    CurrencyType.CurrencyA,
    10,
    "",
    null,
    null
);

if (wasTransactionSuccessful)
{
    /**
        You can execute specific functions here
    **/
    Debug.Log("transaction was successful");
}
```

#### Getting Currency Amount

- Get the current amount of a specific currency.

```c#
int CurrencyA = CurrencySystem.instance.GetCurrencyAmount(CurrencyType.CurrencyA);
Debug.Log("Points: " + CurrencyA);
```

## Event Subscription

- You can subscribe to the OnCurrencyChanged event to update the UI or other game elements when the currency value changes.

```c#
    private void OnEnable()
    {
        // Subscribe to the OnCurrencyChanged event
        if (CurrencySystem.instance != null)
        {
            CurrencySystem.instance.OnCurrencyChanged += UpdateCurrencyUI;
        }
    }

    private void OnDisable()
    {
        // Unsubscribe from the OnCurrencyChanged event
        if (CurrencySystem.instance != null)
        {
            CurrencySystem.instance.OnCurrencyChanged -= UpdateCurrencyUI;
        }
    }

    // Method to update the UI elements
    private void UpdateCurrencyUI(CurrencyType currencyType, int amount)
    {
        switch (currencyType)
        {
            case CurrencyType.CurrencyA:
                CurrencyA.text = amount.ToString();
                break;
            case CurrencyType.CurrencyB:
                CurrencyB.text = amount.ToString();
                break;
        }
    }
```

## Example Use Cases

- **In-Game Store Purchase**
- Deduct currency when the player buys an item.
- Provide callbacks to handle successful and unsuccessful purchases.

```c#
CurrencySystem.instance.ChargeCurrency(CurrencyType.CurrencyA, 50, OnPurchaseSuccess, OnPurchaseFailure);

void OnPurchaseSuccess()
{
    Debug.Log("Item purchased successfully.");
}

void OnPurchaseFailure()
{
    Debug.Log("Failed to purchase item. Not enough currency.");
}
```

- **Level Completion Reward**
- Reward the player with currency upon completing a level.

```c#
CurrencySystem.instance.RewardCurrency(CurrencyType.CurrencyA, 100);
```

This `README.md` file provides a comprehensive guide to using the Currency System, including setup, usage examples, and event subscription. It should help developers integrate and utilize the currency management functionality in their Unity projects effectively.
