using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Qrio
{
    public class SlimeController : MonoBehaviour, IPunObservable
	{

		public float moveSpeed = 10f;
		public float maxHeight;
		public bool isDead = false;
		public bool hasFinished = false;

		public Camera mainCamera;
		public GameObject background;
		public GameObject wallsPrefab;
		public GameObject spikePrefab;
		public GameObject deadPlayerPrefab;

		private GameObject spikeWall;
		private GameObject sideWalls;

		public GameObject gameManager;
		public PhotonView photonView;
		public TextMeshPro nicknameBar;

		private Rigidbody2D rb;
		private SpriteRenderer spriteRenderer;
		private BoxCollider2D box;

		private float moveX;
        private float spikeWallSpeed = 0.25f;

        void Awake()
		{
			
			photonView = GetComponent<PhotonView>();
			rb = GetComponent<Rigidbody2D>();
			spriteRenderer = GetComponent<SpriteRenderer>();
			box = GetComponent<BoxCollider2D>();
			gameManager = GameObject.FindGameObjectWithTag("GameManager");

			mainCamera = Camera.main;
		}

        void Start()
        {
            if (photonView.IsMine)
            {
				Vector2 spikesPos = new Vector2(transform.position.x, transform.position.y - 6);
				spikeWall = Instantiate(spikePrefab, spikesPos, Quaternion.identity);
				sideWalls = Instantiate(wallsPrefab, transform.position, Quaternion.identity);
            }

			gameManager.GetComponent<SlimeGameManager>().AddPlayer(this);
		}

        void Update()
		{
			//Input movement, spikes and walls movement.
			if (photonView.IsMine && !isDead)
            {
				moveX = Input.GetAxis("Horizontal");

				float dist = transform.position.y - spikeWall.transform.position.y;
				if (dist > 6)
					spikeWall.transform.position += new Vector3(0, spikeWallSpeed * (dist / 2) * Time.deltaTime, 0);

				spikeWall.transform.position += new Vector3(0, spikeWallSpeed * Time.deltaTime, 0);
				sideWalls.transform.position = new Vector2(sideWalls.transform.position.x, transform.position.y);
            }
            if (!photonView.IsMine && isDead)
            {
				spriteRenderer.sprite = null;
			}

			nicknameBar.SetText(photonView.Owner.NickName);

			if (maxHeight < transform.position.y)
				maxHeight = transform.position.y;
		}

        private void FixedUpdate()
        {
			//Control logic
			if (photonView.IsMine && !isDead)
				rb.velocity = new Vector2(moveX * moveSpeed * Time.fixedDeltaTime, rb.velocity.y);

			//Should teleport to the other side
			if (transform.position.x > 4.8f)
				transform.position = new Vector2(-4.7f, transform.position.y);
			else if (transform.position.x < -4.8f)
				transform.position = new Vector2(4.7f, transform.position.y);
		}

        private void LateUpdate()
        {
			//Turn gray if dead and return
			if(isDead)
				Kill();

			//Move camera and BG if it is your Character. Else - turn it red
			if (photonView.IsMine)
            {
				mainCamera.transform.position = new Vector3(transform.position.x, transform.position.y, mainCamera.transform.position.z);
				background.transform.position = new Vector3(transform.position.x, transform.position.y, background.transform.position.z);
			}
            else
            {
				spriteRenderer.color = Color.red;
				nicknameBar.color = Color.red;
			}
        }

        public void Kill()
        {
			if(spriteRenderer.sprite != null)
            {
				PhotonNetwork.Instantiate(deadPlayerPrefab.name, transform.position, Quaternion.identity);
				isDead = true;

				rb.gravityScale = 0;

				spriteRenderer.sprite = null;
				if (hasFinished)
					nicknameBar.color = Color.magenta;
				else
					nicknameBar.color = Color.gray;
            }
		}

		private void OnCollisionEnter2D(Collision2D collision)
        {
			//Kill Character on touching spikes
			if (photonView.IsMine && !isDead && collision.gameObject.CompareTag("KillOnTouch"))
            {
				Kill();
            }
			//Ignore player collisions
			if (collision.gameObject.CompareTag("Player"))
				Physics2D.IgnoreCollision(collision.collider, box);
			if (collision.gameObject.CompareTag("Finish"))
            {
				hasFinished = true;
				Kill();
            }
				
        }

		//Used to sync variables over the network
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
			if (stream.IsWriting)
			{
				stream.SendNext(isDead);
				stream.SendNext(hasFinished);
			}
			else
			{
				isDead = (bool)stream.ReceiveNext();
				hasFinished = (bool)stream.ReceiveNext();
			}
		}

    }
}