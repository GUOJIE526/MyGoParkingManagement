﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace EasyPark.Models;

public partial class EasyParkContext : DbContext
{
    public EasyParkContext()
    {
    }

    public EasyParkContext(DbContextOptions<EasyParkContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Car> Car { get; set; }

    public virtual DbSet<Coupon> Coupon { get; set; }

    public virtual DbSet<Customer> Customer { get; set; }

    public virtual DbSet<DealRecord> DealRecord { get; set; }

    public virtual DbSet<EntryExitManagement> EntryExitManagement { get; set; }

    public virtual DbSet<MonApplyList> MonApplyList { get; set; }

    public virtual DbSet<MonthlyRental> MonthlyRental { get; set; }

    public virtual DbSet<Orders> Orders { get; set; }

    public virtual DbSet<ParkingLotImages> ParkingLotImages { get; set; }

    public virtual DbSet<ParkingLots> ParkingLots { get; set; }

    public virtual DbSet<ParkingSlot> ParkingSlot { get; set; }

    public virtual DbSet<Reservation> Reservation { get; set; }

    public virtual DbSet<Revenue> Revenue { get; set; }

    public virtual DbSet<Survey> Survey { get; set; }

    public virtual DbSet<Transactions> Transactions { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=EasyPark;TrustServerCertificate=true;Integrated Security=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Car>(entity =>
        {
            entity.HasKey(e => e.CarId).HasName("PK__Car__4C9A0DB3DC2A9718");

            entity.Property(e => e.CarId).HasColumnName("car_id");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.LicensePlate)
                .HasMaxLength(30)
                .HasColumnName("license_plate");
            entity.Property(e => e.RegisterDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("register_date");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Car)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Car_User");
        });

        modelBuilder.Entity<Coupon>(entity =>
        {
            entity.HasKey(e => e.CouponId).HasName("PK__Coupon__58CF63898CBC0AF0");

            entity.Property(e => e.CouponId).HasColumnName("coupon_id");
            entity.Property(e => e.CouponCode)
                .HasMaxLength(50)
                .HasColumnName("coupon_code");
            entity.Property(e => e.DiscountAmount).HasColumnName("discount_amount");
            entity.Property(e => e.IsUsed).HasColumnName("is_used");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.ValidFrom)
                .HasColumnType("datetime")
                .HasColumnName("valid_from");
            entity.Property(e => e.ValidUntil)
                .HasColumnType("datetime")
                .HasColumnName("valid_until");

            entity.HasOne(d => d.User).WithMany(p => p.Coupon)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Coupon_User");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Customer__B9BE370F40F9907A");

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.BlackCount).HasColumnName("blackCount");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.IsBlack).HasColumnName("is_black");
            entity.Property(e => e.Password).HasColumnName("password");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasColumnName("phone");
            entity.Property(e => e.RegisterDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("register_date");
            entity.Property(e => e.Salt).HasColumnName("salt");
            entity.Property(e => e.Token).HasColumnName("token");
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .HasColumnName("username");
        });

        modelBuilder.Entity<DealRecord>(entity =>
        {
            entity.HasKey(e => e.DealId).HasName("PK__DealReco__C012A76C820600B9");

            entity.Property(e => e.DealId).HasColumnName("deal_id");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.CarId).HasColumnName("car_id");
            entity.Property(e => e.ParkType)
                .HasMaxLength(50)
                .HasColumnName("parkType");
            entity.Property(e => e.PaymentTime)
                .HasColumnType("datetime")
                .HasColumnName("payment_time");

            entity.HasOne(d => d.Car).WithMany(p => p.DealRecord)
                .HasForeignKey(d => d.CarId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DealRecord");
        });

        modelBuilder.Entity<EntryExitManagement>(entity =>
        {
            entity.HasKey(e => e.EntryexitId).HasName("PK__EntryExi__FD3EA5F6174CE504");

            entity.Property(e => e.EntryexitId).HasColumnName("entryexit_id");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.CarId).HasColumnName("car_id");
            entity.Property(e => e.EntryTime)
                .HasColumnType("datetime")
                .HasColumnName("entry_time");
            entity.Property(e => e.ExitTime)
                .HasColumnType("datetime")
                .HasColumnName("exit_time");
            entity.Property(e => e.IsFinish).HasColumnName("is_finish");
            entity.Property(e => e.LicensePlateKeyinTime)
                .HasColumnType("datetime")
                .HasColumnName("license_plate_keyin_time");
            entity.Property(e => e.LicensePlatePhoto).HasColumnName("license_plate_photo");
            entity.Property(e => e.LotId).HasColumnName("lot_id");
            entity.Property(e => e.Parktype)
                .HasMaxLength(50)
                .HasColumnName("parktype");
            entity.Property(e => e.PaymentStatus).HasColumnName("payment_status");
            entity.Property(e => e.PaymentTime)
                .HasColumnType("datetime")
                .HasColumnName("payment_time");
            entity.Property(e => e.ValidTime)
                .HasColumnType("datetime")
                .HasColumnName("valid_time");

            entity.HasOne(d => d.Car).WithMany(p => p.EntryExitManagement)
                .HasForeignKey(d => d.CarId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EntryExit_Car");

            entity.HasOne(d => d.Lot).WithMany(p => p.EntryExitManagement)
                .HasForeignKey(d => d.LotId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EntryExit_Lot");
        });

        modelBuilder.Entity<MonApplyList>(entity =>
        {
            entity.HasKey(e => e.ApplyId).HasName("PK__MonApply__8260CA8247676E62");

            entity.Property(e => e.ApplyId).HasColumnName("apply_id");
            entity.Property(e => e.ApplyDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("apply_date");
            entity.Property(e => e.ApplyStatus)
                .HasMaxLength(50)
                .HasDefaultValue("pending")
                .HasColumnName("apply_status");
            entity.Property(e => e.CarId).HasColumnName("car_id");
            entity.Property(e => e.IsCanceled).HasColumnName("is_canceled");
            entity.Property(e => e.LotId).HasColumnName("lot_id");
            entity.Property(e => e.NotificationStatus).HasColumnName("notification_status");

            entity.HasOne(d => d.Car).WithMany(p => p.MonApplyList)
                .HasForeignKey(d => d.CarId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MonApplyList_Car");

            entity.HasOne(d => d.Lot).WithMany(p => p.MonApplyList)
                .HasForeignKey(d => d.LotId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MonApplyList_Lot");
        });

        modelBuilder.Entity<MonthlyRental>(entity =>
        {
            entity.HasKey(e => e.RenId).HasName("PK__MonthlyR__5833C152FA762C76");

            entity.Property(e => e.RenId).HasColumnName("ren_id");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.CarId).HasColumnName("car_id");
            entity.Property(e => e.EndDate)
                .HasColumnType("datetime")
                .HasColumnName("end_date");
            entity.Property(e => e.LotId).HasColumnName("lot_id");
            entity.Property(e => e.PaymentStatus).HasColumnName("payment_status");
            entity.Property(e => e.StartDate)
                .HasColumnType("datetime")
                .HasColumnName("start_date");
            entity.Property(e => e.TransactionId).HasMaxLength(50);

            entity.HasOne(d => d.Car).WithMany(p => p.MonthlyRental)
                .HasForeignKey(d => d.CarId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MonthlyRental_Car");

            entity.HasOne(d => d.Lot).WithMany(p => p.MonthlyRental)
                .HasForeignKey(d => d.LotId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MonthlyRental_Lot");
        });

        modelBuilder.Entity<Orders>(entity =>
        {
            entity.HasKey(e => e.OrdId).HasName("PK__Orders__DC39D7DF1E7759D7");

            entity.Property(e => e.OrdId).HasColumnName("ord_id");
            entity.Property(e => e.Amount)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("amount");
            entity.Property(e => e.CreatedTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_time");
            entity.Property(e => e.OrdType)
                .HasMaxLength(50)
                .HasColumnName("ord_type");
            entity.Property(e => e.PaymentStatus).HasColumnName("payment_status");
            entity.Property(e => e.PaymentTime)
                .HasColumnType("datetime")
                .HasColumnName("payment_time");
            entity.Property(e => e.ReferenceId).HasColumnName("reference_id");
        });

        modelBuilder.Entity<ParkingLotImages>(entity =>
        {
            entity.HasKey(e => e.ImageId).HasName("PK__ParkingL__DC9AC955F9C4E2A9");

            entity.Property(e => e.ImageId).HasColumnName("image_id");
            entity.Property(e => e.ImgPath).HasColumnName("imgPath");
            entity.Property(e => e.ImgTitle).HasColumnName("imgTitle");
            entity.Property(e => e.LotId).HasColumnName("lot_id");

            entity.HasOne(d => d.Lot).WithMany(p => p.ParkingLotImages)
                .HasForeignKey(d => d.LotId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_ParkingLotImages_Lot");
        });

        modelBuilder.Entity<ParkingLots>(entity =>
        {
            entity.HasKey(e => e.LotId);

            entity.Property(e => e.LotId)
                .ValueGeneratedNever()
                .HasColumnName("lot_id");
            entity.Property(e => e.District)
                .HasMaxLength(50)
                .HasColumnName("district");
            entity.Property(e => e.EtcSpace).HasColumnName("etcSpace");
            entity.Property(e => e.HolidayRate).HasColumnName("holidayRate");
            entity.Property(e => e.Latitude)
                .HasColumnType("decimal(18, 10)")
                .HasColumnName("latitude");
            entity.Property(e => e.Location)
                .HasMaxLength(250)
                .HasColumnName("location");
            entity.Property(e => e.Longitude)
                .HasColumnType("decimal(18, 15)")
                .HasColumnName("longitude");
            entity.Property(e => e.LotName)
                .HasMaxLength(255)
                .HasColumnName("lot_name");
            entity.Property(e => e.MotherSpace).HasColumnName("motherSpace");
            entity.Property(e => e.MotoSpace).HasColumnName("motoSpace");
            entity.Property(e => e.OpendoorTime)
                .HasMaxLength(150)
                .HasColumnName("opendoorTime");
            entity.Property(e => e.SmallCarSpace).HasColumnName("smallCarSpace");
            entity.Property(e => e.Tel)
                .HasMaxLength(100)
                .HasDefaultValue("empty")
                .HasColumnName("tel");
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .HasColumnName("type");
            entity.Property(e => e.ValidSpace).HasColumnName("validSpace");
            entity.Property(e => e.WeekdayRate).HasColumnName("weekdayRate");
        });

        modelBuilder.Entity<ParkingSlot>(entity =>
        {
            entity.HasKey(e => e.SlotId).HasName("PK__ParkingS__971A01BB45C83BCE");

            entity.Property(e => e.SlotId).HasColumnName("slot_id");
            entity.Property(e => e.IsRented).HasColumnName("is_Rented");
            entity.Property(e => e.LotId).HasColumnName("lot_id");
            entity.Property(e => e.SlotNum).HasColumnName("slot_num");

            entity.HasOne(d => d.Lot).WithMany(p => p.ParkingSlot)
                .HasForeignKey(d => d.LotId)
                .HasConstraintName("FK_lotName");
        });

        modelBuilder.Entity<Reservation>(entity =>
        {
            entity.HasKey(e => e.ResId).HasName("PK__Reservat__2090B50D7D0EB7C2");

            entity.Property(e => e.ResId).HasColumnName("res_id");
            entity.Property(e => e.CarId).HasColumnName("car_id");
            entity.Property(e => e.IsCanceled).HasColumnName("is_canceled");
            entity.Property(e => e.IsFinish).HasColumnName("is_finish");
            entity.Property(e => e.IsOverdue).HasColumnName("is_overdue");
            entity.Property(e => e.IsRefoundDeposit).HasColumnName("is_refound_deposit");
            entity.Property(e => e.LotId).HasColumnName("lot_id");
            entity.Property(e => e.NotificationStatus).HasColumnName("notification_status");
            entity.Property(e => e.PaymentStatus).HasColumnName("payment_status");
            entity.Property(e => e.ResTime)
                .HasColumnType("datetime")
                .HasColumnName("res_time");
            entity.Property(e => e.StartTime)
                .HasColumnType("datetime")
                .HasColumnName("start_time");
            entity.Property(e => e.ValidUntil)
                .HasColumnType("datetime")
                .HasColumnName("valid_until");

            entity.HasOne(d => d.Car).WithMany(p => p.Reservation)
                .HasForeignKey(d => d.CarId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Reservation_Car");

            entity.HasOne(d => d.Lot).WithMany(p => p.Reservation)
                .HasForeignKey(d => d.LotId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Reservation_Lot");
        });

        modelBuilder.Entity<Revenue>(entity =>
        {
            entity.HasKey(e => e.RevenueId).HasName("PK__Revenue__3DF902E9E457026E");

            entity.Property(e => e.RevenueId).HasColumnName("revenue_id");
            entity.Property(e => e.CreatedTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_time");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.RentalIncome).HasColumnName("rental_income");
            entity.Property(e => e.ReservationIncome).HasColumnName("reservation_income");
            entity.Property(e => e.TotalAmount).HasColumnName("total_amount");
        });

        modelBuilder.Entity<Survey>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Survey__3213E83F5D21C2F9");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IsReplied).HasColumnName("is_replied");
            entity.Property(e => e.Question).HasColumnName("question");
            entity.Property(e => e.RepliedAt)
                .HasColumnType("datetime")
                .HasColumnName("replied_at");
            entity.Property(e => e.ReplyMessage).HasColumnName("reply_message");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("未回覆")
                .HasColumnName("status");
            entity.Property(e => e.SubmittedAt)
                .HasColumnType("datetime")
                .HasColumnName("submitted_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Survey)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Survey_Customer");
        });

        modelBuilder.Entity<Transactions>(entity =>
        {
            entity.HasKey(e => e.TranId).HasName("PK__Transact__A67F8A20F420CE3D");

            entity.Property(e => e.TranId).HasColumnName("tran_id");
            entity.Property(e => e.CouponId).HasColumnName("coupon_id");
            entity.Property(e => e.CreatedTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_time");
            entity.Property(e => e.DiscountAmount)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("discount_amount");
            entity.Property(e => e.OrdId).HasColumnName("ord_id");
            entity.Property(e => e.PaymentAmount)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("payment_amount");
            entity.Property(e => e.PaymentMethod)
                .HasMaxLength(50)
                .HasColumnName("payment_method");
            entity.Property(e => e.PaymentStatus).HasColumnName("payment_status");
            entity.Property(e => e.PaymentTime)
                .HasColumnType("datetime")
                .HasColumnName("payment_time");

            entity.HasOne(d => d.Coupon).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.CouponId)
                .HasConstraintName("FK_Transaction_Coupon");

            entity.HasOne(d => d.Ord).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.OrdId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Transaction_Order");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
