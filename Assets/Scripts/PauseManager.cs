using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Experimental.Input;

public class PauseManager : MonoBehaviour
{
    public InputMaster Controls;
    public static PauseManager Instance;
    public Text pauseText;
    public bool _isPaused;
    // Start is called before the first frame update
    void Awake()
    {
        Controls.Player.Pause.performed += ctx => TogglePause();
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void TogglePause()
    {
        _isPaused = !_isPaused;
        pauseText.enabled = _isPaused;
        Time.timeScale = _isPaused ? 0 : 1;
    }
}
