using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace aaa_aspdotnet.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DeviceTypes",
                columns: table => new
                {
                    TypeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceTypes", x => x.TypeID);
                });

            migrationBuilder.CreateTable(
                name: "Plans",
                columns: table => new
                {
                    PlanID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Contents = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    CreateBy = table.Column<int>(type: "int", nullable: true),
                    BeginDate = table.Column<DateTime>(type: "date", nullable: true),
                    EndDate = table.Column<DateTime>(type: "date", nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Plans__755C22D76733A685", x => x.PlanID);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RoleId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false, defaultValueSql: "(newid())"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsActived = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "((1))"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "WorkStatus",
                columns: table => new
                {
                    StatusID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StatusName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__WorkStat__C8EE20433B62230C", x => x.StatusID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false, defaultValueSql: "(newid())"),
                    Username = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Email = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    Gender = table.Column<bool>(type: "bit", nullable: true),
                    Avatar = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoleId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    IsActived = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "((1))"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "((0))"),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Zalo = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Users_Roles",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleId");
                });

            migrationBuilder.CreateTable(
                name: "Factory",
                columns: table => new
                {
                    FacID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FacName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Alias = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Phone = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    Phone2 = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Factory__815081C831BD8E9C", x => x.FacID);
                    table.ForeignKey(
                        name: "FK_Factory_Users",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "Devices",
                columns: table => new
                {
                    DeviceID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeID = table.Column<int>(type: "int", nullable: false),
                    DeviceName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Code = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Color = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Descriptions = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    QRCode = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    FacID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Devices__49E1233103C7F49A", x => x.DeviceID);
                    table.ForeignKey(
                        name: "FK_Devices_DeviceTypes",
                        column: x => x.TypeID,
                        principalTable: "DeviceTypes",
                        principalColumn: "TypeID");
                    table.ForeignKey(
                        name: "fk_Device_Factory",
                        column: x => x.FacID,
                        principalTable: "Factory",
                        principalColumn: "FacID");
                });

            migrationBuilder.CreateTable(
                name: "DailyDivision",
                columns: table => new
                {
                    DeviceID = table.Column<int>(type: "int", nullable: false),
                    PlanID = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    WorkDay = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    EstimateFinishTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    TotalTime = table.Column<double>(type: "float", nullable: true),
                    SpecificContents = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false, defaultValueSql: "((1))"),
                    Reason = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    JobDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    BeforeImage = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    AfterImage = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    CompletedDate = table.Column<DateTime>(type: "date", nullable: true),
                    CheckedBy = table.Column<int>(type: "int", nullable: true),
                    StatusID = table.Column<int>(type: "int", nullable: false, defaultValueSql: "((4))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_DailyDivision", x => new { x.DeviceID, x.PlanID, x.UserId });
                    table.ForeignKey(
                        name: "FK_DailyDivision_Users",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "fk_DailyDivision_Device",
                        column: x => x.DeviceID,
                        principalTable: "Devices",
                        principalColumn: "DeviceID");
                    table.ForeignKey(
                        name: "fk_DailyDivision_Plan",
                        column: x => x.PlanID,
                        principalTable: "Plans",
                        principalColumn: "PlanID");
                    table.ForeignKey(
                        name: "fk_DailyDivision_WorkStatus",
                        column: x => x.StatusID,
                        principalTable: "WorkStatus",
                        principalColumn: "StatusID");
                });

            migrationBuilder.CreateTable(
                name: "DetailPlan",
                columns: table => new
                {
                    DeviceID = table.Column<int>(type: "int", nullable: false),
                    PlanID = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false, defaultValueSql: "((1))"),
                    Unit = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Specification = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ExpectedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Percents = table.Column<double>(type: "float", nullable: false),
                    TypePlan = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, defaultValueSql: "('PM')"),
                    StatusID = table.Column<int>(type: "int", nullable: false, defaultValueSql: "((4))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_DetailPlan", x => new { x.DeviceID, x.PlanID });
                    table.ForeignKey(
                        name: "fk_DetailPlan_Device",
                        column: x => x.DeviceID,
                        principalTable: "Devices",
                        principalColumn: "DeviceID");
                    table.ForeignKey(
                        name: "fk_DetailPlan_Plan",
                        column: x => x.PlanID,
                        principalTable: "Plans",
                        principalColumn: "PlanID");
                    table.ForeignKey(
                        name: "fk_DetailPlan_WorkStatus",
                        column: x => x.StatusID,
                        principalTable: "WorkStatus",
                        principalColumn: "StatusID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DailyDivision_PlanID",
                table: "DailyDivision",
                column: "PlanID");

            migrationBuilder.CreateIndex(
                name: "IX_DailyDivision_StatusID",
                table: "DailyDivision",
                column: "StatusID");

            migrationBuilder.CreateIndex(
                name: "IX_DailyDivision_UserId",
                table: "DailyDivision",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DetailPlan_PlanID",
                table: "DetailPlan",
                column: "PlanID");

            migrationBuilder.CreateIndex(
                name: "IX_DetailPlan_StatusID",
                table: "DetailPlan",
                column: "StatusID");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_FacID",
                table: "Devices",
                column: "FacID");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_TypeID",
                table: "Devices",
                column: "TypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Factory_UserId",
                table: "Factory",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username_Email",
                table: "Users",
                columns: new[] { "Username", "Email" },
                unique: true,
                filter: "([Email] IS NOT NULL)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DailyDivision");

            migrationBuilder.DropTable(
                name: "DetailPlan");

            migrationBuilder.DropTable(
                name: "Devices");

            migrationBuilder.DropTable(
                name: "Plans");

            migrationBuilder.DropTable(
                name: "WorkStatus");

            migrationBuilder.DropTable(
                name: "DeviceTypes");

            migrationBuilder.DropTable(
                name: "Factory");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
