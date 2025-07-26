using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    static int coinCount;
    public static Text coinsText; // Static variable to hold the UI text to make it easier to update, if needed

    // Start is called before the first frame update
    void Start()
    {
        // Initialize coin count if needed
        coinCount = 100;
    }
    

    // Update is called once a game is completed.
    void setCoins(int value) // Method that updates the coins.
    {
        cointCount += value;
    }
}
