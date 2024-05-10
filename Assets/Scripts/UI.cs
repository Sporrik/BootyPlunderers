using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public Text moveText;
    public Text healthText;

    public void SetHUD(PirateCrew pirate)
    {
        moveText.text = pirate._movement.ToString();
    }
}
