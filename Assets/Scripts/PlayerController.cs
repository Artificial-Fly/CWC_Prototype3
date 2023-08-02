using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public AudioClip jumpSound, crashSound;
    private AudioSource playerAudio;
    //
    private Rigidbody playerRb;
    //
    private Animator playerAnim;
    //
    public ParticleSystem explosionParticle;
    public ParticleSystem fallParticle;
    public ParticleSystem dirtParticle;
    //
    float jumpForce = 100.5f, gravityModifier = 1.5f; 
    private bool isOnGround = true;
    //https://discussions.unity.com/t/how-to-get-time-of-key-held-down/120288
    float jkDownTime, jkUpTime, jkPressTime=0;
    float Countdown =2.1f;
    bool jkReady = false;
    float jumpForceMod = 1;
    float jumpStage = 0.6f;
    //
    public bool gameOver = false;
    //
    bool jumpThreeFall = false;
    // Start is called before the first frame update
    void Start()
    {
        playerAudio = GetComponent<AudioSource>();
        playerRb = GetComponent<Rigidbody>();//get component Rigidbody from current game object
        playerAnim = GetComponent<Animator>();
        Physics.gravity *= gravityModifier;
        //playerRb.AddForce(Vector3.up*1000 );// move current game object by using rigid body (by applying directed force)
        InvokeRepeating("StagesAlert", 0, jumpStage-0.5f);
    }

    // Update is called once per frame
    void Update()
    {    
        if(!gameOver){
            Jump();
        }
    }
    void Jump(){
        if(isOnGround){
            if(Input.GetKeyDown(KeyCode.Space)&&jkReady==false){
                jkDownTime = Time.time; 
                jkPressTime = jkDownTime + Countdown;
                jkReady = true;
            }        
            if((Input.GetKeyUp(KeyCode.Space))||((Time.time>=jkPressTime)&&jkReady)){
                jkUpTime = Time.time;
                if(jkUpTime-jkDownTime>3*jumpStage){Debug.Log("Jump3!");jumpThreeFall=true;jumpForceMod=jumpForceMod+7.5f*(1+jumpStage);}else if(jkUpTime-jkDownTime>2*jumpStage){Debug.Log("Jump2!");jumpForceMod=jumpForceMod+5*(1+jumpStage);}else{Debug.Log("Jump1!");jumpForceMod=jumpForceMod+3.5f*(1+jumpStage);}
                playerRb.AddForce(Vector3.up*jumpForce*(jumpForceMod), ForceMode.Impulse/*<-Type of force application mode*/);
                playerAudio.PlayOneShot(jumpSound, 1.0f);
                playerAnim.SetTrigger("Jump_trig");
                jkReady = false;
                jumpForceMod=1;
                isOnGround = false;
                playerAnim.SetBool("Grounded", false);
                dirtParticle.Stop();
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
       if(collision.gameObject.CompareTag("Ground")){
            isOnGround = true;
            playerAnim.SetBool("Grounded", true);
            if(jumpThreeFall){
                fallParticle.Play();
                jumpThreeFall=false;
            }
            dirtParticle.Play();
       }else{
            dirtParticle.Stop();
            explosionParticle.Play();
            playerAudio.PlayOneShot(crashSound, 1.0f);
            gameOver = true;
            Debug.Log("GAME OVER!");
            playerAnim.SetBool("Death_b", true);
            playerAnim.SetInteger("DeathType_int", 1);
       }
    }
    void StagesAlert(){
        if(jkReady){if(Time.time-jkDownTime>3*jumpStage){Debug.Log("Stage 3");}else if(Time.time-jkDownTime>2*jumpStage){Debug.Log("Stage 2");} else{Debug.Log("Stage 1");}}
    }
}
