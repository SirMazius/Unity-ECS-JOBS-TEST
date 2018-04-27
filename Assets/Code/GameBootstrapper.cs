using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Rendering;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Jobs;

public class GameBootstrapper
{

    public static EntityArchetype CubeArchetype; //< La info del objeto

    public static MeshInstanceRenderer CubeLook; //< El look

    public static NativeMultiHashMap<int, int> hTable;
    
    public static TestSettings Settings;

    //public static NativeArray<int> a;

    public struct Data
    {
        public int Lenght;
        public ComponentDataArray<Position> positions;
        public ComponentDataArray<Heading> directions;
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Initialize()
    {
        EntityManager entityManager = World.Active.GetOrCreateManager<EntityManager>();
        CubeArchetype = entityManager.CreateArchetype(typeof(Position), typeof(Heading), typeof(VelocityMag), typeof(TransformMatrix));
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void InitializeWithScene()
    {
        var settingsGO = GameObject.Find("Settings");
        Settings = settingsGO.GetComponent<TestSettings>();
        CubeLook = GetLook("CubeRender");

        var j = new HashTableSystem.GetTamTable() { };
        JobHandle jH = j.Schedule();
        jH.Complete();

        hTable = new NativeMultiHashMap<int, int>(Settings.HashTam, Allocator.Persistent);

        

        var j2 = new HashTableSystem.InsertParticles() { hM = hTable };
        JobHandle jH2 =  j2.Schedule(jH);
        jH2.Complete();


        NativeMultiHashMapIterator<int> it;
        int item;
        hTable.TryGetFirstValue(5, out item, out it);
        Debug.Log("ESPERO QUE SALGA -> " + item);

        NewGame();
    }


    

    private static MeshInstanceRenderer GetLook(string name)
    {
        var obj = GameObject.Find(name);
        var result = obj.GetComponent<MeshInstanceRendererComponent>().Value;
        Object.Destroy(obj);
        return result;
    }

    public static void NewGame()
    {
        var entityManager = World.Active.GetOrCreateManager<EntityManager>();
        CreateCubes(entityManager);
    }


    /*
        TODO: Hacer que pueda crear cubos o de forma Random o de acuerdo a un Grid
        Crea los cubos segun el esquema indicado en el flag 
    */
    private static void CreateCubes(EntityManager entityManager)
    {
        var n = Settings.number;

        for (int i = 0; i < n; i++)
        {
            Entity cube = entityManager.CreateEntity(CubeArchetype);

            entityManager.SetComponentData(cube, new Position() { Value = new float3(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10)) });
            entityManager.SetComponentData(cube, new Heading() { Value = new float3(0, 0, 1) });
            entityManager.SetComponentData(cube, new VelocityMag() { Value = 5f });

            entityManager.AddSharedComponentData(cube, CubeLook);
        }

    }

}
