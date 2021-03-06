/*    ==Scripting Parameters==

    Source Database Engine Edition : Microsoft Azure SQL Database Edition
    Source Database Engine Type : Microsoft Azure SQL Database

    Target Server Version : SQL Server 2017
    Target Database Engine Edition : Microsoft SQL Server Standard Edition
    Target Database Engine Type : Standalone SQL Server
*/
USE [master]
GO
/****** Object:  Database [minismart]    Script Date: 9/2/2017 12:36:42 PM ******/
CREATE DATABASE [minismart]
GO
ALTER DATABASE [minismart] SET COMPATIBILITY_LEVEL = 130
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [minismart].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [minismart] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [minismart] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [minismart] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [minismart] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [minismart] SET ARITHABORT OFF 
GO
ALTER DATABASE [minismart] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [minismart] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [minismart] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [minismart] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [minismart] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [minismart] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [minismart] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [minismart] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [minismart] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [minismart] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [minismart] SET ALLOW_SNAPSHOT_ISOLATION ON 
GO
ALTER DATABASE [minismart] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [minismart] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [minismart] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [minismart] SET  MULTI_USER 
GO
ALTER DATABASE [minismart] SET DB_CHAINING OFF 
GO
ALTER DATABASE [minismart] SET ENCRYPTION ON
GO
ALTER DATABASE [minismart] SET QUERY_STORE = ON
GO
ALTER DATABASE [minismart] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 100, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO)
GO
USE [minismart]
GO
ALTER DATABASE SCOPED CONFIGURATION SET IDENTITY_CACHE = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET LEGACY_CARDINALITY_ESTIMATION = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET MAXDOP = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET PARAMETER_SNIFFING = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET QUERY_OPTIMIZER_HOTFIXES = PRIMARY;
GO
USE [minismart]
GO
/****** Object:  Table [dbo].[tab_CA_CRITICAL_DTC_ADD]    Script Date: 9/2/2017 12:36:43 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tab_CA_CRITICAL_DTC_ADD](
	[crt_DTC_PK] [int] IDENTITY(1,1) NOT NULL,
	[crt_DTC_CODE] [nvarchar](50) NOT NULL,
	[crt_DTC_STATUS] [bit] NOT NULL,
	[crt_DTC_THRESHOLD] [nvarchar](50) NULL,
	[crt_DTC_TYPE] [nvarchar](150) NULL,
	[crt_ECU_NAME] [nvarchar](100) NULL,
	[crt_PLATFORM_NAME] [nvarchar](100) NULL,
	[crt_TYPE_OF_OPERATION] [nvarchar](50) NULL,
	[crt_INVERT_OPERATON] [bit] NULL,
	[crt_TYPE_OF_LOGIC] [nvarchar](10) NULL,
	[crt_ISCRITICAL] [bit] NULL,
	[crt_DTC_CREATED_ON] [datetime] NOT NULL,
	[crt_DTC_INACTIVE_ON] [datetime] NULL,
 CONSTRAINT [PK_tab_CA_CRITICAL_DTC_ADD] PRIMARY KEY CLUSTERED 
(
	[crt_DTC_PK] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)
GO
/****** Object:  Table [dbo].[tab_CA_EMS_DTC_INFO]    Script Date: 9/2/2017 12:36:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tab_CA_EMS_DTC_INFO](
	[dtc_ID] [int] IDENTITY(1,1) NOT NULL,
	[freeze_MASTER_ID] [int] NOT NULL,
	[freeze_DTC_CODE] [nvarchar](10) NOT NULL,
	[freeze_DTC_DESCRIPTION] [nvarchar](250) NOT NULL,
	[freeze_DTC_STATUS] [nvarchar](15) NOT NULL,
 CONSTRAINT [PK_tab_CA_EMS_DTC_INFO] PRIMARY KEY CLUSTERED 
(
	[dtc_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)
GO
/****** Object:  Table [dbo].[tab_CA_EMS_DTC_LOGIC_ADD]    Script Date: 9/2/2017 12:36:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tab_CA_EMS_DTC_LOGIC_ADD](
	[dtc_LOGIC_ADD_PK] [int] IDENTITY(1,1) NOT NULL,
	[dtc_DTC_CODE] [nvarchar](50) NOT NULL,
	[dtc_LOGIC] [nvarchar](10) NOT NULL,
	[dtc_LOGIC_THRESHOLD] [float] NULL,
	[dtc_LOGIC_RESULT] [bit] NULL,
 CONSTRAINT [PK_tab_CA_EMS_DTC_LOGIC_ADD] PRIMARY KEY CLUSTERED 
(
	[dtc_LOGIC_ADD_PK] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)
GO
/****** Object:  Table [dbo].[tab_CA_EMS_DTC_LOGIC_BUILD]    Script Date: 9/2/2017 12:36:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tab_CA_EMS_DTC_LOGIC_BUILD](
	[dtc_LOGIC_BUILD_PK] [int] IDENTITY(1,1) NOT NULL,
	[dtc_DTC_CODE_A] [nvarchar](50) NOT NULL,
	[dtc_DTC_CODE_A_INVERT] [bit] NULL,
	[dtc_DTC_CODE_B] [nvarchar](50) NULL,
	[dtc_DTC_CODE_B_INVERT] [bit] NULL,
	[dtc_DTC_CODE_C] [nvarchar](50) NULL,
	[dtc_DTC_CODE_C_INVERT] [bit] NULL,
	[dtc_DTC_CODE_D] [nvarchar](50) NULL,
	[dtc_DTC_CODE_D_INVERT] [bit] NULL,
	[dtc_LOGIC_OPERATION] [nvarchar](50) NOT NULL,
	[dtc_LOGIC_CONCLUSION] [nvarchar](250) NOT NULL,
	[dtc_LOGIC_STATUS] [bit] NULL,
	[dtc_LOGIC_SMILEY] [nvarchar](250) NULL,
	[dtc_DTC_LOGIC_A] [nvarchar](10) NULL,
	[dtc_DTC_LOGIC_B] [nvarchar](10) NULL,
	[dtc_DTC_LOGIC_C] [nvarchar](10) NULL,
	[dtc_DTC_LOGIC_D] [nvarchar](10) NULL,
	[dtc_DTC_THRESHOLD_A] [nvarchar](10) NULL,
	[dtc_DTC_THRESHOLD_B] [nvarchar](10) NULL,
	[dtc_DTC_THRESHOLD_C] [nvarchar](10) NULL,
	[dtc_DTC_THRESHOLD_D] [nvarchar](10) NULL,
 CONSTRAINT [PK_tab_CA_EMS_DTC_LOGIC_BUILD] PRIMARY KEY CLUSTERED 
(
	[dtc_LOGIC_BUILD_PK] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)
GO
/****** Object:  Table [dbo].[tab_CA_EMS_FREEZEFRAME_DATA]    Script Date: 9/2/2017 12:36:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tab_CA_EMS_FREEZEFRAME_DATA](
	[freeze_ID] [int] IDENTITY(1,1) NOT NULL,
	[master_ID] [int] NOT NULL,
	[dtc_INFO_ID] [int] NOT NULL,
	[freeze_SIGNAL] [nvarchar](250) NOT NULL,
	[freeze_VALUE] [nvarchar](75) NOT NULL,
 CONSTRAINT [PK_tab_CA_EMS_FREEZEFRAME_DATA] PRIMARY KEY CLUSTERED 
(
	[freeze_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)
GO
/****** Object:  Table [dbo].[tab_CA_EMS_MASTER_INFO]    Script Date: 9/2/2017 12:36:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tab_CA_EMS_MASTER_INFO](
	[freeze_MASTER_ID] [int] IDENTITY(1,1) NOT NULL,
	[freeze_VIN_NUMBER] [nvarchar](25) NOT NULL,
	[freeze_MOBILE_NUMBER] [nvarchar](10) NOT NULL,
	[freeze_LATITUDE] [nvarchar](50) NOT NULL,
	[freeze_LONGITUDE] [nvarchar](50) NOT NULL,
	[freeze_TIMESTAMP] [datetime] NOT NULL,
	[freeze_UPDATEDON] [datetime] NOT NULL,
	[freeze_ECUNAME] [nvarchar](500) NULL,
	[freeze_VEHICLENAME] [nvarchar](500) NULL,
	[freeze_LOGINID] [nvarchar](50) NULL,
	[freeze_LOGINNAME] [nvarchar](50) NULL,
	[freeze_MOBILE_MAC_ADDRESS] [nvarchar](50) NULL,
	[freeze_VCIID] [nvarchar](50) NULL,
	[freeze_APP_VERSION] [nchar](10) NULL,
	[freeze_DEALER_NAME] [nvarchar](500) NULL,
	[freeze_AREA] [nvarchar](500) NULL,
	[freeze_LOCATION] [nvarchar](500) NULL,
	[freeze_SOURCE] [nvarchar](50) NULL,
	[freeze_ODOValue] [nvarchar](50) NULL,
	[freeze_PUSH_NOTIFY] [bit] NULL,
	[freeze_NOTIFY_COUNTER] [int] NULL,
 CONSTRAINT [PK_tab_CA_EMS_MASTER_INFO] PRIMARY KEY CLUSTERED 
(
	[freeze_MASTER_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)
GO
/****** Object:  Table [dbo].[tab_m_ADMIN_USER]    Script Date: 9/2/2017 12:36:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tab_m_ADMIN_USER](
	[user_ID_PK] [int] IDENTITY(1,1) NOT NULL,
	[user_FIRST_NAME] [nvarchar](50) NOT NULL,
	[user_LAST_NAME] [nvarchar](25) NULL,
	[user_GENDER] [bit] NOT NULL,
	[user_ROLE] [smallint] NOT NULL,
	[user_EMAIL_ID] [nvarchar](50) NOT NULL,
	[user_CONTACT_NO] [nvarchar](15) NOT NULL,
	[user_ACCESS_CODE] [nvarchar](15) NOT NULL,
	[user_PASSWORD] [nvarchar](100) NOT NULL,
	[user_ACTIVE_STATUS] [bit] NULL,
 CONSTRAINT [PK_tab_ADMIN_USER] PRIMARY KEY CLUSTERED 
(
	[user_ID_PK] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)
GO
/****** Object:  Table [dbo].[tab_m_DEVICE_VIN_MAPPING]    Script Date: 9/2/2017 12:36:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tab_m_DEVICE_VIN_MAPPING](
	[mapping_ID] [int] IDENTITY(1,1) NOT NULL,
	[VIN_NUMBER] [nvarchar](50) NOT NULL,
	[device_TOKEN] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_tab_m_DEVICE_VIN_MAPPING] PRIMARY KEY CLUSTERED 
(
	[mapping_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)
GO
/****** Object:  Table [dbo].[tab_m_VCI_REGISTERATION]    Script Date: 9/2/2017 12:36:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tab_m_VCI_REGISTERATION](
	[register_ID_PK] [int] IDENTITY(1,1) NOT NULL,
	[register_MOBILE_DEVICE_ID] [nvarchar](50) NOT NULL,
	[register_VCI_ID] [nvarchar](20) NOT NULL,
	[register_ID] [nvarchar](20) NOT NULL,
	[register_COTEK_ID] [nvarchar](20) NOT NULL,
	[register_FIRST_NAME] [nvarchar](50) NOT NULL,
	[register_LAST_NAME] [nvarchar](50) NOT NULL,
	[register_IS_ACTIVE] [nvarchar](20) NOT NULL,
	[register_USER_TYPE] [nvarchar](20) NOT NULL,
	[register_SKILL_ID] [nvarchar](20) NOT NULL,
	[register_SKILL_NAME] [nvarchar](150) NOT NULL,
	[register_USER_TYPE_DESC] [nvarchar](250) NOT NULL,
	[register_PASSWORD_CHANGED] [nvarchar](20) NOT NULL,
	[register_AREA_NAME] [nvarchar](100) NOT NULL,
	[register_ZONE] [nvarchar](50) NOT NULL,
	[register_CODE] [nvarchar](20) NOT NULL,
	[register_LOCATION_NAME] [nvarchar](300) NOT NULL,
	[register_CHANNEL_NO] [nvarchar](50) NOT NULL,
	[register_IS_LDAP__AUTHENDICATED] [nvarchar](20) NULL,
	[register_LM_COMPANY_NAME] [nvarchar](250) NULL,
	[register_LM_LOCATION] [nvarchar](50) NULL,
	[register_LM_OWNER_NAME] [nvarchar](50) NULL,
	[register_LM_MOBILE_NO] [nvarchar](20) NULL,
	[register_DATE] [datetime] NOT NULL,
	[register_DEALER_NAME] [nvarchar](max) NULL,
	[register_APP_VERSION] [nvarchar](10) NULL,
	[register_SERIAL_NUMBER] [nvarchar](50) NULL,
	[register_DEVICE_TOKEN] [nvarchar](max) NULL,
 CONSTRAINT [PK_tab_m_VCI_REGISTERATION] PRIMARY KEY CLUSTERED 
(
	[register_ID_PK] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)
GO
ALTER TABLE [dbo].[tab_m_VCI_REGISTERATION] ADD  CONSTRAINT [DF_tab_m_VCI_REGISTERATION_register_IS_ACTIVE]  DEFAULT ((1)) FOR [register_IS_ACTIVE]
GO
/****** Object:  StoredProcedure [dbo].[Get_Analysis_Value]    Script Date: 9/2/2017 12:36:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/****** Object:  Table [dbo].[tab_m_VIN_SCORE]    Script Date: 9/2/2017 14:36:46 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tab_m_VIN_SCORE](
	[Leader_ID_PK] [bigint] IDENTITY(1,1) NOT NULL,
	[Leader_Name] [nvarchar](100) NULL,
	[Leader_VIN_NUMBER] [nvarchar](25) NOT NULL,
	[Leader_TIMESTAMP] [datetime] NOT NULL,
	[Leader_SCORE] [decimal](18, 0) NOT NULL,
 CONSTRAINT [PK_tab_m_VIN_SCORE] PRIMARY KEY CLUSTERED 
(
	[Leader_ID_PK] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)
GO
-- =============================================
-- Author:		<Author, Kannan>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- exec Get_Analysis_Value 1
-- =============================================
CREATE PROCEDURE [dbo].[Get_Analysis_Value] @ID INT
AS
BEGIN	

		DECLARE @CurrentReturnTBL AS TABLE (DTCCODE VARCHAR(50), CurrentODOValue DECIMAL(18,3),  previousODOValue DECIMAL(18,3), CurrentOC DECIMAL(18,3), PreviousOC DECIMAL(18,3), Result DECIMAL(18,3), Invert bit)			
		DECLARE @previousID INT, @CurrentODOValue DECIMAL(18,3), @previousODOValue DECIMAL(18,3), @DTCID DECIMAL(18,3), @DTCCODE VARCHAR(50), @CurrentOC DECIMAL(18,3), @PreviousOC DECIMAL(18,3), @Result DECIMAL(18,3), @OC DECIMAL(18,3), @ODO DECIMAL(18,3),@DTCLogic VARCHAR(50), @DTCInvertCode VARCHAR(50), @status bit, @Count int
		DECLARE @CurrentTBL AS TABLE(DTCID INT, DTCCODE VARCHAR(50))		
		
		SET @CurrentODOValue =(SELECT freeze_ODOValue FROM tab_CA_EMS_MASTER_INFO WHERE freeze_MASTER_ID = @ID)
		IF(@CurrentODOValue IS NULL)
			BEGIN
				SET @CurrentODOValue = 0
			END
				
		SET @previousID = (SELECT top 1 freeze_MASTER_ID FROM(SELECT top 2 * FROM tab_CA_EMS_MASTER_INFO WHERE freeze_VIN_NUMBER in (SELECT freeze_VIN_NUMBER FROM tab_CA_EMS_MASTER_INFO WHERE freeze_MASTER_ID=@ID) and freeze_MASTER_ID!>@ID order by freeze_MASTER_ID desc) a order by freeze_MASTER_ID asc)
		
		SET @previousODOValue=(SELECT freeze_ODOValue FROM tab_CA_EMS_MASTER_INFO WHERE freeze_MASTER_ID = @previousID)		
		IF(@previousODOValue IS NULL)
			BEGIN
				SET	@previousODOValue = 0
			END
		
		SET @Count =(SELECT count(freeze_VIN_NUMBER) FROM tab_CA_EMS_MASTER_INFO WHERE freeze_VIN_NUMBER in (SELECT freeze_VIN_NUMBER FROM tab_CA_EMS_MASTER_INFO WHERE freeze_MASTER_ID=@ID )and freeze_MASTER_ID!>@ID)
		IF(@Count=1)
		BEGIN
			SET @previousODOValue = 0
		END
		
		SET @ODO = (@CurrentODOValue-@previousODOValue)		 

		IF(@ODO>0)
		BEGIN

		INSERT INTO @CurrentTBL(DTCID, DTCCODE)
		SELECT dtc_ID, freeze_DTC_CODE FROM tab_CA_EMS_DTC_INFO WHERE freeze_MASTER_ID=@ID		
							
		WHILE EXISTS (SELECT DTCID FROM @CurrentTBL)
		
			BEGIN
				SELECT TOP 1 @DTCID=DTCID, @DTCCODE=DTCCODE  FROM @CurrentTBL
				BEGIN				

					SET @DTCInvertCode=(SELECT distinct dtc_DTC_CODE_A FROM tab_CA_EMS_DTC_LOGIC_BUILD 
					WHERE dtc_DTC_CODE_A=@DTCCODE and dtc_DTC_CODE_A_INVERT=1)

					IF(@DTCInvertCode is not null)
						BEGIN
							SET @status=1
						END
					ELSE
						BEGIN 
							SET @status=0
						END													
													
					 SET @CurrentOC = (SELECT freeze_VALUE FROM tab_CA_EMS_FREEZEFRAME_DATA WHERE dtc_INFO_ID=@DTCID and freeze_SIGNAL='OccurrenceCounter')
					 IF(@CurrentOC IS NULL)
						BEGIN
							SET @CurrentOC = 0
						END						
																															
					 SET @PreviousOC = (SELECT freeze_VALUE FROM tab_CA_EMS_FREEZEFRAME_DATA WHERE dtc_INFO_ID IN(SELECT dtc_ID FROM tab_CA_EMS_DTC_INFO WHERE freeze_MASTER_ID = @previousID and freeze_DTC_CODE=@DTCCODE) and freeze_SIGNAL='OccurrenceCounter')
					 IF(@PreviousOC IS NULL)
						BEGIN
							SET @PreviousOC = 0
						END	

						SET @Count =(SELECT count(freeze_VIN_NUMBER) FROM tab_CA_EMS_MASTER_INFO WHERE freeze_VIN_NUMBER in (SELECT freeze_VIN_NUMBER FROM tab_CA_EMS_MASTER_INFO WHERE freeze_MASTER_ID=@ID )and freeze_MASTER_ID!>@ID)
						IF(@Count=1)
						BEGIN
							SET @PreviousOC = 0
						END

						SET @OC = (@CurrentOC-@PreviousOC)
						IF(@OC>0 AND @ODO>0)
							BEGIN
								SET @Result= round((@CurrentOC-@PreviousOC)/(@CurrentODOValue-@previousODOValue),3,1)
							END 
						ELSE
							BEGIN
								SET @Result= 0
							END

				END
				INSERT INTO @CurrentReturnTBL values(@DTCCODE,@CurrentODOValue,@previousODOValue, @CurrentOC, @PreviousOC, @Result, @status)
				DELETE @CurrentTBL WHERE DTCID=@DTCID
			END

			END

			SELECT DTCCODE, Result FROM @CurrentReturnTBL	
	
END
GO
/****** Object:  StoredProcedure [dbo].[Get_Invert_Value]    Script Date: 9/2/2017 12:36:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Get_Invert_Value] @ID INT
AS
BEGIN	

		DECLARE @CurrentReturnTBL AS TABLE (BuildID int, Opreator varchar(50), DTCCODE_A varchar(50), DTCLOGIC_A varchar(10), DTCTHRESHOLD_A  varchar(10), DTCCODE_A_INVERT bit, DTCCODE_B varchar(50), DTCLOGIC_B varchar(10), DTCTHRESHOLD_B  varchar(10),DTCCODE_B_INVERT bit, DTCCODE_C varchar(50), DTCLOGIC_C varchar(10), DTCTHRESHOLD_C  varchar(10), DTCCODE_C_INVERT bit, DTCCODE_D varchar(50), DTCLOGIC_D varchar(10), DTCTHRESHOLD_D  varchar(10), DTCCODE_D_INVERT bit)			
		DECLARE @DTCID decimal(18,2), @DTCCODE varchar(50), @DTCBuildCode varchar(50)
		DECLARE @CurrentTBL AS TABLE(DTCID INT, DTCCODE varchar(50))		
		
		INSERT INTO @CurrentTBL(DTCID, DTCCODE)
		SELECT dtc_ID,  freeze_DTC_CODE FROM tab_CA_EMS_DTC_INFO WHERE freeze_MASTER_ID=@ID		
							
		WHILE EXISTS (SELECT DTCID FROM @CurrentTBL)
		
			BEGIN
				SELECT TOP 1 @DTCID=DTCID, @DTCCODE=DTCCODE  FROM @CurrentTBL
				BEGIN
				insert into @CurrentReturnTBL 
				select distinct dtc_LOGIC_BUILD_PK, case when dtc_LOGIC_OPERATION='0' then ' ' else dtc_LOGIC_OPERATION end, 
				dtc_DTC_CODE_A, dtc_DTC_LOGIC_A, dtc_DTC_THRESHOLD_A, dtc_DTC_CODE_A_INVERT, dtc_DTC_CODE_B, dtc_DTC_LOGIC_B, dtc_DTC_THRESHOLD_B, dtc_DTC_CODE_B_INVERT, dtc_DTC_CODE_C, dtc_DTC_LOGIC_C, dtc_DTC_THRESHOLD_C, dtc_DTC_CODE_C_INVERT, dtc_DTC_CODE_D, dtc_DTC_LOGIC_D, dtc_DTC_THRESHOLD_D, dtc_DTC_CODE_D_INVERT 
				from tab_CA_EMS_DTC_LOGIC_BUILD 
				where dtc_DTC_CODE_A =@DTCCODE or dtc_DTC_CODE_B =@DTCCODE or dtc_DTC_CODE_C =@DTCCODE or dtc_DTC_CODE_D =@DTCCODE

				END
				DELETE @CurrentTBL WHERE DTCID=@DTCID
			END

			SELECT distinct * from @CurrentReturnTBL
END
GO
/****** Object:  StoredProcedure [dbo].[GetDTCValue]    Script Date: 9/2/2017 12:36:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetDTCValue] (@ID INT)
AS

BEGIN

	DECLARE @DYNAMICQUERY AS NVARCHAR(MAX)
	DECLARE @COLUMNS AS NVARCHAR (MAX)

	SELECT @COLUMNS = ISNULL(@COLUMNS + ',', '') + QUOTENAME(freeze_SIGNAL) FROM (SELECT DISTINCT freeze_SIGNAL FROM tab_CA_EMS_FREEZEFRAME_DATA) AS BUTTON

	
			SET @DYNAMICQUERY = N' SELECT * FROM (select d.freeze_DTC_CODE, d.freeze_DTC_DESCRIPTION, d.freeze_DTC_STATUS, freeze_SIGNAL,freeze_VALUE 
								from tab_CA_EMS_DTC_INFO d
								join tab_CA_EMS_FREEZEFRAME_DATA fm on d.dtc_id = fm.dtc_INFO_ID
								where fm.MASTER_ID  =@ID) source
								pivot (MAX(freeze_VALUE) for freeze_SIGNAL in ('+ @COLUMNS +')) pov '
			EXEC SP_EXECUTESQL @DYNAMICQUERY, N'@ID int', @ID=@ID
	END
GO
/****** Object:  StoredProcedure [dbo].[SP_CheckLogin]    Script Date: 9/2/2017 12:36:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_CheckLogin](@loginID nvarchar(15),@password nvarchar(100))
AS
BEGIN	
	SELECT user_ID_PK, user_FIRST_NAME, user_LAST_NAME, user_GENDER, user_ACCESS_CODE, user_ROLE, user_ACTIVE_STATUS
	FROM tab_m_ADMIN_USER
	WHERE user_ACCESS_CODE=@loginID and user_PASSWORD=@password 	
END
GO
USE [master]
GO
ALTER DATABASE [minismart] SET  READ_WRITE 
GO
