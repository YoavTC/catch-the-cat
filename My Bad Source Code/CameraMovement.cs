using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float cameraSpeed;

    private Vector3 cameraPos;
    private Vector3 playerPos;

    [SerializeField] float maxHeight;
    [SerializeField] float minHeight;
    void Update()
    {
        cameraPos = transform.position;
        
        playerPos = player.transform.position;
        playerPos.z = cameraPos.z;
        playerPos.x = cameraPos.x;
        
        playerPos.y = Mathf.Clamp(playerPos.y, minHeight, maxHeight);
        
        transform.position = Vector3.Lerp(cameraPos, playerPos, cameraSpeed * Time.deltaTime);
    }
}
