using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Download.Infrastructure.Migrations;

/// <inheritdoc />
public partial class _20261126053228_FirstDbMigration : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
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
            name: "ExternalLinksModels",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                ExternalLinkName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ExernalLinkDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ExternalLinkUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ExternalLinksModels", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "GameTagModels",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                TagName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Slug = table.Column<string>(type: "nvarchar(max)", nullable: true),
                TagPlatform = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                RequestCount = table.Column<int>(type: "int", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_GameTagModels", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "MagazineModels",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                MagazineName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                MagazineDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                MagazineUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_MagazineModels", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "MisturebaModels",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                LocalOndeFica = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Segredo = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_MisturebaModels", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "PlataformModels",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                PlatformName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Slug = table.Column<string>(type: "nvarchar(max)", nullable: true),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                PlatformDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                PlataformDeveloper = table.Column<string>(type: "nvarchar(max)", nullable: true),
                PlataformGamesBoxArt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                PlataformGamesSnipet = table.Column<string>(type: "nvarchar(max)", nullable: true),
                PlataformGamesNamedTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                PlataformGameImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                PlatafomGameLogo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                PlataformGameImageInternalUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                PlatafomGameLogoInternalUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                TotalGames = table.Column<int>(type: "int", nullable: false),
                Count = table.Column<int>(type: "int", nullable: false),
                PlataformGamesFolder = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_PlataformModels", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "PostModels",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Slug = table.Column<string>(type: "nvarchar(max)", nullable: true),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                Paragraph1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Paragraph2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Paragraph3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Paragraph4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Paragraph5 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Strong1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Strong2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Strong3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Strong4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Strong5 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ParagrathPreview = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ImageUrl1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ImageUrl2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ImageUrl3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ImageUrl4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ImageUrl5 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ImageUrl1InternalUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ImageUrl2InternalUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ImageUrl3InternalUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ImageUrl4InternalUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ImageUrl5InternalUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                UrlVideo1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                UrlVideo2 = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_PostModels", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Publicitys",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                PublicityName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Company = table.Column<string>(type: "nvarchar(max)", nullable: true),
                PublicityFolder = table.Column<string>(type: "nvarchar(max)", nullable: true),
                PublictyId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ReleaseDate = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Publicitys", x => x.Id);
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
            name: "Emulators",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                EmulatorName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                EmulatorVersion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Slug = table.Column<string>(type: "nvarchar(max)", nullable: true),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                EmulatorDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                EmulatorOS = table.Column<string>(type: "nvarchar(max)", nullable: true),
                EmulatorInternalUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                EmulatorUrlOficial = table.Column<string>(type: "nvarchar(max)", nullable: true),
                EmulatorFolder = table.Column<string>(type: "nvarchar(max)", nullable: true),
                EmulatorLogo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                EmulatorSnipet = table.Column<string>(type: "nvarchar(max)", nullable: true),
                EmulatorLogoInternalUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                EmulatorSnipetInternalUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                PlataformModelsId = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Emulators", x => x.Id);
                table.ForeignKey(
                    name: "FK_Emulators_PlataformModels_PlataformModelsId",
                    column: x => x.PlataformModelsId,
                    principalTable: "PlataformModels",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "GameCategoryModels",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                GameCategoryModelName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                GameCategoryModelDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                PlataformId = table.Column<int>(type: "int", nullable: true),
                PlataformModelssId = table.Column<int>(type: "int", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_GameCategoryModels", x => x.Id);
                table.ForeignKey(
                    name: "FK_GameCategoryModels_PlataformModels_PlataformModelssId",
                    column: x => x.PlataformModelssId,
                    principalTable: "PlataformModels",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateTable(
            name: "GameModels",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                GameName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Slug = table.Column<string>(type: "nvarchar(max)", nullable: true),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                GameDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                GameLenguage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                GameOriginalOrMode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ImageBoxArt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ImageSnaps = table.Column<string>(type: "nvarchar(max)", nullable: false),
                NamedTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ImageBoxArtUrlInternal = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ImageSnapsUrlInternal = table.Column<string>(type: "nvarchar(max)", nullable: false),
                NamedTitleUrlInternal = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Video1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Video2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Video3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Count = table.Column<int>(type: "int", nullable: false),
                PlataformId = table.Column<int>(type: "int", nullable: false),
                PlataformSlug = table.Column<string>(type: "nvarchar(max)", nullable: false),
                PlataformModelsId = table.Column<int>(type: "int", nullable: true),
                GameCategoryModelIds = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_GameModels", x => x.Id);
                table.ForeignKey(
                    name: "FK_GameModels_PlataformModels_PlataformModelsId",
                    column: x => x.PlataformModelsId,
                    principalTable: "PlataformModels",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateTable(
            name: "GameCategoryModelGameModel",
            columns: table => new
            {
                GameCategoryModelsId = table.Column<int>(type: "int", nullable: false),
                GameModelssId = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_GameCategoryModelGameModel", x => new { x.GameCategoryModelsId, x.GameModelssId });
                table.ForeignKey(
                    name: "FK_GameCategoryModelGameModel_GameCategoryModels_GameCategoryModelsId",
                    column: x => x.GameCategoryModelsId,
                    principalTable: "GameCategoryModels",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_GameCategoryModelGameModel_GameModels_GameModelssId",
                    column: x => x.GameModelssId,
                    principalTable: "GameModels",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.InsertData(
            table: "MisturebaModels",
            columns: new[] { "Id", "LocalOndeFica", "Name", "Segredo" },
            values: new object[] { 1, "E:\\PROJETOS GIT CURRICULO\\Api games\\DownloadApi\\Download.Infrastructure\\bin\\Debug\\net11.0\\appsettings.json", "appsettings.json", "Arquivo de configuração da aplicação" });

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
            name: "IX_Emulators_PlataformModelsId",
            table: "Emulators",
            column: "PlataformModelsId");

        migrationBuilder.CreateIndex(
            name: "IX_GameCategoryModelGameModel_GameModelssId",
            table: "GameCategoryModelGameModel",
            column: "GameModelssId");

        migrationBuilder.CreateIndex(
            name: "IX_GameCategoryModels_PlataformModelssId",
            table: "GameCategoryModels",
            column: "PlataformModelssId");

        migrationBuilder.CreateIndex(
            name: "IX_GameModels_PlataformModelsId",
            table: "GameModels",
            column: "PlataformModelsId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
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
            name: "Emulators");

        migrationBuilder.DropTable(
            name: "ExternalLinksModels");

        migrationBuilder.DropTable(
            name: "GameCategoryModelGameModel");

        migrationBuilder.DropTable(
            name: "GameTagModels");

        migrationBuilder.DropTable(
            name: "MagazineModels");

        migrationBuilder.DropTable(
            name: "MisturebaModels");

        migrationBuilder.DropTable(
            name: "PostModels");

        migrationBuilder.DropTable(
            name: "Publicitys");

        migrationBuilder.DropTable(
            name: "AspNetRoles");

        migrationBuilder.DropTable(
            name: "AspNetUsers");

        migrationBuilder.DropTable(
            name: "GameCategoryModels");

        migrationBuilder.DropTable(
            name: "GameModels");

        migrationBuilder.DropTable(
            name: "PlataformModels");
    }
}
