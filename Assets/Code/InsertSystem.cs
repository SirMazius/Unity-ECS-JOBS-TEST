using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using UnityEngine;
using Unity.Collections;

[UpdateBefore(typeof(MovementJobSystem))]
public class InsertSystem : ComponentSystem {

	public struct Data
    {
        public int Length;
        public ComponentDataArray<Position> positions;
    }

    [Inject]
    Data data;


    protected override void OnUpdate()
    {
        var job = new InsertParticles()
        {
            dt = Time.deltaTime,
            h = 10f,
            pos = data.positions,
            hM = GameBootstrapper.hTable,
            size = data.Length
        };

        var Handle = job.Schedule(data.Length, 64);
        Handle.Complete();
    }


    //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
    /*JOBS*/
    //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

    [ComputeJobOptimization]
    struct InsertParticles : IJobParallelFor
    {
        const int prime1 = 73856093;
        const int prime2 = 19349663;
        const int prime3 = 83492791;

        public ComponentDataArray<Position> pos;
        public NativeMultiHashMap<int, int>.Concurrent hM;
        public float dt, h;
        public int size;

        public void Execute(int i)
        {
            int index = Hash(pos[i]);
            hM.Add(index, i);
        }

        int Hash(Position p)
        {
            return ((int)(p.Value.x / h * prime1) ^ (int)(p.Value.y / h * prime2) ^ (int)(p.Value.z / h * prime3)) % size;
        }
    }
}
