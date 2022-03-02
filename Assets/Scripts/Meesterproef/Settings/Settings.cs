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
        SetStatics();

        if (PlayerPrefs.HasKey("username"))
        {
            LoadFromPlayerPrefs();
        }
        else
        {
            SaveToPlayerPrefs();
        }
    }

    void SetStatics()
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

    void LoadFromPlayerPrefs()
    {
        foreach (FieldInfo fieldinfo in options)
        {
            switch (System.Type.GetTypeCode(fieldinfo.FieldType))
            {
                case System.TypeCode.Boolean:
                    fieldinfo.SetValue(fieldinfo, System.Convert.ToBoolean(PlayerPrefs.GetInt(fieldinfo.Name, System.Convert.ToInt32(fieldinfo.GetValue(this)))));
                    break;
                case System.TypeCode.Int32:
                    fieldinfo.SetValue(fieldinfo, PlayerPrefs.GetInt(fieldinfo.Name, System.Convert.ToInt32(fieldinfo.GetValue(this))));
                    break;
                case System.TypeCode.String:
                    fieldinfo.SetValue(fieldinfo, PlayerPrefs.GetString(fieldinfo.Name, System.Convert.ToString(fieldinfo.GetValue(this))));
                    break;
                case System.TypeCode.Single:
                    fieldinfo.SetValue(fieldinfo, PlayerPrefs.GetFloat(fieldinfo.Name, System.Convert.ToSingle(fieldinfo.GetValue(this))));
                    break;
                default:
                    Debug.LogWarning(fieldinfo.Name + " with fieldtype " + fieldinfo.FieldType + " did not load correctly from playerprefs! ");
                    break;
            }
        }

        print("Loaded settings from player prefs");
    }

    void SaveToPlayerPrefs()
    {
        foreach (FieldInfo fieldinfo in options)
        {
            switch (System.Type.GetTypeCode(fieldinfo.FieldType))
            {
                case System.TypeCode.Boolean:
                    PlayerPrefs.SetInt(fieldinfo.Name, System.Convert.ToInt32(fieldinfo.GetValue(this)));
                    break;
                case System.TypeCode.Int32:
                    PlayerPrefs.SetInt(fieldinfo.Name, System.Convert.ToInt32(fieldinfo.GetValue(this)));
                    break;
                case System.TypeCode.String:
                    PlayerPrefs.SetString(fieldinfo.Name, System.Convert.ToString(fieldinfo.GetValue(this)));
                    break;
                case System.TypeCode.Single:
                    PlayerPrefs.SetFloat(fieldinfo.Name, System.Convert.ToSingle(fieldinfo.GetValue(this)));
                    break;
                default:
                    Debug.LogWarning(fieldinfo.Name + " with fieldtype " + fieldinfo.FieldType + " did not save correctly to playerprefs! ");
                    break;
            }
        }

        PlayerPrefs.Save();

        print("Saved new settings to player prefs");
    }

    public static void PauseGame(bool boolean)
    {
        gamePaused = boolean;
    }
}
