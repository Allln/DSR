using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallaxer : MonoBehaviour
{
    class PoolObject{
        public Transform transform;
        public bool inUse;
        public PoolObject(Transform t){ transform = t; }
        public void Use(){ inUse = true; }
        public void Dispose(){ inUse = false; }

    }
    public float min;
    public float max;
    public GameObject Prefab;
    public int poolSize;
    public float shiftSpeed;
    public float spawnRate;
    
    public Vector3 defaultSpawnPos;
    public bool spawnImmediate;
    public Vector3 immediateSpawnPos;
    public Vector2 targetAspectRatio;

    float spawnTimer;
    float targetAspect;
    PoolObject[] poolObjects;
    game_manage game;


    void Awake(){
        Configure();
    }

    void Start(){
        game = game_manage.Instance;
    }

    void OnEnable(){
        game_manage.OnGameOverConfirmed += OnGameOverConfirmed;
    }
    void OnDisable(){  
        game_manage.OnGameOverConfirmed -= OnGameOverConfirmed;
    }

    void OnGameOverConfirmed(){
        for (int i = 0;i < poolObjects.Length;i++){
            poolObjects[i].Dispose();
            poolObjects[i].transform.position = Vector3.one * 1000;
        }
        if(spawnImmediate){
            SpawnImmediate();
        }  
    }
    void Update(){
        Shift();
        if(game.gameOver == false){
        spawnTimer += Time.deltaTime;}
        if(game.gameOver == true){
            spawnTimer = spawnTimer;
        }
        if(spawnTimer > spawnRate){
            Spawn();
            spawnTimer = 0;
        }

    }

    void Configure(){
        targetAspect = targetAspectRatio.x / targetAspectRatio.y;
        poolObjects = new PoolObject[poolSize];
        for (int i = 0; i < poolObjects.Length; i++){
             GameObject go = Instantiate(Prefab) as GameObject;
             Transform t = go.transform;
             t.SetParent(transform);
             /*Vector3 pos = Vector3.zero;
             pos.x = defaultSpawnPos.x;
             pos.y = defaultSpawnPos.y;
             */
             t.position = Vector3.one*1000;
             poolObjects[i] = new PoolObject(t);

        }
        if(spawnImmediate){
            SpawnImmediate();
        }
    }

    void Spawn(){
        Transform t = GetPoolObject();

        if (t == null) return;
        Vector3 pos = Vector3.zero;
        if (min == max){
            pos.x = (min * Camera.main.aspect) / targetAspect;
        }
        if (min != max ){
            pos.x = Random.Range(min,max);
        }
        pos.y = defaultSpawnPos.y;
        t.position = pos;
    }

    void SpawnImmediate(){
        Transform t = GetPoolObject();
        if (t == null) return;
        Vector3 pos = Vector3.zero;
        pos.x = (immediateSpawnPos.x * Camera.main.aspect) / targetAspect;
        pos.y = defaultSpawnPos.y;
        t.position = pos;
        Spawn();
        }
    void Shift(){
        if (game.gameOver == false){
        for (int i = 0;i < poolObjects.Length;i++){
            poolObjects[i].transform.localPosition += -Vector3.right * shiftSpeed * Time.deltaTime;
            CheckDisposeObject(poolObjects[i]);
        }
        }
    }

    void CheckDisposeObject(PoolObject poolObject){
        if(poolObject.transform.position.x < -defaultSpawnPos.x * Camera.main.aspect / targetAspect) {
            poolObject.Dispose();
            poolObject.transform.position = Vector3.one*1000;
        }
    }

    Transform GetPoolObject(){
        
        for (int i = 0; i < poolObjects.Length;i++){
            if (!poolObjects[i].inUse){
                poolObjects[i].Use();
                return poolObjects[i].transform;
            }
        }
        return null;
    }



}
