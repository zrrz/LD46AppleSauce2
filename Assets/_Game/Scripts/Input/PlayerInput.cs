using UnityEngine;
using System.Collections;

public class PlayerInput : BaseInput
{
    public static PlayerInput Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void OnEnable()
    {
        Rewired.Player player = Rewired.ReInput.players.GetPlayer(0);
        player.AddInputEventDelegate(OnInputUpdate, Rewired.UpdateLoopType.Update);
    }

    void OnInputUpdate(Rewired.InputActionEventData data)
    {
        switch (data.actionId)
        {
            case RewiredConsts.Action.MoveHorizontal:
                moveHorizontal = new AxisAction(data);
                break;
            case RewiredConsts.Action.MoveVertical:
                moveVertical = new AxisAction(data);
                break;
            case RewiredConsts.Action.LookHorizontal:
                lookHorizontal = new AxisAction(data);
                break;
            case RewiredConsts.Action.LookVertical:
                lookVertical = new AxisAction(data);
                break;
            case RewiredConsts.Action.Sprint:
                sprint = new ButtonAction(data);
                break;
            case RewiredConsts.Action.Interact:
                interact = new ButtonAction(data);
                break;
            case RewiredConsts.Action.Jump:
                jump = new ButtonAction(data);
                break;
            default:
                break;
        }
    }
}