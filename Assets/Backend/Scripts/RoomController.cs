using System;
using System.IO;
using Photon.Pun;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Backend.Scripts
{
    public class RoomController : MonoBehaviourPunCallbacks
    {
        [SerializeField] private PhotonView photonView;
        [SerializeField] private int multiplayerScene;

        private int currentScene;
        
        public static RoomController GameRoomController;

        private void Awake()
        {
            if (RoomController.GameRoomController == null)
            {
                RoomController.GameRoomController = this;
            }
            else
            {
                if (RoomController.GameRoomController != this)
                {
                    Destroy(RoomController.GameRoomController.gameObject);
                    RoomController.GameRoomController = this;
                }
            }
            DontDestroyOnLoad(this.gameObject);
        }

        void Start()
        {
        
        }

        void Update()
        {
        
        }
        
        private void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
        {
            currentScene = scene.buildIndex;
            if (currentScene == multiplayerScene)
            {
                CreatePlayer();
            }
        }

        private void CreatePlayer()
        {
            Debug.Log(Path.Combine("Prefabs", "Player"));
            PhotonNetwork.Instantiate(
                Path.Combine("Prefabs", "Player"),
                transform.position,
                quaternion.identity,
                0
            );
        }

        private void StartGame()
        {
            if (!PhotonNetwork.IsMasterClient)
                return;
            
            PhotonNetwork.LoadLevel(multiplayerScene);
        }

        public override void OnEnable()
        {
            base.OnEnable();
            PhotonNetwork.AddCallbackTarget(this);
            SceneManager.sceneLoaded += OnSceneFinishedLoading;
        }

        public override void OnDisable()
        {
            base.OnDisable();
            PhotonNetwork.RemoveCallbackTarget(this);
            SceneManager.sceneLoaded -= OnSceneFinishedLoading;
        }
        
        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
            Debug.Log("Joined room successfully");
            StartGame();
        }
    }
}
