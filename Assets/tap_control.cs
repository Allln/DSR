using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Rigidbody2D))]

public class tap_control : MonoBehaviour{
    public delegate void PlayerDelegate();
    public static event PlayerDelegate OnPlayerDied;
    public static event PlayerDelegate OnPlayerScored;
    public static event PlayerDelegate OnPlayerTapped;
    public float tapForce;
    public float Smooth;
    public Vector3 startPos;

    public AudioSource tapAudio;
    public AudioSource scoreAudio;
    public AudioSource dieAudio;

    Rigidbody2D rigidbody;
    Quaternion downRotation;
    Quaternion forwardRotation;
    public float fly;
    game_manage game;

    void Start(){
        /*Parallaxer.spawn = true;
        */
        rigidbody = GetComponent<Rigidbody2D>();
        downRotation = Quaternion.Euler(0,0,8);
        forwardRotation = Quaternion.Euler(0,0,30);
        game = game_manage.Instance;
        rigidbody.simulated = true;
        if(transform.position.x != -2)
        transform.position = new Vector3(-2,transform.position.y,transform.position.z);

    }
    void OnEnable(){
        game_manage.OnGameStarted += OnGameStarted;
        game_manage.OnGameOverConfirmed += OnGameOverConfirmed;
 
    }
    void OnGameStarted(){
        /*Parallaxer.spawn = true;
        */
        

        rigidbody = GetComponent<Rigidbody2D>();
        fly = 404;
        transform.localPosition = startPos;
        rigidbody.velocity = Vector3.zero;
        downRotation = Quaternion.Euler(0,0,0);
        forwardRotation = Quaternion.Euler(0,0,30);
        rigidbody.simulated = true;
        if(transform.position.x != -2)
        transform.position = new Vector3(-2,transform.position.y,transform.position.z);
    }
    void OnGameOverConfirmed(){
        fly = 404;
        transform.localPosition = startPos;
        transform.rotation = Quaternion.identity;
    }
    void OnDisable(){
        game_manage.OnGameStarted -= OnGameStarted;
        game_manage.OnGameOverConfirmed -= OnGameOverConfirmed;
    }

    void Update(){
        if (game.GameOver) {
            rigidbody.simulated = false;
            return;
        }
        if(Input.GetMouseButtonDown(0) && fly > -1){
            OnPlayerTapped();
            Time.timeScale +=  1F/404F;
            tapAudio.Play();
            /*Time.timeScale +=  1;
            */fly--;
            transform.rotation = forwardRotation;
            rigidbody.velocity = Vector3.zero;
            rigidbody.AddForce(Vector2.up * tapForce, ForceMode2D.Force);
        }
        transform.rotation = Quaternion.Lerp(transform.rotation,downRotation,Smooth*Time.deltaTime);
        if(transform.position.x != -2)
        transform.position = new Vector3(-2,transform.position.y,transform.position.z);
        if(fly == -1){
            OnPlayerDied();
        }
    }
    void OnTriggerEnter2D(Collider2D col){
        if (col.gameObject.tag == "ScoreZone"){
            OnPlayerScored();
            scoreAudio.Play();

        }
        if (col.gameObject.tag == "DeadZone"){
            rigidbody.simulated = false;
            OnPlayerDied();
            dieAudio.Play();

        }
    }


}
