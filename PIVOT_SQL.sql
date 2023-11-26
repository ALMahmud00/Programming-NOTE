---=== PIVOT QUERY
	DECLARE @dynamicColumnList NVARCHAR(MAX),@dynamicColumnListSelect NVARCHAR(MAX), @pivotQuery NVARCHAR(MAX)
	
	SELECT @dynamicColumnList = COALESCE(@dynamicColumnList + ', ', '') + '['+ strItemName +']'
	FROM [AkijFoodDB].[dbo].[tblTransaction] GROUP BY strItemName

	SELECT @dynamicColumnListSelect = COALESCE(@dynamicColumnListSelect + ',', '') + 'ISNULL(['+ strItemName +'],0) as ' + '['+ strItemName +']'
	FROM [AkijFoodDB].[dbo].[tblTransaction] GROUP BY strItemName

	SET @pivotQuery =	'select transactionDate,'+ @dynamicColumnListSelect+ ' from
						(select FORMAT(dteTransDate, '+CHAR(39)+'dd MMM, yyyy'+CHAR(39)+') AS transactionDate, strItemName, numQuantity 
						 from [AkijFoodDB].[dbo].[tblTransaction]) AS tableData
						 PIVOT(
							SUM(numQuantity)
							FOR [strItemName]
							IN ('+@dynamicColumnList+')
							) AS pivotTable'
	 EXECUTE(@pivotQuery)
---===


---=== REQUIRED TABLE and DATA SCRIPT
	--USE [AkijFoodDB]
	--GO
	--/****** Object:  Table [dbo].[tblTransaction]    Script Date: 11-Jun-22 11:03:39 PM ******/
	--SET ANSI_NULLS ON
	--GO
	--SET QUOTED_IDENTIFIER ON
	--GO
	--CREATE TABLE [dbo].[tblTransaction](
	--	[intId] [bigint] IDENTITY(1,1) NOT NULL,
	--	[dteTransDate] [date] NULL,
	--	[strItemName] [nvarchar](250) NULL,
	--	[numQuantity] [numeric](18, 2) NULL,
	-- CONSTRAINT [PK_tblTransaction] PRIMARY KEY CLUSTERED 
	--(
	--	[intId] ASC
	--)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
	--) ON [PRIMARY]
	--GO
	--SET IDENTITY_INSERT [dbo].[tblTransaction] ON 
	--GO
	--INSERT [dbo].[tblTransaction] ([intId], [dteTransDate], [strItemName], [numQuantity]) VALUES (1, CAST(N'2022-01-01' AS Date), N'Mojo', CAST(10.00 AS Numeric(18, 2)))
	--GO
	--INSERT [dbo].[tblTransaction] ([intId], [dteTransDate], [strItemName], [numQuantity]) VALUES (2, CAST(N'2022-01-01' AS Date), N'Mojo', CAST(20.00 AS Numeric(18, 2)))
	--GO
	--INSERT [dbo].[tblTransaction] ([intId], [dteTransDate], [strItemName], [numQuantity]) VALUES (3, CAST(N'2022-01-01' AS Date), N'Clemon', CAST(5.00 AS Numeric(18, 2)))
	--GO
	--INSERT [dbo].[tblTransaction] ([intId], [dteTransDate], [strItemName], [numQuantity]) VALUES (4, CAST(N'2022-01-01' AS Date), N'Speed', CAST(7.00 AS Numeric(18, 2)))
	--GO
	--INSERT [dbo].[tblTransaction] ([intId], [dteTransDate], [strItemName], [numQuantity]) VALUES (5, CAST(N'2022-01-02' AS Date), N'Mojo', CAST(10.00 AS Numeric(18, 2)))
	--GO
	--INSERT [dbo].[tblTransaction] ([intId], [dteTransDate], [strItemName], [numQuantity]) VALUES (6, CAST(N'2022-01-02' AS Date), N'Clemon', CAST(20.00 AS Numeric(18, 2)))
	--GO
	--INSERT [dbo].[tblTransaction] ([intId], [dteTransDate], [strItemName], [numQuantity]) VALUES (7, CAST(N'2022-01-02' AS Date), N'Clemon', CAST(5.00 AS Numeric(18, 2)))
	--GO
	--INSERT [dbo].[tblTransaction] ([intId], [dteTransDate], [strItemName], [numQuantity]) VALUES (8, CAST(N'2022-01-02' AS Date), N'Speed', CAST(7.00 AS Numeric(18, 2)))
	--GO
	--INSERT [dbo].[tblTransaction] ([intId], [dteTransDate], [strItemName], [numQuantity]) VALUES (9, CAST(N'2022-01-03' AS Date), N'Speed', CAST(7.00 AS Numeric(18, 2)))
	--GO
	--INSERT [dbo].[tblTransaction] ([intId], [dteTransDate], [strItemName], [numQuantity]) VALUES (10, CAST(N'2022-01-03' AS Date), N'Tang', CAST(7.00 AS Numeric(18, 2)))
	--GO
	--INSERT [dbo].[tblTransaction] ([intId], [dteTransDate], [strItemName], [numQuantity]) VALUES (11, CAST(N'2022-01-04' AS Date), N'Juice', CAST(7.00 AS Numeric(18, 2)))
	--GO
	--SET IDENTITY_INSERT [dbo].[tblTransaction] OFF
	--GO
---================



