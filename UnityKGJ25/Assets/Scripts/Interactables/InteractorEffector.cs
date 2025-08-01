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
    public float boostAmount = 5;
    // case CustomInteractorData.INTERACTOR_EFFECTS.CRASH:
    public Rigidbody rbody;
    public Animator animPlayer;
    public ClickAndDrag ref_clickAndDrag;

    // case CustomInteractorData.INTERACTOR_EFFECTS.WIN:

    // case CustomInteractorData.INTERACTOR_EFFECTS.LOSE:

    // case CustomInteractorData.INTERACTOR_EFFECTS.RESET:
    private Vector3 positionStart;
    private Quaternion rotationStart;
    // checkpoints
    private Vector3 positionCheckpoint;
    private Quaternion rotationCheckpoint;


    public UnityEvent onInteractWin, onInteractCrash, onInteractLose, onBoost;

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
        if (!ref_clickAndDrag)
            TryGetComponent(out ref_clickAndDrag);
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
                if (rbody) rbody.AddForce(transform.forward * boostAmount);// - rbody.velocity, ForceMode.VelocityChange);\
                onBoost?.Invoke();
                break;
            case CustomInteractorData.INTERACTOR_EFFECTS.CRASH:
                print("CRASH ON OBJ");
                StoreCheckpoint(_objSendingReactor.position, transform.rotation);
                if (ref_clickAndDrag) ref_clickAndDrag.DockBoat(true);
                 onInteractCrash?.Invoke();
                SimpleCameraEffects.Instance.PlayShakeEffect(3f,5f);
                break;
            case CustomInteractorData.INTERACTOR_EFFECTS.WIN:
                print("WIN ON OBJ");
                onInteractWin?.Invoke();
                if (ref_clickAndDrag) ref_clickAndDrag.ChangeWin(true);
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
