using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LevelEndController : MonoBehaviour
{
    // Start is called before the first frame update
    Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.CompareTag("Player"))
        {
            other.gameObject.SetActive(false);
            anim.SetTrigger("End");
            GameObject.FindGameObjectWithTag("UIText").GetComponent<Text>().text = "-------------Scaped!---------------";
            GameObject.FindGameObjectWithTag("UIAnimator").GetComponent<Animator>().SetTrigger("GameEnd");
            GameObject.Find("Sprites-Ghosts").SetActive(false);
        }
    }
}
