using ConventionConfiguration.Configuration;
using Microsoft.ConventionConfiguration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConventionConfiguration
{
    [TestClass]
    public class ConfigurationLoaderTests
    {
        [TestMethod]
        public void LoadLocalDummyConfiguration_VerifyRegistration()
        {
            var container = new StructureMap.Container();
            ConfigurationLoader.LoadConfigurations(container, ".\\Configuration\\", "{0}Configuration");

            var config = container.GetInstance<DummyConfiguration>();
            Assert.IsNotNull(config);
            Assert.AreEqual(1234, config.Port);
            Assert.AreEqual("DataCollectorQueue", config.MsmqQueueName);
        }

        [TestMethod]
        public void LoadLocalDummyConfigurationWithFrameworkConfiguration_VerifySingleton()
        {
            var container = new StructureMap.Container();
            ConfigurationLoader.LoadConfigurations(container, ".\\Configuration\\", "{0}Configuration");

            var config = container.GetInstance<DummyConfiguration>();
            var config2 = container.GetInstance<DummyConfiguration>();

            Assert.AreEqual(config, config2);
        }
    }
}
