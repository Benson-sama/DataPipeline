# DataPipeline
This application uses reflection to create a data pipeline using .NET Framework 4.7.2.

## General Information
The provided data units share a generic ValueOutputEventArgs class, that is referenced using the name DataUnits in all of the data units projects. Therefore it is required to place the DataUnits assembly into the data units folders. These folders are named DSU for DataSourceUnits, DPU for DataProcessingUnits and DVU for DataVisualisationUnits and are located in the current working directory of the main application. (DataPipeline.View) If the data units do not appear in the application, either uncomment the section in the App.config file of the DataPipeline.View project or unblock each data unit manually. (For further information about unblocking refer to to the source below.) At the moment, only unique class types are loaded into the DataPipeline. If desired this can be changed by commenting the corresponding sections of the ConfigurationApplication class out, at your own risk as it was not tested before. They can be found in the methods LoadDSUs(), LoadDPUs() and LoadDVUs(), lines 531, 561, 591.

## Data Unit Guidelines
To implement your own data units, reference the DataPipeline.Model assembly and add the namespace DataPipeline.Model.Attributes to the usings. When designing your data units, apply the DataOutputAttribute to events that output values from the data unit and the DataInputAttribute to methods that input values into the data unit. (Only the first appearances will be used!) The output signature of the event must match the input signature of the method, otherwise connection attempts will fail. All DataUnits must provide a parameterless constructor, a Start() method and a Stop() method. If your data unit is stateless, leave these methods empty. Additionally apply the DataUnitInformationAttribute to the class that represents the data unit. It is advised to use all properties of the DataUnitInformationAttribute in order to provide enough information to the user interface later on. For data visualisation units in particular for ones that bind via an ItemsControl, the bound collection needs to be enabled for synchronisation. (See source below for further information.)

## Sources
#### BindingOperations.EnableCollectionSynchronization Method
https://docs.microsoft.com/en-us/dotnet/api/system.windows.data.bindingoperations.enablecollectionsynchronization?view=netframework-4.7.2

#### How to: Use an assembly from the Web in Visual Studio.
https://docs.microsoft.com/en-us/previous-versions/visualstudio/visual-studio-2010/ee890038(v=vs.100)
