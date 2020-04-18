using UnityEngine;
using System.Collections;

public class BaseInput : MonoBehaviour
{
    public class ButtonAction
    {
        public ButtonAction(Rewired.InputActionEventData data)
        {
            this.data = data;
        }

        //TODO long press and single press and stuff
        Rewired.InputActionEventData data;

        /// <summary>
        /// Was pressed this frame
        /// </summary>
        public bool WasPressed => data.GetButtonDown();

        /// <summary>
        /// Is currently pressed
        /// </summary>
        public bool IsPressed => data.GetButton();

        /// <summary>
        /// Was released this frame
        /// </summary>
        public bool Released => data.GetButtonUp();
    }

    public class AxisAction
    {
        public AxisAction(Rewired.InputActionEventData data)
        {
            this.data = data;
        }

        Rewired.InputActionEventData data;
        public float Value => data.GetAxis();
    }

    public Vector3 MoveDir => new Vector3(moveHorizontal != null ? moveHorizontal.Value : 0f, 0f, moveVertical != null ? moveVertical.Value : 0f);

    public Vector3 LookDir => new Vector3(lookHorizontal != null ? lookHorizontal.Value : 0f, 0f, lookVertical != null ? lookVertical.Value : 0f);

    public AxisAction moveHorizontal;
    public AxisAction moveVertical;
    public AxisAction lookHorizontal;
    public AxisAction lookVertical;
    public ButtonAction sprint;
    public ButtonAction aim;
    public ButtonAction shoot;
    public ButtonAction melee;
    public ButtonAction reload;
    public ButtonAction interact;
    public ButtonAction jump;
}
