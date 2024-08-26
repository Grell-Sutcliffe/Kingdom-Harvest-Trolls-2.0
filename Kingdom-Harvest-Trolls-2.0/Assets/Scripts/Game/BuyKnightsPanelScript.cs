using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuyKnightsPanelScript : MonoBehaviour
{
    public int knights_amount = 1;
    public int cost_coins = 20;
    public int cost_wheat = 15;

    public TextMeshProUGUI knights_amount_text;
    public TextMeshProUGUI cost_coins_text;
    public TextMeshProUGUI cost_wheat_text;

    public Button buy_button;

    private void Start()
    {
        UpdateInfo();
    }

    public void UpdateInfo()
    {
        knights_amount_text.text = knights_amount.ToString();
        cost_coins_text.text = cost_coins.ToString();
        cost_wheat_text.text = cost_wheat.ToString();
    }
}
