using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class TargetController : MonoBehaviour
{

    public TargetState _currState = TargetState.AVAILABLE;

    public Color _availableColor;
    public Color _unavailableColor;

    public float _popupOrder = 0;


    private Renderer _renderer;

    // Start is called before the first frame update
    void Start()
    {
        _renderer = GetComponent<Renderer>();
    }

    private void ChangeState(TargetState newState)
    {
        switch(newState)
        {
            case TargetState.AVAILABLE:
                _renderer.material.color = _availableColor;
            break;
            case TargetState.UNAVAILABLE:
                _renderer.material.color = _unavailableColor;
            break;
        }

        _currState = newState;
    }

    public void OnHit()
    {
        ChangeState(TargetState.UNAVAILABLE);
        StartCoroutine(WaitAndTurnBackOn());
    }

    private IEnumerator WaitAndTurnBackOn()
    {
        yield return new WaitForSeconds(3f);
        ChangeState(TargetState.AVAILABLE);
    }

    private void OnTriggerEnter(Collider other) {
        BulletScript otherScript = other.gameObject.GetComponent<BulletScript>();
        if (otherScript != null)
        {
            OnHit();
        }
    }
}

public enum TargetState {
    AVAILABLE,
    UNAVAILABLE
}