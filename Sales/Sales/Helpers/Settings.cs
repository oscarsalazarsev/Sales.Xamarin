﻿using System;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace Sales.Helpers
{
    /// <summary>
    /// This is the Settings static class that can be used in your Core solution or in any
    /// of your client applications. All settings are laid out the same exact way with getters
    /// and setters. 
    /// </summary>
    public static class Settings
    {
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        #region Setting Constants

        private const string tokenType = "TokenType";
        private const string accessToken = "AccessToken";
        private const string isRemembered = "IsRemembered";
        private const string issued = "Issued";
        private const string expires = "Expires";
        private static readonly string stringDefault = string.Empty;
        private static readonly bool booleanDefault = false;
        private static readonly DateTime datetimeDefault = DateTime.MinValue;
        private const string userASP = "UserASP";

        #endregion

        #region Properties

        public static string TokenType
        {
            get
            {
                return AppSettings.GetValueOrDefault(tokenType, stringDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(tokenType, value);
            }
        }

        public static string AccessToken
        {
            get
            {
                return AppSettings.GetValueOrDefault(accessToken, stringDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(accessToken, value);
            }
        }

        public static bool IsRemembered
        {
            get
            {
                return AppSettings.GetValueOrDefault(isRemembered, booleanDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(isRemembered, value);
            }
        }

        public static DateTime Issued
        {
            get
            {
                return AppSettings.GetValueOrDefault(issued, datetimeDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(issued, value);
            }
        }

        public static DateTime Expires
        {
            get
            {
                return AppSettings.GetValueOrDefault(expires, datetimeDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(expires, value);
            }
        }

        public static string UserASP
        {
            get
            {
                return AppSettings.GetValueOrDefault(userASP, stringDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(userASP, value);
            }
        }

        #endregion

    }
}
