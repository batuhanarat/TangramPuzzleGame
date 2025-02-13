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
    public static ILevelReader LevelReader => GetManager<BaseLevelReader>();
    public static ITangramManager TangramManager => GetManager<BaseTangramManager>();
    public static ISpawnManager SpawnManager => GetManager<SpawnManager>();
    public static GameConfig GameConfig;



    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void InitializeServiceProvider()
    {

        InitializeConfiguration();
        InitializeCoreServices();
        RegisterSceneEvents();

    }

    private static void InitializeConfiguration()
    {
        GameConfig = Resources.Load<GameConfig>("ScriptableObjects/GameConfig");
        if (GameConfig == null)
            Debug.LogError("Failed to load GameConfig!");
    }

    private static void InitializeCoreServices()
    {
        _ = new Board();
        CreateLevelReaderInstance();
        _ = new LevelManager();
        _ = new SpawnManager();
        _ = new PieceManager();
        CreateTangramManagerInstance();
        _ = new TriangleFactory();
    }


    private static ILevelReader CreateLevelReaderInstance()
    {
        return GameConfig.ShouldCreateLevelDynamically
            ? new DynamicLevelReader()
            : new LocalLevelReader();
    }

    private static ITangramManager CreateTangramManagerInstance()
    {
        return GameConfig.ShouldAnimatePieceCreation
            ? new AnimatedTangramManager()
            : new InstantTangramManager();
    }

    private static void RegisterSceneEvents()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Board?.Reset();
        PieceManager?.Reset();
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