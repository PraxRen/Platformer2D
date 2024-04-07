using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AnimationEventController : MonoBehaviour
{
    private Dictionary<TypeAnimationEvent, List<SettingAnimationEvent>> _actions = new Dictionary<TypeAnimationEvent, List<SettingAnimationEvent>>();

    public void AddAction(TypeAnimationEvent typeAnimationEvent, IListenerAnimationEvent listener, Action action)
    {
        if (_actions.ContainsKey(typeAnimationEvent) == false) 
        {
            _actions[typeAnimationEvent] = new List<SettingAnimationEvent> { new SettingAnimationEvent(listener, action)};
            return;
        }

        SettingAnimationEvent settingWithListener = _actions[typeAnimationEvent].FirstOrDefault(setting => setting.Listener == listener);

        if (settingWithListener != null)
            settingWithListener.Action += action;
        else
            _actions[typeAnimationEvent].Add(new SettingAnimationEvent(listener, action));
        
    }

    public void RemoveAction(TypeAnimationEvent typeAnimationEvent, IListenerAnimationEvent listener, Action action)
    {
        if (_actions.ContainsKey(typeAnimationEvent) == false)
            return;

        SettingAnimationEvent settingWithListener = _actions[typeAnimationEvent].FirstOrDefault(setting => setting.Listener == listener);

        if(settingWithListener == null)
            return;

        settingWithListener.Action -= action;

        if (settingWithListener.Action == null)
        {
            _actions[typeAnimationEvent].Remove(settingWithListener);

            if (_actions[typeAnimationEvent].Count == 0)
                _actions.Remove(typeAnimationEvent);
        }
    }

    //Animation Event
    private void RunEvent(TypeAnimationEvent typeAnimationEvent)
    {
        if (_actions.ContainsKey(typeAnimationEvent) == false)
            return;

        foreach (SettingAnimationEvent settingAnimationEvent in _actions[typeAnimationEvent])
        {
            settingAnimationEvent.Action?.Invoke();
        }
    }
}
