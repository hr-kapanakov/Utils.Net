using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Markup;

[assembly: System.Windows.ThemeInfo(System.Windows.ResourceDictionaryLocation.None, System.Windows.ResourceDictionaryLocation.SourceAssembly)]

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Utils.Net")]
[assembly: AssemblyDescription("Utility library for .Net framework and WPF")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Hristo Kapanakov")]
[assembly: AssemblyProduct("Utils.Net")]
[assembly: AssemblyCopyright("Copyright ©  2017")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible
// to COM components.  If you need to access a type in this assembly from
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("a014b4c6-b225-4cba-995f-9f84df930eb2")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("0.5.*")]


[assembly: XmlnsPrefix("http://schemas.utils.net/managers", "mgr")]
[assembly: XmlnsDefinition("http://schemas.utils.net/managers", "Utils.Net.Managers")]

[assembly: XmlnsPrefix("http://schemas.utils.net/interactivity", "i")]
[assembly: XmlnsDefinition("http://schemas.utils.net/interactivity", "Utils.Net.Interactivity")]
[assembly: XmlnsDefinition("http://schemas.utils.net/interactivity", "Utils.Net.Interactivity.Behaviors")]
[assembly: XmlnsDefinition("http://schemas.utils.net/interactivity", "Utils.Net.Interactivity.Triggers")]
[assembly: XmlnsDefinition("http://schemas.utils.net/interactivity", "Utils.Net.Interactivity.TriggerActions")]

[assembly: XmlnsPrefix("http://schemas.utils.net/controls", "ctrls")]
[assembly: XmlnsDefinition("http://schemas.utils.net/controls", "Utils.Net.Controls")]

[assembly: XmlnsPrefix("http://schemas.utils.net/converters", "convs")]
[assembly: XmlnsDefinition("http://schemas.utils.net/converters", "Utils.Net.Converters")]

[assembly: XmlnsPrefix("http://schemas.utils.net/common", "common")]
[assembly: XmlnsDefinition("http://schemas.utils.net/common", "Utils.Net.Common")]
[assembly: XmlnsDefinition("http://schemas.utils.net/common", "Utils.Net.Helpers")]
