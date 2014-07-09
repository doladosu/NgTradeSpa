using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace NgTrade.Models.Data
{
    public class UsersContext : DbContext
    {
        public UsersContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Companyprofile> Companyprofiles { get; set; }
        public DbSet<News> NewsList { get; set; }
        public DbSet<Quote> Quotes { get; set; }
        public DbSet<Holding> Holdings { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<MailingList> MailingLists { get; set; }
        public DbSet<AccountProfile> AccountProfiles { get; set; }
        public DbSet<Dailypricelist> Dailypricelists { get; set; }

    }

    [Table("UserProfile")]
    public class UserProfile
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BankName { get; set; }
        public int? AccountNumber { get; set; }
        public int? RoutingNumber { get; set; }
        public string AccountType { get; set; }
        public bool? Verified { get; set; }
        public bool? BankVerified { get; set; }
        public DateTime? SignupDate { get; set; }
        public string Occupation { get; set; }
        public DateTime BirthDate { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Email { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public decimal? Balance { get; set; }
    }

    [Table("Companyprofile")]
    public class Companyprofile
    {
        [Key]
        public string Symbol { get; set; }

        public string SymbolName { get; set; }
        public string SectorCode { get; set; }
        public string RegCode { get; set; }
        public string IsnCode { get; set; }
        public DateTime? ListingDate { get; set; }
        public string Category { get; set; }
        public DateTime? AccountYearEnd { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public string MAddress1 { get; set; }
        public string MAddress2 { get; set; }
        public string MCity { get; set; }
        public string MState { get; set; }
        public string MCountry { get; set; }
        public string MPostalCode { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Phone3 { get; set; }
        public string Fax { get; set; }
        public string Telex { get; set; }
        public string Email { get; set; }
        public string Secretary { get; set; }
        public string Website { get; set; }
        public string CInfo { get; set; }
    }

    [Table("News")]
    public class News
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PostId { get; set; }

        public string Subject { get; set; }
        public string Text { get; set; }
        public DateTime? PostDate { get; set; }
    }

    [Table("Quote")]
    public class Quote
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int QuoteId { get; set; }

        public DateTime Date { get; set; }
        public decimal Low { get; set; }
        public decimal Open { get; set; }
        public int Volume { get; set; }
        public decimal Close { get; set; }
        public decimal High { get; set; }
        public string Symbol { get; set; }
        public decimal Change1 { get; set; }
        public int Trades { get; set; }
    }

    [Table("Holding")]
    public class Holding
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public DateTime? DatePurchased { get; set; }
        public int AccountId { get; set; }
        public string Symbol { get; set; }
    }

    [Table("Order")]
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id  { get; set; }

        public string Type { get; set; }
        public string Status { get; set; }
        public DateTime? CompletionDate { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public DateTime OpenDate { get; set; }
        public int AccountId { get; set; }
        public string Symbol { get; set; }
        public int HoldingId { get; set; }
    }

    [Table("MailingList")]
    public class MailingList
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Email { get; set; }
        public DateTime DateAdded { get; set; }
        public bool Subscribed { get; set; }
    }

    [Table("AccountProfile")]
    public class AccountProfile
    {
        [Key]
        public Guid UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string BankName { get; set; }

        public string AccountNumber { get; set; }

        public string RoutingNumber { get; set; }

        public string AccountType { get; set; }

        public bool? Verified { get; set; }

        public bool? BankVerified { get; set; }

        public DateTime? SignupDate { get; set; }

        public string Occupation { get; set; }

        public DateTime BirthDate { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Country { get; set; }

        public string PostalCode { get; set; }

        public string NAddress1 { get; set; }

        public string NAddress2 { get; set; }

        public string NCity { get; set; }

        public string NState { get; set; }

        public string NCountry { get; set; }

        public string NPostalCode { get; set; }

        public string Phone1 { get; set; }

        public string Phone2 { get; set; }

        public string Phone3 { get; set; }

        public string Fax { get; set; }

        public string Telex { get; set; }

        public string Email { get; set; }

        public string NextOfKin { get; set; }

        public string Question { get; set; }

        public string Answer { get; set; }
    }

    [Table("dailypricelist")]
    public class Dailypricelist
    {
        public int Id { get; set; }

        public DateTime? Date { get; set; }

        [StringLength(255)]
        public string Stock { get; set; }

        public double? Open { get; set; }

        public double? High { get; set; }

        public double? Low { get; set; }

        public double? Close { get; set; }

        public double? Change { get; set; }

        public double? Deals { get; set; }

        public double? Volume { get; set; }
    }
}