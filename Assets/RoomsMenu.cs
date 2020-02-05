using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomsMenu : MonoBehaviour
{
    
    private List<Button> btns = new List<Button>();
    private RoomInfo[] rooms;
    [SerializeField] private Button btnPrefab;

    private Coroutine coroutine;

    private List<Button> pool = new List<Button>();
    private LobbyController connection;

    // Start is called before the first frame update
    void Awake()
    {
        connection = FindObjectOfType<LobbyController>();
    }

    private void OnEnable()
    {
        coroutine = StartCoroutine(UpdateList());
    }

    private void OnDisable()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
    }

    private IEnumerator UpdateList()
    {
        while (true)
        {
            drawList();
            yield return new WaitForSeconds(1);
        }
    }

    private void drawList()
    {
        foreach (Button btn in btns)
        {
            RemoveBtn(btn);
        }

        btns.Clear();

        if (PhotonNetwork.insideLobby)
        {
            rooms = PhotonNetwork.GetRoomList();

            foreach (RoomInfo room in rooms)
            {
                if (room.PlayerCount == 1)
                {
                    Button btn = createButton();
                    btn.onClick.AddListener(() => joinRoom(room.Name));
                    btn.GetComponentInChildren<Text>().text = room.Name;
                    btns.Add(btn);
                }
            }
        }
    }

    private Button createButton()
    {
        Button btn;
        if (pool.Count > 0)
        {
            btn = pool[0];
            btn.gameObject.SetActive(true);
        }
        else
        {
            btn = Instantiate(btnPrefab, transform);
        }

        return btn;
    }

    private void RemoveBtn(Button btn)
    {
        btn.onClick.RemoveAllListeners();
        btn.gameObject.SetActive(false);
        pool.Add(btn);
    }

    private void joinRoom(string name)
    {
        connection.JoinRoom(name);
    }
}