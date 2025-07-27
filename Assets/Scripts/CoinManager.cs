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


public static class CoinManager
{
    private static int coinCount = 100; // Intial coin count set to 100

    public static void ChangeCoins(int change)
    {
        coinCount += change;
        Debug.Log("Coins added: " + change + ". Total coins: " + coinCount);
    }

    public static int GetCoinCount()
    {
        Debug.Log("player has: " + coinCount + "coins");
        return coinCount;
    }
}
