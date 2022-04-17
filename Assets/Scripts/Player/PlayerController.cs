using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qrio
{
	public class PlayerController : MonoBehaviour
	{
		private PhotonView photonView;

		public Rigidbody2D rb;
        public float speed = 2000;

		public Vector2 inputMovement;

        void Start()
		{
			photonView = GetComponent<PhotonView>();
			rb = GetComponent<Rigidbody2D>();
		}

		void Update()
		{
			if (photonView.IsMine)
            {
				inputMovement = new Vector2(Input.GetAxis("Horizontal"), 0);
				rb.velocity = new Vector2(inputMovement.x * speed * Time.deltaTime, rb.velocity.y);

            }
		}
	}
}