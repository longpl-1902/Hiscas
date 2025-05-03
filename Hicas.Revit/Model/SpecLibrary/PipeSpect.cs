using System.Collections.Generic;

namespace Hicas.Revit.Setting
{
    public class PipeSpec
    {
        public ICollection<SpecInfo> PN6 { get; set; }
        public ICollection<SpecInfo> PN8 { get; set; }

        public ICollection<SpecInfo> SummarySpec()
        {
            var summary = new List<SpecInfo>();

            var properties = this.GetType().GetProperties();

            foreach (var prop in properties)
            {
                var value = prop.GetValue(this) as ICollection<SpecInfo>;
                if (value != null)
                {
                    summary.AddRange(value);
                }
            }

            return summary;
        }
    }
}
