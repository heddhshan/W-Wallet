
/****** Object:  Table [dbo].[HD_Address]    Script Date: 2022/12/1 10:42:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HD_Address](
	[MneId] [uniqueidentifier] NOT NULL,
	[MneSecondSalt] [nvarchar](64) NULL,
	[AddressIndex] [int] NOT NULL,
	[AddressAlias] [nvarchar](64) NULL,
	[Address] [nvarchar](43) NOT NULL,
	[IsTxAddress] [bit] NULL,
	[HasPrivatekey] [bit] NULL,
 CONSTRAINT [PK_HDWallet_Address] PRIMARY KEY CLUSTERED 
(
	[Address] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[HD_Mnemonic]    Script Date: 2022/12/1 10:42:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HD_Mnemonic](
	[MneId] [uniqueidentifier] NOT NULL,
	[MneAlias] [nvarchar](64) NOT NULL,
	[MneEncrypted] [nvarchar](1024) NOT NULL,
	[EncryptedTimes] [int] NOT NULL,
	[MneHash] [nvarchar](64) NOT NULL,
	[WordCount] [int] NOT NULL,
	[MnePath] [nvarchar](32) NOT NULL,
	[Salt] [nvarchar](64) NOT NULL,
	[UserPasswordHash] [nvarchar](64) NOT NULL,
	[UserPasswordTip] [nvarchar](64) NULL,
	[MneFirstSalt] [nvarchar](128) NULL,
	[MneSource] [int] NOT NULL,
	[IsBackUp] [bit] NULL,
	[CreateTime] [datetime] NULL,
	[HasPrivatekey] [bit] NULL,
 CONSTRAINT [PK_HDWallet_Mnemonic] PRIMARY KEY CLUSTERED 
(
	[MneId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[KeyStore_Address]    Script Date: 2022/12/1 10:42:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KeyStore_Address](
	[AddressAlias] [nvarchar](64) NULL,
	[Address] [nvarchar](43) NOT NULL,
	[FilePath] [nvarchar](2048) NOT NULL,
	[KeyStoreText] [nvarchar](2048) NULL,
	[IsTxAddress] [bit] NULL,
	[HasPrivatekey] [bit] NULL,
 CONSTRAINT [PK_KeyStore_Address] PRIMARY KEY CLUSTERED 
(
	[Address] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[View_Address]    Script Date: 2022/12/1 10:42:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[View_Address]
AS
SELECT   AddressAlias, Address, 'KeyStore' AS SourceName, IsTxAddress, HasPrivatekey
FROM      KeyStore_Address
UNION
SELECT   HD_Address.AddressAlias, HD_Address.Address, HD_Mnemonic.MneAlias AS SourceName, 
                HD_Address.IsTxAddress, HD_Address.HasPrivatekey
FROM      HD_Address INNER JOIN
                HD_Mnemonic ON HD_Address.MneId = HD_Mnemonic.MneId
GO
/****** Object:  Table [dbo].[AddressBalance]    Script Date: 2022/12/1 10:42:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AddressBalance](
	[UserAddress] [nvarchar](64) NOT NULL,
	[TokenAddress] [nvarchar](64) NOT NULL,
	[Balance] [decimal](36, 18) NOT NULL,
	[UpdateBlockNumber] [bigint] NOT NULL,
	[Balance_Text] [nvarchar](80) NULL,
	[UpdateDateTime] [datetime] NULL,
 CONSTRAINT [PK_AddressToken] PRIMARY KEY CLUSTERED 
(
	[UserAddress] ASC,
	[TokenAddress] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AppInfo_OnPublishAppDownload]    Script Date: 2022/12/1 10:42:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AppInfo_OnPublishAppDownload](
	[ContractAddress] [nvarchar](43) NOT NULL,
	[BlockNumber] [bigint] NOT NULL,
	[TransactionHash] [nvarchar](66) NOT NULL,
	[_eventId] [bigint] NOT NULL,
	[_AppId] [int] NULL,
	[_PlatformId] [int] NULL,
	[_Version] [int] NULL,
	[_HttpLink] [nvarchar](1024) NULL,
	[_BTLink] [nvarchar](1024) NULL,
	[_eMuleLink] [nvarchar](1024) NULL,
	[_IpfsLink] [nvarchar](1024) NULL,
	[_OtherLink] [nvarchar](1024) NULL,
 CONSTRAINT [PK_AppInfo_OnPublishAppDownload] PRIMARY KEY CLUSTERED 
(
	[ContractAddress] ASC,
	[_eventId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AppInfo_OnPublishAppVersion]    Script Date: 2022/12/1 10:42:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AppInfo_OnPublishAppVersion](
	[ContractAddress] [nvarchar](43) NOT NULL,
	[BlockNumber] [bigint] NOT NULL,
	[TransactionHash] [nvarchar](66) NOT NULL,
	[_eventId] [bigint] NOT NULL,
	[_AppId] [int] NULL,
	[_PlatformId] [int] NULL,
	[_Version] [int] NULL,
	[_Sha256Value] [nvarchar](128) NULL,
	[_AppName] [nvarchar](128) NULL,
	[_UpdateInfo] [nvarchar](1024) NULL,
	[_IconUri] [nvarchar](1024) NULL,
 CONSTRAINT [PK_AppInfo_OnPublishAppVersion] PRIMARY KEY CLUSTERED 
(
	[ContractAddress] ASC,
	[_eventId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Appinfo_OnPublishNotice]    Script Date: 2022/12/1 10:42:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Appinfo_OnPublishNotice](
	[ContractAddress] [nvarchar](43) NOT NULL,
	[BlockNumber] [bigint] NOT NULL,
	[TransactionHash] [nvarchar](66) NOT NULL,
	[_publisher] [nvarchar](43) NOT NULL,
	[_appId] [bigint] NOT NULL,
	[_subject] [nvarchar](1024) NOT NULL,
	[_body] [nvarchar](max) NOT NULL,
	[BlockTime] [datetime] NULL,
 CONSTRAINT [PK_Tools_OnPublishNotice] PRIMARY KEY NONCLUSTERED 
(
	[TransactionHash] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Contact]    Script Date: 2022/12/1 10:42:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Contact](
	[ContactName] [nvarchar](64) NOT NULL,
	[ContactAddress] [nvarchar](43) NOT NULL,
	[ContactRemark] [nvarchar](2048) NULL,
	[CreateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_Contact] PRIMARY KEY CLUSTERED 
(
	[ContactAddress] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ContEventBlockNum]    Script Date: 2022/12/1 10:42:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ContEventBlockNum](
	[ContractAddress] [nvarchar](43) NOT NULL,
	[EventName] [nvarchar](256) NOT NULL,
	[LastBlockNumber] [bigint] NOT NULL,
 CONSTRAINT [PK_ContEventBlockNum] PRIMARY KEY CLUSTERED 
(
	[ContractAddress] ASC,
	[EventName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[T_Language]    Script Date: 2022/12/1 10:42:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_Language](
	[LCID] [int] NOT NULL,
	[CultureInfoName] [nvarchar](32) NOT NULL,
	[TwoLetterISOLanguageName] [nvarchar](8) NOT NULL,
	[ThreeLetterISOLanguageName] [nvarchar](8) NOT NULL,
	[ThreeLetterWindowsLanguageName] [nvarchar](8) NOT NULL,
	[NativeName] [nvarchar](64) NOT NULL,
	[DisplayName] [nvarchar](64) NOT NULL,
	[EnglishName] [nvarchar](64) NOT NULL,
	[IsSelected] [bit] NOT NULL,
	[ItemsNumber] [int] NULL,
 CONSTRAINT [PK_T_Language] PRIMARY KEY CLUSTERED 
(
	[CultureInfoName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[T_OriginalText]    Script Date: 2022/12/1 10:42:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_OriginalText](
	[OriginalHash] [nvarchar](64) NOT NULL,
	[OriginalText] [nvarchar](4000) NOT NULL,
 CONSTRAINT [PK_T_OriginalText1] PRIMARY KEY CLUSTERED 
(
	[OriginalHash] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[T_OriginalText_BAK]    Script Date: 2022/12/1 10:42:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_OriginalText_BAK](
	[OriginalHash] [nvarchar](64) NOT NULL,
	[OriginalText] [nvarchar](4000) NOT NULL,
 CONSTRAINT [PK_T_OriginalText] PRIMARY KEY CLUSTERED 
(
	[OriginalHash] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[T_Refrence]    Script Date: 2022/12/1 10:42:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_Refrence](
	[OriginalHash] [nvarchar](64) NOT NULL,
	[RefrenceFormHash] [nvarchar](64) NOT NULL,
	[RefrenceForm] [nvarchar](3096) NULL,
 CONSTRAINT [PK_T_Refrence] PRIMARY KEY CLUSTERED 
(
	[OriginalHash] ASC,
	[RefrenceFormHash] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[T_TranslationText]    Script Date: 2022/12/1 10:42:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_TranslationText](
	[OriginalHash] [nvarchar](64) NOT NULL,
	[LanCode] [nvarchar](8) NOT NULL,
	[TranslationText] [nvarchar](4000) NOT NULL,
 CONSTRAINT [PK_T_TranslationText] PRIMARY KEY CLUSTERED 
(
	[OriginalHash] ASC,
	[LanCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[T_TranslationText_BAK]    Script Date: 2022/12/1 10:42:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_TranslationText_BAK](
	[OriginalHash] [nvarchar](64) NOT NULL,
	[LanCode] [nvarchar](8) NOT NULL,
	[TranslationText] [nvarchar](4000) NOT NULL,
 CONSTRAINT [PK_T_TranslationText_1] PRIMARY KEY CLUSTERED 
(
	[OriginalHash] ASC,
	[LanCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Token]    Script Date: 2022/12/1 10:42:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Token](
	[ChainId] [int] NOT NULL,
	[Name] [nvarchar](64) NOT NULL,
	[Address] [nvarchar](43) NOT NULL,
	[Decimals] [int] NOT NULL,
	[Symbol] [nvarchar](64) NOT NULL,
	[ImagePath] [nvarchar](2048) NULL,
	[IsPricingToken] [bit] NOT NULL,
	[PricingTokenAddress] [nvarchar](43) NULL,
	[PricingTokenPrice] [float] NULL,
	[PricingTokenPriceUpdateTime] [datetime] NULL,
	[PricingIsFixed] [bit] NULL,
 CONSTRAINT [PK_Token_1] PRIMARY KEY CLUSTERED 
(
	[ChainId] ASC,
	[Address] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TransactionReceipt]    Script Date: 2022/12/1 10:42:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TransactionReceipt](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TransactionHash] [nvarchar](66) NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[UserMethod] [nvarchar](128) NOT NULL,
	[UserFrom] [nvarchar](43) NOT NULL,
	[UserRemark] [nvarchar](1024) NOT NULL,
	[TransactionIndex] [bigint] NULL,
	[GotReceipt] [bit] NOT NULL,
	[BlockHash] [nvarchar](66) NULL,
	[BlockNumber] [bigint] NULL,
	[CumulativeGasUsed] [bigint] NULL,
	[GasUsed] [bigint] NULL,
	[GasPrice] [float] NULL,
	[ContractAddress] [nvarchar](43) NULL,
	[Status] [bigint] NULL,
	[Logs] [nvarchar](max) NULL,
	[HasErrors] [bit] NULL,
	[ResultTime] [datetime] NULL,
	[Canceled] [bit] NULL,
 CONSTRAINT [PK_TransactionReceipt] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserTokenApprove]    Script Date: 2022/12/1 10:42:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserTokenApprove](
	[ChainId] [int] NOT NULL,
	[TransactionHash] [nvarchar](66) NOT NULL,
	[UserAddress] [nvarchar](43) NOT NULL,
	[TokenAddress] [nvarchar](43) NOT NULL,
	[SpenderAddress] [nvarchar](43) NOT NULL,
	[TokenDecimals] [int] NULL,
	[TokenSymbol] [nvarchar](64) NULL,
	[CurrentAmount] [float] NULL,
	[IsDivToken] [bit] NULL,
	[DivTokenIsWithdrawable] [bit] NULL,
 CONSTRAINT [PK_UserTokenApprove] PRIMARY KEY CLUSTERED 
(
	[ChainId] ASC,
	[TokenAddress] ASC,
	[UserAddress] ASC,
	[SpenderAddress] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WalletHelper_OnDeployContract]    Script Date: 2022/12/1 10:42:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WalletHelper_OnDeployContract](
	[ContractAddress] [nvarchar](43) NOT NULL,
	[BlockNumber] [bigint] NOT NULL,
	[TransactionHash] [nvarchar](66) NOT NULL,
	[ChainId] [int] NOT NULL,
	[_contract] [nvarchar](43) NOT NULL,
	[_user] [nvarchar](43) NOT NULL,
	[_amount] [decimal](36, 18) NOT NULL,
	[_salt] [nvarchar](66) NOT NULL,
	[_bytecodeHash] [nvarchar](66) NOT NULL,
	[_amount_Text] [nvarchar](80) NULL,
 CONSTRAINT [PK_Tools_OnDeployContract_1] PRIMARY KEY NONCLUSTERED 
(
	[ContractAddress] ASC,
	[TransactionHash] ASC,
	[ChainId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WalletHelper_OnDeployContract_Local]    Script Date: 2022/12/1 10:42:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WalletHelper_OnDeployContract_Local](
	[ContractAddress] [nvarchar](43) NOT NULL,
	[TransactionHash] [nvarchar](66) NOT NULL,
	[ChainId] [int] NOT NULL,
	[_bytecode] [nvarchar](max) NOT NULL,
	[LocalRemark] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_Tools_OnDeployContract_Local] PRIMARY KEY CLUSTERED 
(
	[ContractAddress] ASC,
	[TransactionHash] ASC,
	[ChainId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Web3Url]    Script Date: 2022/12/1 10:42:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Web3Url](
	[UrlAlias] [nvarchar](64) NOT NULL,
	[UrlHash] [nvarchar](128) NOT NULL,
	[Url] [nvarchar](2000) NOT NULL,
	[IsSelected] [bit] NOT NULL,
 CONSTRAINT [PK_Web3Url] PRIMARY KEY CLUSTERED 
(
	[UrlHash] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AddressBalance] ADD  CONSTRAINT [DF_AddressToken_Balance]  DEFAULT ((0)) FOR [Balance]
GO
ALTER TABLE [dbo].[HD_Address] ADD  CONSTRAINT [DF_HD_Address_IsTxAddress]  DEFAULT ((0)) FOR [IsTxAddress]
GO
ALTER TABLE [dbo].[HD_Address] ADD  CONSTRAINT [DF_HD_Address_HasPrivatekey_1]  DEFAULT ((1)) FOR [HasPrivatekey]
GO
ALTER TABLE [dbo].[HD_Mnemonic] ADD  CONSTRAINT [DF_HDWallet_Mnemonic_EncryptTimes]  DEFAULT ((1)) FOR [EncryptedTimes]
GO
ALTER TABLE [dbo].[HD_Mnemonic] ADD  CONSTRAINT [DF_HD_Mnemonic_CreateTime]  DEFAULT (getdate()) FOR [CreateTime]
GO
ALTER TABLE [dbo].[HD_Mnemonic] ADD  CONSTRAINT [DF_HD_Mnemonic_HasPrivatekey]  DEFAULT ((1)) FOR [HasPrivatekey]
GO
ALTER TABLE [dbo].[KeyStore_Address] ADD  CONSTRAINT [DF_KeyStore_Address_IsTxAddress]  DEFAULT ((1)) FOR [IsTxAddress]
GO
ALTER TABLE [dbo].[KeyStore_Address] ADD  CONSTRAINT [DF_KeyStore_Address_HasPrivatekey]  DEFAULT ((1)) FOR [HasPrivatekey]
GO
ALTER TABLE [dbo].[T_Language] ADD  CONSTRAINT [DF_T_Language_IsSelected]  DEFAULT ((0)) FOR [IsSelected]
GO
ALTER TABLE [dbo].[T_Language] ADD  CONSTRAINT [DF_T_Language_ItemsNumber]  DEFAULT ((0)) FOR [ItemsNumber]
GO
ALTER TABLE [dbo].[Token] ADD  CONSTRAINT [DF_Token_IsPricingToken]  DEFAULT ((0)) FOR [IsPricingToken]
GO
ALTER TABLE [dbo].[Token] ADD  CONSTRAINT [DF_Token_PricingIsFixed]  DEFAULT ((0)) FOR [PricingIsFixed]
GO
ALTER TABLE [dbo].[TransactionReceipt] ADD  CONSTRAINT [DF_TransactionReceipt_HasReceipt]  DEFAULT ((0)) FOR [GotReceipt]
GO
ALTER TABLE [dbo].[UserTokenApprove] ADD  CONSTRAINT [DF_UserTokenApprove_Amount]  DEFAULT ((0)) FOR [CurrentAmount]
GO
ALTER TABLE [dbo].[UserTokenApprove] ADD  CONSTRAINT [DF_UserTokenApprove_IsDivToken]  DEFAULT ((0)) FOR [IsDivToken]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[24] 4[37] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_Address'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_Address'
GO
