using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{

public GameObject Player1Active;
public GameObject Player2Active;
public GameObject Player1Inactive;
public GameObject Player2Inactive;

bool player1IsReady = false;
bool player2IsReady = false;
bool gameStarting = false;

public float startDelay;



    // Start is called before the first frame update
    void Start()
    {
        this.Player1Inactive.SetActive(!player1IsReady);
        this.Player2Inactive.SetActive(!player2IsReady);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) && !this.gameStarting){

            this.player1IsReady = !this.player1IsReady;
            this.Player1Active.SetActive(this.player1IsReady);
            this.Player1Inactive.SetActive(!this.player1IsReady);

        } else if (Input.GetKeyDown(KeyCode.D) && !this.gameStarting){

            this.player2IsReady = !this.player2IsReady;
            this.Player2Active.SetActive(this.player2IsReady);
            this.Player2Inactive.SetActive(!this.player2IsReady);

        }

        if (this.player1IsReady && this.player2IsReady){
            this.gameStarting = true;
            Invoke("startGame", this.startDelay);            
        }
        
    }

    void startGame(){
        SceneManager.LoadScene(1);
    }
}
