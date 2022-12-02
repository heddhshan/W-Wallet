
/****** Object:  Table [dbo].[HD_Address]    Script Date: 2022/11/8 16:19:24 ******/
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
/****** Object:  Table [dbo].[HD_Mnemonic]    Script Date: 2022/11/8 16:19:24 ******/
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
/****** Object:  Table [dbo].[KeyStore_Address]    Script Date: 2022/11/8 16:19:24 ******/
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
/****** Object:  View [dbo].[View_Address]    Script Date: 2022/11/8 16:19:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[View_Address]
AS
SELECT AddressAlias, Address, 'KeyStore' AS SourceName, IsTxAddress,HasPrivatekey
FROM   KeyStore_Address
UNION
SELECT HD_Address.AddressAlias, HD_Address.Address, HD_Mnemonic.MneAlias AS SourceName, HD_Address.IsTxAddress,HD_Address.HasPrivatekey

FROM   HD_Address INNER JOIN
          HD_Mnemonic ON HD_Address.MneId = HD_Mnemonic.MneId
GO
/****** Object:  Table [dbo].[AbiFunction]    Script Date: 2022/11/8 16:19:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AbiFunction](
	[FunId] [uniqueidentifier] NOT NULL,
	[FunctionFullName] [nvarchar](256) NOT NULL,
	[Remark] [nvarchar](64) NOT NULL,
	[FunctionFullNameHash] [nvarchar](128) NOT NULL,
	[FunctionFullNameHash4] [nvarchar](16) NOT NULL,
	[IsSysDefine] [bit] NOT NULL,
	[IsTestOk] [bit] NOT NULL,
	[IsEthTransfer] [bit] NOT NULL,
 CONSTRAINT [PK_AbiFunction] PRIMARY KEY CLUSTERED 
(
	[FunId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AbiParamer]    Script Date: 2022/11/8 16:19:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AbiParamer](
	[FunId] [uniqueidentifier] NOT NULL,
	[ParamerType] [nvarchar](128) NOT NULL,
	[ParamerName] [nvarchar](128) NULL,
	[ParamerOrder] [int] NOT NULL,
 CONSTRAINT [PK_AbiParamer_1] PRIMARY KEY CLUSTERED 
(
	[FunId] ASC,
	[ParamerOrder] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Contact]    Script Date: 2022/11/8 16:19:24 ******/
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
/****** Object:  Table [dbo].[T_Language]    Script Date: 2022/11/8 16:19:24 ******/
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
	[ItemsNumber] INT NULL DEFAULT 0, 
 CONSTRAINT [PK_T_Language] PRIMARY KEY CLUSTERED 
(
	[CultureInfoName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[T_OriginalText]    Script Date: 2022/11/8 16:19:24 ******/
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
/****** Object:  Table [dbo].[T_OriginalText_BAK]    Script Date: 2022/11/8 16:19:24 ******/
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
/****** Object:  Table [dbo].[T_Refrence]    Script Date: 2022/11/8 16:19:24 ******/
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
/****** Object:  Table [dbo].[T_TranslationText]    Script Date: 2022/11/8 16:19:24 ******/
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
/****** Object:  Table [dbo].[T_TranslationText_BAK]    Script Date: 2022/11/8 16:19:24 ******/
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
/****** Object:  Table [dbo].[Token]    Script Date: 2022/11/8 16:19:24 ******/
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
	[PricingIsFixed] [bit] NULL DEFAULT 0, 
 CONSTRAINT [PK_Token_1] PRIMARY KEY CLUSTERED 
(
	[ChainId] ASC,
	[Address] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AbiFunction] ADD  CONSTRAINT [DF_AbiFunction_IsEthTransfer]  DEFAULT ((0)) FOR [IsEthTransfer]
GO
ALTER TABLE [dbo].[HD_Address] ADD  CONSTRAINT [DF_HD_Address_IsTxAddress]  DEFAULT ((0)) FOR [IsTxAddress]
GO
ALTER TABLE [dbo].[HD_Address] ADD  CONSTRAINT [DF_HD_Address_HasPrivatekey]  DEFAULT ((1)) FOR [HasPrivatekey]
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
ALTER TABLE [dbo].[Token] ADD  CONSTRAINT [DF_Token_IsPricingToken]  DEFAULT ((0)) FOR [IsPricingToken]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
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
