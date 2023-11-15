using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pikachu.Scripts.Common.GameBoard
{
    public class BoardInput : MonoBehaviour
    {
        [SerializeField] private LayerMask mouseCastLayer;

        public Action<GridSlotCell> OnPress;
        public Action<bool> OnRelease;

        private Camera _mainCamera;

        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 mouseWorldPosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
                Collider2D mouseHitCollider = Physics2D.OverlapPoint(mouseWorldPosition, mouseCastLayer);
                
                if(mouseHitCollider != null)
                {
                    if(mouseHitCollider.TryGetComponent<GridSlotCell>(out var slotCell))
                    {
                        OnPress?.Invoke(slotCell);
                    }
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                OnRelease?.Invoke(true);
            }
        }
    }
}
