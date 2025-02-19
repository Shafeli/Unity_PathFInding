
using System.Collections.Generic;

using UnityEngine;

//TODO: Finish features as needed

[CreateAssetMenu]
public class GameEvent : ScriptableObject
{
    private List<GameEventListener> _listeners = new List<GameEventListener>();
    public List<GameEventListener> Listeners => _listeners;

    [SerializeField] public bool DebugLogging;

    // Raise Loops backward in case the event includes removing the response
    // this should account for and avoid the out of bounds exception.
    public void RaiseAll()
    {
        for (int i = _listeners.Count - 1; i >= 0; --i)
            _listeners[i].OnEventRaised();

        if (DebugLogging)
            Debug.Log("Event Raised! Notifying " + _listeners.Count + " Listeners...");
    }

    public void RaiseSpecific(GameEventListener listener)
    {
        if (_listeners.Contains(listener))
        {
            listener.OnEventRaised();

            if (DebugLogging)
                Debug.Log("Event Raised! Notifying Listener " + listener.ListenerName);
        }
    }

    public void RegisterListener(GameEventListener listener)
    {
        if (!_listeners.Contains(listener))
        {
            _listeners.Add(listener);
        }
    }
    public void UnregisterListener(GameEventListener listener)
    {
        if (_listeners.Contains(listener))
        {
            _listeners.Remove(listener);
        }
    }
}
