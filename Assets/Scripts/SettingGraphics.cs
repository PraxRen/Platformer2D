using UnityEngine;

[System.Serializable]
public class SettingGraphics
{
    [SerializeField] private TypeGraphics _typeGraphics;
    [SerializeField] private GameObject _graphics;

    public TypeGraphics TypeGraphics => _typeGraphics;
    public GameObject Graphics => _graphics;
}
