using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace POSV1.TenantModel.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccessLists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MainHeading = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessLists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Add01Additionalcharges",
                columns: table => new
                {
                    add01uin = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    add01title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    add01description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DeletedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DateCreated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DateDeleted = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Add01Additionalcharges", x => x.add01uin);
                });

            migrationBuilder.CreateTable(
                name: "Add02Purchaseadditionalcharges",
                columns: table => new
                {
                    add02uin = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DeletedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DateCreated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DateDeleted = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Add02Purchaseadditionalcharges", x => x.add02uin);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BranchDatas",
                columns: table => new
                {
                    BranchCode = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    BranchName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    CreatedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DeletedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DateCreated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DateDeleted = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BranchDatas", x => x.BranchCode);
                });

            migrationBuilder.CreateTable(
                name: "cfg01configurations",
                columns: table => new
                {
                    cfg01uin = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    cfg01module = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    cfg01key = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    cfg01value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    cfg01created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    cfg01created_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    cfg01updated_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    cfg01updated_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cfg01configurations", x => x.cfg01uin);
                });

            migrationBuilder.CreateTable(
                name: "ConfigurationSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(MAX)", maxLength: 2147483647, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CanBeEdited = table.Column<bool>(type: "bit", nullable: true),
                    CreatedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DeletedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DateCreated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DateDeleted = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfigurationSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "cus02customerType",
                columns: table => new
                {
                    cus02Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    cus02Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    cus02DiscountPercenatge = table.Column<double>(type: "float", nullable: true),
                    CreatedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DeletedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DateCreated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DateDeleted = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cus02customerType", x => x.cus02Id);
                });

            migrationBuilder.CreateTable(
                name: "est01estimate",
                columns: table => new
                {
                    est01uin = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    est01customername = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    est01refnumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    est01status = table.Column<int>(type: "int", nullable: false),
                    CreatedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DeletedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DateCreated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DateDeleted = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_est01estimate", x => x.est01uin);
                });

            migrationBuilder.CreateTable(
                name: "led05ledger_types",
                columns: table => new
                {
                    led05uin = table.Column<int>(type: "int", nullable: false),
                    led05title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    led05title_nep = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    led05add_dr = table.Column<bool>(type: "bit", nullable: false),
                    CreatedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DeletedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DateCreated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DateDeleted = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_led05ledger_types", x => x.led05uin);
                });

            migrationBuilder.CreateTable(
                name: "MainSetups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrgName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Logo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Server = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DbName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DbPassword = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DeletedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DateCreated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DateDeleted = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MainSetups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "pro01categories",
                columns: table => new
                {
                    pro01uin = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    pro01code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    pro01name_eng = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    pro01name_nep = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    pro01status = table.Column<bool>(type: "bit", nullable: false),
                    CreatedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DeletedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DateCreated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DateDeleted = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pro01categories", x => x.pro01uin);
                });

            migrationBuilder.CreateTable(
                name: "Prod01Productions",
                columns: table => new
                {
                    prod01uin = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    prod01title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    prod01code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    prod01description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    prod01status = table.Column<int>(type: "int", nullable: false),
                    prod01startdate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    prod01enddate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DeletedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DateCreated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DateDeleted = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prod01Productions", x => x.prod01uin);
                });

            migrationBuilder.CreateTable(
                name: "settings",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    modules = table.Column<int>(type: "int", nullable: false),
                    values = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    lastUpdated_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    lastUpdated_by = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_settings", x => new { x.id, x.modules });
                });

            migrationBuilder.CreateTable(
                name: "sup01suppliers",
                columns: table => new
                {
                    sup01uin = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    sup01address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    sup01tel = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    sup01regDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    sup01regNo = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    sup01balance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DeletedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DateCreated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DateDeleted = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sup01suppliers", x => x.sup01uin);
                });

            migrationBuilder.CreateTable(
                name: "ta01taxsettlement",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    taxPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsForTDs = table.Column<bool>(type: "bit", nullable: false),
                    CanBeDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DeletedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DateCreated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DateDeleted = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ta01taxsettlement", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tran01transaction_types",
                columns: table => new
                {
                    tran01uin = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tran02name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DeletedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DateCreated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DateDeleted = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tran01transaction_types", x => x.tran01uin);
                });

            migrationBuilder.CreateTable(
                name: "un01units",
                columns: table => new
                {
                    un01uin = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    un01name_eng = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    un01name_nep = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    un01desc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    un01ratio = table.Column<double>(type: "float", nullable: false),
                    un01status = table.Column<bool>(type: "bit", nullable: false),
                    CreatedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DeletedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DateCreated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DateDeleted = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_un01units", x => x.un01uin);
                });

            migrationBuilder.CreateTable(
                name: "UserPermissionLists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    roleId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccesListId = table.Column<int>(type: "int", nullable: false),
                    CreatedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DeletedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DateCreated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DateDeleted = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPermissionLists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "vou01voucher_types",
                columns: table => new
                {
                    vou01uin = table.Column<int>(type: "int", nullable: false),
                    vou01title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    vou01last_no = table.Column<int>(type: "int", nullable: false),
                    vou01prefix = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DeletedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DateCreated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DateDeleted = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vou01voucher_types", x => x.vou01uin);
                });

            migrationBuilder.CreateTable(
                name: "vou05voucher_log",
                columns: table => new
                {
                    vou05uin = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    vou05vou02uin = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    voucherStatus = table.Column<int>(type: "int", nullable: false),
                    CreatedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DeletedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DateCreated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DateDeleted = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vou05voucher_log", x => x.vou05uin);
                });

            migrationBuilder.CreateTable(
                name: "Wag01Wagerates",
                columns: table => new
                {
                    wag01uin = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    wag01title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    wag01identificationcode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    wag01value = table.Column<double>(type: "float", nullable: false),
                    wag01wagetype = table.Column<int>(type: "int", nullable: false),
                    wag01canbedeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DeletedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DateCreated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DateDeleted = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wag01Wagerates", x => x.wag01uin);
                });

            migrationBuilder.CreateTable(
                name: "Add03Purchaseadditionalchargesdetails",
                columns: table => new
                {
                    add03uin = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    add03title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    add03amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    add03remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    add02uin = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Add03Purchaseadditionalchargesdetails", x => x.add03uin);
                    table.ForeignKey(
                        name: "FK_Add03Purchaseadditionalchargesdetails_Add02Purchaseadditionalcharges_add02uin",
                        column: x => x.add02uin,
                        principalTable: "Add02Purchaseadditionalcharges",
                        principalColumn: "add02uin",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserBranches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BranchCode = table.Column<string>(type: "nvarchar(25)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBranches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserBranches_BranchDatas_BranchCode",
                        column: x => x.BranchCode,
                        principalTable: "BranchDatas",
                        principalColumn: "BranchCode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ven01vendors",
                columns: table => new
                {
                    ven01uin = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ven01led_code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ven01name_eng = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ven01name_nep = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ven01address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ven01reg_no = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ven01tel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ven01opening_bal = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ven01status = table.Column<bool>(type: "bit", nullable: false),
                    ven01is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    ven01isvat = table.Column<bool>(type: "bit", nullable: false),
                    BranchCode = table.Column<string>(type: "nvarchar(25)", nullable: false),
                    ven01registered_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DeletedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DateCreated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DateDeleted = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ven01vendors", x => x.ven01uin);
                    table.ForeignKey(
                        name: "FK_ven01vendors_BranchDatas_BranchCode",
                        column: x => x.BranchCode,
                        principalTable: "BranchDatas",
                        principalColumn: "BranchCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "cus01customers",
                columns: table => new
                {
                    cus01uin = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    cus01led_code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    cus01name_eng = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    cus01name_nep = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    cus01address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    cus01tel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    cus01opening_bal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    cus01reg_no = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    cus01status = table.Column<bool>(type: "bit", nullable: false),
                    cus01deleted = table.Column<bool>(type: "bit", nullable: false),
                    cus01customerTypeId = table.Column<int>(type: "int", nullable: true),
                    cus01registered_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    cus01isvat = table.Column<bool>(type: "bit", nullable: false),
                    BranchCode = table.Column<string>(type: "nvarchar(25)", nullable: false),
                    CreatedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DeletedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DateCreated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DateDeleted = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cus01customers", x => x.cus01uin);
                    table.ForeignKey(
                        name: "FK_cus01customers_BranchDatas_BranchCode",
                        column: x => x.BranchCode,
                        principalTable: "BranchDatas",
                        principalColumn: "BranchCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_cus01customers_cus02customerType_cus01customerTypeId",
                        column: x => x.cus01customerTypeId,
                        principalTable: "cus02customerType",
                        principalColumn: "cus02Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "led03general_ledgers",
                columns: table => new
                {
                    led03uin = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    led03led05uin = table.Column<int>(type: "int", nullable: false),
                    led03title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    led03code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    led03desc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    led03balance = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    led03status = table.Column<bool>(type: "bit", nullable: false),
                    led03deleted = table.Column<bool>(type: "bit", nullable: false),
                    led03led03uin = table.Column<int>(type: "int", nullable: true),
                    CreatedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DeletedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DateCreated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DateDeleted = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_led03general_ledgers", x => x.led03uin);
                    table.ForeignKey(
                        name: "FK_led03general_ledgers_led03general_ledgers_led03led03uin",
                        column: x => x.led03led03uin,
                        principalTable: "led03general_ledgers",
                        principalColumn: "led03uin",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_led03general_ledgers_led05ledger_types_led03led05uin",
                        column: x => x.led03led05uin,
                        principalTable: "led05ledger_types",
                        principalColumn: "led05uin",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "prod03statuslog",
                columns: table => new
                {
                    prod03uin = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    prod03previousstatus = table.Column<int>(type: "int", nullable: false),
                    prod03newstatus = table.Column<int>(type: "int", nullable: false),
                    prod03remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    prod03productionuin = table.Column<int>(type: "int", nullable: false),
                    CreatedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DeletedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DateCreated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DateDeleted = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_prod03statuslog", x => x.prod03uin);
                    table.ForeignKey(
                        name: "FK_prod03statuslog_Prod01Productions_prod03productionuin",
                        column: x => x.prod03productionuin,
                        principalTable: "Prod01Productions",
                        principalColumn: "prod01uin",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "pro02products",
                columns: table => new
                {
                    pro02uin = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    pro02code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    pro02hscode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    pro02name_eng = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    pro02name_nep = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    pro02pro01uin = table.Column<int>(type: "int", nullable: false),
                    pro02description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    pro02un01uin = table.Column<int>(type: "int", nullable: true),
                    pro02last_cp = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    pro02last_sp = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    pro02opening_qty = table.Column<double>(type: "float", nullable: false),
                    pro02status = table.Column<bool>(type: "bit", nullable: false),
                    pro02enable_stock = table.Column<bool>(type: "bit", nullable: false),
                    pro02is_taxable = table.Column<bool>(type: "bit", nullable: false),
                    pro02hasmultipleunits = table.Column<bool>(type: "bit", nullable: false),
                    pro02image_url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DeletedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DateCreated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DateDeleted = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pro02products", x => x.pro02uin);
                    table.ForeignKey(
                        name: "FK_pro02products_pro01categories_pro02pro01uin",
                        column: x => x.pro02pro01uin,
                        principalTable: "pro01categories",
                        principalColumn: "pro01uin",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_pro02products_un01units_pro02un01uin",
                        column: x => x.pro02un01uin,
                        principalTable: "un01units",
                        principalColumn: "un01uin",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "pur01purchases",
                columns: table => new
                {
                    pur01uin = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    pur01date_nep = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    pur01date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    pur01invoice_no = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    pur01ven01uin = table.Column<int>(type: "int", nullable: false),
                    pur01remarks = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    pur01sub_total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    pur01vat_per = table.Column<double>(type: "float", nullable: false),
                    pur01vat_amt = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    pur01total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    pur01additionalcharge = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    pur01status = table.Column<bool>(type: "bit", nullable: false),
                    pur01is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    pur01disc_amt = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    pur01disc_percentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    pur01additional_disc = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    pur01net_amt = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    pur01vatapplicable = table.Column<bool>(type: "bit", nullable: false),
                    pur01vatclamable = table.Column<bool>(type: "bit", nullable: false),
                    pur01tdspercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    pur01payment_status = table.Column<int>(type: "int", nullable: false),
                    pur01voucher_no = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BranchCode = table.Column<string>(type: "nvarchar(25)", nullable: false),
                    CreatedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DeletedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DateCreated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DateDeleted = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pur01purchases", x => x.pur01uin);
                    table.ForeignKey(
                        name: "FK_pur01purchases_BranchDatas_BranchCode",
                        column: x => x.BranchCode,
                        principalTable: "BranchDatas",
                        principalColumn: "BranchCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_pur01purchases_ven01vendors_pur01ven01uin",
                        column: x => x.pur01ven01uin,
                        principalTable: "ven01vendors",
                        principalColumn: "ven01uin",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "est02estimatesales",
                columns: table => new
                {
                    est02uin = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    est02date_nep = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    est02date_eng = table.Column<DateTime>(type: "datetime2", nullable: false),
                    est02est01uin = table.Column<int>(type: "int", nullable: false),
                    est02cus01uin = table.Column<int>(type: "int", nullable: true),
                    est02invoice_no = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    est02remarks = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    est02sub_total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    est02disc_amt = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    est02disc_percentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    est02total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    est02status = table.Column<bool>(type: "bit", nullable: false),
                    est02deleted = table.Column<bool>(type: "bit", nullable: false),
                    est02voucher_no = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    cus01customerscus01uin = table.Column<int>(type: "int", nullable: true),
                    CreatedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DeletedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DateCreated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DateDeleted = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_est02estimatesales", x => x.est02uin);
                    table.ForeignKey(
                        name: "FK_est02estimatesales_cus01customers_cus01customerscus01uin",
                        column: x => x.cus01customerscus01uin,
                        principalTable: "cus01customers",
                        principalColumn: "cus01uin");
                    table.ForeignKey(
                        name: "FK_est02estimatesales_est01estimate_est02est01uin",
                        column: x => x.est02est01uin,
                        principalTable: "est01estimate",
                        principalColumn: "est01uin",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "sal01sales",
                columns: table => new
                {
                    sal01uin = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    sal01date_nep = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    sal01date_eng = table.Column<DateTime>(type: "datetime2", nullable: false),
                    sal01cus01uin = table.Column<int>(type: "int", nullable: false),
                    sal01invoice_no = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    sal01remarks = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    sal01sub_total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    sal01disc_amt = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    sal01disc_percentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    sal01total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    sal01vat_per = table.Column<double>(type: "float", nullable: false),
                    sal01vat_amt = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    sal01net_amt = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    sal01status = table.Column<bool>(type: "bit", nullable: false),
                    sal01deleted = table.Column<bool>(type: "bit", nullable: false),
                    sal01vatapplicable = table.Column<bool>(type: "bit", nullable: false),
                    sal01vatclamable = table.Column<bool>(type: "bit", nullable: false),
                    sal01voucher_no = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BranchCode = table.Column<string>(type: "nvarchar(25)", nullable: false),
                    sal01payment_status = table.Column<int>(type: "int", nullable: false),
                    CreatedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DeletedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DateCreated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DateDeleted = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sal01sales", x => x.sal01uin);
                    table.ForeignKey(
                        name: "FK_sal01sales_BranchDatas_BranchCode",
                        column: x => x.BranchCode,
                        principalTable: "BranchDatas",
                        principalColumn: "BranchCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_sal01sales_cus01customers_sal01cus01uin",
                        column: x => x.sal01cus01uin,
                        principalTable: "cus01customers",
                        principalColumn: "cus01uin",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tran02transaction_summaries",
                columns: table => new
                {
                    tran02uin = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tran02tran01uin = table.Column<int>(type: "int", nullable: false),
                    tran02sup01uin = table.Column<int>(type: "int", nullable: true),
                    tran02cus01uin = table.Column<int>(type: "int", nullable: true),
                    tran02invoice_no = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    tran02bill_no = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    tran02taxable_amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    tran02tax_percent = table.Column<float>(type: "real", nullable: false),
                    tran02tax_amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    tran02sub_total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    tran02remark = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    tran02discount_amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    tran02discount_percent = table.Column<float>(type: "real", nullable: false),
                    tran02discount_type = table.Column<bool>(type: "bit", nullable: false),
                    tran02net_amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    tran02status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    tran02transaction_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    tran02created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    tran02updated_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    tran02created_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    tran02updated_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DeletedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DateCreated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DateDeleted = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tran02transaction_summaries", x => x.tran02uin);
                    table.ForeignKey(
                        name: "FK_tran02transaction_summaries_cus01customers_tran02cus01uin",
                        column: x => x.tran02cus01uin,
                        principalTable: "cus01customers",
                        principalColumn: "cus01uin",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tran02transaction_summaries_sup01suppliers_tran02sup01uin",
                        column: x => x.tran02sup01uin,
                        principalTable: "sup01suppliers",
                        principalColumn: "sup01uin",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tran02transaction_summaries_tran01transaction_types_tran02tran01uin",
                        column: x => x.tran02tran01uin,
                        principalTable: "tran01transaction_types",
                        principalColumn: "tran01uin",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "led01ledgers",
                columns: table => new
                {
                    led01uin = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    led01led05uin = table.Column<int>(type: "int", nullable: false),
                    led01title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    led01code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    led01desc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    led01open_bal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    led01balance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    led01prev_bal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    led01status = table.Column<bool>(type: "bit", nullable: false),
                    led01deleted = table.Column<bool>(type: "bit", nullable: false),
                    led01led03uin = table.Column<int>(type: "int", nullable: true),
                    led01related_id = table.Column<int>(type: "int", nullable: true),
                    led01isdefaultled = table.Column<bool>(type: "bit", nullable: true),
                    led01date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DeletedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DateCreated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DateDeleted = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_led01ledgers", x => x.led01uin);
                    table.ForeignKey(
                        name: "FK_led01ledgers_led03general_ledgers_led01led03uin",
                        column: x => x.led01led03uin,
                        principalTable: "led03general_ledgers",
                        principalColumn: "led03uin",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_led01ledgers_led05ledger_types_led01led05uin",
                        column: x => x.led01led05uin,
                        principalTable: "led05ledger_types",
                        principalColumn: "led05uin",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "pro03units",
                columns: table => new
                {
                    pro03uin = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    pro03pro02uin = table.Column<int>(type: "int", nullable: false),
                    pro03un01uin = table.Column<int>(type: "int", nullable: false),
                    pro03ratio = table.Column<double>(type: "float", nullable: false),
                    pro03last_cp = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    pro03last_sp = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    pro03status = table.Column<bool>(type: "bit", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DeletedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DateCreated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DateDeleted = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pro03units", x => x.pro03uin);
                    table.ForeignKey(
                        name: "FK_pro03units_pro02products_pro03pro02uin",
                        column: x => x.pro03pro02uin,
                        principalTable: "pro02products",
                        principalColumn: "pro02uin",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_pro03units_un01units_pro03un01uin",
                        column: x => x.pro03un01uin,
                        principalTable: "un01units",
                        principalColumn: "un01uin",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Prod02Consumerawproducts",
                columns: table => new
                {
                    prod02uin = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    prod02productionuin = table.Column<int>(type: "int", nullable: false),
                    prod02productuin = table.Column<int>(type: "int", nullable: false),
                    prod2productname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    prod02unituin = table.Column<int>(type: "int", nullable: false),
                    prod02unitname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    prod02rate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    prod02qty = table.Column<double>(type: "float", nullable: false),
                    prod02isallused = table.Column<bool>(type: "bit", nullable: false),
                    prod02remainingqty = table.Column<double>(type: "float", nullable: true),
                    CreatedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DeletedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DateCreated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DateDeleted = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prod02Consumerawproducts", x => x.prod02uin);
                    table.ForeignKey(
                        name: "FK_Prod02Consumerawproducts_Prod01Productions_prod02productionuin",
                        column: x => x.prod02productionuin,
                        principalTable: "Prod01Productions",
                        principalColumn: "prod01uin",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Prod02Consumerawproducts_pro02products_prod02productuin",
                        column: x => x.prod02productuin,
                        principalTable: "pro02products",
                        principalColumn: "pro02uin",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Prod02Consumerawproducts_un01units_prod02unituin",
                        column: x => x.prod02unituin,
                        principalTable: "un01units",
                        principalColumn: "un01uin",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "prod04finalproducts",
                columns: table => new
                {
                    prod04uin = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    prod4productionuin = table.Column<int>(type: "int", nullable: false),
                    prod04productuin = table.Column<int>(type: "int", nullable: false),
                    prod04productname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    prod04unituin = table.Column<int>(type: "int", nullable: false),
                    prod04unitname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    prod04unitratio = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    prod04desc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    prod04qty = table.Column<int>(type: "int", nullable: false),
                    prod04date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    prod04remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    prod04finalproductsprod04uin = table.Column<int>(type: "int", nullable: true),
                    CreatedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DeletedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DateCreated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DateDeleted = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_prod04finalproducts", x => x.prod04uin);
                    table.ForeignKey(
                        name: "FK_prod04finalproducts_Prod01Productions_prod4productionuin",
                        column: x => x.prod4productionuin,
                        principalTable: "Prod01Productions",
                        principalColumn: "prod01uin",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_prod04finalproducts_pro02products_prod04productuin",
                        column: x => x.prod04productuin,
                        principalTable: "pro02products",
                        principalColumn: "pro02uin",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_prod04finalproducts_prod04finalproducts_prod04finalproductsprod04uin",
                        column: x => x.prod04finalproductsprod04uin,
                        principalTable: "prod04finalproducts",
                        principalColumn: "prod04uin");
                    table.ForeignKey(
                        name: "FK_prod04finalproducts_un01units_prod04unituin",
                        column: x => x.prod04unituin,
                        principalTable: "un01units",
                        principalColumn: "un01uin",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Add04Chargepurchaserels",
                columns: table => new
                {
                    add04uin = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    add04puraddchargeuin = table.Column<int>(type: "int", nullable: false),
                    add04purchaseuin = table.Column<int>(type: "int", nullable: false),
                    add04isdeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Add04Chargepurchaserels", x => x.add04uin);
                    table.ForeignKey(
                        name: "FK_Add04Chargepurchaserels_Add02Purchaseadditionalcharges_add04puraddchargeuin",
                        column: x => x.add04puraddchargeuin,
                        principalTable: "Add02Purchaseadditionalcharges",
                        principalColumn: "add02uin",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Add04Chargepurchaserels_pur01purchases_add04purchaseuin",
                        column: x => x.add04purchaseuin,
                        principalTable: "pur01purchases",
                        principalColumn: "pur01uin",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CloudR2Purchase",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PurchaseId = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProcStatus = table.Column<int>(type: "int", nullable: false),
                    FailRemarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CloudR2Path = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CloudR2Purchase", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CloudR2Purchase_pur01purchases_PurchaseId",
                        column: x => x.PurchaseId,
                        principalTable: "pur01purchases",
                        principalColumn: "pur01uin",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "pur01purchasereturns",
                columns: table => new
                {
                    pur01returnuin = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    pur01uin = table.Column<int>(type: "int", nullable: false),
                    pur01return_date_nep = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    pur01return_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    pur01return_invoice_no = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    pur01ven01uin = table.Column<int>(type: "int", nullable: false),
                    pur01return_remarks = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    pur01return_sub_total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    pur01return_vat_per = table.Column<double>(type: "float", nullable: false),
                    pur01return_vat_amt = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    pur01return_total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    pur01return_disc_amt = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    pur01return_additional_disc = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    pur01return_net_amt = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    pur01return_status = table.Column<bool>(type: "bit", nullable: false),
                    pur01return_is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    pur01returnvoucher_no = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DeletedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DateCreated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DateDeleted = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pur01purchasereturns", x => x.pur01returnuin);
                    table.ForeignKey(
                        name: "FK_pur01purchasereturns_pur01purchases_pur01uin",
                        column: x => x.pur01uin,
                        principalTable: "pur01purchases",
                        principalColumn: "pur01uin",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_pur01purchasereturns_ven01vendors_pur01ven01uin",
                        column: x => x.pur01ven01uin,
                        principalTable: "ven01vendors",
                        principalColumn: "ven01uin",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "pur02items",
                columns: table => new
                {
                    pur02uin = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    pur02pur01uin = table.Column<int>(type: "int", nullable: false),
                    pur02pro02uin = table.Column<int>(type: "int", nullable: false),
                    pur02rate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    pur02qty = table.Column<int>(type: "int", nullable: false),
                    pur02un01uin = table.Column<int>(type: "int", nullable: false),
                    pur02amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    pur02mfg_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    pur02exp_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    pur02batch_no = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    pur02disc_amt = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    pur02net_amt = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DeletedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DateCreated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DateDeleted = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pur02items", x => x.pur02uin);
                    table.ForeignKey(
                        name: "FK_pur02items_pro02products_pur02pro02uin",
                        column: x => x.pur02pro02uin,
                        principalTable: "pro02products",
                        principalColumn: "pro02uin",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_pur02items_pur01purchases_pur02pur01uin",
                        column: x => x.pur02pur01uin,
                        principalTable: "pur01purchases",
                        principalColumn: "pur01uin",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_pur02items_un01units_pur02un01uin",
                        column: x => x.pur02un01uin,
                        principalTable: "un01units",
                        principalColumn: "un01uin",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "est02estimatesalesitems",
                columns: table => new
                {
                    est02uin = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    est02estimatesalesuin = table.Column<int>(type: "int", nullable: false),
                    est02pro02uin = table.Column<int>(type: "int", nullable: false),
                    est02qty = table.Column<double>(type: "float", nullable: false),
                    est02un01uin = table.Column<int>(type: "int", nullable: false),
                    est02rate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    est02sub_total = table.Column<double>(type: "float", nullable: false),
                    est02disc_amt = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    est02net_amt = table.Column<double>(type: "float", nullable: false),
                    est02ref_no = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    est02emp01uin = table.Column<int>(type: "int", nullable: true),
                    est02vec02uin = table.Column<int>(type: "int", nullable: true),
                    est02vatper = table.Column<double>(type: "float", nullable: true),
                    est02transportationfee = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    est02destination = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    pro02productspro02uin = table.Column<int>(type: "int", nullable: true),
                    un01unitsun01uin = table.Column<int>(type: "int", nullable: true),
                    CreatedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DeletedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DateCreated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DateDeleted = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_est02estimatesalesitems", x => x.est02uin);
                    table.ForeignKey(
                        name: "FK_est02estimatesalesitems_est02estimatesales_est02estimatesalesuin",
                        column: x => x.est02estimatesalesuin,
                        principalTable: "est02estimatesales",
                        principalColumn: "est02uin",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_est02estimatesalesitems_pro02products_est02pro02uin",
                        column: x => x.est02pro02uin,
                        principalTable: "pro02products",
                        principalColumn: "pro02uin",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_est02estimatesalesitems_pro02products_pro02productspro02uin",
                        column: x => x.pro02productspro02uin,
                        principalTable: "pro02products",
                        principalColumn: "pro02uin");
                    table.ForeignKey(
                        name: "FK_est02estimatesalesitems_un01units_est02un01uin",
                        column: x => x.est02un01uin,
                        principalTable: "un01units",
                        principalColumn: "un01uin",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_est02estimatesalesitems_un01units_un01unitsun01uin",
                        column: x => x.un01unitsun01uin,
                        principalTable: "un01units",
                        principalColumn: "un01uin");
                });

            migrationBuilder.CreateTable(
                name: "cas01cashsettlement",
                columns: table => new
                {
                    cas01uin = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    cas01purchaseuin = table.Column<int>(type: "int", nullable: true),
                    cas01saleuin = table.Column<int>(type: "int", nullable: true),
                    cas01customeruin = table.Column<int>(type: "int", nullable: true),
                    cas01vendoruin = table.Column<int>(type: "int", nullable: true),
                    cas01invoice_no = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    cas01payment_status = table.Column<int>(type: "int", nullable: true),
                    cas01payment_type = table.Column<int>(type: "int", nullable: false),
                    cas01amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    cas01remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    cas01isbank = table.Column<bool>(type: "bit", nullable: false),
                    cas01bank_ledname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    cas01chqnumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    cas01tdspercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    cas01transaction_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    cas0101voucher_no = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DeletedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DateCreated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DateDeleted = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cas01cashsettlement", x => x.cas01uin);
                    table.ForeignKey(
                        name: "FK_cas01cashsettlement_cus01customers_cas01customeruin",
                        column: x => x.cas01customeruin,
                        principalTable: "cus01customers",
                        principalColumn: "cus01uin",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_cas01cashsettlement_pur01purchases_cas01purchaseuin",
                        column: x => x.cas01purchaseuin,
                        principalTable: "pur01purchases",
                        principalColumn: "pur01uin",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_cas01cashsettlement_sal01sales_cas01saleuin",
                        column: x => x.cas01saleuin,
                        principalTable: "sal01sales",
                        principalColumn: "sal01uin",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_cas01cashsettlement_ven01vendors_cas01vendoruin",
                        column: x => x.cas01vendoruin,
                        principalTable: "ven01vendors",
                        principalColumn: "ven01uin",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "sal01salesreturn",
                columns: table => new
                {
                    sal01uin = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    sal01_salId = table.Column<int>(type: "int", nullable: false),
                    sal01date_nep = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    sal01date_eng = table.Column<DateTime>(type: "datetime2", nullable: false),
                    sal01cus01uin = table.Column<int>(type: "int", nullable: false),
                    sal01invoice_no = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    sal01remarks = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    sal01sub_total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    sal01disc_amt = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    sal01total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    sal01vat_per = table.Column<double>(type: "float", nullable: false),
                    sal01vat_amt = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    sal01net_amt = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    sal01status = table.Column<bool>(type: "bit", nullable: false),
                    sal01deleted = table.Column<bool>(type: "bit", nullable: false),
                    sal01returnvoucher_no = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DeletedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DateCreated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DateDeleted = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sal01salesreturn", x => x.sal01uin);
                    table.ForeignKey(
                        name: "FK_sal01salesreturn_cus01customers_sal01cus01uin",
                        column: x => x.sal01cus01uin,
                        principalTable: "cus01customers",
                        principalColumn: "cus01uin",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_sal01salesreturn_sal01sales_sal01_salId",
                        column: x => x.sal01_salId,
                        principalTable: "sal01sales",
                        principalColumn: "sal01uin",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "sal02items",
                columns: table => new
                {
                    sal02uin = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    sal02sal01uin = table.Column<int>(type: "int", nullable: false),
                    sal02pro02uin = table.Column<int>(type: "int", nullable: false),
                    sal02qty = table.Column<double>(type: "float", nullable: false),
                    sal02un01uin = table.Column<int>(type: "int", nullable: false),
                    sal02rate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    sal02sub_total = table.Column<double>(type: "float", nullable: false),
                    sal02disc_amt = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    sal02net_amt = table.Column<double>(type: "float", nullable: false),
                    sal02ref_no = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sal02emp01uin = table.Column<int>(type: "int", nullable: true),
                    sal02vec02uin = table.Column<int>(type: "int", nullable: true),
                    sal02vatper = table.Column<double>(type: "float", nullable: true),
                    sal02transportationfee = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    sal02destination = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DeletedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DateCreated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DateDeleted = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sal02items", x => x.sal02uin);
                    table.ForeignKey(
                        name: "FK_sal02items_pro02products_sal02pro02uin",
                        column: x => x.sal02pro02uin,
                        principalTable: "pro02products",
                        principalColumn: "pro02uin",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_sal02items_sal01sales_sal02sal01uin",
                        column: x => x.sal02sal01uin,
                        principalTable: "sal01sales",
                        principalColumn: "sal01uin",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_sal02items_un01units_sal02un01uin",
                        column: x => x.sal02un01uin,
                        principalTable: "un01units",
                        principalColumn: "un01uin",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tran04transaction_out_details",
                columns: table => new
                {
                    tran04uin = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tran04tran02uin = table.Column<int>(type: "int", nullable: false),
                    tran04pro02uin = table.Column<int>(type: "int", nullable: false),
                    tran04sal01uin = table.Column<int>(type: "int", nullable: false),
                    tran04unit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    tran04unit_price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    tran04quantity = table.Column<float>(type: "real", nullable: false),
                    tran04base_quantity = table.Column<float>(type: "real", nullable: false),
                    tran04taxable = table.Column<bool>(type: "bit", nullable: false),
                    tran04tax_percent = table.Column<float>(type: "real", nullable: false),
                    tran04tax_amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    tran04sub_total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    tran04discount_amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    tran04discount_percent = table.Column<float>(type: "real", nullable: false),
                    tran04discount_type = table.Column<bool>(type: "bit", nullable: false),
                    tran04net_amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    tran04status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DeletedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DateCreated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DateDeleted = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tran04transaction_out_details", x => x.tran04uin);
                    table.ForeignKey(
                        name: "FK_tran04transaction_out_details_pro02products_tran04pro02uin",
                        column: x => x.tran04pro02uin,
                        principalTable: "pro02products",
                        principalColumn: "pro02uin",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tran04transaction_out_details_sal01sales_tran04sal01uin",
                        column: x => x.tran04sal01uin,
                        principalTable: "sal01sales",
                        principalColumn: "sal01uin",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tran04transaction_out_details_tran02transaction_summaries_tran04tran02uin",
                        column: x => x.tran04tran02uin,
                        principalTable: "tran02transaction_summaries",
                        principalColumn: "tran02uin",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "vou02voucher_summary",
                columns: table => new
                {
                    vou02full_no = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    vou02number = table.Column<int>(type: "int", nullable: false),
                    vou02vou01uin = table.Column<int>(type: "int", nullable: false),
                    vou02amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    vou02description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    vou02chq = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    vou02contra_led05uin = table.Column<int>(type: "int", nullable: true),
                    vou02manual_vno = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    vou02deleted = table.Column<bool>(type: "bit", nullable: false),
                    vou02is_approved = table.Column<bool>(type: "bit", nullable: false),
                    vou02is_sys_generated = table.Column<bool>(type: "bit", nullable: false),
                    vou02value_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    vou02status = table.Column<int>(type: "int", nullable: false),
                    CreatedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DeletedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DateCreated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DateDeleted = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vou02voucher_summary", x => x.vou02full_no);
                    table.ForeignKey(
                        name: "FK_vou02voucher_summary_led01ledgers_vou02contra_led05uin",
                        column: x => x.vou02contra_led05uin,
                        principalTable: "led01ledgers",
                        principalColumn: "led01uin",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_vou02voucher_summary_vou01voucher_types_vou02vou01uin",
                        column: x => x.vou02vou01uin,
                        principalTable: "vou01voucher_types",
                        principalColumn: "vou01uin",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "pur02returnitems",
                columns: table => new
                {
                    pur02returnuin = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    pur02returnpur01uin = table.Column<int>(type: "int", nullable: false),
                    pur02returnpro02uin = table.Column<int>(type: "int", nullable: false),
                    pur02returnrate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    pur02returnqty = table.Column<int>(type: "int", nullable: false),
                    pur02returnun01uin = table.Column<int>(type: "int", nullable: false),
                    pur02returnamount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    pur02returnmfg_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    pur02returnexp_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    pur02returnbatch_no = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    pur02returndisc_amt = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    pur02net_amt = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DeletedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DateCreated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DateDeleted = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pur02returnitems", x => x.pur02returnuin);
                    table.ForeignKey(
                        name: "FK_pur02returnitems_pro02products_pur02returnpro02uin",
                        column: x => x.pur02returnpro02uin,
                        principalTable: "pro02products",
                        principalColumn: "pro02uin",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_pur02returnitems_pur01purchasereturns_pur02returnpur01uin",
                        column: x => x.pur02returnpur01uin,
                        principalTable: "pur01purchasereturns",
                        principalColumn: "pur01returnuin",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_pur02returnitems_un01units_pur02returnun01uin",
                        column: x => x.pur02returnun01uin,
                        principalTable: "un01units",
                        principalColumn: "un01uin",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "sal02itemsreturn",
                columns: table => new
                {
                    sal02uin = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    sal02sal01uin = table.Column<int>(type: "int", nullable: false),
                    sal02pro02uin = table.Column<int>(type: "int", nullable: false),
                    sal02qty = table.Column<double>(type: "float", nullable: false),
                    sal02un01uin = table.Column<int>(type: "int", nullable: false),
                    sal02rate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    sal02sub_total = table.Column<double>(type: "float", nullable: false),
                    sal02disc_amt = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    sal02net_amt = table.Column<double>(type: "float", nullable: false),
                    sal02ref_no = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sal02emp01uin = table.Column<int>(type: "int", nullable: true),
                    sal02vec02uin = table.Column<int>(type: "int", nullable: true),
                    sal02transportationfee = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    sal02destination = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DeletedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DateCreated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DateDeleted = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sal02itemsreturn", x => x.sal02uin);
                    table.ForeignKey(
                        name: "FK_sal02itemsreturn_pro02products_sal02pro02uin",
                        column: x => x.sal02pro02uin,
                        principalTable: "pro02products",
                        principalColumn: "pro02uin",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_sal02itemsreturn_sal01salesreturn_sal02sal01uin",
                        column: x => x.sal02sal01uin,
                        principalTable: "sal01salesreturn",
                        principalColumn: "sal01uin",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_sal02itemsreturn_un01units_sal02un01uin",
                        column: x => x.sal02un01uin,
                        principalTable: "un01units",
                        principalColumn: "un01uin",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "vou03voucher_details",
                columns: table => new
                {
                    vou03uin = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    vou03vou02full_no = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    vou03led05uin = table.Column<int>(type: "int", nullable: false),
                    vou03dr = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    vou03cr = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    vou03description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    vou03chq = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    vou03balance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DeletedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DateCreated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DateDeleted = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vou03voucher_details", x => x.vou03uin);
                    table.ForeignKey(
                        name: "FK_vou03voucher_details_led01ledgers_vou03led05uin",
                        column: x => x.vou03led05uin,
                        principalTable: "led01ledgers",
                        principalColumn: "led01uin",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_vou03voucher_details_vou02voucher_summary_vou03vou02full_no",
                        column: x => x.vou03vou02full_no,
                        principalTable: "vou02voucher_summary",
                        principalColumn: "vou02full_no",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "vou04file_attachments",
                columns: table => new
                {
                    vou04uin = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    vou04vou02full_no = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    vou04filename = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    vou04location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    vou04deleted = table.Column<bool>(type: "bit", nullable: false),
                    vou04created_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    vou04updated_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    vou04created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    vou04updated_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DeletedName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DateCreated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateUpdated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DateDeleted = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vou04file_attachments", x => x.vou04uin);
                    table.ForeignKey(
                        name: "FK_vou04file_attachments_vou02voucher_summary_vou04vou02full_no",
                        column: x => x.vou04vou02full_no,
                        principalTable: "vou02voucher_summary",
                        principalColumn: "vou02full_no",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "BranchDatas",
                columns: new[] { "BranchCode", "BranchName", "CreatedName", "DateCreated", "DateDeleted", "DateUpdated", "DeletedName", "IsDefault", "UpdatedName" },
                values: new object[] { "HO-001", "Head Office", "SYSTEM", new DateTimeOffset(new DateTime(2025, 3, 21, 4, 21, 55, 703, DateTimeKind.Unspecified).AddTicks(500), new TimeSpan(0, 0, 0, 0, 0)), null, null, "", true, "" });

            migrationBuilder.InsertData(
                table: "MainSetups",
                columns: new[] { "Id", "CreatedName", "DateCreated", "DateDeleted", "DateUpdated", "DbName", "DbPassword", "DeletedName", "Logo", "OrgName", "Server", "UpdatedName" },
                values: new object[] { -1, "SYSTEM", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "POS_V1", "asdf", "", null, "Default Org", ".", "" });

            migrationBuilder.InsertData(
                table: "led05ledger_types",
                columns: new[] { "led05uin", "CreatedName", "DateCreated", "DateDeleted", "DateUpdated", "DeletedName", "UpdatedName", "led05add_dr", "led05title", "led05title_nep" },
                values: new object[,]
                {
                    { 1, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "", "", true, "Assets", " सम्पत्ति" },
                    { 2, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "", "", false, "Liabilities", " देयता" },
                    { 3, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "", "", false, "Income", " आम्दानी" },
                    { 4, "", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "", "", true, "Expenses", "व्यय " }
                });

            migrationBuilder.InsertData(
                table: "led01ledgers",
                columns: new[] { "led01uin", "CreatedName", "DateCreated", "DateDeleted", "DateUpdated", "DeletedName", "UpdatedName", "led01balance", "led01code", "led01date", "led01deleted", "led01desc", "led01isdefaultled", "led01led03uin", "led01led05uin", "led01open_bal", "led01prev_bal", "led01related_id", "led01status", "led01title" },
                values: new object[,]
                {
                    { -2, "SYSTEM", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "", "", 0m, "PurchaseDiscount", null, false, "Discount detail", null, null, 4, 0m, 0m, 0, true, "Discount Received" },
                    { -1, "SYSTEM", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "", "", 0m, "SalesDiscount", null, false, "Discount detail", null, null, 4, 0m, 0m, 0, true, "Discount Given" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Add03Purchaseadditionalchargesdetails_add02uin",
                table: "Add03Purchaseadditionalchargesdetails",
                column: "add02uin");

            migrationBuilder.CreateIndex(
                name: "IX_Add04Chargepurchaserels_add04puraddchargeuin",
                table: "Add04Chargepurchaserels",
                column: "add04puraddchargeuin");

            migrationBuilder.CreateIndex(
                name: "IX_Add04Chargepurchaserels_add04purchaseuin",
                table: "Add04Chargepurchaserels",
                column: "add04purchaseuin");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_cas01cashsettlement_cas01customeruin",
                table: "cas01cashsettlement",
                column: "cas01customeruin");

            migrationBuilder.CreateIndex(
                name: "IX_cas01cashsettlement_cas01purchaseuin",
                table: "cas01cashsettlement",
                column: "cas01purchaseuin");

            migrationBuilder.CreateIndex(
                name: "IX_cas01cashsettlement_cas01saleuin",
                table: "cas01cashsettlement",
                column: "cas01saleuin");

            migrationBuilder.CreateIndex(
                name: "IX_cas01cashsettlement_cas01vendoruin",
                table: "cas01cashsettlement",
                column: "cas01vendoruin");

            migrationBuilder.CreateIndex(
                name: "IX_CloudR2Purchase_PurchaseId",
                table: "CloudR2Purchase",
                column: "PurchaseId");

            migrationBuilder.CreateIndex(
                name: "IX_cus01customers_BranchCode",
                table: "cus01customers",
                column: "BranchCode");

            migrationBuilder.CreateIndex(
                name: "IX_cus01customers_cus01customerTypeId",
                table: "cus01customers",
                column: "cus01customerTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_est02estimatesales_cus01customerscus01uin",
                table: "est02estimatesales",
                column: "cus01customerscus01uin");

            migrationBuilder.CreateIndex(
                name: "IX_est02estimatesales_est02est01uin",
                table: "est02estimatesales",
                column: "est02est01uin");

            migrationBuilder.CreateIndex(
                name: "IX_est02estimatesalesitems_est02estimatesalesuin",
                table: "est02estimatesalesitems",
                column: "est02estimatesalesuin");

            migrationBuilder.CreateIndex(
                name: "IX_est02estimatesalesitems_est02pro02uin",
                table: "est02estimatesalesitems",
                column: "est02pro02uin");

            migrationBuilder.CreateIndex(
                name: "IX_est02estimatesalesitems_est02un01uin",
                table: "est02estimatesalesitems",
                column: "est02un01uin");

            migrationBuilder.CreateIndex(
                name: "IX_est02estimatesalesitems_pro02productspro02uin",
                table: "est02estimatesalesitems",
                column: "pro02productspro02uin");

            migrationBuilder.CreateIndex(
                name: "IX_est02estimatesalesitems_un01unitsun01uin",
                table: "est02estimatesalesitems",
                column: "un01unitsun01uin");

            migrationBuilder.CreateIndex(
                name: "IX_led01ledgers_led01led03uin",
                table: "led01ledgers",
                column: "led01led03uin");

            migrationBuilder.CreateIndex(
                name: "IX_led01ledgers_led01led05uin",
                table: "led01ledgers",
                column: "led01led05uin");

            migrationBuilder.CreateIndex(
                name: "IX_led03general_ledgers_led03led03uin",
                table: "led03general_ledgers",
                column: "led03led03uin");

            migrationBuilder.CreateIndex(
                name: "IX_led03general_ledgers_led03led05uin",
                table: "led03general_ledgers",
                column: "led03led05uin");

            migrationBuilder.CreateIndex(
                name: "IX_pro02products_pro02pro01uin",
                table: "pro02products",
                column: "pro02pro01uin");

            migrationBuilder.CreateIndex(
                name: "IX_pro02products_pro02un01uin",
                table: "pro02products",
                column: "pro02un01uin");

            migrationBuilder.CreateIndex(
                name: "IX_pro03units_pro03pro02uin",
                table: "pro03units",
                column: "pro03pro02uin");

            migrationBuilder.CreateIndex(
                name: "IX_pro03units_pro03un01uin",
                table: "pro03units",
                column: "pro03un01uin");

            migrationBuilder.CreateIndex(
                name: "IX_Prod02Consumerawproducts_prod02productionuin",
                table: "Prod02Consumerawproducts",
                column: "prod02productionuin");

            migrationBuilder.CreateIndex(
                name: "IX_Prod02Consumerawproducts_prod02productuin",
                table: "Prod02Consumerawproducts",
                column: "prod02productuin");

            migrationBuilder.CreateIndex(
                name: "IX_Prod02Consumerawproducts_prod02unituin",
                table: "Prod02Consumerawproducts",
                column: "prod02unituin");

            migrationBuilder.CreateIndex(
                name: "IX_prod03statuslog_prod03productionuin",
                table: "prod03statuslog",
                column: "prod03productionuin");

            migrationBuilder.CreateIndex(
                name: "IX_prod04finalproducts_prod04finalproductsprod04uin",
                table: "prod04finalproducts",
                column: "prod04finalproductsprod04uin");

            migrationBuilder.CreateIndex(
                name: "IX_prod04finalproducts_prod04productuin",
                table: "prod04finalproducts",
                column: "prod04productuin");

            migrationBuilder.CreateIndex(
                name: "IX_prod04finalproducts_prod04unituin",
                table: "prod04finalproducts",
                column: "prod04unituin");

            migrationBuilder.CreateIndex(
                name: "IX_prod04finalproducts_prod4productionuin",
                table: "prod04finalproducts",
                column: "prod4productionuin");

            migrationBuilder.CreateIndex(
                name: "IX_pur01purchasereturns_pur01uin",
                table: "pur01purchasereturns",
                column: "pur01uin");

            migrationBuilder.CreateIndex(
                name: "IX_pur01purchasereturns_pur01ven01uin",
                table: "pur01purchasereturns",
                column: "pur01ven01uin");

            migrationBuilder.CreateIndex(
                name: "IX_pur01purchases_BranchCode",
                table: "pur01purchases",
                column: "BranchCode");

            migrationBuilder.CreateIndex(
                name: "IX_pur01purchases_pur01ven01uin",
                table: "pur01purchases",
                column: "pur01ven01uin");

            migrationBuilder.CreateIndex(
                name: "IX_pur02items_pur02pro02uin",
                table: "pur02items",
                column: "pur02pro02uin");

            migrationBuilder.CreateIndex(
                name: "IX_pur02items_pur02pur01uin",
                table: "pur02items",
                column: "pur02pur01uin");

            migrationBuilder.CreateIndex(
                name: "IX_pur02items_pur02un01uin",
                table: "pur02items",
                column: "pur02un01uin");

            migrationBuilder.CreateIndex(
                name: "IX_pur02returnitems_pur02returnpro02uin",
                table: "pur02returnitems",
                column: "pur02returnpro02uin");

            migrationBuilder.CreateIndex(
                name: "IX_pur02returnitems_pur02returnpur01uin",
                table: "pur02returnitems",
                column: "pur02returnpur01uin");

            migrationBuilder.CreateIndex(
                name: "IX_pur02returnitems_pur02returnun01uin",
                table: "pur02returnitems",
                column: "pur02returnun01uin");

            migrationBuilder.CreateIndex(
                name: "IX_sal01sales_BranchCode",
                table: "sal01sales",
                column: "BranchCode");

            migrationBuilder.CreateIndex(
                name: "IX_sal01sales_sal01cus01uin",
                table: "sal01sales",
                column: "sal01cus01uin");

            migrationBuilder.CreateIndex(
                name: "IX_sal01salesreturn_sal01_salId",
                table: "sal01salesreturn",
                column: "sal01_salId");

            migrationBuilder.CreateIndex(
                name: "IX_sal01salesreturn_sal01cus01uin",
                table: "sal01salesreturn",
                column: "sal01cus01uin");

            migrationBuilder.CreateIndex(
                name: "IX_sal02items_sal02pro02uin",
                table: "sal02items",
                column: "sal02pro02uin");

            migrationBuilder.CreateIndex(
                name: "IX_sal02items_sal02sal01uin",
                table: "sal02items",
                column: "sal02sal01uin");

            migrationBuilder.CreateIndex(
                name: "IX_sal02items_sal02un01uin",
                table: "sal02items",
                column: "sal02un01uin");

            migrationBuilder.CreateIndex(
                name: "IX_sal02itemsreturn_sal02pro02uin",
                table: "sal02itemsreturn",
                column: "sal02pro02uin");

            migrationBuilder.CreateIndex(
                name: "IX_sal02itemsreturn_sal02sal01uin",
                table: "sal02itemsreturn",
                column: "sal02sal01uin");

            migrationBuilder.CreateIndex(
                name: "IX_sal02itemsreturn_sal02un01uin",
                table: "sal02itemsreturn",
                column: "sal02un01uin");

            migrationBuilder.CreateIndex(
                name: "IX_tran02transaction_summaries_tran02cus01uin",
                table: "tran02transaction_summaries",
                column: "tran02cus01uin");

            migrationBuilder.CreateIndex(
                name: "IX_tran02transaction_summaries_tran02sup01uin",
                table: "tran02transaction_summaries",
                column: "tran02sup01uin");

            migrationBuilder.CreateIndex(
                name: "IX_tran02transaction_summaries_tran02tran01uin",
                table: "tran02transaction_summaries",
                column: "tran02tran01uin");

            migrationBuilder.CreateIndex(
                name: "IX_tran04transaction_out_details_tran04pro02uin",
                table: "tran04transaction_out_details",
                column: "tran04pro02uin");

            migrationBuilder.CreateIndex(
                name: "IX_tran04transaction_out_details_tran04sal01uin",
                table: "tran04transaction_out_details",
                column: "tran04sal01uin");

            migrationBuilder.CreateIndex(
                name: "IX_tran04transaction_out_details_tran04tran02uin",
                table: "tran04transaction_out_details",
                column: "tran04tran02uin");

            migrationBuilder.CreateIndex(
                name: "IX_UserBranches_BranchCode",
                table: "UserBranches",
                column: "BranchCode");

            migrationBuilder.CreateIndex(
                name: "IX_ven01vendors_BranchCode",
                table: "ven01vendors",
                column: "BranchCode");

            migrationBuilder.CreateIndex(
                name: "IX_vou02voucher_summary_vou02contra_led05uin",
                table: "vou02voucher_summary",
                column: "vou02contra_led05uin");

            migrationBuilder.CreateIndex(
                name: "IX_vou02voucher_summary_vou02vou01uin",
                table: "vou02voucher_summary",
                column: "vou02vou01uin");

            migrationBuilder.CreateIndex(
                name: "IX_vou03voucher_details_vou03led05uin",
                table: "vou03voucher_details",
                column: "vou03led05uin");

            migrationBuilder.CreateIndex(
                name: "IX_vou03voucher_details_vou03vou02full_no",
                table: "vou03voucher_details",
                column: "vou03vou02full_no");

            migrationBuilder.CreateIndex(
                name: "IX_vou04file_attachments_vou04vou02full_no",
                table: "vou04file_attachments",
                column: "vou04vou02full_no");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccessLists");

            migrationBuilder.DropTable(
                name: "Add01Additionalcharges");

            migrationBuilder.DropTable(
                name: "Add03Purchaseadditionalchargesdetails");

            migrationBuilder.DropTable(
                name: "Add04Chargepurchaserels");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "cas01cashsettlement");

            migrationBuilder.DropTable(
                name: "cfg01configurations");

            migrationBuilder.DropTable(
                name: "CloudR2Purchase");

            migrationBuilder.DropTable(
                name: "ConfigurationSettings");

            migrationBuilder.DropTable(
                name: "est02estimatesalesitems");

            migrationBuilder.DropTable(
                name: "MainSetups");

            migrationBuilder.DropTable(
                name: "pro03units");

            migrationBuilder.DropTable(
                name: "Prod02Consumerawproducts");

            migrationBuilder.DropTable(
                name: "prod03statuslog");

            migrationBuilder.DropTable(
                name: "prod04finalproducts");

            migrationBuilder.DropTable(
                name: "pur02items");

            migrationBuilder.DropTable(
                name: "pur02returnitems");

            migrationBuilder.DropTable(
                name: "sal02items");

            migrationBuilder.DropTable(
                name: "sal02itemsreturn");

            migrationBuilder.DropTable(
                name: "settings");

            migrationBuilder.DropTable(
                name: "ta01taxsettlement");

            migrationBuilder.DropTable(
                name: "tran04transaction_out_details");

            migrationBuilder.DropTable(
                name: "UserBranches");

            migrationBuilder.DropTable(
                name: "UserPermissionLists");

            migrationBuilder.DropTable(
                name: "vou03voucher_details");

            migrationBuilder.DropTable(
                name: "vou04file_attachments");

            migrationBuilder.DropTable(
                name: "vou05voucher_log");

            migrationBuilder.DropTable(
                name: "Wag01Wagerates");

            migrationBuilder.DropTable(
                name: "Add02Purchaseadditionalcharges");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "est02estimatesales");

            migrationBuilder.DropTable(
                name: "Prod01Productions");

            migrationBuilder.DropTable(
                name: "pur01purchasereturns");

            migrationBuilder.DropTable(
                name: "sal01salesreturn");

            migrationBuilder.DropTable(
                name: "pro02products");

            migrationBuilder.DropTable(
                name: "tran02transaction_summaries");

            migrationBuilder.DropTable(
                name: "vou02voucher_summary");

            migrationBuilder.DropTable(
                name: "est01estimate");

            migrationBuilder.DropTable(
                name: "pur01purchases");

            migrationBuilder.DropTable(
                name: "sal01sales");

            migrationBuilder.DropTable(
                name: "pro01categories");

            migrationBuilder.DropTable(
                name: "un01units");

            migrationBuilder.DropTable(
                name: "sup01suppliers");

            migrationBuilder.DropTable(
                name: "tran01transaction_types");

            migrationBuilder.DropTable(
                name: "led01ledgers");

            migrationBuilder.DropTable(
                name: "vou01voucher_types");

            migrationBuilder.DropTable(
                name: "ven01vendors");

            migrationBuilder.DropTable(
                name: "cus01customers");

            migrationBuilder.DropTable(
                name: "led03general_ledgers");

            migrationBuilder.DropTable(
                name: "BranchDatas");

            migrationBuilder.DropTable(
                name: "cus02customerType");

            migrationBuilder.DropTable(
                name: "led05ledger_types");
        }
    }
}
