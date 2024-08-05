using H.Necessaire;
using H.Necessaire.Runtime;
using System;
using System.IO;
using System.Reflection;

namespace H.Qubiz.Xperiments.CLI
{
    internal static class AppConfig
    {
        const string srcFolderRelativePath = "/Src/H.Qubiz.Xperiments.CLI/";

        public static ImAnApiWireup WithDefaultRuntimeConfig(this ImAnApiWireup wireup)
        {
            return
                wireup
                .With(x => x.Register<RuntimeConfig>(() => new RuntimeConfig
                {
                    Values = [
                        "NuSpecRootFolderPath".ConfigWith(GetCodebaseFolderPath()),
                        "HMQ".ConfigWith(
                            "Azure".ConfigWith(
                                "ServiceBus".ConfigWith(
                                    "ConnectionString".ConfigWith(ReadConfigFromFile("AzureServiceBusConnectionString.cfg.txt")),
                                    "TopicName".ConfigWith(ReadConfigFromFile("AzureServiceBusTopicName.cfg.txt")),
                                    "SubscriptionName".ConfigWith(ReadConfigFromFile("AzureServiceBusSubscriptionName.cfg.txt"))
                                )
                            ),
                            "RabbitMQ".ConfigWith(
                                "HostName".ConfigWith(ReadConfigFromFile("RabbitMqHost.cfg.txt")),
                                "VirtualHost".ConfigWith(ReadConfigFromFile("RabbitMqVirtualHost.cfg.txt")),
                                "UserName".ConfigWith(ReadConfigFromFile("RabbitMqUser.cfg.txt")),
                                "Password".ConfigWith(ReadConfigFromFile("RabbitMqPass.cfg.txt"))
                            )
                        ),
                        "RavenDbConnections".ConfigWith(
                            "ClientCertificateName".ConfigWith(ReadConfigFromFile("RavenDbCertName.cfg.txt")),
                            "ClientCertificatePassword".ConfigWith(ReadConfigFromFile("RavenDbCertPass.cfg.txt")),
                            "DatabaseUrls".ConfigWith(
                                "0".ConfigWith(ReadConfigFromFile("RavenDbUrl.cfg.txt"))
                            ),
                            "DatabaseNames".ConfigWith(
                                "Core".ConfigWith("H.Xperiments.Core.DevTesting"),
                                "Default".ConfigWith("H.Xperiments.Default.DevTesting")
                            )
                        ),
                        "SqlConnections".ConfigWith(
                            "DefaultConnectionString".ConfigWith(ReadConfigFromFile("SqlServerDefaultConnectionString.cfg.txt")),
                            "DatabaseNames".ConfigWith(
                                "Core".ConfigWith("H.Xperiments.DevTest.Core"),
                                "Default".ConfigWith("H.Xperiments.DevTest"),
                                "HMQRegistry".ConfigWith("H.MQ.Registry")
                            )
                        )
                    ],
                }));
            ;
        }

        private static string ReadConfigFromFile(string filePath)
        {
            FileInfo fileInfo = new FileInfo(filePath);

            if (!fileInfo.Exists)
                return null;

            string result = null;

            new Action(() =>
            {
                result = File.ReadAllText(fileInfo.FullName);
            })
            .TryOrFailWithGrace(onFail: ex => result = null);

            return result;
        }

        private static string GetCodebaseFolderPath()
        {
            string codeBase = Assembly.GetExecutingAssembly()?.Location ?? string.Empty;
            UriBuilder uri = new UriBuilder(codeBase);
            string dllPath = Uri.UnescapeDataString(uri.Path);
            int srcFolderIndex = dllPath.ToLowerInvariant().IndexOf(srcFolderRelativePath, StringComparison.InvariantCultureIgnoreCase);
            if (srcFolderIndex < 0)
                return string.Empty;
            string srcFolderPath = Path.GetDirectoryName(dllPath.Substring(0, srcFolderIndex + srcFolderRelativePath.Length)) ?? string.Empty;
            return srcFolderPath;
        }
    }
}
