using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class ServiceProvider
{
    private static readonly Dictionary<Type, IProvidable> _registerDictionary = new();

    public static AssetLibrary AssetLibrary => GetManager<AssetLibrary>();
    public static BoardRenderer BoardRenderer => GetManager<BoardRenderer>();
    public static TriangleFactory TriangleFactory => GetManager<TriangleFactory>();

    public static Board Board => GetManager<Board>();
    public static IPieceManager PieceManager => GetManager<PieceManager>();
    public static ILevelManager LevelManager => GetManager<LevelManager>();
    public static ITangramManager TangramManager => GetManager<BaseTangramManager>();
    public static ISpawnManager SpawnManager => GetManager<SpawnManager>();
    public static GameColorConfig GameColorConfig;
    public static LevelSo Level;
    public static TriangleConfig TriangleConfig;


    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void InitializeServiceProvider()
    {

        if(Level == null) {
            Level = Resources.Load<LevelSo>("ScriptableObjects/LevelConfig");
        }

        UnityEngine.Random.InitState(Level.seed);

        if(GameColorConfig == null)
        {
            GameColorConfig = Resources.Load<GameColorConfig>("ScriptableObjects/GameColorConfig");
        }
        if(TriangleConfig == null)
        {
            TriangleConfig = Resources.Load<TriangleConfig>("ScriptableObjects/TriangleConfig");
        }

        //Self registered.

        _ = new Board();
        _ = new LevelManager();
        _ = new SpawnManager();
        _ = new PieceManager();
        _ = new InstantTangramManager();

        SceneManager.sceneLoaded += (_, _) =>
        {
            Board.Reset();
            PieceManager.Reset();
        };

    }

    private static T GetManager<T>() where T : class, IProvidable
    {
        if (_registerDictionary.ContainsKey(typeof(T)))
        {
            return (T)_registerDictionary[typeof(T)];
        }

        return null;
    }

    public static T Register<T>(T target) where T: class, IProvidable
    {
        _registerDictionary[typeof(T)] = target;
        return target;
    }
}