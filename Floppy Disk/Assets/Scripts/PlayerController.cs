using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]         // Make sure the player has a Rigidbody2D
public class PlayerController : MonoBehaviour
{

    public float jump_force = 10;
    public float move_smooth = 5;
    public Vector3 start_position;

    Rigidbody2D rb;
    Quaternion down_rotation;
    Quaternion up_rotation;
    bool playing = false;

	// Use this for initialization
	void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
        down_rotation = Quaternion.Euler(0, 0, 0);
        up_rotation = Quaternion.Euler(0, 0, 130);

    }
	
	// Update is called once per frame
	void Update ()
    {
        if (playing)
        {
            // Mouse Input is automaticaly translated to tap on phone devices
            if (Input.GetMouseButtonDown(0))
            {
                transform.rotation = up_rotation;
                rb.velocity = Vector3.zero;
                rb.AddForce(Vector2.up * jump_force, ForceMode2D.Force);
            }

            if (rb.velocity.y <= -3)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, down_rotation, move_smooth * Time.deltaTime);
            }
        }
	}

    public void SetPlaying(bool b)
    {
        playing = b;
        rb.simulated = true;
    }

    public void ResetPlayer()
    {
        transform.position = start_position;
        playing = false;
        rb.simulated = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Score"))
        {
            GameManager.Instance.AddToScore();
        }

        if (collision.CompareTag("Obstacle"))
        {
            rb.simulated = false;
            GameManager.Instance.SetGameState(GameManager.StateType.Gameover);
        }

    }

}
