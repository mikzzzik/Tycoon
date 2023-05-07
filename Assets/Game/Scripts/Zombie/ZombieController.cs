using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController : MonoBehaviour
{
    [SerializeField] private Building _townHall;
    
    [SerializeField] private List<Wave> _waveList;

    [SerializeField] private List<SpawnZone> _spawnList;

    [SerializeField] private ZombieWavePanel _wavePanel;

    private void OnEnable()
    {
        ZombieWaveSlot.OnChooseWave += InitWave;
    }

    private void OnDisable()
    {
        ZombieWaveSlot.OnChooseWave -= InitWave;
    }

    private void Awake()
    {
        _wavePanel.SetWaveList(_waveList);
        StartCoroutine(CooldownCoroutine());
    }


    public void InitWave(Wave wave)
    {
        Wave nowWave = wave;
        
        SpawnZone zone = _spawnList[Random.Range(0, _spawnList.Count)];

        nowWave.NowCooldown = nowWave.CoolDown;



        Debug.Log("init");
    }

    public IEnumerator CooldownCoroutine()
    {
        yield return new WaitForSeconds(1);

        for (int i = 0; i < _waveList.Count; i++)
        {
            if (_waveList[i].NowCooldown > 0)
            {
                _waveList[i].NowCooldown--;
            }
        }

        StartCoroutine(CooldownCoroutine());
    }
}

[System.Serializable]
public class SpawnZone
{
    [SerializeField] private List<ZombieSpawnPoint> _spawnPoint;

}

[System.Serializable]

public class Wave 
{
    [SerializeField] private string _waveName;
    [SerializeField] private List<Zombies> _zombies;
    [SerializeField] private int _cooldown;
    [SerializeField] private int _difficultyStars;

    public System.Action OnChangeNowCooldown;

    private int _nowCooldown;

    public string WaveName { get { return _waveName; } }
    public List<Zombies> Zombies { get { return _zombies; } }
    public int CoolDown { get { return _cooldown; } }
    public int DifficultyStars { get { return _difficultyStars; } }
    public int NowCooldown { get { return _nowCooldown; } set { _nowCooldown = value; OnChangeNowCooldown?.Invoke(); } }
}

[System.Serializable]
public class Zombies
{
    [SerializeField] private ZombieTier _zombieTier;
    [SerializeField] private int _zombieCount;

    public ZombieTier ZombieTier { get { return _zombieTier; } }
    public int ZombieCount { get { return _zombieCount; } }
}


    public enum ZombieTier
    {
        Low,
        Medium,
        High
    }