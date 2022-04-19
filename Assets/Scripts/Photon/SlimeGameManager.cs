using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Qrio
{
    public class SlimeGameManager : MonoBehaviourPunCallbacks
    {
        public GameObject playerPrefab;
        public GameObject[] platformPrefab;
        public GameObject finishPrefab;
        public PlayerTop top;

        public List<SlimeController> players = new List<SlimeController>();

        public int platformCount = 150;

        public void AddPlayer(SlimeController player)
        {
            players.Add(player);
        }

        void Awake()
        {
            
        }

        void Start()
		{
            Vector2 spawnPos = new Vector2();

            for(int i = 0; i < platformCount; i++)
            {
                spawnPos.y += Random.Range(1f, 2.5f);
                spawnPos.x = Random.Range(-5f, 5f);

                float j = Random.Range(0f, 1.5f);
                if (j <= 0.9f)
                    Instantiate(platformPrefab[0], spawnPos, Quaternion.identity);
                else if (j <= 1.2f)
                    Instantiate(platformPrefab[1], spawnPos, Quaternion.identity);
                else
                    Instantiate(platformPrefab[2], spawnPos, Quaternion.identity);

            }

            spawnPos.y += 3;
            for (int j = 0; j < 5; j++)
            {
                if (j % 2 == 0)
                    spawnPos.x = j;
                else
                    spawnPos.x = -j - 1;
                Instantiate(finishPrefab, spawnPos, Quaternion.identity);
            }

            for (int i = 0; i < 6; i++)
            {
                spawnPos.y = i * 50;
                for (int j = 0; j < 5; j++)
                {
                    if (j % 2 == 0)
                        spawnPos.x = j;
                    else
                        spawnPos.x = -j -1;
                    Instantiate(platformPrefab[0], spawnPos, Quaternion.identity);
                }

            }

            Vector3 pos = new Vector3(0, 1);
            PhotonNetwork.Instantiate(playerPrefab.name, pos, Quaternion.identity);
		}

		void Update()
		{
            top.SetTexts(players);
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