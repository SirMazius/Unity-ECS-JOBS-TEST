using UnityEngine;
using Unity.Entities;
using Unity.Rendering;
using Unity.Transforms;
using Unity.Mathematics;


public class GameBootstrapper
{

    public static EntityArchetype CubeArchetype; //< La info del objeto

    public static MeshInstanceRenderer CubeLook; //< El look

    public static TestSettings Settings; //< Los settings almacenados en un Script asociado a un GameObject normal en la escena

    /*
        Creamos un Arquetipo que representara la informacion de cada uno de las entidades 
    */
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Initialize()
    {
        EntityManager entityManager = World.Active.GetOrCreateManager<EntityManager>();
        CubeArchetype = entityManager.CreateArchetype(typeof(Position), typeof(Heading), typeof(VelocityMag), typeof(TransformMatrix));
    }


    /*
        Inicializamos el Juego 
    */
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void InitializeWithScene()
    {
        var settingsGO = GameObject.Find("Settings");
        Settings = settingsGO.GetComponent<TestSettings>();
        CubeLook = GetLook("CubeRender");

        NewGame();
    }

    /*
        Recogemos el EntityManager y añadimos todos los cubos 
    */
    public static void NewGame()
    {
        var entityManager = World.Active.GetOrCreateManager<EntityManager>();
        CreateCubes(entityManager);
    }

    /*
        Busacamos la skin de un GameObject convencional y la extraemos para asociarla a las entidades 
    */
    private static MeshInstanceRenderer GetLook(string name)
    {
        var obj = GameObject.Find(name);
        var result = obj.GetComponent<MeshInstanceRendererComponent>().Value;
        Object.Destroy(obj);
        return result;
    }


    /*
        Genera una malla de objetos de forma aleatoria
    */
    private static void CreateCubes(EntityManager entityManager)
    {
        var n = Settings.number;

        for (int i = 0; i < n; i++)
        {
            Entity cube = entityManager.CreateEntity(CubeArchetype); //< Creamos una entidad nueva y guardamos una referencia


            /*
                Añadimos los componentes especificos de esa entidad 
            */
            entityManager.SetComponentData(cube, new Position() { Value = new float3(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10)) });
            entityManager.SetComponentData(cube, new Heading() { Value = new float3(0, 0, 1) });
            entityManager.SetComponentData(cube, new VelocityMag() { Value = 5f });

            /*
                Añadimos los valores comunes a las instancias   
            */
            entityManager.AddSharedComponentData(cube, CubeLook);
        }

    }

}
