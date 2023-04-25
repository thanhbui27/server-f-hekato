using Newtonsoft.Json.Linq;
using System.Runtime.Serialization;

namespace DoAn.Helpers.TypeCheck
{
    public class EnumRole
    {
        private EnumRole(string value) { Value = value; }
        public EnumRole() {  }
        public string Value { get; private set; }

        public static EnumRole Admin { get { return new EnumRole("Admin"); } }
        public static EnumRole User { get { return new EnumRole("User"); } }
        public static EnumRole Other { get { return new EnumRole("Other"); } }
        public static EnumRole AdminAndUser { get { return new EnumRole("Admin, User"); } }

        public override string ToString()
        {
            return Value;
        }
    }
}
