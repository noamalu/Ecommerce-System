using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarketBackend.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ShoppingCart",
                columns: table => new
                {
                    _shoppingCartId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingCart", x => x._shoppingCartId);
                });

            migrationBuilder.CreateTable(
                name: "Stores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNum = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Rating = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BasketItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    PriceBeforeDiscount = table.Column<double>(type: "float", nullable: false),
                    PriceAfterDiscount = table.Column<double>(type: "float", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    BasketDTOBasketId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BasketItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Baskets",
                columns: table => new
                {
                    BasketId = table.Column<int>(type: "int", nullable: false),
                    StoreId = table.Column<int>(type: "int", nullable: false),
                    CartId = table.Column<int>(type: "int", nullable: false),
                    TotalPrice = table.Column<double>(type: "float", nullable: false),
                    ShoppingCartHistoryDTOShoppingCartId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Baskets", x => x.BasketId);
                    table.ForeignKey(
                        name: "FK_Baskets_ShoppingCart_CartId",
                        column: x => x.CartId,
                        principalTable: "ShoppingCart",
                        principalColumn: "_shoppingCartId");
                    table.ForeignKey(
                        name: "FK_Baskets_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Purchases",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    StoreId = table.Column<int>(type: "int", nullable: false),
                    Baskets = table.Column<int>(type: "int", nullable: false),
                    Identifierr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    StoreDTOId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Purchases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Purchases_Baskets_Baskets",
                        column: x => x.Baskets,
                        principalTable: "Baskets",
                        principalColumn: "BasketId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Purchases_Stores_StoreDTOId",
                        column: x => x.StoreDTOId,
                        principalTable: "Stores",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StoreId = table.Column<int>(type: "int", nullable: false),
                    ListenerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Events_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Members",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    ShoppingCart_shoppingCartId = table.Column<int>(type: "int", nullable: false),
                    IsSystemAdmin = table.Column<bool>(type: "bit", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsNotification = table.Column<bool>(type: "bit", nullable: false),
                    RoleDTOstoreId = table.Column<int>(type: "int", nullable: true),
                    RoleDTOuserName = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Members", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Members_ShoppingCart_ShoppingCart_shoppingCartId",
                        column: x => x.ShoppingCart_shoppingCartId,
                        principalTable: "ShoppingCart",
                        principalColumn: "_shoppingCartId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MessageContent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Seen = table.Column<bool>(type: "bit", nullable: false),
                    MemberDTOId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_Members_MemberDTOId",
                        column: x => x.MemberDTOId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    storeId = table.Column<int>(type: "int", nullable: false),
                    userName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    appointerId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => new { x.storeId, x.userName });
                    table.ForeignKey(
                        name: "FK_Roles_Members_appointerId",
                        column: x => x.appointerId,
                        principalTable: "Members",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Roles_Stores_storeId",
                        column: x => x.storeId,
                        principalTable: "Stores",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ShoppingCartHistory",
                columns: table => new
                {
                    ShoppingCartId = table.Column<int>(type: "int", nullable: false),
                    MemberDTOId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingCartHistory", x => x.ShoppingCartId);
                    table.ForeignKey(
                        name: "FK_ShoppingCartHistory_Members_MemberDTOId",
                        column: x => x.MemberDTOId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Keywords = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SellMethod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductRating = table.Column<double>(type: "float", nullable: false),
                    BasketDTOBasketId = table.Column<int>(type: "int", nullable: true),
                    ShoppingCartHistoryDTOShoppingCartId = table.Column<int>(type: "int", nullable: true),
                    StoreDTOId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductId);
                    table.ForeignKey(
                        name: "FK_Products_Baskets_BasketDTOBasketId",
                        column: x => x.BasketDTOBasketId,
                        principalTable: "Baskets",
                        principalColumn: "BasketId");
                    table.ForeignKey(
                        name: "FK_Products_ShoppingCartHistory_ShoppingCartHistoryDTOShoppingCartId",
                        column: x => x.ShoppingCartHistoryDTOShoppingCartId,
                        principalTable: "ShoppingCartHistory",
                        principalColumn: "ShoppingCartId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Products_Stores_StoreDTOId",
                        column: x => x.StoreDTOId,
                        principalTable: "Stores",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Policy Subjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Policy Subjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Policy Subjects_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rule Subjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rule Subjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rule Subjects_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Policies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RuleId = table.Column<int>(type: "int", nullable: false),
                    PolicySubjectId = table.Column<int>(type: "int", nullable: false),
                    DiscountCompositePolicyDTOId = table.Column<int>(type: "int", nullable: true),
                    Discriminator = table.Column<string>(type: "nvarchar(34)", maxLength: 34, nullable: false),
                    StoreDTOId = table.Column<int>(type: "int", nullable: true),
                    Precentage = table.Column<double>(type: "float", nullable: true),
                    NumericOperator = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Policies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Policies_Policies_DiscountCompositePolicyDTOId",
                        column: x => x.DiscountCompositePolicyDTOId,
                        principalTable: "Policies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Policies_Policy Subjects_PolicySubjectId",
                        column: x => x.PolicySubjectId,
                        principalTable: "Policy Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Policies_Stores_StoreDTOId",
                        column: x => x.StoreDTOId,
                        principalTable: "Stores",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Rules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    SubjectId = table.Column<int>(type: "int", nullable: false),
                    CompositeRuleDTOId = table.Column<int>(type: "int", nullable: true),
                    Discriminator = table.Column<string>(type: "nvarchar(21)", maxLength: 21, nullable: false),
                    StoreDTOId = table.Column<int>(type: "int", nullable: true),
                    Operator = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MinQuantity = table.Column<int>(type: "int", nullable: true),
                    MaxQuantity = table.Column<int>(type: "int", nullable: true),
                    TotalPrice = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rules_Rule Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Rule Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rules_Rules_CompositeRuleDTOId",
                        column: x => x.CompositeRuleDTOId,
                        principalTable: "Rules",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Rules_Stores_StoreDTOId",
                        column: x => x.StoreDTOId,
                        principalTable: "Stores",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_BasketItems_BasketDTOBasketId",
                table: "BasketItems",
                column: "BasketDTOBasketId");

            migrationBuilder.CreateIndex(
                name: "IX_BasketItems_ProductId",
                table: "BasketItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Baskets_CartId",
                table: "Baskets",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "IX_Baskets_ShoppingCartHistoryDTOShoppingCartId",
                table: "Baskets",
                column: "ShoppingCartHistoryDTOShoppingCartId");

            migrationBuilder.CreateIndex(
                name: "IX_Baskets_StoreId",
                table: "Baskets",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_ListenerId",
                table: "Events",
                column: "ListenerId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_StoreId",
                table: "Events",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_Members_RoleDTOstoreId_RoleDTOuserName",
                table: "Members",
                columns: new[] { "RoleDTOstoreId", "RoleDTOuserName" });

            migrationBuilder.CreateIndex(
                name: "IX_Members_ShoppingCart_shoppingCartId",
                table: "Members",
                column: "ShoppingCart_shoppingCartId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_MemberDTOId",
                table: "Messages",
                column: "MemberDTOId");

            migrationBuilder.CreateIndex(
                name: "IX_Policies_DiscountCompositePolicyDTOId",
                table: "Policies",
                column: "DiscountCompositePolicyDTOId");

            migrationBuilder.CreateIndex(
                name: "IX_Policies_PolicySubjectId",
                table: "Policies",
                column: "PolicySubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Policies_StoreDTOId",
                table: "Policies",
                column: "StoreDTOId");

            migrationBuilder.CreateIndex(
                name: "IX_Policy Subjects_ProductId",
                table: "Policy Subjects",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_BasketDTOBasketId",
                table: "Products",
                column: "BasketDTOBasketId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ShoppingCartHistoryDTOShoppingCartId",
                table: "Products",
                column: "ShoppingCartHistoryDTOShoppingCartId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_StoreDTOId",
                table: "Products",
                column: "StoreDTOId");

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_Baskets",
                table: "Purchases",
                column: "Baskets",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_StoreDTOId",
                table: "Purchases",
                column: "StoreDTOId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_appointerId",
                table: "Roles",
                column: "appointerId");

            migrationBuilder.CreateIndex(
                name: "IX_Rule Subjects_ProductId",
                table: "Rule Subjects",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Rules_CompositeRuleDTOId",
                table: "Rules",
                column: "CompositeRuleDTOId");

            migrationBuilder.CreateIndex(
                name: "IX_Rules_StoreDTOId",
                table: "Rules",
                column: "StoreDTOId");

            migrationBuilder.CreateIndex(
                name: "IX_Rules_SubjectId",
                table: "Rules",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCartHistory_MemberDTOId",
                table: "ShoppingCartHistory",
                column: "MemberDTOId");

            migrationBuilder.AddForeignKey(
                name: "FK_BasketItems_Baskets_BasketDTOBasketId",
                table: "BasketItems",
                column: "BasketDTOBasketId",
                principalTable: "Baskets",
                principalColumn: "BasketId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BasketItems_Products_ProductId",
                table: "BasketItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Baskets_ShoppingCartHistory_ShoppingCartHistoryDTOShoppingCartId",
                table: "Baskets",
                column: "ShoppingCartHistoryDTOShoppingCartId",
                principalTable: "ShoppingCartHistory",
                principalColumn: "ShoppingCartId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Members_ListenerId",
                table: "Events",
                column: "ListenerId",
                principalTable: "Members",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Members_Roles_RoleDTOstoreId_RoleDTOuserName",
                table: "Members",
                columns: new[] { "RoleDTOstoreId", "RoleDTOuserName" },
                principalTable: "Roles",
                principalColumns: new[] { "storeId", "userName" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Members_ShoppingCart_ShoppingCart_shoppingCartId",
                table: "Members");

            migrationBuilder.DropForeignKey(
                name: "FK_Roles_Stores_storeId",
                table: "Roles");

            migrationBuilder.DropForeignKey(
                name: "FK_Roles_Members_appointerId",
                table: "Roles");

            migrationBuilder.DropTable(
                name: "BasketItems");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "Policies");

            migrationBuilder.DropTable(
                name: "Purchases");

            migrationBuilder.DropTable(
                name: "Rules");

            migrationBuilder.DropTable(
                name: "Policy Subjects");

            migrationBuilder.DropTable(
                name: "Rule Subjects");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Baskets");

            migrationBuilder.DropTable(
                name: "ShoppingCartHistory");

            migrationBuilder.DropTable(
                name: "ShoppingCart");

            migrationBuilder.DropTable(
                name: "Stores");

            migrationBuilder.DropTable(
                name: "Members");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
