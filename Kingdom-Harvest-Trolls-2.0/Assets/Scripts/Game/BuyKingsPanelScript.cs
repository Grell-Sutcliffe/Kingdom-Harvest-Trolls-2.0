using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuyKingsPanelScript : MonoBehaviour
{
    public int kings_amount = 1;
    public int cost_coins = 40;
    public int cost_wheat = 50;

    public TextMeshProUGUI kings_amount_text;
    public TextMeshProUGUI cost_coins_text;
    public TextMeshProUGUI cost_wheat_text;

    public Button buy_button;

    private void Start()
    {
        UpdateInfo();
    }

    public void UpdateInfo()
    {
        kings_amount_text.text = kings_amount.ToString();
        cost_coins_text.text = cost_coins.ToString();
        cost_wheat_text.text = cost_wheat.ToString();
    }
}
