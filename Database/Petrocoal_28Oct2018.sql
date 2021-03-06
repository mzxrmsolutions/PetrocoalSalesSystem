USE [master]
GO
/****** Object:  Database [Petrocoal]    Script Date: 10/28/2018 7:59:01 PM ******/
CREATE DATABASE [Petrocoal]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Petrocoal', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.RAHEEL\MSSQL\DATA\Petrocoal.mdf' , SIZE = 5120KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'Petrocoal_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.RAHEEL\MSSQL\DATA\Petrocoal_log.ldf' , SIZE = 10176KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [Petrocoal] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Petrocoal].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Petrocoal] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Petrocoal] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Petrocoal] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Petrocoal] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Petrocoal] SET ARITHABORT OFF 
GO
ALTER DATABASE [Petrocoal] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Petrocoal] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Petrocoal] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Petrocoal] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Petrocoal] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Petrocoal] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Petrocoal] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Petrocoal] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Petrocoal] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Petrocoal] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Petrocoal] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Petrocoal] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Petrocoal] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Petrocoal] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Petrocoal] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Petrocoal] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Petrocoal] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Petrocoal] SET RECOVERY FULL 
GO
ALTER DATABASE [Petrocoal] SET  MULTI_USER 
GO
ALTER DATABASE [Petrocoal] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Petrocoal] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Petrocoal] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Petrocoal] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [Petrocoal] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'Petrocoal', N'ON'
GO
USE [Petrocoal]
GO
/****** Object:  UserDefinedFunction [dbo].[fn_GenerateNextDCNumber]    Script Date: 10/28/2018 7:59:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: <Author,,Name>
-- Create date: <Create Date, ,>
-- Description: <Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[fn_GenerateNextDCNumber]
(
 
)
RETURNS varchar(50)
AS
BEGIN
 DECLARE @DCNumber varchar(50)
 DECLARE @DCNumberInt int
 
 SELECT TOP 1 @DCNumberInt = SUBSTRING(DCNumber, 4, LEN(DCNumber)) FROM DC ORDER BY Id DESC
 --select @DCNumber,@DCNumberInt
 IF(@DCNumberInt is  null or  LEN(@DCNumberInt) <1)
 BEGIN
 SET @DCNumberInt = '1000'
 END
 RETURN 'DC-' +CAST(( @DCNumberInt + 1) AS varchar(50))

END
GO
/****** Object:  UserDefinedFunction [dbo].[fn_GenerateNextDONumber]    Script Date: 10/28/2018 7:59:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: <Kashif Abbas>
-- Create date: <13th March 2018>
-- Description: <Generating Next DO Number>
-- =============================================
CREATE FUNCTION [dbo].[fn_GenerateNextDONumber]
(
 
)
RETURNS varchar(50)
AS
BEGIN
 DECLARE @DONubmer varchar(50)
 DECLARE @DONumberInt int
 
 SELECT TOP 1 @DONumberInt = SUBSTRING(DONumber, 4, LEN(DONumber)) FROM DO ORDER BY Id DESC
 
 IF(@DONumberInt is  null or  LEN(@DONumberInt) <1)
 BEGIN
 SET @DONumberInt = '1000'
 END

 RETURN 'DO-' +CAST(( @DONumberInt + 1) AS varchar(50))

END

GO
/****** Object:  UserDefinedFunction [dbo].[SplitString]    Script Date: 10/28/2018 7:59:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[SplitString] (@strString varchar(MAX))RETURNS @Result TABLE(Value varchar(8000))ASBEGIN  DECLARE @x XML  SELECT @x = CAST('<A>'+ REPLACE(@strString,',','</A><A>')+ '</A>' AS XML)  INSERT INTO @Result  SELECT t.value('.', 'varchar(50)') AS inVal FROM @x.nodes('/A') AS x(t) RETURNEND
GO
/****** Object:  Table [dbo].[Customer]    Script Date: 10/28/2018 7:59:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customer](
	[Id] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_Customer_Id]  DEFAULT (newid()),
	[Status] [int] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[CreatedBy] [uniqueidentifier] NOT NULL,
	[ModifiedOn] [datetime] NOT NULL,
	[ModifiedBy] [uniqueidentifier] NOT NULL,
	[Lead] [uniqueidentifier] NOT NULL,
	[FullName] [nvarchar](250) NOT NULL,
	[ShortName] [nvarchar](250) NULL,
	[NTN] [nvarchar](250) NULL,
	[STRN] [nvarchar](250) NULL,
	[Address] [nvarchar](250) NULL,
	[InvoiceAddress] [nvarchar](250) NULL,
	[Email] [nvarchar](250) NULL,
	[Phone] [nvarchar](250) NULL,
	[ContactPerson] [nvarchar](250) NULL,
	[HeadOffice] [nvarchar](250) NULL,
	[Remarks] [nvarchar](max) NULL,
 CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CustomerDestination]    Script Date: 10/28/2018 7:59:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CustomerDestination](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CustomerId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](250) NOT NULL,
	[Status] [int] NOT NULL,
 CONSTRAINT [PK_CustomerDestination] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CustomerStock]    Script Date: 10/28/2018 7:59:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CustomerStock](
	[Id] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[CustomerId] [uniqueidentifier] NOT NULL,
	[StoreId] [uniqueidentifier] NOT NULL,
	[Vessel] [int] NOT NULL,
	[Origin] [int] NOT NULL,
	[Size] [int] NOT NULL,
	[Quantity] [decimal](18, 0) NOT NULL,
	[Remarks] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_CustomerStock] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[DC]    Script Date: 10/28/2018 7:59:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DC](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[LeadId] [uniqueidentifier] NOT NULL,
	[Store] [uniqueidentifier] NOT NULL,
	[Status] [int] NOT NULL,
	[DCNumber] [varchar](50) NOT NULL,
	[DCDate] [datetime] NOT NULL,
	[Quantity] [decimal](18, 0) NOT NULL,
	[TruckNo] [varchar](50) NOT NULL,
	[BiltyNo] [varchar](50) NOT NULL,
	[SlipNo] [varchar](50) NOT NULL,
	[Weight] [decimal](18, 0) NOT NULL,
	[NetWeight] [decimal](18, 0) NOT NULL,
	[DriverName] [varchar](100) NOT NULL,
	[DriverPhone] [varchar](50) NULL,
	[Remarks] [varchar](250) NULL,
	[CreatedOn] [datetime] NOT NULL,
	[CreatedBy] [uniqueidentifier] NOT NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [uniqueidentifier] NULL,
	[TransporterId] [int] NOT NULL,
	[DOId] [int] NOT NULL,
 CONSTRAINT [PK_DC] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[DCL]    Script Date: 10/28/2018 7:59:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DCL](
	[Id] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_DCL_Id]  DEFAULT (newid()),
	[Status] [int] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[CreatedBy] [uniqueidentifier] NOT NULL,
	[ModifiedOn] [datetime] NOT NULL,
	[ModifiedBy] [uniqueidentifier] NOT NULL,
	[CompletedOn] [datetime] NULL,
	[DCLNumber] [nvarchar](50) NOT NULL,
	[DCLDate] [datetime] NOT NULL,
	[PODetailId] [uniqueidentifier] NOT NULL,
	[Store] [uniqueidentifier] NOT NULL,
	[Quantity] [decimal](18, 0) NOT NULL,
	[Remarks] [nvarchar](max) NULL,
 CONSTRAINT [PK_DCL] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[DO]    Script Date: 10/28/2018 7:59:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DO](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[StoreId] [uniqueidentifier] NULL,
	[SaleStationId] [uniqueidentifier] NOT NULL,
	[SOId] [int] NOT NULL,
	[LeadId] [uniqueidentifier] NOT NULL,
	[Status] [int] NOT NULL CONSTRAINT [DF_DO_Status]  DEFAULT ((1)),
	[CompletedOn] [datetime] NULL,
	[ApprovedDate] [datetime] NULL,
	[ApprovedBy] [uniqueidentifier] NULL,
	[DONumber] [varchar](50) NOT NULL,
	[DODate] [datetime] NOT NULL,
	[Quantity] [decimal](18, 0) NOT NULL,
	[LiftingStartDate] [datetime] NOT NULL,
	[LiftingEndDate] [datetime] NULL,
	[DeliveryDestination] [varchar](100) NULL,
	[TransporterId] [int] NULL,
	[DumperRate] [decimal](18, 0) NOT NULL,
	[FreightPaymentTerms] [decimal](18, 0) NULL,
	[FreightPerTon] [decimal](18, 0) NULL,
	[FreightTaxPerTon] [decimal](18, 0) NULL,
	[FreightComissionPSL] [decimal](18, 0) NULL,
	[FreightComissionAgent] [decimal](18, 0) NULL,
	[Remarks] [varchar](max) NULL,
	[CreatedOn] [datetime] NOT NULL,
	[CreatedBy] [uniqueidentifier] NOT NULL,
	[ModifiedOn] [datetime] NOT NULL,
	[ModifiedBy] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_DO] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ExceptionLogs]    Script Date: 10/28/2018 7:59:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ExceptionLogs](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ExceptionDate] [datetime] NOT NULL CONSTRAINT [DF_ExceptionLogs_ExceptionDate]  DEFAULT (getdate()),
	[ExceptionMessage] [nvarchar](200) NULL,
	[ExceptionType] [nvarchar](200) NULL,
	[ExceptionSource] [nvarchar](max) NULL,
	[ExceptionURL] [nvarchar](100) NULL,
	[ExceptionSystem] [nvarchar](50) NULL,
 CONSTRAINT [PK_ExceptionLogs] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[GRN]    Script Date: 10/28/2018 7:59:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GRN](
	[Id] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_GRN_Id]  DEFAULT (newid()),
	[Status] [int] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[CreatedBy] [uniqueidentifier] NOT NULL,
	[ModifiedOn] [datetime] NOT NULL,
	[ModifiedBy] [uniqueidentifier] NOT NULL,
	[CompletedOn] [datetime] NULL,
	[GRNNumber] [nvarchar](50) NOT NULL,
	[GRNDate] [datetime] NOT NULL,
	[PODetailId] [uniqueidentifier] NOT NULL,
	[Store] [uniqueidentifier] NOT NULL,
	[InvoiceNo] [nvarchar](50) NULL,
	[AdjPrice] [decimal](18, 0) NULL,
	[Quantity] [decimal](18, 0) NOT NULL,
	[Remarks] [nvarchar](max) NULL,
 CONSTRAINT [PK_GRN] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Organization]    Script Date: 10/28/2018 7:59:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Organization](
	[Id] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_Organization_Id]  DEFAULT (newid()),
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Organization] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Origin]    Script Date: 10/28/2018 7:59:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Origin](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Description] [varchar](250) NULL,
	[Status] [bit] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[CreatedBy] [uniqueidentifier] NOT NULL,
	[ModifiedOn] [datetime] NOT NULL,
	[ModifiedBy] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Origin] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PO]    Script Date: 10/28/2018 7:59:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PO](
	[Id] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_PO_Id]  DEFAULT (newid()),
	[Status] [int] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[CreatedBy] [uniqueidentifier] NOT NULL,
	[ModifiedOn] [datetime] NOT NULL,
	[ModifiedBy] [uniqueidentifier] NOT NULL,
	[CompletedOn] [datetime] NULL,
	[LeadId] [uniqueidentifier] NOT NULL,
	[ApprovedDate] [datetime] NULL,
	[ApprovedBy] [uniqueidentifier] NULL,
	[PONumber] [nvarchar](50) NOT NULL,
	[PODate] [datetime] NOT NULL,
	[Origin] [int] NOT NULL,
	[Size] [int] NOT NULL,
	[Vessel] [int] NOT NULL,
	[TargetDays] [int] NULL,
	[Supplier] [int] NULL,
	[TermsOfPayment] [nvarchar](50) NULL,
	[BufferQuantityMax] [decimal](18, 0) NULL,
	[BufferQuantityMin] [decimal](18, 0) NULL,
 CONSTRAINT [PK_PO] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PODetail]    Script Date: 10/28/2018 7:59:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PODetail](
	[Id] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_PODetail_Id]  DEFAULT (newid()),
	[POId] [uniqueidentifier] NULL,
	[CustomerId] [uniqueidentifier] NULL,
	[Quantity] [decimal](18, 0) NULL,
	[Rate] [decimal](18, 0) NOT NULL,
	[CostPerTon] [decimal](18, 0) NOT NULL,
	[AllowedWastage] [decimal](18, 3) NOT NULL,
	[TargetDate] [datetime] NULL,
	[Remarks] [nvarchar](max) NULL,
 CONSTRAINT [PK_PODetail] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Role]    Script Date: 10/28/2018 7:59:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Role](
	[Id] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_Role_Id]  DEFAULT (newid()),
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Role] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SaleStation]    Script Date: 10/28/2018 7:59:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SaleStation](
	[Id] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_SaleStation_Id]  DEFAULT (newid()),
	[Status] [int] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[CreatedBy] [uniqueidentifier] NOT NULL,
	[ModifiedOn] [datetime] NOT NULL,
	[ModifiedBy] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[OrganizationId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_SaleStation] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Seiving]    Script Date: 10/28/2018 7:59:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Seiving](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[SeivingNo] [nvarchar](50) NULL,
	[Origin] [int] NULL,
	[Date] [datetime] NULL,
	[StoreId] [nvarchar](50) NULL,
	[VesselId] [int] NULL,
	[CustomerId] [uniqueidentifier] NULL,
	[FromSize] [nvarchar](50) NULL,
	[FromQuantity] [decimal](18, 0) NULL,
	[Remarks] [nvarchar](max) NULL,
 CONSTRAINT [PK_Seiving] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SeivingSizeQty]    Script Date: 10/28/2018 7:59:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SeivingSizeQty](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[SeivingID] [int] NULL,
	[SizeId] [int] NULL,
	[SizeQuantity] [decimal](18, 0) NULL,
 CONSTRAINT [PK_SeivingSizeQty] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Size]    Script Date: 10/28/2018 7:59:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Size](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Description] [varchar](250) NULL,
	[Status] [bit] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[CreatedBy] [uniqueidentifier] NOT NULL,
	[ModifiedOn] [datetime] NOT NULL,
	[ModifiedBy] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Size] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SO]    Script Date: 10/28/2018 7:59:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SO](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LeadId] [uniqueidentifier] NOT NULL,
	[OriginId] [int] NOT NULL,
	[SizeId] [int] NOT NULL,
	[VesselId] [int] NOT NULL,
	[CustomerId] [uniqueidentifier] NOT NULL,
	[TaxRateId] [int] NULL,
	[TraderId] [int] NULL,
	[Status] [int] NOT NULL,
	[OrderType] [int] NOT NULL,
	[SONumber] [nvarchar](50) NOT NULL,
	[SODate] [datetime] NOT NULL,
	[SOExpiryDate] [datetime] NOT NULL,
	[PartyPONumber] [varchar](10) NOT NULL,
	[PartyPODate] [datetime] NOT NULL,
	[PartyPOExpiryDate] [datetime] NOT NULL,
	[CreditPeriod] [int] NOT NULL,
	[Quantity] [decimal](18, 0) NOT NULL,
	[AgreedRate] [decimal](18, 0) NULL,
	[TraderCommision] [decimal](18, 0) NULL,
	[CompletedOn] [datetime] NULL,
	[ApprovedDate] [datetime] NULL,
	[ApprovedBy] [uniqueidentifier] NULL,
	[CreatedOn] [datetime] NOT NULL,
	[CreatedBy] [uniqueidentifier] NOT NULL,
	[ModifiedOn] [datetime] NOT NULL,
	[ModifiedBy] [uniqueidentifier] NOT NULL,
	[POScannedImage] [varchar](50) NULL,
	[Remarks] [varchar](max) NULL,
	[BufferQuantityMax] [decimal](18, 0) NULL,
	[BufferQuantityMin] [decimal](18, 0) NULL,
 CONSTRAINT [PK_SO] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[StockMovement]    Script Date: 10/28/2018 7:59:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StockMovement](
	[Id] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_StockMovement_Id]  DEFAULT (newid()),
	[StoreId] [uniqueidentifier] NOT NULL,
	[CustomerId] [uniqueidentifier] NOT NULL,
	[Type] [int] NOT NULL,
	[Quantity] [decimal](18, 0) NOT NULL,
	[InOut] [bit] NOT NULL,
	[Reference] [nvarchar](50) NOT NULL,
	[Date] [datetime] NULL,
	[Vessel] [int] NOT NULL,
	[Origin] [int] NOT NULL,
	[Size] [int] NOT NULL,
	[Remarks] [nvarchar](max) NULL,
 CONSTRAINT [PK_StockMovement] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Store]    Script Date: 10/28/2018 7:59:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Store](
	[Id] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_Store_Id]  DEFAULT (newid()),
	[Status] [int] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[CreatedBy] [uniqueidentifier] NOT NULL,
	[ModifiedOn] [datetime] NOT NULL,
	[ModifiedBy] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Location] [nvarchar](50) NOT NULL,
	[Capacity] [decimal](18, 0) NOT NULL,
	[SaleStationId] [uniqueidentifier] NULL,
	[SubType] [uniqueidentifier] NULL,
 CONSTRAINT [PK_Store] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[StoreInOut]    Script Date: 10/28/2018 7:59:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StoreInOut](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Type] [int] NOT NULL,
	[Status] [int] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[CreatedBy] [uniqueidentifier] NOT NULL,
	[ModifiedOn] [datetime] NOT NULL,
	[ModifiedBy] [uniqueidentifier] NOT NULL,
	[CompletedOn] [datetime] NULL,
	[LeadId] [uniqueidentifier] NOT NULL,
	[SMNumber] [nvarchar](50) NOT NULL,
	[SMDate] [datetime] NOT NULL,
	[Origin] [int] NOT NULL,
	[Size] [int] NOT NULL,
	[Vessel] [int] NOT NULL,
	[Quantity] [decimal](18, 3) NULL,
	[FromStoreId] [uniqueidentifier] NOT NULL,
	[ToStoreId] [uniqueidentifier] NOT NULL,
	[VehicleNo] [nvarchar](50) NOT NULL,
	[BiltyNo] [nvarchar](50) NOT NULL,
	[BiltyDate] [datetime] NOT NULL,
	[RRInvoice] [nvarchar](50) NOT NULL,
	[CCMNumber] [nvarchar](50) NOT NULL,
	[Transporter] [int] NOT NULL,
	[StoreInDate] [datetime] NULL,
	[StoreInQuantity] [decimal](18, 3) NULL,
	[CustomerId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_StoreInOut] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Supplier]    Script Date: 10/28/2018 7:59:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Supplier](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Description] [varchar](250) NULL,
	[Status] [bit] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[CreatedBy] [uniqueidentifier] NOT NULL,
	[ModifiedOn] [datetime] NOT NULL,
	[ModifiedBy] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Supplier] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TaxRate]    Script Date: 10/28/2018 7:59:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TaxRate](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Description] [varchar](250) NULL,
	[Status] [bit] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[CreatedBy] [uniqueidentifier] NOT NULL,
	[ModifiedOn] [datetime] NOT NULL,
	[ModifiedBy] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_AgreedTaxRate] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Team]    Script Date: 10/28/2018 7:59:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Team](
	[Id] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[Status] [int] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[CreatedBy] [uniqueidentifier] NOT NULL,
	[ModifiedOn] [datetime] NOT NULL,
	[ModifiedBy] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[SaleStationId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Team] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Team_User]    Script Date: 10/28/2018 7:59:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Team_User](
	[Id] [int] NOT NULL,
	[TeamId] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[Status] [bit] NOT NULL CONSTRAINT [DF_Team_User_Status]  DEFAULT ((0)),
	[CreatedOn] [datetime] NOT NULL CONSTRAINT [DF_Team_User_CreatedOn]  DEFAULT (getdate()),
	[CreatedBy] [uniqueidentifier] NOT NULL,
	[ModifiedOn] [datetime] NOT NULL,
	[ModifiedBy] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Team_User] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Trader]    Script Date: 10/28/2018 7:59:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Trader](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Description] [varchar](250) NULL,
	[Status] [bit] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[CreatedBy] [uniqueidentifier] NOT NULL,
	[ModifiedOn] [datetime] NOT NULL,
	[ModifiedBy] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Trader] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Transporter]    Script Date: 10/28/2018 7:59:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Transporter](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Description] [varchar](250) NULL,
	[Status] [bit] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[CreatedBy] [uniqueidentifier] NOT NULL,
	[ModifiedOn] [datetime] NOT NULL,
	[ModifiedBy] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Transporter] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[User]    Script Date: 10/28/2018 7:59:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[Id] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_User_Id]  DEFAULT (newid()),
	[Status] [int] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[CreatedBy] [uniqueidentifier] NOT NULL,
	[ModifiedOn] [datetime] NOT NULL,
	[ModifiedBy] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[LoginName] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](50) NOT NULL,
	[Designation] [nvarchar](50) NULL,
	[Email] [nvarchar](50) NULL,
	[Mobile] [nvarchar](50) NULL,
	[Office] [nvarchar](50) NULL,
	[Home] [nvarchar](50) NULL,
	[Address] [nvarchar](50) NULL,
	[Picture] [nvarchar](50) NULL,
	[Remarks] [nvarchar](max) NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[User_Role]    Script Date: 10/28/2018 7:59:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User_Role](
	[Id] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_User_Role_id]  DEFAULT (newid()),
	[UserId] [uniqueidentifier] NOT NULL,
	[RoleId] [uniqueidentifier] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[CreatedBy] [uniqueidentifier] NOT NULL,
	[ModifiedOn] [datetime] NOT NULL,
	[ModifiedBy] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_User_Role] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Vessel]    Script Date: 10/28/2018 7:59:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Vessel](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Description] [varchar](250) NULL,
	[Status] [bit] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[CreatedBy] [uniqueidentifier] NOT NULL,
	[ModifiedOn] [datetime] NOT NULL,
	[ModifiedBy] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Vessel] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'0bef2889-0462-432e-a4d3-085b9821c7a1', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'-- PSL/Commercial --', N'PSL', N'11111111', N'XYZ', N'Evacuee Trust Islamabad Pakistan', N'Same', N'faisal@afriditraders.com', N'123456789', N'Faisal Nawaz', N'Islamabad', N'Commercial Coal')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'4994d1c6-ad3c-4be4-932e-08f992f54531', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'AALA PROCESSING MILLS', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'027b5ce4-03ab-46a1-b708-092aaeb0b9fa', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Reliance Textile Industries', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'24cbac57-cc6c-41b8-b5f8-0b561ddb2fb8', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Manzoor Sizing Industries', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'7bf9b13d-296b-4e71-8907-0d7fbf6334f8', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Amtex Limited', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'46beaf1b-b849-4360-aa7a-0e26da69842d', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Al Hamra Fabrics (Pvt) Limited (FSD)', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'a5c7d30f-b49a-4626-a9dc-0ea2e1b1dca1', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Pakistan Textile Processing-Cash', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'decf5498-93f5-431d-96a1-145c74d8a8bd', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Orient Coating Company', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'd5d3e763-ad38-4c9e-9448-14954ac773a5', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Kohinoor', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'034c87c4-24fb-4c41-9145-159b09baa2c3', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Al Mumtaz Textile Industries (Pvt) Ltd', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'ba0e2cfe-e743-4819-9ddf-1664d5d94e86', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Ittehad Textile Industries (Pvt) Limited', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'186e9015-b79c-49bd-993c-17ff0c12e623', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Zenotex (Pvt) Ltd.', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'77d6dc74-c818-455a-b305-24fde8ec4554', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Warptex Industries', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'dc66541b-4ec7-4fc5-b438-262a33437e9d', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'SM Textile', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'00701a5c-5e2f-4c9f-93d8-271afba096a6', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Kamal Textile Mills', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'6f7f248b-38d9-4213-96fc-27bbe6945eb7', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Karachi Dyeing', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'5f6981a1-624b-4f7f-8df0-29be24f34b47', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Abdul Rehman Corp. Pvt Ltd.', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'93649f0f-5e82-42af-b455-2cc8ff4a6b2d', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Sapphire Textile Mills Limited', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'e4411ea8-c991-4e16-85e4-2da6a0fe5a00', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Kausar Processing Industries Pvt. Ltd', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'8b8e97cd-4c10-4f6c-a728-2ea04b7335e9', 1, CAST(N'2018-10-02 20:16:54.637' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-10-02 20:16:54.637' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Raheel Ahmad', N'123', N'12312', N'123', N'Barrier 3  Wah Cantt', N'123', N'raheel_marx@outlook.com', N'03325035385', N'Raheel Ahmad', N'123', N'Test')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'53c08dc6-ec5b-4ae0-90b4-2f052d4661cf', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'National Silk', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'114f5a06-a5a6-47b1-984d-2fa254dce2f7', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Bismillah Sizing', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'a0bf9999-cc2f-460e-a017-314c7af75d7b', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Ashfaq Textile Mills Limited', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'79fd5cb8-3db7-4939-bff3-3412caff9154', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Eman Textile Processing Mills', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'2349684a-ecdd-419c-b0ab-34bca4384228', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Ali & Sameer International', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'218da152-48d5-43e1-80fd-3631da276ab7', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Karachi Dyeing (Pvt) Ltd', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'9c0944fe-8012-4fc1-8767-3738c0616a16', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'khyber Textile Industries.', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'68ea7ea8-ccab-4a05-90f1-388227c6dad5', 1, CAST(N'2018-10-02 18:49:38.407' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-10-02 18:49:38.407' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Raheel Ahmad', N'123', N'12312', N'123', N'Barrier 3  Wah Cantt', N'123', N'raheel_marx@outlook.com', N'03325035385', N'Raheel Ahmad', N'123', N'Test')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'538868a6-09e0-4cad-869f-38a6270fb5e2', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Naveed Munir Dyeing', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'de85fad4-c142-43ac-80b6-3e42807893f8', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Asif Finishing', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'ce3c5c88-6560-4aff-a2f0-49745bb40094', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Nishat Mills', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'd23c699d-daaf-4d47-8b95-49b2c3f815d5', 1, CAST(N'2018-03-29 16:02:56.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-03-29 16:02:56.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Zeeshan updated', N'MZee', N'123', N'1334', N'1aafsdfj sdflj sdfl ', N'sdlfjsdlkjsdflkj sdf', N'zeeshan@mzxrmsolutions.com', N'321654987', N'xyzsdf', N'sfdsdf', N'sdfsdf')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'7b638191-7600-4786-bbba-49fa5d895776', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Ali Textile', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'33e2297e-96ee-4556-81d7-4a1be591bbee', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Shalimar Industries', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'585c2cbe-c444-4bad-94f6-4a4eae4c3fc1', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'KAY & EMMS (Pvt)', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'3fbc5adf-627c-46eb-b608-4a5b8a6a4482', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Gillan Dyeing', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'5ad9179b-4633-46b6-b20a-4a9bb6a8494e', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Sapphire Diamond Fabrics', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'f1b3aeef-1e08-4acb-b437-4c6c83ee1463', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Raheem Kareem Dying', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'6a77f26d-6247-4eb9-b097-4e1960ac3cb2', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'SAHIB TEXTILES PVT LTD', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'd02f8410-6fed-4e8b-abda-542564117b9b', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Jhang Fabrics', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'1c8c6b38-d3ea-4cfd-a23d-560b200cdb36', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Ibrahim Dying', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'82163fec-01bf-4bcb-b4b0-560bc1b1b5ad', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Rashid Textile Printing (Pvt) Limited', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'338dd5b2-d6f3-4e18-9256-57157855766a', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Noor Habib Textile Industries (Pvt) Ltd', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'9c86ce9c-1131-479c-90c5-598d62d409e6', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Zimbis Knitwear (Pvt) Ltd', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'db6786e6-7711-40a9-bd46-5ec2b14e83c5', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Diamond Fabrics', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'b88c16b1-4484-44cf-ba56-5f02a72fa2e9', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Riaz Fabrics (Pvt) Ltd', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'8baf1dfa-026d-4e0b-8be1-607400b7235b', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Al Hamra Fabrics (Pvt) Limited', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'4ef67c1a-ec84-4157-9e04-60d5e9961d57', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Sarena Industries and Embroidery pvt Ltd', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'67dfe217-327b-484e-9192-61bec172e12b', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Niagara Mills (Pvt) Ltd.', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'9e12c96e-a1de-4407-a730-6303f212c4ec', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'modern Fabrics', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'21ac2880-470b-47f1-861b-681393622ff0', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Interloop Limited', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'c9cbb1d2-6e11-43a0-889f-68f8d8f7cfbe', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Rafique Processing Textile Industry', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'e374a522-69af-48a9-80a9-6a1c92394ee9', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Kamran Textile Private Limited', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'2dbc7344-f4e2-45fb-ba4e-71af02559b9c', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Jublee Textile', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'f9724507-4490-41a0-9089-722615ef9ebc', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Al Barka', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'69f40c34-f490-42a2-9543-72264634895b', 1, CAST(N'2018-10-02 20:23:45.867' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-10-02 20:23:45.867' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Raheel Ahmad', N'123', N'12312', N'123', N'Barrier 3  Wah Cantt', N'123', N'raheel_marx@outlook.com', N'03325035385', N'Raheel Ahmad', N'123', N'Test')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'ce98dd4c-4f52-4918-973d-72bf443b0f42', 1, CAST(N'2018-10-02 20:46:29.447' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-10-02 20:46:29.447' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Raheel Ahmad', N'123', N'12312', N'123', N'Barrier 3  Wah Cantt', N'123', N'raheel_marx@outlook.com', N'03325035385', N'Raheel Ahmad', N'123', N'Test')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'b0291fce-e6c2-4972-b1e3-73a32c8f46b0', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Hirra Terry Textile-Afg', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'fb239834-fa61-4c3e-95cc-744c72af178c', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Chaman Processing Industries (Pvt) Ltd', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'76773082-54e2-48e3-9f58-78a1e0d9d134', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'AMMAR BILAL TEXTILE', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'ccef174b-142c-4f2a-8db4-78afc0cec7e6', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Lahore Dying', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'e2114583-c739-4bdb-b2e6-7e765b7d3ab3', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Asco international', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'4f09e3b0-d42e-48ad-a402-7e91a2b8e4ad', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'HABIB FABRICS (PVT) LTD', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'1b2a4833-0ba8-4676-a6ee-87a43c5395a4', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Rafique Dyeing Industry', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'43cf03d4-6eba-481f-8dbd-8aaf27119949', 1, CAST(N'2018-10-02 18:49:38.407' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-10-02 18:49:38.407' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Raheel Ahmad', N'123', N'12312', N'123', N'Barrier 3  Wah Cantt', N'123', N'raheel_marx@outlook.com', N'03325035385', N'Raheel Ahmad', N'123', N'Test')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'4b0a2d99-fc0c-48dc-b0b8-8b2f60c54074', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Umer Sizing', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'436cba3f-e248-4a2b-8e42-8b34f3f7a312', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Kamal Limited Garment', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'16a3b640-7302-4d4f-872b-8cf727e32eb2', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Welcome textile', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'3e6a14af-29c8-40dd-b905-8ead6de3e2d9', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Rupali polyster', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'85ca4780-0e36-4fa2-8728-901c0e5a40fd', 1, CAST(N'2018-01-01 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-01 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Sapphire Finishing', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'2202aec3-eafe-4f88-b000-94a4942e604a', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Hilal Textile Corporation Pvt Ltd', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'edae6e0e-ffec-40c1-b47b-94bb6ccd5ea5', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Saad Textile Mills Pvt Ltd', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'65d6b616-ab16-416c-b4aa-96d6171980a2', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Zam Zam Textile Industry', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'e7873adf-09c3-42bc-964d-975c76f40412', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Magna', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'b63400ea-e529-4588-908a-989f9fd97ccb', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Zafar Fabrics', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'2dc55e02-a566-4046-b340-99c62fc13163', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Kamal Ltd', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'9e821382-0768-4998-82a9-9c00782dc601', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Pakistan Textile Processing', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'c9d0f1e0-e05e-49a0-9bc0-9ca25eed7fc9', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Arzoo Textile Mills Limited', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'213b7691-c9b3-427a-a30b-9d0dcc25b7ae', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Kalash Dying House', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'608d1132-6af4-49f0-bebf-a1ddfaa422fe', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Hunbul Tex (Pvt) Ltd', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'18e946e6-c9b5-47f1-9672-a3d4dc58eb98', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Modern Weaving Factory', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'5912cec8-6dc6-4976-a963-a473ca9d549b', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Nishat Chunian Limited', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'9e8673af-fc07-418f-b0c2-a62fdecad959', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'A.B Exports (Pvt) Limited', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'5cb6e819-4047-43f3-8172-abbf5b0a3c7c', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Dil Pasand Hosiery Pvt Ltd', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'9a5c86db-fc80-49e4-bb89-abe9739ac4fd', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Hina Sana Textile Mills (Pvt) Ltd', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'8b821b7b-9cec-4aa0-ac6f-ac02f5c53471', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Bismillah sizing', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'1f752911-b8bd-4fca-b4f7-ad4ee059f2fe', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'GOHAR TEXTILE MILLS (PVT) LTD', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'd10c061e-08d4-4d5f-a45f-ad9056982648', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Al-Jannat Processing Mills (Pvt) Ltd', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'b934d8b1-01d9-4cf8-a096-b13632f48dac', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Karam Textile', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'9b52dfaa-4348-4ba8-a218-b2f1b6a4a1da', 1, CAST(N'2018-10-13 02:24:00.030' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-10-13 02:24:00.030' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Raheel Ahmad', N'Raheel', N'12312', N'123', N'Barrier 3  Wah Cantt', N'123', N'raheel_marx@outlook.com', N'03325035385', N'Raheel Ahmad', N'123', N'12')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'2d2a4a68-257e-4e71-b11c-b55f80aa7303', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Opera Textile', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'a281df69-cbf9-4bfd-838e-b5e3abcaa42a', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'S.S.Fabrics (Crystal Dying)', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'57705b2d-6d22-4f52-9209-ba00a93cabb8', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'A.F Steel Re Rolling Mills', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'c2a19fc8-8a6d-481a-b936-c1663679dd2d', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Aruj Industries Limited', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'd08c750d-6ebc-4e70-baa4-c490d6589e1e', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'AHMED JAMAL TEXTILE MILLS', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'dc303180-a424-4c48-92ed-c65eb3c69520', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Rana Taxtile', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'628c6a1d-0e5f-4cc4-a5e9-c801bfe36422', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Master Textile Mills Limited', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'889b54c3-7aed-4fd1-ab7c-ca64db400a00', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Ajwa Textile (Pvt) Ltd', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'289bea81-9167-4cbd-ad41-ceb65c5181f2', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Masood Textile', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'39ccb9ef-d604-4745-acdb-cfbaab74332a', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Tariq Textile', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
GO
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'394ce95a-9a2c-4520-b289-d4660ad8e2dc', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'MAKKAH FABRICS', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'f015719c-9b0d-41b0-b39b-d58347bdeb72', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Bashir Printing Industries Pvt Ltd', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'bed4f4b8-26b5-419e-af01-d6d9a91b7500', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'SADAQAT LTD', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'89026ccb-db18-4398-aecc-d8d6ba6cd2ff', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Noor Fatima  Fabrics (Pvt) Ltd', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'fe020faf-a5a1-440b-a798-dbca2a6d7f91', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Bismillah Textile', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'595ec661-a4d8-4901-9edc-dcd71b967c7f', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Al Aziz Sizing Industry', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'7076b786-7f2c-452b-816e-e26a50c5bc2b', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'A.M Knit Wear', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'b4242dff-93d9-4b48-8b5c-e284ba6bbe24', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Moon Alhamra', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'bc746f17-04c9-4b1f-ac9a-eb599032f4d5', 1, CAST(N'2018-03-29 16:01:27.763' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-03-29 16:01:27.763' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Zeeshan', N'MZee', N'123', N'13', N'1aafsdfj sdflj sdfl ', N'sdlfjsdlkjsdflkj ', N'zeeshan@mzxrmsolutions.com', N'321654987', N'xyz', N'sfd', N'sdf')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'3dc83d5c-d29d-453e-8607-ec53f2851d13', 1, CAST(N'2018-10-02 20:40:16.787' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-10-02 20:40:16.787' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Raheel Ahmad', N'123', N'12312', N'123', N'Barrier 3  Wah Cantt', N'123', N'raheel_marx@outlook.com', N'03325035385', N'Raheel Ahmad', N'123', N'Test')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'1530166d-9b61-4b0e-98b5-ec6015b491e2', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Al Noor Dying', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'2142d74f-64ce-47f9-8cab-ed43151b1fcf', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Har Textile Mills (Pvt) Ltd', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'43f706fe-bd39-49b0-963a-f27489032292', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Al Hamra Hoisery', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'f7926a98-e35e-4838-8712-f45750330289', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Olympia Industries Private Limited', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'cc03fff2-5637-4c94-9075-f6e9b99b325c', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Kamal Hosery', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'61823356-4e68-442b-bf54-fbac77561ec4', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Sapphire Fibers', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'd970bc9a-9bef-4ca0-a0bc-fcb269d4c5a5', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Pakistan Textile Processing-Freight', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'022e39f6-3ec4-45e8-a0e5-fe4a33841a16', 1, CAST(N'2018-10-02 20:54:02.977' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-10-02 20:54:02.977' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Raheel Ali', N'123', N'12312', N'123', N'Rawalpindi Saddar', N'123', N'3@yahoo.com', N'03325035385', N'Raheel Ahmad', N'123', N'123')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'947263df-b758-4802-a955-ff2242f84bea', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'New Al-Riaz Fabrics', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Customer] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Lead], [FullName], [ShortName], [NTN], [STRN], [Address], [InvoiceAddress], [Email], [Phone], [ContactPerson], [HeadOffice], [Remarks]) VALUES (N'443d052e-ea66-4007-9cc2-ffe65799f9ba', 1, CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-18 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'AL HARAM TEXTILE INDUSTRIES PVT LTD', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'')
SET IDENTITY_INSERT [dbo].[CustomerDestination] ON 

INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (1, N'0bef2889-0462-432e-a4d3-085b9821c7a1', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (2, N'4994d1c6-ad3c-4be4-932e-08f992f54531', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (3, N'027b5ce4-03ab-46a1-b708-092aaeb0b9fa', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (4, N'24cbac57-cc6c-41b8-b5f8-0b561ddb2fb8', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (5, N'7bf9b13d-296b-4e71-8907-0d7fbf6334f8', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (6, N'46beaf1b-b849-4360-aa7a-0e26da69842d', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (7, N'a5c7d30f-b49a-4626-a9dc-0ea2e1b1dca1', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (8, N'decf5498-93f5-431d-96a1-145c74d8a8bd', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (9, N'd5d3e763-ad38-4c9e-9448-14954ac773a5', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (10, N'034c87c4-24fb-4c41-9145-159b09baa2c3', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (11, N'ba0e2cfe-e743-4819-9ddf-1664d5d94e86', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (12, N'186e9015-b79c-49bd-993c-17ff0c12e623', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (13, N'77d6dc74-c818-455a-b305-24fde8ec4554', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (14, N'dc66541b-4ec7-4fc5-b438-262a33437e9d', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (15, N'00701a5c-5e2f-4c9f-93d8-271afba096a6', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (16, N'6f7f248b-38d9-4213-96fc-27bbe6945eb7', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (17, N'5f6981a1-624b-4f7f-8df0-29be24f34b47', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (18, N'93649f0f-5e82-42af-b455-2cc8ff4a6b2d', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (19, N'e4411ea8-c991-4e16-85e4-2da6a0fe5a00', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (20, N'53c08dc6-ec5b-4ae0-90b4-2f052d4661cf', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (21, N'114f5a06-a5a6-47b1-984d-2fa254dce2f7', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (22, N'a0bf9999-cc2f-460e-a017-314c7af75d7b', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (23, N'79fd5cb8-3db7-4939-bff3-3412caff9154', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (24, N'2349684a-ecdd-419c-b0ab-34bca4384228', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (25, N'218da152-48d5-43e1-80fd-3631da276ab7', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (26, N'9c0944fe-8012-4fc1-8767-3738c0616a16', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (27, N'538868a6-09e0-4cad-869f-38a6270fb5e2', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (28, N'de85fad4-c142-43ac-80b6-3e42807893f8', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (29, N'ce3c5c88-6560-4aff-a2f0-49745bb40094', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (30, N'd23c699d-daaf-4d47-8b95-49b2c3f815d5', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (31, N'7b638191-7600-4786-bbba-49fa5d895776', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (32, N'33e2297e-96ee-4556-81d7-4a1be591bbee', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (33, N'585c2cbe-c444-4bad-94f6-4a4eae4c3fc1', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (34, N'3fbc5adf-627c-46eb-b608-4a5b8a6a4482', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (35, N'5ad9179b-4633-46b6-b20a-4a9bb6a8494e', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (36, N'f1b3aeef-1e08-4acb-b437-4c6c83ee1463', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (37, N'6a77f26d-6247-4eb9-b097-4e1960ac3cb2', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (38, N'd02f8410-6fed-4e8b-abda-542564117b9b', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (39, N'1c8c6b38-d3ea-4cfd-a23d-560b200cdb36', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (40, N'82163fec-01bf-4bcb-b4b0-560bc1b1b5ad', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (41, N'338dd5b2-d6f3-4e18-9256-57157855766a', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (42, N'9c86ce9c-1131-479c-90c5-598d62d409e6', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (43, N'db6786e6-7711-40a9-bd46-5ec2b14e83c5', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (44, N'b88c16b1-4484-44cf-ba56-5f02a72fa2e9', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (45, N'8baf1dfa-026d-4e0b-8be1-607400b7235b', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (46, N'4ef67c1a-ec84-4157-9e04-60d5e9961d57', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (47, N'67dfe217-327b-484e-9192-61bec172e12b', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (48, N'9e12c96e-a1de-4407-a730-6303f212c4ec', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (49, N'21ac2880-470b-47f1-861b-681393622ff0', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (50, N'c9cbb1d2-6e11-43a0-889f-68f8d8f7cfbe', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (51, N'e374a522-69af-48a9-80a9-6a1c92394ee9', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (52, N'2dbc7344-f4e2-45fb-ba4e-71af02559b9c', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (53, N'f9724507-4490-41a0-9089-722615ef9ebc', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (54, N'b0291fce-e6c2-4972-b1e3-73a32c8f46b0', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (55, N'fb239834-fa61-4c3e-95cc-744c72af178c', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (56, N'76773082-54e2-48e3-9f58-78a1e0d9d134', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (57, N'ccef174b-142c-4f2a-8db4-78afc0cec7e6', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (58, N'e2114583-c739-4bdb-b2e6-7e765b7d3ab3', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (59, N'4f09e3b0-d42e-48ad-a402-7e91a2b8e4ad', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (60, N'1b2a4833-0ba8-4676-a6ee-87a43c5395a4', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (61, N'4b0a2d99-fc0c-48dc-b0b8-8b2f60c54074', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (62, N'436cba3f-e248-4a2b-8e42-8b34f3f7a312', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (63, N'16a3b640-7302-4d4f-872b-8cf727e32eb2', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (64, N'3e6a14af-29c8-40dd-b905-8ead6de3e2d9', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (65, N'85ca4780-0e36-4fa2-8728-901c0e5a40fd', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (66, N'2202aec3-eafe-4f88-b000-94a4942e604a', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (67, N'edae6e0e-ffec-40c1-b47b-94bb6ccd5ea5', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (68, N'65d6b616-ab16-416c-b4aa-96d6171980a2', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (69, N'e7873adf-09c3-42bc-964d-975c76f40412', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (70, N'b63400ea-e529-4588-908a-989f9fd97ccb', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (71, N'2dc55e02-a566-4046-b340-99c62fc13163', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (72, N'9e821382-0768-4998-82a9-9c00782dc601', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (73, N'c9d0f1e0-e05e-49a0-9bc0-9ca25eed7fc9', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (74, N'213b7691-c9b3-427a-a30b-9d0dcc25b7ae', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (75, N'608d1132-6af4-49f0-bebf-a1ddfaa422fe', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (76, N'18e946e6-c9b5-47f1-9672-a3d4dc58eb98', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (77, N'5912cec8-6dc6-4976-a963-a473ca9d549b', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (78, N'9e8673af-fc07-418f-b0c2-a62fdecad959', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (79, N'5cb6e819-4047-43f3-8172-abbf5b0a3c7c', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (80, N'9a5c86db-fc80-49e4-bb89-abe9739ac4fd', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (81, N'8b821b7b-9cec-4aa0-ac6f-ac02f5c53471', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (82, N'1f752911-b8bd-4fca-b4f7-ad4ee059f2fe', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (83, N'd10c061e-08d4-4d5f-a45f-ad9056982648', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (84, N'b934d8b1-01d9-4cf8-a096-b13632f48dac', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (85, N'2d2a4a68-257e-4e71-b11c-b55f80aa7303', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (86, N'a281df69-cbf9-4bfd-838e-b5e3abcaa42a', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (87, N'57705b2d-6d22-4f52-9209-ba00a93cabb8', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (88, N'c2a19fc8-8a6d-481a-b936-c1663679dd2d', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (89, N'd08c750d-6ebc-4e70-baa4-c490d6589e1e', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (90, N'dc303180-a424-4c48-92ed-c65eb3c69520', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (91, N'628c6a1d-0e5f-4cc4-a5e9-c801bfe36422', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (92, N'889b54c3-7aed-4fd1-ab7c-ca64db400a00', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (93, N'289bea81-9167-4cbd-ad41-ceb65c5181f2', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (94, N'39ccb9ef-d604-4745-acdb-cfbaab74332a', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (95, N'394ce95a-9a2c-4520-b289-d4660ad8e2dc', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (96, N'f015719c-9b0d-41b0-b39b-d58347bdeb72', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (97, N'bed4f4b8-26b5-419e-af01-d6d9a91b7500', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (98, N'89026ccb-db18-4398-aecc-d8d6ba6cd2ff', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (99, N'fe020faf-a5a1-440b-a798-dbca2a6d7f91', N'default', 1)
GO
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (100, N'595ec661-a4d8-4901-9edc-dcd71b967c7f', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (101, N'7076b786-7f2c-452b-816e-e26a50c5bc2b', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (102, N'b4242dff-93d9-4b48-8b5c-e284ba6bbe24', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (103, N'bc746f17-04c9-4b1f-ac9a-eb599032f4d5', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (104, N'1530166d-9b61-4b0e-98b5-ec6015b491e2', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (105, N'2142d74f-64ce-47f9-8cab-ed43151b1fcf', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (106, N'43f706fe-bd39-49b0-963a-f27489032292', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (107, N'f7926a98-e35e-4838-8712-f45750330289', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (108, N'cc03fff2-5637-4c94-9075-f6e9b99b325c', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (109, N'61823356-4e68-442b-bf54-fbac77561ec4', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (110, N'd970bc9a-9bef-4ca0-a0bc-fcb269d4c5a5', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (111, N'947263df-b758-4802-a955-ff2242f84bea', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (112, N'443d052e-ea66-4007-9cc2-ffe65799f9ba', N'default', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (113, N'3dc83d5c-d29d-453e-8607-ec53f2851d13', N'Test Address 1', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (114, N'4994d1c6-ad3c-4be4-932e-08f992f54531', N'Test Address 1', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (115, N'4994d1c6-ad3c-4be4-932e-08f992f54531', N'Test Address 2', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (116, N'4994d1c6-ad3c-4be4-932e-08f992f54531', N'Test Address 3', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (117, N'022e39f6-3ec4-45e8-a0e5-fe4a33841a16', N'Test Address 1', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (118, N'022e39f6-3ec4-45e8-a0e5-fe4a33841a16', N'Test Address 2', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (119, N'9b52dfaa-4348-4ba8-a218-b2f1b6a4a1da', N'Test Address 1', 1)
INSERT [dbo].[CustomerDestination] ([Id], [CustomerId], [Name], [Status]) VALUES (120, N'9b52dfaa-4348-4ba8-a218-b2f1b6a4a1da', N'Test Address 2', 1)
SET IDENTITY_INSERT [dbo].[CustomerDestination] OFF
SET IDENTITY_INSERT [dbo].[DC] ON 

INSERT [dbo].[DC] ([ID], [LeadId], [Store], [Status], [DCNumber], [DCDate], [Quantity], [TruckNo], [BiltyNo], [SlipNo], [Weight], [NetWeight], [DriverName], [DriverPhone], [Remarks], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [TransporterId], [DOId]) VALUES (5, N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'0ce63c9c-9e2c-4535-a8d0-5ccec93eca6c', 0, N'DC-1001', CAST(N'2018-04-04 00:00:00.000' AS DateTime), CAST(2000 AS Decimal(18, 0)), N'er', N'sdf', N'sdf', CAST(100 AS Decimal(18, 0)), CAST(120 AS Decimal(18, 0)), N'sdf', N'3015502004', N'dfghj', CAST(N'2018-04-04 13:05:07.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-10-06 14:41:28.660' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', 1, 1005)
INSERT [dbo].[DC] ([ID], [LeadId], [Store], [Status], [DCNumber], [DCDate], [Quantity], [TruckNo], [BiltyNo], [SlipNo], [Weight], [NetWeight], [DriverName], [DriverPhone], [Remarks], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [TransporterId], [DOId]) VALUES (6, N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'0ce63c9c-9e2c-4535-a8d0-5ccec93eca6c', 0, N'DC-1002', CAST(N'2018-04-04 00:00:00.000' AS DateTime), CAST(200 AS Decimal(18, 0)), N'er', N'sdf', N'sdf', CAST(1000 AS Decimal(18, 0)), CAST(1200 AS Decimal(18, 0)), N'sdf', N'3015502004', N'', CAST(N'2018-04-04 14:53:14.093' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-04-04 14:53:14.093' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', 1, 1006)
INSERT [dbo].[DC] ([ID], [LeadId], [Store], [Status], [DCNumber], [DCDate], [Quantity], [TruckNo], [BiltyNo], [SlipNo], [Weight], [NetWeight], [DriverName], [DriverPhone], [Remarks], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [TransporterId], [DOId]) VALUES (7, N'00000000-0000-0000-0000-000000000000', N'0ce63c9c-9e2c-4535-a8d0-5ccec93eca6c', 0, N'DC-1003', CAST(N'2018-12-12 00:00:00.000' AS DateTime), CAST(0 AS Decimal(18, 0)), N'12', N'12', N'12', CAST(12 AS Decimal(18, 0)), CAST(12 AS Decimal(18, 0)), N'Ali', N'03333333333', N'Test', CAST(N'2018-10-10 14:05:13.347' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-10-10 14:05:13.347' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', 1, 1012)
INSERT [dbo].[DC] ([ID], [LeadId], [Store], [Status], [DCNumber], [DCDate], [Quantity], [TruckNo], [BiltyNo], [SlipNo], [Weight], [NetWeight], [DriverName], [DriverPhone], [Remarks], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [TransporterId], [DOId]) VALUES (8, N'00000000-0000-0000-0000-000000000000', N'0ce63c9c-9e2c-4535-a8d0-5ccec93eca6c', 0, N'DC-1004', CAST(N'2018-12-12 00:00:00.000' AS DateTime), CAST(0 AS Decimal(18, 0)), N'12', N'12', N'12', CAST(12 AS Decimal(18, 0)), CAST(12 AS Decimal(18, 0)), N'Ali Kahn', N'03333333333', N'Test', CAST(N'2018-10-10 14:06:47.557' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-10-10 14:06:47.557' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', 2, 1012)
INSERT [dbo].[DC] ([ID], [LeadId], [Store], [Status], [DCNumber], [DCDate], [Quantity], [TruckNo], [BiltyNo], [SlipNo], [Weight], [NetWeight], [DriverName], [DriverPhone], [Remarks], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [TransporterId], [DOId]) VALUES (9, N'00000000-0000-0000-0000-000000000000', N'0ce63c9c-9e2c-4535-a8d0-5ccec93eca6c', 0, N'DC-1005', CAST(N'2018-12-12 00:00:00.000' AS DateTime), CAST(0 AS Decimal(18, 0)), N'13', N'13', N'13', CAST(13 AS Decimal(18, 0)), CAST(13 AS Decimal(18, 0)), N'Khan Ali', N'09999999999', N'Testing', CAST(N'2018-10-10 14:06:48.617' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-10-10 14:06:48.617' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', 2, 1012)
INSERT [dbo].[DC] ([ID], [LeadId], [Store], [Status], [DCNumber], [DCDate], [Quantity], [TruckNo], [BiltyNo], [SlipNo], [Weight], [NetWeight], [DriverName], [DriverPhone], [Remarks], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [TransporterId], [DOId]) VALUES (10, N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'0ce63c9c-9e2c-4535-a8d0-5ccec93eca6c', 0, N'DC-1006', CAST(N'2018-11-12 00:00:00.000' AS DateTime), CAST(0 AS Decimal(18, 0)), N'12', N'12', N'12', CAST(12 AS Decimal(18, 0)), CAST(12 AS Decimal(18, 0)), N'Ali', N'03333333333', N'Test', CAST(N'2018-10-10 18:26:41.553' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-10-10 18:26:42.190' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', 1, 1012)
INSERT [dbo].[DC] ([ID], [LeadId], [Store], [Status], [DCNumber], [DCDate], [Quantity], [TruckNo], [BiltyNo], [SlipNo], [Weight], [NetWeight], [DriverName], [DriverPhone], [Remarks], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [TransporterId], [DOId]) VALUES (11, N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'0ce63c9c-9e2c-4535-a8d0-5ccec93eca6c', 0, N'DC-1007', CAST(N'2019-12-12 00:00:00.000' AS DateTime), CAST(0 AS Decimal(18, 0)), N'13', N'13', N'13', CAST(13 AS Decimal(18, 0)), CAST(13 AS Decimal(18, 0)), N'Khan Ali', N'09999999999', N'Testing', CAST(N'2018-10-10 18:27:17.487' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-10-10 18:27:17.487' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', 1, 1012)
INSERT [dbo].[DC] ([ID], [LeadId], [Store], [Status], [DCNumber], [DCDate], [Quantity], [TruckNo], [BiltyNo], [SlipNo], [Weight], [NetWeight], [DriverName], [DriverPhone], [Remarks], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [TransporterId], [DOId]) VALUES (12, N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'0ce63c9c-9e2c-4535-a8d0-5ccec93eca6c', 0, N'DC-1008', CAST(N'2018-12-12 00:00:00.000' AS DateTime), CAST(12 AS Decimal(18, 0)), N'12', N'12', N'12', CAST(12 AS Decimal(18, 0)), CAST(12 AS Decimal(18, 0)), N'Ali', N'03333333333', N'Test', CAST(N'2018-10-10 18:41:38.310' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-10-10 18:41:38.310' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', 1, 1012)
INSERT [dbo].[DC] ([ID], [LeadId], [Store], [Status], [DCNumber], [DCDate], [Quantity], [TruckNo], [BiltyNo], [SlipNo], [Weight], [NetWeight], [DriverName], [DriverPhone], [Remarks], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [TransporterId], [DOId]) VALUES (13, N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'0ce63c9c-9e2c-4535-a8d0-5ccec93eca6c', 0, N'DC-1009', CAST(N'2018-12-11 00:00:00.000' AS DateTime), CAST(13 AS Decimal(18, 0)), N'13', N'13', N'13', CAST(13 AS Decimal(18, 0)), CAST(13 AS Decimal(18, 0)), N'Khan Ali', N'09999999999', N'Testing', CAST(N'2018-10-10 18:41:39.640' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-10-10 18:41:39.640' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', 2, 1012)
INSERT [dbo].[DC] ([ID], [LeadId], [Store], [Status], [DCNumber], [DCDate], [Quantity], [TruckNo], [BiltyNo], [SlipNo], [Weight], [NetWeight], [DriverName], [DriverPhone], [Remarks], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [TransporterId], [DOId]) VALUES (14, N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'0ce63c9c-9e2c-4535-a8d0-5ccec93eca6c', 0, N'DC-1010', CAST(N'2018-10-10 00:00:00.000' AS DateTime), CAST(120 AS Decimal(18, 0)), N'120', N'120', N'120', CAST(120 AS Decimal(18, 0)), CAST(120 AS Decimal(18, 0)), N'120', N'120', N'Testing', CAST(N'2018-10-10 18:51:51.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-10-13 05:09:50.610' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', 2, 1012)
INSERT [dbo].[DC] ([ID], [LeadId], [Store], [Status], [DCNumber], [DCDate], [Quantity], [TruckNo], [BiltyNo], [SlipNo], [Weight], [NetWeight], [DriverName], [DriverPhone], [Remarks], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [TransporterId], [DOId]) VALUES (15, N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'0ce63c9c-9e2c-4535-a8d0-5ccec93eca6c', 0, N'DC-1011', CAST(N'2018-10-13 00:00:00.000' AS DateTime), CAST(12 AS Decimal(18, 0)), N'12', N'1', N'12', CAST(1 AS Decimal(18, 0)), CAST(12 AS Decimal(18, 0)), N'Ali', N'03333333333', N'Test', CAST(N'2018-10-13 02:21:30.677' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-10-13 02:21:30.677' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', 2, 1012)
INSERT [dbo].[DC] ([ID], [LeadId], [Store], [Status], [DCNumber], [DCDate], [Quantity], [TruckNo], [BiltyNo], [SlipNo], [Weight], [NetWeight], [DriverName], [DriverPhone], [Remarks], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [TransporterId], [DOId]) VALUES (16, N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'0ce63c9c-9e2c-4535-a8d0-5ccec93eca6c', 0, N'DC-1012', CAST(N'2018-10-13 00:00:00.000' AS DateTime), CAST(13 AS Decimal(18, 0)), N'13', N'13', N'13', CAST(13 AS Decimal(18, 0)), CAST(13 AS Decimal(18, 0)), N'Khan Ali', N'09999999999', N'Testing', CAST(N'2018-10-13 02:21:30.693' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-10-13 02:21:30.693' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', 2, 1012)
INSERT [dbo].[DC] ([ID], [LeadId], [Store], [Status], [DCNumber], [DCDate], [Quantity], [TruckNo], [BiltyNo], [SlipNo], [Weight], [NetWeight], [DriverName], [DriverPhone], [Remarks], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [TransporterId], [DOId]) VALUES (17, N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'0ce63c9c-9e2c-4535-a8d0-5ccec93eca6c', 0, N'DC-1013', CAST(N'2018-10-22 00:00:00.000' AS DateTime), CAST(12 AS Decimal(18, 0)), N'12', N'12', N'12', CAST(30 AS Decimal(18, 0)), CAST(12 AS Decimal(18, 0)), N'Ali', N'03333333333', N'Test', CAST(N'2018-10-22 20:15:56.020' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-10-22 20:15:56.020' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', 1, 1017)
INSERT [dbo].[DC] ([ID], [LeadId], [Store], [Status], [DCNumber], [DCDate], [Quantity], [TruckNo], [BiltyNo], [SlipNo], [Weight], [NetWeight], [DriverName], [DriverPhone], [Remarks], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [TransporterId], [DOId]) VALUES (18, N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'0ce63c9c-9e2c-4535-a8d0-5ccec93eca6c', 0, N'DC-1014', CAST(N'2018-10-22 00:00:00.000' AS DateTime), CAST(13 AS Decimal(18, 0)), N'1300', N'13', N'13', CAST(300 AS Decimal(18, 0)), CAST(13 AS Decimal(18, 0)), N'Khan Ali', N'09999999999', N'Testing 1200', CAST(N'2018-10-22 20:15:56.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-10-22 20:16:29.830' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', 2, 1017)
INSERT [dbo].[DC] ([ID], [LeadId], [Store], [Status], [DCNumber], [DCDate], [Quantity], [TruckNo], [BiltyNo], [SlipNo], [Weight], [NetWeight], [DriverName], [DriverPhone], [Remarks], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [TransporterId], [DOId]) VALUES (19, N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'43961369-a7e4-4fcc-8756-4359cb20aae5', 0, N'DC-1015', CAST(N'2018-10-26 00:00:00.000' AS DateTime), CAST(12 AS Decimal(18, 0)), N'12', N'12', N'12', CAST(12 AS Decimal(18, 0)), CAST(12 AS Decimal(18, 0)), N'Ali', N'03333333333', N'Test', CAST(N'2018-10-26 20:28:16.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-10-26 20:39:59.613' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', 1, 1018)
INSERT [dbo].[DC] ([ID], [LeadId], [Store], [Status], [DCNumber], [DCDate], [Quantity], [TruckNo], [BiltyNo], [SlipNo], [Weight], [NetWeight], [DriverName], [DriverPhone], [Remarks], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [TransporterId], [DOId]) VALUES (20, N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'8b13ea2a-67e4-487f-8f02-11c9be1a1983', 0, N'DC-1016', CAST(N'2018-10-26 00:00:00.000' AS DateTime), CAST(12 AS Decimal(18, 0)), N'12', N'12', N'12', CAST(12 AS Decimal(18, 0)), CAST(12 AS Decimal(18, 0)), N'Ali', N'03333333333', N'Test', CAST(N'2018-10-26 21:27:14.313' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-10-26 21:27:14.313' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', 1, 1018)
INSERT [dbo].[DC] ([ID], [LeadId], [Store], [Status], [DCNumber], [DCDate], [Quantity], [TruckNo], [BiltyNo], [SlipNo], [Weight], [NetWeight], [DriverName], [DriverPhone], [Remarks], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [TransporterId], [DOId]) VALUES (21, N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'43961369-a7e4-4fcc-8756-4359cb20aae5', 0, N'DC-1017', CAST(N'2018-10-26 00:00:00.000' AS DateTime), CAST(67 AS Decimal(18, 0)), N'13', N'13', N'13', CAST(13 AS Decimal(18, 0)), CAST(13 AS Decimal(18, 0)), N'Khan Ali', N'09999999999', N'Testing', CAST(N'2018-10-26 21:27:14.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-10-26 21:41:49.223' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', 2, 1018)
INSERT [dbo].[DC] ([ID], [LeadId], [Store], [Status], [DCNumber], [DCDate], [Quantity], [TruckNo], [BiltyNo], [SlipNo], [Weight], [NetWeight], [DriverName], [DriverPhone], [Remarks], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [TransporterId], [DOId]) VALUES (22, N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'0ce63c9c-9e2c-4535-a8d0-5ccec93eca6c', 0, N'DC-1018', CAST(N'2018-10-27 00:00:00.000' AS DateTime), CAST(12 AS Decimal(18, 0)), N'12', N'12', N'12', CAST(12 AS Decimal(18, 0)), CAST(12 AS Decimal(18, 0)), N'Ali', N'03333333333', N'Test', CAST(N'2018-10-27 20:39:11.470' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-10-27 20:39:11.470' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', 1, 1020)
INSERT [dbo].[DC] ([ID], [LeadId], [Store], [Status], [DCNumber], [DCDate], [Quantity], [TruckNo], [BiltyNo], [SlipNo], [Weight], [NetWeight], [DriverName], [DriverPhone], [Remarks], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [TransporterId], [DOId]) VALUES (23, N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'c0ffef5f-60af-485f-96f5-9bbb69223679', 0, N'DC-1019', CAST(N'2018-10-27 00:00:00.000' AS DateTime), CAST(13 AS Decimal(18, 0)), N'13', N'13', N'13', CAST(13 AS Decimal(18, 0)), CAST(13 AS Decimal(18, 0)), N'Khan Ali', N'09999999999', N'Testing', CAST(N'2018-10-27 20:39:11.537' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-10-27 20:39:11.537' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', 2, 1020)
INSERT [dbo].[DC] ([ID], [LeadId], [Store], [Status], [DCNumber], [DCDate], [Quantity], [TruckNo], [BiltyNo], [SlipNo], [Weight], [NetWeight], [DriverName], [DriverPhone], [Remarks], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [TransporterId], [DOId]) VALUES (24, N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'c0ffef5f-60af-485f-96f5-9bbb69223679', 0, N'DC-1020', CAST(N'2018-10-27 00:00:00.000' AS DateTime), CAST(12 AS Decimal(18, 0)), N'12', N'12', N'12', CAST(12 AS Decimal(18, 0)), CAST(12 AS Decimal(18, 0)), N'Ali', N'03333333333', N'Test', CAST(N'2018-10-27 20:43:29.217' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-10-27 20:43:29.217' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', 2, 1020)
INSERT [dbo].[DC] ([ID], [LeadId], [Store], [Status], [DCNumber], [DCDate], [Quantity], [TruckNo], [BiltyNo], [SlipNo], [Weight], [NetWeight], [DriverName], [DriverPhone], [Remarks], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [TransporterId], [DOId]) VALUES (25, N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'0ce63c9c-9e2c-4535-a8d0-5ccec93eca6c', 0, N'DC-1021', CAST(N'2018-10-27 00:00:00.000' AS DateTime), CAST(150 AS Decimal(18, 0)), N'12', N'12', N'12', CAST(12 AS Decimal(18, 0)), CAST(12 AS Decimal(18, 0)), N'Ali', N'03333333333', N'Test', CAST(N'2018-10-27 20:47:10.630' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-10-27 20:47:10.630' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', 1, 1020)
INSERT [dbo].[DC] ([ID], [LeadId], [Store], [Status], [DCNumber], [DCDate], [Quantity], [TruckNo], [BiltyNo], [SlipNo], [Weight], [NetWeight], [DriverName], [DriverPhone], [Remarks], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [TransporterId], [DOId]) VALUES (26, N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'8b13ea2a-67e4-487f-8f02-11c9be1a1983', 0, N'DC-1022', CAST(N'2018-10-28 00:00:00.000' AS DateTime), CAST(150 AS Decimal(18, 0)), N'12', N'12', N'12', CAST(12 AS Decimal(18, 0)), CAST(12 AS Decimal(18, 0)), N'Ali', N'03333333333', N'Test', CAST(N'2018-10-28 16:23:21.420' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-10-28 16:23:21.420' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', 1, 1021)
INSERT [dbo].[DC] ([ID], [LeadId], [Store], [Status], [DCNumber], [DCDate], [Quantity], [TruckNo], [BiltyNo], [SlipNo], [Weight], [NetWeight], [DriverName], [DriverPhone], [Remarks], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [TransporterId], [DOId]) VALUES (27, N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'b37a4521-51bd-4e13-a317-2ff51f8c6280', 0, N'DC-1023', CAST(N'2018-10-28 00:00:00.000' AS DateTime), CAST(400 AS Decimal(18, 0)), N'13', N'13', N'13', CAST(13 AS Decimal(18, 0)), CAST(13 AS Decimal(18, 0)), N'Khan Ali', N'09999999999', N'Testing', CAST(N'2018-10-28 16:23:21.433' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-10-28 16:23:21.433' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', 1, 1021)
SET IDENTITY_INSERT [dbo].[DC] OFF
INSERT [dbo].[DCL] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [CompletedOn], [DCLNumber], [DCLDate], [PODetailId], [Store], [Quantity], [Remarks]) VALUES (N'1f96936d-a5fa-4b39-9d8d-0bec36c7f4a2', 1, CAST(N'2018-04-04 14:33:58.683' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-04-04 14:33:58.683' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', NULL, N'DCL-1002', CAST(N'2018-04-04 14:33:58.683' AS DateTime), N'9c80abf1-9791-4d95-a0af-9e9047d94ca3', N'8b90f160-1e81-41ed-93ee-b1f7ea3e97f2', CAST(5000 AS Decimal(18, 0)), N'sdfg')
INSERT [dbo].[DCL] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [CompletedOn], [DCLNumber], [DCLDate], [PODetailId], [Store], [Quantity], [Remarks]) VALUES (N'a45e60c5-14ad-41a2-9c8c-2c6d61276fac', 1, CAST(N'2018-04-04 14:36:08.107' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-04-04 14:36:08.107' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', NULL, N'DCL-1004', CAST(N'2018-04-04 14:36:08.107' AS DateTime), N'c2f29cd0-3309-4a4a-a194-fae1315e4cba', N'8b90f160-1e81-41ed-93ee-b1f7ea3e97f2', CAST(10000 AS Decimal(18, 0)), N'sdfg')
INSERT [dbo].[DCL] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [CompletedOn], [DCLNumber], [DCLDate], [PODetailId], [Store], [Quantity], [Remarks]) VALUES (N'95eea7e4-dbf1-4f65-832d-7c00dbc135c5', 1, CAST(N'2018-04-04 14:34:29.077' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-04-04 14:34:29.077' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', NULL, N'DCL-1003', CAST(N'2018-04-04 14:34:29.077' AS DateTime), N'95b4b3a4-28e0-4afd-81b0-a0f59bc4d833', N'8b90f160-1e81-41ed-93ee-b1f7ea3e97f2', CAST(5000 AS Decimal(18, 0)), N'sdfg')
INSERT [dbo].[DCL] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [CompletedOn], [DCLNumber], [DCLDate], [PODetailId], [Store], [Quantity], [Remarks]) VALUES (N'9b8996b0-6446-41f3-9fb1-82e771f018ea', 1, CAST(N'2018-08-09 12:20:55.700' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-08-09 12:20:55.700' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', NULL, N'DCL-1005', CAST(N'2018-08-09 12:20:55.700' AS DateTime), N'1c850e76-dc7e-4dd4-ab86-5eaa92bce505', N'8b13ea2a-67e4-487f-8f02-11c9be1a1983', CAST(20000 AS Decimal(18, 0)), N'sdfasdf')
INSERT [dbo].[DCL] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [CompletedOn], [DCLNumber], [DCLDate], [PODetailId], [Store], [Quantity], [Remarks]) VALUES (N'422d6d1c-445d-4f06-8042-be2e6c41f4e3', 1, CAST(N'2018-08-09 14:11:48.373' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-08-09 14:11:48.373' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', NULL, N'DCL-1006', CAST(N'2018-08-09 00:00:00.000' AS DateTime), N'05e79baa-78e4-4e8f-8df3-d3c810226614', N'8b13ea2a-67e4-487f-8f02-11c9be1a1983', CAST(50000 AS Decimal(18, 0)), N'54')
SET IDENTITY_INSERT [dbo].[DO] ON 

INSERT [dbo].[DO] ([ID], [StoreId], [SaleStationId], [SOId], [LeadId], [Status], [CompletedOn], [ApprovedDate], [ApprovedBy], [DONumber], [DODate], [Quantity], [LiftingStartDate], [LiftingEndDate], [DeliveryDestination], [TransporterId], [DumperRate], [FreightPaymentTerms], [FreightPerTon], [FreightTaxPerTon], [FreightComissionPSL], [FreightComissionAgent], [Remarks], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (1005, NULL, N'8b90f160-1e81-41ed-93ee-b1f7ea3e97f2', 6, N'1ee1700d-1ca9-4c56-983a-0285f581379e', 3, NULL, CAST(N'2018-04-04 13:03:14.817' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'DO-1001', CAST(N'2018-04-04 00:00:00.000' AS DateTime), CAST(5000 AS Decimal(18, 0)), CAST(N'2018-04-04 00:00:00.000' AS DateTime), CAST(N'2018-04-04 00:00:00.000' AS DateTime), N'114', 1, CAST(12 AS Decimal(18, 0)), CAST(12 AS Decimal(18, 0)), CAST(12 AS Decimal(18, 0)), CAST(12 AS Decimal(18, 0)), CAST(12 AS Decimal(18, 0)), CAST(12 AS Decimal(18, 0)), N'sdfg', CAST(N'2018-04-04 13:01:41.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-04-04 13:03:14.817' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e')
INSERT [dbo].[DO] ([ID], [StoreId], [SaleStationId], [SOId], [LeadId], [Status], [CompletedOn], [ApprovedDate], [ApprovedBy], [DONumber], [DODate], [Quantity], [LiftingStartDate], [LiftingEndDate], [DeliveryDestination], [TransporterId], [DumperRate], [FreightPaymentTerms], [FreightPerTon], [FreightTaxPerTon], [FreightComissionPSL], [FreightComissionAgent], [Remarks], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (1006, NULL, N'2e92b454-74a2-4dbd-90e5-f585d5d47e80', 7, N'1ee1700d-1ca9-4c56-983a-0285f581379e', 1, NULL, NULL, NULL, N'DO-1002', CAST(N'2018-04-04 00:00:00.000' AS DateTime), CAST(500 AS Decimal(18, 0)), CAST(N'2018-04-04 00:00:00.000' AS DateTime), CAST(N'2018-04-04 00:00:00.000' AS DateTime), N'2', 1, CAST(23 AS Decimal(18, 0)), CAST(12 AS Decimal(18, 0)), CAST(12 AS Decimal(18, 0)), CAST(12 AS Decimal(18, 0)), CAST(12 AS Decimal(18, 0)), CAST(12 AS Decimal(18, 0)), N'dfgh', CAST(N'2018-04-04 14:44:25.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-04-11 18:41:58.413' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e')
INSERT [dbo].[DO] ([ID], [StoreId], [SaleStationId], [SOId], [LeadId], [Status], [CompletedOn], [ApprovedDate], [ApprovedBy], [DONumber], [DODate], [Quantity], [LiftingStartDate], [LiftingEndDate], [DeliveryDestination], [TransporterId], [DumperRate], [FreightPaymentTerms], [FreightPerTon], [FreightTaxPerTon], [FreightComissionPSL], [FreightComissionAgent], [Remarks], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (1008, N'0ce63c9c-9e2c-4535-a8d0-5ccec93eca6c', N'9ef74e38-8e39-40de-a727-d56f745a6042', 7, N'1ee1700d-1ca9-4c56-983a-0285f581379e', 1, NULL, NULL, NULL, N'DO-1003', CAST(N'2018-10-05 00:00:00.000' AS DateTime), CAST(1000 AS Decimal(18, 0)), CAST(N'2018-10-05 00:00:00.000' AS DateTime), CAST(N'2018-10-05 00:00:00.000' AS DateTime), N'114', 1, CAST(1230 AS Decimal(18, 0)), CAST(12 AS Decimal(18, 0)), CAST(12 AS Decimal(18, 0)), CAST(12 AS Decimal(18, 0)), CAST(12 AS Decimal(18, 0)), CAST(12 AS Decimal(18, 0)), N'Testing', CAST(N'2018-10-05 17:46:13.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-10-13 04:28:01.690' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e')
INSERT [dbo].[DO] ([ID], [StoreId], [SaleStationId], [SOId], [LeadId], [Status], [CompletedOn], [ApprovedDate], [ApprovedBy], [DONumber], [DODate], [Quantity], [LiftingStartDate], [LiftingEndDate], [DeliveryDestination], [TransporterId], [DumperRate], [FreightPaymentTerms], [FreightPerTon], [FreightTaxPerTon], [FreightComissionPSL], [FreightComissionAgent], [Remarks], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (1009, N'c0ffef5f-60af-485f-96f5-9bbb69223679', N'304af16b-60d1-4ed6-a4a4-3b3f6662d97b', 7, N'1ee1700d-1ca9-4c56-983a-0285f581379e', 1, NULL, NULL, NULL, N'DO-1004', CAST(N'2018-10-06 00:00:00.000' AS DateTime), CAST(1000 AS Decimal(18, 0)), CAST(N'2018-10-06 00:00:00.000' AS DateTime), CAST(N'2018-10-06 00:00:00.000' AS DateTime), N'116', 2, CAST(1230 AS Decimal(18, 0)), CAST(120 AS Decimal(18, 0)), CAST(120 AS Decimal(18, 0)), CAST(12 AS Decimal(18, 0)), CAST(12 AS Decimal(18, 0)), CAST(12 AS Decimal(18, 0)), N'Testing Update', CAST(N'2018-10-06 15:14:14.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-10-13 04:32:45.607' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e')
INSERT [dbo].[DO] ([ID], [StoreId], [SaleStationId], [SOId], [LeadId], [Status], [CompletedOn], [ApprovedDate], [ApprovedBy], [DONumber], [DODate], [Quantity], [LiftingStartDate], [LiftingEndDate], [DeliveryDestination], [TransporterId], [DumperRate], [FreightPaymentTerms], [FreightPerTon], [FreightTaxPerTon], [FreightComissionPSL], [FreightComissionAgent], [Remarks], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (1010, N'b37a4521-51bd-4e13-a317-2ff51f8c6280', N'304af16b-60d1-4ed6-a4a4-3b3f6662d97b', 7, N'1ee1700d-1ca9-4c56-983a-0285f581379e', 1, NULL, NULL, NULL, N'DO-1005', CAST(N'2018-10-06 00:00:00.000' AS DateTime), CAST(1000 AS Decimal(18, 0)), CAST(N'2018-10-06 00:00:00.000' AS DateTime), CAST(N'2018-10-06 00:00:00.000' AS DateTime), N'114', 1, CAST(1230 AS Decimal(18, 0)), CAST(1233 AS Decimal(18, 0)), CAST(12 AS Decimal(18, 0)), CAST(12 AS Decimal(18, 0)), CAST(12 AS Decimal(18, 0)), CAST(12 AS Decimal(18, 0)), N'123', CAST(N'2018-10-06 17:49:09.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-10-06 18:20:33.007' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e')
INSERT [dbo].[DO] ([ID], [StoreId], [SaleStationId], [SOId], [LeadId], [Status], [CompletedOn], [ApprovedDate], [ApprovedBy], [DONumber], [DODate], [Quantity], [LiftingStartDate], [LiftingEndDate], [DeliveryDestination], [TransporterId], [DumperRate], [FreightPaymentTerms], [FreightPerTon], [FreightTaxPerTon], [FreightComissionPSL], [FreightComissionAgent], [Remarks], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (1011, N'43961369-a7e4-4fcc-8756-4359cb20aae5', N'304af16b-60d1-4ed6-a4a4-3b3f6662d97b', 7, N'1ee1700d-1ca9-4c56-983a-0285f581379e', 1, NULL, NULL, NULL, N'DO-1006', CAST(N'2018-10-06 00:00:00.000' AS DateTime), CAST(1000 AS Decimal(18, 0)), CAST(N'2018-10-06 00:00:00.000' AS DateTime), CAST(N'2018-10-06 00:00:00.000' AS DateTime), N'114', 1, CAST(1233 AS Decimal(18, 0)), CAST(130 AS Decimal(18, 0)), CAST(130 AS Decimal(18, 0)), CAST(1300 AS Decimal(18, 0)), CAST(1300 AS Decimal(18, 0)), CAST(1300 AS Decimal(18, 0)), N'Test 130', CAST(N'2018-10-06 18:21:25.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-10-08 17:24:57.260' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e')
INSERT [dbo].[DO] ([ID], [StoreId], [SaleStationId], [SOId], [LeadId], [Status], [CompletedOn], [ApprovedDate], [ApprovedBy], [DONumber], [DODate], [Quantity], [LiftingStartDate], [LiftingEndDate], [DeliveryDestination], [TransporterId], [DumperRate], [FreightPaymentTerms], [FreightPerTon], [FreightTaxPerTon], [FreightComissionPSL], [FreightComissionAgent], [Remarks], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (1012, N'8b13ea2a-67e4-487f-8f02-11c9be1a1983', N'304af16b-60d1-4ed6-a4a4-3b3f6662d97b', 7, N'1ee1700d-1ca9-4c56-983a-0285f581379e', 3, NULL, CAST(N'2018-10-09 06:43:15.463' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'DO-1007', CAST(N'2018-10-09 00:00:00.000' AS DateTime), CAST(1000 AS Decimal(18, 0)), CAST(N'2018-10-09 00:00:00.000' AS DateTime), CAST(N'2018-10-09 00:00:00.000' AS DateTime), N'114', 1, CAST(1230 AS Decimal(18, 0)), CAST(1233 AS Decimal(18, 0)), CAST(130 AS Decimal(18, 0)), CAST(1300 AS Decimal(18, 0)), CAST(130 AS Decimal(18, 0)), CAST(120 AS Decimal(18, 0)), N'Test 130', CAST(N'2018-10-09 06:42:47.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-10-09 06:43:15.463' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e')
INSERT [dbo].[DO] ([ID], [StoreId], [SaleStationId], [SOId], [LeadId], [Status], [CompletedOn], [ApprovedDate], [ApprovedBy], [DONumber], [DODate], [Quantity], [LiftingStartDate], [LiftingEndDate], [DeliveryDestination], [TransporterId], [DumperRate], [FreightPaymentTerms], [FreightPerTon], [FreightTaxPerTon], [FreightComissionPSL], [FreightComissionAgent], [Remarks], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (1013, N'c0ffef5f-60af-485f-96f5-9bbb69223679', N'304af16b-60d1-4ed6-a4a4-3b3f6662d97b', 7, N'1ee1700d-1ca9-4c56-983a-0285f581379e', 1, NULL, NULL, NULL, N'DO-1008', CAST(N'2018-10-11 00:00:00.000' AS DateTime), CAST(1000 AS Decimal(18, 0)), CAST(N'2018-10-11 00:00:00.000' AS DateTime), CAST(N'2018-10-11 00:00:00.000' AS DateTime), N'114', 0, CAST(1230 AS Decimal(18, 0)), CAST(120 AS Decimal(18, 0)), CAST(120 AS Decimal(18, 0)), CAST(12 AS Decimal(18, 0)), CAST(1300 AS Decimal(18, 0)), CAST(1300 AS Decimal(18, 0)), N'Test 130', CAST(N'2018-10-11 19:26:44.633' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-10-11 19:26:44.633' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e')
INSERT [dbo].[DO] ([ID], [StoreId], [SaleStationId], [SOId], [LeadId], [Status], [CompletedOn], [ApprovedDate], [ApprovedBy], [DONumber], [DODate], [Quantity], [LiftingStartDate], [LiftingEndDate], [DeliveryDestination], [TransporterId], [DumperRate], [FreightPaymentTerms], [FreightPerTon], [FreightTaxPerTon], [FreightComissionPSL], [FreightComissionAgent], [Remarks], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (1014, N'b37a4521-51bd-4e13-a317-2ff51f8c6280', N'304af16b-60d1-4ed6-a4a4-3b3f6662d97b', 7, N'1ee1700d-1ca9-4c56-983a-0285f581379e', 1, NULL, NULL, NULL, N'DO-1009', CAST(N'2018-10-13 00:00:00.000' AS DateTime), CAST(1000 AS Decimal(18, 0)), CAST(N'2018-10-13 00:00:00.000' AS DateTime), CAST(N'2018-10-13 00:00:00.000' AS DateTime), N'114', 0, CAST(1230 AS Decimal(18, 0)), CAST(120 AS Decimal(18, 0)), CAST(12 AS Decimal(18, 0)), CAST(1300 AS Decimal(18, 0)), CAST(130 AS Decimal(18, 0)), CAST(120 AS Decimal(18, 0)), N'Test 130', CAST(N'2018-10-13 00:56:04.650' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-10-13 00:56:04.650' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e')
INSERT [dbo].[DO] ([ID], [StoreId], [SaleStationId], [SOId], [LeadId], [Status], [CompletedOn], [ApprovedDate], [ApprovedBy], [DONumber], [DODate], [Quantity], [LiftingStartDate], [LiftingEndDate], [DeliveryDestination], [TransporterId], [DumperRate], [FreightPaymentTerms], [FreightPerTon], [FreightTaxPerTon], [FreightComissionPSL], [FreightComissionAgent], [Remarks], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (1015, N'c0ffef5f-60af-485f-96f5-9bbb69223679', N'304af16b-60d1-4ed6-a4a4-3b3f6662d97b', 7, N'1ee1700d-1ca9-4c56-983a-0285f581379e', 1, NULL, NULL, NULL, N'DO-1010', CAST(N'2018-10-13 00:00:00.000' AS DateTime), CAST(1000 AS Decimal(18, 0)), CAST(N'2018-10-13 00:00:00.000' AS DateTime), CAST(N'2018-10-13 00:00:00.000' AS DateTime), N'114', 0, CAST(1230 AS Decimal(18, 0)), CAST(12 AS Decimal(18, 0)), CAST(12 AS Decimal(18, 0)), CAST(12 AS Decimal(18, 0)), CAST(120 AS Decimal(18, 0)), CAST(130 AS Decimal(18, 0)), N'Test 130777709909hjjhg', CAST(N'2018-10-13 02:19:10.230' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-10-13 02:19:10.230' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e')
INSERT [dbo].[DO] ([ID], [StoreId], [SaleStationId], [SOId], [LeadId], [Status], [CompletedOn], [ApprovedDate], [ApprovedBy], [DONumber], [DODate], [Quantity], [LiftingStartDate], [LiftingEndDate], [DeliveryDestination], [TransporterId], [DumperRate], [FreightPaymentTerms], [FreightPerTon], [FreightTaxPerTon], [FreightComissionPSL], [FreightComissionAgent], [Remarks], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (1016, N'c0ffef5f-60af-485f-96f5-9bbb69223679', N'9ef74e38-8e39-40de-a727-d56f745a6042', 7, N'1ee1700d-1ca9-4c56-983a-0285f581379e', 1, NULL, NULL, NULL, N'DO-1011', CAST(N'2018-10-13 00:00:00.000' AS DateTime), CAST(5009 AS Decimal(18, 0)), CAST(N'2018-10-13 00:00:00.000' AS DateTime), CAST(N'2018-10-13 00:00:00.000' AS DateTime), N'116', 0, CAST(1233 AS Decimal(18, 0)), CAST(120 AS Decimal(18, 0)), CAST(120 AS Decimal(18, 0)), CAST(1300 AS Decimal(18, 0)), CAST(130 AS Decimal(18, 0)), CAST(120 AS Decimal(18, 0)), N'Testing', CAST(N'2018-10-13 04:33:46.510' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-10-13 04:33:46.510' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e')
INSERT [dbo].[DO] ([ID], [StoreId], [SaleStationId], [SOId], [LeadId], [Status], [CompletedOn], [ApprovedDate], [ApprovedBy], [DONumber], [DODate], [Quantity], [LiftingStartDate], [LiftingEndDate], [DeliveryDestination], [TransporterId], [DumperRate], [FreightPaymentTerms], [FreightPerTon], [FreightTaxPerTon], [FreightComissionPSL], [FreightComissionAgent], [Remarks], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (1017, N'8b13ea2a-67e4-487f-8f02-11c9be1a1983', N'304af16b-60d1-4ed6-a4a4-3b3f6662d97b', 17, N'1ee1700d-1ca9-4c56-983a-0285f581379e', 3, NULL, CAST(N'2018-10-22 20:10:58.913' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'DO-1012', CAST(N'2018-10-22 00:00:00.000' AS DateTime), CAST(10000 AS Decimal(18, 0)), CAST(N'2018-10-22 00:00:00.000' AS DateTime), CAST(N'2018-10-22 00:00:00.000' AS DateTime), N'114', 0, CAST(1230 AS Decimal(18, 0)), CAST(120 AS Decimal(18, 0)), CAST(120 AS Decimal(18, 0)), CAST(1300 AS Decimal(18, 0)), CAST(130 AS Decimal(18, 0)), CAST(120 AS Decimal(18, 0)), N'Test', CAST(N'2018-10-22 20:08:19.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-10-22 20:10:58.913' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e')
INSERT [dbo].[DO] ([ID], [StoreId], [SaleStationId], [SOId], [LeadId], [Status], [CompletedOn], [ApprovedDate], [ApprovedBy], [DONumber], [DODate], [Quantity], [LiftingStartDate], [LiftingEndDate], [DeliveryDestination], [TransporterId], [DumperRate], [FreightPaymentTerms], [FreightPerTon], [FreightTaxPerTon], [FreightComissionPSL], [FreightComissionAgent], [Remarks], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (1018, N'43961369-a7e4-4fcc-8756-4359cb20aae5', N'9ef74e38-8e39-40de-a727-d56f745a6042', 9, N'1ee1700d-1ca9-4c56-983a-0285f581379e', 3, NULL, CAST(N'2018-10-25 15:49:48.733' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'DO-1013', CAST(N'2018-10-25 00:00:00.000' AS DateTime), CAST(100 AS Decimal(18, 0)), CAST(N'2018-10-25 00:00:00.000' AS DateTime), CAST(N'2018-10-25 00:00:00.000' AS DateTime), N'116', 0, CAST(1230 AS Decimal(18, 0)), CAST(120 AS Decimal(18, 0)), CAST(130 AS Decimal(18, 0)), CAST(12 AS Decimal(18, 0)), CAST(1300 AS Decimal(18, 0)), CAST(1300 AS Decimal(18, 0)), N'Testing', CAST(N'2018-10-25 00:25:04.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-10-25 15:49:48.733' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e')
INSERT [dbo].[DO] ([ID], [StoreId], [SaleStationId], [SOId], [LeadId], [Status], [CompletedOn], [ApprovedDate], [ApprovedBy], [DONumber], [DODate], [Quantity], [LiftingStartDate], [LiftingEndDate], [DeliveryDestination], [TransporterId], [DumperRate], [FreightPaymentTerms], [FreightPerTon], [FreightTaxPerTon], [FreightComissionPSL], [FreightComissionAgent], [Remarks], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (1019, NULL, N'304af16b-60d1-4ed6-a4a4-3b3f6662d97b', 9, N'1ee1700d-1ca9-4c56-983a-0285f581379e', 1, NULL, NULL, NULL, N'DO-1014', CAST(N'2018-10-26 00:00:00.000' AS DateTime), CAST(400 AS Decimal(18, 0)), CAST(N'2018-10-26 00:00:00.000' AS DateTime), CAST(N'2018-10-26 00:00:00.000' AS DateTime), N'114', 1, CAST(1230 AS Decimal(18, 0)), CAST(12 AS Decimal(18, 0)), CAST(12 AS Decimal(18, 0)), CAST(12 AS Decimal(18, 0)), CAST(12 AS Decimal(18, 0)), CAST(120 AS Decimal(18, 0)), N'Testing', CAST(N'2018-10-26 21:58:15.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-10-26 22:02:25.963' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e')
INSERT [dbo].[DO] ([ID], [StoreId], [SaleStationId], [SOId], [LeadId], [Status], [CompletedOn], [ApprovedDate], [ApprovedBy], [DONumber], [DODate], [Quantity], [LiftingStartDate], [LiftingEndDate], [DeliveryDestination], [TransporterId], [DumperRate], [FreightPaymentTerms], [FreightPerTon], [FreightTaxPerTon], [FreightComissionPSL], [FreightComissionAgent], [Remarks], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (1020, NULL, N'304af16b-60d1-4ed6-a4a4-3b3f6662d97b', 42, N'1ee1700d-1ca9-4c56-983a-0285f581379e', 3, NULL, CAST(N'2018-10-27 20:37:45.253' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'DO-1015', CAST(N'2018-10-27 00:00:00.000' AS DateTime), CAST(1100 AS Decimal(18, 0)), CAST(N'2018-10-27 00:00:00.000' AS DateTime), CAST(N'2018-10-27 00:00:00.000' AS DateTime), N'114', 1, CAST(1230 AS Decimal(18, 0)), CAST(12 AS Decimal(18, 0)), CAST(12 AS Decimal(18, 0)), CAST(12 AS Decimal(18, 0)), CAST(12 AS Decimal(18, 0)), CAST(12 AS Decimal(18, 0)), N'Testing Edit', CAST(N'2018-10-27 19:03:15.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-10-27 20:37:45.253' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e')
INSERT [dbo].[DO] ([ID], [StoreId], [SaleStationId], [SOId], [LeadId], [Status], [CompletedOn], [ApprovedDate], [ApprovedBy], [DONumber], [DODate], [Quantity], [LiftingStartDate], [LiftingEndDate], [DeliveryDestination], [TransporterId], [DumperRate], [FreightPaymentTerms], [FreightPerTon], [FreightTaxPerTon], [FreightComissionPSL], [FreightComissionAgent], [Remarks], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (1021, NULL, N'304af16b-60d1-4ed6-a4a4-3b3f6662d97b', 43, N'1ee1700d-1ca9-4c56-983a-0285f581379e', 3, NULL, CAST(N'2018-10-28 16:20:36.420' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'DO-1016', CAST(N'2018-10-28 00:00:00.000' AS DateTime), CAST(1000 AS Decimal(18, 0)), CAST(N'2018-10-28 00:00:00.000' AS DateTime), CAST(N'2018-10-28 00:00:00.000' AS DateTime), N'114', 0, CAST(1233 AS Decimal(18, 0)), CAST(120 AS Decimal(18, 0)), CAST(120 AS Decimal(18, 0)), CAST(12 AS Decimal(18, 0)), CAST(1300 AS Decimal(18, 0)), CAST(120 AS Decimal(18, 0)), N'Testing', CAST(N'2018-10-28 16:20:07.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-10-28 16:20:36.420' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e')
SET IDENTITY_INSERT [dbo].[DO] OFF
SET IDENTITY_INSERT [dbo].[ExceptionLogs] ON 

INSERT [dbo].[ExceptionLogs] ([Id], [ExceptionDate], [ExceptionMessage], [ExceptionType], [ExceptionSource], [ExceptionURL], [ExceptionSystem]) VALUES (1, CAST(N'2018-02-15 16:39:43.657' AS DateTime), N'Object reference not set to an instance of an object.', N'NullReferenceException', N'   at MZXRM.PSS.DataManager.DataMap.reMapSOData(SaleOrder SO) in E:\MZ XRM Solutions\Petrocoal Sales System\PSS\MZXRM.PSS.DataManager\Common\DataMap.cs:line 150
   at MZXRM.PSS.DataManager.SaleDataManager.SaveSO(SaleOrder SO) in E:\MZ XRM Solutions\Petrocoal Sales System\PSS\MZXRM.PSS.DataManager\SaleDataManager.cs:line 46
   at MZXRM.PSS.Business.SaleManager.CreateSO(Dictionary`2 values) in E:\MZ XRM Solutions\Petrocoal Sales System\PSS\MZXRM.PSS.Business\SaleUtil.cs:line 68', NULL, NULL)
INSERT [dbo].[ExceptionLogs] ([Id], [ExceptionDate], [ExceptionMessage], [ExceptionType], [ExceptionSource], [ExceptionURL], [ExceptionSystem]) VALUES (2, CAST(N'2018-02-15 16:40:28.120' AS DateTime), N'Object reference not set to an instance of an object.', N'NullReferenceException', N'   at MZXRM.PSS.DataManager.DataMap.reMapSOData(SaleOrder SO) in E:\MZ XRM Solutions\Petrocoal Sales System\PSS\MZXRM.PSS.DataManager\Common\DataMap.cs:line 150
   at MZXRM.PSS.DataManager.SaleDataManager.SaveSO(SaleOrder SO) in E:\MZ XRM Solutions\Petrocoal Sales System\PSS\MZXRM.PSS.DataManager\SaleDataManager.cs:line 46
   at MZXRM.PSS.Business.SaleManager.CreateSO(Dictionary`2 values) in E:\MZ XRM Solutions\Petrocoal Sales System\PSS\MZXRM.PSS.Business\SaleUtil.cs:line 68', NULL, NULL)
INSERT [dbo].[ExceptionLogs] ([Id], [ExceptionDate], [ExceptionMessage], [ExceptionType], [ExceptionSource], [ExceptionURL], [ExceptionSystem]) VALUES (3, CAST(N'2018-02-15 16:43:40.347' AS DateTime), N'SqlDateTime overflow. Must be between 1/1/1753 12:00:00 AM and 12/31/9999 11:59:59 PM.', N'SqlTypeException', N'   at System.Data.SqlClient.TdsParser.TdsExecuteRPC(SqlCommand cmd, _SqlRPC[] rpcArray, Int32 timeout, Boolean inSchema, SqlNotificationRequest notificationRequest, TdsParserStateObject stateObj, Boolean isCommandProc, Boolean sync, TaskCompletionSource`1 completion, Int32 startRpc, Int32 startParam)
   at System.Data.SqlClient.SqlCommand.RunExecuteReaderTds(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, Boolean async, Int32 timeout, Task& task, Boolean asyncWrite, Boolean inRetry, SqlDataReader ds, Boolean describeParameterEncryptionRequest)
   at System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method, TaskCompletionSource`1 completion, Int32 timeout, Task& task, Boolean& usedCache, Boolean asyncWrite, Boolean inRetry)
   at System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method)
   at System.Data.SqlClient.SqlCommand.ExecuteScalar()
   at MZXRM.PSS.DataManager.SaleDataManager.SaveSO(SaleOrder SO) in E:\MZ XRM Solutions\Petrocoal Sales System\PSS\MZXRM.PSS.DataManager\SaleDataManager.cs:line 54
   at MZXRM.PSS.Business.SaleManager.CreateSO(Dictionary`2 values) in E:\MZ XRM Solutions\Petrocoal Sales System\PSS\MZXRM.PSS.Business\SaleUtil.cs:line 68', NULL, NULL)
INSERT [dbo].[ExceptionLogs] ([Id], [ExceptionDate], [ExceptionMessage], [ExceptionType], [ExceptionSource], [ExceptionURL], [ExceptionSystem]) VALUES (4, CAST(N'2018-02-15 16:45:57.957' AS DateTime), N'The given key was not present in the dictionary.', N'KeyNotFoundException', N'   at System.ThrowHelper.ThrowKeyNotFoundException()
   at System.Collections.Generic.Dictionary`2.get_Item(TKey key)
   at MZXRM.PSS.Business.SaleManager.CreateSO(Dictionary`2 values) in E:\MZ XRM Solutions\Petrocoal Sales System\PSS\MZXRM.PSS.Business\SaleUtil.cs:line 52', NULL, NULL)
INSERT [dbo].[ExceptionLogs] ([Id], [ExceptionDate], [ExceptionMessage], [ExceptionType], [ExceptionSource], [ExceptionURL], [ExceptionSystem]) VALUES (5, CAST(N'2018-02-15 16:54:49.563' AS DateTime), N'Input string was not in a correct format.', N'FormatException', N'   at System.Number.StringToNumber(String str, NumberStyles options, NumberBuffer& number, NumberFormatInfo info, Boolean parseDecimal)
   at System.Number.ParseDecimal(String value, NumberStyles options, NumberFormatInfo numfmt)
   at System.Decimal.Parse(String s)
   at MZXRM.PSS.Business.SaleManager.CreateSO(Dictionary`2 values) in E:\MZ XRM Solutions\Petrocoal Sales System\PSS\MZXRM.PSS.Business\SaleUtil.cs:line 64', NULL, NULL)
INSERT [dbo].[ExceptionLogs] ([Id], [ExceptionDate], [ExceptionMessage], [ExceptionType], [ExceptionSource], [ExceptionURL], [ExceptionSystem]) VALUES (6, CAST(N'2018-02-15 16:59:19.560' AS DateTime), N'Input string was not in a correct format.', N'FormatException', N'   at System.Number.StringToNumber(String str, NumberStyles options, NumberBuffer& number, NumberFormatInfo info, Boolean parseDecimal)
   at System.Number.ParseDecimal(String value, NumberStyles options, NumberFormatInfo numfmt)
   at System.Decimal.Parse(String s)
   at MZXRM.PSS.Business.SaleManager.CreateSO(Dictionary`2 values) in E:\MZ XRM Solutions\Petrocoal Sales System\PSS\MZXRM.PSS.Business\SaleUtil.cs:line 64', NULL, NULL)
INSERT [dbo].[ExceptionLogs] ([Id], [ExceptionDate], [ExceptionMessage], [ExceptionType], [ExceptionSource], [ExceptionURL], [ExceptionSystem]) VALUES (7, CAST(N'2018-02-15 17:00:43.513' AS DateTime), N'Input string was not in a correct format.', N'FormatException', N'   at System.Number.StringToNumber(String str, NumberStyles options, NumberBuffer& number, NumberFormatInfo info, Boolean parseDecimal)
   at System.Number.ParseInt32(String s, NumberStyles style, NumberFormatInfo info)
   at MZXRM.PSS.Business.SaleManager.CreateSO(Dictionary`2 values) in E:\MZ XRM Solutions\Petrocoal Sales System\PSS\MZXRM.PSS.Business\SaleUtil.cs:line 46', NULL, NULL)
INSERT [dbo].[ExceptionLogs] ([Id], [ExceptionDate], [ExceptionMessage], [ExceptionType], [ExceptionSource], [ExceptionURL], [ExceptionSystem]) VALUES (8, CAST(N'2018-02-15 17:04:52.403' AS DateTime), N'SqlDateTime overflow. Must be between 1/1/1753 12:00:00 AM and 12/31/9999 11:59:59 PM.', N'SqlTypeException', N'   at System.Data.SqlClient.TdsParser.TdsExecuteRPC(SqlCommand cmd, _SqlRPC[] rpcArray, Int32 timeout, Boolean inSchema, SqlNotificationRequest notificationRequest, TdsParserStateObject stateObj, Boolean isCommandProc, Boolean sync, TaskCompletionSource`1 completion, Int32 startRpc, Int32 startParam)
   at System.Data.SqlClient.SqlCommand.RunExecuteReaderTds(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, Boolean async, Int32 timeout, Task& task, Boolean asyncWrite, Boolean inRetry, SqlDataReader ds, Boolean describeParameterEncryptionRequest)
   at System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method, TaskCompletionSource`1 completion, Int32 timeout, Task& task, Boolean& usedCache, Boolean asyncWrite, Boolean inRetry)
   at System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method)
   at System.Data.SqlClient.SqlCommand.ExecuteScalar()
   at MZXRM.PSS.DataManager.SaleDataManager.SaveSO(SaleOrder SO) in E:\MZ XRM Solutions\Petrocoal Sales System\PSS\MZXRM.PSS.DataManager\SaleDataManager.cs:line 54
   at MZXRM.PSS.Business.SaleManager.CreateSO(Dictionary`2 values) in E:\MZ XRM Solutions\Petrocoal Sales System\PSS\MZXRM.PSS.Business\SaleUtil.cs:line 68', NULL, NULL)
INSERT [dbo].[ExceptionLogs] ([Id], [ExceptionDate], [ExceptionMessage], [ExceptionType], [ExceptionSource], [ExceptionURL], [ExceptionSystem]) VALUES (9, CAST(N'2018-02-15 17:10:43.240' AS DateTime), N'Procedure or function ''sp_InsertSO'' expects parameter ''@ApprovedBy'', which was not supplied.', N'SqlException', N'   at System.Data.SqlClient.SqlConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
   at System.Data.SqlClient.TdsParser.ThrowExceptionAndWarning(TdsParserStateObject stateObj, Boolean callerHasConnectionLock, Boolean asyncClose)
   at System.Data.SqlClient.TdsParser.TryRun(RunBehavior runBehavior, SqlCommand cmdHandler, SqlDataReader dataStream, BulkCopySimpleResultSet bulkCopyHandler, TdsParserStateObject stateObj, Boolean& dataReady)
   at System.Data.SqlClient.SqlDataReader.TryConsumeMetaData()
   at System.Data.SqlClient.SqlDataReader.get_MetaData()
   at System.Data.SqlClient.SqlCommand.FinishExecuteReader(SqlDataReader ds, RunBehavior runBehavior, String resetOptionsString, Boolean isInternal, Boolean forDescribeParameterEncryption)
   at System.Data.SqlClient.SqlCommand.RunExecuteReaderTds(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, Boolean async, Int32 timeout, Task& task, Boolean asyncWrite, Boolean inRetry, SqlDataReader ds, Boolean describeParameterEncryptionRequest)
   at System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method, TaskCompletionSource`1 completion, Int32 timeout, Task& task, Boolean& usedCache, Boolean asyncWrite, Boolean inRetry)
   at System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method)
   at System.Data.SqlClient.SqlCommand.ExecuteScalar()
   at MZXRM.PSS.DataManager.SaleDataManager.SaveSO(SaleOrder SO) in E:\MZ XRM Solutions\Petrocoal Sales System\PSS\MZXRM.PSS.DataManager\SaleDataManager.cs:line 54
   at MZXRM.PSS.Business.SaleManager.CreateSO(Dictionary`2 values) in E:\MZ XRM Solutions\Petrocoal Sales System\PSS\MZXRM.PSS.Business\SaleUtil.cs:line 68', NULL, NULL)
INSERT [dbo].[ExceptionLogs] ([Id], [ExceptionDate], [ExceptionMessage], [ExceptionType], [ExceptionSource], [ExceptionURL], [ExceptionSystem]) VALUES (10, CAST(N'2018-02-15 17:13:31.480' AS DateTime), N'Procedure or function ''sp_InsertSO'' expects parameter ''@POScannedImage'', which was not supplied.', N'SqlException', N'   at System.Data.SqlClient.SqlConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
   at System.Data.SqlClient.TdsParser.ThrowExceptionAndWarning(TdsParserStateObject stateObj, Boolean callerHasConnectionLock, Boolean asyncClose)
   at System.Data.SqlClient.TdsParser.TryRun(RunBehavior runBehavior, SqlCommand cmdHandler, SqlDataReader dataStream, BulkCopySimpleResultSet bulkCopyHandler, TdsParserStateObject stateObj, Boolean& dataReady)
   at System.Data.SqlClient.SqlDataReader.TryConsumeMetaData()
   at System.Data.SqlClient.SqlDataReader.get_MetaData()
   at System.Data.SqlClient.SqlCommand.FinishExecuteReader(SqlDataReader ds, RunBehavior runBehavior, String resetOptionsString, Boolean isInternal, Boolean forDescribeParameterEncryption)
   at System.Data.SqlClient.SqlCommand.RunExecuteReaderTds(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, Boolean async, Int32 timeout, Task& task, Boolean asyncWrite, Boolean inRetry, SqlDataReader ds, Boolean describeParameterEncryptionRequest)
   at System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method, TaskCompletionSource`1 completion, Int32 timeout, Task& task, Boolean& usedCache, Boolean asyncWrite, Boolean inRetry)
   at System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method)
   at System.Data.SqlClient.SqlCommand.ExecuteScalar()
   at MZXRM.PSS.DataManager.SaleDataManager.SaveSO(SaleOrder SO) in E:\MZ XRM Solutions\Petrocoal Sales System\PSS\MZXRM.PSS.DataManager\SaleDataManager.cs:line 54
   at MZXRM.PSS.Business.SaleManager.CreateSO(Dictionary`2 values) in E:\MZ XRM Solutions\Petrocoal Sales System\PSS\MZXRM.PSS.Business\SaleUtil.cs:line 68', NULL, NULL)
INSERT [dbo].[ExceptionLogs] ([Id], [ExceptionDate], [ExceptionMessage], [ExceptionType], [ExceptionSource], [ExceptionURL], [ExceptionSystem]) VALUES (11, CAST(N'2018-03-14 19:26:59.143' AS DateTime), N'Input string was not in a correct format.', N'FormatException', N'   at System.Number.StringToNumber(String str, NumberStyles options, NumberBuffer& number, NumberFormatInfo info, Boolean parseDecimal)
   at System.Number.ParseDecimal(String value, NumberStyles options, NumberFormatInfo numfmt)
   at System.Decimal.Parse(String s)
   at MZXRM.PSS.Business.SaleManager.CreateDC(Dictionary`2 values) in E:\MZ XRM Solutions\Petrocoal Sales System\PSS\MZXRM.PSS.Business\SaleManager.cs:line 165', NULL, N'Creating SO')
INSERT [dbo].[ExceptionLogs] ([Id], [ExceptionDate], [ExceptionMessage], [ExceptionType], [ExceptionSource], [ExceptionURL], [ExceptionSystem]) VALUES (12, CAST(N'2018-03-14 19:32:40.160' AS DateTime), N'Input string was not in a correct format.', N'FormatException', N'   at System.Number.StringToNumber(String str, NumberStyles options, NumberBuffer& number, NumberFormatInfo info, Boolean parseDecimal)
   at System.Number.ParseDecimal(String value, NumberStyles options, NumberFormatInfo numfmt)
   at System.Decimal.Parse(String s)
   at MZXRM.PSS.Business.SaleManager.CreateDC(Dictionary`2 values) in E:\MZ XRM Solutions\Petrocoal Sales System\PSS\MZXRM.PSS.Business\SaleManager.cs:line 165', NULL, N'Creating SO')
INSERT [dbo].[ExceptionLogs] ([Id], [ExceptionDate], [ExceptionMessage], [ExceptionType], [ExceptionSource], [ExceptionURL], [ExceptionSystem]) VALUES (13, CAST(N'2018-03-14 19:34:08.933' AS DateTime), N'Input string was not in a correct format.', N'FormatException', N'   at System.Number.StringToNumber(String str, NumberStyles options, NumberBuffer& number, NumberFormatInfo info, Boolean parseDecimal)
   at System.Number.ParseDecimal(String value, NumberStyles options, NumberFormatInfo numfmt)
   at System.Decimal.Parse(String s)
   at MZXRM.PSS.Business.SaleManager.CreateDC(Dictionary`2 values) in E:\MZ XRM Solutions\Petrocoal Sales System\PSS\MZXRM.PSS.Business\SaleManager.cs:line 165', NULL, N'Creating SO')
INSERT [dbo].[ExceptionLogs] ([Id], [ExceptionDate], [ExceptionMessage], [ExceptionType], [ExceptionSource], [ExceptionURL], [ExceptionSystem]) VALUES (14, CAST(N'2018-03-14 19:34:35.857' AS DateTime), N'Input string was not in a correct format.', N'FormatException', N'   at System.Number.StringToNumber(String str, NumberStyles options, NumberBuffer& number, NumberFormatInfo info, Boolean parseDecimal)
   at System.Number.ParseDecimal(String value, NumberStyles options, NumberFormatInfo numfmt)
   at System.Decimal.Parse(String s)
   at MZXRM.PSS.Business.SaleManager.CreateDC(Dictionary`2 values) in E:\MZ XRM Solutions\Petrocoal Sales System\PSS\MZXRM.PSS.Business\SaleManager.cs:line 165', NULL, N'Creating SO')
INSERT [dbo].[ExceptionLogs] ([Id], [ExceptionDate], [ExceptionMessage], [ExceptionType], [ExceptionSource], [ExceptionURL], [ExceptionSystem]) VALUES (15, CAST(N'2018-03-14 19:34:49.827' AS DateTime), N'Cannot insert the value NULL into column ''DCNumber'', table ''Petrocoal.dbo.DC''; column does not allow nulls. INSERT fails.', N'SqlException', N'   at System.Data.SqlClient.SqlConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
   at System.Data.SqlClient.TdsParser.ThrowExceptionAndWarning(TdsParserStateObject stateObj, Boolean callerHasConnectionLock, Boolean asyncClose)
   at System.Data.SqlClient.TdsParser.TryRun(RunBehavior runBehavior, SqlCommand cmdHandler, SqlDataReader dataStream, BulkCopySimpleResultSet bulkCopyHandler, TdsParserStateObject stateObj, Boolean& dataReady)
   at System.Data.SqlClient.SqlDataReader.TryConsumeMetaData()
   at System.Data.SqlClient.SqlDataReader.get_MetaData()
   at System.Data.SqlClient.SqlCommand.FinishExecuteReader(SqlDataReader ds, RunBehavior runBehavior, String resetOptionsString, Boolean isInternal, Boolean forDescribeParameterEncryption)
   at System.Data.SqlClient.SqlCommand.RunExecuteReaderTds(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, Boolean async, Int32 timeout, Task& task, Boolean asyncWrite, Boolean inRetry, SqlDataReader ds, Boolean describeParameterEncryptionRequest)
   at System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method, TaskCompletionSource`1 completion, Int32 timeout, Task& task, Boolean& usedCache, Boolean asyncWrite, Boolean inRetry)
   at System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method)
   at System.Data.SqlClient.SqlCommand.ExecuteScalar()
   at MZXRM.PSS.DataManager.SaleDataManager.SaveDC(DeliveryChalan DC) in E:\MZ XRM Solutions\Petrocoal Sales System\PSS\MZXRM.PSS.DataManager\SaleDataManager.cs:line 289
   at MZXRM.PSS.Business.SaleManager.CreateDC(Dictionary`2 values) in E:\MZ XRM Solutions\Petrocoal Sales System\PSS\MZXRM.PSS.Business\SaleManager.cs:line 178', NULL, N'Creating SO')
INSERT [dbo].[ExceptionLogs] ([Id], [ExceptionDate], [ExceptionMessage], [ExceptionType], [ExceptionSource], [ExceptionURL], [ExceptionSystem]) VALUES (16, CAST(N'2018-03-14 19:40:27.063' AS DateTime), N'Cannot insert the value NULL into column ''DCNumber'', table ''Petrocoal.dbo.DC''; column does not allow nulls. INSERT fails.', N'SqlException', N'   at System.Data.SqlClient.SqlConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
   at System.Data.SqlClient.TdsParser.ThrowExceptionAndWarning(TdsParserStateObject stateObj, Boolean callerHasConnectionLock, Boolean asyncClose)
   at System.Data.SqlClient.TdsParser.TryRun(RunBehavior runBehavior, SqlCommand cmdHandler, SqlDataReader dataStream, BulkCopySimpleResultSet bulkCopyHandler, TdsParserStateObject stateObj, Boolean& dataReady)
   at System.Data.SqlClient.SqlDataReader.TryConsumeMetaData()
   at System.Data.SqlClient.SqlDataReader.get_MetaData()
   at System.Data.SqlClient.SqlCommand.FinishExecuteReader(SqlDataReader ds, RunBehavior runBehavior, String resetOptionsString, Boolean isInternal, Boolean forDescribeParameterEncryption)
   at System.Data.SqlClient.SqlCommand.RunExecuteReaderTds(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, Boolean async, Int32 timeout, Task& task, Boolean asyncWrite, Boolean inRetry, SqlDataReader ds, Boolean describeParameterEncryptionRequest)
   at System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method, TaskCompletionSource`1 completion, Int32 timeout, Task& task, Boolean& usedCache, Boolean asyncWrite, Boolean inRetry)
   at System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method)
   at System.Data.SqlClient.SqlCommand.ExecuteScalar()
   at MZXRM.PSS.DataManager.SaleDataManager.SaveDC(DeliveryChalan DC) in E:\MZ XRM Solutions\Petrocoal Sales System\PSS\MZXRM.PSS.DataManager\SaleDataManager.cs:line 289
   at MZXRM.PSS.Business.SaleManager.CreateDC(Dictionary`2 values) in E:\MZ XRM Solutions\Petrocoal Sales System\PSS\MZXRM.PSS.Business\SaleManager.cs:line 178', NULL, N'Creating SO')
INSERT [dbo].[ExceptionLogs] ([Id], [ExceptionDate], [ExceptionMessage], [ExceptionType], [ExceptionSource], [ExceptionURL], [ExceptionSystem]) VALUES (17, CAST(N'2018-03-29 17:16:11.547' AS DateTime), N'The given key was not present in the dictionary.', N'KeyNotFoundException', N'   at System.ThrowHelper.ThrowKeyNotFoundException()
   at System.Collections.Generic.Dictionary`2.get_Item(TKey key)
   at MZXRM.PSS.Business.SaleManager.UpdateDO(Dictionary`2 values) in E:\MZ XRM Solutions\Petrocoal Sales System\PSS\MZXRM.PSS.Business\SaleManager.cs:line 102', NULL, N'Creating SO')
INSERT [dbo].[ExceptionLogs] ([Id], [ExceptionDate], [ExceptionMessage], [ExceptionType], [ExceptionSource], [ExceptionURL], [ExceptionSystem]) VALUES (18, CAST(N'2018-03-29 17:16:24.030' AS DateTime), N'The given key was not present in the dictionary.', N'KeyNotFoundException', N'   at System.ThrowHelper.ThrowKeyNotFoundException()
   at System.Collections.Generic.Dictionary`2.get_Item(TKey key)
   at MZXRM.PSS.Business.SaleManager.UpdateDO(Dictionary`2 values) in E:\MZ XRM Solutions\Petrocoal Sales System\PSS\MZXRM.PSS.Business\SaleManager.cs:line 102', NULL, N'Creating SO')
INSERT [dbo].[ExceptionLogs] ([Id], [ExceptionDate], [ExceptionMessage], [ExceptionType], [ExceptionSource], [ExceptionURL], [ExceptionSystem]) VALUES (19, CAST(N'2018-03-29 17:20:14.883' AS DateTime), N'The given key was not present in the dictionary.', N'KeyNotFoundException', N'   at System.ThrowHelper.ThrowKeyNotFoundException()
   at System.Collections.Generic.Dictionary`2.get_Item(TKey key)
   at MZXRM.PSS.Business.SaleManager.UpdateDO(Dictionary`2 values) in E:\MZ XRM Solutions\Petrocoal Sales System\PSS\MZXRM.PSS.Business\SaleManager.cs:line 102', NULL, N'Creating SO')
INSERT [dbo].[ExceptionLogs] ([Id], [ExceptionDate], [ExceptionMessage], [ExceptionType], [ExceptionSource], [ExceptionURL], [ExceptionSystem]) VALUES (20, CAST(N'2018-03-29 17:21:39.440' AS DateTime), N'Procedure or function sp_InsertDO has too many arguments specified.', N'SqlException', N'   at System.Data.SqlClient.SqlConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
   at System.Data.SqlClient.TdsParser.ThrowExceptionAndWarning(TdsParserStateObject stateObj, Boolean callerHasConnectionLock, Boolean asyncClose)
   at System.Data.SqlClient.TdsParser.TryRun(RunBehavior runBehavior, SqlCommand cmdHandler, SqlDataReader dataStream, BulkCopySimpleResultSet bulkCopyHandler, TdsParserStateObject stateObj, Boolean& dataReady)
   at System.Data.SqlClient.SqlDataReader.TryConsumeMetaData()
   at System.Data.SqlClient.SqlDataReader.get_MetaData()
   at System.Data.SqlClient.SqlCommand.FinishExecuteReader(SqlDataReader ds, RunBehavior runBehavior, String resetOptionsString, Boolean isInternal, Boolean forDescribeParameterEncryption)
   at System.Data.SqlClient.SqlCommand.RunExecuteReaderTds(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, Boolean async, Int32 timeout, Task& task, Boolean asyncWrite, Boolean inRetry, SqlDataReader ds, Boolean describeParameterEncryptionRequest)
   at System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method, TaskCompletionSource`1 completion, Int32 timeout, Task& task, Boolean& usedCache, Boolean asyncWrite, Boolean inRetry)
   at System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method)
   at System.Data.SqlClient.SqlCommand.ExecuteScalar()
   at MZXRM.PSS.DataManager.SaleDataManager.SaveDO(DeliveryOrder DO) in E:\MZ XRM Solutions\Petrocoal Sales System\PSS\MZXRM.PSS.DataManager\SaleDataManager.cs:line 264
   at MZXRM.PSS.Business.SaleManager.CreateDO(Dictionary`2 values) in E:\MZ XRM Solutions\Petrocoal Sales System\PSS\MZXRM.PSS.Business\SaleManager.cs:line 88', NULL, N'Creating SO')
INSERT [dbo].[ExceptionLogs] ([Id], [ExceptionDate], [ExceptionMessage], [ExceptionType], [ExceptionSource], [ExceptionURL], [ExceptionSystem]) VALUES (21, CAST(N'2018-03-29 17:31:54.880' AS DateTime), N'Guid should contain 32 digits with 4 dashes (xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx).', N'FormatException', N'   at System.Guid.GuidResult.SetFailure(ParseFailureKind failure, String failureMessageID, Object failureMessageFormatArgument, String failureArgumentName, Exception innerException)
   at System.Guid.TryParseGuidWithNoStyle(String guidString, GuidResult& result)
   at System.Guid.TryParseGuid(String g, GuidStyles flags, GuidResult& result)
   at System.Guid..ctor(String g)
   at MZXRM.PSS.DataManager.StoreDataManager.GetStoreRef(String id) in E:\MZ XRM Solutions\Petrocoal Sales System\PSS\MZXRM.PSS.DataManager\StoreDataManager.cs:line 246
   at MZXRM.PSS.Business.SaleManager.CreateDO(Dictionary`2 values) in E:\MZ XRM Solutions\Petrocoal Sales System\PSS\MZXRM.PSS.Business\SaleManager.cs:line 59', NULL, N'Creating SO')
INSERT [dbo].[ExceptionLogs] ([Id], [ExceptionDate], [ExceptionMessage], [ExceptionType], [ExceptionSource], [ExceptionURL], [ExceptionSystem]) VALUES (22, CAST(N'2018-03-29 17:32:30.963' AS DateTime), N'Procedure or function sp_InsertDO has too many arguments specified.', N'SqlException', N'   at System.Data.SqlClient.SqlConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
   at System.Data.SqlClient.TdsParser.ThrowExceptionAndWarning(TdsParserStateObject stateObj, Boolean callerHasConnectionLock, Boolean asyncClose)
   at System.Data.SqlClient.TdsParser.TryRun(RunBehavior runBehavior, SqlCommand cmdHandler, SqlDataReader dataStream, BulkCopySimpleResultSet bulkCopyHandler, TdsParserStateObject stateObj, Boolean& dataReady)
   at System.Data.SqlClient.SqlDataReader.TryConsumeMetaData()
   at System.Data.SqlClient.SqlDataReader.get_MetaData()
   at System.Data.SqlClient.SqlCommand.FinishExecuteReader(SqlDataReader ds, RunBehavior runBehavior, String resetOptionsString, Boolean isInternal, Boolean forDescribeParameterEncryption)
   at System.Data.SqlClient.SqlCommand.RunExecuteReaderTds(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, Boolean async, Int32 timeout, Task& task, Boolean asyncWrite, Boolean inRetry, SqlDataReader ds, Boolean describeParameterEncryptionRequest)
   at System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method, TaskCompletionSource`1 completion, Int32 timeout, Task& task, Boolean& usedCache, Boolean asyncWrite, Boolean inRetry)
   at System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method)
   at System.Data.SqlClient.SqlCommand.ExecuteScalar()
   at MZXRM.PSS.DataManager.SaleDataManager.SaveDO(DeliveryOrder DO) in E:\MZ XRM Solutions\Petrocoal Sales System\PSS\MZXRM.PSS.DataManager\SaleDataManager.cs:line 264
   at MZXRM.PSS.Business.SaleManager.CreateDO(Dictionary`2 values) in E:\MZ XRM Solutions\Petrocoal Sales System\PSS\MZXRM.PSS.Business\SaleManager.cs:line 88', NULL, N'Creating SO')
INSERT [dbo].[ExceptionLogs] ([Id], [ExceptionDate], [ExceptionMessage], [ExceptionType], [ExceptionSource], [ExceptionURL], [ExceptionSystem]) VALUES (23, CAST(N'2018-04-04 12:55:28.583' AS DateTime), N'Input string was not in a correct format.', N'FormatException', N'   at System.Number.StringToNumber(String str, NumberStyles options, NumberBuffer& number, NumberFormatInfo info, Boolean parseDecimal)
   at System.Number.ParseDecimal(String value, NumberStyles options, NumberFormatInfo numfmt)
   at System.Decimal.Parse(String s)
   at MZXRM.PSS.Business.SaleManager.CreateSO(Dictionary`2 values) in E:\MZ XRM Solutions\Petrocoal Sales System\PSS\MZXRM.PSS.Business\SaleManager.cs:line 319', NULL, N'Creating SO')
INSERT [dbo].[ExceptionLogs] ([Id], [ExceptionDate], [ExceptionMessage], [ExceptionType], [ExceptionSource], [ExceptionURL], [ExceptionSystem]) VALUES (24, CAST(N'2018-04-04 12:56:52.923' AS DateTime), N'Cannot insert the value NULL into column ''DONumber'', table ''Petrocoal.dbo.DO''; column does not allow nulls. INSERT fails.', N'SqlException', N'   at System.Data.SqlClient.SqlConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
   at System.Data.SqlClient.TdsParser.ThrowExceptionAndWarning(TdsParserStateObject stateObj, Boolean callerHasConnectionLock, Boolean asyncClose)
   at System.Data.SqlClient.TdsParser.TryRun(RunBehavior runBehavior, SqlCommand cmdHandler, SqlDataReader dataStream, BulkCopySimpleResultSet bulkCopyHandler, TdsParserStateObject stateObj, Boolean& dataReady)
   at System.Data.SqlClient.SqlDataReader.TryConsumeMetaData()
   at System.Data.SqlClient.SqlDataReader.get_MetaData()
   at System.Data.SqlClient.SqlCommand.FinishExecuteReader(SqlDataReader ds, RunBehavior runBehavior, String resetOptionsString, Boolean isInternal, Boolean forDescribeParameterEncryption)
   at System.Data.SqlClient.SqlCommand.RunExecuteReaderTds(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, Boolean async, Int32 timeout, Task& task, Boolean asyncWrite, Boolean inRetry, SqlDataReader ds, Boolean describeParameterEncryptionRequest)
   at System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method, TaskCompletionSource`1 completion, Int32 timeout, Task& task, Boolean& usedCache, Boolean asyncWrite, Boolean inRetry)
   at System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method)
   at System.Data.SqlClient.SqlCommand.ExecuteScalar()
   at MZXRM.PSS.DataManager.SaleDataManager.SaveDO(DeliveryOrder DO) in E:\MZ XRM Solutions\Petrocoal Sales System\PSS\MZXRM.PSS.DataManager\SaleDataManager.cs:line 299
   at MZXRM.PSS.Business.SaleManager.CreateDO(Dictionary`2 values) in E:\MZ XRM Solutions\Petrocoal Sales System\PSS\MZXRM.PSS.Business\SaleManager.cs:line 88', NULL, N'Creating SO')
INSERT [dbo].[ExceptionLogs] ([Id], [ExceptionDate], [ExceptionMessage], [ExceptionType], [ExceptionSource], [ExceptionURL], [ExceptionSystem]) VALUES (25, CAST(N'2018-04-04 13:03:45.750' AS DateTime), N'Object reference not set to an instance of an object.', N'NullReferenceException', N'   at MZXRM.PSS.Business.SaleManager.CreateDC(Dictionary`2 values) in E:\MZ XRM Solutions\Petrocoal Sales System\PSS\MZXRM.PSS.Business\SaleManager.cs:line 155', NULL, N'Creating SO')
INSERT [dbo].[ExceptionLogs] ([Id], [ExceptionDate], [ExceptionMessage], [ExceptionType], [ExceptionSource], [ExceptionURL], [ExceptionSystem]) VALUES (26, CAST(N'2018-04-04 15:03:02.390' AS DateTime), N'Object reference not set to an instance of an object.', N'NullReferenceException', N'   at MZXRM.PSS.Business.SaleManager.CreateDC(Dictionary`2 values) in E:\MZ XRM Solutions\Petrocoal Sales System\PSS\MZXRM.PSS.Business\SaleManager.cs:line 155', NULL, N'Creating SO')
INSERT [dbo].[ExceptionLogs] ([Id], [ExceptionDate], [ExceptionMessage], [ExceptionType], [ExceptionSource], [ExceptionURL], [ExceptionSystem]) VALUES (27, CAST(N'2018-04-04 15:03:09.770' AS DateTime), N'Object reference not set to an instance of an object.', N'NullReferenceException', N'   at MZXRM.PSS.Business.SaleManager.CreateDC(Dictionary`2 values) in E:\MZ XRM Solutions\Petrocoal Sales System\PSS\MZXRM.PSS.Business\SaleManager.cs:line 155', NULL, N'Creating SO')
INSERT [dbo].[ExceptionLogs] ([Id], [ExceptionDate], [ExceptionMessage], [ExceptionType], [ExceptionSource], [ExceptionURL], [ExceptionSystem]) VALUES (28, CAST(N'2018-04-11 18:39:19.113' AS DateTime), N'The given key was not present in the dictionary.', N'KeyNotFoundException', N'   at System.ThrowHelper.ThrowKeyNotFoundException()
   at System.Collections.Generic.Dictionary`2.get_Item(TKey key)
   at MZXRM.PSS.Business.SaleManager.UpdateDO(Dictionary`2 values) in E:\MZ XRM Solutions\Petrocoal Sales System\PSS\MZXRM.PSS.Business\SaleManager.cs:line 120', NULL, N'Creating SO')
INSERT [dbo].[ExceptionLogs] ([Id], [ExceptionDate], [ExceptionMessage], [ExceptionType], [ExceptionSource], [ExceptionURL], [ExceptionSystem]) VALUES (29, CAST(N'2018-04-18 19:46:33.660' AS DateTime), N'Input string was not in a correct format.', N'FormatException', N'   at System.Number.StringToNumber(String str, NumberStyles options, NumberBuffer& number, NumberFormatInfo info, Boolean parseDecimal)
   at System.Number.ParseInt32(String s, NumberStyles style, NumberFormatInfo info)
   at MZXRM.PSS.Business.SaleManager.CreateSO(Dictionary`2 values) in E:\MZ XRM Solutions\Petrocoal Sales System\PSS\MZXRM.PSS.Business\SaleManager.cs:line 302', NULL, N'Creating SO')
INSERT [dbo].[ExceptionLogs] ([Id], [ExceptionDate], [ExceptionMessage], [ExceptionType], [ExceptionSource], [ExceptionURL], [ExceptionSystem]) VALUES (30, CAST(N'2018-04-18 19:46:46.437' AS DateTime), N'Input string was not in a correct format.', N'FormatException', N'   at System.Number.StringToNumber(String str, NumberStyles options, NumberBuffer& number, NumberFormatInfo info, Boolean parseDecimal)
   at System.Number.ParseInt32(String s, NumberStyles style, NumberFormatInfo info)
   at MZXRM.PSS.Business.SaleManager.CreateSO(Dictionary`2 values) in E:\MZ XRM Solutions\Petrocoal Sales System\PSS\MZXRM.PSS.Business\SaleManager.cs:line 302', NULL, N'Creating SO')
INSERT [dbo].[ExceptionLogs] ([Id], [ExceptionDate], [ExceptionMessage], [ExceptionType], [ExceptionSource], [ExceptionURL], [ExceptionSystem]) VALUES (10029, CAST(N'2018-08-09 12:30:20.180' AS DateTime), N'The given key was not present in the dictionary.', N'KeyNotFoundException', N'   at System.ThrowHelper.ThrowKeyNotFoundException()
   at System.Collections.Generic.Dictionary`2.get_Item(TKey key)
   at MZXRM.PSS.Business.SaleManager.CreateSO(Dictionary`2 values) in E:\MZ XRM Solutions\Petrocoal Sales System\PSS\MZXRM.PSS.Business\SaleManager.cs:line 407', NULL, N'Creating SO')
INSERT [dbo].[ExceptionLogs] ([Id], [ExceptionDate], [ExceptionMessage], [ExceptionType], [ExceptionSource], [ExceptionURL], [ExceptionSystem]) VALUES (10030, CAST(N'2018-10-05 16:12:29.160' AS DateTime), N'Procedure or function ''sp_InsertDO'' expects parameter ''@StoreId'', which was not supplied.', N'SqlException', N'   at System.Data.SqlClient.SqlConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
   at System.Data.SqlClient.SqlInternalConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
   at System.Data.SqlClient.TdsParser.ThrowExceptionAndWarning(TdsParserStateObject stateObj, Boolean callerHasConnectionLock, Boolean asyncClose)
   at System.Data.SqlClient.TdsParser.TryRun(RunBehavior runBehavior, SqlCommand cmdHandler, SqlDataReader dataStream, BulkCopySimpleResultSet bulkCopyHandler, TdsParserStateObject stateObj, Boolean& dataReady)
   at System.Data.SqlClient.SqlDataReader.TryConsumeMetaData()
   at System.Data.SqlClient.SqlDataReader.get_MetaData()
   at System.Data.SqlClient.SqlCommand.FinishExecuteReader(SqlDataReader ds, RunBehavior runBehavior, String resetOptionsString, Boolean isInternal, Boolean forDescribeParameterEncryption, Boolean shouldCacheForAlwaysEncrypted)
   at System.Data.SqlClient.SqlCommand.RunExecuteReaderTds(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, Boolean async, Int32 timeout, Task& task, Boolean asyncWrite, Boolean inRetry, SqlDataReader ds, Boolean describeParameterEncryptionRequest)
   at System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method, TaskCompletionSource`1 completion, Int32 timeout, Task& task, Boolean& usedCache, Boolean asyncWrite, Boolean inRetry)
   at System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method)
   at System.Data.SqlClient.SqlCommand.ExecuteScalar()
   at MZXRM.PSS.DataManager.SaleDataManager.SaveDO(Dictionary`2 keyValues) in C:\Users\Dell\Source\Repos\PetrocoalSalesSystem\PSS\MZXRM.PSS.DataManager\SaleDataManager.cs:line 185
   at MZXRM.PSS.Business.SaleManager.CreateDO(Dictionary`2 values) in C:\Users\Dell\Source\Repos\PetrocoalSalesSystem\PSS\MZXRM.PSS.Business\SaleManager.cs:line 196', NULL, N'Creating SO')
INSERT [dbo].[ExceptionLogs] ([Id], [ExceptionDate], [ExceptionMessage], [ExceptionType], [ExceptionSource], [ExceptionURL], [ExceptionSystem]) VALUES (10031, CAST(N'2018-10-05 16:17:20.430' AS DateTime), N'Procedure or function ''sp_InsertDO'' expects parameter ''@StoreId'', which was not supplied.', N'SqlException', N'   at System.Data.SqlClient.SqlConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
   at System.Data.SqlClient.SqlInternalConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
   at System.Data.SqlClient.TdsParser.ThrowExceptionAndWarning(TdsParserStateObject stateObj, Boolean callerHasConnectionLock, Boolean asyncClose)
   at System.Data.SqlClient.TdsParser.TryRun(RunBehavior runBehavior, SqlCommand cmdHandler, SqlDataReader dataStream, BulkCopySimpleResultSet bulkCopyHandler, TdsParserStateObject stateObj, Boolean& dataReady)
   at System.Data.SqlClient.SqlDataReader.TryConsumeMetaData()
   at System.Data.SqlClient.SqlDataReader.get_MetaData()
   at System.Data.SqlClient.SqlCommand.FinishExecuteReader(SqlDataReader ds, RunBehavior runBehavior, String resetOptionsString, Boolean isInternal, Boolean forDescribeParameterEncryption, Boolean shouldCacheForAlwaysEncrypted)
   at System.Data.SqlClient.SqlCommand.RunExecuteReaderTds(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, Boolean async, Int32 timeout, Task& task, Boolean asyncWrite, Boolean inRetry, SqlDataReader ds, Boolean describeParameterEncryptionRequest)
   at System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method, TaskCompletionSource`1 completion, Int32 timeout, Task& task, Boolean& usedCache, Boolean asyncWrite, Boolean inRetry)
   at System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method)
   at System.Data.SqlClient.SqlCommand.ExecuteScalar()
   at MZXRM.PSS.DataManager.SaleDataManager.SaveDO(Dictionary`2 keyValues) in C:\Users\Dell\Source\Repos\PetrocoalSalesSystem\PSS\MZXRM.PSS.DataManager\SaleDataManager.cs:line 185
   at MZXRM.PSS.Business.SaleManager.CreateDO(Dictionary`2 values) in C:\Users\Dell\Source\Repos\PetrocoalSalesSystem\PSS\MZXRM.PSS.Business\SaleManager.cs:line 196', NULL, N'Creating SO')
INSERT [dbo].[ExceptionLogs] ([Id], [ExceptionDate], [ExceptionMessage], [ExceptionType], [ExceptionSource], [ExceptionURL], [ExceptionSystem]) VALUES (10032, CAST(N'2018-10-05 16:29:06.293' AS DateTime), N'Procedure or function ''sp_InsertDO'' expects parameter ''@StoreId'', which was not supplied.', N'SqlException', N'   at System.Data.SqlClient.SqlConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
   at System.Data.SqlClient.SqlInternalConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
   at System.Data.SqlClient.TdsParser.ThrowExceptionAndWarning(TdsParserStateObject stateObj, Boolean callerHasConnectionLock, Boolean asyncClose)
   at System.Data.SqlClient.TdsParser.TryRun(RunBehavior runBehavior, SqlCommand cmdHandler, SqlDataReader dataStream, BulkCopySimpleResultSet bulkCopyHandler, TdsParserStateObject stateObj, Boolean& dataReady)
   at System.Data.SqlClient.SqlDataReader.TryConsumeMetaData()
   at System.Data.SqlClient.SqlDataReader.get_MetaData()
   at System.Data.SqlClient.SqlCommand.FinishExecuteReader(SqlDataReader ds, RunBehavior runBehavior, String resetOptionsString, Boolean isInternal, Boolean forDescribeParameterEncryption, Boolean shouldCacheForAlwaysEncrypted)
   at System.Data.SqlClient.SqlCommand.RunExecuteReaderTds(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, Boolean async, Int32 timeout, Task& task, Boolean asyncWrite, Boolean inRetry, SqlDataReader ds, Boolean describeParameterEncryptionRequest)
   at System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method, TaskCompletionSource`1 completion, Int32 timeout, Task& task, Boolean& usedCache, Boolean asyncWrite, Boolean inRetry)
   at System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method)
   at System.Data.SqlClient.SqlCommand.ExecuteScalar()
   at MZXRM.PSS.DataManager.SaleDataManager.SaveDO(Dictionary`2 keyValues) in C:\Users\Dell\Source\Repos\PetrocoalSalesSystem\PSS\MZXRM.PSS.DataManager\SaleDataManager.cs:line 185
   at MZXRM.PSS.Business.SaleManager.CreateDO(Dictionary`2 values) in C:\Users\Dell\Source\Repos\PetrocoalSalesSystem\PSS\MZXRM.PSS.Business\SaleManager.cs:line 196', NULL, N'Creating SO')
INSERT [dbo].[ExceptionLogs] ([Id], [ExceptionDate], [ExceptionMessage], [ExceptionType], [ExceptionSource], [ExceptionURL], [ExceptionSystem]) VALUES (10033, CAST(N'2018-10-05 16:30:30.167' AS DateTime), N'Procedure or function ''sp_InsertDO'' expects parameter ''@StoreId'', which was not supplied.', N'SqlException', N'   at System.Data.SqlClient.SqlConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
   at System.Data.SqlClient.SqlInternalConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
   at System.Data.SqlClient.TdsParser.ThrowExceptionAndWarning(TdsParserStateObject stateObj, Boolean callerHasConnectionLock, Boolean asyncClose)
   at System.Data.SqlClient.TdsParser.TryRun(RunBehavior runBehavior, SqlCommand cmdHandler, SqlDataReader dataStream, BulkCopySimpleResultSet bulkCopyHandler, TdsParserStateObject stateObj, Boolean& dataReady)
   at System.Data.SqlClient.SqlDataReader.TryConsumeMetaData()
   at System.Data.SqlClient.SqlDataReader.get_MetaData()
   at System.Data.SqlClient.SqlCommand.FinishExecuteReader(SqlDataReader ds, RunBehavior runBehavior, String resetOptionsString, Boolean isInternal, Boolean forDescribeParameterEncryption, Boolean shouldCacheForAlwaysEncrypted)
   at System.Data.SqlClient.SqlCommand.RunExecuteReaderTds(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, Boolean async, Int32 timeout, Task& task, Boolean asyncWrite, Boolean inRetry, SqlDataReader ds, Boolean describeParameterEncryptionRequest)
   at System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method, TaskCompletionSource`1 completion, Int32 timeout, Task& task, Boolean& usedCache, Boolean asyncWrite, Boolean inRetry)
   at System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method)
   at System.Data.SqlClient.SqlCommand.ExecuteScalar()
   at MZXRM.PSS.DataManager.SaleDataManager.SaveDO(Dictionary`2 keyValues) in C:\Users\Dell\Source\Repos\PetrocoalSalesSystem\PSS\MZXRM.PSS.DataManager\SaleDataManager.cs:line 185
   at MZXRM.PSS.Business.SaleManager.CreateDO(Dictionary`2 values) in C:\Users\Dell\Source\Repos\PetrocoalSalesSystem\PSS\MZXRM.PSS.Business\SaleManager.cs:line 196', NULL, N'Creating SO')
INSERT [dbo].[ExceptionLogs] ([Id], [ExceptionDate], [ExceptionMessage], [ExceptionType], [ExceptionSource], [ExceptionURL], [ExceptionSystem]) VALUES (10034, CAST(N'2018-10-05 17:03:08.103' AS DateTime), N'Procedure or function ''sp_InsertDO'' expects parameter ''@StoreId'', which was not supplied.', N'SqlException', N'   at System.Data.SqlClient.SqlConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
   at System.Data.SqlClient.SqlInternalConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
   at System.Data.SqlClient.TdsParser.ThrowExceptionAndWarning(TdsParserStateObject stateObj, Boolean callerHasConnectionLock, Boolean asyncClose)
   at System.Data.SqlClient.TdsParser.TryRun(RunBehavior runBehavior, SqlCommand cmdHandler, SqlDataReader dataStream, BulkCopySimpleResultSet bulkCopyHandler, TdsParserStateObject stateObj, Boolean& dataReady)
   at System.Data.SqlClient.SqlDataReader.TryConsumeMetaData()
   at System.Data.SqlClient.SqlDataReader.get_MetaData()
   at System.Data.SqlClient.SqlCommand.FinishExecuteReader(SqlDataReader ds, RunBehavior runBehavior, String resetOptionsString, Boolean isInternal, Boolean forDescribeParameterEncryption, Boolean shouldCacheForAlwaysEncrypted)
   at System.Data.SqlClient.SqlCommand.RunExecuteReaderTds(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, Boolean async, Int32 timeout, Task& task, Boolean asyncWrite, Boolean inRetry, SqlDataReader ds, Boolean describeParameterEncryptionRequest)
   at System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method, TaskCompletionSource`1 completion, Int32 timeout, Task& task, Boolean& usedCache, Boolean asyncWrite, Boolean inRetry)
   at System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method)
   at System.Data.SqlClient.SqlCommand.ExecuteScalar()
   at MZXRM.PSS.DataManager.SaleDataManager.SaveDO(Dictionary`2 keyValues) in C:\Users\Dell\Source\Repos\PetrocoalSalesSystem\PSS\MZXRM.PSS.DataManager\SaleDataManager.cs:line 185
   at MZXRM.PSS.Business.SaleManager.CreateDO(Dictionary`2 values)', NULL, N'Creating SO')
INSERT [dbo].[ExceptionLogs] ([Id], [ExceptionDate], [ExceptionMessage], [ExceptionType], [ExceptionSource], [ExceptionURL], [ExceptionSystem]) VALUES (10035, CAST(N'2018-10-05 17:42:18.947' AS DateTime), N'No mapping exists from object type MZXRM.PSS.Data.Reference to a known managed provider native type.', N'ArgumentException', N'   at System.Data.SqlClient.MetaType.GetMetaTypeFromValue(Type dataType, Object value, Boolean inferLen, Boolean streamAllowed)
   at System.Data.SqlClient.SqlParameter.GetMetaTypeOnly()
   at System.Data.SqlClient.SqlParameter.Validate(Int32 index, Boolean isCommandProc)
   at System.Data.SqlClient.SqlCommand.SetUpRPCParameters(_SqlRPC rpc, Int32 startCount, Boolean inSchema, SqlParameterCollection parameters)
   at System.Data.SqlClient.SqlCommand.BuildRPC(Boolean inSchema, SqlParameterCollection parameters, _SqlRPC& rpc)
   at System.Data.SqlClient.SqlCommand.RunExecuteReaderTds(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, Boolean async, Int32 timeout, Task& task, Boolean asyncWrite, Boolean inRetry, SqlDataReader ds, Boolean describeParameterEncryptionRequest)
   at System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method, TaskCompletionSource`1 completion, Int32 timeout, Task& task, Boolean& usedCache, Boolean asyncWrite, Boolean inRetry)
   at System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method)
   at System.Data.SqlClient.SqlCommand.ExecuteScalar()
   at MZXRM.PSS.DataManager.SaleDataManager.SaveDO(Dictionary`2 keyValues) in C:\Users\Dell\Source\Repos\PetrocoalSalesSystem\PSS\MZXRM.PSS.DataManager\SaleDataManager.cs:line 185
   at MZXRM.PSS.Business.SaleManager.CreateDO(Dictionary`2 values)', NULL, N'Creating SO')
INSERT [dbo].[ExceptionLogs] ([Id], [ExceptionDate], [ExceptionMessage], [ExceptionType], [ExceptionSource], [ExceptionURL], [ExceptionSystem]) VALUES (10036, CAST(N'2018-10-05 17:48:16.370' AS DateTime), N'Object reference not set to an instance of an object.', N'NullReferenceException', N'   at MZXRM.PSS.Business.DBMap.SOMap.reMapDOData(DeliveryOrder DO)
   at MZXRM.PSS.Business.SaleManager.UpdateDO(Dictionary`2 values)', NULL, N'Creating SO')
INSERT [dbo].[ExceptionLogs] ([Id], [ExceptionDate], [ExceptionMessage], [ExceptionType], [ExceptionSource], [ExceptionURL], [ExceptionSystem]) VALUES (10037, CAST(N'2018-10-05 17:49:53.013' AS DateTime), N'Object reference not set to an instance of an object.', N'NullReferenceException', N'   at MZXRM.PSS.Business.DBMap.SOMap.reMapDOData(DeliveryOrder DO)
   at MZXRM.PSS.Business.SaleManager.UpdateDO(Dictionary`2 values)', NULL, N'Creating SO')
INSERT [dbo].[ExceptionLogs] ([Id], [ExceptionDate], [ExceptionMessage], [ExceptionType], [ExceptionSource], [ExceptionURL], [ExceptionSystem]) VALUES (10038, CAST(N'2018-10-05 17:51:38.873' AS DateTime), N'The given key was not present in the dictionary.', N'KeyNotFoundException', N'   at System.Collections.Generic.Dictionary`2.get_Item(TKey key)
   at MZXRM.PSS.Business.SaleManager.UpdateDO(Dictionary`2 values)', NULL, N'Creating SO')
INSERT [dbo].[ExceptionLogs] ([Id], [ExceptionDate], [ExceptionMessage], [ExceptionType], [ExceptionSource], [ExceptionURL], [ExceptionSystem]) VALUES (10039, CAST(N'2018-10-05 17:53:18.530' AS DateTime), N'The given key was not present in the dictionary.', N'KeyNotFoundException', N'   at System.Collections.Generic.Dictionary`2.get_Item(TKey key)
   at MZXRM.PSS.Business.SaleManager.UpdateDO(Dictionary`2 values) in C:\Users\Dell\Source\Repos\PetrocoalSalesSystem\PSS\MZXRM.PSS.Business\SaleManager.cs:line 215', NULL, N'Creating SO')
INSERT [dbo].[ExceptionLogs] ([Id], [ExceptionDate], [ExceptionMessage], [ExceptionType], [ExceptionSource], [ExceptionURL], [ExceptionSystem]) VALUES (10040, CAST(N'2018-10-06 14:46:43.137' AS DateTime), N'The given key was not present in the dictionary.', N'KeyNotFoundException', N'   at System.Collections.Generic.Dictionary`2.get_Item(TKey key)
   at MZXRM.PSS.Business.SaleManager.UpdateDO(Dictionary`2 values) in C:\Users\Dell\Source\Repos\PetrocoalSalesSystem\PSS\MZXRM.PSS.Business\SaleManager.cs:line 215', NULL, N'Creating SO')
INSERT [dbo].[ExceptionLogs] ([Id], [ExceptionDate], [ExceptionMessage], [ExceptionType], [ExceptionSource], [ExceptionURL], [ExceptionSystem]) VALUES (10041, CAST(N'2018-10-06 15:02:38.473' AS DateTime), N'The given key was not present in the dictionary.', N'KeyNotFoundException', N'   at System.Collections.Generic.Dictionary`2.get_Item(TKey key)
   at MZXRM.PSS.Business.SaleManager.UpdateDO(Dictionary`2 values) in C:\Users\Dell\Source\Repos\PetrocoalSalesSystem\PSS\MZXRM.PSS.Business\SaleManager.cs:line 215', NULL, N'Creating SO')
INSERT [dbo].[ExceptionLogs] ([Id], [ExceptionDate], [ExceptionMessage], [ExceptionType], [ExceptionSource], [ExceptionURL], [ExceptionSystem]) VALUES (10042, CAST(N'2018-10-07 15:57:03.193' AS DateTime), N'The given key was not present in the dictionary.', N'KeyNotFoundException', N'   at System.Collections.Generic.Dictionary`2.get_Item(TKey key)
   at MZXRM.PSS.Business.SaleManager.CreateSO(Dictionary`2 values) in C:\Users\Dell\Source\Repos\PetrocoalSalesSystem\PSS\MZXRM.PSS.Business\SaleManager.cs:line 414', NULL, N'Creating SO')
INSERT [dbo].[ExceptionLogs] ([Id], [ExceptionDate], [ExceptionMessage], [ExceptionType], [ExceptionSource], [ExceptionURL], [ExceptionSystem]) VALUES (10043, CAST(N'2018-10-09 07:04:59.477' AS DateTime), N'The given key was not present in the dictionary.', N'KeyNotFoundException', N'   at System.Collections.Generic.Dictionary`2.get_Item(TKey key)
   at MZXRM.PSS.Business.SaleManager.CreateDC(Dictionary`2 values) in C:\Users\Dell\Source\Repos\PetrocoalSalesSystem\PSS\MZXRM.PSS.Business\SaleManager.cs:line 263', NULL, N'Creating SO')
INSERT [dbo].[ExceptionLogs] ([Id], [ExceptionDate], [ExceptionMessage], [ExceptionType], [ExceptionSource], [ExceptionURL], [ExceptionSystem]) VALUES (10044, CAST(N'2018-10-09 07:13:58.943' AS DateTime), N'The given key was not present in the dictionary.', N'KeyNotFoundException', N'   at System.Collections.Generic.Dictionary`2.get_Item(TKey key)
   at MZXRM.PSS.Business.SaleManager.CreateDC(Dictionary`2 values) in C:\Users\Dell\Source\Repos\PetrocoalSalesSystem\PSS\MZXRM.PSS.Business\SaleManager.cs:line 263', NULL, N'Creating SO')
INSERT [dbo].[ExceptionLogs] ([Id], [ExceptionDate], [ExceptionMessage], [ExceptionType], [ExceptionSource], [ExceptionURL], [ExceptionSystem]) VALUES (10045, CAST(N'2018-10-09 07:19:15.443' AS DateTime), N'The given key was not present in the dictionary.', N'KeyNotFoundException', N'   at System.Collections.Generic.Dictionary`2.get_Item(TKey key)
   at MZXRM.PSS.Business.SaleManager.CreateDC(Dictionary`2 values) in C:\Users\Dell\Source\Repos\PetrocoalSalesSystem\PSS\MZXRM.PSS.Business\SaleManager.cs:line 266', NULL, N'Creating SO')
INSERT [dbo].[ExceptionLogs] ([Id], [ExceptionDate], [ExceptionMessage], [ExceptionType], [ExceptionSource], [ExceptionURL], [ExceptionSystem]) VALUES (10046, CAST(N'2018-10-09 07:23:27.833' AS DateTime), N'The given key was not present in the dictionary.', N'KeyNotFoundException', N'   at System.Collections.Generic.Dictionary`2.get_Item(TKey key)
   at MZXRM.PSS.Business.SaleManager.CreateDC(Dictionary`2 values)', NULL, N'Creating SO')
INSERT [dbo].[ExceptionLogs] ([Id], [ExceptionDate], [ExceptionMessage], [ExceptionType], [ExceptionSource], [ExceptionURL], [ExceptionSystem]) VALUES (10047, CAST(N'2018-10-10 13:47:39.003' AS DateTime), N'The given key was not present in the dictionary.', N'KeyNotFoundException', N'   at System.Collections.Generic.Dictionary`2.get_Item(TKey key)
   at MZXRM.PSS.Business.SaleManager.CreateDC(Dictionary`2 values)', NULL, N'Creating SO')
INSERT [dbo].[ExceptionLogs] ([Id], [ExceptionDate], [ExceptionMessage], [ExceptionType], [ExceptionSource], [ExceptionURL], [ExceptionSystem]) VALUES (10048, CAST(N'2018-10-10 14:02:24.407' AS DateTime), N'Procedure or function ''sp_InsertDC'' expects parameter ''@LeadId'', which was not supplied.', N'SqlException', N'   at System.Data.SqlClient.SqlConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
   at System.Data.SqlClient.SqlInternalConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
   at System.Data.SqlClient.TdsParser.ThrowExceptionAndWarning(TdsParserStateObject stateObj, Boolean callerHasConnectionLock, Boolean asyncClose)
   at System.Data.SqlClient.TdsParser.TryRun(RunBehavior runBehavior, SqlCommand cmdHandler, SqlDataReader dataStream, BulkCopySimpleResultSet bulkCopyHandler, TdsParserStateObject stateObj, Boolean& dataReady)
   at System.Data.SqlClient.SqlDataReader.TryConsumeMetaData()
   at System.Data.SqlClient.SqlDataReader.get_MetaData()
   at System.Data.SqlClient.SqlCommand.FinishExecuteReader(SqlDataReader ds, RunBehavior runBehavior, String resetOptionsString, Boolean isInternal, Boolean forDescribeParameterEncryption, Boolean shouldCacheForAlwaysEncrypted)
   at System.Data.SqlClient.SqlCommand.RunExecuteReaderTds(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, Boolean async, Int32 timeout, Task& task, Boolean asyncWrite, Boolean inRetry, SqlDataReader ds, Boolean describeParameterEncryptionRequest)
   at System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method, TaskCompletionSource`1 completion, Int32 timeout, Task& task, Boolean& usedCache, Boolean asyncWrite, Boolean inRetry)
   at System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method)
   at System.Data.SqlClient.SqlCommand.ExecuteScalar()
   at MZXRM.PSS.DataManager.SaleDataManager.SaveDC(Dictionary`2 keyValues) in C:\Users\Dell\Source\Repos\PetrocoalSalesSystem\PSS\MZXRM.PSS.DataManager\SaleDataManager.cs:line 209
   at MZXRM.PSS.Business.SaleManager.CreateDC(Dictionary`2 values)', NULL, N'Creating SO')
INSERT [dbo].[ExceptionLogs] ([Id], [ExceptionDate], [ExceptionMessage], [ExceptionType], [ExceptionSource], [ExceptionURL], [ExceptionSystem]) VALUES (10049, CAST(N'2018-10-10 14:03:17.290' AS DateTime), N'Operand type clash: int is incompatible with uniqueidentifier', N'SqlException', N'   at System.Data.SqlClient.SqlConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
   at System.Data.SqlClient.SqlInternalConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
   at System.Data.SqlClient.TdsParser.ThrowExceptionAndWarning(TdsParserStateObject stateObj, Boolean callerHasConnectionLock, Boolean asyncClose)
   at System.Data.SqlClient.TdsParser.TryRun(RunBehavior runBehavior, SqlCommand cmdHandler, SqlDataReader dataStream, BulkCopySimpleResultSet bulkCopyHandler, TdsParserStateObject stateObj, Boolean& dataReady)
   at System.Data.SqlClient.SqlDataReader.TryConsumeMetaData()
   at System.Data.SqlClient.SqlDataReader.get_MetaData()
   at System.Data.SqlClient.SqlCommand.FinishExecuteReader(SqlDataReader ds, RunBehavior runBehavior, String resetOptionsString, Boolean isInternal, Boolean forDescribeParameterEncryption, Boolean shouldCacheForAlwaysEncrypted)
   at System.Data.SqlClient.SqlCommand.RunExecuteReaderTds(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, Boolean async, Int32 timeout, Task& task, Boolean asyncWrite, Boolean inRetry, SqlDataReader ds, Boolean describeParameterEncryptionRequest)
   at System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method, TaskCompletionSource`1 completion, Int32 timeout, Task& task, Boolean& usedCache, Boolean asyncWrite, Boolean inRetry)
   at System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method)
   at System.Data.SqlClient.SqlCommand.ExecuteScalar()
   at MZXRM.PSS.DataManager.SaleDataManager.SaveDC(Dictionary`2 keyValues) in C:\Users\Dell\Source\Repos\PetrocoalSalesSystem\PSS\MZXRM.PSS.DataManager\SaleDataManager.cs:line 209
   at MZXRM.PSS.Business.SaleManager.CreateDC(Dictionary`2 values)', NULL, N'Creating SO')
INSERT [dbo].[ExceptionLogs] ([Id], [ExceptionDate], [ExceptionMessage], [ExceptionType], [ExceptionSource], [ExceptionURL], [ExceptionSystem]) VALUES (10050, CAST(N'2018-10-10 22:09:44.980' AS DateTime), N'Cannot insert the value NULL into column ''VesselId'', table ''Petrocoal.dbo.SO''; column does not allow nulls. INSERT fails.
The statement has been terminated.', N'SqlException', N'   at System.Data.SqlClient.SqlConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
   at System.Data.SqlClient.SqlInternalConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
   at System.Data.SqlClient.TdsParser.ThrowExceptionAndWarning(TdsParserStateObject stateObj, Boolean callerHasConnectionLock, Boolean asyncClose)
   at System.Data.SqlClient.TdsParser.TryRun(RunBehavior runBehavior, SqlCommand cmdHandler, SqlDataReader dataStream, BulkCopySimpleResultSet bulkCopyHandler, TdsParserStateObject stateObj, Boolean& dataReady)
   at System.Data.SqlClient.SqlDataReader.TryConsumeMetaData()
   at System.Data.SqlClient.SqlDataReader.get_MetaData()
   at System.Data.SqlClient.SqlCommand.FinishExecuteReader(SqlDataReader ds, RunBehavior runBehavior, String resetOptionsString, Boolean isInternal, Boolean forDescribeParameterEncryption, Boolean shouldCacheForAlwaysEncrypted)
   at System.Data.SqlClient.SqlCommand.RunExecuteReaderTds(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, Boolean async, Int32 timeout, Task& task, Boolean asyncWrite, Boolean inRetry, SqlDataReader ds, Boolean describeParameterEncryptionRequest)
   at System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method, TaskCompletionSource`1 completion, Int32 timeout, Task& task, Boolean& usedCache, Boolean asyncWrite, Boolean inRetry)
   at System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method)
   at System.Data.SqlClient.SqlCommand.ExecuteScalar()
   at MZXRM.PSS.DataManager.SaleDataManager.SaveSO(Dictionary`2 keyValues) in C:\Users\Dell\Source\Repos\PetrocoalSalesSystem\PSS\MZXRM.PSS.DataManager\SaleDataManager.cs:line 141
   at MZXRM.PSS.Business.SaleManager.CreateSO(Dictionary`2 values)', NULL, N'Creating SO')
INSERT [dbo].[ExceptionLogs] ([Id], [ExceptionDate], [ExceptionMessage], [ExceptionType], [ExceptionSource], [ExceptionURL], [ExceptionSystem]) VALUES (10051, CAST(N'2018-10-10 22:19:19.340' AS DateTime), N'The given key was not present in the dictionary.', N'KeyNotFoundException', N'   at System.Collections.Generic.Dictionary`2.get_Item(TKey key)
   at MZXRM.PSS.Business.SaleManager.CreateSO(Dictionary`2 values) in C:\Users\Dell\Source\Repos\PetrocoalSalesSystem\PSS\MZXRM.PSS.Business\SaleManager.cs:line 423', NULL, N'Creating SO')
INSERT [dbo].[ExceptionLogs] ([Id], [ExceptionDate], [ExceptionMessage], [ExceptionType], [ExceptionSource], [ExceptionURL], [ExceptionSystem]) VALUES (10052, CAST(N'2018-10-10 22:23:42.457' AS DateTime), N'The given key was not present in the dictionary.', N'KeyNotFoundException', N'   at System.Collections.Generic.Dictionary`2.get_Item(TKey key)
   at MZXRM.PSS.Business.SaleManager.CreateSO(Dictionary`2 values)', NULL, N'Creating SO')
INSERT [dbo].[ExceptionLogs] ([Id], [ExceptionDate], [ExceptionMessage], [ExceptionType], [ExceptionSource], [ExceptionURL], [ExceptionSystem]) VALUES (10053, CAST(N'2018-10-10 22:27:13.280' AS DateTime), N'Object reference not set to an instance of an object.', N'NullReferenceException', N'   at MZXRM.PSS.Business.DBMap.SOMap.reMapSOData(SaleOrder SO)
   at MZXRM.PSS.Business.SaleManager.CreateSO(Dictionary`2 values)', NULL, N'Creating SO')
INSERT [dbo].[ExceptionLogs] ([Id], [ExceptionDate], [ExceptionMessage], [ExceptionType], [ExceptionSource], [ExceptionURL], [ExceptionSystem]) VALUES (10054, CAST(N'2018-10-10 22:54:40.430' AS DateTime), N'An item with the same key has already been added.', N'ArgumentException', N'   at System.ThrowHelper.ThrowArgumentException(ExceptionResource resource)
   at System.Collections.Generic.Dictionary`2.Insert(TKey key, TValue value, Boolean add)
   at System.Collections.Generic.Dictionary`2.Add(TKey key, TValue value)
   at MZXRM.PSS.Business.DBMap.SOMap.reMapSOData(SaleOrder SO)
   at MZXRM.PSS.Business.SaleManager.CreateSO(Dictionary`2 values)', NULL, N'Creating SO')
INSERT [dbo].[ExceptionLogs] ([Id], [ExceptionDate], [ExceptionMessage], [ExceptionType], [ExceptionSource], [ExceptionURL], [ExceptionSystem]) VALUES (10055, CAST(N'2018-10-13 01:22:31.820' AS DateTime), N'An item with the same key has already been added.', N'ArgumentException', N'   at System.ThrowHelper.ThrowArgumentException(ExceptionResource resource)
   at System.Collections.Generic.Dictionary`2.Insert(TKey key, TValue value, Boolean add)
   at System.Collections.Generic.Dictionary`2.Add(TKey key, TValue value)
   at MZXRM.PSS.Business.DBMap.SOMap.reMapSOData(SaleOrder SO) in C:\Users\Dell\Source\Repos\PetrocoalSalesSystem\PSS\MZXRM.PSS.Business\DBMap\SOMap.cs:line 176
   at MZXRM.PSS.Business.SaleManager.CreateSO(Dictionary`2 values) in C:\Users\Dell\Source\Repos\PetrocoalSalesSystem\PSS\MZXRM.PSS.Business\SaleManager.cs:line 455', NULL, N'Creating SO')
INSERT [dbo].[ExceptionLogs] ([Id], [ExceptionDate], [ExceptionMessage], [ExceptionType], [ExceptionSource], [ExceptionURL], [ExceptionSystem]) VALUES (10056, CAST(N'2018-10-13 01:33:23.493' AS DateTime), N'An item with the same key has already been added.', N'ArgumentException', N'   at System.ThrowHelper.ThrowArgumentException(ExceptionResource resource)
   at System.Collections.Generic.Dictionary`2.Insert(TKey key, TValue value, Boolean add)
   at System.Collections.Generic.Dictionary`2.Add(TKey key, TValue value)
   at MZXRM.PSS.Business.DBMap.SOMap.reMapSOData(SaleOrder SO) in C:\Users\Dell\Source\Repos\PetrocoalSalesSystem\PSS\MZXRM.PSS.Business\DBMap\SOMap.cs:line 176
   at MZXRM.PSS.Business.SaleManager.CreateSO(Dictionary`2 values) in C:\Users\Dell\Source\Repos\PetrocoalSalesSystem\PSS\MZXRM.PSS.Business\SaleManager.cs:line 455', NULL, N'Creating SO')
INSERT [dbo].[ExceptionLogs] ([Id], [ExceptionDate], [ExceptionMessage], [ExceptionType], [ExceptionSource], [ExceptionURL], [ExceptionSystem]) VALUES (10057, CAST(N'2018-10-13 02:45:39.157' AS DateTime), N'The given key was not present in the dictionary.', N'KeyNotFoundException', N'   at System.Collections.Generic.Dictionary`2.get_Item(TKey key)
   at MZXRM.PSS.Business.SaleManager.UpdateSO(Dictionary`2 values) in C:\Users\Dell\Source\Repos\PetrocoalSalesSystem\PSS\MZXRM.PSS.Business\SaleManager.cs:line 369', NULL, N'Creating SO')
INSERT [dbo].[ExceptionLogs] ([Id], [ExceptionDate], [ExceptionMessage], [ExceptionType], [ExceptionSource], [ExceptionURL], [ExceptionSystem]) VALUES (10058, CAST(N'2018-10-13 02:59:15.393' AS DateTime), N'The given key was not present in the dictionary.', N'KeyNotFoundException', N'   at System.Collections.Generic.Dictionary`2.get_Item(TKey key)
   at MZXRM.PSS.Business.SaleManager.UpdateSO(Dictionary`2 values) in C:\Users\Dell\Source\Repos\PetrocoalSalesSystem\PSS\MZXRM.PSS.Business\SaleManager.cs:line 369', NULL, N'Creating SO')
INSERT [dbo].[ExceptionLogs] ([Id], [ExceptionDate], [ExceptionMessage], [ExceptionType], [ExceptionSource], [ExceptionURL], [ExceptionSystem]) VALUES (10059, CAST(N'2018-10-13 03:09:36.327' AS DateTime), N'The given key was not present in the dictionary.', N'KeyNotFoundException', N'   at System.Collections.Generic.Dictionary`2.get_Item(TKey key)
   at MZXRM.PSS.Business.SaleManager.UpdateSO(Dictionary`2 values) in C:\Users\Dell\Source\Repos\PetrocoalSalesSystem\PSS\MZXRM.PSS.Business\SaleManager.cs:line 369', NULL, N'Creating SO')
INSERT [dbo].[ExceptionLogs] ([Id], [ExceptionDate], [ExceptionMessage], [ExceptionType], [ExceptionSource], [ExceptionURL], [ExceptionSystem]) VALUES (10060, CAST(N'2018-10-13 03:11:32.977' AS DateTime), N'The given key was not present in the dictionary.', N'KeyNotFoundException', N'   at System.Collections.Generic.Dictionary`2.get_Item(TKey key)
   at MZXRM.PSS.Business.SaleManager.UpdateSO(Dictionary`2 values) in C:\Users\Dell\Source\Repos\PetrocoalSalesSystem\PSS\MZXRM.PSS.Business\SaleManager.cs:line 369', NULL, N'Creating SO')
INSERT [dbo].[ExceptionLogs] ([Id], [ExceptionDate], [ExceptionMessage], [ExceptionType], [ExceptionSource], [ExceptionURL], [ExceptionSystem]) VALUES (10061, CAST(N'2018-10-13 03:15:04.793' AS DateTime), N'The given key was not present in the dictionary.', N'KeyNotFoundException', N'   at System.Collections.Generic.Dictionary`2.get_Item(TKey key)
   at MZXRM.PSS.Business.SaleManager.UpdateSO(Dictionary`2 values) in C:\Users\Dell\Source\Repos\PetrocoalSalesSystem\PSS\MZXRM.PSS.Business\SaleManager.cs:line 369', NULL, N'Creating SO')
INSERT [dbo].[ExceptionLogs] ([Id], [ExceptionDate], [ExceptionMessage], [ExceptionType], [ExceptionSource], [ExceptionURL], [ExceptionSystem]) VALUES (10062, CAST(N'2018-10-13 03:22:48.080' AS DateTime), N'The given key was not present in the dictionary.', N'KeyNotFoundException', N'   at System.Collections.Generic.Dictionary`2.get_Item(TKey key)
   at MZXRM.PSS.Business.SaleManager.UpdateSO(Dictionary`2 values) in C:\Users\Dell\Source\Repos\PetrocoalSalesSystem\PSS\MZXRM.PSS.Business\SaleManager.cs:line 369', NULL, N'Creating SO')
INSERT [dbo].[ExceptionLogs] ([Id], [ExceptionDate], [ExceptionMessage], [ExceptionType], [ExceptionSource], [ExceptionURL], [ExceptionSystem]) VALUES (10063, CAST(N'2018-10-26 21:35:40.950' AS DateTime), N'Guid should contain 32 digits with 4 dashes (xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx).', N'FormatException', N'   at System.Guid.TryParseGuidWithNoStyle(String guidString, GuidResult& result)
   at System.Guid.TryParseGuid(String g, GuidStyles flags, GuidResult& result)
   at System.Guid..ctor(String g)
   at MZXRM.PSS.Business.StoreManager.GetStoreRef(String id) in C:\Users\Dell\Source\Repos\PetrocoalSalesSystem\PSS\MZXRM.PSS.Business\StoreManager.cs:line 250
   at MZXRM.PSS.Business.SaleManager.UpdateDC(Dictionary`2 values) in C:\Users\Dell\Source\Repos\PetrocoalSalesSystem\PSS\MZXRM.PSS.Business\SaleManager.cs:line 323', NULL, N'Creating SO')
SET IDENTITY_INSERT [dbo].[ExceptionLogs] OFF
INSERT [dbo].[GRN] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [CompletedOn], [GRNNumber], [GRNDate], [PODetailId], [Store], [InvoiceNo], [AdjPrice], [Quantity], [Remarks]) VALUES (N'2ae21360-7d76-41a8-951a-006b6201d70d', 1, CAST(N'2018-08-09 02:15:34.133' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-08-09 02:15:34.133' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-08-09 02:15:34.133' AS DateTime), N'GRN-1005', CAST(N'2018-08-09 00:00:00.000' AS DateTime), N'2eab4f65-84b5-409e-9e9e-e6dc2f80b9a9', N'8b13ea2a-67e4-487f-8f02-11c9be1a1983', N'1234', CAST(5000 AS Decimal(18, 0)), CAST(20000 AS Decimal(18, 0)), N'sdfasdf')
INSERT [dbo].[GRN] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [CompletedOn], [GRNNumber], [GRNDate], [PODetailId], [Store], [InvoiceNo], [AdjPrice], [Quantity], [Remarks]) VALUES (N'39fd6b52-07bf-4f60-97e8-1d595e81a5c9', 1, CAST(N'2018-08-09 12:18:25.203' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-08-09 12:18:25.203' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-08-09 12:18:25.203' AS DateTime), N'GRN-1007', CAST(N'2018-08-09 00:00:00.000' AS DateTime), N'1c850e76-dc7e-4dd4-ab86-5eaa92bce505', N'0ce63c9c-9e2c-4535-a8d0-5ccec93eca6c', N'1234', CAST(5000 AS Decimal(18, 0)), CAST(20000 AS Decimal(18, 0)), N'sdfasdf')
INSERT [dbo].[GRN] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [CompletedOn], [GRNNumber], [GRNDate], [PODetailId], [Store], [InvoiceNo], [AdjPrice], [Quantity], [Remarks]) VALUES (N'16820814-b10d-4703-a6fb-48884f1b9f18', 1, CAST(N'2018-08-09 03:15:30.877' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-08-09 03:15:30.877' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-08-09 03:15:30.877' AS DateTime), N'GRN-1006', CAST(N'2018-08-09 00:00:00.000' AS DateTime), N'05e79baa-78e4-4e8f-8df3-d3c810226614', N'0ce63c9c-9e2c-4535-a8d0-5ccec93eca6c', N'1234', CAST(5000 AS Decimal(18, 0)), CAST(20000 AS Decimal(18, 0)), N'sdfasdf')
INSERT [dbo].[GRN] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [CompletedOn], [GRNNumber], [GRNDate], [PODetailId], [Store], [InvoiceNo], [AdjPrice], [Quantity], [Remarks]) VALUES (N'dcbffe32-ecdd-4ea8-aa31-4bc180967214', 1, CAST(N'2018-04-04 14:33:30.673' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-04-04 14:33:30.673' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-04-04 14:33:30.673' AS DateTime), N'GRN-1004', CAST(N'2018-04-04 00:00:00.000' AS DateTime), N'95b4b3a4-28e0-4afd-81b0-a0f59bc4d833', N'3473d11c-43fd-45b2-9cb0-f4a9c0da1610', N'asdfg', CAST(45 AS Decimal(18, 0)), CAST(5000 AS Decimal(18, 0)), N'sdfg')
INSERT [dbo].[GRN] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [CompletedOn], [GRNNumber], [GRNDate], [PODetailId], [Store], [InvoiceNo], [AdjPrice], [Quantity], [Remarks]) VALUES (N'3bd2f864-0644-4585-a996-55c10dfa5109', 1, CAST(N'2018-04-04 14:33:13.563' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-04-04 14:33:13.563' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-04-04 14:33:13.563' AS DateTime), N'GRN-1003', CAST(N'2018-04-04 00:00:00.000' AS DateTime), N'c2f29cd0-3309-4a4a-a194-fae1315e4cba', N'3473d11c-43fd-45b2-9cb0-f4a9c0da1610', N'asdfg', CAST(45 AS Decimal(18, 0)), CAST(10000 AS Decimal(18, 0)), N'sdfg')
INSERT [dbo].[GRN] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [CompletedOn], [GRNNumber], [GRNDate], [PODetailId], [Store], [InvoiceNo], [AdjPrice], [Quantity], [Remarks]) VALUES (N'32a2daab-5dff-431d-b2d6-5efd9111e900', 1, CAST(N'2018-08-09 01:56:46.027' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-08-09 01:56:46.027' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-08-09 01:56:46.027' AS DateTime), N'GRN-1005', CAST(N'2018-08-09 00:00:00.000' AS DateTime), N'2eab4f65-84b5-409e-9e9e-e6dc2f80b9a9', N'8b13ea2a-67e4-487f-8f02-11c9be1a1983', N'1234', CAST(5000 AS Decimal(18, 0)), CAST(20000 AS Decimal(18, 0)), N'sdfasdf')
INSERT [dbo].[GRN] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [CompletedOn], [GRNNumber], [GRNDate], [PODetailId], [Store], [InvoiceNo], [AdjPrice], [Quantity], [Remarks]) VALUES (N'2ed4e56d-3a3c-42e3-8f01-71d40752d23c', 1, CAST(N'2018-04-04 14:32:44.650' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-04-04 14:32:44.650' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-04-04 14:32:44.650' AS DateTime), N'GRN-1002', CAST(N'2018-04-04 00:00:00.000' AS DateTime), N'9c80abf1-9791-4d95-a0af-9e9047d94ca3', N'3473d11c-43fd-45b2-9cb0-f4a9c0da1610', N'asdfg', CAST(45 AS Decimal(18, 0)), CAST(10000 AS Decimal(18, 0)), N'sdfg')
INSERT [dbo].[GRN] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [CompletedOn], [GRNNumber], [GRNDate], [PODetailId], [Store], [InvoiceNo], [AdjPrice], [Quantity], [Remarks]) VALUES (N'b61f1f2e-45d5-45c4-b223-99fb9aef687b', 1, CAST(N'2018-08-09 14:11:11.673' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-08-09 14:11:11.673' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-08-09 14:11:11.673' AS DateTime), N'GRN-1008', CAST(N'2018-08-09 00:00:00.000' AS DateTime), N'05e79baa-78e4-4e8f-8df3-d3c810226614', N'8b13ea2a-67e4-487f-8f02-11c9be1a1983', N'1234', CAST(5000 AS Decimal(18, 0)), CAST(30000 AS Decimal(18, 0)), N'sdfasdf')
INSERT [dbo].[GRN] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [CompletedOn], [GRNNumber], [GRNDate], [PODetailId], [Store], [InvoiceNo], [AdjPrice], [Quantity], [Remarks]) VALUES (N'41154328-34cc-4dfa-a392-c9beede4caa9', 1, CAST(N'2018-10-02 18:40:41.570' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-10-02 18:40:41.570' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-10-02 18:40:41.570' AS DateTime), N'GRN-1009', CAST(N'2018-10-02 00:00:00.000' AS DateTime), N'9c80abf1-9791-4d95-a0af-9e9047d94ca3', N'8b13ea2a-67e4-487f-8f02-11c9be1a1983', N'124', CAST(350 AS Decimal(18, 0)), CAST(1000 AS Decimal(18, 0)), N'Test')
INSERT [dbo].[Organization] ([Id], [Name]) VALUES (N'eadfde17-3b0b-4d2f-b266-2adef1ff157d', N'Petro Coal')
SET IDENTITY_INSERT [dbo].[Origin] ON 

INSERT [dbo].[Origin] ([Id], [Name], [Description], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (2, N'SA', NULL, 1, CAST(N'2018-02-15 13:02:31.197' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-02-15 13:02:31.197' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e')
INSERT [dbo].[Origin] ([Id], [Name], [Description], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (3, N'Indo', NULL, 1, CAST(N'2018-02-15 13:02:40.117' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-02-15 13:02:40.117' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e')
INSERT [dbo].[Origin] ([Id], [Name], [Description], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (4, N'Afghan', NULL, 1, CAST(N'2018-02-15 13:02:51.037' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-02-15 13:02:51.037' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e')
INSERT [dbo].[Origin] ([Id], [Name], [Description], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (5, N'Quette', NULL, 1, CAST(N'2018-02-15 13:02:54.193' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-02-15 13:02:54.193' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e')
INSERT [dbo].[Origin] ([Id], [Name], [Description], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (6, N'Russian', NULL, 1, CAST(N'2018-02-15 13:03:00.410' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-02-15 13:03:00.410' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e')
SET IDENTITY_INSERT [dbo].[Origin] OFF
INSERT [dbo].[PO] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [CompletedOn], [LeadId], [ApprovedDate], [ApprovedBy], [PONumber], [PODate], [Origin], [Size], [Vessel], [TargetDays], [Supplier], [TermsOfPayment], [BufferQuantityMax], [BufferQuantityMin]) VALUES (N'609e5703-a7ef-41de-b6cc-0b457aa8c760', 3, CAST(N'2018-04-04 14:31:43.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-04-04 14:31:43.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', NULL, N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-04-04 14:32:11.150' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'PO-1002', CAST(N'2018-04-04 00:00:00.000' AS DateTime), 3, 3, 1, 45, 1, N'123', CAST(10 AS Decimal(18, 0)), CAST(10 AS Decimal(18, 0)))
INSERT [dbo].[PO] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [CompletedOn], [LeadId], [ApprovedDate], [ApprovedBy], [PONumber], [PODate], [Origin], [Size], [Vessel], [TargetDays], [Supplier], [TermsOfPayment], [BufferQuantityMax], [BufferQuantityMin]) VALUES (N'dc1ab809-99e7-480b-b479-440fc5e42a82', 1, CAST(N'2018-08-08 23:39:14.930' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-08-08 23:39:14.930' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', NULL, N'1ee1700d-1ca9-4c56-983a-0285f581379e', NULL, N'00000000-0000-0000-0000-000000000000', N'PO-1005', CAST(N'2018-08-08 00:00:00.000' AS DateTime), 4, 6, 1, 45, 2, N'1111', CAST(10 AS Decimal(18, 0)), CAST(10 AS Decimal(18, 0)))
INSERT [dbo].[PO] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [CompletedOn], [LeadId], [ApprovedDate], [ApprovedBy], [PONumber], [PODate], [Origin], [Size], [Vessel], [TargetDays], [Supplier], [TermsOfPayment], [BufferQuantityMax], [BufferQuantityMin]) VALUES (N'e2b2aa56-2646-4286-8b24-5310fa1e2393', 4, CAST(N'2018-08-09 02:27:51.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-08-09 02:27:51.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', NULL, N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-08-09 03:14:19.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'PO-1007', CAST(N'2018-08-09 00:00:00.000' AS DateTime), 6, 6, 5, 45, 3, N'1111', CAST(10 AS Decimal(18, 0)), CAST(10 AS Decimal(18, 0)))
INSERT [dbo].[PO] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [CompletedOn], [LeadId], [ApprovedDate], [ApprovedBy], [PONumber], [PODate], [Origin], [Size], [Vessel], [TargetDays], [Supplier], [TermsOfPayment], [BufferQuantityMax], [BufferQuantityMin]) VALUES (N'63a13c00-78a0-4d31-aa7a-aff90a3e2772', 1, CAST(N'2018-04-19 02:36:02.587' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-04-19 02:36:02.587' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', NULL, N'1ee1700d-1ca9-4c56-983a-0285f581379e', NULL, N'00000000-0000-0000-0000-000000000000', N'PO-1003', CAST(N'2018-04-19 00:00:00.000' AS DateTime), 4, 4, 2, 45, 2, N'chk', CAST(10 AS Decimal(18, 0)), CAST(10 AS Decimal(18, 0)))
INSERT [dbo].[PO] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [CompletedOn], [LeadId], [ApprovedDate], [ApprovedBy], [PONumber], [PODate], [Origin], [Size], [Vessel], [TargetDays], [Supplier], [TermsOfPayment], [BufferQuantityMax], [BufferQuantityMin]) VALUES (N'af004c1a-14fc-414e-aeb6-b4b2d26c4971', 4, CAST(N'2018-08-07 00:24:28.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-08-07 00:24:28.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', NULL, N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-08-08 01:14:55.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'PO-1004', CAST(N'2018-08-07 00:00:00.000' AS DateTime), 3, 6, 7, 45, 2, N'1111', CAST(10 AS Decimal(18, 0)), CAST(10 AS Decimal(18, 0)))
INSERT [dbo].[PO] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [CompletedOn], [LeadId], [ApprovedDate], [ApprovedBy], [PONumber], [PODate], [Origin], [Size], [Vessel], [TargetDays], [Supplier], [TermsOfPayment], [BufferQuantityMax], [BufferQuantityMin]) VALUES (N'e2d5a9b7-aba4-47dd-adfc-f58fd8a8bb74', 4, CAST(N'2018-08-07 00:23:47.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-08-07 00:23:47.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', NULL, N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-08-09 01:03:52.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'PO-1006', CAST(N'2018-08-07 00:00:00.000' AS DateTime), 3, 6, 7, 45, 2, N'1111', CAST(10 AS Decimal(18, 0)), CAST(10 AS Decimal(18, 0)))
INSERT [dbo].[PODetail] ([Id], [POId], [CustomerId], [Quantity], [Rate], [CostPerTon], [AllowedWastage], [TargetDate], [Remarks]) VALUES (N'f6ebcc7c-d649-4a13-8e64-058dddc1c6c1', N'e2d5a9b7-aba4-47dd-adfc-f58fd8a8bb74', N'186e9015-b79c-49bd-993c-17ff0c12e623', CAST(50000 AS Decimal(18, 0)), CAST(1000 AS Decimal(18, 0)), CAST(85 AS Decimal(18, 0)), CAST(0.050 AS Decimal(18, 3)), CAST(N'2018-09-21 00:00:00.000' AS DateTime), N'')
INSERT [dbo].[PODetail] ([Id], [POId], [CustomerId], [Quantity], [Rate], [CostPerTon], [AllowedWastage], [TargetDate], [Remarks]) VALUES (N'fd8c1ec5-edb3-4003-b81e-418213fa3934', N'af004c1a-14fc-414e-aeb6-b4b2d26c4971', N'0bef2889-0462-432e-a4d3-085b9821c7a1', CAST(20000 AS Decimal(18, 0)), CAST(1000 AS Decimal(18, 0)), CAST(85 AS Decimal(18, 0)), CAST(0.050 AS Decimal(18, 3)), CAST(N'2018-09-21 00:00:00.000' AS DateTime), N'')
INSERT [dbo].[PODetail] ([Id], [POId], [CustomerId], [Quantity], [Rate], [CostPerTon], [AllowedWastage], [TargetDate], [Remarks]) VALUES (N'4b48bdb6-f912-4101-8093-4b5536b7ede5', N'af004c1a-14fc-414e-aeb6-b4b2d26c4971', N'186e9015-b79c-49bd-993c-17ff0c12e623', CAST(50000 AS Decimal(18, 0)), CAST(1000 AS Decimal(18, 0)), CAST(85 AS Decimal(18, 0)), CAST(0.050 AS Decimal(18, 3)), CAST(N'2018-09-21 00:00:00.000' AS DateTime), N'')
INSERT [dbo].[PODetail] ([Id], [POId], [CustomerId], [Quantity], [Rate], [CostPerTon], [AllowedWastage], [TargetDate], [Remarks]) VALUES (N'6eb05647-cd4c-450e-85ae-4e0b52c3e6eb', N'63a13c00-78a0-4d31-aa7a-aff90a3e2772', N'0bef2889-0462-432e-a4d3-085b9821c7a1', CAST(10000 AS Decimal(18, 0)), CAST(100 AS Decimal(18, 0)), CAST(100 AS Decimal(18, 0)), CAST(0.050 AS Decimal(18, 3)), CAST(N'2018-06-03 00:00:00.000' AS DateTime), N'')
INSERT [dbo].[PODetail] ([Id], [POId], [CustomerId], [Quantity], [Rate], [CostPerTon], [AllowedWastage], [TargetDate], [Remarks]) VALUES (N'1c850e76-dc7e-4dd4-ab86-5eaa92bce505', N'e2b2aa56-2646-4286-8b24-5310fa1e2393', N'0bef2889-0462-432e-a4d3-085b9821c7a1', CAST(20000 AS Decimal(18, 0)), CAST(1000 AS Decimal(18, 0)), CAST(85 AS Decimal(18, 0)), CAST(0.050 AS Decimal(18, 3)), CAST(N'2018-09-23 00:00:00.000' AS DateTime), N'')
INSERT [dbo].[PODetail] ([Id], [POId], [CustomerId], [Quantity], [Rate], [CostPerTon], [AllowedWastage], [TargetDate], [Remarks]) VALUES (N'7b67915a-acef-46c1-b37c-9978bb6d2a52', N'dc1ab809-99e7-480b-b479-440fc5e42a82', N'0bef2889-0462-432e-a4d3-085b9821c7a1', CAST(20000 AS Decimal(18, 0)), CAST(1000 AS Decimal(18, 0)), CAST(85 AS Decimal(18, 0)), CAST(0.050 AS Decimal(18, 3)), CAST(N'2018-09-22 00:00:00.000' AS DateTime), N'')
INSERT [dbo].[PODetail] ([Id], [POId], [CustomerId], [Quantity], [Rate], [CostPerTon], [AllowedWastage], [TargetDate], [Remarks]) VALUES (N'9c80abf1-9791-4d95-a0af-9e9047d94ca3', N'609e5703-a7ef-41de-b6cc-0b457aa8c760', N'4994d1c6-ad3c-4be4-932e-08f992f54531', CAST(10000 AS Decimal(18, 0)), CAST(1 AS Decimal(18, 0)), CAST(200 AS Decimal(18, 0)), CAST(0.050 AS Decimal(18, 3)), CAST(N'2018-05-01 00:00:00.000' AS DateTime), N'')
INSERT [dbo].[PODetail] ([Id], [POId], [CustomerId], [Quantity], [Rate], [CostPerTon], [AllowedWastage], [TargetDate], [Remarks]) VALUES (N'95b4b3a4-28e0-4afd-81b0-a0f59bc4d833', N'609e5703-a7ef-41de-b6cc-0b457aa8c760', N'bc746f17-04c9-4b1f-ac9a-eb599032f4d5', CAST(5000 AS Decimal(18, 0)), CAST(85 AS Decimal(18, 0)), CAST(200 AS Decimal(18, 0)), CAST(0.050 AS Decimal(18, 3)), CAST(N'2018-04-30 00:00:00.000' AS DateTime), N'')
INSERT [dbo].[PODetail] ([Id], [POId], [CustomerId], [Quantity], [Rate], [CostPerTon], [AllowedWastage], [TargetDate], [Remarks]) VALUES (N'e5390ec8-3e03-45ff-ba70-a7d60196fc2f', N'dc1ab809-99e7-480b-b479-440fc5e42a82', N'7bf9b13d-296b-4e71-8907-0d7fbf6334f8', CAST(50000 AS Decimal(18, 0)), CAST(1000 AS Decimal(18, 0)), CAST(85 AS Decimal(18, 0)), CAST(0.050 AS Decimal(18, 3)), CAST(N'2018-09-22 00:00:00.000' AS DateTime), N'')
INSERT [dbo].[PODetail] ([Id], [POId], [CustomerId], [Quantity], [Rate], [CostPerTon], [AllowedWastage], [TargetDate], [Remarks]) VALUES (N'05e79baa-78e4-4e8f-8df3-d3c810226614', N'e2b2aa56-2646-4286-8b24-5310fa1e2393', N'ccef174b-142c-4f2a-8db4-78afc0cec7e6', CAST(50000 AS Decimal(18, 0)), CAST(1000 AS Decimal(18, 0)), CAST(85 AS Decimal(18, 0)), CAST(0.050 AS Decimal(18, 3)), CAST(N'2018-09-23 00:00:00.000' AS DateTime), N'')
INSERT [dbo].[PODetail] ([Id], [POId], [CustomerId], [Quantity], [Rate], [CostPerTon], [AllowedWastage], [TargetDate], [Remarks]) VALUES (N'f8ee9a82-7fab-4a79-bf7c-e01af5ee6d5a', N'63a13c00-78a0-4d31-aa7a-aff90a3e2772', N'4994d1c6-ad3c-4be4-932e-08f992f54531', CAST(10000 AS Decimal(18, 0)), CAST(100 AS Decimal(18, 0)), CAST(200 AS Decimal(18, 0)), CAST(0.050 AS Decimal(18, 3)), CAST(N'2018-04-16 00:00:00.000' AS DateTime), N'')
INSERT [dbo].[PODetail] ([Id], [POId], [CustomerId], [Quantity], [Rate], [CostPerTon], [AllowedWastage], [TargetDate], [Remarks]) VALUES (N'2eab4f65-84b5-409e-9e9e-e6dc2f80b9a9', N'e2d5a9b7-aba4-47dd-adfc-f58fd8a8bb74', N'0bef2889-0462-432e-a4d3-085b9821c7a1', CAST(20000 AS Decimal(18, 0)), CAST(1000 AS Decimal(18, 0)), CAST(85 AS Decimal(18, 0)), CAST(0.050 AS Decimal(18, 3)), CAST(N'2018-09-21 00:00:00.000' AS DateTime), N'')
INSERT [dbo].[PODetail] ([Id], [POId], [CustomerId], [Quantity], [Rate], [CostPerTon], [AllowedWastage], [TargetDate], [Remarks]) VALUES (N'c2f29cd0-3309-4a4a-a194-fae1315e4cba', N'609e5703-a7ef-41de-b6cc-0b457aa8c760', N'0bef2889-0462-432e-a4d3-085b9821c7a1', CAST(20000 AS Decimal(18, 0)), CAST(1 AS Decimal(18, 0)), CAST(100 AS Decimal(18, 0)), CAST(0.500 AS Decimal(18, 3)), CAST(N'2018-04-30 00:00:00.000' AS DateTime), N'')
INSERT [dbo].[Role] ([Id], [Name]) VALUES (N'62d7488b-5e91-4d6f-b0ec-872176eeb25c', N'Admin')
INSERT [dbo].[SaleStation] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Name], [OrganizationId]) VALUES (N'304af16b-60d1-4ed6-a4a4-3b3f6662d97b', 1, CAST(N'2018-01-01 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-01 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Lahore', N'eadfde17-3b0b-4d2f-b266-2adef1ff157d')
INSERT [dbo].[SaleStation] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Name], [OrganizationId]) VALUES (N'9ef74e38-8e39-40de-a727-d56f745a6042', 1, CAST(N'2018-01-01 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-01 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Karachi', N'eadfde17-3b0b-4d2f-b266-2adef1ff157d')
INSERT [dbo].[SaleStation] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Name], [OrganizationId]) VALUES (N'2e92b454-74a2-4dbd-90e5-f585d5d47e80', 1, CAST(N'2018-01-01 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-01 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Islamabad', N'eadfde17-3b0b-4d2f-b266-2adef1ff157d')
SET IDENTITY_INSERT [dbo].[Seiving] ON 

INSERT [dbo].[Seiving] ([ID], [SeivingNo], [Origin], [Date], [StoreId], [VesselId], [CustomerId], [FromSize], [FromQuantity], [Remarks]) VALUES (1, N'SV-123', NULL, CAST(N'2012-12-12 00:00:00.000' AS DateTime), N'1', 1, N'4994d1c6-ad3c-4be4-932e-08f992f54531', N'12', CAST(1999 AS Decimal(18, 0)), N'Test 130 123')
INSERT [dbo].[Seiving] ([ID], [SeivingNo], [Origin], [Date], [StoreId], [VesselId], [CustomerId], [FromSize], [FromQuantity], [Remarks]) VALUES (2, N'SV-101', NULL, CAST(N'2012-12-12 00:00:00.000' AS DateTime), N'1', 1, N'4994d1c6-ad3c-4be4-932e-08f992f54531', N'0-50', CAST(10000 AS Decimal(18, 0)), N'Hello Man')
INSERT [dbo].[Seiving] ([ID], [SeivingNo], [Origin], [Date], [StoreId], [VesselId], [CustomerId], [FromSize], [FromQuantity], [Remarks]) VALUES (3, N'SV-102', 3, CAST(N'2012-12-12 00:00:00.000' AS DateTime), N'8b13ea2a-67e4-487f-8f02-11c9be1a1983', 1, N'4994d1c6-ad3c-4be4-932e-08f992f54531', N'1', CAST(1999 AS Decimal(18, 0)), N'Test 130')
INSERT [dbo].[Seiving] ([ID], [SeivingNo], [Origin], [Date], [StoreId], [VesselId], [CustomerId], [FromSize], [FromQuantity], [Remarks]) VALUES (4, N'SV-1002', 2, CAST(N'2012-12-12 00:00:00.000' AS DateTime), N'b37a4521-51bd-4e13-a317-2ff51f8c6280', 2, N'027b5ce4-03ab-46a1-b708-092aaeb0b9fa', N'3', CAST(1999 AS Decimal(18, 0)), N'Test 130')
INSERT [dbo].[Seiving] ([ID], [SeivingNo], [Origin], [Date], [StoreId], [VesselId], [CustomerId], [FromSize], [FromQuantity], [Remarks]) VALUES (5, N'SV-1003', 2, CAST(N'2012-12-12 00:00:00.000' AS DateTime), N'b37a4521-51bd-4e13-a317-2ff51f8c6280', 2, N'24cbac57-cc6c-41b8-b5f8-0b561ddb2fb8', N'3', CAST(1999 AS Decimal(18, 0)), N'Test 130')
INSERT [dbo].[Seiving] ([ID], [SeivingNo], [Origin], [Date], [StoreId], [VesselId], [CustomerId], [FromSize], [FromQuantity], [Remarks]) VALUES (6, N'SV-1004', 2, CAST(N'2012-12-12 00:00:00.000' AS DateTime), N'43961369-a7e4-4fcc-8756-4359cb20aae5', 3, N'4994d1c6-ad3c-4be4-932e-08f992f54531', N'5', CAST(1999 AS Decimal(18, 0)), N'Testing')
SET IDENTITY_INSERT [dbo].[Seiving] OFF
SET IDENTITY_INSERT [dbo].[SeivingSizeQty] ON 

INSERT [dbo].[SeivingSizeQty] ([ID], [SeivingID], [SizeId], [SizeQuantity]) VALUES (1, 1, 1, CAST(100 AS Decimal(18, 0)))
INSERT [dbo].[SeivingSizeQty] ([ID], [SeivingID], [SizeId], [SizeQuantity]) VALUES (2, 1, 2, CAST(30 AS Decimal(18, 0)))
INSERT [dbo].[SeivingSizeQty] ([ID], [SeivingID], [SizeId], [SizeQuantity]) VALUES (3, 1, 3, CAST(3 AS Decimal(18, 0)))
INSERT [dbo].[SeivingSizeQty] ([ID], [SeivingID], [SizeId], [SizeQuantity]) VALUES (4, 1, 4, CAST(40 AS Decimal(18, 0)))
INSERT [dbo].[SeivingSizeQty] ([ID], [SeivingID], [SizeId], [SizeQuantity]) VALUES (5, 1, 5, CAST(500 AS Decimal(18, 0)))
INSERT [dbo].[SeivingSizeQty] ([ID], [SeivingID], [SizeId], [SizeQuantity]) VALUES (6, 1, 6, CAST(60 AS Decimal(18, 0)))
INSERT [dbo].[SeivingSizeQty] ([ID], [SeivingID], [SizeId], [SizeQuantity]) VALUES (7, 1, 7, CAST(700 AS Decimal(18, 0)))
INSERT [dbo].[SeivingSizeQty] ([ID], [SeivingID], [SizeId], [SizeQuantity]) VALUES (10002, 2, 1, CAST(100 AS Decimal(18, 0)))
INSERT [dbo].[SeivingSizeQty] ([ID], [SeivingID], [SizeId], [SizeQuantity]) VALUES (10003, 2, 2, CAST(30 AS Decimal(18, 0)))
INSERT [dbo].[SeivingSizeQty] ([ID], [SeivingID], [SizeId], [SizeQuantity]) VALUES (10004, 2, 3, CAST(3 AS Decimal(18, 0)))
INSERT [dbo].[SeivingSizeQty] ([ID], [SeivingID], [SizeId], [SizeQuantity]) VALUES (10005, 2, 4, CAST(40 AS Decimal(18, 0)))
INSERT [dbo].[SeivingSizeQty] ([ID], [SeivingID], [SizeId], [SizeQuantity]) VALUES (10006, 2, 5, CAST(500 AS Decimal(18, 0)))
INSERT [dbo].[SeivingSizeQty] ([ID], [SeivingID], [SizeId], [SizeQuantity]) VALUES (10007, 2, 6, CAST(60 AS Decimal(18, 0)))
INSERT [dbo].[SeivingSizeQty] ([ID], [SeivingID], [SizeId], [SizeQuantity]) VALUES (10008, 2, 7, CAST(700 AS Decimal(18, 0)))
INSERT [dbo].[SeivingSizeQty] ([ID], [SeivingID], [SizeId], [SizeQuantity]) VALUES (10009, 3, 1, CAST(100 AS Decimal(18, 0)))
INSERT [dbo].[SeivingSizeQty] ([ID], [SeivingID], [SizeId], [SizeQuantity]) VALUES (10010, 3, 2, CAST(30 AS Decimal(18, 0)))
INSERT [dbo].[SeivingSizeQty] ([ID], [SeivingID], [SizeId], [SizeQuantity]) VALUES (10011, 3, 3, CAST(300 AS Decimal(18, 0)))
INSERT [dbo].[SeivingSizeQty] ([ID], [SeivingID], [SizeId], [SizeQuantity]) VALUES (10012, 3, 4, CAST(40 AS Decimal(18, 0)))
INSERT [dbo].[SeivingSizeQty] ([ID], [SeivingID], [SizeId], [SizeQuantity]) VALUES (10013, 3, 5, CAST(500 AS Decimal(18, 0)))
INSERT [dbo].[SeivingSizeQty] ([ID], [SeivingID], [SizeId], [SizeQuantity]) VALUES (10014, 3, 6, CAST(60 AS Decimal(18, 0)))
INSERT [dbo].[SeivingSizeQty] ([ID], [SeivingID], [SizeId], [SizeQuantity]) VALUES (10015, 3, 7, CAST(700 AS Decimal(18, 0)))
INSERT [dbo].[SeivingSizeQty] ([ID], [SeivingID], [SizeId], [SizeQuantity]) VALUES (10016, 4, 1, CAST(100 AS Decimal(18, 0)))
INSERT [dbo].[SeivingSizeQty] ([ID], [SeivingID], [SizeId], [SizeQuantity]) VALUES (10017, 4, 2, CAST(30 AS Decimal(18, 0)))
INSERT [dbo].[SeivingSizeQty] ([ID], [SeivingID], [SizeId], [SizeQuantity]) VALUES (10018, 4, 3, CAST(3 AS Decimal(18, 0)))
INSERT [dbo].[SeivingSizeQty] ([ID], [SeivingID], [SizeId], [SizeQuantity]) VALUES (10019, 4, 4, CAST(40 AS Decimal(18, 0)))
INSERT [dbo].[SeivingSizeQty] ([ID], [SeivingID], [SizeId], [SizeQuantity]) VALUES (10020, 4, 5, CAST(500 AS Decimal(18, 0)))
INSERT [dbo].[SeivingSizeQty] ([ID], [SeivingID], [SizeId], [SizeQuantity]) VALUES (10021, 4, 6, CAST(60 AS Decimal(18, 0)))
INSERT [dbo].[SeivingSizeQty] ([ID], [SeivingID], [SizeId], [SizeQuantity]) VALUES (10022, 4, 7, CAST(700 AS Decimal(18, 0)))
INSERT [dbo].[SeivingSizeQty] ([ID], [SeivingID], [SizeId], [SizeQuantity]) VALUES (10023, 5, 1, CAST(100 AS Decimal(18, 0)))
INSERT [dbo].[SeivingSizeQty] ([ID], [SeivingID], [SizeId], [SizeQuantity]) VALUES (10024, 5, 2, CAST(30 AS Decimal(18, 0)))
INSERT [dbo].[SeivingSizeQty] ([ID], [SeivingID], [SizeId], [SizeQuantity]) VALUES (10025, 5, 3, CAST(3 AS Decimal(18, 0)))
INSERT [dbo].[SeivingSizeQty] ([ID], [SeivingID], [SizeId], [SizeQuantity]) VALUES (10026, 5, 4, CAST(40 AS Decimal(18, 0)))
INSERT [dbo].[SeivingSizeQty] ([ID], [SeivingID], [SizeId], [SizeQuantity]) VALUES (10027, 5, 5, CAST(500 AS Decimal(18, 0)))
INSERT [dbo].[SeivingSizeQty] ([ID], [SeivingID], [SizeId], [SizeQuantity]) VALUES (10028, 5, 6, CAST(70 AS Decimal(18, 0)))
INSERT [dbo].[SeivingSizeQty] ([ID], [SeivingID], [SizeId], [SizeQuantity]) VALUES (10029, 5, 7, CAST(700 AS Decimal(18, 0)))
INSERT [dbo].[SeivingSizeQty] ([ID], [SeivingID], [SizeId], [SizeQuantity]) VALUES (10030, 6, 1, CAST(100 AS Decimal(18, 0)))
INSERT [dbo].[SeivingSizeQty] ([ID], [SeivingID], [SizeId], [SizeQuantity]) VALUES (10031, 6, 2, CAST(30 AS Decimal(18, 0)))
INSERT [dbo].[SeivingSizeQty] ([ID], [SeivingID], [SizeId], [SizeQuantity]) VALUES (10032, 6, 3, CAST(3 AS Decimal(18, 0)))
INSERT [dbo].[SeivingSizeQty] ([ID], [SeivingID], [SizeId], [SizeQuantity]) VALUES (10033, 6, 4, CAST(40 AS Decimal(18, 0)))
INSERT [dbo].[SeivingSizeQty] ([ID], [SeivingID], [SizeId], [SizeQuantity]) VALUES (10034, 6, 5, CAST(500 AS Decimal(18, 0)))
INSERT [dbo].[SeivingSizeQty] ([ID], [SeivingID], [SizeId], [SizeQuantity]) VALUES (10035, 6, 6, CAST(70 AS Decimal(18, 0)))
SET IDENTITY_INSERT [dbo].[SeivingSizeQty] OFF
SET IDENTITY_INSERT [dbo].[Size] ON 

INSERT [dbo].[Size] ([Id], [Name], [Description], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (1, N'00-10', N'This is for test purpose', 1, CAST(N'2018-02-15 15:25:22.570' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-02-15 15:25:22.570' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e')
INSERT [dbo].[Size] ([Id], [Name], [Description], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (2, N'00-04', N'This is for test purpose', 1, CAST(N'2018-02-15 15:25:22.570' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-02-15 15:25:22.570' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e')
INSERT [dbo].[Size] ([Id], [Name], [Description], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (3, N'10-25', N'This is for test purpose', 1, CAST(N'2018-02-15 15:25:22.570' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-02-15 15:25:22.570' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e')
INSERT [dbo].[Size] ([Id], [Name], [Description], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (4, N'25-50', N'This is for test purpose', 1, CAST(N'2018-02-15 15:25:22.570' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-02-15 15:25:22.570' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e')
INSERT [dbo].[Size] ([Id], [Name], [Description], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (5, N'05-10', N'This is for test purpose', 1, CAST(N'2018-02-15 15:25:22.570' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-02-15 15:25:22.570' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e')
INSERT [dbo].[Size] ([Id], [Name], [Description], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (6, N'00-50', N'This is for test purpose', 1, CAST(N'2018-02-15 15:25:22.570' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-02-15 15:25:22.570' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e')
INSERT [dbo].[Size] ([Id], [Name], [Description], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (7, N'00-00', N'This is for test purpose', 1, CAST(N'2018-02-15 15:25:22.570' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-02-15 15:25:22.570' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e')
SET IDENTITY_INSERT [dbo].[Size] OFF
SET IDENTITY_INSERT [dbo].[SO] ON 

INSERT [dbo].[SO] ([Id], [LeadId], [OriginId], [SizeId], [VesselId], [CustomerId], [TaxRateId], [TraderId], [Status], [OrderType], [SONumber], [SODate], [SOExpiryDate], [PartyPONumber], [PartyPODate], [PartyPOExpiryDate], [CreditPeriod], [Quantity], [AgreedRate], [TraderCommision], [CompletedOn], [ApprovedDate], [ApprovedBy], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [POScannedImage], [Remarks], [BufferQuantityMax], [BufferQuantityMin]) VALUES (6, N'1ee1700d-1ca9-4c56-983a-0285f581379e', 2, 1, 1, N'bc746f17-04c9-4b1f-ac9a-eb599032f4d5', 2, 2, 4, 1, N'SO-1002', CAST(N'2018-04-04 00:00:00.000' AS DateTime), CAST(N'2018-04-04 00:00:00.000' AS DateTime), N'sdf23', CAST(N'2018-04-04 00:00:00.000' AS DateTime), CAST(N'2018-04-04 00:00:00.000' AS DateTime), 50, CAST(20000 AS Decimal(18, 0)), CAST(50 AS Decimal(18, 0)), CAST(5000 AS Decimal(18, 0)), NULL, CAST(N'2018-04-04 15:51:55.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-04-04 12:55:59.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-04-04 15:51:58.987' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'', N'updated by zeeshan', NULL, NULL)
INSERT [dbo].[SO] ([Id], [LeadId], [OriginId], [SizeId], [VesselId], [CustomerId], [TaxRateId], [TraderId], [Status], [OrderType], [SONumber], [SODate], [SOExpiryDate], [PartyPONumber], [PartyPODate], [PartyPOExpiryDate], [CreditPeriod], [Quantity], [AgreedRate], [TraderCommision], [CompletedOn], [ApprovedDate], [ApprovedBy], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [POScannedImage], [Remarks], [BufferQuantityMax], [BufferQuantityMin]) VALUES (7, N'1ee1700d-1ca9-4c56-983a-0285f581379e', 3, 3, 4, N'4994d1c6-ad3c-4be4-932e-08f992f54531', 1, 1, 2, 1, N'SO-1003', CAST(N'2018-04-04 00:00:00.000' AS DateTime), CAST(N'2018-04-04 00:00:00.000' AS DateTime), N'55', CAST(N'2018-04-04 00:00:00.000' AS DateTime), CAST(N'2018-04-04 00:00:00.000' AS DateTime), 45, CAST(500 AS Decimal(18, 0)), CAST(5600 AS Decimal(18, 0)), CAST(400 AS Decimal(18, 0)), NULL, CAST(N'2018-04-04 15:50:58.063' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-04-04 14:42:19.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-04-04 15:50:58.063' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'', N'sdfg', NULL, NULL)
INSERT [dbo].[SO] ([Id], [LeadId], [OriginId], [SizeId], [VesselId], [CustomerId], [TaxRateId], [TraderId], [Status], [OrderType], [SONumber], [SODate], [SOExpiryDate], [PartyPONumber], [PartyPODate], [PartyPOExpiryDate], [CreditPeriod], [Quantity], [AgreedRate], [TraderCommision], [CompletedOn], [ApprovedDate], [ApprovedBy], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [POScannedImage], [Remarks], [BufferQuantityMax], [BufferQuantityMin]) VALUES (9, N'1ee1700d-1ca9-4c56-983a-0285f581379e', 3, 2, 3, N'4994d1c6-ad3c-4be4-932e-08f992f54531', 0, 0, 3, 2, N'SO-1004', CAST(N'2018-10-10 00:00:00.000' AS DateTime), CAST(N'2018-10-10 00:00:00.000' AS DateTime), N'777', CAST(N'2018-10-10 00:00:00.000' AS DateTime), CAST(N'2018-10-10 00:00:00.000' AS DateTime), 777, CAST(777 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), NULL, CAST(N'2018-10-25 00:24:01.540' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-10-10 22:27:22.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-10-25 00:24:01.540' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'', N'777', NULL, NULL)
INSERT [dbo].[SO] ([Id], [LeadId], [OriginId], [SizeId], [VesselId], [CustomerId], [TaxRateId], [TraderId], [Status], [OrderType], [SONumber], [SODate], [SOExpiryDate], [PartyPONumber], [PartyPODate], [PartyPOExpiryDate], [CreditPeriod], [Quantity], [AgreedRate], [TraderCommision], [CompletedOn], [ApprovedDate], [ApprovedBy], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [POScannedImage], [Remarks], [BufferQuantityMax], [BufferQuantityMin]) VALUES (10, N'1ee1700d-1ca9-4c56-983a-0285f581379e', 3, 3, 2, N'46beaf1b-b849-4360-aa7a-0e26da69842d', 1, 1, 1, 1, N'SO-1005', CAST(N'2018-10-13 00:00:00.000' AS DateTime), CAST(N'2018-10-13 00:00:00.000' AS DateTime), N'234', CAST(N'2018-10-13 00:00:00.000' AS DateTime), CAST(N'2018-10-13 00:00:00.000' AS DateTime), 3, CAST(1000 AS Decimal(18, 0)), CAST(45 AS Decimal(18, 0)), CAST(3 AS Decimal(18, 0)), NULL, NULL, NULL, CAST(N'2018-10-13 01:48:56.267' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-10-13 01:48:56.267' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'84-GET-YOUR-ASSIGNMENTS-DONE-BY-EXPERTS-AT-ACADEMI', N'Test 130', NULL, NULL)
INSERT [dbo].[SO] ([Id], [LeadId], [OriginId], [SizeId], [VesselId], [CustomerId], [TaxRateId], [TraderId], [Status], [OrderType], [SONumber], [SODate], [SOExpiryDate], [PartyPONumber], [PartyPODate], [PartyPOExpiryDate], [CreditPeriod], [Quantity], [AgreedRate], [TraderCommision], [CompletedOn], [ApprovedDate], [ApprovedBy], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [POScannedImage], [Remarks], [BufferQuantityMax], [BufferQuantityMin]) VALUES (11, N'1ee1700d-1ca9-4c56-983a-0285f581379e', 2, 3, 2, N'46beaf1b-b849-4360-aa7a-0e26da69842d', 1, 1, 1, 1, N'SO-1005', CAST(N'2018-10-13 00:00:00.000' AS DateTime), CAST(N'2018-10-13 00:00:00.000' AS DateTime), N'234', CAST(N'2018-10-13 00:00:00.000' AS DateTime), CAST(N'2018-10-13 00:00:00.000' AS DateTime), 3, CAST(5009 AS Decimal(18, 0)), CAST(45 AS Decimal(18, 0)), CAST(3 AS Decimal(18, 0)), NULL, NULL, NULL, CAST(N'2018-10-13 01:53:27.373' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-10-13 01:53:27.373' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'84-GET-YOUR-ASSIGNMENTS-DONE-BY-EXPERTS-AT-ACADEMI', N'123', NULL, NULL)
INSERT [dbo].[SO] ([Id], [LeadId], [OriginId], [SizeId], [VesselId], [CustomerId], [TaxRateId], [TraderId], [Status], [OrderType], [SONumber], [SODate], [SOExpiryDate], [PartyPONumber], [PartyPODate], [PartyPOExpiryDate], [CreditPeriod], [Quantity], [AgreedRate], [TraderCommision], [CompletedOn], [ApprovedDate], [ApprovedBy], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [POScannedImage], [Remarks], [BufferQuantityMax], [BufferQuantityMin]) VALUES (12, N'1ee1700d-1ca9-4c56-983a-0285f581379e', 2, 1, 1, N'a5c7d30f-b49a-4626-a9dc-0ea2e1b1dca1', 2, 2, 1, 1, N'SO-1006', CAST(N'2018-10-13 00:00:00.000' AS DateTime), CAST(N'2018-10-13 00:00:00.000' AS DateTime), N'234', CAST(N'2018-10-13 00:00:00.000' AS DateTime), CAST(N'2018-10-13 00:00:00.000' AS DateTime), 3, CAST(1333 AS Decimal(18, 0)), CAST(45 AS Decimal(18, 0)), CAST(3 AS Decimal(18, 0)), NULL, NULL, NULL, CAST(N'2018-10-13 02:01:24.817' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-10-13 02:01:24.817' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'84-GET-YOUR-ASSIGNMENTS-DONE-BY-EXPERTS-AT-ACADEMI', N'Test 130', NULL, NULL)
INSERT [dbo].[SO] ([Id], [LeadId], [OriginId], [SizeId], [VesselId], [CustomerId], [TaxRateId], [TraderId], [Status], [OrderType], [SONumber], [SODate], [SOExpiryDate], [PartyPONumber], [PartyPODate], [PartyPOExpiryDate], [CreditPeriod], [Quantity], [AgreedRate], [TraderCommision], [CompletedOn], [ApprovedDate], [ApprovedBy], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [POScannedImage], [Remarks], [BufferQuantityMax], [BufferQuantityMin]) VALUES (13, N'1ee1700d-1ca9-4c56-983a-0285f581379e', 3, 3, 2, N'46beaf1b-b849-4360-aa7a-0e26da69842d', 0, 0, 1, 2, N'SO-1006', CAST(N'2018-10-13 00:00:00.000' AS DateTime), CAST(N'2018-10-13 00:00:00.000' AS DateTime), N'234', CAST(N'2018-10-13 00:00:00.000' AS DateTime), CAST(N'2018-10-13 00:00:00.000' AS DateTime), 3, CAST(5009 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), NULL, NULL, NULL, CAST(N'2018-10-13 02:04:07.277' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-10-13 02:04:07.277' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'f13bdee5-e832-4a31-b33c-abf9f0b9976e-original.jpeg', N'122222222', NULL, NULL)
INSERT [dbo].[SO] ([Id], [LeadId], [OriginId], [SizeId], [VesselId], [CustomerId], [TaxRateId], [TraderId], [Status], [OrderType], [SONumber], [SODate], [SOExpiryDate], [PartyPONumber], [PartyPODate], [PartyPOExpiryDate], [CreditPeriod], [Quantity], [AgreedRate], [TraderCommision], [CompletedOn], [ApprovedDate], [ApprovedBy], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [POScannedImage], [Remarks], [BufferQuantityMax], [BufferQuantityMin]) VALUES (14, N'1ee1700d-1ca9-4c56-983a-0285f581379e', 3, 4, 2, N'a5c7d30f-b49a-4626-a9dc-0ea2e1b1dca1', 2, 1, 1, 3, N'SO-1006', CAST(N'2018-10-13 00:00:00.000' AS DateTime), CAST(N'2018-10-13 00:00:00.000' AS DateTime), N'234', CAST(N'2018-10-13 00:00:00.000' AS DateTime), CAST(N'2018-10-13 00:00:00.000' AS DateTime), 3, CAST(1000 AS Decimal(18, 0)), CAST(45 AS Decimal(18, 0)), CAST(3 AS Decimal(18, 0)), NULL, NULL, NULL, CAST(N'2018-10-13 02:08:28.167' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-10-13 02:08:28.963' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'28055888_429302487487993_8851743078609873974_n.jpg', N'Test 1307777', NULL, NULL)
INSERT [dbo].[SO] ([Id], [LeadId], [OriginId], [SizeId], [VesselId], [CustomerId], [TaxRateId], [TraderId], [Status], [OrderType], [SONumber], [SODate], [SOExpiryDate], [PartyPONumber], [PartyPODate], [PartyPOExpiryDate], [CreditPeriod], [Quantity], [AgreedRate], [TraderCommision], [CompletedOn], [ApprovedDate], [ApprovedBy], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [POScannedImage], [Remarks], [BufferQuantityMax], [BufferQuantityMin]) VALUES (15, N'1ee1700d-1ca9-4c56-983a-0285f581379e', 5, 4, 1, N'decf5498-93f5-431d-96a1-145c74d8a8bd', 2, 2, 1, 3, N'SO-1006', CAST(N'2018-10-13 00:00:00.000' AS DateTime), CAST(N'2018-10-13 00:00:00.000' AS DateTime), N'234', CAST(N'2018-10-13 00:00:00.000' AS DateTime), CAST(N'2018-10-13 00:00:00.000' AS DateTime), 3, CAST(1000 AS Decimal(18, 0)), CAST(45 AS Decimal(18, 0)), CAST(3 AS Decimal(18, 0)), NULL, NULL, NULL, CAST(N'2018-10-13 02:13:01.697' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-10-13 02:13:02.567' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'84-GET-YOUR-ASSIGNMENTS-DONE-BY-EXPERTS-AT-ACADEMI', N'Test 130', NULL, NULL)
INSERT [dbo].[SO] ([Id], [LeadId], [OriginId], [SizeId], [VesselId], [CustomerId], [TaxRateId], [TraderId], [Status], [OrderType], [SONumber], [SODate], [SOExpiryDate], [PartyPONumber], [PartyPODate], [PartyPOExpiryDate], [CreditPeriod], [Quantity], [AgreedRate], [TraderCommision], [CompletedOn], [ApprovedDate], [ApprovedBy], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [POScannedImage], [Remarks], [BufferQuantityMax], [BufferQuantityMin]) VALUES (16, N'1ee1700d-1ca9-4c56-983a-0285f581379e', 2, 1, 1, N'24cbac57-cc6c-41b8-b5f8-0b561ddb2fb8', 1, 1, 1, 1, N'SO-1007', CAST(N'2018-10-22 00:00:00.000' AS DateTime), CAST(N'2018-10-22 00:00:00.000' AS DateTime), N'234', CAST(N'2018-10-22 00:00:00.000' AS DateTime), CAST(N'2018-10-22 00:00:00.000' AS DateTime), 37, CAST(100000 AS Decimal(18, 0)), CAST(45 AS Decimal(18, 0)), CAST(3 AS Decimal(18, 0)), NULL, NULL, NULL, CAST(N'2018-10-22 19:55:30.120' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-10-22 19:55:30.120' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'84-GET-YOUR-ASSIGNMENTS-DONE-BY-EXPERTS-AT-ACADEMI', N'Test', NULL, NULL)
INSERT [dbo].[SO] ([Id], [LeadId], [OriginId], [SizeId], [VesselId], [CustomerId], [TaxRateId], [TraderId], [Status], [OrderType], [SONumber], [SODate], [SOExpiryDate], [PartyPONumber], [PartyPODate], [PartyPOExpiryDate], [CreditPeriod], [Quantity], [AgreedRate], [TraderCommision], [CompletedOn], [ApprovedDate], [ApprovedBy], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [POScannedImage], [Remarks], [BufferQuantityMax], [BufferQuantityMin]) VALUES (17, N'1ee1700d-1ca9-4c56-983a-0285f581379e', 3, 1, 3, N'4994d1c6-ad3c-4be4-932e-08f992f54531', 1, 1, 2, 1, N'SO-1008', CAST(N'2018-10-22 00:00:00.000' AS DateTime), CAST(N'2018-10-22 00:00:00.000' AS DateTime), N'2348787', CAST(N'2018-10-22 00:00:00.000' AS DateTime), CAST(N'2018-10-22 00:00:00.000' AS DateTime), 37, CAST(100000 AS Decimal(18, 0)), CAST(45 AS Decimal(18, 0)), CAST(3 AS Decimal(18, 0)), NULL, NULL, NULL, CAST(N'2018-10-22 19:57:18.050' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-10-22 19:57:18.050' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'84-GET-YOUR-ASSIGNMENTS-DONE-BY-EXPERTS-AT-ACADEMI', N'Test', NULL, NULL)
INSERT [dbo].[SO] ([Id], [LeadId], [OriginId], [SizeId], [VesselId], [CustomerId], [TaxRateId], [TraderId], [Status], [OrderType], [SONumber], [SODate], [SOExpiryDate], [PartyPONumber], [PartyPODate], [PartyPOExpiryDate], [CreditPeriod], [Quantity], [AgreedRate], [TraderCommision], [CompletedOn], [ApprovedDate], [ApprovedBy], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [POScannedImage], [Remarks], [BufferQuantityMax], [BufferQuantityMin]) VALUES (18, N'1ee1700d-1ca9-4c56-983a-0285f581379e', 4, 4, 1, N'4994d1c6-ad3c-4be4-932e-08f992f54531', 0, 0, 1, 2, N'SO-1009', CAST(N'2018-10-23 00:00:00.000' AS DateTime), CAST(N'2018-10-23 00:00:00.000' AS DateTime), N'234', CAST(N'2018-10-23 00:00:00.000' AS DateTime), CAST(N'2018-10-23 00:00:00.000' AS DateTime), 300, CAST(500 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), NULL, NULL, NULL, CAST(N'2018-10-23 23:25:07.087' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-10-23 23:25:07.087' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'', N'Testing', NULL, NULL)
INSERT [dbo].[SO] ([Id], [LeadId], [OriginId], [SizeId], [VesselId], [CustomerId], [TaxRateId], [TraderId], [Status], [OrderType], [SONumber], [SODate], [SOExpiryDate], [PartyPONumber], [PartyPODate], [PartyPOExpiryDate], [CreditPeriod], [Quantity], [AgreedRate], [TraderCommision], [CompletedOn], [ApprovedDate], [ApprovedBy], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [POScannedImage], [Remarks], [BufferQuantityMax], [BufferQuantityMin]) VALUES (19, N'1ee1700d-1ca9-4c56-983a-0285f581379e', 4, 3, 2, N'4994d1c6-ad3c-4be4-932e-08f992f54531', 0, 0, 1, 2, N'SO-1010', CAST(N'2018-10-23 00:00:00.000' AS DateTime), CAST(N'2018-10-23 00:00:00.000' AS DateTime), N'234', CAST(N'2018-10-23 00:00:00.000' AS DateTime), CAST(N'2018-10-23 00:00:00.000' AS DateTime), 300, CAST(1000 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), NULL, NULL, NULL, CAST(N'2018-10-23 23:29:14.707' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-10-23 23:29:14.810' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'', N'Testing', NULL, NULL)
INSERT [dbo].[SO] ([Id], [LeadId], [OriginId], [SizeId], [VesselId], [CustomerId], [TaxRateId], [TraderId], [Status], [OrderType], [SONumber], [SODate], [SOExpiryDate], [PartyPONumber], [PartyPODate], [PartyPOExpiryDate], [CreditPeriod], [Quantity], [AgreedRate], [TraderCommision], [CompletedOn], [ApprovedDate], [ApprovedBy], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [POScannedImage], [Remarks], [BufferQuantityMax], [BufferQuantityMin]) VALUES (20, N'1ee1700d-1ca9-4c56-983a-0285f581379e', 2, 3, 1, N'4994d1c6-ad3c-4be4-932e-08f992f54531', 0, 0, 1, 2, N'SO-1011', CAST(N'2018-10-24 00:00:00.000' AS DateTime), CAST(N'2018-10-24 00:00:00.000' AS DateTime), N'234', CAST(N'2018-10-24 00:00:00.000' AS DateTime), CAST(N'2018-10-24 00:00:00.000' AS DateTime), 37000, CAST(100000 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), NULL, NULL, NULL, CAST(N'2018-10-24 20:08:54.360' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-10-24 20:08:54.360' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'', N'Testing', NULL, NULL)
INSERT [dbo].[SO] ([Id], [LeadId], [OriginId], [SizeId], [VesselId], [CustomerId], [TaxRateId], [TraderId], [Status], [OrderType], [SONumber], [SODate], [SOExpiryDate], [PartyPONumber], [PartyPODate], [PartyPOExpiryDate], [CreditPeriod], [Quantity], [AgreedRate], [TraderCommision], [CompletedOn], [ApprovedDate], [ApprovedBy], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [POScannedImage], [Remarks], [BufferQuantityMax], [BufferQuantityMin]) VALUES (21, N'1ee1700d-1ca9-4c56-983a-0285f581379e', 3, 2, 1, N'4994d1c6-ad3c-4be4-932e-08f992f54531', 0, 0, 1, 2, N'SO-1011', CAST(N'2018-10-24 00:00:00.000' AS DateTime), CAST(N'2018-10-24 00:00:00.000' AS DateTime), N'234', CAST(N'2018-10-24 00:00:00.000' AS DateTime), CAST(N'2018-10-24 00:00:00.000' AS DateTime), 30, CAST(10000 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), NULL, NULL, NULL, CAST(N'2018-10-24 20:10:30.833' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-10-24 20:10:30.833' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'', N'Test 130', NULL, NULL)
INSERT [dbo].[SO] ([Id], [LeadId], [OriginId], [SizeId], [VesselId], [CustomerId], [TaxRateId], [TraderId], [Status], [OrderType], [SONumber], [SODate], [SOExpiryDate], [PartyPONumber], [PartyPODate], [PartyPOExpiryDate], [CreditPeriod], [Quantity], [AgreedRate], [TraderCommision], [CompletedOn], [ApprovedDate], [ApprovedBy], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [POScannedImage], [Remarks], [BufferQuantityMax], [BufferQuantityMin]) VALUES (22, N'1ee1700d-1ca9-4c56-983a-0285f581379e', 2, 1, 1, N'4994d1c6-ad3c-4be4-932e-08f992f54531', 0, 0, 1, 2, N'SO-1012', CAST(N'2018-10-24 00:00:00.000' AS DateTime), CAST(N'2018-10-24 00:00:00.000' AS DateTime), N'234', CAST(N'2018-10-24 00:00:00.000' AS DateTime), CAST(N'2018-10-24 00:00:00.000' AS DateTime), 37, CAST(10000 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), NULL, NULL, NULL, CAST(N'2018-10-24 20:18:12.943' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-10-24 20:18:12.943' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'', N'Testing', NULL, NULL)
INSERT [dbo].[SO] ([Id], [LeadId], [OriginId], [SizeId], [VesselId], [CustomerId], [TaxRateId], [TraderId], [Status], [OrderType], [SONumber], [SODate], [SOExpiryDate], [PartyPONumber], [PartyPODate], [PartyPOExpiryDate], [CreditPeriod], [Quantity], [AgreedRate], [TraderCommision], [CompletedOn], [ApprovedDate], [ApprovedBy], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [POScannedImage], [Remarks], [BufferQuantityMax], [BufferQuantityMin]) VALUES (23, N'1ee1700d-1ca9-4c56-983a-0285f581379e', 3, 3, 1, N'4994d1c6-ad3c-4be4-932e-08f992f54531', 0, 0, 1, 2, N'SO-1013', CAST(N'2018-10-24 00:00:00.000' AS DateTime), CAST(N'2018-10-24 00:00:00.000' AS DateTime), N'2348787', CAST(N'2018-10-24 00:00:00.000' AS DateTime), CAST(N'2018-10-24 00:00:00.000' AS DateTime), 30, CAST(10000 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), NULL, NULL, NULL, CAST(N'2018-10-24 20:23:27.473' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-10-24 20:23:27.473' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'84-GET-YOUR-ASSIGNMENTS-DONE-BY-EXPERTS-AT-ACADEMI', N'Testing', NULL, NULL)
INSERT [dbo].[SO] ([Id], [LeadId], [OriginId], [SizeId], [VesselId], [CustomerId], [TaxRateId], [TraderId], [Status], [OrderType], [SONumber], [SODate], [SOExpiryDate], [PartyPONumber], [PartyPODate], [PartyPOExpiryDate], [CreditPeriod], [Quantity], [AgreedRate], [TraderCommision], [CompletedOn], [ApprovedDate], [ApprovedBy], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [POScannedImage], [Remarks], [BufferQuantityMax], [BufferQuantityMin]) VALUES (24, N'1ee1700d-1ca9-4c56-983a-0285f581379e', 2, 1, 1, N'4994d1c6-ad3c-4be4-932e-08f992f54531', 0, 0, 1, 2, N'SO-1013', CAST(N'2018-10-24 00:00:00.000' AS DateTime), CAST(N'2018-10-24 00:00:00.000' AS DateTime), N'234', CAST(N'2018-10-24 00:00:00.000' AS DateTime), CAST(N'2018-10-24 00:00:00.000' AS DateTime), 300, CAST(500 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), NULL, NULL, NULL, CAST(N'2018-10-24 20:24:54.560' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-10-24 20:24:54.560' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'', N'Testing', NULL, NULL)
INSERT [dbo].[SO] ([Id], [LeadId], [OriginId], [SizeId], [VesselId], [CustomerId], [TaxRateId], [TraderId], [Status], [OrderType], [SONumber], [SODate], [SOExpiryDate], [PartyPONumber], [PartyPODate], [PartyPOExpiryDate], [CreditPeriod], [Quantity], [AgreedRate], [TraderCommision], [CompletedOn], [ApprovedDate], [ApprovedBy], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [POScannedImage], [Remarks], [BufferQuantityMax], [BufferQuantityMin]) VALUES (25, N'1ee1700d-1ca9-4c56-983a-0285f581379e', 3, 2, 1, N'4994d1c6-ad3c-4be4-932e-08f992f54531', 0, 0, 1, 2, N'SO-1013', CAST(N'2018-10-24 00:00:00.000' AS DateTime), CAST(N'2018-10-24 00:00:00.000' AS DateTime), N'234', CAST(N'2018-10-24 00:00:00.000' AS DateTime), CAST(N'2018-10-24 00:00:00.000' AS DateTime), 30, CAST(10000 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), NULL, NULL, NULL, CAST(N'2018-10-24 20:25:45.637' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-10-24 20:25:45.637' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'84-GET-YOUR-ASSIGNMENTS-DONE-BY-EXPERTS-AT-ACADEMI', N'Testing', NULL, NULL)
INSERT [dbo].[SO] ([Id], [LeadId], [OriginId], [SizeId], [VesselId], [CustomerId], [TaxRateId], [TraderId], [Status], [OrderType], [SONumber], [SODate], [SOExpiryDate], [PartyPONumber], [PartyPODate], [PartyPOExpiryDate], [CreditPeriod], [Quantity], [AgreedRate], [TraderCommision], [CompletedOn], [ApprovedDate], [ApprovedBy], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [POScannedImage], [Remarks], [BufferQuantityMax], [BufferQuantityMin]) VALUES (26, N'1ee1700d-1ca9-4c56-983a-0285f581379e', 3, 1, 1, N'4994d1c6-ad3c-4be4-932e-08f992f54531', 0, 0, 1, 2, N'SO-1013', CAST(N'2018-10-24 00:00:00.000' AS DateTime), CAST(N'2018-10-24 00:00:00.000' AS DateTime), N'234', CAST(N'2018-10-24 00:00:00.000' AS DateTime), CAST(N'2018-10-24 00:00:00.000' AS DateTime), 30, CAST(10000 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), NULL, NULL, NULL, CAST(N'2018-10-24 20:26:55.120' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-10-24 20:26:55.120' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'84-GET-YOUR-ASSIGNMENTS-DONE-BY-EXPERTS-AT-ACADEMI', N'Test 130', NULL, NULL)
INSERT [dbo].[SO] ([Id], [LeadId], [OriginId], [SizeId], [VesselId], [CustomerId], [TaxRateId], [TraderId], [Status], [OrderType], [SONumber], [SODate], [SOExpiryDate], [PartyPONumber], [PartyPODate], [PartyPOExpiryDate], [CreditPeriod], [Quantity], [AgreedRate], [TraderCommision], [CompletedOn], [ApprovedDate], [ApprovedBy], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [POScannedImage], [Remarks], [BufferQuantityMax], [BufferQuantityMin]) VALUES (27, N'1ee1700d-1ca9-4c56-983a-0285f581379e', 3, 2, 1, N'4994d1c6-ad3c-4be4-932e-08f992f54531', 0, 0, 1, 2, N'SO-1014', CAST(N'2018-10-24 00:00:00.000' AS DateTime), CAST(N'2018-10-24 00:00:00.000' AS DateTime), N'234', CAST(N'2018-10-24 00:00:00.000' AS DateTime), CAST(N'2018-10-24 00:00:00.000' AS DateTime), 37, CAST(1333 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), NULL, NULL, NULL, CAST(N'2018-10-24 20:31:30.383' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-10-24 20:31:30.383' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'84-GET-YOUR-ASSIGNMENTS-DONE-BY-EXPERTS-AT-ACADEMI', N'Testing', NULL, NULL)
INSERT [dbo].[SO] ([Id], [LeadId], [OriginId], [SizeId], [VesselId], [CustomerId], [TaxRateId], [TraderId], [Status], [OrderType], [SONumber], [SODate], [SOExpiryDate], [PartyPONumber], [PartyPODate], [PartyPOExpiryDate], [CreditPeriod], [Quantity], [AgreedRate], [TraderCommision], [CompletedOn], [ApprovedDate], [ApprovedBy], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [POScannedImage], [Remarks], [BufferQuantityMax], [BufferQuantityMin]) VALUES (28, N'1ee1700d-1ca9-4c56-983a-0285f581379e', 4, 2, 1, N'4994d1c6-ad3c-4be4-932e-08f992f54531', 0, 0, 1, 2, N'SO-1015', CAST(N'2018-10-24 00:00:00.000' AS DateTime), CAST(N'2018-10-24 00:00:00.000' AS DateTime), N'234', CAST(N'2018-10-24 00:00:00.000' AS DateTime), CAST(N'2018-10-24 00:00:00.000' AS DateTime), 300, CAST(500 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), NULL, NULL, NULL, CAST(N'2018-10-24 20:34:20.290' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-10-24 20:34:20.290' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'84-GET-YOUR-ASSIGNMENTS-DONE-BY-EXPERTS-AT-ACADEMI', N'Test 130', NULL, NULL)
INSERT [dbo].[SO] ([Id], [LeadId], [OriginId], [SizeId], [VesselId], [CustomerId], [TaxRateId], [TraderId], [Status], [OrderType], [SONumber], [SODate], [SOExpiryDate], [PartyPONumber], [PartyPODate], [PartyPOExpiryDate], [CreditPeriod], [Quantity], [AgreedRate], [TraderCommision], [CompletedOn], [ApprovedDate], [ApprovedBy], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [POScannedImage], [Remarks], [BufferQuantityMax], [BufferQuantityMin]) VALUES (29, N'1ee1700d-1ca9-4c56-983a-0285f581379e', 3, 1, 1, N'4994d1c6-ad3c-4be4-932e-08f992f54531', 0, 0, 1, 2, N'SO-1016', CAST(N'2018-10-24 00:00:00.000' AS DateTime), CAST(N'2018-10-24 00:00:00.000' AS DateTime), N'234', CAST(N'2018-10-24 00:00:00.000' AS DateTime), CAST(N'2018-10-24 00:00:00.000' AS DateTime), 30, CAST(10000 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), NULL, NULL, NULL, CAST(N'2018-10-24 20:37:28.193' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-10-24 20:37:28.193' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'84-GET-YOUR-ASSIGNMENTS-DONE-BY-EXPERTS-AT-ACADEMI', N'Testing', NULL, NULL)
INSERT [dbo].[SO] ([Id], [LeadId], [OriginId], [SizeId], [VesselId], [CustomerId], [TaxRateId], [TraderId], [Status], [OrderType], [SONumber], [SODate], [SOExpiryDate], [PartyPONumber], [PartyPODate], [PartyPOExpiryDate], [CreditPeriod], [Quantity], [AgreedRate], [TraderCommision], [CompletedOn], [ApprovedDate], [ApprovedBy], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [POScannedImage], [Remarks], [BufferQuantityMax], [BufferQuantityMin]) VALUES (30, N'1ee1700d-1ca9-4c56-983a-0285f581379e', 3, 3, 1, N'4994d1c6-ad3c-4be4-932e-08f992f54531', 0, 0, 1, 2, N'SO-1017', CAST(N'2018-10-24 00:00:00.000' AS DateTime), CAST(N'2018-10-24 00:00:00.000' AS DateTime), N'234', CAST(N'2018-10-24 00:00:00.000' AS DateTime), CAST(N'2018-10-24 00:00:00.000' AS DateTime), 37, CAST(100000 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), NULL, NULL, NULL, CAST(N'2018-10-24 20:42:35.617' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-10-24 20:42:35.617' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'84-GET-YOUR-ASSIGNMENTS-DONE-BY-EXPERTS-AT-ACADEMI', N'Test 130', NULL, NULL)
INSERT [dbo].[SO] ([Id], [LeadId], [OriginId], [SizeId], [VesselId], [CustomerId], [TaxRateId], [TraderId], [Status], [OrderType], [SONumber], [SODate], [SOExpiryDate], [PartyPONumber], [PartyPODate], [PartyPOExpiryDate], [CreditPeriod], [Quantity], [AgreedRate], [TraderCommision], [CompletedOn], [ApprovedDate], [ApprovedBy], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [POScannedImage], [Remarks], [BufferQuantityMax], [BufferQuantityMin]) VALUES (31, N'1ee1700d-1ca9-4c56-983a-0285f581379e', 2, 1, 2, N'4994d1c6-ad3c-4be4-932e-08f992f54531', 0, 0, 1, 2, N'SO-1018', CAST(N'2018-10-24 00:00:00.000' AS DateTime), CAST(N'2018-10-24 00:00:00.000' AS DateTime), N'234', CAST(N'2018-10-24 00:00:00.000' AS DateTime), CAST(N'2018-10-24 00:00:00.000' AS DateTime), 37, CAST(10000 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), NULL, NULL, NULL, CAST(N'2018-10-24 20:50:43.433' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-10-24 20:50:43.433' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'84-GET-YOUR-ASSIGNMENTS-DONE-BY-EXPERTS-AT-ACADEMI', N'Testing', NULL, NULL)
INSERT [dbo].[SO] ([Id], [LeadId], [OriginId], [SizeId], [VesselId], [CustomerId], [TaxRateId], [TraderId], [Status], [OrderType], [SONumber], [SODate], [SOExpiryDate], [PartyPONumber], [PartyPODate], [PartyPOExpiryDate], [CreditPeriod], [Quantity], [AgreedRate], [TraderCommision], [CompletedOn], [ApprovedDate], [ApprovedBy], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [POScannedImage], [Remarks], [BufferQuantityMax], [BufferQuantityMin]) VALUES (32, N'1ee1700d-1ca9-4c56-983a-0285f581379e', 3, 3, 1, N'0bef2889-0462-432e-a4d3-085b9821c7a1', 0, 0, 1, 2, N'SO-1019', CAST(N'2018-10-24 00:00:00.000' AS DateTime), CAST(N'2018-10-24 00:00:00.000' AS DateTime), N'234', CAST(N'2018-10-24 00:00:00.000' AS DateTime), CAST(N'2018-10-24 00:00:00.000' AS DateTime), 3, CAST(500 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), NULL, NULL, NULL, CAST(N'2018-10-24 21:11:21.700' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-10-24 21:11:21.700' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'84-GET-YOUR-ASSIGNMENTS-DONE-BY-EXPERTS-AT-ACADEMI', N'Testing', NULL, NULL)
INSERT [dbo].[SO] ([Id], [LeadId], [OriginId], [SizeId], [VesselId], [CustomerId], [TaxRateId], [TraderId], [Status], [OrderType], [SONumber], [SODate], [SOExpiryDate], [PartyPONumber], [PartyPODate], [PartyPOExpiryDate], [CreditPeriod], [Quantity], [AgreedRate], [TraderCommision], [CompletedOn], [ApprovedDate], [ApprovedBy], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [POScannedImage], [Remarks], [BufferQuantityMax], [BufferQuantityMin]) VALUES (33, N'1ee1700d-1ca9-4c56-983a-0285f581379e', 2, 1, 1, N'4994d1c6-ad3c-4be4-932e-08f992f54531', 0, 0, 1, 2, N'SO-1020', CAST(N'2018-10-24 00:00:00.000' AS DateTime), CAST(N'2018-10-24 00:00:00.000' AS DateTime), N'234', CAST(N'2018-10-24 00:00:00.000' AS DateTime), CAST(N'2018-10-24 00:00:00.000' AS DateTime), 3, CAST(500 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), NULL, NULL, NULL, CAST(N'2018-10-24 21:14:03.340' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-10-24 21:14:03.340' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'84-GET-YOUR-ASSIGNMENTS-DONE-BY-EXPERTS-AT-ACADEMI', N'Testing', NULL, NULL)
INSERT [dbo].[SO] ([Id], [LeadId], [OriginId], [SizeId], [VesselId], [CustomerId], [TaxRateId], [TraderId], [Status], [OrderType], [SONumber], [SODate], [SOExpiryDate], [PartyPONumber], [PartyPODate], [PartyPOExpiryDate], [CreditPeriod], [Quantity], [AgreedRate], [TraderCommision], [CompletedOn], [ApprovedDate], [ApprovedBy], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [POScannedImage], [Remarks], [BufferQuantityMax], [BufferQuantityMin]) VALUES (34, N'1ee1700d-1ca9-4c56-983a-0285f581379e', 3, 2, 5, N'4994d1c6-ad3c-4be4-932e-08f992f54531', 0, 0, 3, 2, N'SO-1021', CAST(N'2018-10-24 00:00:00.000' AS DateTime), CAST(N'2018-10-24 00:00:00.000' AS DateTime), N'234', CAST(N'2018-10-24 00:00:00.000' AS DateTime), CAST(N'2018-10-24 00:00:00.000' AS DateTime), 3, CAST(500 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), NULL, CAST(N'2018-10-24 21:43:42.017' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-10-24 21:26:29.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-10-24 21:43:42.017' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'', N'Testing', NULL, NULL)
INSERT [dbo].[SO] ([Id], [LeadId], [OriginId], [SizeId], [VesselId], [CustomerId], [TaxRateId], [TraderId], [Status], [OrderType], [SONumber], [SODate], [SOExpiryDate], [PartyPONumber], [PartyPODate], [PartyPOExpiryDate], [CreditPeriod], [Quantity], [AgreedRate], [TraderCommision], [CompletedOn], [ApprovedDate], [ApprovedBy], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [POScannedImage], [Remarks], [BufferQuantityMax], [BufferQuantityMin]) VALUES (35, N'1ee1700d-1ca9-4c56-983a-0285f581379e', 2, 1, 3, N'4994d1c6-ad3c-4be4-932e-08f992f54531', 0, 0, 3, 2, N'SO-1022', CAST(N'2018-10-24 00:00:00.000' AS DateTime), CAST(N'2018-10-24 00:00:00.000' AS DateTime), N'234', CAST(N'2018-10-24 00:00:00.000' AS DateTime), CAST(N'2018-10-24 00:00:00.000' AS DateTime), 3, CAST(500 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), NULL, CAST(N'2018-10-24 21:41:25.043' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-10-24 21:29:35.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-10-24 21:41:25.043' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'', N'Testing 123', NULL, NULL)
INSERT [dbo].[SO] ([Id], [LeadId], [OriginId], [SizeId], [VesselId], [CustomerId], [TaxRateId], [TraderId], [Status], [OrderType], [SONumber], [SODate], [SOExpiryDate], [PartyPONumber], [PartyPODate], [PartyPOExpiryDate], [CreditPeriod], [Quantity], [AgreedRate], [TraderCommision], [CompletedOn], [ApprovedDate], [ApprovedBy], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [POScannedImage], [Remarks], [BufferQuantityMax], [BufferQuantityMin]) VALUES (36, N'1ee1700d-1ca9-4c56-983a-0285f581379e', 2, 2, 1, N'4994d1c6-ad3c-4be4-932e-08f992f54531', 0, 0, 3, 2, N'SO-1023', CAST(N'2018-10-24 00:00:00.000' AS DateTime), CAST(N'2018-10-24 00:00:00.000' AS DateTime), N'234', CAST(N'2018-10-24 00:00:00.000' AS DateTime), CAST(N'2018-10-24 00:00:00.000' AS DateTime), 3, CAST(500 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), NULL, CAST(N'2018-10-24 21:39:14.183' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-10-24 21:31:55.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-10-24 21:39:14.183' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'', N'Testing', NULL, NULL)
INSERT [dbo].[SO] ([Id], [LeadId], [OriginId], [SizeId], [VesselId], [CustomerId], [TaxRateId], [TraderId], [Status], [OrderType], [SONumber], [SODate], [SOExpiryDate], [PartyPONumber], [PartyPODate], [PartyPOExpiryDate], [CreditPeriod], [Quantity], [AgreedRate], [TraderCommision], [CompletedOn], [ApprovedDate], [ApprovedBy], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [POScannedImage], [Remarks], [BufferQuantityMax], [BufferQuantityMin]) VALUES (37, N'1ee1700d-1ca9-4c56-983a-0285f581379e', 2, 1, 2, N'4994d1c6-ad3c-4be4-932e-08f992f54531', 0, 0, 1, 2, N'SO-1024', CAST(N'2018-10-24 00:00:00.000' AS DateTime), CAST(N'2018-10-24 00:00:00.000' AS DateTime), N'234', CAST(N'2018-10-24 00:00:00.000' AS DateTime), CAST(N'2018-10-24 00:00:00.000' AS DateTime), 3, CAST(500 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), NULL, NULL, NULL, CAST(N'2018-10-24 21:34:49.877' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-10-24 21:34:49.877' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'952c1d38-0212-4df9-b464-48832ca9f169-original.jpeg', N'Testing', NULL, NULL)
INSERT [dbo].[SO] ([Id], [LeadId], [OriginId], [SizeId], [VesselId], [CustomerId], [TaxRateId], [TraderId], [Status], [OrderType], [SONumber], [SODate], [SOExpiryDate], [PartyPONumber], [PartyPODate], [PartyPOExpiryDate], [CreditPeriod], [Quantity], [AgreedRate], [TraderCommision], [CompletedOn], [ApprovedDate], [ApprovedBy], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [POScannedImage], [Remarks], [BufferQuantityMax], [BufferQuantityMin]) VALUES (38, N'1ee1700d-1ca9-4c56-983a-0285f581379e', 3, 1, 2, N'0bef2889-0462-432e-a4d3-085b9821c7a1', 0, 0, 1, 2, N'SO-1025', CAST(N'2018-10-25 00:00:00.000' AS DateTime), CAST(N'2018-10-25 00:00:00.000' AS DateTime), N'234', CAST(N'2018-10-25 00:00:00.000' AS DateTime), CAST(N'2018-10-25 00:00:00.000' AS DateTime), 3, CAST(500 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), NULL, NULL, NULL, CAST(N'2018-10-25 00:19:30.863' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-10-25 00:19:30.863' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'', N'Testing', NULL, NULL)
INSERT [dbo].[SO] ([Id], [LeadId], [OriginId], [SizeId], [VesselId], [CustomerId], [TaxRateId], [TraderId], [Status], [OrderType], [SONumber], [SODate], [SOExpiryDate], [PartyPONumber], [PartyPODate], [PartyPOExpiryDate], [CreditPeriod], [Quantity], [AgreedRate], [TraderCommision], [CompletedOn], [ApprovedDate], [ApprovedBy], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [POScannedImage], [Remarks], [BufferQuantityMax], [BufferQuantityMin]) VALUES (39, N'1ee1700d-1ca9-4c56-983a-0285f581379e', 3, 2, 1, N'4994d1c6-ad3c-4be4-932e-08f992f54531', 0, 0, 1, 2, N'SO-1026', CAST(N'2018-10-25 00:00:00.000' AS DateTime), CAST(N'2018-10-25 00:00:00.000' AS DateTime), N'2348787', CAST(N'2018-10-25 00:00:00.000' AS DateTime), CAST(N'2018-10-25 00:00:00.000' AS DateTime), 3, CAST(1000 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), NULL, NULL, NULL, CAST(N'2018-10-25 00:21:16.777' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-10-25 00:21:16.777' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'', N'123', NULL, NULL)
INSERT [dbo].[SO] ([Id], [LeadId], [OriginId], [SizeId], [VesselId], [CustomerId], [TaxRateId], [TraderId], [Status], [OrderType], [SONumber], [SODate], [SOExpiryDate], [PartyPONumber], [PartyPODate], [PartyPOExpiryDate], [CreditPeriod], [Quantity], [AgreedRate], [TraderCommision], [CompletedOn], [ApprovedDate], [ApprovedBy], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [POScannedImage], [Remarks], [BufferQuantityMax], [BufferQuantityMin]) VALUES (40, N'1ee1700d-1ca9-4c56-983a-0285f581379e', 3, 1, 1, N'4994d1c6-ad3c-4be4-932e-08f992f54531', 0, 0, 1, 2, N'SO-1027', CAST(N'2018-10-27 00:00:00.000' AS DateTime), CAST(N'2018-10-27 00:00:00.000' AS DateTime), N'234', CAST(N'2018-10-27 00:00:00.000' AS DateTime), CAST(N'2018-10-27 00:00:00.000' AS DateTime), 3, CAST(500 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), NULL, NULL, NULL, CAST(N'2018-10-27 16:20:46.870' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-10-27 16:20:46.870' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'', N'Testing', CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)))
INSERT [dbo].[SO] ([Id], [LeadId], [OriginId], [SizeId], [VesselId], [CustomerId], [TaxRateId], [TraderId], [Status], [OrderType], [SONumber], [SODate], [SOExpiryDate], [PartyPONumber], [PartyPODate], [PartyPOExpiryDate], [CreditPeriod], [Quantity], [AgreedRate], [TraderCommision], [CompletedOn], [ApprovedDate], [ApprovedBy], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [POScannedImage], [Remarks], [BufferQuantityMax], [BufferQuantityMin]) VALUES (41, N'1ee1700d-1ca9-4c56-983a-0285f581379e', 2, 2, 1, N'4994d1c6-ad3c-4be4-932e-08f992f54531', 0, 0, 1, 2, N'SO-1028', CAST(N'2018-10-27 00:00:00.000' AS DateTime), CAST(N'2018-10-27 00:00:00.000' AS DateTime), N'234', CAST(N'2018-10-27 00:00:00.000' AS DateTime), CAST(N'2018-10-27 00:00:00.000' AS DateTime), 3, CAST(500 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), NULL, NULL, NULL, CAST(N'2018-10-27 16:48:14.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-10-27 17:03:26.053' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'', N'Testing', CAST(20 AS Decimal(18, 0)), CAST(15 AS Decimal(18, 0)))
INSERT [dbo].[SO] ([Id], [LeadId], [OriginId], [SizeId], [VesselId], [CustomerId], [TaxRateId], [TraderId], [Status], [OrderType], [SONumber], [SODate], [SOExpiryDate], [PartyPONumber], [PartyPODate], [PartyPOExpiryDate], [CreditPeriod], [Quantity], [AgreedRate], [TraderCommision], [CompletedOn], [ApprovedDate], [ApprovedBy], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [POScannedImage], [Remarks], [BufferQuantityMax], [BufferQuantityMin]) VALUES (42, N'1ee1700d-1ca9-4c56-983a-0285f581379e', 2, 1, 2, N'4994d1c6-ad3c-4be4-932e-08f992f54531', 1, 1, 3, 1, N'SO-1029', CAST(N'2018-10-27 00:00:00.000' AS DateTime), CAST(N'2018-10-27 00:00:00.000' AS DateTime), N'234', CAST(N'2018-10-27 00:00:00.000' AS DateTime), CAST(N'2018-10-27 00:00:00.000' AS DateTime), 3, CAST(5000 AS Decimal(18, 0)), CAST(45 AS Decimal(18, 0)), CAST(3 AS Decimal(18, 0)), NULL, CAST(N'2018-10-27 19:02:02.133' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-10-27 18:36:03.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-10-27 19:02:02.133' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'', N'Testing Edit', CAST(13 AS Decimal(18, 0)), CAST(12 AS Decimal(18, 0)))
INSERT [dbo].[SO] ([Id], [LeadId], [OriginId], [SizeId], [VesselId], [CustomerId], [TaxRateId], [TraderId], [Status], [OrderType], [SONumber], [SODate], [SOExpiryDate], [PartyPONumber], [PartyPODate], [PartyPOExpiryDate], [CreditPeriod], [Quantity], [AgreedRate], [TraderCommision], [CompletedOn], [ApprovedDate], [ApprovedBy], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [POScannedImage], [Remarks], [BufferQuantityMax], [BufferQuantityMin]) VALUES (43, N'1ee1700d-1ca9-4c56-983a-0285f581379e', 2, 2, 1, N'4994d1c6-ad3c-4be4-932e-08f992f54531', 0, 0, 3, 2, N'SO-1030', CAST(N'2018-10-28 00:00:00.000' AS DateTime), CAST(N'2018-10-28 00:00:00.000' AS DateTime), N'234', CAST(N'2018-10-28 00:00:00.000' AS DateTime), CAST(N'2018-10-28 00:00:00.000' AS DateTime), 30, CAST(5000 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), CAST(0 AS Decimal(18, 0)), NULL, CAST(N'2018-10-28 16:19:30.273' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-10-28 16:17:29.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-10-28 16:19:30.273' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'', N'Testing', CAST(10 AS Decimal(18, 0)), CAST(10 AS Decimal(18, 0)))
SET IDENTITY_INSERT [dbo].[SO] OFF
INSERT [dbo].[StockMovement] ([Id], [StoreId], [CustomerId], [Type], [Quantity], [InOut], [Reference], [Date], [Vessel], [Origin], [Size], [Remarks]) VALUES (N'4d0521be-c1be-41fe-a951-1e1a82a0259f', N'8b13ea2a-67e4-487f-8f02-11c9be1a1983', N'ccef174b-142c-4f2a-8db4-78afc0cec7e6', 2, CAST(50000 AS Decimal(18, 0)), 1, N'422D6D1C-445D-4F06-8042-BE2E6C41F4E3', CAST(N'2018-08-09 00:00:00.000' AS DateTime), 5, 6, 6, N'54')
INSERT [dbo].[StockMovement] ([Id], [StoreId], [CustomerId], [Type], [Quantity], [InOut], [Reference], [Date], [Vessel], [Origin], [Size], [Remarks]) VALUES (N'fec538c9-c0a0-405f-9082-298b2b9027d8', N'8b90f160-1e81-41ed-93ee-b1f7ea3e97f2', N'0bef2889-0462-432e-a4d3-085b9821c7a1', 2, CAST(10000 AS Decimal(18, 0)), 1, N'A45E60C5-14AD-41A2-9C8C-2C6D61276FAC', CAST(N'2018-04-04 14:36:08.107' AS DateTime), 1, 3, 3, N'sdfg')
INSERT [dbo].[StockMovement] ([Id], [StoreId], [CustomerId], [Type], [Quantity], [InOut], [Reference], [Date], [Vessel], [Origin], [Size], [Remarks]) VALUES (N'4426b9fa-d2dd-4aea-81e8-44ec3d31bcca', N'8b90f160-1e81-41ed-93ee-b1f7ea3e97f2', N'bc746f17-04c9-4b1f-ac9a-eb599032f4d5', 2, CAST(5000 AS Decimal(18, 0)), 0, N'95EEA7E4-DBF1-4F65-832D-7C00DBC135C5', CAST(N'2018-04-04 14:34:29.077' AS DateTime), 1, 3, 3, N'sdfg')
INSERT [dbo].[StockMovement] ([Id], [StoreId], [CustomerId], [Type], [Quantity], [InOut], [Reference], [Date], [Vessel], [Origin], [Size], [Remarks]) VALUES (N'a5359ff1-1bd3-467c-bf0f-49d0e1939bab', N'8b90f160-1e81-41ed-93ee-b1f7ea3e97f2', N'4994d1c6-ad3c-4be4-932e-08f992f54531', 2, CAST(200 AS Decimal(18, 0)), 1, N'DC-1002', CAST(N'2018-04-04 14:53:14.000' AS DateTime), 4, 3, 3, N'')
INSERT [dbo].[StockMovement] ([Id], [StoreId], [CustomerId], [Type], [Quantity], [InOut], [Reference], [Date], [Vessel], [Origin], [Size], [Remarks]) VALUES (N'53aa6ac1-799e-4991-9cdd-d0856d169b23', N'8b90f160-1e81-41ed-93ee-b1f7ea3e97f2', N'4994d1c6-ad3c-4be4-932e-08f992f54531', 2, CAST(5000 AS Decimal(18, 0)), 0, N'1F96936D-A5FA-4B39-9D8D-0BEC36C7F4A2', CAST(N'2018-04-04 14:33:58.683' AS DateTime), 1, 3, 3, N'sdfg')
INSERT [dbo].[StockMovement] ([Id], [StoreId], [CustomerId], [Type], [Quantity], [InOut], [Reference], [Date], [Vessel], [Origin], [Size], [Remarks]) VALUES (N'855b46de-1bad-41c9-841e-e358189e3088', N'8b90f160-1e81-41ed-93ee-b1f7ea3e97f2', N'bc746f17-04c9-4b1f-ac9a-eb599032f4d5', 2, CAST(2000 AS Decimal(18, 0)), 0, N'DC-1001', CAST(N'2018-04-04 13:05:07.000' AS DateTime), 1, 2, 1, N'dfghj')
INSERT [dbo].[Store] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Name], [Location], [Capacity], [SaleStationId], [SubType]) VALUES (N'8b13ea2a-67e4-487f-8f02-11c9be1a1983', 1, CAST(N'2018-02-27 18:19:14.440' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-02-27 18:19:14.440' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Port Qasim', N'Karachi', CAST(100000 AS Decimal(18, 0)), N'9ef74e38-8e39-40de-a727-d56f745a6042', NULL)
INSERT [dbo].[Store] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Name], [Location], [Capacity], [SaleStationId], [SubType]) VALUES (N'b37a4521-51bd-4e13-a317-2ff51f8c6280', 1, CAST(N'2018-02-27 18:19:14.440' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-02-27 18:19:14.440' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'New Godown', N'Karachi', CAST(100000 AS Decimal(18, 0)), N'9ef74e38-8e39-40de-a727-d56f745a6042', NULL)
INSERT [dbo].[Store] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Name], [Location], [Capacity], [SaleStationId], [SubType]) VALUES (N'43961369-a7e4-4fcc-8756-4359cb20aae5', 1, CAST(N'2018-02-27 18:19:14.440' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-02-27 18:19:14.440' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Wazir Mention', N'Karachi', CAST(100000 AS Decimal(18, 0)), N'9ef74e38-8e39-40de-a727-d56f745a6042', NULL)
INSERT [dbo].[Store] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Name], [Location], [Capacity], [SaleStationId], [SubType]) VALUES (N'0ce63c9c-9e2c-4535-a8d0-5ccec93eca6c', 1, CAST(N'2018-02-27 18:19:14.573' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-02-27 18:19:14.573' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'CCM', N'Lahore', CAST(100000 AS Decimal(18, 0)), N'304af16b-60d1-4ed6-a4a4-3b3f6662d97b', NULL)
INSERT [dbo].[Store] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Name], [Location], [Capacity], [SaleStationId], [SubType]) VALUES (N'c0ffef5f-60af-485f-96f5-9bbb69223679', 1, CAST(N'2018-02-27 18:19:14.573' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-02-27 18:19:14.573' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'PIBT', N'Lahore', CAST(100000 AS Decimal(18, 0)), N'304af16b-60d1-4ed6-a4a4-3b3f6662d97b', NULL)
INSERT [dbo].[Store] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Name], [Location], [Capacity], [SaleStationId], [SubType]) VALUES (N'8b90f160-1e81-41ed-93ee-b1f7ea3e97f2', 1, CAST(N'2018-02-27 18:19:14.573' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-02-27 18:19:14.573' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'KPT', N'Lahore', CAST(100000 AS Decimal(18, 0)), N'304af16b-60d1-4ed6-a4a4-3b3f6662d97b', NULL)
INSERT [dbo].[Store] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Name], [Location], [Capacity], [SaleStationId], [SubType]) VALUES (N'c06b7913-00ff-47e2-9ce9-b4b939a49612', 1, CAST(N'2018-02-27 17:30:11.280' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-02-27 17:30:11.280' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Hawks bay', N'Karachi', CAST(100000 AS Decimal(18, 0)), N'9ef74e38-8e39-40de-a727-d56f745a6042', NULL)
INSERT [dbo].[Store] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Name], [Location], [Capacity], [SaleStationId], [SubType]) VALUES (N'10e14d33-d96e-4925-af38-b7850eeccb60', 1, CAST(N'2018-02-27 18:19:14.440' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-02-27 18:19:14.440' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'New Godown 2', N'Karachi', CAST(100000 AS Decimal(18, 0)), N'9ef74e38-8e39-40de-a727-d56f745a6042', NULL)
INSERT [dbo].[Store] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Name], [Location], [Capacity], [SaleStationId], [SubType]) VALUES (N'4197a421-e736-40b8-aab1-bace9595b774', 1, CAST(N'2018-02-27 18:19:14.440' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-02-27 18:19:14.440' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Groney Yard', N'Karachi', CAST(100000 AS Decimal(18, 0)), N'9ef74e38-8e39-40de-a727-d56f745a6042', NULL)
SET IDENTITY_INSERT [dbo].[StoreInOut] ON 

INSERT [dbo].[StoreInOut] ([Id], [Type], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [CompletedOn], [LeadId], [SMNumber], [SMDate], [Origin], [Size], [Vessel], [Quantity], [FromStoreId], [ToStoreId], [VehicleNo], [BiltyNo], [BiltyDate], [RRInvoice], [CCMNumber], [Transporter], [StoreInDate], [StoreInQuantity], [CustomerId]) VALUES (1, 1, 1, CAST(N'2018-03-14 15:03:31.973' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-03-14 15:03:31.973' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', NULL, N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'ST-1002', CAST(N'2018-03-14 00:00:00.000' AS DateTime), 2, 0, 2, CAST(10000.000 AS Decimal(18, 3)), N'8b90f160-1e81-41ed-93ee-b1f7ea3e97f2', N'43961369-a7e4-4fcc-8756-4359cb20aae5', N'sdf', N'sdf', CAST(N'2018-03-14 00:00:00.000' AS DateTime), N'asdfg', N'sdf', 1, NULL, CAST(0.000 AS Decimal(18, 3)), N'4994d1c6-ad3c-4be4-932e-08f992f54531')
INSERT [dbo].[StoreInOut] ([Id], [Type], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [CompletedOn], [LeadId], [SMNumber], [SMDate], [Origin], [Size], [Vessel], [Quantity], [FromStoreId], [ToStoreId], [VehicleNo], [BiltyNo], [BiltyDate], [RRInvoice], [CCMNumber], [Transporter], [StoreInDate], [StoreInQuantity], [CustomerId]) VALUES (2, 1, 0, CAST(N'2018-03-14 16:24:08.790' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-03-14 16:24:08.790' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', NULL, N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'ST-1003', CAST(N'2018-03-14 00:00:00.000' AS DateTime), 2, 0, 0, CAST(10000.000 AS Decimal(18, 3)), N'8b13ea2a-67e4-487f-8f02-11c9be1a1983', N'b37a4521-51bd-4e13-a317-2ff51f8c6280', N'sdf', N'sdf', CAST(N'2018-03-14 00:00:00.000' AS DateTime), N'asdfg', N'sdf', 1, NULL, CAST(0.000 AS Decimal(18, 3)), N'4994d1c6-ad3c-4be4-932e-08f992f54531')
INSERT [dbo].[StoreInOut] ([Id], [Type], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [CompletedOn], [LeadId], [SMNumber], [SMDate], [Origin], [Size], [Vessel], [Quantity], [FromStoreId], [ToStoreId], [VehicleNo], [BiltyNo], [BiltyDate], [RRInvoice], [CCMNumber], [Transporter], [StoreInDate], [StoreInQuantity], [CustomerId]) VALUES (3, 1, 0, CAST(N'2018-03-14 17:08:30.237' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-03-14 17:08:30.237' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', NULL, N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'ST-1004', CAST(N'2018-03-14 00:00:00.000' AS DateTime), 2, 4, 0, CAST(5000.000 AS Decimal(18, 3)), N'b37a4521-51bd-4e13-a317-2ff51f8c6280', N'8b13ea2a-67e4-487f-8f02-11c9be1a1983', N'sdf', N'sdf', CAST(N'2018-03-14 00:00:00.000' AS DateTime), N'asdfg', N'sdf', 1, NULL, CAST(0.000 AS Decimal(18, 3)), N'4994d1c6-ad3c-4be4-932e-08f992f54531')
INSERT [dbo].[StoreInOut] ([Id], [Type], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [CompletedOn], [LeadId], [SMNumber], [SMDate], [Origin], [Size], [Vessel], [Quantity], [FromStoreId], [ToStoreId], [VehicleNo], [BiltyNo], [BiltyDate], [RRInvoice], [CCMNumber], [Transporter], [StoreInDate], [StoreInQuantity], [CustomerId]) VALUES (4, 1, 0, CAST(N'2018-04-04 15:24:02.137' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-04-04 15:24:02.137' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', NULL, N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'ST-1005', CAST(N'2018-04-04 00:00:00.000' AS DateTime), 2, 2, 0, CAST(5000.000 AS Decimal(18, 3)), N'8b13ea2a-67e4-487f-8f02-11c9be1a1983', N'c0ffef5f-60af-485f-96f5-9bbb69223679', N'sdf', N'sdf', CAST(N'2018-04-04 00:00:00.000' AS DateTime), N'asdfg', N'sdf', 1, NULL, CAST(0.000 AS Decimal(18, 3)), N'4994d1c6-ad3c-4be4-932e-08f992f54531')
SET IDENTITY_INSERT [dbo].[StoreInOut] OFF
SET IDENTITY_INSERT [dbo].[Supplier] ON 

INSERT [dbo].[Supplier] ([Id], [Name], [Description], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (2, N'supplier 1', N'supplier 1', 1, CAST(N'2018-05-02 23:22:34.040' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-05-02 23:22:34.040' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e')
INSERT [dbo].[Supplier] ([Id], [Name], [Description], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (3, N'supplier 2', N'supplier 2', 1, CAST(N'2018-05-02 23:22:40.197' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-05-02 23:22:40.197' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e')
INSERT [dbo].[Supplier] ([Id], [Name], [Description], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (4, N'supplier 3', N'supplier 3', 1, CAST(N'2018-05-02 23:22:45.403' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-05-02 23:22:45.403' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e')
INSERT [dbo].[Supplier] ([Id], [Name], [Description], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (5, N'supplier 4', N'supplier 4', 1, CAST(N'2018-05-02 23:22:50.930' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-05-02 23:22:50.930' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e')
INSERT [dbo].[Supplier] ([Id], [Name], [Description], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (6, N'supplier 5', N'supplier 5', 1, CAST(N'2018-05-02 23:22:56.393' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-05-02 23:22:56.393' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e')
SET IDENTITY_INSERT [dbo].[Supplier] OFF
SET IDENTITY_INSERT [dbo].[TaxRate] ON 

INSERT [dbo].[TaxRate] ([Id], [Name], [Description], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (1, N'19%', N'This is for test purpose', 1, CAST(N'2018-02-15 15:22:31.023' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-02-15 15:22:31.023' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e')
INSERT [dbo].[TaxRate] ([Id], [Name], [Description], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (2, N'17%', N'This is for test purpose', 1, CAST(N'2018-02-15 15:22:31.023' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-02-15 15:22:31.023' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e')
SET IDENTITY_INSERT [dbo].[TaxRate] OFF
INSERT [dbo].[Team] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Name], [SaleStationId]) VALUES (N'4f4783dc-1394-43a8-8ed0-6ed5742000b3', 1, CAST(N'2018-01-01 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-01 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Marketing Team', N'2e92b454-74a2-4dbd-90e5-f585d5d47e80')
INSERT [dbo].[Team_User] ([Id], [TeamId], [UserId], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (1, N'4f4783dc-1394-43a8-8ed0-6ed5742000b3', N'1ee1700d-1ca9-4c56-983a-0285f581379e', 1, CAST(N'2018-02-22 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-02-22 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e')
SET IDENTITY_INSERT [dbo].[Trader] ON 

INSERT [dbo].[Trader] ([Id], [Name], [Description], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (1, N'Trader ABC', N'This is for test purpose', 1, CAST(N'2018-02-15 15:22:31.023' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-02-15 15:22:31.023' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e')
INSERT [dbo].[Trader] ([Id], [Name], [Description], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (2, N' Trader XYZ', N'This is for test purpose', 1, CAST(N'2018-02-15 15:22:31.023' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-02-15 15:22:31.023' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e')
SET IDENTITY_INSERT [dbo].[Trader] OFF
INSERT [dbo].[User] ([Id], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [Name], [LoginName], [Password], [Designation], [Email], [Mobile], [Office], [Home], [Address], [Picture], [Remarks]) VALUES (N'1ee1700d-1ca9-4c56-983a-0285f581379e', 1, CAST(N'2018-01-01 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-01 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'Zeeshan', N'zeeshan', N'1', N'Software Developer', N'zeeshan@mzxrmsolutions.com', N'03204148980', N'03204148980', N'03015502004', N'Wah Cantt', N'Zeeshan Shafqat.jpg', NULL)
INSERT [dbo].[User_Role] ([Id], [UserId], [RoleId], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (N'10db64f9-aad9-4ad6-ac75-406e25ded0ee', N'1ee1700d-1ca9-4c56-983a-0285f581379e', N'62d7488b-5e91-4d6f-b0ec-872176eeb25c', CAST(N'2018-01-01 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-01-01 00:00:00.000' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e')
SET IDENTITY_INSERT [dbo].[Vessel] ON 

INSERT [dbo].[Vessel] ([Id], [Name], [Description], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (1, N'Sbi Hera', N'This is for test purpose', 1, CAST(N'2018-02-15 15:22:30.897' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-02-15 15:22:30.897' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e')
INSERT [dbo].[Vessel] ([Id], [Name], [Description], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (2, N' MARE TRACER', N'This is for test purpose', 1, CAST(N'2018-02-15 15:22:30.897' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-02-15 15:22:30.897' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e')
INSERT [dbo].[Vessel] ([Id], [Name], [Description], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (3, N'IVS Pinehurst', N'This is for test purpose', 1, CAST(N'2018-02-15 15:22:30.897' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-02-15 15:22:30.897' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e')
INSERT [dbo].[Vessel] ([Id], [Name], [Description], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (4, N'ARIETTA', N'This is for test purpose', 1, CAST(N'2018-02-15 15:22:30.897' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-02-15 15:22:30.897' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e')
INSERT [dbo].[Vessel] ([Id], [Name], [Description], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (5, N'Greener', N'This is for test purpose', 1, CAST(N'2018-02-15 15:22:30.897' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-02-15 15:22:30.897' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e')
INSERT [dbo].[Vessel] ([Id], [Name], [Description], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (6, N'Achilleas S', N'This is for test purpose', 1, CAST(N'2018-02-15 15:22:30.897' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-02-15 15:22:30.897' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e')
INSERT [dbo].[Vessel] ([Id], [Name], [Description], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (7, N'Alam Manis', N'This is for test purpose', 1, CAST(N'2018-02-15 15:22:30.897' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-02-15 15:22:30.897' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e')
INSERT [dbo].[Vessel] ([Id], [Name], [Description], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (8, N'Calimero', N'This is for test purpose', 1, CAST(N'2018-02-15 15:22:30.897' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-02-15 15:22:30.897' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e')
INSERT [dbo].[Vessel] ([Id], [Name], [Description], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (9, N'Stove Caledonia', N'This is for test purpose', 1, CAST(N'2018-02-15 15:22:30.897' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-02-15 15:22:30.897' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e')
INSERT [dbo].[Vessel] ([Id], [Name], [Description], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (10, N'Sinar Kutai', N'This is for test purpose', 1, CAST(N'2018-02-15 15:22:30.897' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e', CAST(N'2018-02-15 15:22:30.897' AS DateTime), N'1ee1700d-1ca9-4c56-983a-0285f581379e')
SET IDENTITY_INSERT [dbo].[Vessel] OFF
ALTER TABLE [dbo].[CustomerStock] ADD  CONSTRAINT [DF_CustomerStock_Id]  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[DCL]  WITH CHECK ADD  CONSTRAINT [FK_DCL_PODetail] FOREIGN KEY([PODetailId])
REFERENCES [dbo].[PODetail] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[DCL] CHECK CONSTRAINT [FK_DCL_PODetail]
GO
ALTER TABLE [dbo].[GRN]  WITH CHECK ADD  CONSTRAINT [FK_GRN_PODetail] FOREIGN KEY([PODetailId])
REFERENCES [dbo].[PODetail] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[GRN] CHECK CONSTRAINT [FK_GRN_PODetail]
GO
ALTER TABLE [dbo].[PODetail]  WITH CHECK ADD  CONSTRAINT [FK_PODetail_Customer] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PODetail] CHECK CONSTRAINT [FK_PODetail_Customer]
GO
ALTER TABLE [dbo].[PODetail]  WITH CHECK ADD  CONSTRAINT [FK_PODetail_PO] FOREIGN KEY([POId])
REFERENCES [dbo].[PO] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PODetail] CHECK CONSTRAINT [FK_PODetail_PO]
GO
ALTER TABLE [dbo].[SaleStation]  WITH CHECK ADD  CONSTRAINT [FK_SaleStation_Organization] FOREIGN KEY([OrganizationId])
REFERENCES [dbo].[Organization] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SaleStation] CHECK CONSTRAINT [FK_SaleStation_Organization]
GO
ALTER TABLE [dbo].[SeivingSizeQty]  WITH CHECK ADD  CONSTRAINT [FK_SeivingSizeQty_Seiving] FOREIGN KEY([SeivingID])
REFERENCES [dbo].[Seiving] ([ID])
GO
ALTER TABLE [dbo].[SeivingSizeQty] CHECK CONSTRAINT [FK_SeivingSizeQty_Seiving]
GO
ALTER TABLE [dbo].[StockMovement]  WITH CHECK ADD  CONSTRAINT [FK_StockMovement_Customer] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[StockMovement] CHECK CONSTRAINT [FK_StockMovement_Customer]
GO
ALTER TABLE [dbo].[StockMovement]  WITH CHECK ADD  CONSTRAINT [FK_StockMovement_Store] FOREIGN KEY([StoreId])
REFERENCES [dbo].[Store] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[StockMovement] CHECK CONSTRAINT [FK_StockMovement_Store]
GO
ALTER TABLE [dbo].[Store]  WITH CHECK ADD  CONSTRAINT [FK_Store_SaleStation] FOREIGN KEY([SaleStationId])
REFERENCES [dbo].[SaleStation] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Store] CHECK CONSTRAINT [FK_Store_SaleStation]
GO
ALTER TABLE [dbo].[Team]  WITH CHECK ADD  CONSTRAINT [FK_Team_SaleStation] FOREIGN KEY([SaleStationId])
REFERENCES [dbo].[SaleStation] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Team] CHECK CONSTRAINT [FK_Team_SaleStation]
GO
ALTER TABLE [dbo].[Team_User]  WITH CHECK ADD  CONSTRAINT [FK_Team_User_Team] FOREIGN KEY([TeamId])
REFERENCES [dbo].[Team] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Team_User] CHECK CONSTRAINT [FK_Team_User_Team]
GO
ALTER TABLE [dbo].[Team_User]  WITH CHECK ADD  CONSTRAINT [FK_Team_User_User1] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Team_User] CHECK CONSTRAINT [FK_Team_User_User1]
GO
ALTER TABLE [dbo].[User_Role]  WITH CHECK ADD  CONSTRAINT [FK_User_Role_Role] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Role] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[User_Role] CHECK CONSTRAINT [FK_User_Role_Role]
GO
ALTER TABLE [dbo].[User_Role]  WITH CHECK ADD  CONSTRAINT [FK_User_Role_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[User_Role] CHECK CONSTRAINT [FK_User_Role_User]
GO
/****** Object:  StoredProcedure [dbo].[crud_DCDelete]    Script Date: 10/28/2018 7:59:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[crud_DCDelete]
 @ID [int]
AS
 SET NOCOUNT ON
 SET XACT_ABORT ON
 
 BEGIN TRANSACTION
 DELETE FROM dbo.DC
 WHERE (ID = @ID OR @ID IS NULL)
 COMMIT

GO
/****** Object:  StoredProcedure [dbo].[crud_DCInsert]    Script Date: 10/28/2018 7:59:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[crud_DCInsert]
 (
 @DOId [uniqueidentifier],
 @LeadId [uniqueidentifier],
 @TransporterId [uniqueidentifier],
 @Status [int],
 @DCNumber [varchar](50),
 @DCDate [datetime],
 @Quantity [decimal](18, 0),
 @TruckNo [varchar](50),
 @BiltyNo [varchar](50),
 @SlipNo [varchar](50),
 @Weight [decimal](18, 0),
 @NetWeight [decimal](18, 0),
 @DriverName [varchar](100),
 @DriverPhone [varchar](50),
 @Remarks [varchar](250),
 @CreatedOn [datetime],
 @CreatedBy [uniqueidentifier],
 @ModifiedOn [datetime],
 @ModifiedBy [uniqueidentifier]
 )
AS
 SET NOCOUNT ON
 SET XACT_ABORT ON
 
 BEGIN TRANSACTION

 INSERT INTO dbo.DC
 (
 DOId, LeadId, TransporterId, Status, DCNumber, DCDate, Quantity, TruckNo, BiltyNo, SlipNo, Weight, NetWeight, DriverName, DriverPhone, Remarks, CreatedOn, CreatedBy, ModifiedOn, ModifiedBy
 )
 VALUES
 (
 @DOId,
 @LeadId,
 @TransporterId,
 @Status,
 @DCNumber,
 @DCDate,
 @Quantity,
 @TruckNo,
 @BiltyNo,
 @SlipNo,
 @Weight,
 @NetWeight,
 @DriverName,
 @DriverPhone,
 @Remarks,
 @CreatedOn,
 @CreatedBy,
 @ModifiedOn,
 @ModifiedBy

 )
 SELECT ID, DOId, LeadId, TransporterId, Status, DCNumber, DCDate, Quantity, TruckNo, BiltyNo, SlipNo, Weight, NetWeight, DriverName, DriverPhone, Remarks, CreatedOn, CreatedBy, ModifiedOn, ModifiedBy
 FROM dbo.DC
 WHERE (ID = SCOPE_IDENTITY())

 COMMIT

GO
/****** Object:  StoredProcedure [dbo].[crud_DCSelect]    Script Date: 10/28/2018 7:59:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[crud_DCSelect]
 @ID [int]
AS
 SET NOCOUNT ON
 SET XACT_ABORT ON
 
 BEGIN TRANSACTION

 SELECT ID, DOId, LeadId, TransporterId, Status, DCNumber, DCDate, Quantity, TruckNo, BiltyNo, SlipNo, Weight, NetWeight, DriverName, DriverPhone, Remarks, CreatedOn, CreatedBy, ModifiedOn, ModifiedBy
 FROM dbo.DC
 WHERE (ID = @ID OR @ID IS NULL)

 COMMIT

GO
/****** Object:  StoredProcedure [dbo].[crud_DCUpdate]    Script Date: 10/28/2018 7:59:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[crud_DCUpdate]
 (
 @ID [int],
 @DOId [uniqueidentifier],
 @LeadId [uniqueidentifier],
 @TransporterId [uniqueidentifier],
 @Status [int],
 @DCNumber [varchar](50),
 @DCDate [datetime],
 @Quantity [decimal](18, 0),
 @TruckNo [varchar](50),
 @BiltyNo [varchar](50),
 @SlipNo [varchar](50),
 @Weight [decimal](18, 0),
 @NetWeight [decimal](18, 0),
 @DriverName [varchar](100),
 @DriverPhone [varchar](50),
 @Remarks [varchar](250),
 @CreatedOn [datetime],
 @CreatedBy [uniqueidentifier],
 @ModifiedOn [datetime],
 @ModifiedBy [uniqueidentifier]
 )
AS
 SET NOCOUNT ON
 SET XACT_ABORT ON
 
 BEGIN TRANSACTION
 UPDATE dbo.DC
 SET DOId = @DOId, LeadId = @LeadId, TransporterId = @TransporterId, Status = @Status, DCNumber = @DCNumber, DCDate = @DCDate, Quantity = @Quantity, TruckNo = @TruckNo, BiltyNo = @BiltyNo, SlipNo = @SlipNo, Weight = @Weight, NetWeight = @NetWeight, DriverName = @DriverName, DriverPhone = @DriverPhone, Remarks = @Remarks, CreatedOn = @CreatedOn, CreatedBy = @CreatedBy, ModifiedOn = @ModifiedOn, ModifiedBy = @ModifiedBy
 WHERE (ID = @ID OR @ID IS NULL)
 COMMIT

GO
/****** Object:  StoredProcedure [dbo].[Insert_Exception]    Script Date: 10/28/2018 7:59:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
 CREATE PROCEDURE [dbo].[Insert_Exception]@ExceptionDate datetime = null,@ExceptionMessage nvarchar(200)=null,@ExceptionType nvarchar(200)=null,@ExceptionSource nvarchar(max)=null,@ExceptionURL nvarchar(200)=null,@ExceptionSystem nvarchar(50)=nullASINSERT INTO ExceptionLogs (ExceptionDate, ExceptionMessage, ExceptionType, ExceptionSource, ExceptionURL, ExceptionSystem)VALUES (GETDATE(),@ExceptionMessage,@ExceptionType,@ExceptionSource,@ExceptionURL,@ExceptionSystem)
GO
/****** Object:  StoredProcedure [dbo].[Select_AllExceptions]    Script Date: 10/28/2018 7:59:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Select_AllExceptions]ASSELECT * FROM ExceptionLogs
GO
/****** Object:  StoredProcedure [dbo].[sp_AuthenticateUser]    Script Date: 10/28/2018 7:59:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_AuthenticateUser] 
 @LoginName NVARCHAR(50),
 @Password NVARCHAR(50) 
AS
BEGIN
 -- SET NOCOUNT ON added to prevent extra result sets from
 -- interfering with SELECT statements.
 SET NOCOUNT ON;
DECLARE @ReturnMessage NVARCHAR(250)
 --CHECKING IF LOGIN NAME EXISTS
IF(NOT EXISTS(SELECT * FROM [User] WHERE LoginName = @LoginName))
 BEGIN
 --SETTING INVALID USERNAME ERROR MESSAGE TO BE SHOWN ON UI
 SET @ReturnMessage = 'Invalid user name: ' + @LoginName
 SELECT @ReturnMessage AS [Message]
 END
ELSE
 BEGIN
 IF(NOT EXISTS(SELECT * FROM [User] WHERE LoginName = @LoginName AND [Password] = @Password))
 BEGIN
 --SETTING INVALID PASSWORD ERROR MESSAGE TO BE SHOWN ON UI
 SET @ReturnMessage = 'Invalid password.'
 SELECT @ReturnMessage AS [Message]
 END
 ELSE 
 BEGIN
 IF ((SELECT [Status] FROM [User] WHERE LoginName = @LoginName AND [Password] = @Password) = '0')
 BEGIN
 --IF USER IS DISABLED BY ADMIN, SETTING MESSAGE TO BE SHOWN ON UI
 SET @ReturnMessage = @LoginName + ' is disabled. Please contact administrator for details.'
 SELECT @ReturnMessage AS [Message]
 END
 ELSE
 BEGIN
 --RETURNING ALL THE REQUIRED DATA IN CASE OF VALID LOGIN NAME AND PASSWORD
 SELECT DISTINCT '' AS [Message], U.Id AS UserId, U.[Status] AS [UserStatus], U.[Name] AS UserName, U.LoginName, /*U.[Password],*/ U.Designation, U.Email, U.Mobile, U.Office, U.Home, U.[Address],U.Picture --GETTING USER INFORAMTION
 --, UR.Id AS [UserRoleId] -- GETTING USER ROLE ID
 , R.Id AS RoleId, R.[Name] AS RoleName -- GETTING ROLE RELATED INFORMATION
 --, [Role] = STUFF((SELECT ', ' + [Name] FROM [Role] WHERE UR.RoleId = Id AND UR.UserId = U.Id FOR XML PATH('')), 1, 2, '')
 , T.Id AS TeamId, T.[Name] AS TeamName -- GETTING TEAM RELATED INFORMATION
 FROM [User] AS U
 INNER JOIN User_Role AS UR ON U.ID = UR.UserId AND U.[Status] = 1 -- 1 MEANS USER IS ACTIVE
 INNER JOIN [Role] AS R ON R.Id = UR.RoleId
 INNER JOIN Team_User AS TU ON U.Id = TU.UserId
 INNER JOIN Team AS T ON T.Id = TU.TeamId
 WHERE U.LoginName = @LoginName
 END
 END
 END
END
GO
/****** Object:  StoredProcedure [dbo].[sp_DeleteDOById]    Script Date: 10/28/2018 7:59:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_DeleteDOById]
 @ID [int]
AS
 SET NOCOUNT ON
 SET XACT_ABORT ON
 
 BEGIN TRANSACTION
 DELETE FROM dbo.DO
 WHERE (ID = @ID OR @ID IS NULL)
 COMMIT

GO
/****** Object:  StoredProcedure [dbo].[sp_GetAllCustomer]    Script Date: 10/28/2018 7:59:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Zeeshan Shafqat>
-- Create date: <17th January2018>
-- Description:	
-- =============================================
Create PROCEDURE [dbo].[sp_GetAllCustomer] 
		
AS
BEGIN

BEGIN TRY
	SELECT [Id]
      ,[Status]
      ,[CreatedOn]
      ,[CreatedBy]
      ,[ModifiedOn]
      ,[ModifiedBy]
      ,[Lead]
      ,[FullName]
      ,[ShortName]
      ,[NTN]
      ,[STRN]
      ,[Address]
      ,[InvoiceAddress]
      ,[Email]
      ,[Phone]
      ,[ContactPerson]
      ,[HeadOffice]
      ,[Remarks]
  FROM [dbo].[Customer]
	
END TRY
BEGIN CATCH
		
END CATCH
	
END

GO
/****** Object:  StoredProcedure [dbo].[sp_GetAllCustomerDestination]    Script Date: 10/28/2018 7:59:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_GetAllCustomerDestination]
AS
 SET NOCOUNT ON
 
 SELECT Id, CustomerId, Name, Status
 FROM dbo.CustomerDestination

GO
/****** Object:  StoredProcedure [dbo].[sp_GetAllCustomerStock]    Script Date: 10/28/2018 7:59:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Zeeshan Shafqat>
-- Create date: <17th January2018>
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetAllCustomerStock] 
		
AS
BEGIN

BEGIN TRY
	
	
	select  CustomerId,StoreId,Origin,Vessel,Size,Sum(Quantity)  as Quantity
from StockMovement 
group By StoreId,CustomerId,Origin,Vessel,Size
END TRY
BEGIN CATCH
		
END CATCH
	
END

GO
/****** Object:  StoredProcedure [dbo].[sp_GetAllDC]    Script Date: 10/28/2018 7:59:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_GetAllDC]
 
AS
 SET NOCOUNT ON
 SET XACT_ABORT ON
 
 BEGIN TRANSACTION

SELECT ID, DOId, LeadId, TransporterId, Status, DCNumber, DCDate, Quantity, TruckNo, BiltyNo, SlipNo, Weight, NetWeight, DriverName, DriverPhone, Remarks, CreatedOn, CreatedBy, ModifiedOn, ModifiedBy
 FROM dbo.DC

 COMMIT
GO
/****** Object:  StoredProcedure [dbo].[sp_GetAllDCL]    Script Date: 10/28/2018 7:59:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Zeeshan Shafqat>
-- Create date: <17th January2018>
-- Description:	
-- =============================================
Create PROCEDURE [dbo].[sp_GetAllDCL] 
		
AS
BEGIN

BEGIN TRY
	SELECT [Id]
      ,[Status]
      ,[CreatedOn]
      ,[CreatedBy]
      ,[ModifiedOn]
      ,[ModifiedBy]
      ,[CompletedOn]
      ,[DCLNumber]
      ,[DCLDate]
      ,[PODetailId]
      ,[Store]
      ,[Quantity]
      ,[Remarks]
  FROM [dbo].[DCL]
	
END TRY
BEGIN CATCH
		
END CATCH
	
END

GO
/****** Object:  StoredProcedure [dbo].[sp_GetAllDO]    Script Date: 10/28/2018 7:59:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_GetAllDO]
AS
 SET NOCOUNT ON
 SET XACT_ABORT ON
 
 BEGIN TRANSACTION

 SELECT *
 FROM dbo.DO

 COMMIT
GO
/****** Object:  StoredProcedure [dbo].[sp_GetAllGRN]    Script Date: 10/28/2018 7:59:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Zeeshan Shafqat>
-- Create date: <17th January2018>
-- Description:	
-- =============================================
Create PROCEDURE [dbo].[sp_GetAllGRN] 
		
AS
BEGIN

BEGIN TRY
	SELECT [Id]
      ,[Status]
      ,[CreatedOn]
      ,[CreatedBy]
      ,[ModifiedOn]
      ,[ModifiedBy]
      ,[CompletedOn]
      ,[GRNNumber]
      ,[GRNDate]
      ,[PODetailId]
      ,[Store]
      ,[InvoiceNo]
      ,[AdjPrice]
      ,[Quantity]
      ,[Remarks]
  FROM [dbo].[GRN]
	
END TRY
BEGIN CATCH
		
END CATCH
	
END

GO
/****** Object:  StoredProcedure [dbo].[sp_GetAllOrigin]    Script Date: 10/28/2018 7:59:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Zeeshan Shafqat>
-- Create date: <15th Feb2018>
-- Description:	
-- =============================================
Create PROCEDURE [dbo].[sp_GetAllOrigin] 
		
AS
BEGIN

BEGIN TRY
	SELECT *
  FROM [dbo].[Origin]
	
END TRY
BEGIN CATCH
		
END CATCH
	
END

GO
/****** Object:  StoredProcedure [dbo].[sp_GetAllPO]    Script Date: 10/28/2018 7:59:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Zeeshan Shafqat>
-- Create date: <17th January2018>
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetAllPO] 
		
AS
BEGIN

BEGIN TRY
	SELECT [Id]
      ,[Status]
      ,[CreatedOn]
      ,[CreatedBy]
      ,[ModifiedOn]
      ,[ModifiedBy]
      ,[CompletedOn]
      ,[LeadId]
      ,[ApprovedDate]
      ,[ApprovedBy]
      ,[PONumber]
      ,[PODate]
      ,[Origin]
      ,[Size]
      ,[Vessel]
      ,[TargetDays]
      ,[Supplier]
      ,[TermsOfPayment]
      ,[BufferQuantityMax]
      ,[BufferQuantityMin]
  FROM [dbo].[PO]
	
END TRY
BEGIN CATCH
		
END CATCH
	
END

GO
/****** Object:  StoredProcedure [dbo].[sp_GetAllPODetail]    Script Date: 10/28/2018 7:59:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Zeeshan Shafqat>
-- Create date: <17th January2018>
-- Description:	
-- =============================================
Create PROCEDURE [dbo].[sp_GetAllPODetail] 
		
AS
BEGIN

BEGIN TRY
	SELECT [Id]
      ,[POId]
      ,[CustomerId]
      ,[Quantity]
      ,[Rate]
      ,[CostPerTon]
      ,[AllowedWastage]
      ,[TargetDate]
      ,[Remarks]
  FROM [dbo].[PODetail]
	
END TRY
BEGIN CATCH
		
END CATCH
	
END

GO
/****** Object:  StoredProcedure [dbo].[sp_GetAllSaleStation]    Script Date: 10/28/2018 7:59:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_GetAllSaleStation]
AS
 SET NOCOUNT ON
 SET XACT_ABORT ON
 
 SELECT Id, Status, CreatedOn, CreatedBy, ModifiedOn, ModifiedBy, Name, OrganizationId
 FROM dbo.SaleStation

GO
/****** Object:  StoredProcedure [dbo].[sp_GetAllSize]    Script Date: 10/28/2018 7:59:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Zeeshan Shafqat>
-- Create date: <15th Feb2018>
-- Description:	
-- =============================================
Create PROCEDURE [dbo].[sp_GetAllSize] 
		
AS
BEGIN

BEGIN TRY
	SELECT *
  FROM [dbo].[Size]
	
END TRY
BEGIN CATCH
		
END CATCH
	
END

GO
/****** Object:  StoredProcedure [dbo].[sp_GetAllSO]    Script Date: 10/28/2018 7:59:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Zeeshan Shafqat>
-- Create date: <15th Feb2018>
-- Description:	
-- =============================================
create PROCEDURE [dbo].[sp_GetAllSO] 
		
AS
BEGIN

BEGIN TRY
	SELECT *
  FROM [dbo].[SO]
	
END TRY
BEGIN CATCH
		
END CATCH
	
END

GO
/****** Object:  StoredProcedure [dbo].[sp_GetAllStockMovement]    Script Date: 10/28/2018 7:59:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_GetAllStockMovement]
 
AS
 SET NOCOUNT ON
 SET XACT_ABORT ON
 
 BEGIN TRANSACTION

 SELECT *
 FROM dbo.StockMovement
 order by CustomerId
 

 COMMIT

GO
/****** Object:  StoredProcedure [dbo].[sp_GetAllStore]    Script Date: 10/28/2018 7:59:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Zeeshan Shafqat>
-- Create date: <17th January2018>
-- Description:	
-- =============================================
Create PROCEDURE [dbo].[sp_GetAllStore] 
		
AS
BEGIN

BEGIN TRY
	SELECT [Id]
      ,[Status]
      ,[CreatedOn]
      ,[CreatedBy]
      ,[ModifiedOn]
      ,[ModifiedBy]
      ,[Name]
      ,[Location]
      ,[Capacity]
      ,[SaleStationId]
      ,[SubType]
  FROM [dbo].[Store]
	
END TRY
BEGIN CATCH
		
END CATCH
	
END

GO
/****** Object:  StoredProcedure [dbo].[sp_GetAllStoreInOut]    Script Date: 10/28/2018 7:59:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_GetAllStoreInOut]
 
AS
 SET NOCOUNT ON
 SET XACT_ABORT ON
 
 BEGIN TRANSACTION

 SELECT Id, Type, Status, CreatedOn, CreatedBy, ModifiedOn, ModifiedBy, CompletedOn, LeadId, SMNumber, SMDate,CustomerId, Origin, Size, Vessel, Quantity, FromStoreId, ToStoreId, VehicleNo, BiltyNo, BiltyDate, RRInvoice, CCMNumber, Transporter, StoreInDate, StoreInQuantity
 FROM dbo.StoreInOut

 COMMIT

GO
/****** Object:  StoredProcedure [dbo].[sp_GetAllSupplier]    Script Date: 10/28/2018 7:59:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Zeeshan Shafqat>
-- Create date: <15th Feb2018>
-- Description:	
-- =============================================
create PROCEDURE [dbo].[sp_GetAllSupplier] 
		
AS
BEGIN

BEGIN TRY
	SELECT *
  FROM [dbo].[Supplier]
	
END TRY
BEGIN CATCH
		
END CATCH
	
END

GO
/****** Object:  StoredProcedure [dbo].[sp_GetAllTaxRate]    Script Date: 10/28/2018 7:59:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Zeeshan Shafqat>
-- Create date: <15th Feb2018>
-- Description:	
-- =============================================
create PROCEDURE [dbo].[sp_GetAllTaxRate] 
		
AS
BEGIN

BEGIN TRY
	SELECT *
  FROM [dbo].[TaxRate]
	
END TRY
BEGIN CATCH
		
END CATCH
	
END

GO
/****** Object:  StoredProcedure [dbo].[sp_GetAllTrader]    Script Date: 10/28/2018 7:59:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Zeeshan Shafqat>
-- Create date: <15th Feb2018>
-- Description:	
-- =============================================
Create PROCEDURE [dbo].[sp_GetAllTrader] 
		
AS
BEGIN

BEGIN TRY
	SELECT *
  FROM [dbo].[Trader]
	
END TRY
BEGIN CATCH
		
END CATCH
	
END

GO
/****** Object:  StoredProcedure [dbo].[sp_GetAllTransporter]    Script Date: 10/28/2018 7:59:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Zeeshan Shafqat>
-- Create date: <15th Feb2018>
-- Description:	
-- =============================================
Create PROCEDURE [dbo].[sp_GetAllTransporter] 
		
AS
BEGIN

BEGIN TRY
	SELECT *
  FROM [dbo].[Transporter]
	
END TRY
BEGIN CATCH
		
END CATCH
	
END

GO
/****** Object:  StoredProcedure [dbo].[sp_GetAllUser]    Script Date: 10/28/2018 7:59:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Zeeshan Shafqat>
-- Create date: <17th January2018>
-- Description:	
-- =============================================
Create PROCEDURE [dbo].[sp_GetAllUser] 
		
AS
BEGIN

BEGIN TRY
	SELECT *
  FROM [dbo].[User]
	
END TRY
BEGIN CATCH
		
END CATCH
	
END

GO
/****** Object:  StoredProcedure [dbo].[sp_GetAllUserRole]    Script Date: 10/28/2018 7:59:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Zeeshan Shafqat>
-- Create date: <17th January2018>
-- Description:	
-- =============================================
Create PROCEDURE [dbo].[sp_GetAllUserRole] 
		
AS
BEGIN

BEGIN TRY
	select Userid, RoleId, Role.Name from User_role join [Role] on User_Role.RoleId=Role.Id
	
END TRY
BEGIN CATCH
		
END CATCH
	
END

GO
/****** Object:  StoredProcedure [dbo].[sp_GetAllUserTeam]    Script Date: 10/28/2018 7:59:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Zeeshan Shafqat>
-- Create date: <17th January2018>
-- Description:	
-- =============================================
Create PROCEDURE [dbo].[sp_GetAllUserTeam] 
		
AS
BEGIN

BEGIN TRY
	select Userid, TeamId, Team.Name from Team_User join [Team] on Team_User.TeamId=Team.Id
	
END TRY
BEGIN CATCH
		
END CATCH
	
END

GO
/****** Object:  StoredProcedure [dbo].[sp_GetAllVessel]    Script Date: 10/28/2018 7:59:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Zeeshan Shafqat>
-- Create date: <15th Feb2018>
-- Description:	
-- =============================================
Create PROCEDURE [dbo].[sp_GetAllVessel] 
		
AS
BEGIN

BEGIN TRY
	SELECT *
  FROM [dbo].[Vessel]
	
END TRY
BEGIN CATCH
		
END CATCH
	
END

GO
/****** Object:  StoredProcedure [dbo].[sp_GetStoreBySaleStation]    Script Date: 10/28/2018 7:59:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Zeeshan Shafqat>
-- Create date: <26th Octuber2018>
-- Description:	
-- =============================================
Create PROCEDURE [dbo].[sp_GetStoreBySaleStation] 
		@SaleStationId uniqueidentifier
AS
BEGIN

BEGIN TRY
	SELECT [Id]
      ,[Status]
      ,[CreatedOn]
      ,[CreatedBy]
      ,[ModifiedOn]
      ,[ModifiedBy]
      ,[Name]
      ,[Location]
      ,[Capacity]
      ,[SaleStationId]
      ,[SubType]
  FROM [dbo].[Store]
  where SaleStationId=@SaleStationId
	
END TRY
BEGIN CATCH
		
END CATCH
	
END

GO
/****** Object:  StoredProcedure [dbo].[sp_InsertCustomer]    Script Date: 10/28/2018 7:59:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Zeeshan Shafqat>
-- Create date: <19th January 2018>
-- Description:	<Adding Customer>
-- =============================================
CREATE PROCEDURE [dbo].[sp_InsertCustomer] 
@Status int
,@CreatedOn datetime
,@CreatedBy uniqueidentifier
,@ModifiedOn datetime
,@ModifiedBy uniqueidentifier
,@Lead uniqueidentifier
,@FullName nvarchar(250)
,@ShortName  nvarchar(250)
,@NTN  nvarchar(250)
,@STRN  nvarchar(250)
,@Address  nvarchar(250)
,@InvoiceAddress  nvarchar(250)
,@Email  nvarchar(250)
,@Phone  nvarchar(250)
,@ContactPerson  nvarchar(250)
,@HeadOffice  nvarchar(250)
,@Remarks  nvarchar(MAX)
	
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @id TABLE(NewValue UNIQUEIDENTIFIER);

	SET XACT_ABORT ON
	BEGIN TRAN Customer
		BEGIN TRY
	
		--ADDING DATA TO TABLE 
		INSERT INTO Customer([Status],[CreatedOn],[CreatedBy],[ModifiedOn],[ModifiedBy],[Lead],[FullName],[ShortName],[NTN],[STRN],[Address],[InvoiceAddress],[Email],[Phone],[ContactPerson],[HeadOffice],[Remarks])
		output inserted.Id into @id
		VALUES (@Status,@CreatedOn,@CreatedBy,@ModifiedOn,@ModifiedBy,@Lead,@FullName,@ShortName,@NTN,@STRN,@Address,@InvoiceAddress,@Email,@Phone,@ContactPerson,@HeadOffice,@Remarks)

		select * from @id

		COMMIT TRAN Customer
	END TRY
	BEGIN CATCH
		--IN CASE OF ANY ERROR ROLL BACK THE TRANSACTION
		SELECT 'Error Occured. Error is: ' + ERROR_MESSAGE()
		ROLLBACK TRAN PO_PODetails
	END CATCH
	
END

GO
/****** Object:  StoredProcedure [dbo].[sp_InsertCustomerDestination]    Script Date: 10/28/2018 7:59:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_InsertCustomerDestination]
 (
 @CustomerId [uniqueidentifier],
 @Name [nvarchar](250),
 @Status [int]
 )
AS
 SET NOCOUNT ON
 SET XACT_ABORT ON
 
 BEGIN TRANSACTION

 INSERT INTO dbo.CustomerDestination
 (
 CustomerId, Name, Status
 )
 VALUES
 (
 @CustomerId,
 @Name,
 @Status

 )
 SELECT Id, CustomerId, Name, Status
 FROM dbo.CustomerDestination
 WHERE (Id = SCOPE_IDENTITY())

 COMMIT

GO
/****** Object:  StoredProcedure [dbo].[sp_InsertDC]    Script Date: 10/28/2018 7:59:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_InsertDC]
 (
 @DOId int,
 @LeadId [uniqueidentifier],
 @Store uniqueidentifier,
 @TransporterId int,
 @Status [int],
 
 @DCDate [datetime],
 @Quantity [decimal](18, 0),
 @TruckNo [varchar](50),
 @BiltyNo [varchar](50),
 @SlipNo [varchar](50),
 @Weight [decimal](18, 0),
 @NetWeight [decimal](18, 0),
 @DriverName [varchar](100),
 @DriverPhone [varchar](50),
 @Remarks [varchar](250),
 @CreatedOn [datetime],
 @CreatedBy [uniqueidentifier],
 @ModifiedOn [datetime],
 @ModifiedBy [uniqueidentifier]
 )
AS
 SET NOCOUNT ON
 SET XACT_ABORT ON
 
 BEGIN TRANSACTION
 DECLARE @DCNumber [varchar](50)
 EXEC @DCNumber =dbo.[fn_GenerateNextDCNumber]
 INSERT INTO dbo.DC
 (
 DOId, LeadId,Store, TransporterId, Status, DCNumber, DCDate, Quantity, TruckNo, BiltyNo, SlipNo, Weight, NetWeight, DriverName, DriverPhone, Remarks, CreatedOn, CreatedBy, ModifiedOn, ModifiedBy
 )
 VALUES
 (
 @DOId,
 @LeadId,
 @Store,
 @TransporterId,
 @Status,
 @DCNumber,
 @DCDate,
 @Quantity,
 @TruckNo,
 @BiltyNo,
 @SlipNo,
 @Weight,
 @NetWeight,
 @DriverName,
 @DriverPhone,
 @Remarks,
 @CreatedOn,
 @CreatedBy,
 @ModifiedOn,
 @ModifiedBy

 )
 SELECT ID, DOId, LeadId, Store, TransporterId, Status, DCNumber, DCDate, Quantity, TruckNo, BiltyNo, SlipNo, Weight, NetWeight, DriverName, DriverPhone, Remarks, CreatedOn, CreatedBy, ModifiedOn, ModifiedBy
 FROM dbo.DC
 WHERE (ID = SCOPE_IDENTITY())

 COMMIT
GO
/****** Object:  StoredProcedure [dbo].[sp_InsertDCL]    Script Date: 10/28/2018 7:59:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Zeeshan Shafqat>
-- Create date: <19th January 2018>
-- Description:	<Adding DCL>
-- =============================================
CREATE PROCEDURE [dbo].[sp_InsertDCL] 
	@Status int, 
	@CreatedOn datetime, 
	@CreatedBy uniqueidentifier, 
	@ModifiedOn datetime, 
	@ModifiedBy uniqueidentifier, 
	@CompletedOn datetime, 
	@DCLNumber nvarchar(50)
           ,@DCLDate datetime
           ,@PODetailId uniqueidentifier
           ,@Store uniqueidentifier
           ,@Quantity decimal(18,3)
           ,@Remarks nvarchar(MAX)
	
AS
BEGIN

DECLARE @id TABLE(NewValue UNIQUEIDENTIFIER);
		
		INSERT INTO DCL([Status],[CreatedOn],[CreatedBy],[ModifiedOn],[ModifiedBy],[CompletedOn],[DCLNumber],[DCLDate],[PODetailId],[Store],[Quantity],[Remarks])
		output inserted.Id into @id
		VALUES (@Status,@CreatedOn,@CreatedBy,@ModifiedOn,@ModifiedBy,@CompletedOn,@DCLNumber,@DCLDate,@PODetailId,@Store,@Quantity,@Remarks)

		select * from @id
	
END

GO
/****** Object:  StoredProcedure [dbo].[sp_InsertDO]    Script Date: 10/28/2018 7:59:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_InsertDO]
 (
 --@StoreId uniqueidentifier,
 @SOId [int],
 @SaleStationId uniqueidentifier,
 @LeadId uniqueidentifier,
 @Status [int],
 @CompletedOn [datetime],
 @ApprovedDate [datetime],
 @ApprovedBy [uniqueidentifier],
 @DODate [datetime],
 @Quantity [decimal](18, 0),
 @LiftingStartDate [datetime],
 @LiftingEndDate [datetime],
 @DeliveryDestination [varchar](100),
 @TransporterId [int],
 @DumperRate [decimal](18, 0),
 @FreightPaymentTerms [decimal](18, 0),
 @FreightPerTon [decimal](18, 0),
 @FreightTaxPerTon [decimal](18, 0),
 @FreightComissionPSL [decimal](18, 0),
 @FreightComissionAgent [decimal](18, 0),
 @Remarks [varchar](max),
 @CreatedOn [datetime],
 @CreatedBy [uniqueidentifier],
 @ModifiedOn [datetime],
 @ModifiedBy [uniqueidentifier]
 )
AS
 SET NOCOUNT ON
 SET XACT_ABORT ON
 
 BEGIN TRANSACTION

 DECLARE @DONumber [varchar](50)
 EXEC @DONumber =dbo.[fn_GenerateNextDONumber]

 INSERT INTO dbo.DO
 (
 --StoreId, 
 SaleStationId,SOId, LeadId, Status, CompletedOn, ApprovedDate, ApprovedBy, DONumber, DODate, Quantity, LiftingStartDate, LiftingEndDate, DeliveryDestination, TransporterId, DumperRate, FreightPaymentTerms, FreightPerTon, FreightTaxPerTon, FreightComissionPSL, FreightComissionAgent, Remarks, CreatedOn, CreatedBy, ModifiedOn, ModifiedBy
 )
 VALUES
 (
 --@StoreId,
 @SaleStationId,
 @SOId,
 @LeadId,
 @Status,
 @CompletedOn,
 @ApprovedDate,
 @ApprovedBy,
 @DONumber,
 @DODate,
 @Quantity,
 @LiftingStartDate,
 @LiftingEndDate,
 @DeliveryDestination,
 @TransporterId,
 @DumperRate,
 @FreightPaymentTerms,
 @FreightPerTon,
 @FreightTaxPerTon,
 @FreightComissionPSL,
 @FreightComissionAgent,
 @Remarks,
 @CreatedOn,
 @CreatedBy,
 @ModifiedOn,
 @ModifiedBy

 )
 SELECT ID, 
 --StoreId,
 SaleStationId, SOId, LeadId, Status, CompletedOn, ApprovedDate, ApprovedBy, DONumber, DODate, Quantity, LiftingStartDate, LiftingEndDate, DeliveryDestination, TransporterId, DumperRate, FreightPaymentTerms, FreightPerTon, FreightTaxPerTon, FreightComissionPSL, FreightComissionAgent, Remarks, CreatedOn, CreatedBy, ModifiedOn, ModifiedBy
 FROM dbo.DO
 WHERE (ID = SCOPE_IDENTITY())

 COMMIT

GO
/****** Object:  StoredProcedure [dbo].[sp_InsertGRN]    Script Date: 10/28/2018 7:59:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Zeeshan Shafqat>
-- Create date: <19th January 2018>
-- Description:	<Adding GRN>
-- =============================================
CREATE PROCEDURE [dbo].[sp_InsertGRN] 
	@Status int, 
	@CreatedOn datetime, 
	@CreatedBy uniqueidentifier, 
	@ModifiedOn datetime, 
	@ModifiedBy uniqueidentifier, 
	@CompletedOn datetime, 
	@GRNNumber nvarchar(50)
           ,@GRNDate datetime
           ,@PODetailId uniqueidentifier
           ,@Store uniqueidentifier
           ,@InvoiceNo nvarchar(50)
           ,@AdjPrice decimal(18,3)
           ,@Quantity decimal(18,3)
           ,@Remarks nvarchar(MAX)
	
AS
BEGIN
	DECLARE @id TABLE(NewValue UNIQUEIDENTIFIER);
		
		INSERT INTO GRN([Status],[CreatedOn],[CreatedBy],[ModifiedOn],[ModifiedBy],[CompletedOn],[GRNNumber],[GRNDate],[PODetailId],[Store],[InvoiceNo],[AdjPrice],[Quantity],[Remarks])
		output inserted.Id into @id
		VALUES (@Status,@CreatedOn,@CreatedBy,@ModifiedOn,@ModifiedBy,@CompletedOn,@GRNNumber,@GRNDate,@PODetailId,@Store,@InvoiceNo,@AdjPrice,@Quantity,@Remarks)

		select * from @id
END

GO
/****** Object:  StoredProcedure [dbo].[sp_InsertOrigin]    Script Date: 10/28/2018 7:59:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_InsertOrigin]@Name varchar(50),@Description varchar(250),@Status bit,@CreatedOn DateTime,@CreatedBy uniqueidentifier,@ModifiedOn DateTime,@ModifiedBy uniqueidentifierAS INSERT INTO Origin([Name], [Description],[Status],CreatedOn, CreatedBy,ModifiedOn,ModifiedBy)VALUES(@Name, @Description, @Status, @CreatedOn, @CreatedBy, @ModifiedOn, @ModifiedBy)
GO
/****** Object:  StoredProcedure [dbo].[sp_InsertPO]    Script Date: 10/28/2018 7:59:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Kashif Abbas>
-- Create date: <13th January2018>
-- Description:	<Adding PO information along with PO details>
-- =============================================
CREATE PROCEDURE [dbo].[sp_InsertPO] 
	@Status int, 
	@CreatedOn Datetime, 
	@CreatedBy uniqueidentifier, 
	@ModifiedOn Datetime, 
	@ModifiedBy uniqueidentifier, 
	@CompletedOn Datetime, 
	@LeadId uniqueidentifier, 
	@ApprovedDate Datetime, 
	@ApprovedBy uniqueidentifier, 
	@PONumber nvarchar(50),
	@PODate datetime, 
	@Origin int,
	@Size int, 
	@Vessel int, 
	@TargetDays int, 
	@Supplier int, 
	@TermsOfPayment nvarchar(50), 
	@BufferQuantityMax decimal(18,3), 
	@BufferQuantityMin decimal(18,3)

	

	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	--DECLARING VARIABLE TO SAVE NEWLY INSERTED POID, UNIQUEIDENTIFIER FOR PODETAILS
	DECLARE @POID TABLE(NewValue UNIQUEIDENTIFIER);

	--DECLARING TABLE VARIABLE TO SAVE NEWLY INSERTED POID, UNIQUEIDENTIFIER 
	--DECLARE @T TABLE(POID uniqueidentifier)
    
	--WHEN SET XACT_ABORT IS ON, IF A TRANSACT-SQL STATEMENT RAISES A RUN-TIME ERROR, THE ENTIRE TRANSACTION IS TERMINATED AND ROLLED BACK. 
	SET XACT_ABORT ON
	BEGIN TRAN PO_PODetails

	BEGIN TRY
	
		--ADDING DATA TO PO TABLE 
		INSERT INTO PO(	[Status],[CreatedOn],[CreatedBy],[ModifiedOn],[ModifiedBy],[CompletedOn],[LeadId],[ApprovedDate],[ApprovedBy],[PONumber],[PODate],[Origin],[Size],[Vessel],[TargetDays],[Supplier],[TermsOfPayment],[BufferQuantityMax],[BufferQuantityMin])
		output inserted.Id into @POID
		VALUES (@Status , @CreatedOn  ,@CreatedBy  ,@ModifiedOn  ,@ModifiedBy  ,@CompletedOn , @LeadId  ,@ApprovedDate , @ApprovedBy,  @PONumber ,@PODate  ,@Origin ,@Size  ,@Vessel , @TargetDays ,@Supplier  ,@TermsOfPayment,  @BufferQuantityMax , @BufferQuantityMin )

		--SETTING VALUE OF POID
		--SET @POID = (SELECT POID FROM @T)

		--INSERT INTO PODetail([POId],[CustomerId],[Quantity],[Rate],[CostPerTon],[AllowedWastage],[TargetDate],[Remarks])
		--VALUES (@POID,@CustomerId,@Quantity,@Rate,@CostPerTon,@AllowedWastage,@TargetDate,@Remarks)
		select * from @POID 
		COMMIT TRAN PO_PODetails
	END TRY
	BEGIN CATCH
		--IN CASE OF ANY ERROR ROLL BACK THE TRANSACTION
		SELECT 'Error Occured. Error is: ' + ERROR_MESSAGE()
		ROLLBACK TRAN PO_PODetails
	END CATCH
	
END

GO
/****** Object:  StoredProcedure [dbo].[sp_InsertPODetail]    Script Date: 10/28/2018 7:59:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Zeeshan Shafqat>
-- Create date: <19th January 2018>
-- Description:	<Adding PO details>
-- =============================================
CREATE PROCEDURE [dbo].[sp_InsertPODetail] 
	@POId uniqueidentifier, 
	@CustomerId uniqueidentifier, 
	@Quantity decimal(18,3), 
	@Rate decimal(18,3), 
	@CostPerTon decimal(18,3), 
	@AllowedWastage decimal(18,3), 
	@TargetDate datetime, 
	@Remarks nvarchar(MAX)
	
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @POD TABLE(NewValue UNIQUEIDENTIFIER);

	SET XACT_ABORT ON
	BEGIN TRAN PO_PODetails
		BEGIN TRY
	
		--ADDING DATA TO PODetail TABLE 
		INSERT INTO PODetail(	[POId],[CustomerId],[Quantity],[Rate],[CostPerTon],[AllowedWastage],[TargetDate],[Remarks])
		output inserted.Id into @POD
		VALUES (@POId,@CustomerId,@Quantity,@Rate,@CostPerTon,@AllowedWastage,@TargetDate,@Remarks)

		select * from @POD

		COMMIT TRAN PO_PODetails
	END TRY
	BEGIN CATCH
		--IN CASE OF ANY ERROR ROLL BACK THE TRANSACTION
		SELECT 'Error Occured. Error is: ' + ERROR_MESSAGE()
		ROLLBACK TRAN PO_PODetails
	END CATCH
	
END

GO
/****** Object:  StoredProcedure [dbo].[sp_InsertSaleStation]    Script Date: 10/28/2018 7:59:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_InsertSaleStation]
 (
 @Status [int],
 @CreatedOn [datetime],
 @CreatedBy [uniqueidentifier],
 @ModifiedOn [datetime],
 @ModifiedBy [uniqueidentifier],
 @Name [nvarchar](50),
 @OrganizationId [uniqueidentifier]
 )
AS
 SET NOCOUNT ON
 SET XACT_ABORT ON
 
 BEGIN TRANSACTION

 INSERT INTO dbo.SaleStation
 (
 Status, CreatedOn, CreatedBy, ModifiedOn, ModifiedBy, Name, OrganizationId
 )
 VALUES
 (
 @Status,
 @CreatedOn,
 @CreatedBy,
 @ModifiedOn,
 @ModifiedBy,
 @Name,
 @OrganizationId

 )

 COMMIT

GO
/****** Object:  StoredProcedure [dbo].[sp_InsertSO]    Script Date: 10/28/2018 7:59:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_InsertSO]
 @LeadId uniqueidentifier
 ,@OriginId int
 ,@SizeId int
 ,@CustomerId uniqueidentifier




 ,@TaxRateId int
 ,@TraderId int
 ,@Status int
 ,@OrderType int
 ,@SONumber nvarchar(50)
 ,@SODate Datetime
 ,@SOExpiryDate DateTime
 ,@PartyPONumber varchar(10)
 ,@PartyPODate DateTime
 ,@PartyPOExpiryDate DateTime
 ,@CreditPeriod int
 ,@Quantity decimal(18,0)
 ,@VesselId int
 ,@AgreedRate Decimal(18,0)
 ,@TraderCommision Decimal(18,0)
 
 ,@CompletedOn DateTime
 ,@ApprovedDate DateTime
 ,@ApprovedBy uniqueidentifier
 ,@CreatedOn DateTime
 ,@CreatedBy uniqueidentifier
 ,@ModifiedOn DateTime
 ,@ModifiedBy uniqueidentifier
 ,@POScannedImage varchar(50)
 ,@Remarks varchar(MAX)
 ,@BufferQuantityMax decimal(18,3) 
 ,@BufferQuantityMin decimal(18,3)

 AS
BEGIN
 -- SET NOCOUNT ON added to prevent extra result sets from
 -- interfering with SELECT statements.
 SET NOCOUNT ON;

 INSERT INTO SO( [LeadId],[OriginId],[SizeId],[CustomerId],[TaxRateId],[TraderId],[Status],[OrderType],[SONumber],[SODate],[SOExpiryDate],[PartyPONumber],[PartyPODate],[PartyPOExpiryDate],[CreditPeriod],[Quantity],[VesselId],[AgreedRate]
 ,[TraderCommision],[CompletedOn],[ApprovedDate],[ApprovedBy],[CreatedOn],[CreatedBy],[ModifiedOn],[ModifiedBy],[POScannedImage],[Remarks],[BufferQuantityMax],[BufferQuantityMin])
 VALUES( @LeadId, @OriginId, @SizeId, @CustomerId, @TaxRateId, @TraderId, @Status, @OrderType, @SONumber, @SODate, @SOExpiryDate, @PartyPONumber, @PartyPODate, @PartyPOExpiryDate, @CreditPeriod, @Quantity,@VesselId, @AgreedRate,
 @TraderCommision, @CompletedOn, @ApprovedDate, @ApprovedBy, @CreatedOn, @CreatedBy, @ModifiedOn, @ModifiedBy, @POScannedImage,@Remarks,  @BufferQuantityMax , @BufferQuantityMin)

 select SCOPE_IDENTITY()
 END
GO
/****** Object:  StoredProcedure [dbo].[sp_InsertStockMovement]    Script Date: 10/28/2018 7:59:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_InsertStockMovement] 
	@Store uniqueidentifier, 
	@CustomerId uniqueidentifier, 
	@Type int, 
	@Quantity decimal(18,0), 
	@InOut bit, 
	@Reference nvarchar(50), 
	@Date datetime, 
	@Vessel int,
    @Origin int,
	@Size int,
    @Remarks nvarchar(MAX)
	
AS
BEGIN

DECLARE @id TABLE(NewValue UNIQUEIDENTIFIER);
		
		
INSERT INTO StockMovement([StoreId],[CustomerId],[Type],[Quantity],[InOut],[Reference],[Date],[Vessel],[Origin],[Size],[Remarks])
		VALUES (@Store,@CustomerId,@Type,@Quantity,@InOut,@Reference,@Date,@Vessel,@Origin,@Size,@Remarks)
	select * from @id
END


GO
/****** Object:  StoredProcedure [dbo].[sp_InsertStoreInOut]    Script Date: 10/28/2018 7:59:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_InsertStoreInOut]
 (
 @Type [int],
 @Status [int],
 @CreatedOn [datetime],
 @CreatedBy [uniqueidentifier],
 @ModifiedOn [datetime],
 @ModifiedBy [uniqueidentifier],
 @CompletedOn [datetime],
 @LeadId [uniqueidentifier],
 @SMNumber [nvarchar](50),
 @SMDate [datetime],
 @CustomerId [uniqueidentifier],
 @Origin [int],
 @Size [int],
 @Vessel [int],
 @Quantity [decimal](18, 3),
 @FromStoreId [uniqueidentifier],
 @ToStoreId [uniqueidentifier],
 @VehicleNo [nvarchar](50),
 @BiltyNo [nvarchar](50),
 @BiltyDate [datetime],
 @RRInvoice [nvarchar](50),
 @CCMNumber [nvarchar](50),
 @Transporter [int],
 @StoreInDate [datetime],
 @StoreInQuantity [decimal](18, 3)
 
 )
AS
 SET NOCOUNT ON
 SET XACT_ABORT ON
 
 BEGIN TRANSACTION

 INSERT INTO dbo.StoreInOut
 (
 Type, Status, CreatedOn, CreatedBy, ModifiedOn, ModifiedBy, CompletedOn, LeadId, SMNumber, SMDate,CustomerId, Origin, Size, Vessel, Quantity, FromStoreId, ToStoreId, VehicleNo, BiltyNo, BiltyDate, RRInvoice, CCMNumber, Transporter, StoreInDate, StoreInQuantity
 )
 VALUES
 (
 @Type,
 @Status,
 @CreatedOn,
 @CreatedBy,
 @ModifiedOn,
 @ModifiedBy,
 @CompletedOn,
 @LeadId,
 @SMNumber,
 @SMDate,
 @CustomerId,
 @Origin,
 @Size,
 @Vessel,
 @Quantity,
 @FromStoreId,
 @ToStoreId,
 @VehicleNo,
 @BiltyNo,
 @BiltyDate,
 @RRInvoice,
 @CCMNumber,
 @Transporter,
 @StoreInDate,
 @StoreInQuantity

 )
 SELECT Id, Type, Status, CreatedOn, CreatedBy, ModifiedOn, ModifiedBy, CompletedOn, LeadId, SMNumber, SMDate, Origin, Size, Vessel, Quantity, FromStoreId, ToStoreId, VehicleNo, BiltyNo, BiltyDate, RRInvoice, CCMNumber, Transporter, StoreInDate, StoreInQuantity
 FROM dbo.StoreInOut
 WHERE (Id = SCOPE_IDENTITY())

 COMMIT

GO
/****** Object:  StoredProcedure [dbo].[sp_InsertStoreOut]    Script Date: 10/28/2018 7:59:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Zeeshan Shafqat>
-- Create date: <19th January 2018>
-- Description:	<Adding Customer>
-- =============================================
Create PROCEDURE [dbo].[sp_InsertStoreOut] 
@Type int
,@Status int
,@CreatedOn datetime
,@CreatedBy uniqueidentifier
,@ModifiedOn datetime
,@ModifiedBy uniqueidentifier
,@LeadId uniqueidentifier
,@SMNumber nvarchar(50)
,@SMDate datetime
,@Origin int
,@Size int
,@Vessel int
,@Quantity decimal(18,3)
,@FromStoreId uniqueidentifier
,@ToStoreId uniqueidentifier
,@VehicleNo nvarchar(50)
,@BiltyNo nvarchar(50)
,@BiltyDate datetime
,@RRInvoice nvarchar(50)
,@CCMNumber nvarchar(50)
,@Transporter int
AS
BEGIN
	SET NOCOUNT ON;
		SET XACT_ABORT ON
	BEGIN TRAN StoreOut
		BEGIN TRY
	
		--ADDING DATA TO TABLE 
		INSERT INTO StoreInOut ([Type],[Status],[CreatedOn],[CreatedBy],[ModifiedOn],[ModifiedBy],[LeadId],[SMNumber],[SMDate],[Origin],[Size],[Vessel],[Quantity],[FromStoreId],[ToStoreId],[VehicleNo],[BiltyNo],[BiltyDate],[RRInvoice],[CCMNumber],[Transporter])
		
		VALUES (@Type,@Status,@CreatedOn ,@CreatedBy ,@ModifiedOn ,@ModifiedBy ,@LeadId ,@SMNumber ,@SMDate ,@Origin,@Size ,@Vessel ,@Quantity ,@FromStoreId ,@ToStoreId ,@VehicleNo ,@BiltyNo ,@BiltyDate ,@RRInvoice ,@CCMNumber ,@Transporter)

		select SCOPE_IDENTITY()

		COMMIT TRAN StoreOut
	END TRY
	BEGIN CATCH
		--IN CASE OF ANY ERROR ROLL BACK THE TRANSACTION
		SELECT 'Error Occured. Error is: ' + ERROR_MESSAGE()
		ROLLBACK TRAN PO_PODetails
	END CATCH
	
END

GO
/****** Object:  StoredProcedure [dbo].[sp_InsertTaxRate]    Script Date: 10/28/2018 7:59:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_InsertTaxRate]@Name varchar(50),@Description varchar(250),@Status bit,@CreatedOn DateTime,@CreatedBy uniqueidentifier,@ModifiedOn DateTime,@ModifiedBy uniqueidentifierAS INSERT INTO TaxRate([Name], [Description],[Status],CreatedOn, CreatedBy,ModifiedOn,ModifiedBy)VALUES(@Name, @Description, @Status, @CreatedOn, @CreatedBy, @ModifiedOn, @ModifiedBy)
GO
/****** Object:  StoredProcedure [dbo].[sp_InsertTrader]    Script Date: 10/28/2018 7:59:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_InsertTrader]@Name varchar(50),@Description varchar(250),@Status bit,@CreatedOn DateTime,@CreatedBy uniqueidentifier,@ModifiedOn DateTime,@ModifiedBy uniqueidentifierAS INSERT INTO Trader([Name], [Description],[Status],CreatedOn, CreatedBy,ModifiedOn,ModifiedBy)VALUES(@Name, @Description, @Status, @CreatedOn, @CreatedBy, @ModifiedOn, @ModifiedBy)
GO
/****** Object:  StoredProcedure [dbo].[sp_InsertVessel]    Script Date: 10/28/2018 7:59:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_InsertVessel]@Name varchar(50),@Description varchar(250),@Status bit,@CreatedOn DateTime,@CreatedBy uniqueidentifier,@ModifiedOn DateTime,@ModifiedBy uniqueidentifierAS INSERT INTO Vessel([Name], [Description],[Status],CreatedOn, CreatedBy,ModifiedOn,ModifiedBy)VALUES(@Name, @Description, @Status, @CreatedOn, @CreatedBy, @ModifiedOn, @ModifiedBy)
GO
/****** Object:  StoredProcedure [dbo].[sp_UpdateCustomer]    Script Date: 10/28/2018 7:59:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Zeeshan Shafqat>
-- Create date: <19th January 2018>
-- Description:	<Adding Customer>
-- =============================================
Create PROCEDURE [dbo].[sp_UpdateCustomer] 
@Id uniqueidentifier
,@Status int
,@CreatedOn datetime
,@CreatedBy uniqueidentifier
,@ModifiedOn datetime
,@ModifiedBy uniqueidentifier
,@Lead uniqueidentifier
,@FullName nvarchar(250)
,@ShortName  nvarchar(250)
,@NTN  nvarchar(250)
,@STRN  nvarchar(250)
,@Address  nvarchar(250)
,@InvoiceAddress  nvarchar(250)
,@Email  nvarchar(250)
,@Phone  nvarchar(250)
,@ContactPerson  nvarchar(250)
,@HeadOffice  nvarchar(250)
,@Remarks  nvarchar(MAX)
	
AS
BEGIN
	SET NOCOUNT ON;
	
	SET XACT_ABORT ON
	BEGIN TRAN Customer
		BEGIN TRY
	
		UPDATE Customer
 SET [Status] = @Status,
  CreatedOn=@CreatedOn,
  CreatedBy=@CreatedBy,
  ModifiedOn=@ModifiedOn,
  ModifiedBy=@ModifiedBy,
  Lead=@Lead,
  FullName=@FullName,
  ShortName=@ShortName,
  NTN=@NTN,
  STRN=@STRN,
  [Address]=@Address,
  InvoiceAddress=@InvoiceAddress,
  Email=@Email,
Phone=@Phone,
ContactPerson=@ContactPerson,
HeadOffice=@HeadOffice,
Remarks=@Remarks
 WHERE ID = @ID


		COMMIT TRAN Customer
	END TRY
	BEGIN CATCH
		--IN CASE OF ANY ERROR ROLL BACK THE TRANSACTION
		SELECT 'Error Occured. Error is: ' + ERROR_MESSAGE()
		ROLLBACK TRAN PO_PODetails
	END CATCH
	
END

GO
/****** Object:  StoredProcedure [dbo].[sp_UpdateCustomerDestination]    Script Date: 10/28/2018 7:59:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_UpdateCustomerDestination]
 (
 @Id [int],
 @CustomerId [uniqueidentifier],
 @Name [nvarchar](250),
 @Status [int]
 )
AS
 SET NOCOUNT ON
 SET XACT_ABORT ON
 
 BEGIN TRANSACTION
 UPDATE dbo.CustomerDestination
 SET CustomerId = @CustomerId, Name = @Name, Status = @Status
 WHERE (Id = @Id OR @Id IS NULL)
 COMMIT

GO
/****** Object:  StoredProcedure [dbo].[sp_UpdateDC]    Script Date: 10/28/2018 7:59:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_UpdateDC]
 (
 @ID [int],
 @DOId [int],
 @LeadId [uniqueidentifier],
 @Store [uniqueidentifier],
 @TransporterId [int],
 @Status [int],
 @DCNumber [varchar](50),
 @DCDate [datetime],
 @Quantity [decimal](18, 0),
 @TruckNo [varchar](50),
 @BiltyNo [varchar](50),
 @SlipNo [varchar](50),
 @Weight [decimal](18, 0),
 @NetWeight [decimal](18, 0),
 @DriverName [varchar](100),
 @DriverPhone [varchar](50),
 @Remarks [varchar](250),
 @CreatedOn [datetime],
 @CreatedBy [uniqueidentifier],
 @ModifiedOn [datetime],
 @ModifiedBy [uniqueidentifier]
 )
AS
 SET NOCOUNT ON
 SET XACT_ABORT ON
 
 BEGIN TRANSACTION
 UPDATE dbo.DC
 SET DOId = @DOId, LeadId = @LeadId, Store = @Store, TransporterId = @TransporterId, Status = @Status, DCNumber = @DCNumber, DCDate = @DCDate, Quantity = @Quantity, TruckNo = @TruckNo, BiltyNo = @BiltyNo, SlipNo = @SlipNo, Weight = @Weight, NetWeight = @NetWeight, DriverName = @DriverName, DriverPhone = @DriverPhone, Remarks = @Remarks, CreatedOn = @CreatedOn, CreatedBy = @CreatedBy, ModifiedOn = @ModifiedOn, ModifiedBy = @ModifiedBy
 WHERE ID = @ID
 COMMIT
GO
/****** Object:  StoredProcedure [dbo].[sp_UpdateDCL]    Script Date: 10/28/2018 7:59:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Zeeshan>
-- Create date: <22 January 2018>
-- Description:	<update data>
-- =============================================
CREATE PROCEDURE [dbo].[sp_UpdateDCL] 
@id uniqueidentifier,
	@Status int, 
	@CreatedOn datetime, 
	@CreatedBy uniqueidentifier, 
	@ModifiedOn datetime, 
	@ModifiedBy uniqueidentifier, 
	@CompletedOn datetime, 
	@DCLNumber nvarchar(50)
           ,@DCLDate datetime
           ,@PODetailId uniqueidentifier
           ,@Store uniqueidentifier
           ,@Quantity decimal(18,3)
           ,@Remarks nvarchar(MAX)

	

	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;
	
	BEGIN TRY
	
		--Update DATA TO TABLE 
		UPDATE [DCL] set 
		[Status]=@Status,
		[CreatedOn]=@CreatedOn,
		[CreatedBy]=@CreatedBy,
		[ModifiedOn]=@ModifiedOn,
		[ModifiedBy]=@ModifiedBy,
		[CompletedOn]=@CompletedOn,
		[DCLNumber]=@DCLNumber,
		[DCLDate]=@DCLDate,
		[PODetailId]=@PODetailId,
		[Store]=@Store,
		[Quantity]=@Quantity,
		[Remarks]=@Remarks
		
		Where [id]=@id
	END TRY
	BEGIN CATCH
		--IN CASE OF ANY ERROR ROLL BACK THE TRANSACTION
		SELECT 'Error Occured. Error is: ' + ERROR_MESSAGE()
	END CATCH
	
END

GO
/****** Object:  StoredProcedure [dbo].[sp_UpdateDO]    Script Date: 10/28/2018 7:59:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_UpdateDO]
 (
 @ID [int],
 --@StoreId uniqueidentifier,
 @SaleStationId [uniqueidentifier],
 @SOId [int],
 @LeadId [uniqueidentifier],
 @Status [int],
 @CompletedOn [datetime],
 @ApprovedDate [datetime],
 @ApprovedBy [uniqueidentifier],
 @DONumber [varchar](50),
 @DODate [datetime],
 @Quantity [decimal](18, 0),
 @LiftingStartDate [datetime],
 @LiftingEndDate [datetime],
 @DeliveryDestination [varchar](100),
 @TransporterId [int],
 @DumperRate [decimal](18, 0),
 @FreightPaymentTerms [decimal](18, 0),
 @FreightPerTon [decimal](18, 0),
 @FreightTaxPerTon [decimal](18, 0),
 @FreightComissionPSL [decimal](18, 0),
 @FreightComissionAgent [decimal](18, 0),
 @Remarks [varchar](max),
 @CreatedOn [datetime],
 @CreatedBy [uniqueidentifier],
 @ModifiedOn [datetime],
 @ModifiedBy [uniqueidentifier]
 )
AS
 SET NOCOUNT ON
 SET XACT_ABORT ON
 
 BEGIN TRANSACTION
 UPDATE dbo.DO
 SET SaleStationId = @SaleStationId, 
 --StoreId =@StoreId , 
 SOId = @SOId, LeadId = @LeadId, Status = @Status, CompletedOn = @CompletedOn, ApprovedDate = @ApprovedDate, ApprovedBy = @ApprovedBy, DONumber = @DONumber, DODate = @DODate, Quantity = @Quantity, LiftingStartDate = @LiftingStartDate, LiftingEndDate = @LiftingEndDate, DeliveryDestination = @DeliveryDestination, TransporterId = @TransporterId, DumperRate = @DumperRate, FreightPaymentTerms = @FreightPaymentTerms, FreightPerTon = @FreightPerTon, FreightTaxPerTon = @FreightTaxPerTon, FreightComissionPSL = @FreightComissionPSL, FreightComissionAgent = @FreightComissionAgent, Remarks = @Remarks, CreatedOn = @CreatedOn, CreatedBy = @CreatedBy, ModifiedOn = @ModifiedOn, ModifiedBy = @ModifiedBy
 WHERE (ID = @ID OR @ID IS NULL)

 SELECT ID,
 --StoreId, 
 SaleStationId, SOId, LeadId, Status, CompletedOn, ApprovedDate, ApprovedBy, DONumber, DODate, Quantity, LiftingStartDate, LiftingEndDate, DeliveryDestination, TransporterId, DumperRate, FreightPaymentTerms, FreightPerTon, FreightTaxPerTon, FreightComissionPSL, FreightComissionAgent, Remarks, CreatedOn, CreatedBy, ModifiedOn, ModifiedBy
 FROM dbo.DO
 WHERE (ID = @ID OR @ID IS NULL)

 COMMIT
GO
/****** Object:  StoredProcedure [dbo].[sp_UpdateGRN]    Script Date: 10/28/2018 7:59:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Zeeshan>
-- Create date: <22 January 2018>
-- Description:	<update data>
-- =============================================
Create PROCEDURE [dbo].[sp_UpdateGRN] 
@id uniqueidentifier,
	@Status int, 
	@CreatedOn datetime, 
	@CreatedBy uniqueidentifier, 
	@ModifiedOn datetime, 
	@ModifiedBy uniqueidentifier, 
	@CompletedOn datetime, 
	@GRNNumber nvarchar(50)
           ,@GRNDate datetime
           ,@PODetailId uniqueidentifier
           ,@Store uniqueidentifier
           ,@InvoiceNo nvarchar(50)
           ,@AdjPrice decimal(18,3)
           ,@Quantity decimal(18,3)
           ,@Remarks nvarchar(MAX)

	

	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;
	
	BEGIN TRY
	
		--Update DATA TO TABLE 
		UPDATE [GRN] set 
		[Status]=@Status,
		[CreatedOn]=@CreatedOn,
		[CreatedBy]=@CreatedBy,
		[ModifiedOn]=@ModifiedOn,
		[ModifiedBy]=@ModifiedBy,
		[CompletedOn]=@CompletedOn,
		[GRNNumber]=@GRNNumber,
		[GRNDate]=@GRNDate,
		[PODetailId]=@PODetailId,
		[Store]=@Store,
		[InvoiceNo]=@InvoiceNo,
		[AdjPrice]=@AdjPrice,
		[Quantity]=@Quantity,
		[Remarks]=@Remarks
		
		
		Where [id]=@id
	END TRY
	BEGIN CATCH
		--IN CASE OF ANY ERROR ROLL BACK THE TRANSACTION
		SELECT 'Error Occured. Error is: ' + ERROR_MESSAGE()
	END CATCH
	
END

GO
/****** Object:  StoredProcedure [dbo].[sp_UpdatePO]    Script Date: 10/28/2018 7:59:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Zeeshan>
-- Create date: <22 January 2018>
-- Description:	<update data>
-- =============================================
CREATE PROCEDURE [dbo].[sp_UpdatePO] 
@id uniqueidentifier,
	@Status int, 
	@CreatedOn Datetime, 
	@CreatedBy uniqueidentifier, 
	@ModifiedOn Datetime, 
	@ModifiedBy uniqueidentifier, 
	@CompletedOn Datetime, 
	@LeadId uniqueidentifier, 
	@ApprovedDate Datetime, 
	@ApprovedBy uniqueidentifier, 
	@PONumber nvarchar(50),
	@PODate datetime, 
	@Origin int,
	@Size int, 
	@Vessel int, 
	@TargetDays int, 
	@Supplier int, 
	@TermsOfPayment nvarchar(50), 
	@BufferQuantityMax decimal(18,3), 
	@BufferQuantityMin decimal(18,3)

	

	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;
	
	BEGIN TRY
	
		--Update DATA TO TABLE 
		UPDATE PO set 
		[Status]=@Status,
		[CreatedOn]=@CreatedOn,
		[CreatedBy]=@CreatedBy,
		[ModifiedOn]=@ModifiedOn,
		[ModifiedBy]=@ModifiedBy,
		[CompletedOn]=@CompletedOn,
		[LeadId]=@LeadId,
		[ApprovedDate]=@ApprovedDate,
		[ApprovedBy]=@ApprovedBy,
		[PONumber]=@PONumber,
		[PODate]=@PODate,
		[Origin]=@Origin,
		[Size]=@Size,
		[Vessel]=@Vessel,
		[TargetDays]=@TargetDays,
		[Supplier]=@Supplier,
		[TermsOfPayment]=@TermsOfPayment,
		[BufferQuantityMax]=@BufferQuantityMax,
		[BufferQuantityMin]=@BufferQuantityMin
		
		Where [id]=@id
	END TRY
	BEGIN CATCH
		--IN CASE OF ANY ERROR ROLL BACK THE TRANSACTION
		SELECT 'Error Occured. Error is: ' + ERROR_MESSAGE()
	END CATCH
	
END

GO
/****** Object:  StoredProcedure [dbo].[sp_UpdatePODetail]    Script Date: 10/28/2018 7:59:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Zeeshan>
-- Create date: <22 January 2018>
-- Description:	<update data>
-- =============================================
Create PROCEDURE [dbo].[sp_UpdatePODetail] 
@id uniqueidentifier,
	@POId uniqueidentifier, 
	@CustomerId uniqueidentifier, 
	@Quantity decimal(18,3), 
	@Rate decimal(18,3), 
	@CostPerTon decimal(18,3), 
	@AllowedWastage decimal(18,3), 
	@TargetDate datetime, 
	@Remarks nvarchar(MAX)

	

	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;
	
	BEGIN TRY
	
		--Update DATA TO TABLE 
		UPDATE [PODetail] set 
		[POId]=@POId,
		[CustomerId]=@CustomerId,
		[Quantity]=@Quantity,
		[Rate]=@Rate,
		[CostPerTon]=@CostPerTon,
		[AllowedWastage]=@AllowedWastage,
		[TargetDate]=@TargetDate,
		[Remarks]=@Remarks
		
		
		Where [id]=@id
	END TRY
	BEGIN CATCH
		--IN CASE OF ANY ERROR ROLL BACK THE TRANSACTION
		SELECT 'Error Occured. Error is: ' + ERROR_MESSAGE()
	END CATCH
	
END

GO
/****** Object:  StoredProcedure [dbo].[sp_UpdateSaleStation]    Script Date: 10/28/2018 7:59:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_UpdateSaleStation]
 (
 @Id [uniqueidentifier],
 @Status [int],
 @CreatedOn [datetime],
 @CreatedBy [uniqueidentifier],
 @ModifiedOn [datetime],
 @ModifiedBy [uniqueidentifier],
 @Name [nvarchar](50),
 @OrganizationId [uniqueidentifier]
 )
AS
 SET NOCOUNT ON
 SET XACT_ABORT ON
 
 BEGIN TRANSACTION
 UPDATE dbo.SaleStation
 SET Id = @Id, Status = @Status, CreatedOn = @CreatedOn, CreatedBy = @CreatedBy, ModifiedOn = @ModifiedOn, ModifiedBy = @ModifiedBy, Name = @Name, OrganizationId = @OrganizationId
 WHERE (Id = @Id OR @Id IS NULL)
 COMMIT

GO
/****** Object:  StoredProcedure [dbo].[sp_UpdateSO]    Script Date: 10/28/2018 7:59:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_UpdateSO]
@Id int
 ,@LeadId uniqueidentifier
 ,@OriginId int
 ,@SizeId int
 ,@VesselId int
 ,@CustomerId uniqueidentifier
 ,@TaxRateId int
 ,@TraderId int
 ,@Status int
 ,@SONumber nvarchar(50)
 ,@OrderType int
 ,@SODate Datetime
 ,@SOExpiryDate DateTime
 ,@PartyPONumber varchar(10)
 ,@PartyPODate DateTime
 ,@PartyPOExpiryDate DateTime
 ,@CreditPeriod int
 ,@Quantity decimal(18,0)
 ,@AgreedRate Decimal(18,0)
 ,@TraderCommision Decimal(18,0)
 ,@CompletedOn DateTime
 ,@ApprovedDate DateTime
 ,@ApprovedBy uniqueidentifier
 ,@CreatedOn DateTime
 ,@CreatedBy uniqueidentifier
 ,@ModifiedOn DateTime
 ,@ModifiedBy uniqueidentifier
 ,@POScannedImage varchar(50)
 ,@Remarks varchar(MAX)
 ,@BufferQuantityMax decimal(18,3) 
 ,@BufferQuantityMin decimal(18,3)
 AS
BEGIN
 -- SET NOCOUNT ON added to prevent extra result sets from
 -- interfering with SELECT statements.
 SET NOCOUNT ON;
 UPDATE SO
 SET [LeadId] = @LeadId,
 [OriginId] = @OriginId,
 [SizeId] = @SizeId,
 [VesselId] =@VesselId,
 [CustomerId] = @CustomerId, 
 [TaxRateId] = @TaxRateId,
 [TraderId] = @TraderId,
 [Status] = @Status,
 [SONumber] = @SONumber,
 [OrderType] = @OrderType,
 [SODate] = @SODate,
 [SOExpiryDate] = @SOExpiryDate,
 [PartyPONumber] = @PartyPONumber,
 [PartyPODate] = @PartyPODate,
 [PartyPOExpiryDate] = PartyPOExpiryDate,
 [CreditPeriod] = @CreditPeriod,
 [Quantity] = @Quantity,
 [AgreedRate] = @AgreedRate,
 [TraderCommision] = @TraderCommision,
 [CompletedOn] = @CompletedOn,
 [ApprovedDate] = @ApprovedDate,
 [ApprovedBy] = @ApprovedBy,
 [ModifiedOn] = @ModifiedOn,
 [ModifiedBy] = @ModifiedBy,
 [CreatedOn] = @CreatedOn,
 [CreatedBy] = @CreatedBy,
 [POScannedImage] = @POScannedImage,
 [Remarks] = @Remarks,
 [BufferQuantityMax]=@BufferQuantityMax,
 [BufferQuantityMin]=@BufferQuantityMin
 WHERE Id = @Id

 END
GO
/****** Object:  StoredProcedure [dbo].[sp_UpdateStoreInOut]    Script Date: 10/28/2018 7:59:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_UpdateStoreInOut]
 (
 @Id [int],
 @Type [int],
 @Status [int],
 @CreatedOn [datetime],
 @CreatedBy [uniqueidentifier],
 @ModifiedOn [datetime],
 @ModifiedBy [uniqueidentifier],
 @CompletedOn [datetime],
 @LeadId [uniqueidentifier],
 @SMNumber [nvarchar](50),
 @SMDate [datetime],
 @CustomerId uniqueidentifier,
 @Origin [int],
 @Size [int],
 @Vessel [int],
 @Quantity [decimal](18, 3),
 @FromStoreId [uniqueidentifier],
 @ToStoreId [uniqueidentifier],
 @VehicleNo [nvarchar](50),
 @BiltyNo [nvarchar](50),
 @BiltyDate [datetime],
 @RRInvoice [nvarchar](50),
 @CCMNumber [nvarchar](50),
 @Transporter [int],
 @StoreInDate [datetime],
 @StoreInQuantity [decimal](18, 3)
 )
AS
 SET NOCOUNT ON
 SET XACT_ABORT ON
 
 BEGIN TRANSACTION
 UPDATE dbo.StoreInOut
 SET Type = @Type, Status = @Status, CreatedOn = @CreatedOn, CreatedBy = @CreatedBy, ModifiedOn = @ModifiedOn, ModifiedBy = @ModifiedBy, CompletedOn = @CompletedOn, LeadId = @LeadId, SMNumber = @SMNumber, SMDate = @SMDate,CustomerId=@CustomerId, Origin = @Origin, Size = @Size, Vessel = @Vessel, Quantity = @Quantity, FromStoreId = @FromStoreId, ToStoreId = @ToStoreId, VehicleNo = @VehicleNo, BiltyNo = @BiltyNo, BiltyDate = @BiltyDate, RRInvoice = @RRInvoice, CCMNumber = @CCMNumber, Transporter = @Transporter, StoreInDate = @StoreInDate, StoreInQuantity = @StoreInQuantity
 WHERE (Id = @Id OR @Id IS NULL)
 COMMIT

GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'0 = Canceled, 1 = Created, 2 = Inprocess, 3 = Completed ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DO', @level2type=N'COLUMN',@level2name=N'Status'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1 = Commercial, 2 = LC, 3=Loan' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SO', @level2type=N'COLUMN',@level2name=N'OrderType'
GO
USE [master]
GO
ALTER DATABASE [Petrocoal] SET  READ_WRITE 
GO
