using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace Strawhat.Games
{

    #region XINPUT APIs

    [Flags]
    public enum ButtonFlags : int
    {
        XINPUT_GAMEPAD_DPAD_UP = 0x0001,
        XINPUT_GAMEPAD_DPAD_DOWN = 0x0002,
        XINPUT_GAMEPAD_DPAD_LEFT = 0x0004,
        XINPUT_GAMEPAD_DPAD_RIGHT = 0x0008,

        XINPUT_GAMEPAD_START = 0x0010,
        XINPUT_GAMEPAD_BACK = 0x0020,

        XINPUT_GAMEPAD_LEFT_THUMB = 0x0040,
        XINPUT_GAMEPAD_RIGHT_THUMB = 0x0080,

        XINPUT_GAMEPAD_LEFT_SHOULDER = 0x0100,
        XINPUT_GAMEPAD_RIGHT_SHOULDER = 0x0200,
        //XINPUT_GAMEPAD_LEFT_STICK = 0x0400,
        //XINPUT_GAMEPAD_RIGHT_STICK = 0x0800,

        XINPUT_GAMEPAD_A = 0x1000,
        XINPUT_GAMEPAD_B = 0x2000,
        XINPUT_GAMEPAD_X = 0x4000,
        XINPUT_GAMEPAD_Y = 0x8000
    }

    [Flags]
    public enum ControllerSubtypes
    {
        XINPUT_DEVSUBTYPE_UNKNOWN = 0x00,
        XINPUT_DEVSUBTYPE_WHEEL = 0x02,
        XINPUT_DEVSUBTYPE_ARCADE_STICK = 0x03,
        XINPUT_DEVSUBTYPE_FLIGHT_STICK = 0x04,
        XINPUT_DEVSUBTYPE_DANCE_PAD = 0x05,
        XINPUT_DEVSUBTYPE_GUITAR = 0x06,
        XINPUT_DEVSUBTYPE_GUITAR_ALTERNATE = 0x07,
        XINPUT_DEVSUBTYPE_DRUM_KIT = 0x08,
        XINPUT_DEVSUBTYPE_GUITAR_BASS = 0x0B,
        XINPUT_DEVSUBTYPE_ARCADE_PAD = 0x13
    }

    public enum BatteryTypes : byte
    {
        BATTERY_TYPE_DISCONNECTED = 0x00,
        BATTERY_TYPE_WIRED = 0x01,
        BATTERY_TYPE_ALKALINE = 0x02,
        BATTERY_TYPE_NIMH = 0x03,
        BATTERY_TYPE_UNKNOWN = 0xFF
    }

    public enum BatteryLevel : byte
    {
        BATTERY_LEVEL_EMPTY = 0x00,
        BATTERY_LEVEL_LOW = 0x01,
        BATTERY_LEVEL_MEDIUM = 0x02,
        BATTERY_LEVEL_FULL = 0x03
    }

    public enum BatteryDeviceType : byte
    {
        BATTERY_DEVTYPE_GAMEPAD = 0x00,
        BATTERY_DEVTYPE_HEADSET = 0x01,
    }

    public static class XInputConstants
    {
        public const int XINPUT_DEVTYPE_GAMEPAD = 0x01;
        public const int XINPUT_DEVSUBTYPE_GAMEPAD = 0x01;

        public enum CapabilityFlags
        {
            XINPUT_CAPS_VOICE_SUPPORTED = 0x0004,

            //Win8 Only
            XINPUT_CAPS_FFB_SUPPORTED = 0x0001,
            XINPUT_CAPS_WIRELESS = 0x0002,
            XINPUT_CAPS_PMD_SUPPORTED = 0x0008,
            XINPUT_CAPS_NO_NAVIGATION = 0x0010,
        }

        public const int XINPUT_GAMEPAD_LEFT_THUMB_DEADZONE = 7849;
        public const int XINPUT_GAMEPAD_RIGHT_THUMB_DEADZONE = 8689;
        public const int XINPUT_GAMEPAD_TRIGGER_THRESHOLD = 30;

        public const int XINPUT_FLAG_GAMEPAD = 0x0001;
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct XInputGamepad
    {
        [MarshalAs(UnmanagedType.I2)]
        [FieldOffset(0)]
        public short wButtons;

        [MarshalAs(UnmanagedType.I1)]
        [FieldOffset(2)]
        public short bLeftTrigger;

        [MarshalAs(UnmanagedType.I1)]
        [FieldOffset(3)]
        public short bRightTrigger;

        [MarshalAs(UnmanagedType.I2)]
        [FieldOffset(4)]
        public short sThumbLX;

        [MarshalAs(UnmanagedType.I2)]
        [FieldOffset(6)]
        public short sThumbLY;

        [MarshalAs(UnmanagedType.I2)]
        [FieldOffset(8)]
        public short sThumbRX;

        [MarshalAs(UnmanagedType.I2)]
        [FieldOffset(10)]
        public short sThumbRY;

        public bool IsButtonPressed(int buttonFlags)
        {
            return (wButtons & buttonFlags) == buttonFlags;
        }

        public bool IsButtonPresent(int buttonFlags)
        {
            return (wButtons & buttonFlags) == buttonFlags;
        }

        public void Copy(XInputGamepad source)
        {
            sThumbLX = source.sThumbLX;
            sThumbLY = source.sThumbLY;
            sThumbRX = source.sThumbRX;
            sThumbRY = source.sThumbRY;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is XInputGamepad))
                return false;

            XInputGamepad source = (XInputGamepad)obj;
            return ((sThumbLX == source.sThumbLX)
            && (sThumbLY == source.sThumbLY)
            && (sThumbRX == source.sThumbRX)
            && (sThumbRY == source.sThumbRY)
            && (bLeftTrigger == source.bLeftTrigger)
            && (bRightTrigger == source.bRightTrigger)
            && (wButtons == source.wButtons));
        }

    }

    [StructLayout(LayoutKind.Sequential)]
    public struct XInputVibration
    {
        [MarshalAs(UnmanagedType.I2)]
        public ushort LeftMotorSpeed;

        [MarshalAs(UnmanagedType.I2)]
        public ushort RightMotorSpeed;
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct XInputState
    {
        [FieldOffset(0)]
        public int PacketNumber;

        [FieldOffset(4)]
        public XInputGamepad Gamepad;

        public void Copy(XInputState source)
        {
            PacketNumber = source.PacketNumber;
            Gamepad.Copy(source.Gamepad);
        }

        public override bool Equals(object obj)
        {
            if ((obj == null) || (!(obj is XInputState)))
                return false;
            XInputState source = (XInputState)obj;

            return ((PacketNumber == source.PacketNumber)
                && (Gamepad.Equals(source.Gamepad)));
        }
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct XInputCapabilities
    {
        [MarshalAs(UnmanagedType.I1)]
        [FieldOffset(0)]
        byte Type;

        [MarshalAs(UnmanagedType.I1)]
        [FieldOffset(1)]
        public byte SubType;

        [MarshalAs(UnmanagedType.I2)]
        [FieldOffset(2)]
        public short Flags;


        [FieldOffset(4)]
        public XInputGamepad Gamepad;

        [FieldOffset(16)]
        public XInputVibration Vibration;
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct XInputBatteryInformation
    {
        [MarshalAs(UnmanagedType.I1)]
        [FieldOffset(0)]
        public byte BatteryType;

        [MarshalAs(UnmanagedType.I1)]
        [FieldOffset(1)]
        public byte BatteryLevel;
    }
    #endregion

    public static class XInputAPIs
    {
#if WINDOWS7
        [DllImport("xinput9_1_0.dll")]
        public static extern int XInputGetState
        (
            int dwUserIndex,
            ref XInputState pState
        );

        [DllImport("xinput9_1_0.dll")]
        public static extern int XInputSetState
        (
            int dwUserIndex,
            ref XInputVibration pVibration
        );

        [DllImport("xinput9_1_0.dll")]
        public static extern int XInputGetCapabilities
        (
            int dwUserIndex,
            int dwFlags,
            ref XInputCapabilities pCapabilities
        );
#else
        [DllImport("xinput1_4.dll")]
        public static extern int XInputGetState
        (
            int dwUserIndex,  // [in] Index of the gamer associated with the device
            ref XInputState pState        // [out] Receives the current state
        );

        [DllImport("xinput1_4.dll")]
        public static extern int XInputSetState
        (
            int dwUserIndex,  // [in] Index of the gamer associated with the device
            ref XInputVibration pVibration    // [in, out] The vibration information to send to the controller
        );

        [DllImport("xinput1_4.dll")]
        public static extern int XInputGetCapabilities
        (
            int dwUserIndex,   // [in] Index of the gamer associated with the device
            int dwFlags,       // [in] Input flags that identify the device type
            ref XInputCapabilities pCapabilities  // [out] Receives the capabilities
        );


        [DllImport("xinput1_4.dll")]
        public static extern int XInputGetBatteryInformation
        (
              int dwUserIndex,        // Index of the gamer associated with the device
              byte devType,            // Which device on this user index
            ref XInputBatteryInformation pBatteryInformation // Contains the level and types of batteries
        );

        [DllImport("xinput1_4.dll")]
        public static extern int XInputGetKeystroke
        (
            int dwUserIndex,
            int dwReserved,
            ref XInputKeystroke pKeystroke
        );
#endif
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct XInputKeystroke
    {
        [MarshalAs(UnmanagedType.I2)]
        [FieldOffset(0)]
        public short VirtualKey;

        [MarshalAs(UnmanagedType.I2)]
        [FieldOffset(2)]
        public char Unicode;

        [MarshalAs(UnmanagedType.I2)]
        [FieldOffset(4)]
        public short Flags;

        [MarshalAs(UnmanagedType.I2)]
        [FieldOffset(5)]
        public byte UserIndex;

        [MarshalAs(UnmanagedType.I1)]
        [FieldOffset(6)]
        public byte HidCode;
    }

    public class XBoxControllerStateChangedEventArgs : EventArgs
    {
        public XInputState CurrentInputState { get; set; }
        public XInputState PreviousInputState { set; get; }
    }

    public class XBoxController
    {
        int _playerIndex;
        static bool keepRunning;
        static int updateFrequency;
        static int waitTime;
        static bool isRunning;
        static object SyncLock;
        static Thread pollingThread;

        bool _stopMotorTimerActive;
        DateTime _stopMotorTime;

        XInputState gamepadStatePrev = new XInputState();
        XInputState gamepadStateCurrent = new XInputState();

        public static int UpdateFrequency
        {
            get
            {
                return updateFrequency;
            }
            set
            {
                updateFrequency = value;
                waitTime = 1000 / updateFrequency;
            }
        }

        public XInputBatteryInformation BatteryInformationGamepad { internal set; get; }
        public XInputBatteryInformation BatteryInformationHeadset { internal set; get; }

        public const int MAX_CONTROLLER_COUNT = 4;
        public const int FIRST_CONTROLLER_INDEX = 0;
        public const int LAST_CONTROLLER_INDEX = MAX_CONTROLLER_COUNT - FIRST_CONTROLLER_INDEX - 1;

        static XBoxController[] Controllers;

        public event EventHandler<XBoxControllerStateChangedEventArgs> StateChanged = null;

        static XBoxController()
        {
            Controllers = new XBoxController[MAX_CONTROLLER_COUNT];
            SyncLock = new object();
            for (int i = FIRST_CONTROLLER_INDEX; i <= LAST_CONTROLLER_INDEX; i++)
            {
                Controllers[i] = new XBoxController(i);
            }
            UpdateFrequency = 25;
        }

        public static XBoxController RetrieveController(int index)
        {
            return Controllers[index];
        }

        private XBoxController(int playerIndex)
        {
            _playerIndex = playerIndex;
            gamepadStatePrev.Copy(gamepadStateCurrent);
        }

        public void UpdateBatteryState()
        {
            XInputBatteryInformation headset = new XInputBatteryInformation();
            XInputBatteryInformation gamepad = new XInputBatteryInformation();

            XInputAPIs.XInputGetBatteryInformation(_playerIndex, (byte)BatteryDeviceType.BATTERY_DEVTYPE_GAMEPAD, ref gamepad);
            XInputAPIs.XInputGetBatteryInformation(_playerIndex, (byte)BatteryDeviceType.BATTERY_DEVTYPE_HEADSET, ref headset);

            this.BatteryInformationGamepad = gamepad;
            this.BatteryInformationHeadset = headset;
        }

        protected void OnStateChanged()
        {
            if (this.StateChanged != null)
            {
                this.StateChanged(this, new XBoxControllerStateChangedEventArgs()
                {
                    CurrentInputState = gamepadStateCurrent,
                    PreviousInputState = gamepadStatePrev
                });
            }
        }

        public XInputCapabilities GetCapabilities()
        {
            XInputCapabilities capabilities = new XInputCapabilities();
            XInputAPIs.XInputGetCapabilities(_playerIndex, XInputConstants.XINPUT_FLAG_GAMEPAD, ref capabilities);
            return capabilities;
        }

        #region Motor Functions
        public void Vibrate(double leftMotor, double rightMotor)
        {
            Vibrate(leftMotor, rightMotor, TimeSpan.MinValue);
        }

        public void Vibrate(double leftMotor, double rightMotor, TimeSpan length)
        {
            leftMotor = Math.Max(0d, Math.Min(1d, leftMotor));
            rightMotor = Math.Max(0d, Math.Min(1d, rightMotor));

            XInputVibration vibration = new XInputVibration() { LeftMotorSpeed = (ushort)(65535d * leftMotor), RightMotorSpeed = (ushort)(65535d * rightMotor) };
            Vibrate(vibration, length);
        }


        public void Vibrate(XInputVibration strength)
        {
            _stopMotorTimerActive = false;
            XInputAPIs.XInputSetState(_playerIndex, ref strength);
        }

        public void Vibrate(XInputVibration strength, TimeSpan length)
        {
            XInputAPIs.XInputSetState(_playerIndex, ref strength);
            if (length != TimeSpan.MinValue)
            {
                _stopMotorTime = DateTime.Now.Add(length);
                _stopMotorTimerActive = true;
            }
        }
        #endregion

        #region Digital Button States
        public bool IsDPadUpPressed
        {
            get { return gamepadStateCurrent.Gamepad.IsButtonPressed((int)ButtonFlags.XINPUT_GAMEPAD_DPAD_UP); }
        }

        public bool IsDPadDownPressed
        {
            get { return gamepadStateCurrent.Gamepad.IsButtonPressed((int)ButtonFlags.XINPUT_GAMEPAD_DPAD_DOWN); }
        }

        public bool IsDPadLeftPressed
        {
            get { return gamepadStateCurrent.Gamepad.IsButtonPressed((int)ButtonFlags.XINPUT_GAMEPAD_DPAD_LEFT); }
        }

        public bool IsDPadRightPressed
        {
            get { return gamepadStateCurrent.Gamepad.IsButtonPressed((int)ButtonFlags.XINPUT_GAMEPAD_DPAD_RIGHT); }
        }

        public bool IsAPressed
        {
            get { return gamepadStateCurrent.Gamepad.IsButtonPressed((int)ButtonFlags.XINPUT_GAMEPAD_A); }
        }

        public bool IsBPressed
        {
            get { return gamepadStateCurrent.Gamepad.IsButtonPressed((int)ButtonFlags.XINPUT_GAMEPAD_B); }
        }

        public bool IsXPressed
        {
            get { return gamepadStateCurrent.Gamepad.IsButtonPressed((int)ButtonFlags.XINPUT_GAMEPAD_X); }
        }

        public bool IsYPressed
        {
            get { return gamepadStateCurrent.Gamepad.IsButtonPressed((int)ButtonFlags.XINPUT_GAMEPAD_Y); }
        }


        public bool IsBackPressed
        {
            get { return gamepadStateCurrent.Gamepad.IsButtonPressed((int)ButtonFlags.XINPUT_GAMEPAD_BACK); }
        }


        public bool IsStartPressed
        {
            get { return gamepadStateCurrent.Gamepad.IsButtonPressed((int)ButtonFlags.XINPUT_GAMEPAD_START); }
        }


        public bool IsLeftShoulderPressed
        {
            get { return gamepadStateCurrent.Gamepad.IsButtonPressed((int)ButtonFlags.XINPUT_GAMEPAD_LEFT_SHOULDER); }
        }


        public bool IsRightShoulderPressed
        {
            get { return gamepadStateCurrent.Gamepad.IsButtonPressed((int)ButtonFlags.XINPUT_GAMEPAD_RIGHT_SHOULDER); }
        }

        public bool IsLeftStickPressed
        {
            get { return gamepadStateCurrent.Gamepad.IsButtonPressed((int)ButtonFlags.XINPUT_GAMEPAD_LEFT_THUMB); }
        }

        public bool IsRightStickPressed
        {
            get { return gamepadStateCurrent.Gamepad.IsButtonPressed((int)ButtonFlags.XINPUT_GAMEPAD_RIGHT_THUMB); }
        }
        #endregion

        #region Analogue Input States
        public int LeftTrigger
        {
            get { return (int)gamepadStateCurrent.Gamepad.bLeftTrigger; }
        }

        public int RightTrigger
        {
            get { return (int)gamepadStateCurrent.Gamepad.bRightTrigger; }
        }

        /// <summary>
        /// X, Y
        /// </summary>
        public dynamic LeftThumbStick
        {
            get
            {
                return new
                {
                    X = gamepadStateCurrent.Gamepad.sThumbLX,
                    Y = gamepadStateCurrent.Gamepad.sThumbLY,
                };
            }
        }

        /// <summary>
        /// X, Y
        /// </summary>
        public dynamic RightThumbStick
        {
            get
            {
                return new
                {
                    X = gamepadStateCurrent.Gamepad.sThumbLX,
                    Y = gamepadStateCurrent.Gamepad.sThumbLY,
                };
            }
        }
        #endregion

        public bool IsConnected { internal set; get; }

        #region Polling
        public static void StartPolling()
        {
            if (!isRunning)
            {
                lock (SyncLock)
                {
                    if (!isRunning)
                    {
                        pollingThread = new Thread(PollerLoop);
                        pollingThread.Start();
                    }
                }
            }
        }

        public static void StopPolling()
        {
            if (isRunning)
                keepRunning = false;
        }

        static void PollerLoop()
        {
            lock (SyncLock)
            {
                if (isRunning)
                    return;

                isRunning = true;
            }

            keepRunning = true;
            while (keepRunning)
            {
                for (int i = FIRST_CONTROLLER_INDEX; i <= LAST_CONTROLLER_INDEX; i++)
                {
                    Controllers[i].UpdateState();
                }
                Thread.Sleep(updateFrequency);
            }

            lock (SyncLock)
            {
                isRunning = false;
            }
        }

        public void UpdateState()
        {
            int result = XInputAPIs.XInputGetState(_playerIndex, ref gamepadStateCurrent);
            IsConnected = (result == 0);

            UpdateBatteryState();
            if (gamepadStateCurrent.PacketNumber != gamepadStatePrev.PacketNumber)
            {
                OnStateChanged();
            }

            gamepadStatePrev.Copy(gamepadStateCurrent);

            if (_stopMotorTimerActive && (DateTime.Now >= _stopMotorTime))
            {
                XInputVibration stopStrength = new XInputVibration() { LeftMotorSpeed = 0, RightMotorSpeed = 0 };
                XInputAPIs.XInputSetState(_playerIndex, ref stopStrength);
            }
        }
        #endregion


        public override string ToString()
        {
            return _playerIndex.ToString();
        }
    }
}
