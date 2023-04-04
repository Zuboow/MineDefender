using TowerDefence.Camera;
using UnityEngine;

public class MovableObjectBaseController : MonoBehaviour
{
    [field: SerializeField]
    protected Camera MainCamera { get; set; }

    protected virtual void SetPosition(RaycastHit hit)
    {
        transform.position = new Vector3(hit.point.x, hit.point.y, hit.point.z);
    }

    protected virtual void SetPositionOnRaycastHit() { }

    protected virtual void SetRangeIndicatorSize() { }

    protected bool IsInLayerMask(int layer, int layerMask)
    {
        return (layerMask == (layerMask | (1 << layer)));
    }

    protected void SetMainCamera()
    {
        MainCamera = CameraSingleton.Instance;
    }
}
