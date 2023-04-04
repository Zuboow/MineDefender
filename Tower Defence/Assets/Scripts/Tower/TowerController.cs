using UnityEngine;
using TowerDefence.Camera;
using System.Collections.Generic;
using TowerDefence.Entities.Dwarfs;
using TowerDefence.UI;

namespace TowerDefence.Towers
{
    public class TowerController : MovableObjectBaseController
    {
        [field: Header("Settings")]
        [field: SerializeField]
        private LayerMask BuildingGroundLayerMask { get; set; }
        [field: SerializeField]
        private LayerMask PathLayerMask { get; set; }
        [field: SerializeField]
        private float MaxRaycastDistance { get; set; }
        [field: SerializeField]
        public int TowerCost { get; private set; }

        [field: Header("Materials")]
        [field: SerializeField]
        private Material CorrectTowerPlacementMaterial { get; set; }
        [field: SerializeField]
        private Material WrongTowerPlacementMaterial { get; set; }

        [field: Header("ObjectReferences")]
        [field: SerializeField]
        public Transform DwarfSpawner { get; set; }
        [field: SerializeField]
        public Transform DwarfPrefab { get; set; }
        [field: SerializeField]
        private Transform TowerRangeIndicator { get; set; }
        [field: SerializeField]
        private TowerShootingBase ShootingController { get; set; }

        private List<MeshRenderer> ChildrenMeshRendererCollection { get; set; } = new List<MeshRenderer>();

        private bool IsBlueprint { get; set; }
        private bool IsOnBuildingGround { get; set; }
        private bool IsColliding { get; set; }
        private bool IsSetWithCorrectMaterial { get; set; }

        public bool CanTowerBePlacedInCurrentLocation()
        {
            return IsOnBuildingGround == true && IsColliding == false;
        }

        public void PreparePlacedTower(TowerController originalPrefab)
        {
            SetTowerChildrenOriginalMaterials(originalPrefab);
            TryToSpawnDwarfDefender();
        }

        public void MarkAsBlueprint()
        {
            IsBlueprint = true;
        }

        public void MarkAsPlaced()
        {
            IsBlueprint = false;
            ShootingController.ReadyToShoot = true;
            TowerRangeIndicator.gameObject.SetActive(false);
        }

        protected virtual void Start()
        {
            SetMainCamera();
            InsertChildrenMeshRenderersIntoList();

            SetStartingBlueprintMaterial();

            SetRangeIndicatorSize();
        }

        protected virtual void Update()
        {
            if (IsBlueprint == true)
            {
                SetPositionOnRaycastHit();

                if (CanTowerBePlacedInCurrentLocation() == true && IsSetWithCorrectMaterial == false)
                {
                    SetTowerMaterialAsCorrectPlacement();
                }
                else if (CanTowerBePlacedInCurrentLocation() == false && IsSetWithCorrectMaterial == true)
                {
                    SetTowerMaterialAsWrongPlacement();
                }
            }
        }

        protected virtual void OnTriggerStay(Collider other)
        {
            IsColliding = true;
        }

        protected virtual void OnTriggerExit(Collider other)
        {
            IsColliding = false;
        }

        protected override void SetPosition(RaycastHit hit)
        {
            transform.position = new Vector3(hit.point.x, 0.0f, hit.point.z);
        }

        protected override void SetPositionOnRaycastHit()
        {
            Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, MaxRaycastDistance, BuildingGroundLayerMask | PathLayerMask) == true)
            {
                SetPosition(hit);

                if (IsInLayerMask(hit.collider.gameObject.layer, BuildingGroundLayerMask.value) == true)
                {
                    IsOnBuildingGround = true;
                }
                else
                {
                    IsOnBuildingGround = false;
                }
            }
            else
            {
                IsOnBuildingGround = false;
            }
        }

        protected override void SetRangeIndicatorSize()
        {
            TowerRangeIndicator.localScale = new Vector3(ShootingController.CheckingRadius * 2, TowerRangeIndicator.localScale.y, ShootingController.CheckingRadius * 2);
        }

        private void TryToSpawnDwarfDefender()
        {
            if (DwarfSpawner != null)
            {
                Transform dwarf = Instantiate(DwarfPrefab, DwarfSpawner.position, DwarfSpawner.rotation, transform);
                GetComponent<DwarfTowerShootingController>().Dwarf = dwarf.GetComponent<DwarfController>();
            }
        }

        private void InsertChildrenMeshRenderersIntoList()
        {
            for (int index = 0; index < transform.childCount; index++)
            {
                if (transform.GetChild(index).GetComponent<MeshRenderer>() != null)
                {
                    ChildrenMeshRendererCollection.Add(transform.GetChild(index).GetComponent<MeshRenderer>());
                }
            }
        }

        private void SetTowerChildrenOriginalMaterials(TowerController originalPrefab)
        {
            for (int index = 0; index < ChildrenMeshRendererCollection.Count; index++)
            {
                if (originalPrefab.transform.GetChild(index).GetComponent<MeshRenderer>() != null)
                {
                    ChildrenMeshRendererCollection[index].sharedMaterial = originalPrefab.transform.GetChild(index).GetComponent<MeshRenderer>().sharedMaterial;
                }
            }
        }

        private void SetStartingBlueprintMaterial()
        {
            SetTowerMaterialAsWrongPlacement();
        }

        private void SetTowerMaterialAsWrongPlacement()
        {
            for (int index = 0; index < ChildrenMeshRendererCollection.Count; index++)
            {
                ChildrenMeshRendererCollection[index].sharedMaterial = WrongTowerPlacementMaterial;
            }

            IsSetWithCorrectMaterial = false;
        }

        private void SetTowerMaterialAsCorrectPlacement()
        {
            for (int index = 0; index < ChildrenMeshRendererCollection.Count; index++)
            {
                ChildrenMeshRendererCollection[index].sharedMaterial = CorrectTowerPlacementMaterial;
            }

            IsSetWithCorrectMaterial = true;
        }
    }
}