using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Bigprofits.Models;

namespace Bigprofits.Data
{
    public partial class ContextClass : DbContext
    {
        public ContextClass()
        {
        }

        public ContextClass(DbContextOptions<ContextClass> options)
            : base(options)
        {
        }

        public virtual DbSet<AccountBinary> AccountBinaries { get; set; } = null!;
        public virtual DbSet<AccountLevel> AccountLevels { get; set; } = null!;
        public virtual DbSet<AccountSummary> AccountSummaries { get; set; } = null!;
        public virtual DbSet<AccountVirtual> AccountVirtuals { get; set; } = null!;
        public virtual DbSet<Achiever> Achievers { get; set; } = null!;
        public virtual DbSet<AchieversIncome> AchieversIncomes { get; set; } = null!;
        public virtual DbSet<Achivement> Achivements { get; set; } = null!;
        public virtual DbSet<AddProduct> AddProducts { get; set; } = null!;
        public virtual DbSet<AddressInfo> AddressInfos { get; set; } = null!;
        public virtual DbSet<AdminAction> AdminActions { get; set; } = null!;
        public virtual DbSet<AdminAddressInfo> AdminAddressInfos { get; set; } = null!;
        public virtual DbSet<AlteredWithdrawal> AlteredWithdrawals { get; set; } = null!;
        public virtual DbSet<BuyToken> BuyTokens { get; set; } = null!;
        public virtual DbSet<ClientSystemInfo> ClientSystemInfos { get; set; } = null!;
        public virtual DbSet<CmitRequest> CmitRequests { get; set; } = null!;
        public virtual DbSet<ComplainMail> ComplainMails { get; set; } = null!;
        public virtual DbSet<ContactUsTable> ContactUsTables { get; set; } = null!;
        public virtual DbSet<ContryCode> ContryCodes { get; set; } = null!;
        public virtual DbSet<FundAmount> FundAmounts { get; set; } = null!;
        public virtual DbSet<InspBiller> InspBillers { get; set; } = null!;
        public virtual DbSet<LevelSponsor> LevelSponsors { get; set; } = null!;
        public virtual DbSet<ListCircle> ListCircles { get; set; } = null!;
        public virtual DbSet<ListCountry> ListCountries { get; set; } = null!;
        public virtual DbSet<ListLevel> ListLevels { get; set; } = null!;
        public virtual DbSet<ListMatrixLvl> ListMatrixLvls { get; set; } = null!;
        public virtual DbSet<ListOperator> ListOperators { get; set; } = null!;
        public virtual DbSet<ListState> ListStates { get; set; } = null!;
        public virtual DbSet<Matrix2> Matrix2s { get; set; } = null!;
        public virtual DbSet<MatrixIncome> MatrixIncomes { get; set; } = null!;
        public virtual DbSet<MatrixInfo> MatrixInfos { get; set; } = null!;
        public virtual DbSet<MemPageOlog> MemPageOlogs { get; set; } = null!;
        public virtual DbSet<MemberDirect> MemberDirects { get; set; } = null!;
        public virtual DbSet<MemberInfo> MemberInfos { get; set; } = null!;
        public virtual DbSet<MemberInfoBackup> MemberInfoBackups { get; set; } = null!;
        public virtual DbSet<MemberInfoHistory> MemberInfoHistories { get; set; } = null!;
        public virtual DbSet<MemberNotification> MemberNotifications { get; set; } = null!;
        public virtual DbSet<MemberWidInfo> MemberWidInfos { get; set; } = null!;
        public virtual DbSet<MemberWidInfoBackup> MemberWidInfoBackups { get; set; } = null!;
        public virtual DbSet<OnlineService> OnlineServices { get; set; } = null!;
        public virtual DbSet<OrderDetail> OrderDetails { get; set; } = null!;
        public virtual DbSet<PinDetail> PinDetails { get; set; } = null!;
        public virtual DbSet<Poster> Posters { get; set; } = null!;
        public virtual DbSet<ProductCat> ProductCats { get; set; } = null!;
        public virtual DbSet<ProductRequest> ProductRequests { get; set; } = null!;
        public virtual DbSet<ProductSubCat> ProductSubCats { get; set; } = null!;
        public virtual DbSet<Reward> Rewards { get; set; } = null!;
        public virtual DbSet<RewardInfo> RewardInfos { get; set; } = null!;
        public virtual DbSet<RewardReturn> RewardReturns { get; set; } = null!;
        public virtual DbSet<SmsAdmin> SmsAdmins { get; set; } = null!;
        public virtual DbSet<StakeAmount> StakeAmounts { get; set; } = null!;
        public virtual DbSet<StakeInfo> StakeInfos { get; set; } = null!;
        public virtual DbSet<TableCart> TableCarts { get; set; } = null!;
        public virtual DbSet<TableSupport> TableSupports { get; set; } = null!;
        public virtual DbSet<TblApiPaymentReq> TblApiPaymentReqs { get; set; } = null!;
        public virtual DbSet<TblNews> TblNews { get; set; } = null!;
        public virtual DbSet<TeamDetail> TeamDetails { get; set; } = null!;
        public virtual DbSet<TeamInfo> TeamInfos { get; set; } = null!;
        public virtual DbSet<TokenGrowthRate> TokenGrowthRates { get; set; } = null!;
        public virtual DbSet<UmTryingUpdate> UmTryingUpdates { get; set; } = null!;
        public virtual DbSet<UnilevelPurchase> UnilevelPurchases { get; set; } = null!;
        public virtual DbSet<UpgradeTable> UpgradeTables { get; set; } = null!;
        public virtual DbSet<UserMaster> UserMasters { get; set; } = null!;
        public virtual DbSet<UserMasterSubAdmin> UserMasterSubAdmins { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json")
               .Build();
                var connectionString = configuration.GetConnectionString("DefaultConnection");
                optionsBuilder.UseSqlServer(connectionString!);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccountBinary>(entity =>
            {
                entity.HasKey(e => e.BnrySno);

                entity.ToTable("accountBinary");

                entity.Property(e => e.BnrySno).HasColumnName("bnrySno");

                entity.Property(e => e.Admincharge)
                    .HasColumnType("money")
                    .HasColumnName("admincharge");

                entity.Property(e => e.Bcount).HasColumnName("bcount");

                entity.Property(e => e.BnryAmount)
                    .HasColumnType("money")
                    .HasColumnName("bnryAmount")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.BnryCdate)
                    .HasColumnType("datetime")
                    .HasColumnName("bnryCDate");

                entity.Property(e => e.BnryCfleft)
                    .HasColumnName("bnryCFLeft")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.BnryCfright)
                    .HasColumnName("bnryCFRight")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.BnryMatch)
                    .HasColumnType("money")
                    .HasColumnName("bnryMatch")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.CmitId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("cmitId");

                entity.Property(e => e.Finalamount)
                    .HasColumnType("money")
                    .HasColumnName("finalamount");

                entity.Property(e => e.LeftBv)
                    .HasColumnType("money")
                    .HasColumnName("leftBV");

                entity.Property(e => e.MemberId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("memberId")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.RightBv)
                    .HasColumnType("money")
                    .HasColumnName("rightBV");

                entity.Property(e => e.Rpcharge)
                    .HasColumnType("money")
                    .HasColumnName("rpcharge");

