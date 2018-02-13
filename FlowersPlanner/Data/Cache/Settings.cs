using System;
using Plugin.Settings.Abstractions;
using Plugin.Settings;
namespace FlowersPlanner.Data.Cache
{
    public class Settings
    {
        private readonly ISettings _internal;

        public Settings(ISettings settings = null)
        {
            _internal = settings ?? CrossSettings.Current;
        }

        #region Setting Constants

        private const string ApiTokenKey = "apiTokenKey";
        private static readonly string SettingsDefault = string.Empty;

        #endregion

        public string ApiToken
        {
            get => _internal.GetValueOrDefault(ApiTokenKey, SettingsDefault);
            set => _internal.AddOrUpdateValue(ApiTokenKey, value);
        }
    }
}
