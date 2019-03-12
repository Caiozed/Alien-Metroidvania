using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewPowerPickup : MonoBehaviour
{
    // Start is called before the first frame update
    CircleCollider2D _circleCollider2D;
    public GameObject PickupEffect;
    public PlayerController.PlayerPower PowerToEnable;
    void Start()
    {
        _circleCollider2D = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().EnablePower(this.PowerToEnable);
            Instantiate(PickupEffect, transform.position, transform.rotation);
            GameObject.FindGameObjectWithTag("UIText").GetComponent<Text>().text = "-------------New Module Online---------------";
            GameObject.FindGameObjectWithTag("UIAnimator").GetComponent<Animator>().SetTrigger("TextBlink");
            Destroy(gameObject);
        }
    }
}
