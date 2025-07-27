using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyDisplay : MonoBehaviour
{
    int money;
    Text moneyText;

    private void Awake()
    {
        moneyText = gameObject.GetComponent<Text>();
    }

    private void Update()
    {
        if (moneyText != null)
        {
            money = CoinManager.GetCoinCount();
            moneyText.text = $"${money.ToString()}";
        }
    }
}
