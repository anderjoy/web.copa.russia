/*==============================================================*/
/* DBMS name:      Microsoft SQL Server 2016                    */
/* Created on:     27/03/2018 09:45:46                          */
/*==============================================================*/


if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('FICHA') and o.name = 'FK_FICHA_REFERENCE_JOGADOR')
alter table FICHA
   drop constraint FK_FICHA_REFERENCE_JOGADOR
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('JOGADOR') and o.name = 'FK_JOGADOR_REFERENCE_TIME')
alter table JOGADOR
   drop constraint FK_JOGADOR_REFERENCE_TIME
go

alter table FICHA
   drop constraint PK_FICHA
go

if exists (select 1
            from  sysobjects
           where  id = object_id('FICHA')
            and   type = 'U')
   drop table FICHA
go

alter table JOGADOR
   drop constraint PK_JOGADOR
go

if exists (select 1
            from  sysobjects
           where  id = object_id('JOGADOR')
            and   type = 'U')
   drop table JOGADOR
go

alter table TIME
   drop constraint PK_TIME
go

if exists (select 1
            from  sysobjects
           where  id = object_id('TIME')
            and   type = 'U')
   drop table TIME
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('GOL') and o.name = 'FK_GOL_REFERENCE_JOGO')
alter table GOL
   drop constraint FK_GOL_REFERENCE_JOGO
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('GOL') and o.name = 'FK_GOL_REFERENCE_TIME')
alter table GOL
   drop constraint FK_GOL_REFERENCE_TIME
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('GOL') and o.name = 'FK_GOL_REFERENCE_JOGADOR')
alter table GOL
   drop constraint FK_GOL_REFERENCE_JOGADOR
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('JOGO') and o.name = 'FK_JOGO_TIME1_TIME')
alter table JOGO
   drop constraint FK_JOGO_TIME1_TIME
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('JOGO') and o.name = 'FK_JOGO_TIME2_TIME')
alter table JOGO
   drop constraint FK_JOGO_TIME2_TIME
go

alter table GOL
   drop constraint PK_GOL
go

if exists (select 1
            from  sysobjects
           where  id = object_id('GOL')
            and   type = 'U')
   drop table GOL
go

alter table JOGO
   drop constraint PK_JOGO
go

if exists (select 1
            from  sysobjects
           where  id = object_id('JOGO')
            and   type = 'U')
   drop table JOGO
go

/*==============================================================*/
/* Table: FICHA                                                 */
/*==============================================================*/
create table FICHA (
   JOGADORID            Integer              not null,
   POSICAO              varchar(100)         not null,
   NATURALIDADE         varchar(100)         not null,
   ALTURA               numeric(3,2)         not null,
   CAMISA               integer              not null
)
go

alter table FICHA
   add constraint PK_FICHA primary key (JOGADORID)
go

/*==============================================================*/
/* Table: JOGADOR                                               */
/*==============================================================*/
create table JOGADOR (
   ID                   Integer              identity,
   NOME                 varchar(100)         not null,
   TIMEID               Integer              not null
)
go

alter table JOGADOR
   add constraint PK_JOGADOR primary key (ID)
go

/*==============================================================*/
/* Table: TIME                                                  */
/*==============================================================*/
create table TIME (
   ID                   integer              identity,
   PAIS                 varchar(50)          not null,
   BANDEIRA             varbinary(max)       null,
   NMTECNICO            varchar(100)         not null
)
go

alter table TIME
   add constraint PK_TIME primary key (ID)
go

alter table FICHA
   add constraint FK_FICHA_REFERENCE_JOGADOR foreign key (JOGADORID)
      references JOGADOR (ID)
go

alter table JOGADOR
   add constraint FK_JOGADOR_REFERENCE_TIME foreign key (TIMEID)
      references TIME (ID)
go

/*==============================================================*/
/* Table: GOL                                                   */
/*==============================================================*/
create table GOL (
   ID                   Integer              identity,
   JOGOID               Integer              not null,
   TIMEID               Integer              not null,
   JOGADORID            Integer              not null,
   HORA                 time                 not null
)
go

alter table GOL
   add constraint PK_GOL primary key (ID)
go

/*==============================================================*/
/* Table: JOGO                                                  */
/*==============================================================*/
create table JOGO (
   ID                   Integer              identity,
   TIME1                Integer              not null,
   TIME2                Integer              not null,
   DATA                 date                 not null
)
go

alter table JOGO
   add constraint PK_JOGO primary key (ID)
go

alter table GOL
   add constraint FK_GOL_REFERENCE_JOGO foreign key (JOGOID)
      references JOGO (ID)
go

alter table GOL
   add constraint FK_GOL_REFERENCE_TIME foreign key (TIMEID)
      references TIME (ID)
go

alter table GOL
   add constraint FK_GOL_REFERENCE_JOGADOR foreign key (JOGADORID)
      references JOGADOR (ID)
go

alter table JOGO
   add constraint FK_JOGO_TIME1_TIME foreign key (TIME1)
      references TIME (ID)
go

alter table JOGO
   add constraint FK_JOGO_TIME2_TIME foreign key (TIME2)
      references TIME (ID)
go
