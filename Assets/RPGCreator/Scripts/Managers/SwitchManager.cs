using System.Collections;
using System.IO;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;

public class SwitchManager : MonoBehaviour
{
    public static SwitchManager instance;

    public Switch[] switches;

    readonly private string savePath = "/Saves/";

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

        // Save();
    }

    public void Save()
    {
#if !UNITY_EDITOR
        string saveFilePath = Application.dataPath + savePath + "/switches.SAVE";

        if(!Directory.Exists(Application.dataPath + savePath))
        {
            Debug.Log("NO EXIST");

            Directory.CreateDirectory(Application.dataPath + savePath);
        }
        else
        {
            Debug.LogWarning("THE ONE PIECE IS REAL");
        }

        //Create File if it doesn't exist
        foreach (Switch switchObj in switches)
        {
            File.AppendAllText(saveFilePath, JsonUtility.ToJson(switchObj) + "\n");
        }
#else
        Debug.LogWarning("Unable to save switches while in Editor.");
#endif
    }

    public void Load()
    {
#if !UNITY_EDITOR
        string loadFilePath = Application.dataPath + savePath + "/switches.SAVE";
        string[] loadList = File.ReadAllLines(loadFilePath);

        for (int loadIndex = 0; loadIndex < loadList.Length; loadIndex++)
        {
            Switch loadLine = JsonUtility.FromJson<Switch>(loadList[loadIndex]);

            switches[loadIndex].name = loadLine.name;
            switches[loadIndex].state = loadLine.state;
        }
#else
        Debug.LogWarning("Unable to load switches while in Editor.");
#endif
    }

    // inverts a Switch's state by name
    public static void ToggleSwitchState(string switchName)
    {
        // stores the static instance of switches
        Switch[] switches = instance.switches;

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
