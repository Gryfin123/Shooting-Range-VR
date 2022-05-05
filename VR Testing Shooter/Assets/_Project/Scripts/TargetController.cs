using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class TargetController : MonoBehaviour
{

    [SerializeField] private Transform _frontIndicator;
    [SerializeField] private Transform _targetRoot;
    public TargetState _startingState = TargetState.AVAILABLE;
    public float _rotationSpeed = 90f;

    public Color _availableColor;
    public Color _spinningColor;
    public Color _unavailableColor;

    public float _popupOrder = 0;


    private Renderer _renderer;
    private TargetState _currState;
    private float _targetRotation;
    

    // Start is called before the first frame update
    void Start()
    {
        _renderer = GetComponent<Renderer>();
        ChangeState(_startingState);
        StartCoroutine(WaitAndTurnBackOn());
    }

    void Update(){

    }

    private void ChangeState(TargetState newState)
    {
        switch(newState)
        {
            case TargetState.AVAILABLE:
                ChangeColor(_availableColor);
                break;
            case TargetState.UNAVAILABLE:
                ChangeColor(_unavailableColor);
                break;
            case TargetState.SPINNING:
                ChangeColor(_spinningColor);
                break;
        }

        _currState = newState;
    }

    private void ChangeColor(Color newColor)
    {
        _renderer.material.color = newColor;
    }

    public void OnHit()
    {
        if (_currState == TargetState.AVAILABLE)
        {
            ChangeState(TargetState.UNAVAILABLE);
            StartCoroutine(SpinTarget(TargetState.UNAVAILABLE));
            StartCoroutine(WaitAndTurnBackOn());
        }
    }

    private IEnumerator WaitAndTurnBackOn()
    {
        yield return new WaitForSeconds(6f);
        ChangeState(TargetState.SPINNING);
        StartCoroutine(SpinTarget(TargetState.AVAILABLE));
    }

    private void OnTriggerEnter(Collider other) {
        BulletScript otherScript = other.gameObject.GetComponent<BulletScript>();
        if (otherScript != null)
        {
            OnHit();
        }
    }

    private IEnumerator SpinTarget(TargetState newState)
    {
        Vector3 rotationNeeded = Vector3.up;

        switch(newState)
        {
            case TargetState.AVAILABLE:
                rotationNeeded = Vector3.forward;
                break;
            case TargetState.UNAVAILABLE:
                rotationNeeded = Vector3.back;
                break;
        }


        Vector3 targetRotation = _targetRoot.position - _frontIndicator.position;
        Quaternion toRotation = Quaternion.LookRotation(rotationNeeded, targetRotation);
        
        float finishTime = Time.time + 180 / _rotationSpeed;
        while(Time.time < finishTime)
        {
            _targetRoot.rotation = Quaternion.RotateTowards(_targetRoot.rotation, toRotation, _rotationSpeed * Time.deltaTime);
            
            yield return new WaitForEndOfFrame();
        }

        ChangeState(newState);
    }
}

public enum TargetState {
    AVAILABLE,
    UNAVAILABLE,
    SPINNING
}
/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class TargetController : MonoBehaviour
{

    public TargetState _startingState = TargetState.AVAILABLE;
    public float _flashDuration = 1f;

    public Color _availableColor;
    public Color _spinningColor;
    public Color _unavailableColor;

    public float _popupOrder = 0;


    private Renderer _renderer;
    private TargetState _currState;
    private float _targetRotation;
    

    // Start is called before the first frame update
    void Start()
    {
        _renderer = GetComponent<Renderer>();
        ChangeState(_startingState);
        StartCoroutine(WaitAndTurnBackOn());
    }

    void Update(){

    }

    private void ChangeState(TargetState newState)
    {
        switch(newState)
        {
            case TargetState.AVAILABLE:
                ChangeColor(_availableColor);
                break;
            case TargetState.UNAVAILABLE:
                ChangeColor(_unavailableColor);
                break;
            case TargetState.SPINNING:
                ChangeColor(_spinningColor);
                break;
        }

        _currState = newState;
    }

    public void OnHit()
    {
        if (_currState == TargetState.AVAILABLE)
        {
            StartCoroutine(SpinTarget(TargetState.UNAVAILABLE));
            StartCoroutine(WaitAndTurnBackOn());
        }
    }

    private void ChangeColor(Color newColor)
    {
        _renderer.material.color = newColor;
    }

    private IEnumerator WaitAndTurnBackOn()
    {
        yield return new WaitForSeconds(6f);
        StartCoroutine(SpinTarget(TargetState.AVAILABLE));
    }

    private void OnTriggerEnter(Collider other) {
        BulletScript otherScript = other.gameObject.GetComponent<BulletScript>();
        if (otherScript != null)
        {
            OnHit();
        }
    }

    private IEnumerator SpinTarget(TargetState newState)
    {
        ChangeState(TargetState.SPINNING);
        float endTime = Time.time + _flashDuration;
        Color whiteFlash = new Color(255, 255, 255, 1);

        float amountFlashes = 3f;
        float flashEnter = _flashDuration / (2f / (3f * amountFlashes));
        float flashLeave = _flashDuration / (1f / (3f * amountFlashes));
        while(Time.time < endTime)
        {
            ChangeColor(_spinningColor);
            yield return new WaitForSeconds(flashEnter);
            ChangeColor(whiteFlash);
            yield return new WaitForSeconds(flashLeave);
        }
        ChangeState(newState);
    }
}

public enum TargetState {
    AVAILABLE,
    UNAVAILABLE,
    SPINNING
}
*/