using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MyApi.Models;

public partial class TestContext : DbContext
{
    public TestContext()
    {
    }

    public TestContext(DbContextOptions<TestContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BookingReservation> BookingReservations { get; set; }

    public virtual DbSet<BookingReservationSeat> BookingReservationSeats { get; set; }

    public virtual DbSet<BookingReservationStatus> BookingReservationStatuses { get; set; }

    public virtual DbSet<BookingSeatPrice> BookingSeatPrices { get; set; }

    public virtual DbSet<BookingSeatPriceStatus> BookingSeatPriceStatuses { get; set; }

    public virtual DbSet<BookingTicket> BookingTickets { get; set; }

    public virtual DbSet<BookingTicketStatus> BookingTicketStatuses { get; set; }

    public virtual DbSet<CinemaCinema> CinemaCinemas { get; set; }

    public virtual DbSet<CinemaCinemaStatus> CinemaCinemaStatuses { get; set; }

    public virtual DbSet<CinemaRoom> CinemaRooms { get; set; }

    public virtual DbSet<CinemaRoomStatus> CinemaRoomStatuses { get; set; }

    public virtual DbSet<CinemaRoomType> CinemaRoomTypes { get; set; }

    public virtual DbSet<CinemaSeat> CinemaSeats { get; set; }

    public virtual DbSet<CinemaSeatStatus> CinemaSeatStatuses { get; set; }

    public virtual DbSet<CinemaSeatType> CinemaSeatTypes { get; set; }

    public virtual DbSet<MovieMovie> MovieMovies { get; set; }

    public virtual DbSet<MovieMovieStatus> MovieMovieStatuses { get; set; }

    public virtual DbSet<MovieShowtime> MovieShowtimes { get; set; }

    public virtual DbSet<MovieShowtimeStatus> MovieShowtimeStatuses { get; set; }

    public virtual DbSet<UserCustomer> UserCustomers { get; set; }

    public virtual DbSet<UserCustomerStatus> UserCustomerStatuses { get; set; }

    public virtual DbSet<UserCustomerType> UserCustomerTypes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-76A1KAF\\SQLEXPRESS;Database=test;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BookingReservation>(entity =>
        {
            entity.ToTable("BookingReservation", "Booking");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.DeletedAt).HasColumnType("datetime");
            entity.Property(e => e.ExpiredAt).HasColumnType("datetime");
            entity.Property(e => e.TotalPrice).HasColumnType("decimal(22, 4)");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Customer).WithMany(p => p.BookingReservations)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK_BookingReservation_CustomerId");

            entity.HasOne(d => d.ReservationStatus).WithMany(p => p.BookingReservations)
                .HasForeignKey(d => d.ReservationStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BookingReservation_StatusEnum");

            entity.HasOne(d => d.Showtime).WithMany(p => p.BookingReservations)
                .HasForeignKey(d => d.ShowtimeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BookingReservation_ShowtimeId");
        });

        modelBuilder.Entity<BookingReservationSeat>(entity =>
        {
            entity.ToTable("BookingReservationSeat", "Booking");

            entity.HasIndex(e => new { e.ReservationId, e.SeatId }, "UC_BookingReservationSeat").IsUnique();

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.DeletedAt).HasColumnType("datetime");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Reservation).WithMany(p => p.BookingReservationSeats)
                .HasForeignKey(d => d.ReservationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BookingReservationSeat_ReservationId");

            entity.HasOne(d => d.Seat).WithMany(p => p.BookingReservationSeats)
                .HasForeignKey(d => d.SeatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BookingReservationSeat_SeatId");
        });

        modelBuilder.Entity<BookingReservationStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_BookingReservationEnum");

            entity.ToTable("BookingReservationStatus", "ENUM");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Code)
                .HasMaxLength(400)
                .IsUnicode(false);
            entity.Property(e => e.Color)
                .HasMaxLength(400)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(400)
                .IsUnicode(false);
        });

        modelBuilder.Entity<BookingSeatPrice>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_ShowtimeSeatPrice");

            entity.ToTable("BookingSeatPrice", "Booking");

            entity.HasIndex(e => new { e.ShowtimeId, e.SeatTypeId }, "UIX_ShowtimeSeatPrice_Active")
                .IsUnique()
                .HasFilter("([DeletedAt] IS NULL)");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.DeletedAt).HasColumnType("datetime");
            entity.Property(e => e.SeatPrice).HasColumnType("decimal(22, 4)");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.SeatPriceStatus).WithMany(p => p.BookingSeatPrices)
                .HasForeignKey(d => d.SeatPriceStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MovieSeatPrice_StatusEnum");

            entity.HasOne(d => d.SeatType).WithMany(p => p.BookingSeatPrices)
                .HasForeignKey(d => d.SeatTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CinemaSeatType_SeatTypeId");

            entity.HasOne(d => d.Showtime).WithMany(p => p.BookingSeatPrices)
                .HasForeignKey(d => d.ShowtimeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MovieShowtime_ShowtimeId");
        });

        modelBuilder.Entity<BookingSeatPriceStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_MovieMovieSeatPriceStatusEnum");

            entity.ToTable("BookingSeatPriceStatus", "ENUM");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Code)
                .HasMaxLength(400)
                .IsUnicode(false);
            entity.Property(e => e.Color)
                .HasMaxLength(400)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(400)
                .IsUnicode(false);
        });

        modelBuilder.Entity<BookingTicket>(entity =>
        {
            entity.ToTable("BookingTicket", "Booking");

            entity.HasIndex(e => new { e.ShowtimeId, e.SeatId }, "UC_BookingTicket").IsUnique();

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.DeletedAt).HasColumnType("datetime");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Seat).WithMany(p => p.BookingTickets)
                .HasForeignKey(d => d.SeatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BookingTicket_CinemaSeat");

            entity.HasOne(d => d.Showtime).WithMany(p => p.BookingTickets)
                .HasForeignKey(d => d.ShowtimeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BookingTicket_ShowtimeId");

            entity.HasOne(d => d.TicketStatus).WithMany(p => p.BookingTickets)
                .HasForeignKey(d => d.TicketStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BookingTicket_StatusEnum");
        });

        modelBuilder.Entity<BookingTicketStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_BookingTicketStatusEnum");

            entity.ToTable("BookingTicketStatus", "ENUM");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Code)
                .HasMaxLength(400)
                .IsUnicode(false);
            entity.Property(e => e.Color)
                .HasMaxLength(400)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(400)
                .IsUnicode(false);
        });

        modelBuilder.Entity<CinemaCinema>(entity =>
        {
            entity.ToTable("CinemaCinema", "Cinema");

            entity.HasIndex(e => e.Address, "UC_Cinema_Address").IsUnique();

            entity.Property(e => e.Address)
                .HasMaxLength(400)
                .IsUnicode(false);
            entity.Property(e => e.City)
                .HasMaxLength(400)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.DeletedAt).HasColumnType("datetime");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.CinemaStatus).WithMany(p => p.CinemaCinemas)
                .HasForeignKey(d => d.CinemaStatusId)
                .HasConstraintName("FK_CinemaCinema_StatusEnum");
        });

        modelBuilder.Entity<CinemaCinemaStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_CinemaCinemaStatusEnum");

            entity.ToTable("CinemaCinemaStatus", "ENUM");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Code)
                .HasMaxLength(400)
                .IsUnicode(false);
            entity.Property(e => e.Color)
                .HasMaxLength(400)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(400)
                .IsUnicode(false);
        });

        modelBuilder.Entity<CinemaRoom>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Cinema.CinemaRoom");

            entity.ToTable("CinemaRoom", "Cinema");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.DeletedAt).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(400)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Cinema).WithMany(p => p.CinemaRooms)
                .HasForeignKey(d => d.CinemaId)
                .HasConstraintName("FK_Room_CinemaId");

            entity.HasOne(d => d.RoomStatus).WithMany(p => p.CinemaRooms)
                .HasForeignKey(d => d.RoomStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CinemaRoom_Status");

            entity.HasOne(d => d.RoomType).WithMany(p => p.CinemaRooms)
                .HasForeignKey(d => d.RoomTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CinemaRoom_Type");
        });

        modelBuilder.Entity<CinemaRoomStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_CinemaRoomStatusEnum");

            entity.ToTable("CinemaRoomStatus", "ENUM");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Code)
                .HasMaxLength(400)
                .IsUnicode(false);
            entity.Property(e => e.Color)
                .HasMaxLength(400)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(400)
                .IsUnicode(false);
        });

        modelBuilder.Entity<CinemaRoomType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_CinemaRoomTypeEnum");

            entity.ToTable("CinemaRoomType", "ENUM");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Code)
                .HasMaxLength(400)
                .IsUnicode(false);
            entity.Property(e => e.Color)
                .HasMaxLength(400)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(400)
                .IsUnicode(false);
        });

        modelBuilder.Entity<CinemaSeat>(entity =>
        {
            entity.ToTable("CinemaSeat", "Cinema");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.DeletedAt).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(400)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Room).WithMany(p => p.CinemaSeats)
                .HasForeignKey(d => d.RoomId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CinemaSeat_RoomId");

            entity.HasOne(d => d.SeatStatus).WithMany(p => p.CinemaSeats)
                .HasForeignKey(d => d.SeatStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CinemaSeat_StatusEnum");

            entity.HasOne(d => d.SeatType).WithMany(p => p.CinemaSeats)
                .HasForeignKey(d => d.SeatTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CinemaSeat_TypeEnum");
        });

        modelBuilder.Entity<CinemaSeatStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_CinemaSeatStatusEnum");

            entity.ToTable("CinemaSeatStatus", "ENUM");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Code)
                .HasMaxLength(400)
                .IsUnicode(false);
            entity.Property(e => e.Color)
                .HasMaxLength(400)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(400)
                .IsUnicode(false);
        });

        modelBuilder.Entity<CinemaSeatType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_CinemaSeatTypeEnum");

            entity.ToTable("CinemaSeatType", "ENUM");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Code)
                .HasMaxLength(400)
                .IsUnicode(false);
            entity.Property(e => e.Color)
                .HasMaxLength(400)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(400)
                .IsUnicode(false);
        });

        modelBuilder.Entity<MovieMovie>(entity =>
        {
            entity.ToTable("MovieMovie", "Movie");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.DeletedAt).HasColumnType("datetime");
            entity.Property(e => e.Describe)
                .HasMaxLength(400)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(400)
                .IsUnicode(false);
            entity.Property(e => e.Title)
                .HasMaxLength(400)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.MovieStatus).WithMany(p => p.MovieMovies)
                .HasForeignKey(d => d.MovieStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MovieMovie_StatusEnum");
        });

        modelBuilder.Entity<MovieMovieStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_MovieMovieStatusEnum");

            entity.ToTable("MovieMovieStatus", "ENUM");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Code)
                .HasMaxLength(400)
                .IsUnicode(false);
            entity.Property(e => e.Color)
                .HasMaxLength(400)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(400)
                .IsUnicode(false);
        });

        modelBuilder.Entity<MovieShowtime>(entity =>
        {
            entity.ToTable("MovieShowtime", "Movie");

            entity.HasIndex(e => new { e.MovieId, e.RoomId, e.BeginAt }, "UC_ShowtimeShowtime").IsUnique();

            entity.Property(e => e.BeginAt).HasColumnType("datetime");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.DeletedAt).HasColumnType("datetime");
            entity.Property(e => e.EndAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Movie).WithMany(p => p.MovieShowtimes)
                .HasForeignKey(d => d.MovieId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MovieShowtime_MovieId");

            entity.HasOne(d => d.Room).WithMany(p => p.MovieShowtimes)
                .HasForeignKey(d => d.RoomId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MovieShowtime_RoomId");

            entity.HasOne(d => d.ShowtimeStatus).WithMany(p => p.MovieShowtimes)
                .HasForeignKey(d => d.ShowtimeStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MovieShowtime_StatusEnum");
        });

        modelBuilder.Entity<MovieShowtimeStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_MovieShowtimeStatusEnum");

            entity.ToTable("MovieShowtimeStatus", "ENUM");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Code)
                .HasMaxLength(400)
                .IsUnicode(false);
            entity.Property(e => e.Color)
                .HasMaxLength(400)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(400)
                .IsUnicode(false);
        });

        modelBuilder.Entity<UserCustomer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_UserCustomers");

            entity.ToTable("UserCustomer", "User");

            entity.HasIndex(e => e.Phone, "UC_UserCustomer").IsUnique();

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.DeletedAt).HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(400)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(400)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(400)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            entity.Property(e => e.UserTypeId).HasDefaultValue(1L);

            entity.HasOne(d => d.UserStatus).WithMany(p => p.UserCustomers)
                .HasForeignKey(d => d.UserStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserCustomer_Status");

            entity.HasOne(d => d.UserType).WithMany(p => p.UserCustomers)
                .HasForeignKey(d => d.UserTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserCustomer_Type");
        });

        modelBuilder.Entity<UserCustomerStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_UserCustomerStatusEnum");

            entity.ToTable("UserCustomerStatus", "ENUM");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Code)
                .HasMaxLength(400)
                .IsUnicode(false);
            entity.Property(e => e.Color)
                .HasMaxLength(400)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(400)
                .IsUnicode(false);
        });

        modelBuilder.Entity<UserCustomerType>(entity =>
        {
            entity.ToTable("UserCustomerType", "ENUM");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Code)
                .HasMaxLength(400)
                .IsUnicode(false);
            entity.Property(e => e.Color)
                .HasMaxLength(400)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(400)
                .IsFixedLength();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
