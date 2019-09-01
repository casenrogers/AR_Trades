
using UnityEngine;
using UnityEngine.Rendering;

public class PortalTrigger : MonoBehaviour
{
    [SerializeField] private Transform m_trans;
    [SerializeField] private Transform camTrans;
    [SerializeField] private Material videoMat;
    [SerializeField] private Material planeMat;

    public void StartShaderValues()
    {
        videoMat.SetInt("_StencilComp", (int)CompareFunction.Equal);
        planeMat.SetInt("_CullMode", (int)CullMode.Back);
    }

    private void OnTriggerStay(Collider other)
    {
        Vector3 camPosInPortalSpace = m_trans.InverseTransformPoint(camTrans.position);

        if (camPosInPortalSpace.y <= 0f)
        {
            videoMat.SetInt("_StencilComp", (int)CompareFunction.NotEqual);
            planeMat.SetInt("_CullMode", (int)CullMode.Front);
        }
        else if (camPosInPortalSpace.y < 0.1f)
        {
            videoMat.SetInt("_StencilComp", (int)CompareFunction.Always);
            planeMat.SetInt("_CullMode", (int)CullMode.Off);
        }
        else
        {
            videoMat.SetInt("_StencilComp", (int)CompareFunction.Equal);
            planeMat.SetInt("_CullMode", (int)CullMode.Back);
        }
    }
}
