using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TimelineController : MonoBehaviour
{
    private GameController gameController;
    private FieldScript fieldScript;
    private Vector3 dude_scale;
    private bool IF = true;

    public GameObject bag;
    public GameObject left_panel_shower;
    public GameObject recources_materials_shower;
    public GameObject recources_people_shower;
    public GameObject buy_new_cell_shower;
    public GameObject rotate_new_cell_shower;
    public GameObject buy_knights_shower;
    public GameObject upgrade_castle_shower;
    public GameObject skip_button;

    public Sprite castle_shower;
    private Sprite last_castle;

    private string[] introductions = 
    {
        "Greetings, my King! I'm your servant Sycophant and I will assist you!",
        "You, my king, must defend the kingdom and villagers from evil trolls and expand it!",
    };
    private string left_panel_show = "To the left of the screen, there is a panel displating the kingdom's resources.";
    private string recources_materials_show = "You have some reserves of gold and wheat.";
    private string recources_people_show = "Do not forget to hire knight to protect your villagers and expand their population.";
    private string buy_new_cell_show = "You can buy a new cell of map to expand your Kingdom. Chose one you want from several options.";
    private string rotate_new_cell_show = "Rotate chosen cell by clicking on the bigger image of it and then place it wherever you want on the map of your Kingdom.";
    private string castle_show = "That is your first castle. Click on it to get more information.";
    private string buy_knights_show = "Here you can fire new knights who will protect your Kingdom from troll's attacks.";
    private string upgrade_castle_show = "And here you can upgrade you castle so cillagers could live better in your Kingdom.";
    private string about_village = "You can collect taxes from the Kingdom and roads by clicking on them. Also don't forget to collect ripe wheat.";
    private string luck = "I wish you best luck! Be wise, my King!";

    private int current_index = -1;

    private void Start()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        fieldScript = GameObject.Find("FieldPanel").GetComponent<FieldScript>();
        dude_scale = gameController.Dude.transform.localScale;

        bag.SetActive(true);
        
        Turn_IF(false);
    }

    private void Update()
    {
        if (!gameController.Dude.active)
        {
            if (IF)
            {
                current_index++;

                if (current_index == 0)
                {
                    gameController.OpenDude(introductions[current_index]);
                    ChangeDudeScale(2f);
                }

                else if (current_index == 1)
                {
                    gameController.OpenDude(introductions[current_index]);
                }

                else if (current_index == 2)
                {
                    gameController.Dude.transform.localScale = dude_scale;
                    gameController.OpenDude(left_panel_show);
                    left_panel_shower.gameObject.SetActive(true);
                }

                else if (current_index == 3)
                {
                    Turn_IF(false);
                    gameController.OpenDude(recources_materials_show);
                    recources_materials_shower.gameObject.SetActive(true);
                }

                else if (current_index == 4)
                {
                    Turn_IF(false);
                    gameController.OpenDude(recources_people_show);
                    recources_people_shower.gameObject.SetActive(true);
                }

                else if (current_index == 5)
                {
                    Turn_IF(false);
                    gameController.OpenDude(buy_new_cell_show);
                    buy_new_cell_shower.gameObject.SetActive(true);
                }

                else if (current_index == 6)
                {
                    Turn_IF(false);
                    gameController.OpenDude(rotate_new_cell_show);
                    rotate_new_cell_shower.gameObject.SetActive(true);
                }

                else if (current_index == 7)
                {
                    Turn_IF(false);
                    gameController.OpenDude(castle_show);
                    CastleShower();
                }

                else if (current_index == 8)
                {
                    Turn_IF(false);
                    gameController.FieldCellOnClick(fieldScript.dark_cells[fieldScript.first_castle_x, fieldScript.first_castle_y], fieldScript.first_castle_x, fieldScript.first_castle_y);
                    gameController.OpenDude(buy_knights_show);
                    buy_knights_shower.gameObject.SetActive(true);
                }

                else if (current_index == 9)
                {
                    Turn_IF(false);
                    gameController.FieldCellOnClick(fieldScript.dark_cells[fieldScript.first_castle_x, fieldScript.first_castle_y], fieldScript.first_castle_x, fieldScript.first_castle_y);
                    gameController.OpenDude(upgrade_castle_show);
                    upgrade_castle_shower.gameObject.SetActive(true);
                }

                else if (current_index == 10)
                {
                    Turn_IF(false);
                    gameController.OpenDude(about_village);
                    ChangeDudeScale(2f);
                }

                else if (current_index == 11)
                {
                    gameController.OpenDude(luck);
                }

                else
                {
                    gameController.Dude.transform.localScale = dude_scale;
                    Skip();
                }
            }
        }
    }

    private void ChangeDudeScale(float times)
    {
        Vector3 new_scale = new Vector3(dude_scale.x * times, dude_scale.y * times, dude_scale.z);

        gameController.Dude.transform.localScale = new_scale;
    }

    private void CastleShower()
    {
        last_castle = fieldScript.dark_cells[fieldScript.first_castle_x, fieldScript.first_castle_y].GetComponent<Image>().sprite;
        fieldScript.dark_cells[fieldScript.first_castle_x, fieldScript.first_castle_y].GetComponent<Image>().sprite = castle_shower;
        Invoke("CastleReturn", 3f);
    }

    private void CastleReturn()
    {
        fieldScript.dark_cells[fieldScript.first_castle_x, fieldScript.first_castle_y].GetComponent<Image>().sprite = last_castle;
        gameController.FieldCellOnClick(fieldScript.dark_cells[fieldScript.first_castle_x, fieldScript.first_castle_y], fieldScript.first_castle_x, fieldScript.first_castle_y);

        if (current_index < 8)
        {
            gameController.CloseDude();
        }
    }

    public void CloseCurrentShower()
    {
        Turn_IF(false);
        gameController.CloseDude();
    }

    private void Turn_IF(bool IF)
    {
        left_panel_shower.gameObject.SetActive(IF);
        recources_materials_shower.gameObject.SetActive(IF);
        recources_people_shower.gameObject.SetActive(IF);
        buy_new_cell_shower.gameObject.SetActive(IF);
        rotate_new_cell_shower.gameObject.SetActive(IF);
        buy_knights_shower.gameObject.SetActive(IF);
        upgrade_castle_shower.gameObject.SetActive(IF);
    }

    public void Skip()
    {
        IF = false;
        Turn_IF(false);
        gameController.CloseDude();
        skip_button.gameObject.SetActive(false);
        gameController.StartTrollSpawn();

        Destroy(gameObject);
    }
}
