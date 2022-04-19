using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qrio
{
	public class Platform : MonoBehaviour
	{
		public float jumpForce = 8f;
        public bool isFragile = false;

        private EdgeCollider2D edgeCol;
        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            edgeCol = GetComponent<EdgeCollider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }


        private void OnCollisionEnter2D(Collision2D collision)
        {
            if(collision.relativeVelocity.y <= 0f)
            {
                Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
                if(rb != null)
                {
                    rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                }

                if(isFragile)
                {
                    edgeCol.enabled = false;
                    spriteRenderer.color = Color.gray;
                }
            }
        }
    }
}