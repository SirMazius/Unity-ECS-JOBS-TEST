﻿using Unity.Entities;
using UnityEngine;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Jobs;
using Unity.Collections;


/*
    Procesa el movimiento de todas las particulas una a una
*/
public class MovementSystem : ComponentSystem
{
    /*  
        Datos de una particula
    */
    public struct Data
    {
        public int Length;
        public ComponentDataArray<Position> positions;
        public ComponentDataArray<Heading> directions;
        //public ComponentDataArray<VelocityMag> velocities;
    }

    /*
        El EntityManager filtra los datos que contengan la info que coincida con <<Data>> y la mete en <<data>> 
    */
    [Inject] public Data data;
    
    protected override void OnUpdate()
    {
        float dt = Time.deltaTime;

        for (int i = 0; i < data.Length; i++)
        {
            var current_position = data.positions[i].Value;
            var next_position = current_position + data.directions[i].Value * dt;
            data.positions[i] = new Position() { Value = next_position };
        }

    }

}

[UpdateBefore(typeof(AuxSystem))]
public class MovementJobSystem : JobComponentSystem
{
    [ComputeJobOptimization]
    public struct Movement : IJobProcessComponentData<Position, Heading>
    {

        public float dt;

        public void Execute(ref Position pos, ref Heading head)
        {
            pos.Value = pos.Value + head.Value * dt;
        }
    }

    
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new Movement() { dt = Time.deltaTime };
        return job.Schedule(this, 128, inputDeps);
    }
}
