using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Transform        playerTransform;
    private Camera          cam;
    public float            speedCam;
    public Transform        leftCamLimit, rightCamLimit, upperCamLimit, downCamLimit;

    public bool             isPause;

    public Text             bananasText;
    public Text             shurikensText;
    public Image[]          hearts;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        HeartController();
        ShurikenCount();
    }

    // Update is called once per frame
    void Update()
    {
        PauseControl();
       
    }

    void LateUpdate()
    {
        float camPositionX = playerTransform.position.x;
        float camPositionY = playerTransform.position.y;

        if (cam.transform.position.x < leftCamLimit.position.x && playerTransform.position.x < leftCamLimit.position.x){
            camPositionX = leftCamLimit.position.x;
        }

        else if (cam.transform.position.x > rightCamLimit.position.x && playerTransform.position.x > rightCamLimit.position.x){
            camPositionX = rightCamLimit.position.x;
        }

        if (cam.transform.position.y > upperCamLimit.position.y && playerTransform.position.y > upperCamLimit.position.y){
            camPositionY = upperCamLimit.position.y;
        }
        else if (cam.transform.position.y < downCamLimit.position.y && playerTransform.position.y < downCamLimit.position.y){
            camPositionY = downCamLimit.position.y;
        }

        Vector3 camPosition = new Vector3(camPositionX, camPositionY, cam.transform.position.z);
        cam.transform.position = Vector3.Lerp(cam.transform.position, camPosition, speedCam * Time.deltaTime); 
    }

    void Pause(){
        Time.timeScale = 0;
    }

    void Unpause(){
        Time.timeScale = 1;
    }

    void PauseControl(){
        if (Input.GetKeyDown(KeyCode.Escape)){
            isPause = !isPause;
            
            if (isPause){
                Pause();
            }
            else{
                Unpause();
            }
        }
    }

    public void HeartController(){
        int health = GameObject.Find("Player").GetComponent<PlayerController>().health;
        foreach(Image heart in hearts){
            heart.enabled = false;
        }
        for (int i = 0; i < health; i++){
            hearts[i].enabled = true;
        }
    }

    public void ShurikenCount(){
        int shurikens = GameObject.Find("Player").GetComponent<PlayerController>().amountShuriken;
        shurikensText.text = shurikens.ToString();

    }

    public void BananaCount(){
        int bananas = GameObject.Find("Player").GetComponent<PlayerController>().amountBanana;
        bananasText.text = bananas.ToString();
    }
}
