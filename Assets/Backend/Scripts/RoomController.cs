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
        [SerializeField] private GameObject onlinePlayerReference;
        [SerializeField] private PhotonView photonView;
        [SerializeField] private int multiplayerScene;

        private int currentScene;
        private DefaultPool poolPrefab;
        
        public static RoomController GameRoomController;

        private void Awake()
        {
            poolPrefab = PhotonNetwork.PrefabPool as DefaultPool;
            poolPrefab.ResourceCache.Add(onlinePlayerReference.name, onlinePlayerReference);
            
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
            PhotonNetwork.Instantiate(
                onlinePlayerReference.name,
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
