using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SwitchManager : MonoBehaviour
{
    public static SwitchManager instance;

    public Switch[] switches;

    [SerializeField] private string savePath = "/Saves/";

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.LogWarning("Destroying " + gameObject.name + "'s SwitchManager script");
            Destroy(this);
        }
        else
        {
            instance = this;
        }

        Save();
    }

    public void Save()
    {
#if !UNITY_EDITOR
        string saveFilePath = Application.streamingAssetsPath + savePath + "Switches.txt";

        if(!File.Exists(saveFilePath))
        {
            Debug.Log("NO EXIST");
            File.WriteAllText(saveFilePath, "hehehaha");
        }
#else
        Debug.Log("Unable to save switches while in Editor.");
#endif
    }

    // inverts a Switch's state by name
    public void ToggleSwitchState(string switchName)
    {
        // loops through the list of switches
        for (int switchIndex = 0; switchIndex < switches.Length; switchIndex++)
        {
            Switch changeSwitch = switches[switchIndex];

            // if the current switch matches the switchName,
            // then invert the switch
            if (changeSwitch.name == switchName)
            {
                changeSwitch.state = !changeSwitch.state;
                return;
            }
        }
    }

    public bool GetSwitchState(string switchName)
    {
        for (int switchIndex = 0; switchIndex < switches.Length; switchIndex++)
        {
            Switch returnSwitch = switches[switchIndex];

            if (returnSwitch.name.ToLower() == switchName.ToLower())
            {
                return returnSwitch.state;
            }
        }

        Debug.LogError("The switch '" + switchName + "' does not exist in SwitchManager.");
        return false;
    }

    [System.Serializable]
    public struct Switch
    {
        public string name;
        public bool state;
    }
}
