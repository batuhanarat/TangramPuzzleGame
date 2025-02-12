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
    public static PieceManager PieceManager => GetManager<PieceManager>();
    public static LevelManager LevelManager => GetManager<LevelManager>();
    public static TangramManager TangramManager => GetManager<TangramManager>();
    public static SpawnManager SpawnManager => GetManager<SpawnManager>();

    public static GameColorConfig GameColorConfig;
    public static LevelSo Level;
    public static TriangleConfig TriangleConfig;


    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void InitializeServiceProvider()
    {
        /*
        SceneManager.sceneLoaded += (_, _) =>
        {
            if(GameConfig == null) {
                GameConfig = Resources.Load<GameConfig>("ScriptableObjects/GameConfigSO");
            }
            //Self registered.
            _ = new UIManager();
            _ = new LevelManager();
            _ = new MoveManager();
            _ = new ScoreManager();
        };
        */


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
            _ = new TangramManager();

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