using Unity.VisualScripting;
using UnityEngine;

public class CoinAnimator : MonoBehaviour
{
    [SerializeField] float AngularSpeed = 50f;
     [SerializeField] float CoinHeight = 0.7f;
    [SerializeField] float MovementAmplitude = 0.5f;
     [SerializeField] float MovementFrequency = 1f;
    [SerializeField] Transform CoinMesh;
    // Update is called once per frame
    void Update()
    {   // Rotating the coin
         CoinMesh.Rotate(0f,AngularSpeed *Time.deltaTime,0f);

        // Moving the coin up and down using sin wave.
        float DeltaY = MovementAmplitude * Mathf.Sin(MovementFrequency*Time.time);
        CoinMesh.localPosition = new Vector3(0f,CoinHeight + DeltaY,0f);
    }
}
