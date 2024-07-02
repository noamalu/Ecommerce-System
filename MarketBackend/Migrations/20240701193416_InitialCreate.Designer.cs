﻿// <auto-generated />
using System;
using MarketBackend.DAL.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MarketBackend.Migrations
{
    [DbContext(typeof(DBcontext))]
    [Migration("20240701193416_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("MarketBackend.DAL.DTO.BasketDTO", b =>
                {
                    b.Property<int>("BasketId")
                        .HasColumnType("int");

                    b.Property<int>("CartId")
                        .HasColumnType("int");

                    b.Property<int?>("ShoppingCartHistoryDTOShoppingCartId")
                        .HasColumnType("int");

                    b.Property<int>("StoreId")
                        .HasColumnType("int");

                    b.Property<double>("TotalPrice")
                        .HasColumnType("float");

                    b.HasKey("BasketId");

                    b.HasIndex("CartId");

                    b.HasIndex("ShoppingCartHistoryDTOShoppingCartId");

                    b.HasIndex("StoreId");

                    b.ToTable("Baskets");
                });

            modelBuilder.Entity("MarketBackend.DAL.DTO.BasketItemDTO", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("BasketDTOBasketId")
                        .HasColumnType("int");

                    b.Property<double>("PriceAfterDiscount")
                        .HasColumnType("float");

                    b.Property<double>("PriceBeforeDiscount")
                        .HasColumnType("float");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BasketDTOBasketId");

                    b.HasIndex("ProductId");

                    b.ToTable("BasketItems");
                });

            modelBuilder.Entity("MarketBackend.DAL.DTO.EventDTO", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ListenerId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("StoreId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ListenerId");

                    b.HasIndex("StoreId");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("MarketBackend.DAL.DTO.MemberDTO", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<bool>("IsNotification")
                        .HasColumnType("bit");

                    b.Property<bool>("IsSystemAdmin")
                        .HasColumnType("bit");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("RoleDTOstoreId")
                        .HasColumnType("int");

                    b.Property<string>("RoleDTOuserName")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("ShoppingCart_shoppingCartId")
                        .HasColumnType("int");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ShoppingCart_shoppingCartId");

                    b.HasIndex("RoleDTOstoreId", "RoleDTOuserName");

                    b.ToTable("Members");
                });

            modelBuilder.Entity("MarketBackend.DAL.DTO.MessageDTO", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("MemberDTOId")
                        .HasColumnType("int");

                    b.Property<string>("MessageContent")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Seen")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("MemberDTOId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("MarketBackend.DAL.DTO.PolicyDTO", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<int?>("DiscountCompositePolicyDTOId")
                        .HasColumnType("int");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasMaxLength(34)
                        .HasColumnType("nvarchar(34)");

                    b.Property<DateTime>("ExpirationDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("PolicySubjectId")
                        .HasColumnType("int");

                    b.Property<int>("RuleId")
                        .HasColumnType("int");

                    b.Property<int?>("StoreDTOId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DiscountCompositePolicyDTOId");

                    b.HasIndex("PolicySubjectId");

                    b.HasIndex("StoreDTOId");

                    b.ToTable("Policies");

                    b.HasDiscriminator<string>("Discriminator").HasValue("PolicyDTO");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("MarketBackend.DAL.DTO.PolicySubjectDTO", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("Policy Subjects");
                });

            modelBuilder.Entity("MarketBackend.DAL.DTO.ProductDTO", b =>
                {
                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int?>("BasketDTOBasketId")
                        .HasColumnType("int");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Keywords")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<double>("ProductRating")
                        .HasColumnType("float");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<string>("SellMethod")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ShoppingCartHistoryDTOShoppingCartId")
                        .HasColumnType("int");

                    b.Property<int?>("StoreDTOId")
                        .HasColumnType("int");

                    b.HasKey("ProductId");

                    b.HasIndex("BasketDTOBasketId");

                    b.HasIndex("ShoppingCartHistoryDTOShoppingCartId");

                    b.HasIndex("StoreDTOId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("MarketBackend.DAL.DTO.PurchaseDTO", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<int>("Baskets")
                        .HasColumnType("int");

                    b.Property<string>("Identifierr")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<int?>("StoreDTOId")
                        .HasColumnType("int");

                    b.Property<int>("StoreId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Baskets")
                        .IsUnique();

                    b.HasIndex("StoreDTOId");

                    b.ToTable("Purchases");
                });

            modelBuilder.Entity("MarketBackend.DAL.DTO.RoleDTO", b =>
                {
                    b.Property<int>("storeId")
                        .HasColumnType("int")
                        .HasColumnOrder(0);

                    b.Property<string>("userName")
                        .HasColumnType("nvarchar(450)")
                        .HasColumnOrder(1);

                    b.Property<int?>("appointerId")
                        .HasColumnType("int");

                    b.HasKey("storeId", "userName");

                    b.HasIndex("appointerId");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("MarketBackend.DAL.DTO.RuleDTO", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<int?>("CompositeRuleDTOId")
                        .HasColumnType("int");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasMaxLength(21)
                        .HasColumnType("nvarchar(21)");

                    b.Property<int?>("StoreDTOId")
                        .HasColumnType("int");

                    b.Property<int>("SubjectId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CompositeRuleDTOId");

                    b.HasIndex("StoreDTOId");

                    b.HasIndex("SubjectId");

                    b.ToTable("Rules");

                    b.HasDiscriminator<string>("Discriminator").HasValue("RuleDTO");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("MarketBackend.DAL.DTO.RuleSubjectDTO", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("Rule Subjects");
                });

            modelBuilder.Entity("MarketBackend.DAL.DTO.ShoppingCartDTO", b =>
                {
                    b.Property<int>("_shoppingCartId")
                        .HasColumnType("int");

                    b.HasKey("_shoppingCartId");

                    b.ToTable("ShoppingCart");
                });

            modelBuilder.Entity("MarketBackend.DAL.DTO.ShoppingCartHistoryDTO", b =>
                {
                    b.Property<int>("ShoppingCartId")
                        .HasColumnType("int");

                    b.Property<int?>("MemberDTOId")
                        .HasColumnType("int");

                    b.HasKey("ShoppingCartId");

                    b.HasIndex("MemberDTOId");

                    b.ToTable("ShoppingCartHistory");
                });

            modelBuilder.Entity("MarketBackend.DAL.DTO.StoreDTO", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNum")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Rating")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("Stores");
                });

            modelBuilder.Entity("MarketBackend.DAL.DTO.DiscountPolicyDTO", b =>
                {
                    b.HasBaseType("MarketBackend.DAL.DTO.PolicyDTO");

                    b.Property<double>("Precentage")
                        .HasColumnType("float");

                    b.ToTable("Policies");

                    b.HasDiscriminator().HasValue("DiscountPolicy");
                });

            modelBuilder.Entity("MarketBackend.DAL.DTO.PurchasePolicyDTO", b =>
                {
                    b.HasBaseType("MarketBackend.DAL.DTO.PolicyDTO");

                    b.ToTable("Policies");

                    b.HasDiscriminator().HasValue("PurchasePolicy");
                });

            modelBuilder.Entity("MarketBackend.DAL.DTO.CompositeRuleDTO", b =>
                {
                    b.HasBaseType("MarketBackend.DAL.DTO.RuleDTO");

                    b.Property<string>("Operator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.ToTable("Rules");

                    b.HasDiscriminator().HasValue("CompositeRule");
                });

            modelBuilder.Entity("MarketBackend.DAL.DTO.QuantityRuleDTO", b =>
                {
                    b.HasBaseType("MarketBackend.DAL.DTO.RuleDTO");

                    b.Property<int>("MaxQuantity")
                        .HasColumnType("int");

                    b.Property<int>("MinQuantity")
                        .HasColumnType("int");

                    b.ToTable("Rules");

                    b.HasDiscriminator().HasValue("QuantityRule");
                });

            modelBuilder.Entity("MarketBackend.DAL.DTO.SimpleRuleDTO", b =>
                {
                    b.HasBaseType("MarketBackend.DAL.DTO.RuleDTO");

                    b.ToTable("Rules");

                    b.HasDiscriminator().HasValue("SimpleRule");
                });

            modelBuilder.Entity("MarketBackend.DAL.DTO.TotalPriceRuleDTO", b =>
                {
                    b.HasBaseType("MarketBackend.DAL.DTO.RuleDTO");

                    b.Property<double>("TotalPrice")
                        .HasColumnType("float");

                    b.ToTable("Rules");

                    b.HasDiscriminator().HasValue("TotalPriceRule");
                });

            modelBuilder.Entity("MarketBackend.DAL.DTO.DiscountCompositePolicyDTO", b =>
                {
                    b.HasBaseType("MarketBackend.DAL.DTO.DiscountPolicyDTO");

                    b.Property<string>("NumericOperator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.ToTable("Policies");

                    b.HasDiscriminator().HasValue("CompositeDiscountPolicy");
                });

            modelBuilder.Entity("MarketBackend.DAL.DTO.BasketDTO", b =>
                {
                    b.HasOne("MarketBackend.DAL.DTO.ShoppingCartDTO", null)
                        .WithMany("Baskets")
                        .HasForeignKey("CartId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("MarketBackend.DAL.DTO.ShoppingCartHistoryDTO", null)
                        .WithMany("_baskets")
                        .HasForeignKey("ShoppingCartHistoryDTOShoppingCartId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MarketBackend.DAL.DTO.StoreDTO", null)
                        .WithMany()
                        .HasForeignKey("StoreId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });

            modelBuilder.Entity("MarketBackend.DAL.DTO.BasketItemDTO", b =>
                {
                    b.HasOne("MarketBackend.DAL.DTO.BasketDTO", null)
                        .WithMany("BasketItems")
                        .HasForeignKey("BasketDTOBasketId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MarketBackend.DAL.DTO.ProductDTO", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("MarketBackend.DAL.DTO.EventDTO", b =>
                {
                    b.HasOne("MarketBackend.DAL.DTO.MemberDTO", "Listener")
                        .WithMany()
                        .HasForeignKey("ListenerId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("MarketBackend.DAL.DTO.StoreDTO", null)
                        .WithMany()
                        .HasForeignKey("StoreId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Listener");
                });

            modelBuilder.Entity("MarketBackend.DAL.DTO.MemberDTO", b =>
                {
                    b.HasOne("MarketBackend.DAL.DTO.ShoppingCartDTO", "ShoppingCart")
                        .WithMany()
                        .HasForeignKey("ShoppingCart_shoppingCartId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MarketBackend.DAL.DTO.RoleDTO", null)
                        .WithMany("appointees")
                        .HasForeignKey("RoleDTOstoreId", "RoleDTOuserName")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("ShoppingCart");
                });

            modelBuilder.Entity("MarketBackend.DAL.DTO.MessageDTO", b =>
                {
                    b.HasOne("MarketBackend.DAL.DTO.MemberDTO", null)
                        .WithMany("Alerts")
                        .HasForeignKey("MemberDTOId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MarketBackend.DAL.DTO.PolicyDTO", b =>
                {
                    b.HasOne("MarketBackend.DAL.DTO.DiscountCompositePolicyDTO", null)
                        .WithMany("Policies")
                        .HasForeignKey("DiscountCompositePolicyDTOId");

                    b.HasOne("MarketBackend.DAL.DTO.PolicySubjectDTO", "PolicySubject")
                        .WithMany()
                        .HasForeignKey("PolicySubjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MarketBackend.DAL.DTO.StoreDTO", null)
                        .WithMany("Policies")
                        .HasForeignKey("StoreDTOId");

                    b.Navigation("PolicySubject");
                });

            modelBuilder.Entity("MarketBackend.DAL.DTO.PolicySubjectDTO", b =>
                {
                    b.HasOne("MarketBackend.DAL.DTO.ProductDTO", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("MarketBackend.DAL.DTO.ProductDTO", b =>
                {
                    b.HasOne("MarketBackend.DAL.DTO.BasketDTO", null)
                        .WithMany("Products")
                        .HasForeignKey("BasketDTOBasketId");

                    b.HasOne("MarketBackend.DAL.DTO.ShoppingCartHistoryDTO", null)
                        .WithMany("_products")
                        .HasForeignKey("ShoppingCartHistoryDTOShoppingCartId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MarketBackend.DAL.DTO.StoreDTO", null)
                        .WithMany("Products")
                        .HasForeignKey("StoreDTOId");
                });

            modelBuilder.Entity("MarketBackend.DAL.DTO.PurchaseDTO", b =>
                {
                    b.HasOne("MarketBackend.DAL.DTO.BasketDTO", "Basket")
                        .WithOne()
                        .HasForeignKey("MarketBackend.DAL.DTO.PurchaseDTO", "Baskets")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MarketBackend.DAL.DTO.StoreDTO", null)
                        .WithMany("Purchases")
                        .HasForeignKey("StoreDTOId");

                    b.Navigation("Basket");
                });

            modelBuilder.Entity("MarketBackend.DAL.DTO.RoleDTO", b =>
                {
                    b.HasOne("MarketBackend.DAL.DTO.MemberDTO", "appointer")
                        .WithMany()
                        .HasForeignKey("appointerId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("MarketBackend.DAL.DTO.StoreDTO", null)
                        .WithMany()
                        .HasForeignKey("storeId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("appointer");
                });

            modelBuilder.Entity("MarketBackend.DAL.DTO.RuleDTO", b =>
                {
                    b.HasOne("MarketBackend.DAL.DTO.CompositeRuleDTO", null)
                        .WithMany("Rules")
                        .HasForeignKey("CompositeRuleDTOId");

                    b.HasOne("MarketBackend.DAL.DTO.StoreDTO", null)
                        .WithMany("Rules")
                        .HasForeignKey("StoreDTOId");

                    b.HasOne("MarketBackend.DAL.DTO.RuleSubjectDTO", "Subject")
                        .WithMany()
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Subject");
                });

            modelBuilder.Entity("MarketBackend.DAL.DTO.RuleSubjectDTO", b =>
                {
                    b.HasOne("MarketBackend.DAL.DTO.ProductDTO", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("MarketBackend.DAL.DTO.ShoppingCartHistoryDTO", b =>
                {
                    b.HasOne("MarketBackend.DAL.DTO.MemberDTO", null)
                        .WithMany("OrderHistory")
                        .HasForeignKey("MemberDTOId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MarketBackend.DAL.DTO.BasketDTO", b =>
                {
                    b.Navigation("BasketItems");

                    b.Navigation("Products");
                });

            modelBuilder.Entity("MarketBackend.DAL.DTO.MemberDTO", b =>
                {
                    b.Navigation("Alerts");

                    b.Navigation("OrderHistory");
                });

            modelBuilder.Entity("MarketBackend.DAL.DTO.RoleDTO", b =>
                {
                    b.Navigation("appointees");
                });

            modelBuilder.Entity("MarketBackend.DAL.DTO.ShoppingCartDTO", b =>
                {
                    b.Navigation("Baskets");
                });

            modelBuilder.Entity("MarketBackend.DAL.DTO.ShoppingCartHistoryDTO", b =>
                {
                    b.Navigation("_baskets");

                    b.Navigation("_products");
                });

            modelBuilder.Entity("MarketBackend.DAL.DTO.StoreDTO", b =>
                {
                    b.Navigation("Policies");

                    b.Navigation("Products");

                    b.Navigation("Purchases");

                    b.Navigation("Rules");
                });

            modelBuilder.Entity("MarketBackend.DAL.DTO.CompositeRuleDTO", b =>
                {
                    b.Navigation("Rules");
                });

            modelBuilder.Entity("MarketBackend.DAL.DTO.DiscountCompositePolicyDTO", b =>
                {
                    b.Navigation("Policies");
                });
#pragma warning restore 612, 618
        }
    }
}