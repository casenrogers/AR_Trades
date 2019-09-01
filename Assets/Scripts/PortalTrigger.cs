
using UnityEngine;
using UnityEngine.Rendering;

public class PortalTrigger : MonoBehaviour
{
    [SerializeField] private Transform m_trans;
    [SerializeField] private Transform camTrans;
    [SerializeField] private Material videoMat;
    [SerializeField] private Material planeMat;

    private void OnTriggerStay(Collider other)
    {
        Vector3 camPosInPortalSpace = m_trans.InverseTransformPoint(camTrans.position);

        if (camPosInPortalSpace.x <= 0f)
        {
            videoMat.SetInt("_StencilComp", (int)CompareFunction.NotEqual);
            planeMat.SetInt("_CullMode", (int)CullMode.Front);
        }
        else if (camPosInPortalSpace.x < 0.5f)
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
