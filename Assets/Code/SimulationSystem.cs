using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Jobs;

public class SimulationSystem : ComponentSystem
{

    public struct Data
    {
        public int Length;
        public ComponentDataArray<Position> positions;
    }

    [Inject]
    public Data data;

    protected override void OnUpdate()
    {
        NativeArray<float3> l_forces = new NativeArray<float3>(data.Length, Allocator.Temp);
    }

    //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
    /*JOBS*/
    //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

    struct ComputeNeighbors : IJobParallelFor
    {

        const int prime1 = 73856093;
        const int prime2 = 19349663;
        const int prime3 = 83492791;

        public ComponentDataArray<Position> pos;
        public NativeMultiHashMap<int, int> hM;
        public float dt, h;
        public int size;

        public void Execute(int i)
        {
            NativeList<int> l_neighbors;
            float3 r, posAux;
            
            r.x = pos[i].Value.x / h * prime1;
            r.y = pos[i].Value.x / h * prime2;
            r.z = pos[i].Value.x / h * prime3;

            float3 bbMin, bbMax;

            bbMin.x = (r.x * (pos[i].Value.x - h));
            bbMin.y = (r.y * (pos[i].Value.y - h));
            bbMin.z = (r.z * (pos[i].Value.z - h));

            bbMax.x = (r.x * (pos[i].Value.x + h));
            bbMax.y = (r.y * (pos[i].Value.y + h));
            bbMax.z = (r.z * (pos[i].Value.z + h));

            for (posAux.x = (int)bbMin.x; posAux.x < (int)bbMax.x; posAux.x++)
            {
                for (posAux.y = (int)bbMin.y; posAux.y < (int)bbMax.y; posAux.y++)
                {
                    for (posAux.z = (int)bbMin.z; posAux.z < (int)bbMax.z; posAux.z++)
                    {
                        
                    }
                }
            }
            
        }

        int Hash(float3 p)
        {
            return ((int)(p.x / h * prime1) ^ (int)(p.y / h * prime2) ^ (int)(p.z / h * prime3)) % size;
        }
    }

}
