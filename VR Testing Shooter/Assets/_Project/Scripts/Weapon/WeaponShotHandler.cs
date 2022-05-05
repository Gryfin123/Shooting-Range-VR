using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WeaponScript))]
public class WeaponShotHandler : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private AudioClip[] _shotClips;
    [SerializeField] private WeaponScript _weaponScript;

    // Start is called before the first frame update
    void Awake()
    {
        if (_weaponScript == null)
        {
            _weaponScript = gameObject.GetComponent<WeaponScript>();
        }
    }

    private void OnEnable() {
        _weaponScript.ShotFiredSuccessfully += TriggerShotAnimation;
        _weaponScript.ShotFiredSuccessfully += TriggerShotSound;
        _weaponScript.ShotFiredSuccessfully += SpawnShell;
    }

    private void OnDisable() {
        _weaponScript.ShotFiredSuccessfully -= TriggerShotAnimation;
        _weaponScript.ShotFiredSuccessfully -= TriggerShotSound;
        _weaponScript.ShotFiredSuccessfully -= SpawnShell;
    }

    private void TriggerShotAnimation()
    {
        _animator.Play("Fire");
    }

    private void TriggerShotSound()
    {
        AudioClip clipFound = _shotClips[Random.Range(0, _shotClips.Length)];
        float volume = Random.Range(0.75f, 1f);
        AudioSource.PlayClipAtPoint(clipFound, transform.position, volume);
    }

    private void SpawnShell()
    {
        // create a shell and drop it on the ground
    }
}
