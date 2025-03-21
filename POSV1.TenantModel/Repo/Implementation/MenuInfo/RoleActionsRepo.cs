using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using POSV1.TenantModel.Repo.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POSV1.TenantModel.Repo.Implementation.MenuInfo
{

}



// Check if the user has any of the required actions based on their roles

// Fetch the actions associated with the role
//int[] Actions = GetList().Where(x => axs.Contains(x.Roles.Name)).Select(x => x.ActionId).ToArray();
//return Actions.Contains(code);

//var hasAccess = roles.Any(role => GetList().Any(ra => ra.RoleId == role && Actions.Contains(ra.ActionId)));
//return hasAccess;