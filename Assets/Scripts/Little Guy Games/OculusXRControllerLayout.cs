using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

/*============================================================
**
** Class:  OculusXRControllerLayout
**
** Purpose: This class maps all the Oculus Quest 2's inputs
** so other classes may get InputActions from just passing
** enums.
**
** NOTE: As of right now (January 24th 2023), each
** InputActionProperty refers to InputActions defined in
** XR Interaction Toolkit's ActionMaps.
**
** Author: Lîf Gwaethrakindo
**
==============================================================*/

namespace LittleGuyGames.XR
{
public enum AxisType { Axis1D, Axis2D, Axis3D }

public enum OQ2InputKey
{
    None,
    LeftStickClick,
    RightStickClick,
    A,
    B,
    X,
    Y,
    LeftIndex,
    LeftHand,
    RightIndex,
    RightHand,
    Start,
    Oculus
}

public enum OQ2InputValue
{
    None = OQ2InputKey.None,
    LeftStickClick = OQ2InputKey.LeftStickClick,
    RightStickClick = OQ2InputKey.RightStickClick,
    A = OQ2InputKey.A,
    B = OQ2InputKey.B,
    X = OQ2InputKey.X,
    Y = OQ2InputKey.Y,
    LeftIndex = OQ2InputKey.LeftIndex,
    LeftHand = OQ2InputKey.LeftHand,
    RightIndex = OQ2InputKey.RightIndex,
    RightHand = OQ2InputKey.RightHand,
    Start = OQ2InputKey.Start,
    Oculus = OQ2InputKey.Oculus,
    LeftStickXLeft,
    LeftStickXRight,
    LeftStickYDown,
    LeftStickYUp,
    RightStickXLeft,
    RightStickXRight,
    RightStickYDown,
    RightStickYUp
}

//[Flags]
public enum OQ2Input2DAxes
{
    None = 0,
    LeftStick = 1,
    RightStick = 2,

    //Both = LeftStick | RightStick
}

//[Flags]
public enum OQ2Input1DAxis
{
    None = 0,
    LeftStickX = 1,
    LeftStickY = 2,
    RightStickX = 4,
    RightStickY = 8,
    Indices = 16,
    Hands = 32,

    /*LeftStick = LeftStickX | LeftStickY,
    RightStick = RightStickX | RightStickY*/
}

[CreateAssetMenu(fileName = "OculusControllerLayout_", menuName = "Little Guy Games / Oculus Quest 2's Controller Layout")]
public class OculusXRControllerLayout : ScriptableObject
{
    [SerializeField] private InputActionProperty _leftJoystickAction;               /// <summary>Left Joystick's Input Action.</summary>
    [SerializeField] private InputActionProperty _rightJoystickAction;              /// <summary>Right Joystick's Input Action.</summary>
    [SerializeField] private InputActionProperty _leftJoystickClickAction;          /// <summary>Left Joystick Click's Input Action.</summary>
    [SerializeField] private InputActionProperty _rightJoystickClickAction;         /// <summary>Right Joystick Click's Input Action.</summary>
    [SerializeField] private InputActionProperty _leftPrimaryIndexButton;           /// <summary>Left Primary-Index' Button.</summary>
    [SerializeField] private InputActionProperty _rightPrimaryIndexButton;          /// <summary>Right Primary-Index' Button.</summary>
    [SerializeField] private InputActionProperty _leftPrimaryHandButton;            /// <summary>Left Primary-Hand' Button.</summary>
    [SerializeField] private InputActionProperty _rightPrimaryHandButton;           /// <summary>Right Primary-Hand' Button.</summary>
    [SerializeField] private InputActionProperty _aButtonAction;                    /// <summary>A Button's Input Action.</summary>
    [SerializeField] private InputActionProperty _bButtonAction;                    /// <summary>B Button's Input Action.</summary>
    [SerializeField] private InputActionProperty _xButtonAction;                    /// <summary>X Button's Input Action.</summary>
    [SerializeField] private InputActionProperty _yButtonAction;                    /// <summary>Y Button's Input Action.</summary>
    [SerializeField] private InputActionProperty _startButtonAction;                /// <summary>Start Button's Input Action.</summary>
    [SerializeField] private InputActionProperty _oculusButtonAction;               /// <summary>Oculus Button's Input Action.</summary>
    [SerializeField][Range(0.0f, 1.0f)] private float _joystickValueThreshold;      /// <summary>Joystick Value's Threshold.</summary>

    /// <summary>Gets leftJoystickAction property.</summary>
    public InputActionProperty leftJoystickAction { get { return _leftJoystickAction; } }

    /// <summary>Gets rightJoystickAction property.</summary>
    public InputActionProperty rightJoystickAction { get { return _rightJoystickAction; } }

    /// <summary>Gets leftJoystickClickAction property.</summary>
    public InputActionProperty leftJoystickClickAction { get { return _leftJoystickClickAction; } }

