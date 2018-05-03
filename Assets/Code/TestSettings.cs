using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

public class TestSettings : MonoBehaviour
{

    public float3 direction;
    public float velocity_magnitude;
    public int number;

    private int hashTam;

    public int HashTam
    {
        get
        {
            return hashTam;
        }

        set
        {
            hashTam = value;
        }
    }

    ////public struct Data
    ////{
    ////    public ComponentDataArray<Position> Position;
    ////}
    //[Inject]
    //public ComponentDataArray<Position> positions;
    ////[Inject] private Data m_Group;

    //private void Update()
    //{

    //    //var job = new MovementJobSystem.Movement() { dt = Time.deltaTime};

    //    var job2 = new AuxSystem.AuxMovemente() { dt = Time.deltaTime, pos = positions };
    //    JobHandle jH = job2.Schedule();
    //}

    /*
       Limpiamos la memoria al acabar la escenar
    */
    private void OnDestroy()
    {
       
    }
}
