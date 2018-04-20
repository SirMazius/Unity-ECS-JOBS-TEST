using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Rendering;
using Unity.Transforms;
using Unity.Mathematics;

public class GameBootstrapper
{

    public static EntityArchetype CubeArchetype; //< La info del objeto

    public static MeshInstanceRenderer CubeLook; //< El look

    public static TestSettings Settings;

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

    private static void CreateCubes(EntityManager entityManager)
    {

        Entity cube = entityManager.CreateEntity(CubeArchetype);

        entityManager.SetComponentData(cube, new Position() { Value = new float3(0,0,0) });
        entityManager.SetComponentData(cube, new Heading() { Value = new float3(0,0,1) });
        entityManager.SetComponentData(cube, new VelocityMag() { Value = 5f });

        entityManager.AddSharedComponentData(cube, CubeLook);

    }

}
