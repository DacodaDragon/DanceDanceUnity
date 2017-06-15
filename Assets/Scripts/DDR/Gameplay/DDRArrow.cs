using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DDRArrow : MonoBehaviour
{
    public float beatStamp;

    // Todo: Find way to make this less of a hack.
    public void SetPosition(float currentBeat, float speedMultiplier)
    {
        float newPositionY = ((currentBeat - beatStamp) * speedMultiplier);
        transform.localPosition = new Vector3(0, newPositionY , 0);
    }
}

// Funny modifier for Y axis, might use this later.
//float wobbly = (Mathf.Cos((currentBeat + beatStamp) * 2 * Mathf.PI) * 2) * (currentBeat - beatStamp);