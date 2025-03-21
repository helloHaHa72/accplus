using BaseAppSettings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using POSV1.MasterDBModel;
using POSV1.MasterDBModel.AuthModels;
using POSV1.TenantAPI.Models;
using POSV1.TenantAPI.Models.AuthModels;
using POSV1.TenantAPI.Services.Interface;
using POSV1.TenantModel;
using POSV1.TenantModel.Models;
using POSV1.TenantModel.Models.EntityModels.Accounting;
using POSV1.TenantModel.Models.EntityModels.Inventory;
using POSV1.TenantModel.Models.EntityModels.Settings;
using POSV1.TenantModel.Modules;
using POSV1.TenantModel.Repo.Interface.Settings;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace POSV1.TenantAPI.Services.Implementation;
public class AuthService : IAuthService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly MainDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly IUserPermissionListRepo _userPermissionListRepo;

    public AuthService(
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager,
        MainDbContext context,
        IConfiguration configuration,
        IUserPermissionListRepo userPermissionListRepo
        )
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
        _userPermissionListRepo = userPermissionListRepo;
    }

    public async Task<IdentityResult> RegisterUser(RegisterModel model, int TenantId)
    {
        var userExists = await _userManager.FindByNameAsync(model.Username);
        if (userExists != null)
            return IdentityResult.Failed(new IdentityError { Description = "User already exists!" });

        var user = new IdentityUser
        {
            Email = model.Email,
            PhoneNumber = model.PhoneNumber,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = model.Username,
        };

        //user.AppUserTenantBranches.Add(new AppUserTenantBranches()
        //{
        //    TenantId = TenantId,
        //    CreatedName = "Admin",
        //    DateCreated = DateTime.UtcNow,
        //});

        return await _userManager.CreateAsync(user, model.Password);
    }

    //public async Task<IdentityResult> ResetPassword(ResetPasswordModel model)
    //{
    //    var user = await _userManager.FindByNameAsync(model.Username);
    //    if (user == null)
    //    {
    //        return IdentityResult.Failed(new IdentityError { Description = "Invalid user." });
    //    }

    //    var OTPDetail = _MasterDbContext.aspNetUserOTP.FirstOrDefault(x => x.UserName == model.Username || x.Email == model.Username);
    //    if (OTPDetail == null || model.OTP != OTPDetail.OTP)
    //    {
    //        return IdentityResult.Failed(new IdentityError { Description = "Invalid OTP." });
    //    }

    //    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
    //    return await _userManager.ResetPasswordAsync(user, token, model.NewPassword);
    //}


    public bool IsTokenValid(string token)
    {
        if (string.IsNullOrEmpty(token))
        {
            return false;
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]);

        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _configuration["JWT:ValidIssuer"],
                ValidAudience = _configuration["JWT:ValidAudience"],
                IssuerSigningKey = new SymmetricSecurityKey(key)
            }, out SecurityToken validatedToken);

            return true;
        }
        catch
        {
            return false;
        }
    }


    public async Task<LoginResult> Login(LoginModel model)
    {
        string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
        Regex regex = new Regex(pattern);

        //IdentityUser user = null;
        IdentityUser user = null;
        if (regex.IsMatch(model.UserName))
        {
            user = await _context.Users
                        //.Include(u => u.AppUserTenantBranches) // Include the Branch navigation
                        .FirstOrDefaultAsync(u => u.Email == model.UserName);
        }
        else
        {
            user = await _context.Users
                        //.Include(u => u.AppUserTenantBranches)
                        .FirstOrDefaultAsync(u => u.UserName == model.UserName);
        }
        //if (regex.IsMatch(model.UserName))
        //{
        //    user = await _userManager.FindByEmailAsync(model.UserName);
        //}
        //else
        //{
        //    user = await _userManager.FindByNameAsync(model.UserName);
        //}

        if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
        {
            //var userRoles = await _userManager.GetRolesAsync(user);
            var roles = await _context.UserRoles
                .Where(ur => ur.UserId == user.Id)
                .ToListAsync();

            var userRoles = await _context.Roles
                .Where(r => roles.Select(ur => ur.RoleId).Contains(r.Id))
                .Select(r => r.Name)
                .ToListAsync();

            string TenantId = "";
            string BranchCode = "";
            LoginResult Res = new LoginResult
            {
                Username = user.UserName,
                Email = user.Email,
                AssociatedRoles = userRoles,
            };

            //if (!Res.IsSuperAdmin)
            //{
            //    if (user.AppUserTenantBranches is null)
            //    {
            //        _context
            //            .Entry(user)
            //            .Collection(x => x.AppUserTenantBranches)
            //            .Load();
            //    }
            //    if (user.AppUserTenantBranches is null || !user.AppUserTenantBranches.Any())
            //    {
            //        throw new Exception("Invalid User Data Configuration without assigned branch.");
            //    }
            //    var b1 = user.AppUserTenantBranches.First();
            //    //explicity
            //    _context.Entry(b1).Reference(x => x.TenantBranch).Load();

            //    TenantId = b1.TenantId.ToString();
            //    // Get the assigned branch code
            //    //var userBranch = await _context.TenantBranches
            //    //    .FirstOrDefaultAsync(ub => ub.TenantId.ToString() == TenantId);
            //    //if (userBranch != null)
            //    //{
            //    //    BranchCode = userBranch.BranchCode;
            //    //}

            //    //var orgInfo = await _context.TenantInfos.FirstOrDefaultAsync(x => x.Id.ToString() == TenantId);
            //    //if (orgInfo != null)
            //    //{
            //    //    Res.OrgName = orgInfo.OrgName;
            //    //}


            //}
            var authClaims = new List<Claim>
                {
                    //new Claim(ClaimTypes.Name, user.UserName),
                    new Claim("X-Tenant-Id", TenantId),
                    new Claim("BranchCode", BranchCode),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

            //foreach (var userRole in userRoles)
            //{
            //    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            //}

            HashSet<string> matchedPermissions = new HashSet<string>();
            foreach (var role in userRoles)
            {
                // Fetch role details from the database using normalized role name
                var normalizedRoleName = role.ToUpper();
                var roleData = await _roleManager.Roles
                                            .Where(x => x.NormalizedName == normalizedRoleName)
                                            .FirstOrDefaultAsync();

                if (roleData != null)
                {
                    // Fetch permissions for the current role
                    var permissions = await _context.UserPermissionLists
                        .AsNoTracking() // Improves performance if no update is needed
                        .Where(x => x.roleId == roleData.Id)
                        .Select(x => x.AccesListId)
                        .ToListAsync();

                    // Convert and add unique permissions
                    foreach (var permission in permissions)
                    {
                        if (Enum.IsDefined(typeof(EnumSubCategory), (int)permission)) // Ensure valid enum values
                        {
                            matchedPermissions.Add(((EnumSubCategory)permission).ToString());
                        }
                    }
                }
            }


            var token = GetToken(authClaims);
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            Res.TenantID = TenantId;
            Res.Token = tokenString;
            Res.Expiration = token.ValidTo;
            Res.Permissions = matchedPermissions.ToList();

            return Res;
        }

        return null;
    }

    private JwtSecurityToken GetToken(List<Claim> authClaims)
    {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

        var token = new JwtSecurityToken(
            issuer: _configuration["JWT:ValidIssuer"],
            audience: _configuration["JWT:ValidAudience"],
            expires: DateTime.Now.AddHours(3),
            //expires: DateTime.Now.AddMinutes(1),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

        return token;
    }

    public static string GenerateOTP()
    {
        Random random = new Random();
        int otp = random.Next(100000, 999999);
        return otp.ToString();
    }

    //public async Task ForgetPassword(string username)
    //{
    //    var user = await _userManager.FindByNameAsync(username);
    //    if (user == null || await _userManager.IsEmailConfirmedAsync(user))
    //    {
    //        throw new Exception("User not found");
    //    }
    //    var existingUserOTP = _context.aspNetUserOTP.FirstOrDefault(x => x.UserName == username);

    //    var OTP = GenerateOTP();
    //    if (existingUserOTP == null)
    //    {
    //        var newUserOTP = new AspNetUserOTP
    //        {
    //            UserName = username,
    //            Email = user.Email,
    //            OTP = OTP
    //        };
    //        _MasterDbContext.aspNetUserOTP.Add(newUserOTP);
    //    }
    //    else
    //    {
    //        existingUserOTP.OTP = OTP;
    //    }
    //    await _context.SaveChangesAsync();

    //    await SendMail(user, OTP);
    //}

    private static async Task SendMail(IdentityUser user, string OTP)
    {
        // Compose the email
        var subject = "OTP";
        var body = $"Here is your OTP:{OTP}";

        // Send the email
        using (var client = new SmtpClient())
        {
            var credentials = new NetworkCredential
            {
                UserName = "noreply@sebs.asia",
                Password = "oR-TCPsj6xZz"
            };

            client.Credentials = credentials;
            client.Host = "mail.sebs.asia";
            client.Port = 26;
            client.EnableSsl = false;

            var mailMessage = new MailMessage
            {
                From = new MailAddress("noreply@sebs.asia"),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(new MailAddress(user.Email));

            try
            {
                await client.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to send email. " + ex.Message);
            }
        }
    }

    public async Task<IdentityResult> RegisterAdmin(RegisterModel model)
    {
        var userExists = await _userManager.FindByNameAsync(model.Username);
        if (userExists != null)
        {
            return IdentityResult.Failed(new IdentityError { Description = "User already exists!" });
        }

        IdentityUser user = new IdentityUser
        {
            Email = model.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = model.Username
        };

        var result = await _userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
        {
            return result; // Return the error result directly if user creation fails
        }

        foreach (var role in model.UserType)
        {
            if (await _roleManager.RoleExistsAsync(role))
            {
                await _userManager.AddToRoleAsync(user, role);
            }
        }

        // Assign user to branch
        var branch = await _context.BranchDatas.FindAsync(model.BranchCode);
        if (branch != null)
        {
            var userBranch = new UserBranch
            {
                UserId = user.Id,
                BranchCode = model.BranchCode
            };

            _context.UserBranches.Add(userBranch);
            await _context.SaveChangesAsync();
        }
        else
        {
            return IdentityResult.Failed(new IdentityError { Description = "Invalid branch code!" });
        }

        return IdentityResult.Success; // Return success if everything went well
    }

    public async Task SeedDefaultData()
    {
        //await AssignUserToDefaultBranch(SuperAdminUser);

        #region For Invoking SeedData of All Modules

        // Get a list of types in the current assembly that implement IModules
        List<Type> moduleTypes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => typeof(IModules<MainDbContext>).IsAssignableFrom(t)
                && !t.IsInterface && !t.IsAbstract)
            .ToList();

        // Create instances of the modules and invoke SeedDefault
        foreach (var moduleType in moduleTypes)
        {
            IModules<MainDbContext> module = (IModules<MainDbContext>)Activator.CreateInstance(moduleType);
            module.SeedData(_context);
        }
        #endregion

        // seed configuration
        ConfigurationSetting.SeedDefault(_context);
        ta01taxsettlement.SeedTaxDefault(_context);
        led03general_ledgers.SeedGeneralLedgers(_context);
        //AccessList.InsertAccessList(_MasterDbContext);

        //await AddPermissionToSuperAdmin();
    }


    //public async Task AddPermissionToSuperAdmin()
    //{
    //    try
    //    {
    //        var superAdminData = await _roleManager.Roles
    //            .Where(r => r.Name == nameof(EnumApplicationUserType.SuperAdmin))
    //            .FirstOrDefaultAsync();

    //        if (superAdminData == null)
    //        {
    //            //_logger.LogWarning("Super Admin role not found.");
    //            return;
    //        }

    //        // Fetch existing permissions for Super Admin
    //        var existingPermissionIds = await _userPermissionListRepo.GetList()
    //            .Where(x => x.roleId == superAdminData.Id)
    //            .Select(x => (int)x.AccesListId)  // Convert SubCategory enum to int
    //            .ToListAsync();

    //        // Convert to HashSet<int> for better performance
    //        var existingPermissionSet = new HashSet<int>(existingPermissionIds);

    //        // Get all possible permissions from the SubCategory enum
    //        var allPermissionIds = Enum.GetValues(typeof(EnumSubCategory))
    //            .Cast<EnumSubCategory>()
    //            .Select(p => (int)p)  // Convert enum to int
    //            .ToList();

    //        // Find missing permissions (that don't exist in the database)
    //        var missingPermissions = allPermissionIds.Where(p => !existingPermissionSet.Contains(p)).ToList();


    //        if (missingPermissions.Any())
    //        {
    //            foreach (var permissionId in missingPermissions)
    //            {
    //                var newPermission = new UserPermissionList()
    //                {
    //                    roleId = superAdminData.Id,
    //                    AccesListId = (EnumSubCategory)permissionId,
    //                    DateCreated = DateTime.UtcNow,
    //                    CreatedName = "SYSTEM",
    //                };

    //                _userPermissionListRepo.Insert(newPermission);
    //            }

    //            await _userPermissionListRepo.SaveAsync();
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        throw new Exception("An error occurred while adding permissions.", ex);
    //    }
    //}


    private async Task AssignUserToDefaultBranch(IdentityUser user)
    {
        var defaultBranch = await _context.BranchDatas.FirstOrDefaultAsync(b => b.IsDefault);

        if (defaultBranch == null)
        {
            Console.WriteLine("No default branch found. Skipping branch assignment.");
            return;
        }

        bool isUserAssigned = await _context.UserBranches
            .AnyAsync(ub => ub.UserId == user.Id && ub.BranchCode == defaultBranch.BranchCode);

        if (!isUserAssigned)
        {
            _context.UserBranches.Add(new UserBranch
            {
                UserId = user.Id,
                BranchCode = defaultBranch.BranchCode
            });

            await _context.SaveChangesAsync();
            //await _context.SaveChangesAsync();
            //await _context.SaveChangesAsync();
        }
    }


    public async Task<IdentityResult> ChangePassword(string username, string oldPassword, string newPassword)
    {
        var user = await _userManager.FindByNameAsync(username);
        if (user == null || !await _userManager.CheckPasswordAsync(user, oldPassword))
        {
            return IdentityResult.Failed(new IdentityError { Description = "Invalid username or password." });
        }

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        return await _userManager.ResetPasswordAsync(user, token, newPassword);
    }


}
