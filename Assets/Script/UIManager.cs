using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Button buttUpdateMeshSize;
    [SerializeField] private TextMeshProUGUI textRating;

    private int _countRating;
    private string _nameSaveRating = "Rating";
    
    private void Start()
    {
        _countRating = PlayerPrefs.GetInt(_nameSaveRating);
        UpdateRating(_countRating);
    }

    public void StartGame(GameObject gameObject)
    {
        DOTween.Sequence().Append( gameObject.transform.DOScale(Vector3.zero, 0.25f))
            .AppendCallback(()=>
            {
                GameController.instance.CircleManager.CalculateGame();
                buttUpdateMeshSize.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutElastic);
                buttUpdateMeshSize.onClick.AddListener(UpdateMeshSize);
                textRating.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutElastic);
            });
    }

    public void UpdateRating(int value)
    {
        _countRating = value != _countRating ? value + _countRating : _countRating;
        textRating.text = "Rating: " + _countRating.ToString("f0");
        
        PlayerPrefs.SetInt(_nameSaveRating, _countRating);
        PlayerPrefs.Save();
    }

    private void UpdateMeshSize()
    {
        GameController.instance.CircleManager.UpdateMesh();
    }
}
