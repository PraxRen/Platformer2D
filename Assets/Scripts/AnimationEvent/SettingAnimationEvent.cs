using System;

public class SettingAnimationEvent
{
    public SettingAnimationEvent(IListenerAnimationEvent listener, Action action)
    {
        if (listener == null)
            throw new ArgumentNullException(nameof(listener));

        if (action == null)
            throw new ArgumentNullException(nameof(action));

        Listener = listener;
        Action = action;
    }

    public IListenerAnimationEvent Listener { get; }
    public Action Action { get; set; }
}
