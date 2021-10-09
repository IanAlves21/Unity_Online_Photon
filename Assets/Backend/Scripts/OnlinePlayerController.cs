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
                int index = GameSetupController.GameSetup.spawnPositionArray;
                Debug.Log(index);
                Transform[] positions = GameSetupController.GameSetup.playersSpawnPoints;
                
                mySelectedAvatar = PhotonNetwork.Instantiate(
                    "PlayerCharacter",
                    positions[index].position,
                    positions[index].rotation,
                    0
                );

                GameSetupController.GameSetup.IncreaseSpawnPosition();
            }
        }
    }
}
