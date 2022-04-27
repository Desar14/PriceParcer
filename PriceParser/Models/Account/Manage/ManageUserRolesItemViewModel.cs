using Microsoft.AspNetCore.Mvc.Rendering;

namespace PriceParser.Models.Account.Manage
{
    public class ManageUserRolesItemViewModel
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public List<SelectListItem> Roles { get; set; }

        public List<string> RoleNames { get; set; }
    }

    public class ManageUserRolesViewModel
    {
        public List<ManageUserRolesItemViewModel> Users { get; set; }

        public string StatusMessage {get; set; }
    }
}
