﻿using UnityEngine;

namespace Game
{
    public static class GameConstants
    {
        private static readonly string SerialNumberKey = "SERIAL_NUMBER";

        public const string GameModelKey = "model";
        public const string LastLoginDateKey = "LastLoginDate";
        public const string PlayerDataKey = "PlayerData";
        public const string EnergyRestoreLastDateKey = "EnergyRestoreLastDate";
        public const string LoginToTheGameKey = "LoginToTheGame";

        public const string AdsIcon = "<sprite=\"AdsIcon\" index=0>";
        public const string EnergyIcon = "<sprite=\"EnergyIcon\" index=0>";
        public const string GemsIcon = "<sprite=\"GemsIcon\" index=0>";
        public const string CashIcon = "<sprite=\"CashIcon\" index=0>";
        public const string EnemyIcon = "<sprite=\"EnemyIcon\" index=0>";

        public const string YellowColorHex = "#FFE900";
        public const string GreenColorHex = "#00E539";
        public const string RedColorHex = "#FF0040";

        public const int WeaponsMax = 6;
        public const int BulletsMax = 5;

        public static bool IsDebugBuild()
        {
            return Debug.isDebugBuild;
        }

        public static bool IsDebugIPad()
        {
            bool isDeveloperDevice = IsDebugBuild();
            var identifier = SystemInfo.deviceModel;
            bool isiPad = identifier.StartsWith("iPad");
            return isDeveloperDevice && isiPad;
        }

        public static int GetSerial()
        {
            var serial = PlayerPrefs.GetInt(SerialNumberKey, 1000000);
            var serialNext = serial;
            serialNext++;

            PlayerPrefs.SetInt(SerialNumberKey, serialNext);
            PlayerPrefs.Save();

            return serial;
        }
    }
}