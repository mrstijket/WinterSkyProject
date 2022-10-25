using UnityEngine;
using Cinemachine;

public class PassageController : MonoBehaviour
{
    [SerializeField] CinemachineController cinemachineController;
    [SerializeField] CinemachineVirtualCamera toRight;
    [SerializeField] CinemachineVirtualCamera toLeft;
    [SerializeField] CinemachineVirtualCamera setTo;
    PlayerController playerController;
    [SerializeField] CapsuleCollider2D playerBodyCollider;
    bool isFacingRight = true;
    PlayableObject playableObject;
    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        playableObject = FindObjectOfType<PlayableObject>();
    }
    private void Update()
    {
        FlipCameraSetup();
        playerController.gameObject.transform.position= GameObject.FindGameObjectWithTag("Player").transform.position;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Physics2D.IgnoreCollision(playerBodyCollider, GetComponent<Collider2D>());
            cinemachineController.StateSwitcher(setTo);
        }
    }
    private void FlipCameraSetup()
    {
        if(playerController.gameObject.transform.localScale.x == 1)
        {
            setTo = toRight;
        }
        else
        {
            setTo = toLeft;
        }
        isFacingRight = !isFacingRight;
    }
    public void ChangeCameraFollow()
    {
        if (setTo.Follow == playerController.gameObject.transform)
        {
            playerController.isAlive = false;
            playableObject.isPlayerControlling = true;
            playableObject.stunCollider.enabled = true;
            setTo.Follow = playableObject.gameObject.transform;
        }
        else
        {
            playerController.isAlive = true;
            playableObject.isPlayerControlling = false;
            playableObject.stunCollider.enabled = false;
            playableObject.relativeJoint2D.enabled = false;
            playableObject.relativeJoint2D.connectedBody = null;
            setTo.Follow = playerController.gameObject.transform;
        }
    }
}