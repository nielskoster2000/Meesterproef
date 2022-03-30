using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class Settings : MonoBehaviour
{
    List<FieldInfo> options = new List<FieldInfo>();

    //Inspector values
    [SerializeField] string _username;
    [SerializeField] float _mouseSens;
    [SerializeField] float _volume;
    [SerializeField] bool _equipOnPickup;
    [SerializeField] bool _keepCursorInApplicationWindow;

    //Statics
    public static string username;
    public static float mouseSens;
    public static float volume;
    public static bool equipOnPickup;
    public static bool keepCursorInApplicationWindow;

    public static bool gamePaused = false;

    private void Start()
    {
        SetOptions();

        if (RequireSave())
        {
            print("A save was required, saving now...");
            SaveToPlayerPrefs(); 
        }
        else
        {
            LoadFromPlayerPrefs();
        }
    }

    void SetOptions()
    {
        FieldInfo[] settings = typeof(Settings).GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

        foreach (FieldInfo propertyWithUnderscore in settings)
        {
            if (propertyWithUnderscore.Name.Contains("_"))
            {
                FieldInfo propertyWithoutUnderscore = typeof(Settings).GetField(propertyWithUnderscore.Name.Replace("_", ""), BindingFlags.Public | BindingFlags.Static);
                propertyWithoutUnderscore.SetValue(this, propertyWithUnderscore.GetValue(this));

                options.Add(propertyWithoutUnderscore);
            }
        }
    }

    void SaveToPlayerPrefs()
    {
        foreach (FieldInfo fieldInfo in options)
        {
            switch (System.Type.GetTypeCode(fieldInfo.FieldType))
            {
                case System.TypeCode.Boolean:
                    PlayerPrefs.SetInt(fieldInfo.Name, System.Convert.ToInt32(fieldInfo.GetValue(this)));
                    break;
                case System.TypeCode.Int32:
                    PlayerPrefs.SetInt(fieldInfo.Name, System.Convert.ToInt32(fieldInfo.GetValue(this)));
                    break;
                case System.TypeCode.String:
                    PlayerPrefs.SetString(fieldInfo.Name, System.Convert.ToString(fieldInfo.GetValue(this)));
                    break;
                case System.TypeCode.Single:
                    PlayerPrefs.SetFloat(fieldInfo.Name, System.Convert.ToSingle(fieldInfo.GetValue(this)));
                    break;
                default:
                    Debug.LogWarning(fieldInfo.Name + " with fieldtype " + fieldInfo.FieldType + " did not save correctly to playerprefs! ");
                    break;
            }
        }

        PlayerPrefs.Save();

        print("Saved new settings to player prefs");
    }


    void LoadFromPlayerPrefs()
    {
        foreach (FieldInfo fieldInfo in options)
        {
            switch (System.Type.GetTypeCode(fieldInfo.FieldType))
            {
                case System.TypeCode.Boolean:
                    fieldInfo.SetValue(fieldInfo, System.Convert.ToBoolean(PlayerPrefs.GetInt(fieldInfo.Name, System.Convert.ToInt32(fieldInfo.GetValue(this)))));
                    break;
                case System.TypeCode.Int32:
                    fieldInfo.SetValue(fieldInfo, PlayerPrefs.GetInt(fieldInfo.Name, System.Convert.ToInt32(fieldInfo.GetValue(this))));
                    break;
                case System.TypeCode.String:
                    fieldInfo.SetValue(fieldInfo, PlayerPrefs.GetString(fieldInfo.Name, System.Convert.ToString(fieldInfo.GetValue(this))));
                    break;
                case System.TypeCode.Single:
                    fieldInfo.SetValue(fieldInfo, PlayerPrefs.GetFloat(fieldInfo.Name, System.Convert.ToSingle(fieldInfo.GetValue(this))));
                    break;
                default:
                    Debug.LogWarning(fieldInfo.Name + " with fieldtype " + fieldInfo.FieldType + " did not load correctly from playerprefs! ");
                    break;
            }
        }

        print("Loaded settings from player prefs");
    }

    bool RequireSave()
    {
        foreach (FieldInfo fieldInfo in options)
        {
            if (PlayerPrefs.HasKey(fieldInfo.Name))
            {
                switch (System.Type.GetTypeCode(fieldInfo.FieldType))
                {
                    case System.TypeCode.Boolean:
                        if (PlayerPrefs.GetInt(fieldInfo.Name) != System.Convert.ToInt32(fieldInfo.GetValue(this))) { return true; }
                        break;
                    case System.TypeCode.Int32:
                        if (PlayerPrefs.GetInt(fieldInfo.Name) != System.Convert.ToInt32(fieldInfo.GetValue(this))) { return true; }
                        break;
                    case System.TypeCode.String:
                        if (PlayerPrefs.GetString(fieldInfo.Name) != System.Convert.ToString(fieldInfo.GetValue(this))) { return true; }
                        break;
                    case System.TypeCode.Single:
                        if (PlayerPrefs.GetFloat(fieldInfo.Name) != System.Convert.ToSingle(fieldInfo.GetValue(this))) { return true; }
                        break;
                    default:
                        Debug.LogWarning(fieldInfo.Name + " with fieldtype " + fieldInfo.FieldType + " did specify any of the accepted parameters for playerprefs! ");
                        break;
                }
            }
        }

        return false;
    }

    public static void PauseGame(bool boolean)
    {
        gamePaused = boolean;
    }
}