    /// <summary>Gets rightJoystickClickAction property.</summary>
    public InputActionProperty rightJoystickClickAction { get { return _rightJoystickClickAction; } }

    /// <summary>Gets leftPrimaryIndexButton property.</summary>
    public InputActionProperty leftPrimaryIndexButton { get { return _leftPrimaryIndexButton; } }

    /// <summary>Gets rightPrimaryIndexButton property.</summary>
    public InputActionProperty rightPrimaryIndexButton { get { return _rightPrimaryIndexButton; } }

    /// <summary>Gets leftPrimaryHandButton property.</summary>
    public InputActionProperty leftPrimaryHandButton { get { return _leftPrimaryHandButton; } }

    /// <summary>Gets rightPrimaryHandButton property.</summary>
    public InputActionProperty rightPrimaryHandButton { get { return _rightPrimaryHandButton; } }

    /// <summary>Gets aButtonAction property.</summary>
    public InputActionProperty aButtonAction { get { return _aButtonAction; } }

    /// <summary>Gets bButtonAction property.</summary>
    public InputActionProperty bButtonAction { get { return _bButtonAction; } }

    /// <summary>Gets xButtonAction property.</summary>
    public InputActionProperty xButtonAction { get { return _xButtonAction; } }

    /// <summary>Gets yButtonAction property.</summary>
    public InputActionProperty yButtonAction { get { return _yButtonAction; } }

    /// <summary>Gets startButtonAction property.</summary>
    public InputActionProperty startButtonAction { get { return _startButtonAction; } }

    /// <summary>Gets oculusButtonAction property.</summary>
    public InputActionProperty oculusButtonAction { get { return _oculusButtonAction; } }

    /// <summary>Gets joystickValueThreshold property.</summary>
    public float joystickValueThreshold { get { return _joystickValueThreshold; } }

    /// <summary>Resets OculusXRControllerLayout's instance to its default values.</summary>
    private void Reset()
    {
        _joystickValueThreshold = 0.35f;
    }

#region InputActionRequesters:
    /// <summary>Gets InputAction from OQ2InputKey value.</summary>
    /// <param name="_key">Key as OQ2InputKey.</param>
    public InputAction GetInputAction(OQ2InputKey _key)
    {
        switch(_key)
        {
            case OQ2InputKey.LeftStickClick:    return leftJoystickAction.action;
            case OQ2InputKey.RightStickClick:   return rightJoystickAction.action;
            case OQ2InputKey.A:                 return aButtonAction.action;
            case OQ2InputKey.B:                 return bButtonAction.action;
            case OQ2InputKey.X:                 return xButtonAction.action;
            case OQ2InputKey.Y:                 return yButtonAction.action;
            case OQ2InputKey.LeftIndex:         return leftPrimaryIndexButton.action;
            case OQ2InputKey.LeftHand:          return leftPrimaryHandButton.action;
            case OQ2InputKey.RightIndex:        return rightPrimaryIndexButton.action;
            case OQ2InputKey.RightHand:         return rightPrimaryHandButton.action;
            case OQ2InputKey.Start:             return startButtonAction.action;
            case OQ2InputKey.Oculus:            return oculusButtonAction.action;
            default:                            return null;
        }
    }

    /// <summary>Gets InputAction from OQ2InputValue value.</summary>
    /// <param name="_axis">Axis as OQ2InputValue.</param>
    public InputAction GetValueInputAction(OQ2InputValue _axis)
    {
        switch(_axis)
        {
            case OQ2InputValue.LeftStickClick:    return null;
            case OQ2InputValue.RightStickClick:   return null;
            case OQ2InputValue.A:                 return aButtonAction.action;
            case OQ2InputValue.B:                 return bButtonAction.action;
            case OQ2InputValue.X:                 return xButtonAction.action;
            case OQ2InputValue.Y:                 return yButtonAction.action;
            case OQ2InputValue.LeftIndex:         return leftPrimaryIndexButton.action;
            case OQ2InputValue.LeftHand:          return leftPrimaryHandButton.action;
            case OQ2InputValue.RightIndex:        return rightPrimaryIndexButton.action;
            case OQ2InputValue.RightHand:         return rightPrimaryHandButton.action;
            case OQ2InputValue.Start:             return startButtonAction.action;
            case OQ2InputValue.Oculus:            return oculusButtonAction.action;
            case OQ2InputValue.LeftStickXLeft:    return leftJoystickAction.action;
            case OQ2InputValue.LeftStickXRight:   return leftJoystickAction.action;
            case OQ2InputValue.LeftStickYDown:    return leftJoystickAction.action;
            case OQ2InputValue.LeftStickYUp:      return leftJoystickAction.action;
            case OQ2InputValue.RightStickXLeft:   return rightJoystickAction.action;
            case OQ2InputValue.RightStickXRight:  return rightJoystickAction.action;
            case OQ2InputValue.RightStickYDown:   return rightJoystickAction.action;
            case OQ2InputValue.RightStickYUp:     return rightJoystickAction.action;
            default:                              return null;
        }
    }

