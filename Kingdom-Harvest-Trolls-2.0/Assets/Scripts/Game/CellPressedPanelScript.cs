using Game;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CellPressedPanelScript : MonoBehaviour
{
    public GameObject Field;
    FieldScript fieldScript;

    public TextMeshProUGUI title;
    public Image image;
    public TextMeshProUGUI amount;

    public Sprite coin;
    public Sprite wheat;
    public Sprite knight;

    public Button claim_button;

    private void Start()
    {
        fieldScript = Field.GetComponent<FieldScript>();
    }

    public void ChangeTitle(Cell cell)
    {
        title.text = cell.title;

        if ((cell.coin_per_time > 0) || (cell.type == "road"))
        {
            image.GetComponent<Image>().sprite = coin;
            amount.text = cell.coin_amount.ToString();
        }
        if (cell.wheat_per_time > 0)
        {
            image.GetComponent<Image>().sprite = wheat;
            amount.text = cell.wheat_amount.ToString();
        }
    }

    public void ShowImage(bool IF)
    {
        image.gameObject.SetActive(IF);
        amount.gameObject.SetActive(IF);
    }
}
