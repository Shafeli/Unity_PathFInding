using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ai_EventEnvoker : MonoBehaviour
{
    [SerializeField] private GameEvent _gameEventToWatch;
    [SerializeField] public string EventListenerName;

    [SerializeField] private GameEventListener _eventListener;
    private GameObject _objectTarget;

    public void Envoke()
    {
        if (_eventListener)
            _gameEventToWatch.RaiseSpecific(_eventListener);
    }
    public void SetTarget(GameObject targetGameObject)
    {
        _objectTarget = targetGameObject;
        var tempArray = targetGameObject.GetComponents<GameEventListener>();

        foreach (GameEventListener eventListener in tempArray)
        {
            if (eventListener.ListenerName != EventListenerName) continue;

            _eventListener = eventListener;
            return;
        }
    }

    public void SetTarget(GameEventListener eventListener)
    {
        _eventListener = eventListener;
    }
}
