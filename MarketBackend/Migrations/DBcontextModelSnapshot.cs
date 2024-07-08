﻿// <auto-generated />
using System;
using MarketBackend.DAL.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MarketBackend.Migrations
{
    [DbContext(typeof(DBcontext))]
    partial class DBcontextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

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
                        .HasColumnType("double");

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

                    b.Property<int?>("BasketDTOBasketId")
                        .HasColumnType("int");

                    b.Property<double>("PriceAfterDiscount")
                        .HasColumnType("double");

                    b.Property<double>("PriceBeforeDiscount")
                        .HasColumnType("double");

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

                    b.Property<int>("ListenerId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

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
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsSystemAdmin")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("ShoppingCart_shoppingCartId")
                        .HasColumnType("int");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("ShoppingCart_shoppingCartId");

                    b.ToTable("Members");
                });

            modelBuilder.Entity("MarketBackend.DAL.DTO.MessageDTO", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int?>("MemberDTOId")
                        .HasColumnType("int");

                    b.Property<string>("MessageContent")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("Seen")
                        .HasColumnType("tinyint(1)");

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
                        .HasColumnType("varchar(34)");

                    b.Property<DateTime>("ExpirationDate")
                        .HasColumnType("datetime(6)");

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

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("longtext");

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
                        .HasColumnType("longtext");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Keywords")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<double>("Price")
                        .HasColumnType("double");

                    b.Property<double>("ProductRating")
                        .HasColumnType("double");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<string>("SellMethod")
                        .IsRequired()
                        .HasColumnType("longtext");

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

                    b.Property<int>("BasketId")
                        .HasColumnType("int");

                    b.Property<string>("Identifierr")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<double>("Price")
                        .HasColumnType("double");

                    b.Property<int?>("StoreDTOId")
                        .HasColumnType("int");

                    b.Property<int>("StoreId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("StoreDTOId");

                    b.ToTable("Purchases");
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
                        .HasColumnType("varchar(21)");

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

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("longtext");

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
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("PhoneNum")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<double>("Rating")
                        .HasColumnType("double");

                    b.HasKey("Id");

                    b.ToTable("Stores");
                });

            modelBuilder.Entity("MarketBackend.DAL.DTO.DiscountPolicyDTO", b =>
                {
                    b.HasBaseType("MarketBackend.DAL.DTO.PolicyDTO");

                    b.Property<double>("Precentage")
                        .HasColumnType("double");

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
                        .HasColumnType("longtext");

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
                        .HasColumnType("double");

                    b.ToTable("Rules");

                    b.HasDiscriminator().HasValue("TotalPriceRule");
                });

            modelBuilder.Entity("MarketBackend.DAL.DTO.DiscountCompositePolicyDTO", b =>
                {
                    b.HasBaseType("MarketBackend.DAL.DTO.DiscountPolicyDTO");

                    b.Property<string>("NumericOperator")
                        .IsRequired()
                        .HasColumnType("longtext");

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
                    b.HasOne("MarketBackend.DAL.DTO.StoreDTO", null)
                        .WithMany("Purchases")
                        .HasForeignKey("StoreDTOId");
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
