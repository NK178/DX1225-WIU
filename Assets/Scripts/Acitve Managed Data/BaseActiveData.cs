using System;
using UnityEngine;

public class BaseActiveData
{

    public event Action onStateChanged;
    public float currentMoveSpeed;

    //need to do this for derrived classes so just use this one
    protected void TriggerStateChanged()
    {
        onStateChanged?.Invoke();
    }

    //Using the power of action events
    private bool _isMoving = false;
    public bool isMoving
    {
        get => _isMoving;
        set
        {
            if (_isMoving != value)
            {
                _isMoving = value;
                TriggerStateChanged();
            }
        }
    }


    public BaseActiveData()
    {
        isMoving = false; 
    }
}
