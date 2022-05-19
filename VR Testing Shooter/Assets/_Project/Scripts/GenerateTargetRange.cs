using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateTargetRange : MonoBehaviour
{ 

    [Header("Building Blocks")]
    [SerializeField] Texture2D[] _images;
    [SerializeField] GameObject _targetPrefab;
    [SerializeField] Transform _generalSpawnPosition;
    [SerializeField] float _spaceBetweenCenter = 1; // To put objects apart from eachother

    [Header("Processing")]
    [SerializeField] int _maxTargetsAtOnce;
    TargetController[] _spawnedTargets;
    int _targetsLeft = 0;
    decimal _currOrderPlace = 256256.256M;


    Texture2D _currImage; 
    
    private void Start() {
        GenerateAtPosition();
        EvaluateAvailableTargets();
    }

    public void GenerateAtPosition()
    {
        // Image to pixels
        _currImage = _images[UnityEngine.Random.Range(0, _images.Length)];
        Color[] pixs = _currImage.GetPixels();

        // Spawner setup
        int worldX = _currImage.width;
        int worldY = _currImage.height;

        Vector3[] spawnPosition = new Vector3[pixs.Length];
        Vector3 startSpawnPosition = _generalSpawnPosition.position + new Vector3(-Mathf.Round(worldX/2), 0, 0);
        Vector3 currSpawnPosition = startSpawnPosition; 
        
        _spawnedTargets = new TargetController[pixs.Length];

        // Setup position
        int counter = 0;
        for (int y = 0; y < worldY; y++)
        {
            for (int x = 0; x < worldX; x++)
            {
                spawnPosition[counter] = currSpawnPosition;
                currSpawnPosition.x += _spaceBetweenCenter;
                counter++;
            }

            currSpawnPosition.x = startSpawnPosition.x;
            currSpawnPosition.y += _spaceBetweenCenter;
        }

        // Spawn Targets
        _targetsLeft = 0;
        foreach(Vector3 pos in spawnPosition)
        {
            Color c = pixs[_targetsLeft];
            GameObject created = Instantiate(_targetPrefab, pos, Quaternion.identity);
            
            _spawnedTargets[_targetsLeft] = created.GetComponentInChildren<TargetController>();
            _spawnedTargets[_targetsLeft]._popupOrder = Math.Round((decimal)c.r * 256) * 1000 + Math.Round((decimal)c.g * 256) + Math.Round((decimal)c.b * 256) / 1000;
            _spawnedTargets[_targetsLeft].GotHit += OnTargetShot;
            _spawnedTargets[_targetsLeft].transform.root.parent = transform;
            
            _targetsLeft++;
        }

        // Order list of target controllers (descending by popup order)
        Array.Sort(_spawnedTargets, (x, y) => y._popupOrder.CompareTo(x._popupOrder));
        int extraCoutner = 0;
        foreach(TargetController curr in _spawnedTargets)
        {
            Debug.Log(extraCoutner + ": " + curr._popupOrder);
            extraCoutner++;
        }
    }

    private int GetCurrentAvailableTargets()
    {
        int counter = 0;

        foreach(TargetController curr in _spawnedTargets)
        {
            if (curr.CurrState == TargetState.AVAILABLE ||
                curr.CurrState == TargetState.SPINNING_ON)
            {
                counter++;   
            }
        }
        return counter;
    }

    private void OnTargetShot()
    {
        _targetsLeft--;
        EvaluateAvailableTargets();
    }

    private void EvaluateAvailableTargets()
    {
        if (_targetsLeft >= _maxTargetsAtOnce)
        {
            int counter = 0;
            
            while (GetCurrentAvailableTargets() < _maxTargetsAtOnce)
            {
                UnlockNextTarget();
                if (counter == 1000)
                {
                    Debug.Log("max loops in Evaluate Availalbe targets reached. Something is broken here.");
                    break;
                }
                else
                {
                    counter++;
                }

            }
        }
    }

    private void UnlockNextTarget()
    {
        Debug.Log("Ulnoclking...");
        foreach(TargetController nextTarget in _spawnedTargets)
        {
            if (nextTarget._popupOrder <= _currOrderPlace &&
                nextTarget.CurrState == TargetState.UNAVAILABLE)
            {
                Debug.Log("Unlocked!");
                nextTarget.ToAvailable();
                _currOrderPlace = nextTarget._popupOrder;
                break;
            }
        }
    }

    public void ClearTargets()
    {
        foreach(TargetController curr in _spawnedTargets)
        {
            Destroy(curr.gameObject.transform.root.parent);
        }
    }

    public void Restart()
    {
        ClearTargets();
        GenerateAtPosition();
    }
}
