using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace API.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TIME",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Bandeira = table.Column<byte[]>(nullable: true),
                    NMTecnico = table.Column<string>(maxLength: 100, nullable: false),
                    Pais = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TIME", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JOGADOR",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(maxLength: 100, nullable: false),
                    TimeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JOGADOR", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JOGADOR_TIME_TimeId",
                        column: x => x.TimeId,
                        principalTable: "TIME",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "JOGO",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Data = table.Column<DateTime>(nullable: false),
                    Time1 = table.Column<int>(nullable: false),
                    Time2 = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JOGO", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JOGO_TIME_Time1",
                        column: x => x.Time1,
                        principalTable: "TIME",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_JOGO_TIME_Time2",
                        column: x => x.Time2,
                        principalTable: "TIME",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "FICHA",
                columns: table => new
                {
                    JogadorId = table.Column<int>(nullable: false),
                    Altura = table.Column<decimal>(nullable: false),
                    Camisa = table.Column<int>(nullable: false),
                    Naturalidade = table.Column<string>(maxLength: 100, nullable: false),
                    Posicao = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FICHA", x => x.JogadorId);
                    table.ForeignKey(
                        name: "FK_FICHA_JOGADOR_JogadorId",
                        column: x => x.JogadorId,
                        principalTable: "JOGADOR",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "GOL",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Hora = table.Column<TimeSpan>(nullable: false),
                    JogadorId = table.Column<int>(nullable: false),
                    JogoId = table.Column<int>(nullable: false),
                    TimeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GOL", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GOL_JOGADOR_JogadorId",
                        column: x => x.JogadorId,
                        principalTable: "JOGADOR",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_GOL_JOGO_JogoId",
                        column: x => x.JogoId,
                        principalTable: "JOGO",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_GOL_TIME_TimeId",
                        column: x => x.TimeId,
                        principalTable: "TIME",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GOL_JogadorId",
                table: "GOL",
                column: "JogadorId");

            migrationBuilder.CreateIndex(
                name: "IX_GOL_JogoId",
                table: "GOL",
                column: "JogoId");

            migrationBuilder.CreateIndex(
                name: "IX_GOL_TimeId",
                table: "GOL",
                column: "TimeId");

            migrationBuilder.CreateIndex(
                name: "IX_JOGADOR_TimeId",
                table: "JOGADOR",
                column: "TimeId");

            migrationBuilder.CreateIndex(
                name: "IX_JOGO_Time1",
                table: "JOGO",
                column: "Time1");

            migrationBuilder.CreateIndex(
                name: "IX_JOGO_Time2",
                table: "JOGO",
                column: "Time2");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FICHA");

            migrationBuilder.DropTable(
                name: "GOL");

            migrationBuilder.DropTable(
                name: "JOGADOR");

            migrationBuilder.DropTable(
                name: "JOGO");

            migrationBuilder.DropTable(
                name: "TIME");
        }
    }
}
