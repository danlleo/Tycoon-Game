using Managers;
using Unit.UnitStates;
using UnityEngine;

namespace Unit
{
    public class UnitStepSounds : MonoBehaviour
    {
        private global::Unit.Unit _unit;

        private float _footstepTimer;
        private float _footstepTimerMax = .525f;

        private bool _isWalking;

        private void Awake()
        {
            _unit = GetComponent<global::Unit.Unit>();
        }

        private void Start()
        {
            UnitWalkingState.OnUnitBeganWalking += UnitWalkingState_OnUnitBeganWalking;
            UnitWalkingState.OnUnitEndedWalking += UnitWalkingState_OnUnitEndedWalking;
        }

        private void OnDestroy()
        {
            UnitWalkingState.OnUnitBeganWalking -= UnitWalkingState_OnUnitBeganWalking;
            UnitWalkingState.OnUnitEndedWalking -= UnitWalkingState_OnUnitEndedWalking;
        }

        private void UnitWalkingState_OnUnitBeganWalking(object sender, System.EventArgs e)
        {
            global::Unit.Unit senderUnit = (global::Unit.Unit)sender;

            if (ReferenceEquals(senderUnit, _unit))
            {
                _isWalking = true;
            }
        }

        private void UnitWalkingState_OnUnitEndedWalking(object sender, System.EventArgs e)
        {
            global::Unit.Unit senderUnit = (global::Unit.Unit)sender;

            if (ReferenceEquals(senderUnit, _unit))
            {
                _isWalking = false;
                Destroy(this);
            }
        }

        private void Update()
        {
            if (_isWalking)
            {
                _footstepTimer -= Time.deltaTime;

                if (_footstepTimer <= 0)
                {
                    _footstepTimer = _footstepTimerMax;

                    float volume = 1f;

                    SoundManager.Instance.PlayFootStepsSound(transform.position, volume);
                }
            }
        }
    }
}
