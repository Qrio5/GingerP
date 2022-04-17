using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qrio
{
	public class SlimeWall : MonoBehaviour
	{
		public bool isLeft = false;
		public SlimeWall otherWall;

        public Transform exit;
        public bool onCooldown;

        void Awake()
		{
            exit = transform.parent.GetChild(0);
		}

		void Update()
		{
			
		}

        private void Teleport(GameObject gameObject)
        {
            gameObject.transform.position = otherWall.exit.position;
            otherWall.onCooldown = true;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.gameObject.CompareTag("Player") && !onCooldown)
            {
				Teleport(collision.gameObject);
            }
        }


        private void OnTriggerExit2D(Collider2D collision)
        {
            if(collision.gameObject.CompareTag("Player"))
                onCooldown = false;
        }
    }
}