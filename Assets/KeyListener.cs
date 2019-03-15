using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Experimental.Input;
public class KeyListener : MonoBehaviour
{
    // Start is called before the first frame update
    public UnityEvent OnAnimationEnd;
    Text timerText;
    float timer = 5;
    bool _started;
    public GameObject[] ObjectsToActivateAfterAnim;
    void Start()
    {
        timerText = GetComponent<Text>();
    }
    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        timer = Mathf.Clamp(timer, 0, 10);
        timerText.text = "It starts in..." + Mathf.Floor(timer);
        if (!_started && timer <= 0)
        {
            OnAnimationEnd.Invoke();
            foreach (var item in ObjectsToActivateAfterAnim)
            {
                item.SetActive(true);
            }
            _started = true;
        }
    }
}
