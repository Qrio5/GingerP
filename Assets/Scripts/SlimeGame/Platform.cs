using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qrio
{
	public class Platform : MonoBehaviour
	{
		public float jumpForce = 8f;


        private void OnCollisionEnter2D(Collision2D collision)
        {
            if(collision.relativeVelocity.y <= 0f)
            {
                Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
                if(rb != null)
                {
                    rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                }
            }
        }
    }
}