using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBarController : MonoBehaviour
{
    public Slider prefab;
    
    // Start is called before the first frame update

    private List<Slider> items;
    void Start()
    {
        items = new List<Slider>();
        for (int i = 0; i < GameManager.instance.config.maxStamina; i++)
        {
            items.Add(Instantiate(prefab, transform));
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        float stamina = GameManager.instance.user.stamina;
        int staminaInt = Mathf.FloorToInt(stamina);
        for (int i = 0; i <items.Count; i++)
        {
            Slider item = items[i];
            if (staminaInt > i)
            {
                item.value = 1;
            } else if (staminaInt < i)
            {
                item.value = 0;
            }
            else
            {
                item.value = stamina%staminaInt;
            }
        }
    }
}
