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


    public UnityEvent onInteractWin, onInteractCrash;

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
        switch (_interactorData.interactorEffect)
        {
            case CustomInteractorData.INTERACTOR_EFFECTS.REFLECT:
                print("REFLECT OBJ");
                break;
            case CustomInteractorData.INTERACTOR_EFFECTS.BOOST:
                print("BOOST OBJ");
                break;
            case CustomInteractorData.INTERACTOR_EFFECTS.CRASH:
                onInteractCrash?.Invoke();
                print("CRASH OBJ");
                break;
            case CustomInteractorData.INTERACTOR_EFFECTS.WIN:
                print("WIN OBJ");
                onInteractWin?.Invoke();
                break;
            case CustomInteractorData.INTERACTOR_EFFECTS.LOSE:
                print("LOSE OBJ");
                break;
            case CustomInteractorData.INTERACTOR_EFFECTS.RESET:
                print("RESET OBJ");
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

    public void MoveToStoredPosition() // move to checkpoint, (if any) or move to start position
    {
        if (positionCheckpoint == Vector3.zero)
        { transform.position = positionStart; transform.rotation = rotationStart; }
        else
        { transform.position = positionCheckpoint; transform.rotation = rotationCheckpoint; }
    }

}
