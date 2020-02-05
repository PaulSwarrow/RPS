using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyCounter : MonoBehaviour
{
    private Text tf;
    // Start is called before the first frame update
    void Start()
    {
        tf = GetComponentInChildren<Text>();

    }

    // Update is called once per frame
    void Update()
    {
        tf.text = Mathf.FloorToInt(GameManager.instance.user.stamina).ToString();
    }
}
