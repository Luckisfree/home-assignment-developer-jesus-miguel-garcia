using System;

public class TimerController
{
    private float _timeToAction;
    private float _currentTime = 0f;
    private bool _timerStarted = false;
    private bool _initialized = false;


    public event Action _OnTimeTickReached;

    public void Initialize(float timeToReach)
    {
        _timeToAction = timeToReach;
        ResetTimer();
        _timerStarted = false;
        _initialized = true;
    }

    public bool IsInitialized()
    { 
        return _initialized; 
    }

    public void StartTimer()
    {
        _timerStarted = true;
    }

    public bool IsRunning()
    {
        return _timerStarted;
    }

    public void StopTimer()
    {
        _timerStarted = false;
    }

    public void Update(float deltaTime)
    {
        if(!_timerStarted)
        {
            return;
        }
        _currentTime -= deltaTime;
        if (_currentTime <= 0f)
        {
            _OnTimeTickReached?.Invoke();
            ResetTimer();
        }
    }

    public void ResetTimer()
    {
        _currentTime = _timeToAction;
    }
}
