using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aquazone.Scripts.Helpers
{
    public class CalculateCenter : MonoBehaviour
    {
        private Renderer[] _childRenderers;

        private Vector3 _center;
        private Vector3 _newCenter;
        private float _rendererCount;

        void Start()
        {
            Debug.Log(ReturnObjCenter());
        }

        private Vector3 ReturnObjCenter()
        {
            _childRenderers = transform.GetComponentsInChildren<Renderer>();

            _center = Vector3.zero;
            _rendererCount = 0f;

            foreach (var item in _childRenderers)
            {
                _center += item.transform.position;

                _rendererCount++;
            }

            _newCenter = _center / _rendererCount;

            return _newCenter;
        }
    }
}
