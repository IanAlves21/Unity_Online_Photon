using System;
using Photon.Pun;
using Unity.Mathematics;
using UnityEngine;

namespace Backend.Scripts
{
    public class OnlinePlayerController : MonoBehaviour
    {
        [SerializeField] private PhotonView photonView;
        [SerializeField] private GameObject mySelectedAvatar;
        
        void Start()
        {
            if (photonView.IsMine)
            {
                int index = RoomController.GameRoomController.spawnPositionArray;
                Transform[] positions = GameSetupController.GameSetup.playersSpawnPoints;

                RoomController.GameRoomController.spawnPositionArray += 1;
                
                mySelectedAvatar = PhotonNetwork.Instantiate(
                    "PlayerCharacter",
                    positions[index].position,
                    positions[index].rotation,
                    0
                );
            }
        }
    }
}
