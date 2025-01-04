using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuyVillagersPanelScript : MonoBehaviour
{
    public int villagers_amount = 1;
    public int cost_coins = 25;
    public int cost_wheat = 10;

    public TextMeshProUGUI villagers_amount_text;
    public TextMeshProUGUI cost_coins_text;
    public TextMeshProUGUI cost_wheat_text;

    public Button buy_button;

    private void Start()
    {
        UpdateInfo();
    }

    public void UpdateInfo()
    {
        villagers_amount_text.text = villagers_amount.ToString();
        cost_coins_text.text = cost_coins.ToString();
        cost_wheat_text.text = cost_wheat.ToString();
    }
}