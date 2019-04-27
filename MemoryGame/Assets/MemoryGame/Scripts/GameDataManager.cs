using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataManager
{
    private bool isPersistentData;
    private Dictionary<string, int> intDict = new Dictionary<string, int>();
    private Dictionary<string, float> floatDict = new Dictionary<string, float>();
    private Dictionary<string, bool> boolDict = new Dictionary<string, bool>();
    private Dictionary<string, string> stringDict = new Dictionary<string, string>();
    
    public GameDataManager(bool persistent)
    {
        isPersistentData = persistent;
    }

    public void SaveInt(int toSave, string key)
    {
        if (isPersistentData)
        {
            PlayerPrefs.SetInt(key, toSave);
        }
        else
        {
            intDict[key] = toSave;
        }
    }

    public void SaveFloat(float toSave, string key)
    {
        if (isPersistentData)
        {
            PlayerPrefs.SetFloat(key, toSave);
        }
        else
        {
            floatDict[key] = toSave;
        }
    }

    public void SaveBool(bool toSave, string key)
    {
        if (isPersistentData)
        {
            PlayerPrefs.SetInt(key, toSave? 1 : 0);
        }
        else
        {
            boolDict[key] = toSave;
        }
    }

    public void SaveString(string toSave, string key)
    {
        if (isPersistentData)
        {
            PlayerPrefs.SetString(key, toSave);
        }
        else
        {
            stringDict[key] = toSave;
        }
    }

    public int LoadInt(string key)
    {
        return 0;
    }
    
    public float LoadFloat(string key)
    {
        return 0.0f;
    }
    
    public bool LoadBool(string key)
    {
        if (isPersistentData && PlayerPrefs.HasKey(key))
        {
            return PlayerPrefs.GetInt(key) == 1;
        }
        if (!isPersistentData && boolDict.ContainsKey(key))
        {
            return boolDict[key];
        }
        Debug.LogError("Tried to Load bool value not in memory");
        return false;

    }
    
    public string LoadString(string key)
    {
        return "";
    }
}
