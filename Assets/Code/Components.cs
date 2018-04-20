using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;


    //public struct CubeData : IComponentData
    //{
    //    Position position;
    //    float3 direction;
    //    float velocity_magnitude;
    //}

    public struct VelocityMag : IComponentData
    {
        public float Value;
    }

