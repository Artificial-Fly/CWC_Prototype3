using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    private PlayerController playerControllerScript;
    //
    float leftBound = -5;
    float speed = 5;
    // Start is called before the first frame update
    void Start()
    {
       playerControllerScript = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        //Move Left
        if(!(playerControllerScript.gameOver)){
           transform.Translate(Vector3.left*Time.deltaTime * speed);//Moove current game object by changing position 
        }
        //Destroy obstacle if it is out of bounds
        if(((transform.position.x < leftBound)||(transform.position.y < leftBound))&& gameObject.CompareTag("Obstacle")){
            Destroy(gameObject);
        }
    }
}
