using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class FinalReport : MonoBehaviour
{
    //Define dropdowns
    public TMP_Dropdown culpritBox;
    public TMP_Dropdown causeBox;
    public TMP_Dropdown moneyBox;

    //Flag
    bool tamperIn = false;

    void Start()
    {
        //Unlock the cursor
        Cursor.lockState = CursorLockMode.None;

        //Add options based on clue discovery
        if (JournalManager.momFound) {
            List<string> mom = new List<string> {"Hailey Plotwell"};
            culpritBox.AddOptions(mom);
            if (!tamperIn) {
                List<string> tamper = new List<string> {"Lighter fluid"};
                causeBox.AddOptions(tamper);
                tamperIn = true;
            }

        }

        if (JournalManager.daughterFound) {
            List<string> daughter = new List<string> {"Sadie Plotwell"};
            culpritBox.AddOptions(daughter);
            if (JournalManager.magnifier) {
                List<string> magnifying = new List<string> {"Magnifying accident"};
                causeBox.AddOptions(magnifying);
            }
        }

        if (JournalManager.sonFound) {
            List<string> son = new List<string> {"Jason Plotwell"};
            culpritBox.AddOptions(son);
            if (!tamperIn) {
                List<string> tamper = new List<string> {"Lighter fluid"};
                causeBox.AddOptions(tamper);
                tamperIn = true;
            }
        }
    }

}
