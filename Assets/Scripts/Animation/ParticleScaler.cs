using UnityEngine;

public class ParticleScaler : MonoBehaviour
{
    public void OnWillRenderObject()
    {
        GetComponent<ParticleRenderer>().material.SetVector("_Center", transform.position);
        GetComponent<ParticleRenderer>().material.SetVector("_Scaling", transform.lossyScale);
        GetComponent<ParticleRenderer>().material.SetMatrix("_Camera", Camera.current.worldToCameraMatrix);
        GetComponent<ParticleRenderer>().material.SetMatrix("_CameraInv", Camera.current.worldToCameraMatrix.inverse);
    }
}