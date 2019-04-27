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
        // If we're using persistent data - save to PlayerPrefs
        if (isPersistentData)
        {
            PlayerPrefs.SetInt(key, toSave);
        }
        // Otherwise, use our dictionary
        else
        {
            intDict[key] = toSave;
        }
    }

    public void SaveFloat(float toSave, string key)
    {
        // If we're using persistent data - save to PlayerPrefs
        if (isPersistentData)
        {
            PlayerPrefs.SetFloat(key, toSave);
        }
        // Otherwise, use our dictionary
        else
        {
            floatDict[key] = toSave;
        }
    }

    public void SaveBool(bool toSave, string key)
    {
        // If we're using persistent data - save to PlayerPrefs
        if (isPersistentData)
        {
            PlayerPrefs.SetInt(key, toSave? 1 : 0);
        }
        // Otherwise, use our dictionary
        else
        {
            boolDict[key] = toSave;
        }
    }

    public void SaveString(string toSave, string key)
    {
        // If we're using persistent data - save to PlayerPrefs
        if (isPersistentData)
        {
            PlayerPrefs.SetString(key, toSave);
        }
        // Otherwise, use our dictionary
        else
        {
            stringDict[key] = toSave;
        }
    }

    public int LoadInt(string key)
    {
        // If we're using persistent data - load from PlayerPrefs
        if (isPersistentData && PlayerPrefs.HasKey(key))
        {
            return PlayerPrefs.GetInt(key);
        }

        // Otherwise check if we can load from our dictionary
        if (!isPersistentData && intDict.ContainsKey(key))
        {
            return intDict[key];
        }
        
        // Tried to load without a prior save
        Debug.LogError("Tried to load int value not in memory");
        return -1;
    }
    
    public float LoadFloat(string key)
    {
        // If we're using persistent data - load from PlayerPrefs
        if (isPersistentData && PlayerPrefs.HasKey(key))
        {
            return PlayerPrefs.GetFloat(key);
        }
        
        // Otherwise check if we can load from our dictionary
        if (!isPersistentData && floatDict.ContainsKey(key))
        {
            return floatDict[key];
        }
        
        // Tried to load without a prior save
        Debug.LogError("Tried to load float value not in memory");
        return -1.0f;
    }
    
    public bool LoadBool(string key)
    {
        // If we're using persistent data - load from PlayerPrefs
        if (isPersistentData && PlayerPrefs.HasKey(key))
        {
            return PlayerPrefs.GetInt(key) == 1;
        }
        
        // Otherwise check if we can load from our dictionary
        if (!isPersistentData && boolDict.ContainsKey(key))
        {
            return boolDict[key];
        }
        
        // Tried to load without a prior save
        Debug.LogError("Tried to load bool value not in memory");
        return false;

    }
    
    public string LoadString(string key)
    {
        
        // If we're using persistent data - load from PlayerPrefs
        if (isPersistentData && PlayerPrefs.HasKey(key))
        {
            return PlayerPrefs.GetString(key);
        }
        
        // Otherwise check if we can load from our dictionary
        if (!isPersistentData && stringDict.ContainsKey(key))
        {
            return stringDict[key];
        }
        
        // Tried to load without a prior save
        Debug.LogError("Tried to load string value not in memory");
        return "";
    }
}
