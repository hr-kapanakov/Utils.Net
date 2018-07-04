using System;
using Utils.Net.Attributes;

namespace Utils.Net.Sample.Models
{
    public enum SettingsEnum
    {
        /// <summary>
        /// Test enum 1
        /// </summary>
        Enum1,
        /// <summary>
        /// Test enum 2
        /// </summary>
        Enum2,
        /// <summary>
        /// Test enum 3
        /// </summary>
        Enum3
    }

    public class Settings
    {
        [Setting("Category", "Group1", command: "Browse")]
        public string Name { get; set; } = "name";

        [Setting("Category", "Group1", sortName: "1")]
        public string Path { get; } = "path";

        [Setting("Category", "Group2", "Enumerable")]
        public SettingsEnum Enum { get; set;  } = SettingsEnum.Enum1;

        [Setting("Category", "Group2")]
        [Numeric("0.00", -1, 3, 0.1)]
        public double Number { get; set; } = 0;


        [Setting("Category1", "Group2", "Number")]
        [Numeric(minimum: 0, maximum: 5)]
        public int Count { get; set; } = 0;

        [Setting("Category1", "Group2", "Value")]
        [Numeric(step: 0.01)]
        public double Value { get; set; } = 0.1;

        [Setting("Category1", "Group2", sortName: "z")]
        public DateTime Date { get; } = DateTime.Now;


        [Setting("Category2")]
        public string NoGroup { get; } = "NoGroup";

        [Setting]
        public string NoCategory { get; } = "NoCategory";

        public string NoAttribute { get; } = "NoAttribute";
    }
}
