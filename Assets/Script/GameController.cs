using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class GameController : MonoBehaviour
{
   public static GameController instance;

   private CircleManager _circleManager;
   private UIManager _uiManager;
   
   public CircleManager CircleManager => _circleManager;
   public UIManager UiManager => _uiManager;

   private void Awake()
   {
      instance = this;
      DontDestroyOnLoad(gameObject);
      _circleManager = FindObjectOfType<CircleManager>();
      _uiManager = FindObjectOfType<UIManager>();
   }
}
