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



    // Start is called before the first frame update
    void Start()
    {
        Player1Inactive.SetActive(!player1IsReady);
        Player2Inactive.SetActive(!player2IsReady);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)){
            player1IsReady = !player1IsReady;
            Player1Active.SetActive(player1IsReady);
            Player1Inactive.SetActive(!player1IsReady);
        } else if (Input.GetKeyDown(KeyCode.D)){
            player2IsReady = !player2IsReady;
            Player2Active.SetActive(player2IsReady);
            Player2Inactive.SetActive(!player2IsReady);
        }
        if (player1IsReady && player2IsReady){
            SceneManager.LoadScene(1);
        }
        
    }
}
