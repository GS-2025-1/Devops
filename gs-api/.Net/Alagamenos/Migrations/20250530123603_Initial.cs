using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Alagamenos.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ESTADO",
                columns: table => new
                {
                    ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NOME_ESTADO = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ESTADO", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "USUARIO",
                columns: table => new
                {
                    ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NOME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    DATA_NASCIMENTO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    TELEFONE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    EMAIL = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    SENHA = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USUARIO", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "CIDADE",
                columns: table => new
                {
                    ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NOME_CIDADE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    ESTADO_ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CIDADE", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CIDADE_ESTADO_ESTADO_ID",
                        column: x => x.ESTADO_ID,
                        principalTable: "ESTADO",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BAIRRO",
                columns: table => new
                {
                    ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NOME_BAIRRO = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    CIDADE_ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BAIRRO", x => x.ID);
                    table.ForeignKey(
                        name: "FK_BAIRRO_CIDADE_CIDADE_ID",
                        column: x => x.CIDADE_ID,
                        principalTable: "CIDADE",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RUA",
                columns: table => new
                {
                    ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NOME_RUA = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    OBSERVACAO = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    BAIRRO_ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RUA", x => x.ID);
                    table.ForeignKey(
                        name: "FK_RUA_BAIRRO_BAIRRO_ID",
                        column: x => x.BAIRRO_ID,
                        principalTable: "BAIRRO",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ALERTA",
                columns: table => new
                {
                    ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    MENSAGEM = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    DATA_CRIACAO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    RUA_ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ALERTA", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ALERTA_RUA_RUA_ID",
                        column: x => x.RUA_ID,
                        principalTable: "RUA",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ENDERECO",
                columns: table => new
                {
                    ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NUMERO_ENDERECO = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    COMPLEMENTO = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    RUA_ID = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    USUARIO_ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ENDERECO", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ENDERECO_RUA_RUA_ID",
                        column: x => x.RUA_ID,
                        principalTable: "RUA",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ENDERECO_USUARIO_USUARIO_ID",
                        column: x => x.USUARIO_ID,
                        principalTable: "USUARIO",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "USUARIO_ALERTA",
                columns: table => new
                {
                    USUARIO_ID = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    ALERTA_ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USUARIO_ALERTA", x => new { x.USUARIO_ID, x.ALERTA_ID });
                    table.ForeignKey(
                        name: "FK_USUARIO_ALERTA_ALERTA_ALERTA_ID",
                        column: x => x.ALERTA_ID,
                        principalTable: "ALERTA",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_USUARIO_ALERTA_USUARIO_USUARIO_ID",
                        column: x => x.USUARIO_ID,
                        principalTable: "USUARIO",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ALERTA_RUA_ID",
                table: "ALERTA",
                column: "RUA_ID");

            migrationBuilder.CreateIndex(
                name: "IX_BAIRRO_CIDADE_ID",
                table: "BAIRRO",
                column: "CIDADE_ID");

            migrationBuilder.CreateIndex(
                name: "IX_CIDADE_ESTADO_ID",
                table: "CIDADE",
                column: "ESTADO_ID");

            migrationBuilder.CreateIndex(
                name: "IX_ENDERECO_RUA_ID",
                table: "ENDERECO",
                column: "RUA_ID");

            migrationBuilder.CreateIndex(
                name: "IX_ENDERECO_USUARIO_ID",
                table: "ENDERECO",
                column: "USUARIO_ID");

            migrationBuilder.CreateIndex(
                name: "IX_RUA_BAIRRO_ID",
                table: "RUA",
                column: "BAIRRO_ID");

            migrationBuilder.CreateIndex(
                name: "IX_USUARIO_ALERTA_ALERTA_ID",
                table: "USUARIO_ALERTA",
                column: "ALERTA_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ENDERECO");

            migrationBuilder.DropTable(
                name: "USUARIO_ALERTA");

            migrationBuilder.DropTable(
                name: "ALERTA");

            migrationBuilder.DropTable(
                name: "USUARIO");

            migrationBuilder.DropTable(
                name: "RUA");

            migrationBuilder.DropTable(
                name: "BAIRRO");

            migrationBuilder.DropTable(
                name: "CIDADE");

            migrationBuilder.DropTable(
                name: "ESTADO");
        }
    }
}
