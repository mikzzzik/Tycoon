using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieWavePanel : MonoBehaviour
{
    [SerializeField] private ZombieWaveSlot _slotPrefab;

    private List<ZombieWaveSlot> _zombieWaveSlotList = new List<ZombieWaveSlot>();

    private bool _firstInit = true;

    private void OnEnable()
    {
        ZombieWaveSlot.OnChooseWave += ChooseWave;
    }

    private void OnDisable()
    {
        ZombieWaveSlot.OnChooseWave -= ChooseWave;
    }

    private List<Wave> _waveList;
    public void Show()
    {
        if(gameObject.activeSelf)
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);

        if(_firstInit)
        {
            _firstInit = false;

            for (int i = 0; i < _waveList.Count; i++)
            {
                ZombieWaveSlot tempSlot = Instantiate(_slotPrefab);
                
                _zombieWaveSlotList.Add(tempSlot);

                tempSlot.transform.SetParent(transform,false);
                tempSlot.Init(_waveList[i]);
            }
        }
        else
        {
            for (int i = 0; i < _zombieWaveSlotList.Count; i++)
            {
                _zombieWaveSlotList[i].ChangeCooldown();
            }
        }
    }

    private void ChooseWave(Wave wave)
    {
        Hide();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void SetWaveList(List<Wave> waveList)
    {
        _waveList = waveList;
    }

}
