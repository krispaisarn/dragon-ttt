using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TTT.Character
{
    public class CharacterController : BaseMono
    {

        public int[] animationIndexList;
        private Animator _animator;

        private string animParam = "animation";
        private bool isInitialized;
        public override void Initialize()
        {
            if (isInitialized)
                return;

            _animator = this.GetComponent<Animator>();
            PlayIdle();

            isInitialized = true;
        }

        public void PlayIdle()
        {
            _animator.SetInteger(animParam, 1);
        }

        public void PlayRandom()
        {
            int index = Random.Range(0, animationIndexList.Length);
            _animator.SetInteger(animParam, index);
        }

        public void PlayPlaceMark()
        {
            _animator.SetInteger(animParam, 2);
        }

        public void PlayWinRound()
        {
            _animator.SetInteger(animParam, 15);
        }

        public void PlayLoseRound()
        {
            _animator.SetInteger(animParam, 3);
        }

        public void PlayWin()
        {
            _animator.SetInteger(animParam, 6);
        }

        public void PlayLose()
        {
            _animator.SetInteger(animParam, 7);
        }
        public void PlayOnTurn()
        {
            _animator.SetInteger(animParam, 5);
        }
    }
}