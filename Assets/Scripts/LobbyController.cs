using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LobbyController : MonoBehaviour
{
    public string versionName = "0.1";


    public event Action ConnectionReadyEvent;
    public event Action ConnectionStartEvent;
    public event Action<bool> RoomJoinedEvent;
    public event Action ConnectionLostEvent;

    // Start is called before the first frame update
    public void Connect()
    {
        Log.m("connect to photon...");

        connectToPhoton();

    }


    public void connectToPhoton()
    {
        PhotonNetwork.ConnectUsingSettings(versionName);

        Log.m("connecting to photon and creating room...");
    }

    public void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby(TypedLobby.Default);
        Log.m("connected to master");
    }

    public void OnJoinedLobby()
    {
        Log.m("connected to lobby");
        ConnectionReadyEvent?.Invoke();
    }

    public void OnDisconnectedFromPhoton()
    {
        Debug.Log("disconnected");
        ConnectionLostEvent?.Invoke();
    }


    public void createRoom()
    {
        string name = GenerateRoomNane(6);
        PhotonNetwork.CreateRoom(name, new RoomOptions() {MaxPlayers = 2}, null);
        Log.m("create room");
        ConnectionStartEvent?.Invoke();
    }

    public void JoinRoom(string name)
    {
        //TODO check existence
        PhotonNetwork.JoinRoom(name);
        Log.m("joined room");
        ConnectionStartEvent?.Invoke();
    }


    public void OnJoinedRoom()
    {
        Log.m("Joined room: " + PhotonNetwork.room.Name );
        RoomJoinedEvent?.Invoke(PhotonNetwork.isMasterClient);


    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
    
    public static string GenerateRoomNane(int length) 
    {     
        return Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, length);
    } 
}