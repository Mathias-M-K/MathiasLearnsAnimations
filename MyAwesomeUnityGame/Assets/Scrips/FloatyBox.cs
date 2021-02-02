using System;
using UnityEngine;

namespace Scrips
{
    public class FloatyBox : MonoBehaviour
    {
        private Rigidbody _rb;

        [Header("Settings")] 
        public float targetDistanceFromGround;
        public float maxForce;
        public float minForce;

        
        [Header("Status")]
        
        
        public float force;
        public float distanceToGround;

        void Start()
        {
            _rb = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        private void FixedUpdate()
        {
            RaycastHit rCast;

            if (Physics.Raycast(transform.position, Vector3.down, out rCast, 50))
            {
                distanceToGround = rCast.distance;
            }

            force = Remap(distanceToGround, 0, targetDistanceFromGround, maxForce, 0);

            if (force < 0) force = 0;


            _rb.AddForce(Vector3.up*force);
        }
        
        public  float Remap (float value, float from1, float to1, float from2, float to2) {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }
    }
}
