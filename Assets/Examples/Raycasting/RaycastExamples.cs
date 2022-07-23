using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/*
 * **********************************************************************
 * Project  : Flask
 * Author   : Laurent Montreuil
 * DevTeam  : Antoine-Olivier Monaco, Gabriel Kirouac, Laurent Montreuil
 * Date     : 27/02/2022

 * Brief    : Example de raycast en Unity
 * **********************************************************************
*/
namespace CodeExamples
{
    public class RaycastExamples : MonoBehaviour
    {
        public LayerMask stopRayOnTheseLayers;
        private GameObject lastHit;
        private Vector3 rayMaxDistance;
        public Vector3 direction;

        private Rigidbody rigidBody;
        private CapsuleCollider capsuleCollider;

        private void Awake()
        {
            rigidBody = GetComponent<Rigidbody>();
            capsuleCollider = GetComponent<CapsuleCollider>();
        }
        void NormalRayCast()
        {
            float distance = 5;
            rayMaxDistance = new Vector3(
                    direction.x * distance,
                    direction.y * distance,
                    transform.position.z);
            Debug.DrawRay(rigidBody.position, rayMaxDistance, Color.green); // Pour débugger. Doit être pareil au vrai raycast plus bas


            RaycastHit hit;
            var origin = new Vector3(rigidBody.position.x, rigidBody.position.y, rigidBody.position.z);
            var ray = new Ray(origin, direction);
            if (Physics.Raycast(ray, out hit, distance, stopRayOnTheseLayers))
            {
                lastHit = hit.transform.gameObject;
                Debug.Log("Hitting " + lastHit.name + "at " + hit.distance + " distance");
            }
        }

        void CapsuleRayCast()
        {
            /// Craétion de capsule
            var distanceBetweenPoints = capsuleCollider.height / 2 - capsuleCollider.radius;
            var point1 = transform.position + capsuleCollider.center + Vector3.up * distanceBetweenPoints;
            var point2 = transform.position + capsuleCollider.center - Vector3.up * distanceBetweenPoints;
            var radius = capsuleCollider.radius;

            float distance = 5;
            RaycastHit hit;
            if (Physics.CapsuleCast(point1, point2, radius, direction, out hit, distance, stopRayOnTheseLayers))
            {
                lastHit = hit.transform.gameObject;
                Debug.Log("Hitting " + lastHit.name + "at " + hit.distance + " distance on this point " + hit.point);
                Debug.DrawRay(rigidBody.position, hit.point, Color.green);

                var destination = new Vector3(
                    transform.position.x + direction.x * hit.distance,
                    transform.position.y + direction.y * hit.distance,
                    transform.position.z);

                // Possible de déplacer l'objet à cet endroit en ayant confiance qu'il n'y aura pas de colisions
                //rigidBody.MovePosition(destination);
            }
        }
        private void Update()
        {
            NormalRayCast();
            //CapsuleRayCast();
        }
    }
}