using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// <para>
/// Data that we want all interactors to be able to have affected
/// </para>
/// </summary>
public class InteractorEffector : MonoBehaviour
{
    //case CustomInteractorData.INTERACTOR_EFFECTS.REFLECT:
    //case CustomInteractorData.INTERACTOR_EFFECTS.BOOST:
    // case CustomInteractorData.INTERACTOR_EFFECTS.CRASH:
    public Rigidbody rbody;
    public Animator animPlayer;

    // case CustomInteractorData.INTERACTOR_EFFECTS.WIN:

    // case CustomInteractorData.INTERACTOR_EFFECTS.LOSE:

    // case CustomInteractorData.INTERACTOR_EFFECTS.RESET:
    private Vector3 positionStart;
    private Quaternion rotationStart;
    // checkpoints
    private Vector3 positionCheckpoint;
    private Quaternion rotationCheckpoint;


    public UnityEvent onInteractWin, onInteractCrash, onInteractLose;

    // interaction handler time wait
    private float lastInteractionStamp;
    private float interactionWaitTime = 1f;

    private void OnEnable()
    {
        positionStart = transform.position;
        rotationStart = transform.rotation;

        if (!rbody)
            TryGetComponent(out rbody);
        if (!animPlayer)
            TryGetComponent(out animPlayer);
    }


    public void ReactForInteraction(CustomInteractorData _interactorData, Transform _objSendingReactor)
    {
        if (Time.time < lastInteractionStamp + interactionWaitTime)
            return;

        lastInteractionStamp = Time.time;

        switch (_interactorData.interactorEffect)
        {
            case CustomInteractorData.INTERACTOR_EFFECTS.REFLECT:
                print("REFLECT ON OBJ");
                transform.position = Vector3.Reflect(transform.position, Vector3.right);
                break;
            case CustomInteractorData.INTERACTOR_EFFECTS.BOOST:
                print("BOOST ON OBJ");
                break;
            case CustomInteractorData.INTERACTOR_EFFECTS.CRASH:
                print("CRASH ON OBJ");
                StoreCheckpoint(_objSendingReactor.position, transform.rotation);
                onInteractCrash?.Invoke();                
                break;
            case CustomInteractorData.INTERACTOR_EFFECTS.WIN:
                print("WIN ON OBJ");
                onInteractWin?.Invoke();
                break;
            case CustomInteractorData.INTERACTOR_EFFECTS.LOSE:
                print("LOSE ON OBJ");
                ClearCheckpoint();
                if (rbody) { rbody.velocity = Vector3.zero; rbody.angularVelocity = Vector3.zero; }
                onInteractLose?.Invoke();                
                break;
            case CustomInteractorData.INTERACTOR_EFFECTS.RESET:
                print("RESET ON OBJ");
                break;
            default:
                break;
        }
    }

    public void StoreCheckpoint(Vector3 _checkpointPos, Quaternion _checkpointRot)
    {
        positionCheckpoint = _checkpointPos;
        rotationCheckpoint = _checkpointRot;
    }

    public void ClearCheckpoint()
    {
        positionCheckpoint = Vector3.zero;
        rotationCheckpoint = new Quaternion(0, 0, 0, 0);
    }

    public void TeleportToCheckpoint(float _timeToWait)
    {       
        lastInteractionStamp = Time.time;
        StartCoroutine(MoveToStoredPosition(_timeToWait));
    }

    public IEnumerator MoveToStoredPosition(float _timeToWait) // move to checkpoint, (if any) or move to start position
    {
        yield return new WaitForSeconds(_timeToWait);
        if (rbody) rbody.velocity = Vector3.zero;
        
        if (positionCheckpoint == Vector3.zero)
        { transform.position = positionStart; transform.rotation = rotationStart; }
        else
        { transform.position = positionCheckpoint; transform.rotation = rotationCheckpoint; }
    }

}
