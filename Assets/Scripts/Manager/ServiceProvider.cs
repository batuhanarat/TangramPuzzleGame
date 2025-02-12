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
    public static ILevelReader LevelReader => GetManager<LevelReader>();
    public static ITangramManager TangramManager => GetManager<BaseTangramManager>();
    public static ISpawnManager SpawnManager => GetManager<SpawnManager>();
    //public static LevelSo Level;
    public static GameConfig GameConfig;



    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void InitializeServiceProvider()
    {

        /*
                if(Level == null) {
                    Level = Resources.Load<LevelSo>("ScriptableObjects/LevelConfig");
                }
        */

        GameConfig = Resources.Load<GameConfig>("ScriptableObjects/GameConfig");


        //        UnityEngine.Random.InitState(Level.seed);

        _ = new Board();
        _ = new LevelReader();
        _ = new LevelManager();
        _ = new SpawnManager();
        _ = new PieceManager();

        if(GameConfig.IsPieceCreationAnimated)
        {
            _ = new AnimatedTangramManager();
        } else {
            _ = new InstantTangramManager();
        }

        _ = new TriangleFactory();


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