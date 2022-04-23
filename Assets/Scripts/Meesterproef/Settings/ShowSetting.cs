using UnityEngine;
using UnityEngine.UI;

public class ShowSetting : MonoBehaviour
{

    private void Awake()
    {
        InputField inputField = GetComponentInChildren<InputField>();
        Slider slider = GetComponentInChildren<Slider>();
        Toggle toggle = GetComponentInChildren<Toggle>();

        Settings settings = Managers.settingsInstance;

        if (inputField)
        {
            inputField.text = settings.GetString(gameObject.name);
            settings.SaveSetting(inputField);
        }

        if (slider)
        {
            slider.value= settings.GetFloat(gameObject.name);
            settings.SaveSetting(slider);
        }

        if (toggle)
        {
            toggle.isOn = settings.GetBool(gameObject.name);
            settings.SaveSetting(toggle);
        }
    } 
}