                entity.Property(e => e.Tds)
                    .HasColumnType("money")
                    .HasColumnName("tds");
            });

            modelBuilder.Entity<AccountLevel>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("AccountLevel");

                entity.Property(e => e.AdminCharge).HasColumnType("money");

                entity.Property(e => e.Ammount).HasColumnType("money");

                entity.Property(e => e.Bymemberid)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CmitId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("cmitId");

                entity.Property(e => e.Crntslab).HasColumnName("CRNTSLAB");

                entity.Property(e => e.Ddate).HasColumnType("datetime");

                entity.Property(e => e.FinalAmount).HasColumnType("money");

                entity.Property(e => e.HasId)
                    .HasMaxLength(200)
                    .HasColumnName("hasId");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.LvlNo).HasColumnName("lvlNo");

                entity.Property(e => e.Memberid)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("memberid");

                entity.Property(e => e.Mypackage)
                    .HasColumnType("money")
                    .HasColumnName("mypackage");

                entity.Property(e => e.Package)
                    .HasColumnType("money")
                    .HasColumnName("package");

                entity.Property(e => e.Rpcharge)
                    .HasColumnType("money")
                    .HasColumnName("rpcharge");

                entity.Property(e => e.Tds)
                    .HasColumnType("money")
                    .HasColumnName("TDS");
            });

            modelBuilder.Entity<AccountSummary>(entity =>
            {
                entity.HasKey(e => e.SmrySno);

                entity.ToTable("accountSummary");

                entity.Property(e => e.SmrySno).HasColumnName("smrySno");

                entity.Property(e => e.MemberId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("memberId")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.NewIncome)
                    .HasColumnType("money")
                    .HasColumnName("newIncome");

                entity.Property(e => e.PrimaryFund)
                    .HasColumnType("money")
                    .HasColumnName("primaryFund");

                entity.Property(e => e.SmryRetBnry)
                    .HasColumnType("money")
                    .HasColumnName("smryRetBnry")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.SmryRetClub)
                    .HasColumnType("money")
                    .HasColumnName("smryRetClub")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.SmryRetLvl)
                    .HasColumnType("money")
                    .HasColumnName("smryRetLvl")
                    .HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<AccountVirtual>(entity =>
            {
                entity.HasKey(e => e.VrtlSno);

                entity.ToTable("accountVirtual");

                entity.Property(e => e.VrtlSno).HasColumnName("vrtlSno");

                entity.Property(e => e.Admincharge)
                    .HasColumnType("money")
                    .HasColumnName("admincharge");

                entity.Property(e => e.Booster).HasColumnName("booster");

                entity.Property(e => e.Bymemberid)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("bymemberid");

                entity.Property(e => e.Cmtid)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("cmtid");

                entity.Property(e => e.FinalAmount).HasColumnType("money");

                entity.Property(e => e.Lvl).HasColumnName("lvl");

                entity.Property(e => e.MRoiAmount)
                    .HasColumnType("money")
                    .HasColumnName("mRoiAmount");

                entity.Property(e => e.MemberId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("memberId");

                entity.Property(e => e.Mempackage)
                    .HasColumnType("money")
                    .HasColumnName("mempackage");

                entity.Property(e => e.RoiAmount).HasColumnType("money");

                entity.Property(e => e.Roidate)
                    .HasColumnType("datetime")
                    .HasColumnName("roidate");

                entity.Property(e => e.Rpcharge)
                    .HasColumnType("money")
                    .HasColumnName("rpcharge");

                entity.Property(e => e.Sroi).HasColumnName("sroi");

                entity.Property(e => e.Tds)
                    .HasColumnType("money")
                    .HasColumnName("tds");
            });

            modelBuilder.Entity<Achiever>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.Adate)
                    .HasColumnType("datetime")
                    .HasColumnName("adate");

                entity.Property(e => e.Amount)
                    .HasColumnType("money")
                    .HasColumnName("amount");

                entity.Property(e => e.Astatus).HasColumnName("astatus");

                entity.Property(e => e.Atype)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("atype");

                entity.Property(e => e.Direct).HasColumnName("direct");

                entity.Property(e => e.Icount).HasColumnName("icount");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.Lvl).HasColumnName("lvl");

                entity.Property(e => e.MemberId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("memberId");

                entity.Property(e => e.OLage).HasColumnName("oLage");

                entity.Property(e => e.SLage).HasColumnName("sLage");

                entity.Property(e => e.TillCount).HasColumnName("tillCount");
            });

            modelBuilder.Entity<AchieversIncome>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("AchieversIncome");

                entity.Property(e => e.Adate)
                    .HasColumnType("datetime")
                    .HasColumnName("adate");

                entity.Property(e => e.AdminCharge)
                    .HasColumnType("money")
                    .HasColumnName("adminCharge");

                entity.Property(e => e.Amount)
                    .HasColumnType("money")
                    .HasColumnName("amount");

                entity.Property(e => e.Astatus).HasColumnName("astatus");

                entity.Property(e => e.Atype)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("atype");

                entity.Property(e => e.CmitId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("cmitId");

                entity.Property(e => e.FinalAmount)
                    .HasColumnType("money")
                    .HasColumnName("finalAmount");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.Lvl).HasColumnName("lvl");

                entity.Property(e => e.MemberId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("memberId");

                entity.Property(e => e.Tds)
                    .HasColumnType("money")
                    .HasColumnName("tds");
            });

            modelBuilder.Entity<Achivement>(entity =>
            {
                entity.HasKey(e => e.MemberId);

                entity.ToTable("achivement");

                entity.Property(e => e.MemberId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("memberId");

                entity.Property(e => e.AchvSno)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("achvSno");

                entity.Property(e => e.Dcount)
                    .HasColumnName("DCount")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Lcmit)
                    .HasColumnType("money")
                    .HasColumnName("LCmit")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Lcnfrm)
                    .HasColumnType("money")
                    .HasColumnName("LCnfrm")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Lpoints)
                    .HasColumnName("LPoints")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Lvl1)
                    .HasColumnType("money")
                    .HasColumnName("lvl1");

                entity.Property(e => e.Lvl2)
                    .HasColumnType("money")
                    .HasColumnName("lvl2");

                entity.Property(e => e.Lvl3)
                    .HasColumnType("money")
                    .HasColumnName("lvl3");

                entity.Property(e => e.Pcmit).HasColumnName("pcmit");

                entity.Property(e => e.Plcount)
                    .HasColumnName("PLCount")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Ppoints)
                    .HasColumnName("PPoints")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Prcount)
                    .HasColumnName("PRCount")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Rcmit)
                    .HasColumnType("money")
                    .HasColumnName("RCmit")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Rcnfrm)
                    .HasColumnType("money")
                    .HasColumnName("RCnfrm")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Rpoints)
                    .HasColumnName("RPoints")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Tlcount)
                    .HasColumnName("TLCount")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Trcount)
                    .HasColumnName("TRCount")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.WalAddress)
                    .HasMaxLength(500)
                    .HasColumnName("walAddress");
            });

            modelBuilder.Entity<AddProduct>(entity =>
            {
                entity.ToTable("Add_Product");

                entity.Property(e => e.DateAdded).HasColumnType("date");

                entity.Property(e => e.Dpprise).HasColumnType("money");

                entity.Property(e => e.Field1)
                    .HasColumnType("money")
                    .HasColumnName("field1");

                entity.Property(e => e.Field2)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("field2");

                entity.Property(e => e.Field3).HasColumnName("field3");

                entity.Property(e => e.FinalPrice)
                    .HasColumnType("money")
                    .HasColumnName("Final_Price");

                entity.Property(e => e.Gst)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("GST");

                entity.Property(e => e.PacketType)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("Packet_Type");

                entity.Property(e => e.Price).HasColumnType("money");

                entity.Property(e => e.PriceWithGst)
                    .HasColumnType("money")
                    .HasColumnName("PriceWithGST");

                entity.Property(e => e.ProductBv)
                    .HasColumnType("money")
                    .HasColumnName("Product_BV");

                entity.Property(e => e.ProductDescription).HasColumnName("Product_Description");

                entity.Property(e => e.ProductId)
                    .HasMaxLength(100)
                    .HasColumnName("Product_Id");

                entity.Property(e => e.ProductImage)
                    .HasMaxLength(500)
                    .HasColumnName("Product_Image");

                entity.Property(e => e.ProductName)
                    .HasMaxLength(1000)
                    .HasColumnName("Product_Name");

                entity.Property(e => e.Weight)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<AddressInfo>(entity =>
            {
                entity.ToTable("AddressInfo");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Base58Address).HasColumnName("base58Address");

                entity.Property(e => e.Cdate)
                    .HasColumnType("datetime")
                    .HasColumnName("cdate");

                entity.Property(e => e.Cstatus).HasColumnName("cstatus");

                entity.Property(e => e.HexAddress).HasColumnName("hexAddress");

                entity.Property(e => e.MemberId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("memberId");

                entity.Property(e => e.PrivateKey).HasColumnName("privateKey");

                entity.Property(e => e.PublicKey).HasColumnName("publicKey");
            });

            modelBuilder.Entity<AdminAction>(entity =>
            {
                entity.ToTable("AdminAction");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Adate).HasColumnType("datetime");

                entity.Property(e => e.Amount).HasColumnType("money");

                entity.Property(e => e.Astatus).HasColumnName("astatus");

                entity.Property(e => e.Atype)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("atype");

                entity.Property(e => e.HashId)
                    .HasMaxLength(200)
                    .HasColumnName("hashId");
            });

            modelBuilder.Entity<AdminAddressInfo>(entity =>
            {
                entity.ToTable("AdminAddressInfo");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ContractAbi).HasColumnName("contractAbi");

                entity.Property(e => e.ContractAddress)
                    .HasMaxLength(200)
                    .HasColumnName("contractAddress");

                entity.Property(e => e.Ctype).HasMaxLength(100);

                entity.Property(e => e.DepositAddress).HasColumnName("depositAddress");

                entity.Property(e => e.TokenAddress)
                    .HasMaxLength(200)
                    .HasColumnName("tokenAddress");

                entity.Property(e => e.WithdrawalAddress).HasColumnName("withdrawalAddress");
            });

            modelBuilder.Entity<AlteredWithdrawal>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("alteredWithdrawal");

                entity.Property(e => e.Adate)
                    .HasColumnType("datetime")
                    .HasColumnName("adate");

                entity.Property(e => e.Atype)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("atype");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.MemberId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("memberId");

                entity.Property(e => e.WidId).HasColumnName("widId");

                entity.Property(e => e.WithdrawalUsdt)
                    .HasColumnType("money")
                    .HasColumnName("withdrawalUsdt");
            });

            modelBuilder.Entity<BuyToken>(entity =>
            {
                entity.ToTable("BuyToken");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ADate)
                    .HasColumnType("datetime")
                    .HasColumnName("aDate");

                entity.Property(e => e.AStatus).HasColumnName("aStatus");

                entity.Property(e => e.Amount).HasColumnType("money");

                entity.Property(e => e.CRate)
                    .HasColumnType("money")
                    .HasColumnName("cRate");

                entity.Property(e => e.HashId)
                    .HasMaxLength(200)
                    .HasColumnName("hashId");

                entity.Property(e => e.MemberId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("memberId");

                entity.Property(e => e.Nonce).HasColumnName("nonce");

                entity.Property(e => e.RDate)
                    .HasColumnType("datetime")
                    .HasColumnName("rDate");

                entity.Property(e => e.TokenAmount)
                    .HasColumnType("money")
                    .HasColumnName("tokenAmount");

                entity.Property(e => e.TokenName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("tokenName");
            });

            modelBuilder.Entity<ClientSystemInfo>(entity =>
            {
                entity.ToTable("ClientSystemInfo");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AType)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("aType");

                entity.Property(e => e.ClientIp).HasColumnName("clientIp");

                entity.Property(e => e.Error).HasColumnName("error");

                entity.Property(e => e.ErrorMessage).HasColumnName("errorMessage");

                entity.Property(e => e.MemberId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("memberId");

                entity.Property(e => e.Rdate)
                    .HasColumnType("datetime")
                    .HasColumnName("rdate");
            });

            modelBuilder.Entity<CmitRequest>(entity =>
            {
                entity.HasKey(e => e.CmitSno);

                entity.ToTable("cmitRequests");

                entity.Property(e => e.CmitSno).HasColumnName("cmitSno");

                entity.Property(e => e.Bymemberid)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("bymemberid");

                entity.Property(e => e.CmitId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("cmitId");

                entity.Property(e => e.CmitReqAmount)
                    .HasColumnType("money")
                    .HasColumnName("cmitReqAmount");

                entity.Property(e => e.CmitSdate)
                    .HasColumnType("datetime")
                    .HasColumnName("cmitSDate");

                entity.Property(e => e.CmitStatus).HasColumnName("cmitStatus");

                entity.Property(e => e.HashId).HasColumnName("hashId");

                entity.Property(e => e.MemberId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("memberId");

                entity.Property(e => e.PinAmount).HasColumnType("money");

                entity.Property(e => e.PlanType)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.RoiDate).HasColumnType("datetime");

                entity.Property(e => e.RoiRate)
                    .HasColumnType("money")
                    .HasColumnName("roiRate");

                entity.Property(e => e.RoiReturn)
                    .HasColumnType("money")
                    .HasColumnName("roiReturn");

                entity.Property(e => e.RoiTill)
                    .HasColumnType("money")
                    .HasColumnName("roiTill");

                entity.Property(e => e.TopupWallet)
                    .HasColumnType("money")
                    .HasColumnName("topupWallet");

                entity.Property(e => e.UsdtWalelt)
                    .HasColumnType("money")
                    .HasColumnName("usdtWalelt");

                entity.Property(e => e.WalletType)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ComplainMail>(entity =>
            {
                entity.HasKey(e => e.MailSno);

                entity.ToTable("complainMail");

                entity.Property(e => e.MailSno).HasColumnName("mailSno");

                entity.Property(e => e.MaiBody)
                    .IsUnicode(false)
                    .HasColumnName("maiBody");

                entity.Property(e => e.MailId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("mailId");

                entity.Property(e => e.MailReply)
                    .IsUnicode(false)
                    .HasColumnName("mailReply");

                entity.Property(e => e.MailStatus).HasColumnName("mailStatus");

                entity.Property(e => e.MailSubject)
                    .IsUnicode(false)
                    .HasColumnName("mailSubject");

                entity.Property(e => e.MemberId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("memberId");

                entity.Property(e => e.ToMemberId)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ContactUsTable>(entity =>
            {
                entity.HasKey(e => e.Sono);

                entity.ToTable("ContactUs_table");

                entity.Property(e => e.Sono).HasColumnName("sono");

                entity.Property(e => e.City)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ContactDate).HasColumnType("datetime");

                entity.Property(e => e.EmailId).HasMaxLength(100);

                entity.Property(e => e.MemName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("memName");

                entity.Property(e => e.Mobile)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RStatus).HasColumnName("rStatus");

                entity.Property(e => e.Smg)
                    .IsUnicode(false)
                    .HasColumnName("SMG");

                entity.Property(e => e.Subject)
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ContryCode>(entity =>
            {
                entity.ToTable("ContryCode");

                entity.Property(e => e.ConCode)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("conCode");

                entity.Property(e => e.Country).HasMaxLength(255);

                entity.Property(e => e.CountryCode).HasMaxLength(255);

                entity.Property(e => e.InternationalDialing).HasMaxLength(255);
            });

            modelBuilder.Entity<FundAmount>(entity =>
            {
                entity.ToTable("FundAmount");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AdminCharge)
                    .HasColumnType("money")
                    .HasColumnName("adminCharge");

                entity.Property(e => e.Amount).HasColumnType("money");

                entity.Property(e => e.ApproveDate)
                    .HasColumnType("datetime")
                    .HasColumnName("approveDate");

                entity.Property(e => e.ByMemberId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("byMemberId");

                entity.Property(e => e.Details).HasColumnName("details");

                entity.Property(e => e.Fdate)
                    .HasColumnType("datetime")
                    .HasColumnName("fdate");

                entity.Property(e => e.FinalAmount)
                    .HasColumnType("money")
                    .HasColumnName("finalAmount");

                entity.Property(e => e.FromAddress)
                    .HasMaxLength(200)
                    .HasColumnName("fromAddress");

                entity.Property(e => e.Ftype)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("ftype");

                entity.Property(e => e.HashId).HasColumnName("hashId");

                entity.Property(e => e.MemberId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("memberId");

                entity.Property(e => e.RefId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("refId");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.ToAddress)
                    .HasMaxLength(200)
                    .HasColumnName("toAddress");

                entity.Property(e => e.TokenAmount)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("tokenAmount");

                entity.Property(e => e.WType)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("wType");
            });

            modelBuilder.Entity<InspBiller>(entity =>
            {
                entity.ToTable("INSP_Billers");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.BillerId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("billerId");

                entity.Property(e => e.BillerName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("billerName");

                entity.Property(e => e.CategoryKey)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("categoryKey");

                entity.Property(e => e.CategoryName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("categoryName");

                entity.Property(e => e.IconUrl)
                    .HasMaxLength(200)
                    .HasColumnName("iconUrl");

                entity.Property(e => e.IsAvailable).HasColumnName("isAvailable");
            });

            modelBuilder.Entity<LevelSponsor>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("LevelSponsor");

                entity.Property(e => e.Amount)
                    .HasColumnType("money")
                    .HasColumnName("amount");

                entity.Property(e => e.DirectRequired).HasColumnName("directRequired");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.Lvl).HasColumnName("lvl");

                entity.Property(e => e.MemberId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("memberId");

                entity.Property(e => e.SpoId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("spoId");

                entity.Property(e => e.Sstatus).HasColumnName("sstatus");
            });

            modelBuilder.Entity<ListCircle>(entity =>
            {
                entity.ToTable("ListCircle");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Cname)
                    .HasMaxLength(100)
                    .HasColumnName("CName");
            });

            modelBuilder.Entity<ListCountry>(entity =>
            {
                entity.HasKey(e => e.Cid);

                entity.ToTable("ListCountry");

                entity.Property(e => e.Cid).HasColumnName("CId");

                entity.Property(e => e.Cname)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CName");
            });

            modelBuilder.Entity<ListLevel>(entity =>
            {
                entity.ToTable("ListLevel");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Amount)
                    .HasColumnType("money")
                    .HasColumnName("amount");

                entity.Property(e => e.DirectReq).HasColumnName("directReq");

                entity.Property(e => e.ExtAmount)
                    .HasColumnType("money")
                    .HasColumnName("extAmount");

                entity.Property(e => e.Lrate)
                    .HasColumnType("money")
                    .HasColumnName("LRate");

                entity.Property(e => e.Lvl).HasColumnName("lvl");
            });

            modelBuilder.Entity<ListMatrixLvl>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("ListMatrixLvl");

                entity.Property(e => e.Amount)
                    .HasColumnType("money")
                    .HasColumnName("amount");

                entity.Property(e => e.Downline).HasColumnName("downline");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.Lvl).HasColumnName("lvl");

                entity.Property(e => e.MatrixNo).HasColumnName("matrixNo");
            });

            modelBuilder.Entity<ListOperator>(entity =>
            {
                entity.ToTable("ListOperator");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.OpName).HasMaxLength(100);

                entity.Property(e => e.OpType).HasMaxLength(100);
            });

            modelBuilder.Entity<ListState>(entity =>
            {
                entity.HasKey(e => e.Sid);

                entity.ToTable("ListState");

                entity.Property(e => e.Sid).HasColumnName("SID");

                entity.Property(e => e.Sname)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("SName");
            });

            modelBuilder.Entity<Matrix2>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Matrix2");

                entity.Property(e => e.Birth).HasColumnName("birth");

                entity.Property(e => e.Dcount).HasColumnName("dcount");

                entity.Property(e => e.Etype)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("etype");

                entity.Property(e => e.Fill).HasColumnName("fill");

                entity.Property(e => e.Icount).HasColumnName("icount");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.IncomeTo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("incomeTo");

                entity.Property(e => e.MatrixNo).HasColumnName("matrixNo");

                entity.Property(e => e.Mdate)
                    .HasColumnType("datetime")
                    .HasColumnName("mdate");

                entity.Property(e => e.MemberId)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Mststus).HasColumnName("mststus");

                entity.Property(e => e.Position).HasColumnName("position");

                entity.Property(e => e.RefId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("refId");

                entity.Property(e => e.RefTblId).HasColumnName("refTblId");

                entity.Property(e => e.RootId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("rootId");

                entity.Property(e => e.SpoIncome)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("spoIncome");
            });

            modelBuilder.Entity<MatrixIncome>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("MatrixIncome");

                entity.Property(e => e.AdminCharge)
                    .HasColumnType("money")
                    .HasColumnName("adminCharge");

                entity.Property(e => e.Amount)
                    .HasColumnType("money")
                    .HasColumnName("amount");

                entity.Property(e => e.ByMemberId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("byMemberId");

                entity.Property(e => e.FinalAmount)
                    .HasColumnType("money")
                    .HasColumnName("finalAmount");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.IncomeTo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("incomeTo");

                entity.Property(e => e.MatrixNo).HasColumnName("matrixNo");

                entity.Property(e => e.MatrixType)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("matrixType");

                entity.Property(e => e.Mdate)
                    .HasColumnType("datetime")
                    .HasColumnName("mdate");

                entity.Property(e => e.MemberId)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Mststus).HasColumnName("mststus");

                entity.Property(e => e.Position).HasColumnName("position");

                entity.Property(e => e.Tds)
                    .HasColumnType("money")
                    .HasColumnName("tds");
            });

            modelBuilder.Entity<MatrixInfo>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("MatrixInfo");

                entity.Property(e => e.Amount)
                    .HasColumnType("money")
                    .HasColumnName("amount");

                entity.Property(e => e.Etype)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("etype");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.Income)
                    .HasColumnType("money")
                    .HasColumnName("income");

                entity.Property(e => e.LvlAmount)
                    .HasColumnType("money")
                    .HasColumnName("lvlAmount");

                entity.Property(e => e.Matrix).HasColumnName("matrix");

                entity.Property(e => e.MatrixName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("matrixName");

                entity.Property(e => e.MatrixNo).HasColumnName("matrixNo");

                entity.Property(e => e.Total)
                    .HasColumnType("money")
                    .HasColumnName("total");

                entity.Property(e => e.Upgrade)
                    .HasColumnType("money")
                    .HasColumnName("upgrade");
            });

            modelBuilder.Entity<MemPageOlog>(entity =>
            {
                entity.ToTable("MemPageOLog");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.MemberId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("memberId");

                entity.Property(e => e.ODate)
                    .HasColumnType("datetime")
                    .HasColumnName("oDate");

                entity.Property(e => e.PageName)
                    .HasMaxLength(200)
                    .HasColumnName("pageName");
            });

            modelBuilder.Entity<MemberDirect>(entity =>
            {
                entity.HasKey(e => e.MemberId);

                entity.ToTable("memberDirect");

                entity.Property(e => e.MemberId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("memberId");

                entity.Property(e => e.ChkL).HasColumnName("chkL");

                entity.Property(e => e.ChkR).HasColumnName("chkR");

                entity.Property(e => e.ChkSno)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("chkSno");
            });

            modelBuilder.Entity<MemberInfo>(entity =>
            {
                entity.HasKey(e => e.MemId);

                entity.ToTable("memberInfo");

                entity.Property(e => e.MemId).HasColumnName("memId");

                entity.Property(e => e.BlockName).HasColumnName("blockName");

                entity.Property(e => e.ClubName)
                    .HasMaxLength(100)
                    .HasColumnName("clubName");

                entity.Property(e => e.ClubNo).HasColumnName("clubNo");

                entity.Property(e => e.Idnumber)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("IDNumber");

                entity.Property(e => e.Idtype)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("IDType");

                entity.Property(e => e.KycDocumentNo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("KycDocumentNO");

                entity.Property(e => e.KycDocumentNoothr)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("KycDocumentNOOthr");

                entity.Property(e => e.KycDocumentname)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.KycDocumentnameothr)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Kycimgname)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.KycimgnameOther)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Kycstatus)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MemAcAtm)
                    .HasMaxLength(500)
                    .HasColumnName("memAcATM");

                entity.Property(e => e.MemAcBank)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("memAcBank");

                entity.Property(e => e.MemAcBranch)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("memAcBranch");

                entity.Property(e => e.MemAcCity)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("memAcCity");

                entity.Property(e => e.MemAcIfsc)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("memAcIfsc");

                entity.Property(e => e.MemAcName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("memAcName");

                entity.Property(e => e.MemAcNo)
                    .HasMaxLength(200)
                    .HasColumnName("memAcNo");

                entity.Property(e => e.MemAcType)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("memAcType");

                entity.Property(e => e.MemActive).HasColumnName("memActive");

                entity.Property(e => e.MemAddress)
                    .HasMaxLength(500)
                    .HasColumnName("memAddress");

                entity.Property(e => e.MemAdharNo)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("memAdharNo");

                entity.Property(e => e.MemCity)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("memCity");

                entity.Property(e => e.MemCountry)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("memCountry");

                entity.Property(e => e.MemDob)
                    .HasColumnType("datetime")
                    .HasColumnName("memDob");

                entity.Property(e => e.MemEmail)
                    .HasMaxLength(500)
                    .HasColumnName("memEmail");

                entity.Property(e => e.MemFather)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("memFather");

                entity.Property(e => e.MemGender)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("memGender");

                entity.Property(e => e.MemLogId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("memLogId");

                entity.Property(e => e.MemLogPass)
                    .HasMaxLength(500)
                    .HasColumnName("memLogPass");

                entity.Property(e => e.MemMobile)
                    .HasMaxLength(500)
                    .HasColumnName("memMobile");

                entity.Property(e => e.MemName)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasColumnName("memName");

                entity.Property(e => e.MemPackage)
                    .HasMaxLength(300)
                    .HasColumnName("memPackage");

                entity.Property(e => e.MemPackageValue)
                    .HasColumnType("money")
                    .HasColumnName("memPackageValue");

                entity.Property(e => e.MemPanNo)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("memPanNo");

                entity.Property(e => e.MemRank).HasColumnName("memRank");

                entity.Property(e => e.MemReg)
                    .HasColumnType("datetime")
                    .HasColumnName("memReg");

                entity.Property(e => e.MemState)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("memState");

                entity.Property(e => e.MemberId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("memberId");

                entity.Property(e => e.MemniminiCity)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("memniminiCity");

                entity.Property(e => e.MemnominiAdd)
                    .HasMaxLength(500)
                    .HasColumnName("memnominiAdd");

                entity.Property(e => e.MemnominiName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("memnominiName");

                entity.Property(e => e.MemnominiPhone)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("memnominiPhone");

                entity.Property(e => e.MemnominiRel)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("memnominiRel");

                entity.Property(e => e.Memposno).HasColumnName("memposno");

                entity.Property(e => e.PanApproveDate).HasColumnType("datetime");

                entity.Property(e => e.PinCode)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.PosDate).HasColumnType("datetime");

                entity.Property(e => e.ProfileImage)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.RefId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("refId");

                entity.Property(e => e.RefPos)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("refPos");

                entity.Property(e => e.ResidencyIdnumber)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("ResidencyIDNumber");

                entity.Property(e => e.ResidencyIdtype)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("ResidencyIDType");

                entity.Property(e => e.SpoId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("spoId");
            });

            modelBuilder.Entity<MemberInfoBackup>(entity =>
            {
                entity.HasKey(e => e.MemId);

                entity.ToTable("memberInfoBackup");

                entity.Property(e => e.MemId).HasColumnName("memId");

                entity.Property(e => e.BlockName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("blockName");

                entity.Property(e => e.ClubName)
                    .HasMaxLength(100)
                    .HasColumnName("clubName");

                entity.Property(e => e.ClubNo).HasColumnName("clubNo");

                entity.Property(e => e.Idnumber)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("IDNumber");

                entity.Property(e => e.Idtype)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("IDType");

                entity.Property(e => e.KycDocumentNo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("KycDocumentNO");

                entity.Property(e => e.KycDocumentNoothr)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("KycDocumentNOOthr");

                entity.Property(e => e.KycDocumentname)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.KycDocumentnameothr)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Kycimgname)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.KycimgnameOther)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Kycstatus)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MemAcAtm)
                    .HasMaxLength(500)
                    .HasColumnName("memAcATM");

                entity.Property(e => e.MemAcBank)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("memAcBank");

                entity.Property(e => e.MemAcBranch)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("memAcBranch");

                entity.Property(e => e.MemAcCity)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("memAcCity");

                entity.Property(e => e.MemAcIfsc)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("memAcIfsc");

                entity.Property(e => e.MemAcName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("memAcName");

                entity.Property(e => e.MemAcNo)
                    .HasMaxLength(200)
                    .HasColumnName("memAcNo");

                entity.Property(e => e.MemAcType)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("memAcType");

                entity.Property(e => e.MemActive).HasColumnName("memActive");

                entity.Property(e => e.MemAddress)
                    .HasMaxLength(500)
                    .HasColumnName("memAddress");

                entity.Property(e => e.MemAdharNo)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("memAdharNo");

                entity.Property(e => e.MemCity)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("memCity");

                entity.Property(e => e.MemCountry)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("memCountry");

                entity.Property(e => e.MemDob)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("memDob");

                entity.Property(e => e.MemEmail)
                    .HasMaxLength(500)
                    .HasColumnName("memEmail");

                entity.Property(e => e.MemFather)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("memFather");

                entity.Property(e => e.MemGender)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("memGender");

                entity.Property(e => e.MemLogId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("memLogId");

                entity.Property(e => e.MemLogPass)
                    .HasMaxLength(500)
                    .HasColumnName("memLogPass");

                entity.Property(e => e.MemMobile)
                    .HasMaxLength(500)
                    .HasColumnName("memMobile");

                entity.Property(e => e.MemName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("memName");

                entity.Property(e => e.MemPackage)
                    .HasMaxLength(300)
                    .HasColumnName("memPackage");

                entity.Property(e => e.MemPackageValue)
                    .HasColumnType("money")
                    .HasColumnName("memPackageValue");

                entity.Property(e => e.MemPanNo)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("memPanNo");

                entity.Property(e => e.MemRank).HasColumnName("memRank");

                entity.Property(e => e.MemReg)
                    .HasColumnType("datetime")
                    .HasColumnName("memReg");

                entity.Property(e => e.MemState)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("memState");

                entity.Property(e => e.MemberId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("memberId");

                entity.Property(e => e.MemniminiCity)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("memniminiCity");

                entity.Property(e => e.MemnominiAdd)
                    .HasMaxLength(500)
                    .HasColumnName("memnominiAdd");

                entity.Property(e => e.MemnominiName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("memnominiName");

                entity.Property(e => e.MemnominiPhone)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("memnominiPhone");

                entity.Property(e => e.MemnominiRel)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("memnominiRel");

                entity.Property(e => e.Memposno).HasColumnName("memposno");

                entity.Property(e => e.PanApproveDate).HasColumnType("datetime");

                entity.Property(e => e.PinCode)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.PosDate).HasColumnType("datetime");

                entity.Property(e => e.ProfileImage)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.RefId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("refId");

                entity.Property(e => e.RefPos)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("refPos");

                entity.Property(e => e.ResidencyIdnumber)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("ResidencyIDNumber");

                entity.Property(e => e.ResidencyIdtype)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("ResidencyIDType");

                entity.Property(e => e.SpoId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("spoId");
            });

            modelBuilder.Entity<MemberInfoHistory>(entity =>
            {
                entity.HasKey(e => e.MemId);

                entity.ToTable("memberInfoHistory");

                entity.Property(e => e.MemId).HasColumnName("memId");

                entity.Property(e => e.BlockName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("blockName");

                entity.Property(e => e.ClubName)
                    .HasMaxLength(100)
                    .HasColumnName("clubName");

                entity.Property(e => e.ClubNo).HasColumnName("clubNo");

                entity.Property(e => e.Idnumber)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("IDNumber");

                entity.Property(e => e.Idtype)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("IDType");

                entity.Property(e => e.KycDocumentNo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("KycDocumentNO");

                entity.Property(e => e.KycDocumentNoothr)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("KycDocumentNOOthr");

                entity.Property(e => e.KycDocumentname)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.KycDocumentnameothr)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Kycimgname)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.KycimgnameOther)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Kycstatus)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MemAcAtm)
                    .HasMaxLength(500)
                    .HasColumnName("memAcATM");

                entity.Property(e => e.MemAcBank)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("memAcBank");

                entity.Property(e => e.MemAcBranch)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("memAcBranch");

                entity.Property(e => e.MemAcCity)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("memAcCity");

                entity.Property(e => e.MemAcIfsc)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("memAcIfsc");

                entity.Property(e => e.MemAcName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("memAcName");

                entity.Property(e => e.MemAcNo)
                    .HasMaxLength(200)
                    .HasColumnName("memAcNo");

                entity.Property(e => e.MemAcType)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("memAcType");

                entity.Property(e => e.MemActive).HasColumnName("memActive");

                entity.Property(e => e.MemAddress)
                    .HasMaxLength(500)
                    .HasColumnName("memAddress");

                entity.Property(e => e.MemAdharNo)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("memAdharNo");

                entity.Property(e => e.MemCity)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("memCity");

                entity.Property(e => e.MemCountry)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("memCountry");

                entity.Property(e => e.MemDob)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("memDob");

                entity.Property(e => e.MemEmail)
                    .HasMaxLength(500)
                    .HasColumnName("memEmail");

                entity.Property(e => e.MemFather)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("memFather");

                entity.Property(e => e.MemGender)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("memGender");

                entity.Property(e => e.MemLogId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("memLogId");

                entity.Property(e => e.MemLogPass)
                    .HasMaxLength(500)
                    .HasColumnName("memLogPass");

                entity.Property(e => e.MemMobile)
                    .HasMaxLength(500)
                    .HasColumnName("memMobile");

                entity.Property(e => e.MemName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("memName");

                entity.Property(e => e.MemPackage)
                    .HasMaxLength(300)
                    .HasColumnName("memPackage");

                entity.Property(e => e.MemPackageValue)
                    .HasColumnType("money")
                    .HasColumnName("memPackageValue");

                entity.Property(e => e.MemPanNo)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("memPanNo");

                entity.Property(e => e.MemRank).HasColumnName("memRank");

                entity.Property(e => e.MemReg)
                    .HasColumnType("datetime")
                    .HasColumnName("memReg");

                entity.Property(e => e.MemState)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("memState");

                entity.Property(e => e.MemberId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("memberId");

                entity.Property(e => e.MemniminiCity)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("memniminiCity");

                entity.Property(e => e.MemnominiAdd)
                    .HasMaxLength(500)
                    .HasColumnName("memnominiAdd");

                entity.Property(e => e.MemnominiName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("memnominiName");

                entity.Property(e => e.MemnominiPhone)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("memnominiPhone");

                entity.Property(e => e.MemnominiRel)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("memnominiRel");

                entity.Property(e => e.Memposno).HasColumnName("memposno");

                entity.Property(e => e.PanApproveDate).HasColumnType("datetime");

                entity.Property(e => e.PinCode)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.PosDate).HasColumnType("datetime");

                entity.Property(e => e.ProfileImage)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.RefId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("refId");

                entity.Property(e => e.RefPos)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("refPos");

                entity.Property(e => e.ResidencyIdnumber)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("ResidencyIDNumber");

                entity.Property(e => e.ResidencyIdtype)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("ResidencyIDType");

                entity.Property(e => e.SpoId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("spoId");
            });

            modelBuilder.Entity<MemberNotification>(entity =>
            {
                entity.ToTable("MemberNotification");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Greeting)
                    .HasMaxLength(200)
                    .HasColumnName("greeting");

                entity.Property(e => e.MemberId)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Ndate).HasColumnType("datetime");

                entity.Property(e => e.Ntype)
                    .HasMaxLength(100)
                    .HasColumnName("NType");
            });

            modelBuilder.Entity<MemberWidInfo>(entity =>
            {
                entity.ToTable("memberWidInfo");

                entity.Property(e => e.AdminApprovedDate).HasColumnType("datetime");

                entity.Property(e => e.AdminCharge).HasColumnType("money");

                entity.Property(e => e.AmountBeforeTds)
                    .HasColumnType("money")
                    .HasColumnName("AmountBeforeTDS");

                entity.Property(e => e.BinaryIncome).HasColumnType("money");

                entity.Property(e => e.CmitId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("cmitId");

                entity.Property(e => e.Details)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("details");

                entity.Property(e => e.DirectIncome).HasColumnType("money");

                entity.Property(e => e.FinalAmount)
                    .HasColumnType("money")
                    .HasColumnName("finalAmount");

                entity.Property(e => e.HashId).IsUnicode(false);

                entity.Property(e => e.MemberId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("memberId");

                entity.Property(e => e.OrderId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("orderId");

                entity.Property(e => e.PaymentMode)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("paymentMode");

                entity.Property(e => e.Roiincome)
                    .HasColumnType("money")
                    .HasColumnName("ROIIncome");

                entity.Property(e => e.Status)
                    .IsUnicode(false)
                    .HasColumnName("status");

                entity.Property(e => e.Tds)
                    .HasColumnType("money")
                    .HasColumnName("TDS");

                entity.Property(e => e.Totalamount)
                    .HasColumnType("money")
                    .HasColumnName("TOTALAmount");

                entity.Property(e => e.TransactionNo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("transactionNo");

                entity.Property(e => e.Wdldate)
                    .HasColumnType("datetime")
                    .HasColumnName("WDLDate");

                entity.Property(e => e.Withdrawal)
                    .HasColumnType("money")
                    .HasColumnName("withdrawal");

                entity.Property(e => e.Wtype)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("wtype");
            });

            modelBuilder.Entity<MemberWidInfoBackup>(entity =>
            {
                entity.ToTable("memberWidInfoBackup");

                entity.Property(e => e.AdminApprovedDate).HasColumnType("datetime");

                entity.Property(e => e.AdminCharge).HasColumnType("money");

                entity.Property(e => e.AmountBeforeTds)
                    .HasColumnType("money")
                    .HasColumnName("AmountBeforeTDS");

                entity.Property(e => e.BinaryIncome).HasColumnType("money");

                entity.Property(e => e.Details)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("details");

                entity.Property(e => e.DirectIncome).HasColumnType("money");

                entity.Property(e => e.FinalAmount)
                    .HasColumnType("money")
                    .HasColumnName("finalAmount");

                entity.Property(e => e.HashId).IsUnicode(false);

                entity.Property(e => e.MemberId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("memberId");

                entity.Property(e => e.OrderId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("orderId");

                entity.Property(e => e.PaymentMode)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("paymentMode");

                entity.Property(e => e.Roiincome)
                    .HasColumnType("money")
                    .HasColumnName("ROIIncome");

                entity.Property(e => e.Status)
                    .IsUnicode(false)
                    .HasColumnName("status");

                entity.Property(e => e.Tds)
                    .HasColumnType("money")
                    .HasColumnName("TDS");

                entity.Property(e => e.Totalamount)
                    .HasColumnType("money")
                    .HasColumnName("TOTALAmount");

                entity.Property(e => e.TransactionNo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("transactionNo");

                entity.Property(e => e.Wdldate)
                    .HasColumnType("datetime")
                    .HasColumnName("WDLDate");

                entity.Property(e => e.Withdrawal)
                    .HasColumnType("money")
                    .HasColumnName("withdrawal");

                entity.Property(e => e.Wtype)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("wtype");
            });

            modelBuilder.Entity<OnlineService>(entity =>
            {
                entity.HasKey(e => e.Memid)
                    .HasName("PK__OnlineSe__338C534574639F54");

                entity.Property(e => e.Memid).HasColumnName("memid");

                entity.Property(e => e.Account)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("account");

                entity.Property(e => e.Amount)
                    .HasColumnType("money")
                    .HasColumnName("amount");

                entity.Property(e => e.ApiCharge)
                    .HasColumnType("money")
                    .HasColumnName("apiCharge");

                entity.Property(e => e.ClosingBalance)
                    .HasColumnType("money")
                    .HasColumnName("Closing_balance");

                entity.Property(e => e.IsPaid).HasColumnName("isPaid");

                entity.Property(e => e.Memberid)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Messagess)
                    .IsUnicode(false)
                    .HasColumnName("messagess");

                entity.Property(e => e.Odate)
                    .HasColumnType("datetime")
                    .HasColumnName("odate");

                entity.Property(e => e.OperatorRefNo)
                    .IsUnicode(false)
                    .HasColumnName("Operator_ref_no");

                entity.Property(e => e.Operatorcode)
                    .IsUnicode(false)
                    .HasColumnName("operatorcode");

                entity.Property(e => e.OprtnTime)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.OurTxnNo)
                    .IsUnicode(false)
                    .HasColumnName("Our_txn_no");

                entity.Property(e => e.ServiceType)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Status).IsUnicode(false);

                entity.Property(e => e.UsdAmount)
                    .HasColumnType("money")
                    .HasColumnName("usdAmount");

                entity.Property(e => e.UserTxnNo)
                    .IsUnicode(false)
                    .HasColumnName("User_txn_no");
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.ToTable("Order_Details");

                entity.Property(e => e.AmountWithGst)
                    .HasColumnType("money")
                    .HasColumnName("Amount_With_GST");

                entity.Property(e => e.FinalPrice)
                    .HasColumnType("money")
                    .HasColumnName("Final_Price");

                entity.Property(e => e.Gst).HasColumnName("GST");

                entity.Property(e => e.GstAmount)
                    .HasColumnType("money")
                    .HasColumnName("GST_Amount");

                entity.Property(e => e.Memberid)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("memberid");

                entity.Property(e => e.OrderId)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Price).HasColumnType("money");

                entity.Property(e => e.ProductBv)
                    .HasColumnType("money")
                    .HasColumnName("Product_BV");

                entity.Property(e => e.ProductId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("Product_Id");

                entity.Property(e => e.ProductName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("Product_Name");

                entity.Property(e => e.UserType)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("User_Type");
            });

            modelBuilder.Entity<PinDetail>(entity =>
            {
                entity.HasKey(e => e.Sono);

                entity.ToTable("Pin_Detail");

                entity.Property(e => e.Bv)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("BV");

                entity.Property(e => e.Bvpoint)
                    .HasColumnType("money")
                    .HasColumnName("BVPoint");

                entity.Property(e => e.PackagePercent).HasColumnType("money");

                entity.Property(e => e.PairValue).HasColumnType("money");

                entity.Property(e => e.PinAmout).HasColumnType("money");

                entity.Property(e => e.PinDetail1)
                    .HasMaxLength(100)
                    .HasColumnName("PinDetail");

                entity.Property(e => e.PinType)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ReturnAmount).HasColumnType("money");
            });

            modelBuilder.Entity<Poster>(entity =>
            {
                entity.ToTable("Poster");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Field1)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Field2).HasColumnType("money");

                entity.Property(e => e.Field3).HasColumnType("datetime");

                entity.Property(e => e.PosterImage)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Posterdate).HasColumnType("datetime");

                entity.Property(e => e.Posterid)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Postername)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ProductCat>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("ProductCat");

                entity.Property(e => e.CatName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.IsActive).HasColumnName("isActive");

                entity.Property(e => e.LastUpdate)
                    .HasColumnType("datetime")
                    .HasColumnName("lastUpdate");
            });

            modelBuilder.Entity<ProductRequest>(entity =>
            {
                entity.ToTable("Product_Request");

                entity.Property(e => e.ApprovedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Approved_Date");

                entity.Property(e => e.DeliveryAddress)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("Delivery_Address");

                entity.Property(e => e.FinalAmount)
                    .HasColumnType("money")
                    .HasColumnName("Final_Amount");

                entity.Property(e => e.Memberid)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("memberid");

                entity.Property(e => e.OrderId)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.OrderNotes).HasColumnName("Order_Notes");

                entity.Property(e => e.OrderStatus).HasColumnName("Order_Status");

                entity.Property(e => e.ProductBv)
                    .HasColumnType("money")
                    .HasColumnName("Product_BV");

                entity.Property(e => e.RequestDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Request_Date");

                entity.Property(e => e.TotalGst)
                    .HasColumnType("money")
                    .HasColumnName("Total_GST");

                entity.Property(e => e.TotalPrice)
                    .HasColumnType("money")
                    .HasColumnName("Total_Price");

                entity.Property(e => e.TotalProducts).HasColumnName("Total_Products");

                entity.Property(e => e.UserType)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("User_Type");
            });

            modelBuilder.Entity<ProductSubCat>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("ProductSubCat");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.IsActive).HasColumnName("isActive");

                entity.Property(e => e.LastUpdate)
                    .HasColumnType("datetime")
                    .HasColumnName("lastUpdate");

                entity.Property(e => e.SubCatName)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Reward>(entity =>
            {
                entity.ToTable("Reward");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Adate)
                    .HasColumnType("datetime")
                    .HasColumnName("ADate");

                entity.Property(e => e.AdminCrg).HasColumnType("money");

                entity.Property(e => e.FinalIncome).HasColumnType("money");

                entity.Property(e => e.HashId)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("hashId");

                entity.Property(e => e.ICount).HasColumnName("iCount");

                entity.Property(e => e.Memberid)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("memberid");

                entity.Property(e => e.Memrank)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("memrank");

                entity.Property(e => e.Rdate)
                    .HasColumnType("datetime")
                    .HasColumnName("RDate");

                entity.Property(e => e.Rdescription)
                    .IsUnicode(false)
                    .HasColumnName("RDescription");

                entity.Property(e => e.Retrunamount).HasColumnType("money");

                entity.Property(e => e.Rewardid).HasColumnName("rewardid");

                entity.Property(e => e.Rpcharge)
                    .HasColumnType("money")
                    .HasColumnName("rpcharge");

                entity.Property(e => e.RwrdIncome).HasColumnType("money");

                entity.Property(e => e.Tds).HasColumnType("money");
            });

            modelBuilder.Entity<RewardInfo>(entity =>
            {
                entity.ToTable("RewardInfo");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Direct).HasColumnName("DIRECT");

                entity.Property(e => e.RId).HasColumnName("R_ID");

                entity.Property(e => e.Reward)
                    .HasColumnType("money")
                    .HasColumnName("REWARD");

                entity.Property(e => e.Salary)
                    .HasColumnType("money")
                    .HasColumnName("SALARY");

                entity.Property(e => e.SelfPackage)
                    .HasColumnType("money")
                    .HasColumnName("SELF_PACKAGE");

                entity.Property(e => e.TeamSize).HasColumnName("TEAM_SIZE");

                entity.Property(e => e.TeamVolume)
                    .HasColumnType("money")
                    .HasColumnName("TEAM_VOLUME");

                entity.Property(e => e.Type)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("TYPE");
            });

            modelBuilder.Entity<RewardReturn>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("RewardReturn");

                entity.Property(e => e.AdminCharge)
                    .HasColumnType("money")
                    .HasColumnName("adminCharge");

                entity.Property(e => e.Amount)
                    .HasColumnType("money")
                    .HasColumnName("amount");

                entity.Property(e => e.FinalAmount)
                    .HasColumnType("money")
                    .HasColumnName("finalAmount");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.MemberId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("memberId");

                entity.Property(e => e.RId).HasColumnName("rId");

                entity.Property(e => e.RType)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("rType");

                entity.Property(e => e.Rdate)
                    .HasColumnType("datetime")
                    .HasColumnName("rdate");

                entity.Property(e => e.RewardId).HasColumnName("rewardId");

                entity.Property(e => e.Tds)
                    .HasColumnType("money")
                    .HasColumnName("tds");
            });

            modelBuilder.Entity<SmsAdmin>(entity =>
            {
                entity.HasKey(e => e.MsgId);

                entity.ToTable("smsAdmin");

                entity.Property(e => e.MsgId).HasColumnName("msgId");

                entity.Property(e => e.ContectNo)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("contectNo");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Message)
                    .IsUnicode(false)
                    .HasColumnName("message");

                entity.Property(e => e.Status)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("status");
            });

            modelBuilder.Entity<StakeAmount>(entity =>
            {
                entity.ToTable("StakeAmount");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Amount)
                    .HasColumnType("money")
                    .HasColumnName("amount");

                entity.Property(e => e.LiveRate).HasColumnType("money");

                entity.Property(e => e.Mdate).HasColumnType("datetime");

                entity.Property(e => e.MemberId)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Rate).HasColumnType("money");

                entity.Property(e => e.ReturnAmount).HasColumnType("money");

                entity.Property(e => e.Sdate).HasColumnType("datetime");

                entity.Property(e => e.TokenAmount).HasColumnType("money");
            });

            modelBuilder.Entity<StakeInfo>(entity =>
            {
                entity.ToTable("StakeInfo");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.PlanDetail)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PlanName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Rate).HasColumnType("money");
            });

            modelBuilder.Entity<TableCart>(entity =>
            {
                entity.ToTable("Table_Cart");

                entity.Property(e => e.DateAdded).HasColumnType("date");

                entity.Property(e => e.Memberid)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("memberid");

                entity.Property(e => e.ProductId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("Product_Id");

                entity.Property(e => e.UserType)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("User_Type");
            });

            modelBuilder.Entity<TableSupport>(entity =>
            {
                entity.HasKey(e => e.Srno);

                entity.ToTable("TableSupport");

                entity.Property(e => e.Srno).HasColumnName("SRNO");

                entity.Property(e => e.FromBy)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Img).HasColumnName("img");

                entity.Property(e => e.MyDetails).HasColumnType("text");

                entity.Property(e => e.MySubject).HasColumnType("text");

                entity.Property(e => e.Reply).HasColumnType("text");

                entity.Property(e => e.Sdate)
                    .HasColumnType("datetime")
                    .HasColumnName("SDate");

                entity.Property(e => e.ToBy)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblApiPaymentReq>(entity =>
            {
                entity.ToTable("tblApiPaymentReq");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AdminId)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.AdminReqid)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Admindescription).IsUnicode(false);

                entity.Property(e => e.ApproveDate)
                    .HasColumnType("datetime")
                    .HasColumnName("approveDate");

                entity.Property(e => e.ProjName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("projName");

                entity.Property(e => e.ReqAmount)
                    .HasColumnType("money")
                    .HasColumnName("reqAmount");

                entity.Property(e => e.ReqDate)
                    .HasColumnType("datetime")
                    .HasColumnName("reqDate");

                entity.Property(e => e.ReqStatus).HasColumnName("reqStatus");
            });

            modelBuilder.Entity<TblNews>(entity =>
            {
                entity.HasKey(e => e.NewsId);

                entity.ToTable("tblNews");

                entity.Property(e => e.NewsDate).HasColumnType("datetime");

                entity.Property(e => e.Website)
                    .HasMaxLength(100)
                    .HasColumnName("website");
            });

            modelBuilder.Entity<TeamDetail>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.Amount)
                    .HasColumnType("money")
                    .HasColumnName("amount");

                entity.Property(e => e.Edate)
                    .HasColumnType("datetime")
                    .HasColumnName("edate");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.Itype)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("itype");

                entity.Property(e => e.Lvl).HasColumnName("lvl");

                entity.Property(e => e.MemberId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("memberId");

                entity.Property(e => e.PinType).HasColumnName("pinType");

                entity.Property(e => e.RefPos)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("refPos");

                entity.Property(e => e.SpoId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("spoId");
            });

            modelBuilder.Entity<TeamInfo>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("TeamInfo");

                entity.Property(e => e.Edate)
                    .HasColumnType("datetime")
                    .HasColumnName("edate");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.Lvl).HasColumnName("lvl");

                entity.Property(e => e.MemberId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("memberId");

                entity.Property(e => e.RefPos)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("refPos");

                entity.Property(e => e.SpoId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("spoId");
            });

            modelBuilder.Entity<TokenGrowthRate>(entity =>
            {
                entity.ToTable("TokenGrowthRate");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Price).HasColumnType("money");
            });

            modelBuilder.Entity<UmTryingUpdate>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("umTryingUpdate");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.TokenAddress)
                    .HasMaxLength(200)
                    .HasColumnName("tokenAddress");

                entity.Property(e => e.TrxAddress)
                    .HasMaxLength(200)
                    .HasColumnName("trxAddress");

                entity.Property(e => e.TrxAddressD)
                    .HasMaxLength(200)
                    .HasColumnName("trxAddressD");

                entity.Property(e => e.Udate)
                    .HasColumnType("datetime")
                    .HasColumnName("udate");
            });

            modelBuilder.Entity<UnilevelPurchase>(entity =>
            {
                entity.ToTable("UnilevelPurchase");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Adate)
                    .HasColumnType("datetime")
                    .HasColumnName("adate");

                entity.Property(e => e.Amount)
                    .HasColumnType("money")
                    .HasColumnName("amount");

                entity.Property(e => e.Cmitid)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Directs).HasColumnName("directs");

                entity.Property(e => e.Etype)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("etype");

                entity.Property(e => e.FromAddress)
                    .HasMaxLength(200)
                    .HasColumnName("fromAddress");

                entity.Property(e => e.HashId)
                    .HasMaxLength(200)
                    .HasColumnName("hashId");

                entity.Property(e => e.MemberId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("memberId");

                entity.Property(e => e.Price)
                    .HasColumnType("money")
                    .HasColumnName("price");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.Property(e => e.ToAddress)
                    .HasMaxLength(200)
                    .HasColumnName("toAddress");

                entity.Property(e => e.TokenAmount)
                    .HasColumnType("money")
                    .HasColumnName("tokenAmount");

                entity.Property(e => e.Udate)
                    .HasColumnType("datetime")
                    .HasColumnName("udate");

                entity.Property(e => e.Ustatus).HasColumnName("ustatus");

                entity.Property(e => e.WillCount).HasColumnName("willCount");
            });

            modelBuilder.Entity<UpgradeTable>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("UpgradeTable");

                entity.Property(e => e.AdminCharge).HasColumnType("money");

                entity.Property(e => e.Ammount).HasColumnType("money");

                entity.Property(e => e.Bymemberid)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CmitId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("cmitId");

                entity.Property(e => e.Crntslab).HasColumnName("CRNTSLAB");

                entity.Property(e => e.Ddate).HasColumnType("datetime");

                entity.Property(e => e.FinalAmount).HasColumnType("money");

                entity.Property(e => e.HasId)
                    .HasMaxLength(200)
                    .HasColumnName("hasId");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.LvlNo).HasColumnName("lvlNo");

                entity.Property(e => e.Memberid)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("memberid");

                entity.Property(e => e.Mypackage)
                    .HasColumnType("money")
                    .HasColumnName("mypackage");

                entity.Property(e => e.Package)
                    .HasColumnType("money")
                    .HasColumnName("package");

                entity.Property(e => e.Rpcharge)
                    .HasColumnType("money")
                    .HasColumnName("rpcharge");

                entity.Property(e => e.Tds)
                    .HasColumnType("money")
                    .HasColumnName("TDS");
            });

            modelBuilder.Entity<UserMaster>(entity =>
            {
                entity.ToTable("userMaster");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DailyClosing).HasColumnType("datetime");

                entity.Property(e => e.LoginDate).HasColumnType("datetime");

                entity.Property(e => e.Role)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.TokenPrice)
                    .HasColumnType("money")
                    .HasColumnName("tokenPrice");

                entity.Property(e => e.TrxAddress).HasColumnName("trxAddress");

                entity.Property(e => e.TrxAddressD).HasColumnName("trxAddressD");

                entity.Property(e => e.UserId)
                    .HasMaxLength(200)
                    .HasColumnName("UserID");

                entity.Property(e => e.UserPass).HasMaxLength(200);

                entity.Property(e => e.WeeklyClosing)
                    .HasColumnType("datetime")
                    .HasColumnName("weeklyClosing");

                entity.Property(e => e.WidStatus).HasColumnName("widStatus");
            });

            modelBuilder.Entity<UserMasterSubAdmin>(entity =>
            {
                entity.ToTable("userMasterSubAdmin");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.LoginDate).HasColumnType("datetime");

                entity.Property(e => e.Role)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UserId)
                    .HasMaxLength(200)
                    .HasColumnName("UserID");

                entity.Property(e => e.UserPass).HasMaxLength(200);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
