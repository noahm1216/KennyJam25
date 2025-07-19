using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{  
    public List<CustomInteractorData> interactorRules = new List<CustomInteractorData>();

    public void OnTriggerEnter(Collider trig)
    {
        print("trig");
        if (interactorRules.Count == 0)
            return;

        for (int i = 0; i < interactorRules.Count; i++)
            if (interactorRules[i].onTrigger && (interactorRules[i].layersToAffect & (1 << trig.gameObject.layer)) != 0)
                ReactForInteraction(interactorRules[i], trig.transform);
    }

    public void OnCollisionEnter(Collision col)
    {
        print("col");
        if (interactorRules.Count == 0)
            return;

        for (int i = 0; i < interactorRules.Count; i++)
            if (interactorRules[i].onCollision && (interactorRules[i].layersToAffect & (1 << col.gameObject.layer)) != 0)
                ReactForInteraction(interactorRules[i], col.transform);
    }

    public void ReactForInteraction(CustomInteractorData _interactorData, Transform _obj)
    {
        InteractorEffector intEffector = null;
        _obj.TryGetComponent(out intEffector);
        if (intEffector)
            intEffector.ReactForInteraction(_interactorData, transform);

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


// the custom data for setting up interaction rules
[System.Serializable]
public class CustomInteractorData
{
    public enum INTERACTOR_EFFECTS { REFLECT, CRASH, BOOST, WIN, LOSE, RESET, }

    public string ruleNickname;
    public INTERACTOR_EFFECTS interactorEffect;
    public LayerMask layersToAffect;
    public bool onCollision;
    public bool onTrigger;

    // other settings here like
    // TIMER
    // DESTROY ON INTERACT
    // ETC...

    //public CustomInteractorData(string _newName,)
    //{
    //    //abilityNickname = _newName;
    //}

}//end of data for platforms
