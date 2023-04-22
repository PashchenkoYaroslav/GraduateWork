using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuickToken.Database.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "account",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    roles = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    login = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    password = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    createdat = table.Column<long>(name: "created_at", type: "INTEGER", nullable: false),
                    lastauthat = table.Column<long>(name: "last_auth_at", type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_account", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "asset_serial",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    price = table.Column<long>(type: "INTEGER", nullable: false),
                    dailyinterestrate = table.Column<double>(name: "daily_interest_rate", type: "REAL", nullable: false),
                    ipotimestamp = table.Column<long>(name: "ipo_timestamp", type: "INTEGER", nullable: false),
                    burntimestamp = table.Column<long>(name: "burn_timestamp", type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_asset_serial", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "blockchain_transaction",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    inputpayload = table.Column<string>(name: "input_payload", type: "TEXT", maxLength: 5000, nullable: false),
                    outputpayload = table.Column<string>(name: "output_payload", type: "TEXT", maxLength: 5000, nullable: true),
                    state = table.Column<int>(type: "INTEGER", nullable: false),
                    hash = table.Column<string>(type: "TEXT", nullable: true),
                    createdat = table.Column<long>(name: "created_at", type: "INTEGER", nullable: false),
                    lastupdateat = table.Column<long>(name: "last_update_at", type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_blockchain_transaction", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "wallet",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    address = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    eth = table.Column<string>(type: "TEXT", nullable: true),
                    currency = table.Column<string>(type: "TEXT", nullable: true),
                    accountid = table.Column<Guid>(name: "account_id", type: "TEXT", nullable: true),
                    lastupdateat = table.Column<long>(name: "last_update_at", type: "INTEGER", nullable: true),
                    forcecacheupdate = table.Column<bool>(name: "force_cache_update", type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_wallet", x => x.id);
                    table.ForeignKey(
                        name: "FK_wallet_account_account_id",
                        column: x => x.accountid,
                        principalTable: "account",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "asset",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    tokenid = table.Column<Guid>(name: "token_id", type: "TEXT", nullable: false),
                    walletid = table.Column<Guid>(name: "wallet_id", type: "TEXT", nullable: true),
                    assetserialid = table.Column<Guid>(name: "asset_serial_id", type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_asset", x => x.id);
                    table.ForeignKey(
                        name: "FK_asset_asset_serial_asset_serial_id",
                        column: x => x.assetserialid,
                        principalTable: "asset_serial",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_asset_wallet_wallet_id",
                        column: x => x.walletid,
                        principalTable: "wallet",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "wallet_snapshot",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    timestamp = table.Column<long>(type: "INTEGER", nullable: false),
                    eth = table.Column<string>(type: "TEXT", nullable: true),
                    currency = table.Column<string>(type: "TEXT", nullable: true),
                    WalletId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_wallet_snapshot", x => x.id);
                    table.ForeignKey(
                        name: "FK_wallet_snapshot_wallet_WalletId",
                        column: x => x.WalletId,
                        principalTable: "wallet",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "wallet",
                columns: new[] { "id", "account_id", "address", "currency", "eth", "force_cache_update", "last_update_at" },
                values: new object[] { new Guid("10ababc1-36d1-4708-8b0b-a74c8decf66f"), null, "0xca63bA88e0c6711A3D9177982ea7558d97bA8fBC", null, null, false, null });

            migrationBuilder.CreateIndex(
                name: "IX_account_login",
                table: "account",
                column: "login",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_asset_asset_serial_id",
                table: "asset",
                column: "asset_serial_id");

            migrationBuilder.CreateIndex(
                name: "IX_asset_token_id",
                table: "asset",
                column: "token_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_asset_wallet_id",
                table: "asset",
                column: "wallet_id");

            migrationBuilder.CreateIndex(
                name: "IX_blockchain_transaction_state",
                table: "blockchain_transaction",
                column: "state");

            migrationBuilder.CreateIndex(
                name: "IX_wallet_account_id",
                table: "wallet",
                column: "account_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_wallet_address",
                table: "wallet",
                column: "address",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_wallet_force_cache_update",
                table: "wallet",
                column: "force_cache_update");

            migrationBuilder.CreateIndex(
                name: "IX_wallet_last_update_at",
                table: "wallet",
                column: "last_update_at");

            migrationBuilder.CreateIndex(
                name: "IX_wallet_snapshot_WalletId_timestamp",
                table: "wallet_snapshot",
                columns: new[] { "WalletId", "timestamp" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "asset");

            migrationBuilder.DropTable(
                name: "blockchain_transaction");

            migrationBuilder.DropTable(
                name: "wallet_snapshot");

            migrationBuilder.DropTable(
                name: "asset_serial");

            migrationBuilder.DropTable(
                name: "wallet");

            migrationBuilder.DropTable(
                name: "account");
        }
    }
}
