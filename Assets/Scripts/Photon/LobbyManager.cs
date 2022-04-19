using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Qrio
{
	public class LobbyManager : MonoBehaviourPunCallbacks
	{
		public TextMeshProUGUI logText;
		public TextMeshProUGUI hostName;
		public TextMeshProUGUI playersCounter;
		public TMP_InputField inputName;

		public GameObject btnsParent;
		public GameObject[] btns;
		public GameObject startbtn;

		void Start()
		{
			btns = new GameObject[btnsParent.transform.childCount];
			for (int i = 0; i < btns.Length; i++)
				btns[i] = btnsParent.transform.GetChild(i).gameObject;


            string nickName = "Player" + Random.Range(1000, 9999);
            PhotonNetwork.NickName = nickName;
			inputName.text = nickName;
			if (PlayerPrefs.GetString("NickName") != "")
            {
				PhotonNetwork.NickName = PlayerPrefs.GetString("NickName");
				inputName.text = PlayerPrefs.GetString("NickName");
            }

			Log("Player's name is set to " + PhotonNetwork.NickName);
			if (!PhotonNetwork.IsConnected)
            {
				Log("Connecting to Master. Please wait");
				btns[0].gameObject.GetComponent<Button>().interactable = false;
				btns[1].gameObject.GetComponent<Button>().interactable = false;
			}

			PhotonNetwork.AutomaticallySyncScene = true;
			PhotonNetwork.GameVersion = "1";
			PhotonNetwork.ConnectUsingSettings();


		}

		public override void OnConnectedToMaster()
		{
			Log("Connected to Master");
			btns[0].gameObject.GetComponent<Button>().interactable = true;
			btns[1].gameObject.GetComponent<Button>().interactable = true;
		}

		public void CreateRoom()
		{
			PhotonNetwork.NickName = inputName.text;
			PlayerPrefs.SetString("NickName", inputName.text);
			if (PhotonNetwork.NickName != "")
				PhotonNetwork.CreateRoom(null, new Photon.Realtime.RoomOptions { MaxPlayers = 20 });
			else
				Log("Please enter valid nickname");
		}

		public void JoinRoom()
		{
			PhotonNetwork.NickName = inputName.text;
			PlayerPrefs.SetString("NickName", inputName.text);
			if (PhotonNetwork.NickName != "")
				PhotonNetwork.JoinRandomRoom();
			else
				Log("Please enter valid nickname");
		}

		public override void OnJoinedRoom()
		{
			Log("Joined the room");


			btns[0].SetActive(false);
			btns[1].SetActive(false);
			btns[4].SetActive(false);
			btns[2].SetActive(true);
			btns[3].SetActive(true);
			if (!PhotonNetwork.IsMasterClient)
            {
				startbtn.SetActive(false);
            }

			hostName.text = "Host name: " + PhotonNetwork.MasterClient.NickName;
			playersCounter.text = $"Players: {PhotonNetwork.CurrentRoom.PlayerCount} / 20";
		}
		

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
			playersCounter.text = $"Players: {PhotonNetwork.CurrentRoom.PlayerCount} / 20";
			Log($"{newPlayer.NickName} has joined");
		}

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
			if (PhotonNetwork.IsMasterClient)
				startbtn.SetActive(true);

			Log($"{otherPlayer.NickName} has left");

			hostName.text = "Host name: " + PhotonNetwork.MasterClient.NickName;
			playersCounter.text = $"Players: {PhotonNetwork.CurrentRoom.PlayerCount} / 20";
		}

        public void Leave()
        {
			btns[0].SetActive(true);
			btns[1].SetActive(true);
			btns[4].SetActive(true);
			btns[2].SetActive(false);
			btns[3].SetActive(false);

			hostName.text = "Host name: ";
			playersCounter.text = "Players: 0 / 20";
			logText.text = "";
			Log("Player's name is set to " + PhotonNetwork.NickName);

			PhotonNetwork.LeaveRoom();
        }

		public void StartGame()
        {
			PhotonNetwork.LoadLevel("Game");
		}

        private void Log(string msg)
        {
			Debug.Log(msg);
			logText.text += "\n";
			logText.text += msg;
        }
    }
}