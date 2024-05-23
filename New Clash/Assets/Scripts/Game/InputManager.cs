using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
   [SerializeField] private Camera _sceneCamera;
   private Vector3 lastPosition;
   [SerializeField] private LayerMask _placementLayerMask;

   public event Action OnClicked, OnExit;

   private void Update()
   {
      if (Input.GetMouseButtonDown(0))
      {
         OnClicked?.Invoke();
      }

      if (Input.GetKeyDown(KeyCode.Escape))
      {
         OnExit?.Invoke();
      }
   }

   public bool IsPointerOverUI()
      => EventSystem.current.IsPointerOverGameObject();

   public Vector3 GetSelectedMapPosition()
   {
      Vector3 mousePosition = Input.mousePosition;
      Ray ray = _sceneCamera.ScreenPointToRay(mousePosition);
      RaycastHit hit;
      if (Physics.Raycast(ray, out hit, 100, _placementLayerMask))
      {
         lastPosition = hit.point;
      }
      lastPosition.y = 0;
      return lastPosition;
   }
   
}