using UnityEngine;

namespace TowerDefence.Camera
{
    public class CameraController : MonoBehaviour
    {
        [field: SerializeField]
        private Transform CameraSlot { get; set; }

        [field: Header("Settings")]
        [field: SerializeField]
        private float MoveSpeed { get; set; }
        [field: SerializeField]
        private Vector3 MinMovementLimit { get; set; }
        [field: SerializeField]
        private Vector3 MaxMovementLimit { get; set; }

        private Vector3 CameraPosition { get; set; }

        protected virtual void Awake()
        {
            Initialize();
        }

        protected virtual void Update()
        {
            MoveCamera();
        }

        private void Initialize()
        {
            CameraPosition = new Vector3(CameraSlot.position.x, CameraSlot.position.y, CameraSlot.position.z);
        }

        private void MoveCamera()
        {
            float movementSpeedByTime = MoveSpeed * Time.deltaTime;

            CameraPosition += new Vector3(Input.GetAxisRaw("Vertical") * movementSpeedByTime * -1, 0, Input.GetAxisRaw("Horizontal") * movementSpeedByTime);

            SetCameraPosition(CameraPosition);
        }

        private void SetCameraPosition(Vector3 newPosition)
        {
            CameraPosition = GetLimitedCameraPosition(newPosition);
            CameraSlot.position = CameraPosition;
        }

        private Vector3 GetLimitedCameraPosition(Vector3 cameraPosition)
        {
            return new Vector3(Mathf.Clamp(cameraPosition.x, MinMovementLimit.x, MaxMovementLimit.x), cameraPosition.y, Mathf.Clamp(cameraPosition.z, MinMovementLimit.z, MaxMovementLimit.z));
        }
    }
}