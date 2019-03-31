﻿using System;
using UnityEngine;
using Zenject;

namespace GravityDJ
{
    public class Ball : MonoBehaviour
    {
        public event Action targetHit;

        public void OnTargetHit()
        {
            Destroy(this.gameObject);
            targetHit.Invoke();
        }

        public class Factory:PlaceholderFactory<Ball>
        {
        }
    }
}