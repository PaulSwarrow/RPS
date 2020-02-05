using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomInfoView : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        if (PhotonNetwork.room != null)
        {
            GetComponent<Text>().text = PhotonNetwork.room.Name;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
