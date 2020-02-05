using System.Collections;
using UnityEngine;

namespace DefaultNamespace
{
    public class AppManager : MonoBehaviour
    {
        private AppUIController ui;
        private LobbyController connection;
        private GameManager game;

        private void Start()
        {
            game = FindObjectOfType<GameManager>();
            ui = FindObjectOfType<AppUIController>();
            connection = GetComponent<LobbyController>();

            connection.ConnectionReadyEvent += OnLoaded;
            connection.ConnectionStartEvent += OnConnectionStart;
            connection.RoomJoinedEvent += OnConnectedToRoom;
            connection.ConnectionLostEvent += OnGameEnd;
            
            connection.Connect();
        }


        public void StartSoloGame()
        {
            ui.Show(AppUIController.State.Game);
            game.StartSoloGame();
            game.GameEndEvent += OnGameEnd;
        }

        public void HostGame()
        {
            connection.createRoom();
        }
        

        public void JoinGame()
        {
            ui.Show(AppUIController.State.lobby);
        }

        public void GoHome()
        {
            ui.Show(AppUIController.State.Menu);
        }
        

        private void OnLoaded()
        {
            ui.Show(AppUIController.State.Menu);
        }

        private void OnConnectionStart()
        {
            ui.Show(AppUIController.State.Connecting);
        }

        private void OnConnectedToRoom(bool host)
        {
            Log.m("starting the game..");
            if (host)
            {
                ui.Show(AppUIController.State.WaitForOpponent);
            }

            game.GameStartEvent += OnGameStart;
            game.GameEndEvent += OnGameEnd;
            game.GameDisconnectEvent += OnGameEnd;
            game.StartPvPGame(host);
            
           
        }


        private void OnGameStart()
        {
            ui.Show(AppUIController.State.Game);
        }

        private void OnGameEnd()
        {
            game.KillGame();
            game.GameStartEvent -= OnGameStart;
            game.GameEndEvent -= OnGameEnd;
            game.GameDisconnectEvent -= OnGameEnd;
            ui.Show(AppUIController.State.Menu);
        }

        private void OnGameEnd(bool winner)
        {
            game.KillGame();
            game.GameStartEvent -= OnGameStart;
            game.GameEndEvent -= OnGameEnd;
            game.GameDisconnectEvent -= OnGameEnd;

            StartCoroutine(GameResult(winner));

        }

        private IEnumerator GameResult(bool winner)
        {
            ui.Show(winner ? AppUIController.State.Win : AppUIController.State.loose);
            yield return new WaitForSeconds(3);
            ui.Show(AppUIController.State.Menu);
        }
    }
}