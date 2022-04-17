using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qrio
{
    public class SlimeController : MonoBehaviour, IPunObservable
	{
		private PhotonView photonView;
		private Rigidbody2D rb;
		private SpriteRenderer spriteRenderer;
		private BoxCollider2D box;

		public float moveSpeed = 10f;
		public bool isDead = false;

		public Camera mainCamera;
		public GameObject background;
		public GameObject wallsPrefab;
		public GameObject spikePrefab;
		public GameObject deadPlayerPrefab;

		private GameObject spikeWall;
		private GameObject sideWalls;

		private float moveX;


		void Awake()
		{
			photonView = GetComponent<PhotonView>();
			rb = GetComponent<Rigidbody2D>();
			spriteRenderer = GetComponent<SpriteRenderer>();
			box = GetComponent<BoxCollider2D>();

			mainCamera = Camera.main;
		}

        void Start()
        {
			Vector2 spikesPos = new Vector2(transform.position.x, transform.position.y - 6);
			spikeWall = Instantiate(spikePrefab, spikesPos, Quaternion.identity);
			sideWalls = Instantiate(wallsPrefab, transform.position, Quaternion.identity);
		}

        void Update()
		{
			if (photonView.IsMine && !isDead)
            {
				moveX = Input.GetAxis("Horizontal");
				if (transform.position.y - spikeWall.transform.position.y > 6)
					spikeWall.transform.position = new Vector2(spikeWall.transform.position.x, transform.position.y - 6);

				sideWalls.transform.position = new Vector2(sideWalls.transform.position.x, transform.position.y);
            }
            if (!photonView.IsMine && isDead)
            {
				spriteRenderer.sprite = null;
			}


		}

        private void FixedUpdate()
        {
			if (photonView.IsMine && !isDead)
				rb.velocity = new Vector2(moveX * moveSpeed * Time.fixedDeltaTime, rb.velocity.y);
			if (transform.position.x > 4.8f)
				transform.position = new Vector2(-4.7f, transform.position.y);
			else if (transform.position.x < -4.8f)
				transform.position = new Vector2(4.7f, transform.position.y);
		}

        private void LateUpdate()
        {
			if (isDead)
            {
				spriteRenderer.color = Color.gray;
				return;
            }

			if (photonView.IsMine)
            {
				mainCamera.transform.position = new Vector3(transform.position.x, transform.position.y, mainCamera.transform.position.z);
				background.transform.position = new Vector3(transform.position.x, transform.position.y, background.transform.position.z);
			}
            else
            {
				spriteRenderer.color = Color.red;
			}
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {

			if (photonView.IsMine && !isDead && collision.gameObject.CompareTag("KillOnTouch"))
            {
				PhotonNetwork.Instantiate(deadPlayerPrefab.name, transform.position, Quaternion.identity);
				isDead = true;
				spriteRenderer.sprite = null;
            }
			if (collision.gameObject.CompareTag("Player"))
				Physics2D.IgnoreCollision(collision.collider, box);
        }


        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
			if (stream.IsWriting)
			{
				stream.SendNext(isDead);
			}
			else
			{
				isDead = (bool)stream.ReceiveNext();
			}
		}

    }
}