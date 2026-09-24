using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MyApi.Migrations
{
    /// <inheritdoc />
    public partial class InitPostgres : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Booking");

            migrationBuilder.EnsureSchema(
                name: "ENUM");

            migrationBuilder.EnsureSchema(
                name: "Cinema");

            migrationBuilder.EnsureSchema(
                name: "Message");

            migrationBuilder.EnsureSchema(
                name: "Movie");

            migrationBuilder.EnsureSchema(
                name: "User");

            migrationBuilder.CreateTable(
                name: "BookingReservationStatus",
                schema: "ENUM",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Code = table.Column<string>(type: "character varying(400)", unicode: false, maxLength: 400, nullable: false),
                    Name = table.Column<string>(type: "character varying(400)", unicode: false, maxLength: 400, nullable: true),
                    Color = table.Column<string>(type: "character varying(400)", unicode: false, maxLength: 400, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingReservationEnum", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BookingSeatPriceStatus",
                schema: "ENUM",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Code = table.Column<string>(type: "character varying(400)", unicode: false, maxLength: 400, nullable: true),
                    Name = table.Column<string>(type: "character varying(400)", unicode: false, maxLength: 400, nullable: true),
                    Color = table.Column<string>(type: "character varying(400)", unicode: false, maxLength: 400, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieMovieSeatPriceStatusEnum", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BookingTicketStatus",
                schema: "ENUM",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Code = table.Column<string>(type: "character varying(400)", unicode: false, maxLength: 400, nullable: false),
                    Name = table.Column<string>(type: "character varying(400)", unicode: false, maxLength: 400, nullable: true),
                    Color = table.Column<string>(type: "character varying(400)", unicode: false, maxLength: 400, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingTicketStatusEnum", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CinemaCinemaStatus",
                schema: "ENUM",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Code = table.Column<string>(type: "character varying(400)", unicode: false, maxLength: 400, nullable: false),
                    Name = table.Column<string>(type: "character varying(400)", unicode: false, maxLength: 400, nullable: true),
                    Color = table.Column<string>(type: "character varying(400)", unicode: false, maxLength: 400, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CinemaCinemaStatusEnum", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CinemaRoomStatus",
                schema: "ENUM",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Code = table.Column<string>(type: "character varying(400)", unicode: false, maxLength: 400, nullable: false),
                    Name = table.Column<string>(type: "character varying(400)", unicode: false, maxLength: 400, nullable: true),
                    Color = table.Column<string>(type: "character varying(400)", unicode: false, maxLength: 400, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CinemaRoomStatusEnum", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CinemaRoomType",
                schema: "ENUM",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Code = table.Column<string>(type: "character varying(400)", unicode: false, maxLength: 400, nullable: false),
                    Name = table.Column<string>(type: "character varying(400)", unicode: false, maxLength: 400, nullable: true),
                    Color = table.Column<string>(type: "character varying(400)", unicode: false, maxLength: 400, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CinemaRoomTypeEnum", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CinemaSeatStatus",
                schema: "ENUM",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Code = table.Column<string>(type: "character varying(400)", unicode: false, maxLength: 400, nullable: false),
                    Name = table.Column<string>(type: "character varying(400)", unicode: false, maxLength: 400, nullable: true),
                    Color = table.Column<string>(type: "character varying(400)", unicode: false, maxLength: 400, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CinemaSeatStatusEnum", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CinemaSeatType",
                schema: "ENUM",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Code = table.Column<string>(type: "character varying(400)", unicode: false, maxLength: 400, nullable: false),
                    Name = table.Column<string>(type: "character varying(400)", unicode: false, maxLength: 400, nullable: true),
                    Color = table.Column<string>(type: "character varying(400)", unicode: false, maxLength: 400, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CinemaSeatTypeEnum", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MessageConversation",
                schema: "Message",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Type = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Image = table.Column<string>(type: "character varying(400)", unicode: false, maxLength: 400, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    State = table.Column<string>(type: "character(10)", fixedLength: true, maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageConversation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MovieMovieStatus",
                schema: "ENUM",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Code = table.Column<string>(type: "character varying(400)", unicode: false, maxLength: 400, nullable: false),
                    Name = table.Column<string>(type: "character varying(400)", unicode: false, maxLength: 400, nullable: true),
                    Color = table.Column<string>(type: "character varying(400)", unicode: false, maxLength: 400, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieMovieStatusEnum", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MovieShowtimeStatus",
                schema: "ENUM",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Code = table.Column<string>(type: "character varying(400)", unicode: false, maxLength: 400, nullable: false),
                    Name = table.Column<string>(type: "character varying(400)", unicode: false, maxLength: 400, nullable: true),
                    Color = table.Column<string>(type: "character varying(400)", unicode: false, maxLength: 400, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieShowtimeStatusEnum", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserCustomerStatus",
                schema: "ENUM",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Code = table.Column<string>(type: "character varying(400)", unicode: false, maxLength: 400, nullable: false),
                    Name = table.Column<string>(type: "character varying(400)", unicode: false, maxLength: 400, nullable: true),
                    Color = table.Column<string>(type: "character varying(400)", unicode: false, maxLength: 400, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCustomerStatusEnum", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserCustomerType",
                schema: "ENUM",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Code = table.Column<string>(type: "character varying(400)", unicode: false, maxLength: 400, nullable: false),
                    Role = table.Column<string>(type: "character(400)", fixedLength: true, maxLength: 400, nullable: false),
                    Color = table.Column<string>(type: "character varying(400)", unicode: false, maxLength: 400, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCustomerType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CinemaCinema",
                schema: "Cinema",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    City = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: false),
                    Address = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    RowId = table.Column<Guid>(type: "uuid", nullable: false),
                    CinemaStatusId = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: false),
                    Phone = table.Column<string>(type: "character varying(20)", unicode: false, maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CinemaCinema", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CinemaCinema_StatusEnum",
                        column: x => x.CinemaStatusId,
                        principalSchema: "ENUM",
                        principalTable: "CinemaCinemaStatus",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MovieMovie",
                schema: "Movie",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: false),
                    Title = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: false),
                    Describe = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: true),
                    Duration = table.Column<long>(type: "bigint", nullable: false),
                    Used = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    RowId = table.Column<Guid>(type: "uuid", nullable: false),
                    MovieStatusId = table.Column<long>(type: "bigint", nullable: false),
                    ReleaseDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Genre = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: false),
                    Director = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: false),
                    Cast = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: false),
                    Figure = table.Column<string>(type: "character varying(400)", unicode: false, maxLength: 400, nullable: false),
                    Language = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: true),
                    Trailer = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieMovie", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MovieMovie_StatusEnum",
                        column: x => x.MovieStatusId,
                        principalSchema: "ENUM",
                        principalTable: "MovieMovieStatus",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserCustomer",
                schema: "User",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(400)", unicode: false, maxLength: 400, nullable: false),
                    Phone = table.Column<string>(type: "character varying(400)", unicode: false, maxLength: 400, nullable: false),
                    Email = table.Column<string>(type: "character varying(400)", unicode: false, maxLength: 400, nullable: false),
                    UserStatusId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    RowId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserTypeId = table.Column<long>(type: "bigint", nullable: false, defaultValue: 1L),
                    Password = table.Column<string>(type: "character varying(400)", unicode: false, maxLength: 400, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCustomers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserCustomer_Status",
                        column: x => x.UserStatusId,
                        principalSchema: "ENUM",
                        principalTable: "UserCustomerStatus",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserCustomer_Type",
                        column: x => x.UserTypeId,
                        principalSchema: "ENUM",
                        principalTable: "UserCustomerType",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CinemaRoom",
                schema: "Cinema",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CinemaId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: false),
                    RoomTypeId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    RowId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoomStatusId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cinema.CinemaRoom", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CinemaRoom_Status",
                        column: x => x.RoomStatusId,
                        principalSchema: "ENUM",
                        principalTable: "CinemaRoomStatus",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CinemaRoom_Type",
                        column: x => x.RoomTypeId,
                        principalSchema: "ENUM",
                        principalTable: "CinemaRoomType",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Room_CinemaId",
                        column: x => x.CinemaId,
                        principalSchema: "Cinema",
                        principalTable: "CinemaCinema",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MessageMessage",
                schema: "Message",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ConversationId = table.Column<long>(type: "bigint", nullable: false),
                    SenderId = table.Column<long>(type: "bigint", nullable: false),
                    Message = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    Type = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageMessage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Message_ConversationId",
                        column: x => x.ConversationId,
                        principalSchema: "Message",
                        principalTable: "MessageConversation",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Message_SenderId",
                        column: x => x.SenderId,
                        principalSchema: "User",
                        principalTable: "UserCustomer",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MovieComment",
                schema: "Movie",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CustomerId = table.Column<long>(type: "bigint", nullable: false),
                    MovieId = table.Column<long>(type: "bigint", nullable: false),
                    Comment = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieComment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "User",
                        principalTable: "UserCustomer",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MovieId",
                        column: x => x.MovieId,
                        principalSchema: "Movie",
                        principalTable: "MovieMovie",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CinemaSeat",
                schema: "Cinema",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoomId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: false),
                    SeatTypeId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    RowId = table.Column<Guid>(type: "uuid", nullable: false),
                    SeatStatusId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CinemaSeat", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CinemaSeat_RoomId",
                        column: x => x.RoomId,
                        principalSchema: "Cinema",
                        principalTable: "CinemaRoom",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CinemaSeat_StatusEnum",
                        column: x => x.SeatStatusId,
                        principalSchema: "ENUM",
                        principalTable: "CinemaSeatStatus",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CinemaSeat_TypeEnum",
                        column: x => x.SeatTypeId,
                        principalSchema: "ENUM",
                        principalTable: "CinemaSeatType",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MovieShowtime",
                schema: "Movie",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MovieId = table.Column<long>(type: "bigint", nullable: false),
                    RoomId = table.Column<long>(type: "bigint", nullable: false),
                    BeginAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    RowId = table.Column<Guid>(type: "uuid", nullable: false),
                    ShowtimeStatusId = table.Column<long>(type: "bigint", nullable: false),
                    EndAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieShowtime", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MovieShowtime_MovieId",
                        column: x => x.MovieId,
                        principalSchema: "Movie",
                        principalTable: "MovieMovie",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MovieShowtime_RoomId",
                        column: x => x.RoomId,
                        principalSchema: "Cinema",
                        principalTable: "CinemaRoom",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MovieShowtime_StatusEnum",
                        column: x => x.ShowtimeStatusId,
                        principalSchema: "ENUM",
                        principalTable: "MovieShowtimeStatus",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MessageConversationMember",
                schema: "Message",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    ConversationId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastSeenMessage = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageConversationMember", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConversationMember_ConversationId",
                        column: x => x.ConversationId,
                        principalSchema: "Message",
                        principalTable: "MessageConversation",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ConversationMember_UserId",
                        column: x => x.UserId,
                        principalSchema: "User",
                        principalTable: "UserCustomer",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MessageConversationMember_LastSeenMessage",
                        column: x => x.LastSeenMessage,
                        principalSchema: "Message",
                        principalTable: "MessageMessage",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BookingReservation",
                schema: "Booking",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CustomerId = table.Column<long>(type: "bigint", nullable: false),
                    ShowtimeId = table.Column<long>(type: "bigint", nullable: false),
                    ReservationStatusId = table.Column<long>(type: "bigint", nullable: false),
                    ExpiredAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "numeric(22,4)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    RowId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingReservation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookingReservation_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "User",
                        principalTable: "UserCustomer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookingReservation_ShowtimeId",
                        column: x => x.ShowtimeId,
                        principalSchema: "Movie",
                        principalTable: "MovieShowtime",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BookingReservation_StatusEnum",
                        column: x => x.ReservationStatusId,
                        principalSchema: "ENUM",
                        principalTable: "BookingReservationStatus",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BookingSeatPrice",
                schema: "Booking",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ShowtimeId = table.Column<long>(type: "bigint", nullable: false),
                    SeatTypeId = table.Column<long>(type: "bigint", nullable: false),
                    SeatPrice = table.Column<decimal>(type: "numeric(22,4)", nullable: false),
                    SeatPriceStatusId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    RowId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShowtimeSeatPrice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CinemaSeatType_SeatTypeId",
                        column: x => x.SeatTypeId,
                        principalSchema: "ENUM",
                        principalTable: "CinemaSeatType",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MovieSeatPrice_StatusEnum",
                        column: x => x.SeatPriceStatusId,
                        principalSchema: "ENUM",
                        principalTable: "BookingSeatPriceStatus",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MovieShowtime_ShowtimeId",
                        column: x => x.ShowtimeId,
                        principalSchema: "Movie",
                        principalTable: "MovieShowtime",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BookingReservationSeat",
                schema: "Booking",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ReservationId = table.Column<long>(type: "bigint", nullable: false),
                    SeatId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    RowId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingReservationSeat", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookingReservationSeat_ReservationId",
                        column: x => x.ReservationId,
                        principalSchema: "Booking",
                        principalTable: "BookingReservation",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BookingReservationSeat_SeatId",
                        column: x => x.SeatId,
                        principalSchema: "Cinema",
                        principalTable: "CinemaSeat",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BookingTicket",
                schema: "Booking",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ReservationId = table.Column<long>(type: "bigint", nullable: false),
                    SeatId = table.Column<long>(type: "bigint", nullable: false),
                    TicketStatusId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    RowId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingTicket", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookingTicket_CinemaSeat",
                        column: x => x.SeatId,
                        principalSchema: "Cinema",
                        principalTable: "CinemaSeat",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BookingTicket_ShowtimeId",
                        column: x => x.ReservationId,
                        principalSchema: "Booking",
                        principalTable: "BookingReservation",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BookingTicket_StatusEnum",
                        column: x => x.TicketStatusId,
                        principalSchema: "ENUM",
                        principalTable: "BookingTicketStatus",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookingReservation_CustomerId",
                schema: "Booking",
                table: "BookingReservation",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingReservation_ReservationStatusId",
                schema: "Booking",
                table: "BookingReservation",
                column: "ReservationStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingReservation_ShowtimeId",
                schema: "Booking",
                table: "BookingReservation",
                column: "ShowtimeId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingReservationSeat_SeatId",
                schema: "Booking",
                table: "BookingReservationSeat",
                column: "SeatId");

            migrationBuilder.CreateIndex(
                name: "UC_BookingReservationSeat",
                schema: "Booking",
                table: "BookingReservationSeat",
                columns: new[] { "ReservationId", "SeatId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BookingSeatPrice_SeatPriceStatusId",
                schema: "Booking",
                table: "BookingSeatPrice",
                column: "SeatPriceStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingSeatPrice_SeatTypeId",
                schema: "Booking",
                table: "BookingSeatPrice",
                column: "SeatTypeId");

            migrationBuilder.CreateIndex(
                name: "UIX_ShowtimeSeatPrice_Active",
                schema: "Booking",
                table: "BookingSeatPrice",
                columns: new[] { "ShowtimeId", "SeatTypeId" },
                unique: true,
                filter: "\"DeletedAt\" IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BookingTicket_SeatId",
                schema: "Booking",
                table: "BookingTicket",
                column: "SeatId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingTicket_TicketStatusId",
                schema: "Booking",
                table: "BookingTicket",
                column: "TicketStatusId");

            migrationBuilder.CreateIndex(
                name: "UC_BookingTicket",
                schema: "Booking",
                table: "BookingTicket",
                columns: new[] { "ReservationId", "SeatId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CinemaCinema_CinemaStatusId",
                schema: "Cinema",
                table: "CinemaCinema",
                column: "CinemaStatusId");

            migrationBuilder.CreateIndex(
                name: "UC_Cinema_Address",
                schema: "Cinema",
                table: "CinemaCinema",
                column: "Address",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CinemaRoom_CinemaId",
                schema: "Cinema",
                table: "CinemaRoom",
                column: "CinemaId");

            migrationBuilder.CreateIndex(
                name: "IX_CinemaRoom_RoomStatusId",
                schema: "Cinema",
                table: "CinemaRoom",
                column: "RoomStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_CinemaRoom_RoomTypeId",
                schema: "Cinema",
                table: "CinemaRoom",
                column: "RoomTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CinemaSeat_RoomId",
                schema: "Cinema",
                table: "CinemaSeat",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_CinemaSeat_SeatStatusId",
                schema: "Cinema",
                table: "CinemaSeat",
                column: "SeatStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_CinemaSeat_SeatTypeId",
                schema: "Cinema",
                table: "CinemaSeat",
                column: "SeatTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageConversationMember_ConversationId",
                schema: "Message",
                table: "MessageConversationMember",
                column: "ConversationId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageConversationMember_LastSeenMessage",
                schema: "Message",
                table: "MessageConversationMember",
                column: "LastSeenMessage");

            migrationBuilder.CreateIndex(
                name: "IX_MessageConversationMember_UserId",
                schema: "Message",
                table: "MessageConversationMember",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageMessage_ConversationId",
                schema: "Message",
                table: "MessageMessage",
                column: "ConversationId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageMessage_SenderId",
                schema: "Message",
                table: "MessageMessage",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_MovieComment_CustomerId",
                schema: "Movie",
                table: "MovieComment",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_MovieComment_MovieId",
                schema: "Movie",
                table: "MovieComment",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_MovieMovie_MovieStatusId",
                schema: "Movie",
                table: "MovieMovie",
                column: "MovieStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_MovieShowtime_RoomId",
                schema: "Movie",
                table: "MovieShowtime",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_MovieShowtime_ShowtimeStatusId",
                schema: "Movie",
                table: "MovieShowtime",
                column: "ShowtimeStatusId");

            migrationBuilder.CreateIndex(
                name: "UC_ShowtimeShowtime",
                schema: "Movie",
                table: "MovieShowtime",
                columns: new[] { "MovieId", "RoomId", "BeginAt" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserCustomer_UserStatusId",
                schema: "User",
                table: "UserCustomer",
                column: "UserStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCustomer_UserTypeId",
                schema: "User",
                table: "UserCustomer",
                column: "UserTypeId");

            migrationBuilder.CreateIndex(
                name: "UC_UserCustomer",
                schema: "User",
                table: "UserCustomer",
                column: "Phone",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookingReservationSeat",
                schema: "Booking");

            migrationBuilder.DropTable(
                name: "BookingSeatPrice",
                schema: "Booking");

            migrationBuilder.DropTable(
                name: "BookingTicket",
                schema: "Booking");

            migrationBuilder.DropTable(
                name: "MessageConversationMember",
                schema: "Message");

            migrationBuilder.DropTable(
                name: "MovieComment",
                schema: "Movie");

            migrationBuilder.DropTable(
                name: "BookingSeatPriceStatus",
                schema: "ENUM");

            migrationBuilder.DropTable(
                name: "CinemaSeat",
                schema: "Cinema");

            migrationBuilder.DropTable(
                name: "BookingReservation",
                schema: "Booking");

            migrationBuilder.DropTable(
                name: "BookingTicketStatus",
                schema: "ENUM");

            migrationBuilder.DropTable(
                name: "MessageMessage",
                schema: "Message");

            migrationBuilder.DropTable(
                name: "CinemaSeatStatus",
                schema: "ENUM");

            migrationBuilder.DropTable(
                name: "CinemaSeatType",
                schema: "ENUM");

            migrationBuilder.DropTable(
                name: "MovieShowtime",
                schema: "Movie");

            migrationBuilder.DropTable(
                name: "BookingReservationStatus",
                schema: "ENUM");

            migrationBuilder.DropTable(
                name: "MessageConversation",
                schema: "Message");

            migrationBuilder.DropTable(
                name: "UserCustomer",
                schema: "User");

            migrationBuilder.DropTable(
                name: "MovieMovie",
                schema: "Movie");

            migrationBuilder.DropTable(
                name: "CinemaRoom",
                schema: "Cinema");

            migrationBuilder.DropTable(
                name: "MovieShowtimeStatus",
                schema: "ENUM");

            migrationBuilder.DropTable(
                name: "UserCustomerStatus",
                schema: "ENUM");

            migrationBuilder.DropTable(
                name: "UserCustomerType",
                schema: "ENUM");

            migrationBuilder.DropTable(
                name: "MovieMovieStatus",
                schema: "ENUM");

            migrationBuilder.DropTable(
                name: "CinemaRoomStatus",
                schema: "ENUM");

            migrationBuilder.DropTable(
                name: "CinemaRoomType",
                schema: "ENUM");

            migrationBuilder.DropTable(
                name: "CinemaCinema",
                schema: "Cinema");

            migrationBuilder.DropTable(
                name: "CinemaCinemaStatus",
                schema: "ENUM");
        }
    }
}
