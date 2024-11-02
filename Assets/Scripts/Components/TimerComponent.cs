using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TimerComponent : MonoBehaviour
{
    [SerializeField] float _timeToAction;

    private TimerController _timerController;

    public TimerController TimerController 
    { 
        get 
        { 
            return _timerController; 
        } 
    }

    void Awake()
    {
        _timerController = new TimerController();
        _timerController.Initialize(_timeToAction);
    }


    // Update is called once per frame
    void Update()
    {
        if(_timerController == null || !_timerController.IsInitialized())
        {
            return;
        }
        _timerController.Update(Time.deltaTime);
    }
}
