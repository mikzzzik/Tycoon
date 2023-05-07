using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ZombieWaveSlot : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private List<GameObject> _starsActiveList;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _cooldownText;

    private Wave _nowWave;

    public static Action<Wave> OnChooseWave;

    private void OnEnable()
    {
        if(_nowWave != null)
        _nowWave.OnChangeNowCooldown += ChangeCooldown;
    }

    private void OnDisable()
    {
        _nowWave.OnChangeNowCooldown -= ChangeCooldown;
    }


    public void Init(Wave wave)
    {
        _nowWave = wave;

        _nowWave.OnChangeNowCooldown += ChangeCooldown;

        _slider.value = _nowWave.NowCooldown;
        _slider.maxValue = _nowWave.CoolDown;
        _nameText.text = _nowWave.WaveName;
        _cooldownText.text = _nowWave.NowCooldown.ToString();

        for (int i = 0; i < _starsActiveList.Count; i++)
        {
            if(i < _nowWave.DifficultyStars)
            {

                _starsActiveList[i].SetActive(true);
            }
            else
            {
                _starsActiveList[i].SetActive(false);
            }
        }
    }

    public void Click()
    {
        if (_nowWave.NowCooldown > 0) return;

        OnChooseWave?.Invoke(_nowWave);

    }

    public void ChangeCooldown()
    {
        if(_nowWave != null)
        {
            _slider.value = _nowWave.NowCooldown;
            _cooldownText.text = _nowWave.NowCooldown.ToString();
        }
    }


}
