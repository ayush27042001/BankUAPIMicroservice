using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BankUAPI.Infrastructure.Sql.Entities;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AddContact> AddContacts { get; set; }

    public virtual DbSet<Addfund> Addfunds { get; set; }

    public virtual DbSet<Aepslog> Aepslogs { get; set; }

    public virtual DbSet<AepsrequestLog> AepsrequestLogs { get; set; }

    public virtual DbSet<AllLoginDoc> AllLoginDocs { get; set; }

    public virtual DbSet<ApiacctiveRequest> ApiacctiveRequests { get; set; }

    public virtual DbSet<Apicategory> Apicategories { get; set; }

    public virtual DbSet<Apidocument> Apidocuments { get; set; }

    public virtual DbSet<Apilist> Apilists { get; set; }

    public virtual DbSet<Apilog> Apilogs { get; set; }

    public virtual DbSet<Apiwebhook> Apiwebhooks { get; set; }

    public virtual DbSet<BankPayout> BankPayouts { get; set; }

    public virtual DbSet<BankUblog> BankUblogs { get; set; }

    public virtual DbSet<BankUproduct> BankUproducts { get; set; }

    public virtual DbSet<BankUservice> BankUservices { get; set; }

    public virtual DbSet<BankuOrder> BankuOrders { get; set; }

    public virtual DbSet<Bankuteam> Bankuteams { get; set; }

    public virtual DbSet<BillPayment> BillPayments { get; set; }

    public virtual DbSet<Blog> Blogs { get; set; }

    public virtual DbSet<BlogCategory> BlogCategories { get; set; }

    public virtual DbSet<CompanyInfo> CompanyInfos { get; set; }

    public virtual DbSet<DistAcctiveRequest> DistAcctiveRequests { get; set; }

    public virtual DbSet<DistUserAdd> DistUserAdds { get; set; }

    public virtual DbSet<DmtOtpLog> DmtOtpLogs { get; set; }

    public virtual DbSet<DmtSenderRegistration> DmtSenderRegistrations { get; set; }

    public virtual DbSet<DmtaddBeneficiary> DmtaddBeneficiaries { get; set; }

    public virtual DbSet<DmtaddBeneficiaryError> DmtaddBeneficiaryErrors { get; set; }

    public virtual DbSet<Dmtpay> Dmtpays { get; set; }

    public virtual DbSet<LoginDoc> LoginDocs { get; set; }

    public virtual DbSet<MarginSetting> MarginSettings { get; set; }

    public virtual DbSet<MobileAppVersion> MobileAppVersions { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Onboarding> Onboardings { get; set; }

    public virtual DbSet<PaymentAccount> PaymentAccounts { get; set; }

    public virtual DbSet<ProfileUpdateRequest> ProfileUpdateRequests { get; set; }

    public virtual DbSet<Recharge> Recharges { get; set; }

    public virtual DbSet<Registration> Registrations { get; set; }

    public virtual DbSet<ServiceActivation> ServiceActivations { get; set; }

    public virtual DbSet<TblDispute> TblDisputes { get; set; }

    public virtual DbSet<TblFingerprintLog> TblFingerprintLogs { get; set; }

    public virtual DbSet<Tbloperator> Tbloperators { get; set; }

    public virtual DbSet<Tbluserbalance> Tbluserbalances { get; set; }

    public virtual DbSet<TxnReport> TxnReports { get; set; }

    public virtual DbSet<UserAgreement> UserAgreements { get; set; }

    public virtual DbSet<UserInvoice> UserInvoices { get; set; }

    public virtual DbSet<UserTicket> UserTickets { get; set; }

    public DbSet<CommissionHeader> CommissionHeader { get; set; }
    public DbSet<CommissionSlab> CommissionSlabs { get; set; }
    public DbSet<CommissionDistribution> CommissionDistribution { get; set; }

    public DbSet<MasterProvider> MASTER_PROVIDER { get; set; } = null!;
    public DbSet<ServiceProvider> SERVICE_PROVIDER { get; set; } = null!;
    public DbSet<MasterFeature> MASTER_FEATURE { get; set; } = null!;
    public DbSet<ServiceProviderFeatureMap> SERVICE_PROVIDER_FEATURE_MAP { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=103.205.142.34,1433;Initial Catalog=BankUIndia_db;Persist Security Info=True;User ID=BankUIndia_db;Password=Chandan@80100;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("BankUIndia_db");

        modelBuilder.Entity<AddContact>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("AddContact");

            entity.Property(e => e.Address)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Cin)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CIN");
            entity.Property(e => e.CompanyName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ContactPersonName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ContactType)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.FullName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Gstin)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("GSTIN");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID");
            entity.Property(e => e.Pan)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("PAN");
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Pincode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Tan)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("TAN");
            entity.Property(e => e.Udyam)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("UDYAM");
            entity.Property(e => e.UserId)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Addfund>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Addfund__3214EC073C9E073D");

            entity.ToTable("Addfund", "dbo");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AmountPaid).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.OrderId).HasMaxLength(100);
            entity.Property(e => e.ReqDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.TxnId).HasMaxLength(100);
            entity.Property(e => e.UserId).HasMaxLength(50);
            entity.Property(e => e.UserName).HasMaxLength(100);
        });

        modelBuilder.Entity<Aepslog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AEPSLogs__3214EC0757473063");

            entity.ToTable("AEPSLogs");

            entity.Property(e => e.Aadhar).HasMaxLength(20);
            entity.Property(e => e.Bank).HasMaxLength(50);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Device).HasMaxLength(20);
            entity.Property(e => e.Mobile).HasMaxLength(20);
            entity.Property(e => e.OperatorName).HasMaxLength(50);
            entity.Property(e => e.TxnType).HasMaxLength(50);
        });

        modelBuilder.Entity<AepsrequestLog>(entity =>
        {
            entity.HasKey(e => e.Personid).HasName("PK__AEPSRequ__AA2CFFDD257827AA");

            entity.ToTable("AEPSRequestLogs");

            entity.Property(e => e.ApiType)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Reqdate)
                .HasColumnType("datetime")
                .HasColumnName("reqdate");
            entity.Property(e => e.Request)
                .IsUnicode(false)
                .HasColumnName("request");
            entity.Property(e => e.Responce)
                .IsUnicode(false)
                .HasColumnName("responce");
        });

        modelBuilder.Entity<AllLoginDoc>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AllLogin__3214EC27654451B1");

            entity.ToTable("AllLoginDoc");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ExpireAt).HasColumnType("datetime");
            entity.Property(e => e.Ipaddress)
                .HasMaxLength(50)
                .HasColumnName("IPAddress");
            entity.Property(e => e.UserId)
                .HasMaxLength(20)
                .HasColumnName("UserID");
            entity.Property(e => e.UserType).HasMaxLength(50);
        });

        modelBuilder.Entity<ApiacctiveRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__APIAccti__3214EC07C6D68C11");

            entity.ToTable("APIAcctiveRequest");

            entity.Property(e => e.Apiusecase)
                .IsUnicode(false)
                .HasColumnName("APIUsecase");
            entity.Property(e => e.ReqDate)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ReqId)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserId)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.WebsiteUrl).IsUnicode(false);
        });

        modelBuilder.Entity<Apicategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__APICateg__3214EC07FCE2F383");

            entity.ToTable("APICategory");

            entity.Property(e => e.Category).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(50);
        });

        modelBuilder.Entity<Apidocument>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__APIDocum__3214EC07A202225E");

            entity.ToTable("APIDocument");

            entity.Property(e => e.ApiType).HasMaxLength(10);
            entity.Property(e => e.Apiname)
                .HasMaxLength(250)
                .HasColumnName("APIName");
            entity.Property(e => e.Category).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(30);
        });

        modelBuilder.Entity<Apilist>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__APIList__3214EC0721AB4D82");

            entity.ToTable("APIList");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ApiName).HasMaxLength(100);
            entity.Property(e => e.Category).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(30);
        });

        modelBuilder.Entity<Apilog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__APILOGS__3214EC0783627F0B");

            entity.ToTable("APILOGS");

            entity.Property(e => e.ApiType).HasMaxLength(100);
            entity.Property(e => e.RequestDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UserId).HasMaxLength(20);
        });

        modelBuilder.Entity<Apiwebhook>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__APIWebho__3214EC07CB823784");

            entity.ToTable("APIWebhook");
        });

        modelBuilder.Entity<BankPayout>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BankPayo__3214EC271A8C720D");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AccountNumber).HasMaxLength(50);
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.BankName).HasMaxLength(200);
            entity.Property(e => e.BeneficiaryEmail).HasMaxLength(200);
            entity.Property(e => e.BeneficiaryName).HasMaxLength(200);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Ifsc)
                .HasMaxLength(20)
                .HasColumnName("IFSC");
            entity.Property(e => e.MobileNo).HasMaxLength(50);
            entity.Property(e => e.Mode).HasMaxLength(10);
            entity.Property(e => e.OrderId).HasMaxLength(50);
            entity.Property(e => e.PayoutTo).HasMaxLength(200);
            entity.Property(e => e.Remarks).HasMaxLength(500);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Type).HasMaxLength(50);
            entity.Property(e => e.UserId).HasMaxLength(50);
        });

        modelBuilder.Entity<BankUblog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BankUBlo__3214EC07216000AC");

            entity.ToTable("BankUBlog");

            entity.Property(e => e.Category).IsUnicode(false);
            entity.Property(e => e.DateTime)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.SecondHeading).IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<BankUproduct>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BankUPro__3214EC072810336D");

            entity.ToTable("BankUProduct");

            entity.Property(e => e.Amount).HasMaxLength(10);
            entity.Property(e => e.Model).HasMaxLength(50);
            entity.Property(e => e.ProductName).HasMaxLength(100);
            entity.Property(e => e.Status).HasMaxLength(30);
        });

        modelBuilder.Entity<BankUservice>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BankUSer__3214EC0736392808");

            entity.ToTable("BankUServices");

            entity.Property(e => e.ServiceName).HasMaxLength(250);
            entity.Property(e => e.Status).HasMaxLength(30);
        });

        modelBuilder.Entity<BankuOrder>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BankuOrd__3214EC0735B6F854");

            entity.ToTable("BankuOrder");

            entity.Property(e => e.OrderDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.OrderId).HasMaxLength(50);
            entity.Property(e => e.ProductName).HasMaxLength(250);
            entity.Property(e => e.ProductPrice).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(30);
            entity.Property(e => e.Total).HasMaxLength(50);
        });

        modelBuilder.Entity<Bankuteam>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__bankutea__3214EC076072F281");

            entity.ToTable("bankuteam", "dbo");

            entity.Property(e => e.AadharCardNo)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Dob).HasColumnName("DOB");
            entity.Property(e => e.EmailId)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.FullAddress).IsUnicode(false);
            entity.Property(e => e.Gender)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.MobileNo)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.PancardNo)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.ReqDate).HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UserRoll)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<BillPayment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BillPaym__3214EC07001AEDDA");

            entity.Property(e => e.AccountNo).HasMaxLength(100);
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ApiResponse).IsUnicode(false);
            entity.Property(e => e.BillType).HasMaxLength(50);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Mobile).HasMaxLength(20);
            entity.Property(e => e.Operator).HasMaxLength(100);
            entity.Property(e => e.OperatorName).IsUnicode(false);
            entity.Property(e => e.OrderId).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(20);
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Blog>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Blog");

            entity.Property(e => e.Categories).IsUnicode(false);
            entity.Property(e => e.Content).IsUnicode(false);
            entity.Property(e => e.DateTime).HasColumnType("datetime");
            entity.Property(e => e.Heading).IsUnicode(false);
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID");
            entity.Property(e => e.Longcontent).IsUnicode(false);
            entity.Property(e => e.Picture)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<BlogCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__blogCate__3214EC0796879BE4");

            entity.ToTable("blogCategory");

            entity.Property(e => e.Status).HasMaxLength(20);
        });

        modelBuilder.Entity<CompanyInfo>(entity =>
        {
            entity.HasKey(e => e.CompanyId).HasName("PK__CompanyI__2D971CACCD9D89BC");

            entity.ToTable("CompanyInfo");

            entity.Property(e => e.CompanyName).HasMaxLength(200);
            entity.Property(e => e.CompanyNumber).HasMaxLength(50);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(150);
            entity.Property(e => e.Facebook).HasMaxLength(250);
            entity.Property(e => e.GalleryImage1).HasMaxLength(250);
            entity.Property(e => e.GalleryImage2).HasMaxLength(250);
            entity.Property(e => e.GalleryImage3).HasMaxLength(250);
            entity.Property(e => e.GalleryImage4).HasMaxLength(250);
            entity.Property(e => e.Instagram).HasMaxLength(250);
            entity.Property(e => e.LinkedIn).HasMaxLength(250);
            entity.Property(e => e.Location).HasMaxLength(250);
            entity.Property(e => e.LogoImage).HasMaxLength(250);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Twitter).HasMaxLength(250);
            entity.Property(e => e.UserId)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<DistAcctiveRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DistAcct__3214EC078F05D6CE");

            entity.ToTable("DistAcctiveRequest");

            entity.Property(e => e.ReqDate)
                .HasMaxLength(40)
                .IsUnicode(false);
            entity.Property(e => e.ReqId)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.TeamSize).IsUnicode(false);
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserId)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<DistUserAdd>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DistUser__3214EC07C630656B");

            entity.ToTable("DistUserAdd");

            entity.Property(e => e.Number)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ReqDate)
                .HasMaxLength(40)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.UserId)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<DmtOtpLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DmtOtpLo__3214EC077BD37D27");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.SenderId).HasMaxLength(50);
            entity.Property(e => e.SenderMobileNo).HasMaxLength(20);
        });

        modelBuilder.Entity<DmtSenderRegistration>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("DmtSenderRegistration");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID");
            entity.Property(e => e.SenderGender)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.SenderId)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.SenderMobileNo)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.SenderName)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<DmtaddBeneficiary>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("DMTAddBeneficiary");

            entity.Property(e => e.BankIfscCod)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("bankIfscCod");
            entity.Property(e => e.BeneficiaryAccNo)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("beneficiaryAccNo");
            entity.Property(e => e.BeneficiaryBankName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("beneficiaryBankName");
            entity.Property(e => e.BeneficiaryId)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.BeneficiaryMobileNumber)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("beneficiaryMobileNumber");
            entity.Property(e => e.BeneficiaryName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("beneficiaryName");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID");
            entity.Property(e => e.SenderId)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.SenderMobileNo)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("senderMobileNo");
            entity.Property(e => e.TransferMode)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.TrnasferMode)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("trnasferMode");
        });

        modelBuilder.Entity<DmtaddBeneficiaryError>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DMTAddBe__3214EC073BB06CF7");

            entity.ToTable("DMTAddBeneficiaryErrors");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ErrorMessage).HasMaxLength(500);
            entity.Property(e => e.SenderId).HasMaxLength(50);
            entity.Property(e => e.SenderMobileNo).HasMaxLength(20);
        });

        modelBuilder.Entity<Dmtpay>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("DMTPay");

            entity.Property(e => e.AccountNo)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Amount)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.BankTransferMode)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.BeneName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID");
            entity.Property(e => e.Ifsccode)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("IFSCCode");
            entity.Property(e => e.OrderId)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("orderID");
            entity.Property(e => e.SenderId)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("SenderID");
            entity.Property(e => e.Status)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.TxnPin)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UserId)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("UserID");
        });

        modelBuilder.Entity<LoginDoc>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__LoginDoc__3214EC27E0323B02");

            entity.ToTable("LoginDoc");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ExpireAt).HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.SessionKey).HasMaxLength(100);
            entity.Property(e => e.UserId)
                .HasMaxLength(200)
                .HasColumnName("UserID");
        });

        modelBuilder.Entity<MarginSetting>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MarginSe__3214EC07678ACE48");

            entity.ToTable("MarginSetting");

            entity.Property(e => e.CommissionType).HasMaxLength(50);
            entity.Property(e => e.Ipshare)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("IPShare");
            entity.Property(e => e.OperatorName).HasMaxLength(100);
            entity.Property(e => e.ServiceName).HasMaxLength(100);
            entity.Property(e => e.Wlshare)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("WLShare");
        });

        modelBuilder.Entity<MobileAppVersion>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("MobileAppVersion");

            entity.Property(e => e.AppVer)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.NotificationId).HasName("PK__Notifica__20CF2E32A9B65125");

            entity.Property(e => e.NotificationId).HasColumnName("NotificationID");
            entity.Property(e => e.Content).HasMaxLength(500);
            entity.Property(e => e.Status).HasMaxLength(50);
        });

        modelBuilder.Entity<Onboarding>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__onboardi__1788CCACC14116CB");

            entity.ToTable("onboarding", "dbo");

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.AadharNo)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.AadharUpload)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Aadharbackcopy)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.AccountHolderName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Accountno)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Accounttype)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Address).IsUnicode(false);
            entity.Property(e => e.AdminRemarks)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.AgrementCopy)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.AgrementStatus)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ApprovedBy)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ApprovedDate).HasColumnType("datetime");
            entity.Property(e => e.BankName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Bankaccountstatus)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.BusinessType)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Businessdetailsstatus)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("businessdetailsstatus");
            entity.Property(e => e.CompanyName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Companyaddress)
                .IsUnicode(false)
                .HasColumnName("companyaddress");
            entity.Property(e => e.Dob).HasColumnName("DOB");
            entity.Property(e => e.DocumentStatus)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Education)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.EmailId)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.FatherName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.FullName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Gender)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.GstUpload)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.IfscCode)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.JioTagPhotoUpload)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.KycStatus)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.MobileNo)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.MobileverifyStatus)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.MotherName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.NotificationType)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.OnboardingStatus)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.PanUpload)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.PancardNo)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.PassportSizePhoto)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.PersonalInfoStatus)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Pincode)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.RegDate).HasColumnType("datetime");
            entity.Property(e => e.RegistrationForm)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.RegistrationStatus)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.SignupStatus)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.State)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UdyamRegNo)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UdyamUpload)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UserType)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Userposition)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.VoterCardNo)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.VoterCardUpload)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<PaymentAccount>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PaymentA__3214EC27618AE446");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AccountNumber)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.BankName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.BeneficiaryName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Ifsc)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("IFSC");
            entity.Property(e => e.PaymentMethod)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Upiid)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("UPIID");
            entity.Property(e => e.VendorEmail).HasMaxLength(150);
            entity.Property(e => e.VendorId)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ProfileUpdateRequest>(entity =>
        {
            entity.HasKey(e => e.RequestId).HasName("PK__ProfileU__33A8517A893D500D");

            entity.Property(e => e.DetailType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NewValue).IsUnicode(false);
            entity.Property(e => e.RequestDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.RequestStatus)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("Pending");
        });

        modelBuilder.Entity<Recharge>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Recharge__3214EC2776DE8327");

            entity.ToTable("Recharge");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ApiResponse).IsUnicode(false);
            entity.Property(e => e.Circle)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CurrBalance).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.MobileNo)
                .HasMaxLength(12)
                .HasColumnName("mobileNo");
            entity.Property(e => e.Operatorname)
                .HasMaxLength(50)
                .HasColumnName("operatorname");
            entity.Property(e => e.OrderId)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("status");
            entity.Property(e => e.Type).HasMaxLength(50);
            entity.Property(e => e.UserId)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Registration>(entity =>
        {
            entity.HasKey(e => e.RegistrationId).HasName("PK__Registra__6EF5881086A2FBEF");

            entity.ToTable("Registration");

            entity.Property(e => e.AadharNo)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.AccHolder)
                .HasMaxLength(60)
                .IsUnicode(false);
            entity.Property(e => e.AccountHolderType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AddressUser).IsUnicode(false);
            entity.Property(e => e.ApprovedIp)
                .HasMaxLength(50)
                .HasColumnName("ApprovedIP");
            entity.Property(e => e.BankAccount)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.BankName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.BusinessPan)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("BusinessPAN");
            entity.Property(e => e.BusinessProof)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.BusinessStartOn)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.BusinessType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CompanyName).IsUnicode(false);
            entity.Property(e => e.Dob)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("DOB");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FaceVerificationResult)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FatherName)
                .HasMaxLength(60)
                .IsUnicode(false);
            entity.Property(e => e.FullName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Gender)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Gstno)
                .HasMaxLength(50)
                .HasColumnName("GSTNo");
            entity.Property(e => e.Ifsc)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("IFSC");
            entity.Property(e => e.Ipaddress)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("IPAddress");
            entity.Property(e => e.Latitude)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Longitude)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.MobileNo)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.Mpin)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("MPIN");
            entity.Property(e => e.NatureOfBusiness)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.OutletId)
                .HasMaxLength(10)
                .HasColumnName("outletId");
            entity.Property(e => e.Panno)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("PANNo");
            entity.Property(e => e.Postal)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.RegDate)
                .HasMaxLength(40)
                .IsUnicode(false);
            entity.Property(e => e.RegistrationStatus)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.State)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.UserType)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.VoterIdcard)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("VoterIDCard");
        });

        modelBuilder.Entity<ServiceActivation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ServiceA__3214EC2755E75BC2");

            entity.ToTable("ServiceActivation");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ServiceType).HasMaxLength(100);
            entity.Property(e => e.Status).HasMaxLength(20);
            entity.Property(e => e.UserId)
                .HasMaxLength(20)
                .HasColumnName("UserID");
            entity.Property(e => e.UserMessage).HasMaxLength(10);
        });

        modelBuilder.Entity<TblDispute>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tblDispu__3214EC079407A0CE");

            entity.ToTable("tblDisputes");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DisputeType)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ProofPath)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("Pending");
            entity.Property(e => e.TransactionId)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TblFingerprintLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tblFinge__3214EC07CA16EB8E");

            entity.ToTable("tblFingerprintLog");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Ipaddress)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("IPAddress");
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Tbloperator>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tblopera__3214EC07A3972357");

            entity.ToTable("tbloperator", "dbo");

            entity.Property(e => e.OperatorImage)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.OperatorName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ServiceName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Spkey)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("SPkey");
            entity.Property(e => e.Status)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Tbluserbalance>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tbluserb__3214EC07DEDC8480");

            entity.ToTable("tbluserbalance", "dbo");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CrDrType)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.NewBal)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("New_Bal");
            entity.Property(e => e.OldBal)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Old_Bal");
            entity.Property(e => e.TxnDatetime).HasColumnType("datetime");
            entity.Property(e => e.TxnType)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TxnReport>(entity =>
        {
            entity.HasKey(e => e.TransId).HasName("PK__TxnRepor__9E5DDB1C5843F935");

            entity.ToTable("TxnReport", "dbo");

            entity.Property(e => e.TransId).HasColumnName("TransID");
            entity.Property(e => e.AccountNo)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ApiMsg)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ApiRequest).IsUnicode(false);
            entity.Property(e => e.ApiResponse).IsUnicode(false);
            entity.Property(e => e.Apiname)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("APIName");
            entity.Property(e => e.BankName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.BeneName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Brid)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("BRID");
            entity.Property(e => e.CallbackResponse).IsUnicode(false);
            entity.Property(e => e.Commission).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.IfscCode)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.IpAddress)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.MobileNo)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.NewBal).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.OldBal).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.OperatorId)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.OperatorName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ServiceName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Surcharge).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalCost).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TransactionId)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("TransactionID");
            entity.Property(e => e.TxnDate).HasColumnType("datetime");
            entity.Property(e => e.TxnUpdateDate).HasColumnType("datetime");
            entity.Property(e => e.UserId)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UserName)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<UserAgreement>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserAgre__3214EC078FF3FBF3");

            entity.ToTable("UserAgreement");

            entity.Property(e => e.AgreementId)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.AgreementType)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FilePath).IsUnicode(false);
            entity.Property(e => e.FullName)
                .HasMaxLength(100)
                .HasColumnName("fullName");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("Pending");
        });

        modelBuilder.Entity<UserInvoice>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserInvo__3214EC07B760EFD0");

            entity.ToTable("UserInvoice");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.EndDate)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.FilePath).IsUnicode(false);
            entity.Property(e => e.InvoiceId)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.InvoiceType)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.StartDate)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("Pending");
        });

        modelBuilder.Entity<UserTicket>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__userTick__3214EC072F50E8B5");

            entity.ToTable("userTicket");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(20);
            entity.Property(e => e.Type).HasMaxLength(100);
            entity.Property(e => e.UserId).HasMaxLength(10);
        });

        modelBuilder.Entity<CommissionHeader>()
        .HasMany(h => h.Slabs)
        .WithOne(s => s.CommissionHeader)
        .HasForeignKey(s => s.CommissionRuleId)
        .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<CommissionSlab>()
            .HasMany(s => s.Distributions)
            .WithOne(d => d.CommissionSlab)
            .HasForeignKey(d => d.CommissionSlabId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ServiceProviderFeatureMap>()
               .HasIndex(x => new { x.ServiceCode, x.ProviderCode, x.FeatureCode })
               .IsUnique();

        // Relationships
        modelBuilder.Entity<ServiceProvider>()
            .HasOne(sp => sp.Provider)
            .WithMany(p => p.ServiceProviders)
            .HasForeignKey(sp => sp.ProviderCode)
            .HasPrincipalKey(p => p.ProviderCode);

        modelBuilder.Entity<ServiceProviderFeatureMap>()
            .HasOne(spfm => spfm.Provider)
            .WithMany(p => p.ServiceProviderFeatureMaps)
            .HasForeignKey(spfm => spfm.ProviderCode)
            .HasPrincipalKey(p => p.ProviderCode);

        modelBuilder.Entity<ServiceProviderFeatureMap>()
            .HasOne(spfm => spfm.ServiceProvider)
            .WithMany(sp => sp.FeatureMaps)
            .HasForeignKey(spfm => new { spfm.ServiceCode, spfm.ProviderCode })
            .HasPrincipalKey(sp => new { sp.ServiceCode, sp.ProviderCode });

        modelBuilder.Entity<ServiceProviderFeatureMap>()
            .HasOne(spfm => spfm.Feature)
            .WithMany(f => f.ServiceProviderFeatureMaps)
            .HasForeignKey(spfm => new { spfm.ServiceCode, spfm.FeatureCode })
            .HasPrincipalKey(f => new { f.ServiceCode, f.FeatureCode });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
