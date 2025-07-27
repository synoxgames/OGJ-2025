using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

/* Currently : 
    CoinManager is initialized on gameLoad, 
    CoinManager keeps track of the coin count, but it is not persistent across game sessions.

    Future improvements:
        - Make coin count persistent across game sessions using playerrefs etc.
*/


public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance { get; private set; }
    private int coinCount = 100; // Intial coin count set to 100

    private void Awake() 
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Ensure this instance persists across scenes
        }
        else
        {
            Destroy(gameObject); // Ensure only one instance exists
        }
    }
    public void AddCoins(int amount)
    {
        coinCount += amount;
        Debug.Log($"Coins added: {amount}. Total coins: {coinCount}");
    }
    public int GetCoinCount()
    {
        return coinCount;
    }
}
