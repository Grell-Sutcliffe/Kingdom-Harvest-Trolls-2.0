using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    [System.Serializable]
    public struct Cell
    {
        public Sprite sprite;
        public string type;
        public bool destroyable;
        public string title;
        public string down;
        public string right;
        public string up;
        public string left;
        public int coin_amount;
        public int wheat_amount;
        public int villager_amount;
        public int interval;
        public bool is_destroyed;
        public int cost_of_upgrate;
        public int coin_per_time;
        public int wheat_per_time;
        public int level;
        public int count_of_road;
        public bool is_just_road;
        public int rotation;
        public int index;
        public int time_for_peak;

        public Cell(Sprite _sprite,
                    string _type,
                    bool _destroyable,
                    string _title,
                    string _down,
                    string _right,
                    string _up,
                    string _left,
                    int _coin_amount,
                    int _wheat_amount,
                    int _villager_amount,
                    int _interval,
                    bool _is_destroyed,
                    int _cost_of_upgrate,
                    int _coin_per_time,
                    int _wheat_per_time,
                    int _level,
                    int _count_of_road,
                    bool _is_just_road,
                    int _rotation,
                    int _index
                    )
        {
            sprite = _sprite;
            type = _type;
            destroyable = _destroyable;
            title = _title;
            down = _down;
            right = _right;
            up = _up;
            left = _left;
            coin_amount = _coin_amount;
            wheat_amount = _wheat_amount;
            villager_amount = _villager_amount;
            interval = _interval;
            is_destroyed = _is_destroyed;
            cost_of_upgrate = _cost_of_upgrate;
            coin_per_time = _coin_per_time;
            wheat_per_time = _wheat_per_time;
            level = _level;
            count_of_road = _count_of_road;
            is_just_road = _is_just_road;
            rotation = _rotation;
            index = _index;
            time_for_peak = 60;
        }
    }

    public class CellsScript : MonoBehaviour
    {
        public Cell[] all_cells = new Cell[44];
        public Cell[] choose_cells;

        public Cell[] cellChoose = new Cell[6];

        System.Random random = new System.Random();

        public Image cell1;
        public Image cell2;
        public Image cell3;
        public Image cell4;
        public Image cell5;
        public Image cell6;

        private void Start()
        {
            for (int i = 0; i < all_cells.Length; i++)
            {
                if ((all_cells[i].type != "grass") && (!all_cells[i].is_destroyed && (all_cells[i].level == 0 || all_cells[i].type == "road")))
                {
                    System.Array.Resize(ref choose_cells, choose_cells.Length + 1);
                    choose_cells[choose_cells.Length - 1] = all_cells[i];
                    /*
                        all_cells[i].sprite, 
                        all_cells[i].type, 
                        all_cells[i].is_castle, 
                        all_cells[i].is_destroyed, 
                        all_cells[i].coin_per_time,
                        all_cells[i].wheat_per_time, 
                        all_cells[i].level,
                        all_cells[i].count_of_road,
                        all_cells[i].is_just_road
                    */
                }
                //Debug.Log(choose_cells[5].type);
            }

            RandomCell();
        }

        public void RandomCell()
        {
            HashSet<Cell> unique_cells = new HashSet<Cell>();

            while (unique_cells.Count <  cellChoose.Length)
            {
                unique_cells.Add(choose_cells[random.Next(0, choose_cells.Length)]);
            }

            /*
            for (int i = 0; i < cellChoose.Length; i++)
            {
                cellChoose[i] = choose_cells[random.Next(0, choose_cells.Length)];
                //Debug.Log(cellChoose[i].type);
            }
            */

            List<Cell> new_cells = unique_cells.ToList();

            cell1.sprite = new_cells[0].sprite;
            cell2.sprite = new_cells[1].sprite;
            cell3.sprite = new_cells[2].sprite;
            cell4.sprite = new_cells[3].sprite;
            cell5.sprite = new_cells[4].sprite;
            cell6.sprite = new_cells[5].sprite;
        }
    }
}


