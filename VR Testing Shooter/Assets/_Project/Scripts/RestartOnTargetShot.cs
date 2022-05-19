using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartOnTargetShot : MonoBehaviour
{
    [SerializeField] TargetController _tc;
    [SerializeField] GenerateTargetRange _gtr;
    // Start is called before the first frame update
    void OnEnable()
    {
        _tc.GotHit += Restart;
    }
    private void OnDisable()
    {
        _tc.GotHit -= Restart;
    }

    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        // _gtr.ClearTargets();
        // _gtr.GenerateAtPosition();
    }
}
