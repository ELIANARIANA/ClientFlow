using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ClientFlow.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class _20261027 : Migration
    {
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			// Remove the old UUID ID and create a new Identity ID.
			migrationBuilder.DropPrimaryKey(
				name: "PK_Users",
				table: "Users");

			migrationBuilder.DropColumn(
				name: "Id",
				table: "Users");

			migrationBuilder.AddColumn<long>(
				name: "Id",
				table: "Users",
				type: "bigint",
				nullable: false)
				.Annotation(
					"Npgsql:ValueGenerationStrategy",
					NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

			migrationBuilder.AddPrimaryKey(
				name: "PK_Users",
				table: "Users",
				column: "Id");

			// Customers
			migrationBuilder.DropPrimaryKey(
				name: "PK_Customers",
				table: "Customers");

			migrationBuilder.DropColumn(
				name: "Id",
				table: "Customers");

			migrationBuilder.AddColumn<long>(
				name: "Id",
				table: "Customers",
				type: "bigint",
				nullable: false)
				.Annotation(
					"Npgsql:ValueGenerationStrategy",
					NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

			migrationBuilder.AddPrimaryKey(
				name: "PK_Customers",
				table: "Customers",
				column: "Id");

			// UUID columns remain UUID
			migrationBuilder.AddColumn<Guid>(
				name: "UserId",
				table: "Users",
				type: "uuid",
				nullable: false,
				defaultValue: Guid.Empty);

			migrationBuilder.AddColumn<Guid>(
				name: "CustomerId",
				table: "Customers",
				type: "uuid",
				nullable: false,
				defaultValue: Guid.Empty);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn("UserId", "Users");
			migrationBuilder.DropColumn("CustomerId", "Customers");

			migrationBuilder.DropPrimaryKey("PK_Users", "Users");
			migrationBuilder.DropColumn("Id", "Users");

			migrationBuilder.AddColumn<Guid>(
				name: "Id",
				table: "Users",
				type: "uuid",
				nullable: false,
				defaultValue: Guid.Empty);

			migrationBuilder.AddPrimaryKey("PK_Users", "Users", "Id");

			migrationBuilder.DropPrimaryKey("PK_Customers", "Customers");
			migrationBuilder.DropColumn("Id", "Customers");

			migrationBuilder.AddColumn<Guid>(
				name: "Id",
				table: "Customers",
				type: "uuid",
				nullable: false,
				defaultValue: Guid.Empty);

			migrationBuilder.AddPrimaryKey("PK_Customers", "Customers", "Id");
		}
	}
}
