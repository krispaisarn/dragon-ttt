using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TTT.Character
{
    public class CharacterManager : BaseMono
    {
        [SerializeField] private CharacterController _xCharacter;
        [SerializeField] private CharacterController _oCharacter;

        private CharacterController _characterController;
        private bool isInitialized;
        public override void Initialize()
        {
            if (isInitialized)
                return;

            _xCharacter.Initialize();
            _oCharacter.Initialize();

            isInitialized = true;
        }

        public void Reset()
        {
            _xCharacter.PlayIdle();
            _oCharacter.PlayIdle();
        }

        public void PlayAnimation(MarkType markType, CharAnim charAnim)
        {
            if (markType == MarkType.X)
                _characterController = _xCharacter;
            else if (markType == MarkType.O)
                _characterController = _oCharacter;

            switch (charAnim)
            {
                case CharAnim.Idle:
                    _characterController.PlayIdle();
                    break;
                case CharAnim.Random:
                    _characterController.PlayRandom();
                    break;
                case CharAnim.Mark:
                    _characterController.PlayPlaceMark();
                    break;
                case CharAnim.WinRound:
                    _characterController.PlayWinRound();
                    break;
                case CharAnim.LoseRound:
                    _characterController.PlayLoseRound();
                    break;
                case CharAnim.WinGame:
                    _characterController.PlayWin();
                    break;
                case CharAnim.LoseGame:
                    _characterController.PlayLose();
                    break;
                case CharAnim.OnTurn:
                    _characterController.PlayOnTurn();
                    break;
            }

        }

        public enum CharAnim
        {
            Idle,
            Random,
            Mark,
            WinRound,
            LoseRound,
            WinGame,
            LoseGame,
            OnTurn
        }
    }
}
