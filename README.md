# Convention Configuration using JSON

The easy way to embed and inject JSON configuration in your .NET assemblies. Drop in a JSON config file, create a corresponding class, call 1 line of code and the configuration is auto-wired and injected where needed.

## Installation

Grab the [NuGet package here](https://www.nuget.org/packages/Microsoft.ConventionConfiguration/1.0.0)

## Usage

1. Create a folder called *Configuration* in the root of a project.
2. Add a JSON configuration file to that folder.
3. In the VS properties of the JSON file, set the "Copy to Output Directory" to "Copy if newer".
4. Create a class anywhere in your project with a name matching the JSON file and a suffix of *Configuration*. (Example: for Cloud.json, create a class called CloudConfiguration)
5. When your application starts up, call `ConfigurationLoader.LoadConfigurations`.

For a working example, look at the *ConventionConfiguration.Tests* project.

### Example

**Cloud.json:**

	{
		"Port": 1234,
		"MsmqQueueName": "DataCollectorQueue"
	}

**CloudConfiguration.cs:**

    public class DummyConfiguration
    {
        public long Port { get; set; }
        public string MsmqQueueName { get; set; }
    }

**Wire-up Code:**

	ConfigurationLoader.LoadConfigurations(container, ".\\Configuration\\", "{0}Configuration");

## Notes

Currently, there is a dependency on [StructureMap](http://docs.structuremap.net/). I'm currently looking for ways to remove this dependency.

# License

Microsoft Developer & Platform Evangelism

Copyright (c) Microsoft Corporation. All rights reserved.

THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.

The example companies, organizations, products, domain names, e-mail addresses, logos, people, places, and events depicted herein are fictitious. No association with any real company, organization, product, domain name, email address, logo, person, places, or events is intended or should be inferred.

