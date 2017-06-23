using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDR
{
    public class Arrow : MonoBehaviour
    {
        public float beatStamp;

        Vector3 originalRotation;

        public void Awake()
        {
            originalRotation = transform.localRotation.eulerAngles;
        }

        // Todo: Find way to make this less of a hack.
        public void SetPosition(float currentBeat, float speedMultiplier)
        {
            
            float newPositionY = (((currentBeat - beatStamp) * speedMultiplier));


            transform.localPosition = new Vector3(0, newPositionY, 0);
            transform.localRotation = Quaternion.Euler(originalRotation + new Vector3(0, newPositionY * 10, 0));
        }
    }
}


// Funny modifier for Y axis, might use this later.
//float wobbly = (Mathf.Cos((currentBeat + beatStamp) * 2 * Mathf.PI) * 2) * (currentBeat - beatStamp);