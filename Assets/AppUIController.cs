using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils.Dictionary;

public class AppUIController : MonoBehaviour
{
    [Serializable]
    public class ScreenMap : SerializableDictionary<State, RectTransform>
    {
        
    }
    // Start is called before the first frame update
    public enum State
    {
        loading, 
        Menu, 
        Connecting, 
        Game,
        lobby,
        loose,
        Win,
        WaitForOpponent
    }

    [SerializeField]
    private ScreenMap screens;
    private State current;

    private void Awake()
    {
        foreach (KeyValuePair<State,RectTransform> screen in screens)
        {
            screen.Value.gameObject.SetActive(false);
        }

        screens[State.loading].gameObject.SetActive(true);
    }

    public void Show(State state)
    {
        if (current != state)
        {
            screens[current].gameObject.SetActive(false);
            current = state;
            screens[current].gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
