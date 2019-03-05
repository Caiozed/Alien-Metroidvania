using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class TriggerActivator : MonoBehaviour
{
    public GameObject ObjectRef;
    public UnityEvent OnTrigger;
    public bool Activate;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.CompareTag("Player"))
        {
            OnTrigger.Invoke();
            gameObject.SetActive(false);
        }
    }
}
