using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FrameRateLimit : MonoBehaviour
{
    [SerializeField] private int frameRateLimit = 60;
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = frameRateLimit;
    }
}
