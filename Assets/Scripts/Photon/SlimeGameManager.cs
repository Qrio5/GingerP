using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Qrio
{
	public class SlimeGameManager : MonoBehaviourPunCallbacks
	{
        public GameObject playerPrefab;
        public GameObject platformPrefab;

        public int platformCount = 300;

		void Start()
		{
            Vector2 spawnPos = new Vector2();

            for(int i = 0; i < platformCount; i++)
            {
                spawnPos.y += Random.Range(1f, 3f);
                spawnPos.x = Random.Range(-5f, 5f);
                Instantiate(platformPrefab, spawnPos, Quaternion.identity);
            }

            Vector3 pos = new Vector3(0, 1);
            PhotonNetwork.Instantiate(playerPrefab.name, pos, Quaternion.identity);
		}

		void Update()
		{
			
		}

		public void Leave()
        {
            PhotonNetwork.LeaveRoom();
        }

        public override void OnLeftRoom()
        {
			SceneManager.LoadScene(0);
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
			Debug.LogFormat("{0} entered the room", newPlayer.NickName);
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            Debug.LogFormat("{0} left the room", otherPlayer.NickName);
        }
    }
}