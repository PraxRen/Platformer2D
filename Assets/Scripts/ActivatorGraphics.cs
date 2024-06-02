using System.Linq;
using UnityEngine;

public class ActivatorGraphics : MonoBehaviour
{
    [SerializeField] private SettingGraphics[] _settings;

    public void Activate(TypeGraphics typeGraphics, Vector2 position, Vector2 size)
    {
        SettingGraphics settingGraphics = _settings.FirstOrDefault(setting => setting.TypeGraphics == typeGraphics);

        if (settingGraphics == null)
            return;

        settingGraphics.Graphics.SetActive(true);
        settingGraphics.Graphics.transform.position = position;
        settingGraphics.Graphics.transform.localScale = size;
    }

    public void Deactivate(TypeGraphics typeGraphics)
    {
        SettingGraphics settingGraphics = _settings.FirstOrDefault(setting => setting.TypeGraphics == typeGraphics);

        if (settingGraphics == null)
            return;

        settingGraphics.Graphics.SetActive(false);
    }
}
