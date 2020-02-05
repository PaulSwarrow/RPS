using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Log : MonoBehaviour
{
    private static Log instance;

    public static Log Instance
    {
        get
        {
            if (!instance)
            {
                instance = FindObjectOfType<Log>();
            }

            return instance;
        }
    }

    public static void m(string message)
    {
        if (Instance.show)
        {
            
            Instance.tf.text += message + "\n";
        }
        else
        {
            instance.tf.text = "";
        }
    }


    public bool show;
    public Text tf;

}
