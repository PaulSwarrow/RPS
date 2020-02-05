using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;

public class Lifebar : MonoBehaviour
{
    public bool user;
    // Start is called before the first frame update
    private Slider slider;
    private Animator animator;
    private Text tf;
    void Start()
    {
        tf = GetComponentInChildren<Text>();
        animator = GetComponent<Animator>();
        slider = GetComponent<Slider>();
        GameManager.instance.HitEvent += OnHit;
    }

    private void OnHit(PlayerProfile target)
    {
        if (target == player)
        {
            animator.SetTrigger("trigger");
        }
    }

    // Update is called once per frame
    void Update()
    {
        float health = player.health;
        slider.value = health / GameManager.instance.config.health;
        tf.text = Mathf.FloorToInt(Math.Max(health, 0)).ToString();
    }

    private PlayerProfile player => (user ? GameManager.instance.user : GameManager.instance.opponent);
}
