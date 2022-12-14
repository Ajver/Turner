using System;
using UnityEngine;

public class MenuDoor : MonoBehaviour, IInteractable
{
    [SerializeField] private Animator animator;
    [SerializeField] private Collider closedDoorCollider;

    [SerializeField] private AudioSource doorOpenAudio;
    [SerializeField] private AudioSource doorCloseAudio;

    private bool isOpen;
    
    private Action onActionComplete;

    private float timer;
    private bool isRunning;

    private void Update()
    {
        if (!isRunning)
        {
            return;
        }

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            isRunning = false;
            onActionComplete();
        }
    }

    private void OpenDoor()
    {
        closedDoorCollider.enabled = false;
        isOpen = true;
        
        doorOpenAudio.Play();
        animator.SetBool("IsOpen", isOpen);
    }
    
    private void CloseDoor()
    {
        closedDoorCollider.enabled = true;
        isOpen = false;
        
        doorCloseAudio.Play();
        animator.SetBool("IsOpen", isOpen);
    }

    public void Interact(Action onInteractComplete)
    {
        onActionComplete = onInteractComplete;
        timer = 0.5f;
        isRunning = true;
        
        if (isOpen)
        {
            CloseDoor();
        }
        else
        {
            OpenDoor();
        }
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
