using System;
using UnityEngine;
using Mehroz;

namespace DDR
{
    [System.Serializable]
    public struct ColorScheme
    {
        [SerializeField]
        Material wholes, halves, thirds, fourths, sixths, eights;

        public Material Paint(float beatStamp)
        {
            try
            {
                Fraction fraction;
                if (beatStamp < 1f) fraction = new Fraction(beatStamp);
                else fraction = new Fraction((float)(beatStamp - Math.Truncate(beatStamp)));
                Material material = null;
                if (fraction.Denominator == 1)
                    material = wholes;
                if (fraction.Denominator == 2)
                    material = halves;
                if (fraction.Denominator == 3)
                    material = thirds;
                if (fraction.Denominator == 4)
                    material = fourths;
                if (fraction.Denominator == 6)
                    material = sixths;
                if (fraction.Denominator == 8)
                    material = eights;
                return material;
            }
            catch
            {
                Debug.Log((float)(beatStamp - Math.Truncate(beatStamp)));
                return null;
            }
        }
    }

}