    /// <summary>Gets 1D Axis lecture from OQ2Input1DAxis value.</summary>
    /// <param name="_axis">Axis as OQ2Input1DAxis.</param>
    public InputAction Get1DAxesInputAction(OQ2Input1DAxis _axis)
    {
        switch(_axis)
        {
            case OQ2Input1DAxis.LeftStickX:
            case OQ2Input1DAxis.LeftStickY:     return leftJoystickAction.action;

            case OQ2Input1DAxis.RightStickX:
            case OQ2Input1DAxis.RightStickY:    return rightJoystickAction.action;

            default:                            return null;
        }
    }

    /// <summary>Gets InputAction from OQ2Input2DAxes value.</summary>
    /// <param name="_axes">Axes as OQ2Input2DAxes.</param>
    public InputAction Get2DAxesInputAction(OQ2Input2DAxes _axes)
    {
        switch(_axes)
        {
            case OQ2Input2DAxes.LeftStick:     return leftJoystickAction.action;
            case OQ2Input2DAxes.RightStick:    return rightJoystickAction.action;
            default:                           return null;
        }
    }
#endregion

#region ValueRequesters:
    /// <summary>Gets 2D Axes from OQ2Input2DAxes value.</summary>
    /// <param name="_axes">OQ2Input2DAxes value.</param>
    public Vector2 Get2DAxesValue(OQ2Input2DAxes _axes)
    {
        InputAction action = Get2DAxesInputAction(_axes);
        Vector2 a = Vector2.zero;

        if(action != null) switch(_axes)
        {
            case OQ2Input2DAxes.LeftStick:
                a = action.ReadValue<Vector2>();
            break;

            case OQ2Input2DAxes.RightStick:
                a = action.ReadValue<Vector2>();
            break;
        }

        return a;
    }

    /// <summary>Get float value from OQ2InputAxis value.</summary>
    /// <param name="_axis">OQ2InputAxis value.</param>
    /// <returns>Value between -1.0f and 1.0f.</returns>
    public float Get1DAxisValue(OQ2Input1DAxis _axis)
    {
        float x = 0.0f;

        if(_axis == OQ2Input1DAxis.Indices || _axis == OQ2Input1DAxis.Hands)
        {
            switch(_axis)
            {
                case OQ2Input1DAxis.Indices:
                    x = (GetFloatValue(OQ2InputValue.LeftIndex) * -1.0f) + GetFloatValue(OQ2InputValue.RightIndex);
                break;

                case OQ2Input1DAxis.Hands:
                    x = (GetFloatValue(OQ2InputValue.LeftHand) * -1.0f) + GetFloatValue(OQ2InputValue.RightHand);
                break;
            }
        }
        else
        {
            InputAction action = Get1DAxesInputAction(_axis);

            if(action != null) switch(_axis)
            {
                case OQ2Input1DAxis.LeftStickX:
                case OQ2Input1DAxis.RightStickX:
                    x = action.ReadValue<Vector2>().x;
                break;

                case OQ2Input1DAxis.LeftStickY:
                case OQ2Input1DAxis.RightStickY:
                    x = action.ReadValue<Vector2>().y;
                break;
            }
        }
        return x;
    }

    /// <summary>Gets float value from provided OQ2InputValue value.</summary>
    /// <param name="_value">OQ2InputValue value.</param>
    /// <returns>Float value [either 0.0f or 1.0f].</returns>
    public float GetFloatValue(OQ2InputValue _value)
    {
        InputAction action = GetValueInputAction(_value);
        float x = 0.0f;

        if(action != null) switch(_value)
        {
            case OQ2InputValue.LeftStickXLeft:
            case OQ2InputValue.RightStickXLeft:
                x = action.ReadValue<Vector2>().x < -joystickValueThreshold ? 1.0f : 0.0f;
            break;

            case OQ2InputValue.LeftStickXRight:
            case OQ2InputValue.RightStickXRight:
                x = action.ReadValue<Vector2>().x > joystickValueThreshold ? 1.0f : 0.0f;
            break;
            
            case OQ2InputValue.LeftStickYDown:
            case OQ2InputValue.RightStickYDown:
                x = action.ReadValue<Vector2>().y < -joystickValueThreshold ? 1.0f : 0.0f;
            break;

            case OQ2InputValue.LeftStickYUp:
            case OQ2InputValue.RightStickYUp:
                x = action.ReadValue<Vector2>().y > joystickValueThreshold ? 1.0f : 0.0f;
            break;

            default:
                x = action.ReadValue<float>();
            break;
        }

        return x;
    }
#endregion
}
}