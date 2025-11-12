using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace JobApplicationTracker.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JobApplications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CompanyName = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Position = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Status = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    DateApplied = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Notes = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true),
                    LastUpdatedOn = table.Column<DateTimeOffset>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobApplications", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "JobApplications",
                columns: new[] { "Id", "CompanyName", "DateApplied", "LastUpdatedOn", "Notes", "Position", "Status" },
                values: new object[,]
                {
                    { 1, "Google", new DateTime(2025, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTimeOffset(new DateTime(2025, 1, 15, 8, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Initial phone screen complete.", "Backend Engineer", "Shortlisted" },
                    { 2, "Apple", new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTimeOffset(new DateTime(2025, 2, 11, 10, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Waiting for recruiter feedback.", "Full Stack Developer", "InProcess" },
                    { 3, "Microsoft", new DateTime(2024, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTimeOffset(new DateTime(2025, 1, 2, 14, 15, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Offer expires next week.", "Site Reliability Engineer", "Offer" },
                    { 4, "Amazon", new DateTime(2025, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTimeOffset(new DateTime(2025, 3, 6, 9, 15, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Preparing technical assessment.", "Cloud Support Engineer", "Applied" },
                    { 5, "Meta", new DateTime(2025, 1, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTimeOffset(new DateTime(2025, 2, 5, 16, 45, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Panel interview scheduled for next week.", "Machine Learning Engineer", "Interview" },
                    { 6, "Netflix", new DateTime(2025, 2, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTimeOffset(new DateTime(2025, 2, 25, 11, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Security Engineer", "InProcess" },
                    { 7, "Salesforce", new DateTime(2025, 1, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTimeOffset(new DateTime(2025, 1, 25, 13, 30, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Awaiting hiring manager feedback.", "DevOps Engineer", "Shortlisted" },
                    { 8, "Atlassian", new DateTime(2024, 11, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTimeOffset(new DateTime(2024, 12, 10, 8, 20, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Received polite decline.", "Frontend Developer", "Declined" },
                    { 9, "Adobe", new DateTime(2025, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTimeOffset(new DateTime(2025, 3, 2, 10, 10, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Portfolio submitted.", "Product Designer", "Applied" },
                    { 10, "Shopify", new DateTime(2025, 2, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTimeOffset(new DateTime(2025, 2, 18, 15, 45, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "On-site scheduled.", "Infrastructure Engineer", "Interview" },
                    { 11, "Stripe", new DateTime(2024, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTimeOffset(new DateTime(2025, 1, 10, 17, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Reviewing offer details.", "Payments Engineer", "Offer" },
                    { 12, "Tesla", new DateTime(2025, 3, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTimeOffset(new DateTime(2025, 3, 13, 7, 50, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Automation Engineer", "Applied" },
                    { 13, "Airbnb", new DateTime(2024, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTimeOffset(new DateTime(2024, 11, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Role closed before interview.", "Platform Engineer", "PositionClosed" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobApplications_CompanyName_Position",
                table: "JobApplications",
                columns: new[] { "CompanyName", "Position" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JobApplications");
        }
    }
}
