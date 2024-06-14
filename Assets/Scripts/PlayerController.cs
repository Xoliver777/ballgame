using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float speed = 20f;
    public float jumpForce = 300f;
    public float sideSpeed = 5f;
    bool canJump = false;
    Rigidbody playerRb;
    Vector3 startPos = new Vector3(0f, 1f, 5f);
    CardboardReticlePointer pointer;
    public static int level = 0;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        pointer = GetComponentInChildren<CardboardReticlePointer>();
        pointer.gameObject.SetActive(Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer);
    }

    // Update is called once per frame
    void Update()
    {
        // Folyamatosan előre
        transform.Translate(0f, 0f, speed * Time.deltaTime);

        if(pointer.isActiveAndEnabled) VrControls();
        else PcControls();
    }

    void VrControls()
    {
        transform.Translate(Vector3.Dot(pointer.transform.forward, transform.right) * sideSpeed * 2f * Time.deltaTime, 0, 0);
        float jumpInput =Vector3.Dot(pointer.transform.forward, transform.up);
        if(jumpInput > 0.25 && canJump)
        {
            canJump = false;
            playerRb.AddForce(0f, jumpInput * jumpForce * 4, 0f);
        }
    }

    void PcControls()
    {
        transform.Translate(Input.GetAxis("Horizontal") * sideSpeed * Time.deltaTime, 0f, 0f);
        float jumpInput = Input.GetAxis("Jump");
        if(jumpInput > 0.2f && canJump)
        {
            canJump = false;
            playerRb.AddForce(0f, jumpInput * jumpForce, 0f);
        }
    }

    void OnCollisionEnter(Collision other)
    {
        canJump = true;
        if(other.gameObject.tag == "Obstacle")
        {
            playerRb.velocity = Vector3.zero;
            canJump = false;
            transform.position = startPos;
        }
        if(other.gameObject.tag == "Finish1")
        {
            playerRb.velocity = Vector3.zero;
            canJump = false;
            SceneManager.LoadScene("level2");
        }
        if(other.gameObject.tag == "Finish2")
        {
            playerRb.velocity = Vector3.zero;
            canJump = false;
            speed *= 1.25f;
            sideSpeed *= 1.25f;
            SceneManager.LoadScene("level1");
        }
         if(other.gameObject.tag == "Bouncy")
        {
            canJump = false;
            playerRb.AddForce(0f, 1000, 0f);
        }
    }
}
