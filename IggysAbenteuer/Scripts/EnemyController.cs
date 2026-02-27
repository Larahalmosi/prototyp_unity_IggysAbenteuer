//source:https://www.youtube.com/watch?v=Pkc4A1ukbJU&t=5266s
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 3f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Rigidbody2D rb;
    private bool isGrounded;
    private bool shouldJump;
    void Start()
    {
        rb =GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
         transform.Translate (Vector2.left*Time.deltaTime*moveSpeed);
         // Player Richtung 
         float direction = Mathf.Sign(player.position.x-player.position.x);

         // Player über einen erkennen
         bool isPlayerAbove= Physics2D.Raycast(transform.position, Vector2.up, 3f, 1 << player.gameObject.layer);
        
        if(isGrounded){
         //Player verfolgen
         rb.linearVelocity = new Vector2(direction*moveSpeed, rb.linearVelocity.y);
        }
    }

    private void FixedUpdate(){
        if (isGrounded){
        Vector2 direction= (player.position -transform.position).normalized;
    }
    }
            
}
