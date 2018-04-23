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

    public static TestSettings Settings;

    public static NativeArray<NativeQueue<int>.Concurrent> a;

    //public static NativeArray<int> a;


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
        //a = new NativeArray<int>(Settings.number, Allocator.Persistent);
        CubeLook = GetLook("CubeRender");

        a = new NativeArray<NativeQueue<int>.Concurrent>(500, Allocator.Persistent);
        var j = new CreateTableSystem.InitializeTable() { };
        JobHandle jH = j.Schedule();
        jH.Complete();
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
