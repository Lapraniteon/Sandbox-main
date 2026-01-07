using UnityEngine;

public class ReturnToSpawn : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    
    private CharacterController characterController;

    private void Start()
    {
        characterController = GetComponentInChildren<CharacterController>();
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            TeleportToSpawn();
        }
    }

    private void TeleportToSpawn()
    {
        Debug.Log("Tp to spawn");
        transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);
        characterController.enabled = false;
        characterController.enabled = true;
    }
}
