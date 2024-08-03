using H.MQ.Core;
using H.Necessaire;
using Newtonsoft.Json;
using System;

namespace H.MQ
{
    internal class HmqDependencyGroup : ImADependencyGroup
    {
        public void RegisterDependencies(ImADependencyRegistry dependencyRegistry)
        {
            Func<JsonSerializerSettings> baseDefaultSettingFactory = JsonConvert.DefaultSettings;
            JsonConvert.DefaultSettings = () => {
                JsonSerializerSettings baseDefaultSettings = baseDefaultSettingFactory?.Invoke();
                JsonSerializerSettings defaultSettings = baseDefaultSettings is null ? new JsonSerializerSettings() : new JsonSerializerSettings(baseDefaultSettings);
                defaultSettings.Converters.Add(new HmqActorIdentityJsonDeserializer());
                return defaultSettings;
            };

            dependencyRegistry

                .Register<Concrete.DependencyGroup>(() => new Concrete.DependencyGroup())

                ;
        }
    }
}
