using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.EventSystems;

public class PortalPositioner : MonoBehaviour
{
    [SerializeField] protected GameObject holoPortal;
    [SerializeField] protected GameObject portal;
    [SerializeField] protected Transform rootTrans;
    [SerializeField] protected Transform cameraTransform;
    [SerializeField] protected GameObject assistCanvas;
    [SerializeField] protected PortalTrigger trigger;

    [SerializeField] protected ARRaycastManager raycastManager;
    [SerializeField] protected TrackableType typeMask;
    [SerializeField] protected float scaleSpeed = 0.003f;

    protected float minScale = 0.05f;
    protected float maxScale = 1f;

    protected virtual void Start()
    {
        rootTrans = transform;
        trigger.StartShaderValues();
        HidePortal();
    }

    protected void Update()
    {
        CheckForPositionChanges();
        //CheckForScaleChanges();
    }

    protected void CheckForPositionChanges()
    {
        if (Input.touchCount == 0 || Input.touchCount > 1) return;
        if (IsPointerOverUIObject()) return;

        Vector2 touchPos = Input.GetTouch(0).position;
        List<ARRaycastHit> hit = new List<ARRaycastHit>();

        if (raycastManager.Raycast(touchPos, hit, typeMask))
        {
            rootTrans.position = hit[0].pose.position;
            FaceCamera();
        }
    }

    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.touches[0].position.x, Input.touches[0].position.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    protected void CheckForScaleChanges()
    {
        if (Input.touchCount < 2) return;

        Touch touchZero = Input.GetTouch(0);
        Touch touchOne = Input.GetTouch(1);

        if (touchZero.phase == TouchPhase.Ended) return;
        if (touchOne.phase == TouchPhase.Ended) return;

        Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
        Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

        float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
        float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;
        float deltaMagnitudeDiff = touchDeltaMag - prevTouchDeltaMag;

        SetScale(deltaMagnitudeDiff);
    }

    protected void SetScale(float delta)
    {
        Vector3 scale = rootTrans.localScale;
        Vector3 scaleDelta = Vector3.one;

        scaleDelta *= (scaleSpeed * delta);
        scale += scaleDelta;

        float clampScale = Mathf.Clamp(scale.x, minScale, maxScale);
        scale = new Vector3(clampScale, clampScale, clampScale);

        rootTrans.localScale = scale;
    }

    public void HidePortal()
    {
        portal.SetActive(false);
        holoPortal.SetActive(true);
    }

    public void ShowPortal()
    {
        portal.SetActive(true);
        portal.transform.parent = null;

        assistCanvas.SetActive(false);
        rootTrans.gameObject.SetActive(false);
    }

    protected void FaceCamera()
    {
        rootTrans.LookAt(cameraTransform);
        float yRot = rootTrans.localEulerAngles.y;
        rootTrans.localEulerAngles = new Vector3(0, yRot, 0);
    }
}
