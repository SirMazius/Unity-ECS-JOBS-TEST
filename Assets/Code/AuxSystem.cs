using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using UnityEngine;
using Unity.Mathematics;
using Unity.Collections;

public class AuxSystem : JobComponentSystem
{

    [ComputeJobOptimization]
    public struct AuxMovemente : IJobProcessComponentData<Position, Heading>
    {
        public float dt;

        public void Execute(ref Position pos, ref Heading head)
        {
            
            // pos.Value = pos.Value * dt;
        }


    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {

        
        var Job = new AuxMovemente() { dt = Time.deltaTime };
        return Job.Schedule(this, 128, inputDeps);
    }

}
