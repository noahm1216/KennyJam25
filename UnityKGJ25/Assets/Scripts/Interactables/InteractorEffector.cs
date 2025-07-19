using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                print("CRASH OBJ");
                break;
            case CustomInteractorData.INTERACTOR_EFFECTS.WIN:
                print("WIN OBJ");
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

}
