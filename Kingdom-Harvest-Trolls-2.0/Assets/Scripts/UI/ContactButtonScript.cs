using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactButtonScript : MonoBehaviour
{
    public void ContactButtonClick()
    {
        Application.OpenURL("mailto:kingdom_harvest_trolls@mail.ru");
    }
}
