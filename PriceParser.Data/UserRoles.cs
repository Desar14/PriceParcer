using System.Reflection;

namespace PriceParser.Data
{
    public class UserRoles
    {
        public const string Admin = "Admin";
        public const string Moderator = "Moderator";
        public const string User = "User";

        public static List<string> RolesList()
        {
            var result = new List<string>();

            var obj = new UserRoles();

            var fieldsList = obj.GetType().GetFields();

            foreach (FieldInfo propertyInfo in fieldsList)
            {
                string? roleName = (string)propertyInfo.GetValue(obj);
                if (!String.IsNullOrEmpty(roleName))
                {
                    result.Add(roleName);
                }
            }

            return result;
        }
    }
}
