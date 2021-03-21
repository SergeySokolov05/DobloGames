using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class CircleManager : MonoBehaviour
{
    [SerializeField] private Circle prefabsCircle;
    [Header("Mesh Size")] 
    [SerializeField] private int x;
    [SerializeField] private int y;
    [SerializeField] [Range(1.2f, 3)] private float offSetDistanceCircle;
    [SerializeField] private Color[] arrayColorCircle;

    private Circle[,] _mesh;
    private Vector3 _lastPositionCircle;
    private float _diameterAndOffset;
    private int _rows;
    private int _columns;
    
    public void CalculateGame()
    {
        _mesh = new Circle[y, x];
        _diameterAndOffset = prefabsCircle.Radius * 2 * offSetDistanceCircle;
        _rows = _mesh.GetUpperBound(0) + 1;
        _columns = _mesh.Length / _rows;

        RunArrayCircle(delegate(int i, int j)
        {
            _mesh[i, j] = CalculateCircle(i,j);
        });
    }
    
    public void UpdateMesh()
    {
        RunArrayCircle(OnDestroyCircle);
    }
    
    public void CalculateWin(int countValueWin)
    {
        RunArrayCircle(delegate(int i, int j)
        {
            if (_mesh[i, j].IsSaveLastPosition)
            {
                OnDestroyCircle(i,j);
            }
        });
        
        GameController.instance.UiManager.UpdateRating(countValueWin);
    }

    private void RunArrayCircle(Action<int, int> action)
    {
        for (var i = 0; i < _rows; i++)
        {
            for (var j = 0; j < _columns; j++)
            {
                action?.Invoke(i,j);
            }
        }
    }
    
    private Circle CalculateCircle(int i, int j)
    {
        Circle circle = Instantiate(prefabsCircle, new Vector3(j * _diameterAndOffset, i * _diameterAndOffset, 0) + GetCenteringCircles(), 
            Quaternion.identity, transform);
        int tempRandomIndexColor = Random.Range(0, arrayColorCircle.Length);
        circle.Initialization(this ,arrayColorCircle[tempRandomIndexColor], tempRandomIndexColor, _diameterAndOffset);
        Sequence sequence = DOTween.Sequence();
        sequence.Append(circle.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutElastic));

        return circle;
    }

    private void OnDestroyCircle(int i, int j)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(_mesh[i, j].transform.DOScale(Vector3.zero, 0.5f))
            .AppendCallback(() =>
            {
                Destroy(_mesh[i, j].gameObject);
                _mesh[i, j] = CalculateCircle(i, j);
            });
    }
    
    private Vector3 GetCenteringCircles()
    {
        float tempCalculateRadius = prefabsCircle.Radius * offSetDistanceCircle;
        float tempX = ((x * _diameterAndOffset) / 2.0f - tempCalculateRadius) * (-1);
        float tempY = ((y * _diameterAndOffset) / 2.0f - tempCalculateRadius) * (-1);
        return new Vector3(tempX, tempY, 0);
    }
}