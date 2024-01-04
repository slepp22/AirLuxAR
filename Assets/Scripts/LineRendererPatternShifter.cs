/*========================================================================
Copyright (c) 2021 PTC Inc. All Rights Reserved.
 
Vuforia is a trademark of PTC Inc., registered in the United States and other
countries.
=========================================================================*/

using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineRendererPatternShifter : MonoBehaviour
{
    public FlowDirection Direction = FlowDirection.POSITIVE;
    Material mMaterial;
    
    float mTime;
    
    public Transform objectA;
    public Transform objectB;
    
    void Awake()
    {
        mMaterial = GetComponent<LineRenderer>().material;
        
        // This is for a static implementation - move to update
        mMaterial.SetVector("_WorldPosA", objectA.position);
        mMaterial.SetVector("_WorldPosB", objectB.position);
    }

    void Update()
    {
        mMaterial.mainTextureOffset = new Vector2(mTime * ((int)Direction), 0);
        mTime += Time.deltaTime;
        if (mTime >= 1)
            mTime = 0;
        
        mMaterial.SetVector("_WorldPosA", objectA.position);
        mMaterial.SetVector("_WorldPosB", objectB.position);
        
        //Debug.Log(objectA.position);
    }

    public enum FlowDirection
    {
        POSITIVE = 1,
        NEGATIVE = -1
    }
}