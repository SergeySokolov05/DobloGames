using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEditor.SceneManagement;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class Circle : MonoBehaviour
{
    public LineRenderer lineRender;
    [SerializeField] private CircleCollider2D collider2D;
    [SerializeField] private SpriteRenderer spriteRenderer;
    
    private static Circle _nextCircle;
    private static int _countValue;

    private CircleManager circleManager;
    private int _indexColor;
    private float _maxDistanceLine;
    private bool _isSaveLastPosition;

    private int IndexColor => _indexColor;
    public bool IsSaveLastPosition => _isSaveLastPosition;
    public float Radius => collider2D.radius;

    private void OnMouseDown()
    {
        SetLine();
    }

    private void OnMouseDrag()
    {
        if (_nextCircle != null)
        {
            _nextCircle.CalculateLineNextCircle(Input.mousePosition);
            return;
        }
      
        CalculateLineNextCircle(Input.mousePosition);
    }

    private void OnMouseUp()
    {
        if (_nextCircle != null)
        {
            _nextCircle.SetLineLastPosition(_nextCircle.transform.position);
            _nextCircle = null;
        }
        if (_isSaveLastPosition)
        {
            _countValue++;
            circleManager.CalculateWin(_countValue);
            _countValue = 0;
            return;
        }
        
        lineRender.SetPosition(1, transform.position);
        collider2D.enabled = true;
    }
    
    public void Initialization(CircleManager manager ,Color color, int index, float maxDistance)
    {
        circleManager = manager;
        spriteRenderer.color = color;
        _indexColor = index;
        _maxDistanceLine = maxDistance;
    }

    private void CalculateLineNextCircle(Vector3 mousePosition)
    {
        var tempMouseWordPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        lineRender.SetPosition( 1, tempMouseWordPosition);
        
        var tempDistance = Vector3.Distance(transform.position, tempMouseWordPosition);
      
        if (tempDistance > _maxDistanceLine * 0.9f && tempDistance < _maxDistanceLine * 1.1f)
        {
            RaycastHit2D raycast = Physics2D.Raycast(transform.position, tempMouseWordPosition - transform.position, tempDistance * 0.85f);
            if(raycast.collider == null)
                return;
         
            Circle circle = raycast.collider.GetComponent<Circle>();
            if (circle.IndexColor == _indexColor)
            {
                _countValue++;
                AnimationCompound();
                SetLineLastPosition(circle.transform.position);
                _nextCircle = circle;
                _nextCircle.AnimationCompound();

                if (_nextCircle.IsSaveLastPosition)
                    //Вынужденная мера Лине рендер некрасиво работал, когда ты собираешь квадрат, есди больше времени можно что-то придумать
                    _nextCircle.lineRender = Instantiate(_nextCircle.lineRender, _nextCircle.transform);
                
                _nextCircle.SetLine();
            }
        }
    }

    private void SetLine()
    {
        lineRender.endColor = spriteRenderer.color;
        lineRender.startColor = spriteRenderer.color;
        lineRender.SetPosition(0, transform.position);
        lineRender.SetPosition(1, transform.position);
        collider2D.enabled = false;
    }

    private void SetLineLastPosition(Vector3 lastPosition)
    {
        lineRender.SetPosition(1, lastPosition);
        collider2D.enabled = true;
        _isSaveLastPosition = true;
    }

    private void AnimationCompound()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOScale(new Vector3(1.2f, 1.2f, 0), 0.05f))
            .AppendCallback(() => transform.DOScale(Vector3.one, 0.05f));
    }
}