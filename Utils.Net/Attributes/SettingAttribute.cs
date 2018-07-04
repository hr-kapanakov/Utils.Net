using System;

namespace Utils.Net.Attributes
{
    /// <summary>
    /// Specifies that property is a setting.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class SettingAttribute : Attribute
    {
        /// <summary>
        /// Gets the setting's category.
        /// </summary>
        public string Category { get; }

        /// <summary>
        /// Gets the setting's group.
        /// </summary>
        public string Group { get; }

        /// <summary>
        /// Gets the setting's display name.
        /// </summary>
        public string DisplayName { get; }

        /// <summary>
        /// Gets the settings's sort name.
        /// </summary>
        public string SortName { get; }

        /// <summary>
        /// Gets the setting's command.
        /// </summary>
        public string Command { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingAttribute"/> class.
        /// </summary>
        /// <param name="category">The setting's category.</param>
        /// <param name="group">The setting's group.</param>
        /// <param name="displayName">The setting's display name.</param>
        /// <param name="sortName">The setting's sort name.</param>
        /// <param name="command">The setting's command.</param>
        public SettingAttribute(
            string category = "", 
            string group = "", 
            string displayName = null, 
            string sortName = null, 
            string command = null)
        {
            Category = category;
            Group = group;
            DisplayName = displayName;
            SortName = sortName ?? displayName;
            Command = command;
        }
    }
}
