USE [CourseWork25]
GO
/****** Object:  Table [dbo].[ClientCategories]    Script Date: 02.02.2025 13:39:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ClientCategories](
	[CategoryID] [int] IDENTITY(1,1) NOT NULL,
	[CategoryName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_ClientCategories] PRIMARY KEY CLUSTERED 
(
	[CategoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Clients]    Script Date: 02.02.2025 13:39:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Clients](
	[ClientID] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[Phone] [nvarchar](15) NOT NULL,
	[Email] [nvarchar](100) NOT NULL,
	[Address] [nvarchar](255) NOT NULL,
	[CategoryID] [int] NOT NULL,
	[UserID] [int] NULL,
 CONSTRAINT [PK_Clients] PRIMARY KEY CLUSTERED 
(
	[ClientID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Interactions]    Script Date: 02.02.2025 13:39:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Interactions](
	[InteractionID] [int] IDENTITY(1,1) NOT NULL,
	[ClientID] [int] NOT NULL,
	[InteractionDate] [date] NOT NULL,
	[TypeID] [int] NOT NULL,
	[Notes] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Interactions] PRIMARY KEY CLUSTERED 
(
	[InteractionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[InteractionType]    Script Date: 02.02.2025 13:39:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InteractionType](
	[TypeID] [int] IDENTITY(1,1) NOT NULL,
	[TypeName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_InteractionType] PRIMARY KEY CLUSTERED 
(
	[TypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 02.02.2025 13:39:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[RoleID] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED 
(
	[RoleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 02.02.2025 13:39:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[Username] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](50) NOT NULL,
	[RoleID] [int] NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[ClientCategories] ON 

INSERT [dbo].[ClientCategories] ([CategoryID], [CategoryName]) VALUES (1, N'Новый клиент')
INSERT [dbo].[ClientCategories] ([CategoryID], [CategoryName]) VALUES (2, N'Постоянный клиент')
INSERT [dbo].[ClientCategories] ([CategoryID], [CategoryName]) VALUES (3, N'VIP')
INSERT [dbo].[ClientCategories] ([CategoryID], [CategoryName]) VALUES (4, N'Проблемный клиент')
SET IDENTITY_INSERT [dbo].[ClientCategories] OFF
GO
SET IDENTITY_INSERT [dbo].[Clients] ON 

INSERT [dbo].[Clients] ([ClientID], [FirstName], [LastName], [Phone], [Email], [Address], [CategoryID], [UserID]) VALUES (1, N'Иван', N'Иванов', N'+79011234567', N'ivanov@example.com', N'ул. Ленина, д. 10', 1, NULL)
INSERT [dbo].[Clients] ([ClientID], [FirstName], [LastName], [Phone], [Email], [Address], [CategoryID], [UserID]) VALUES (2, N'Мария', N'Петрова', N'+79019876543', N'petrova@example.com', N'ул. Садовая, д. 15', 2, NULL)
INSERT [dbo].[Clients] ([ClientID], [FirstName], [LastName], [Phone], [Email], [Address], [CategoryID], [UserID]) VALUES (3, N'Павел', N'Смирнов', N'+79161234567', N'smirnov@example.com', N'ул. Горького, д. 5', 3, NULL)
INSERT [dbo].[Clients] ([ClientID], [FirstName], [LastName], [Phone], [Email], [Address], [CategoryID], [UserID]) VALUES (4, N'Анна', N'Кузнецова', N'+79271239876', N'kuznetsova@example.com', N'ул. Пушкина, д. 8', 4, NULL)
SET IDENTITY_INSERT [dbo].[Clients] OFF
GO
SET IDENTITY_INSERT [dbo].[Interactions] ON 

INSERT [dbo].[Interactions] ([InteractionID], [ClientID], [InteractionDate], [TypeID], [Notes]) VALUES (1, 1, CAST(N'2025-01-18' AS Date), 1, N'Звонок по вопросу нового заказа')
INSERT [dbo].[Interactions] ([InteractionID], [ClientID], [InteractionDate], [TypeID], [Notes]) VALUES (2, 2, CAST(N'2025-01-19' AS Date), 2, N'Переговоры о скидке для постоянного клиента')
INSERT [dbo].[Interactions] ([InteractionID], [ClientID], [InteractionDate], [TypeID], [Notes]) VALUES (3, 3, CAST(N'2025-01-20' AS Date), 1, N'Ответ на электронное письмо')
INSERT [dbo].[Interactions] ([InteractionID], [ClientID], [InteractionDate], [TypeID], [Notes]) VALUES (4, 4, CAST(N'2025-01-21' AS Date), 3, N'Назначена встреча для обсуждения условий сотрудничества')
SET IDENTITY_INSERT [dbo].[Interactions] OFF
GO
SET IDENTITY_INSERT [dbo].[InteractionType] ON 

INSERT [dbo].[InteractionType] ([TypeID], [TypeName]) VALUES (1, N'Звонок')
INSERT [dbo].[InteractionType] ([TypeID], [TypeName]) VALUES (2, N'Переговоры')
INSERT [dbo].[InteractionType] ([TypeID], [TypeName]) VALUES (3, N'Встреча')
SET IDENTITY_INSERT [dbo].[InteractionType] OFF
GO
SET IDENTITY_INSERT [dbo].[Roles] ON 

INSERT [dbo].[Roles] ([RoleID], [RoleName]) VALUES (1, N'Администратор')
INSERT [dbo].[Roles] ([RoleID], [RoleName]) VALUES (2, N'Менеджер')
SET IDENTITY_INSERT [dbo].[Roles] OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([UserID], [Username], [Password], [RoleID]) VALUES (1, N'admin', N'Admin@1', 1)
INSERT [dbo].[Users] ([UserID], [Username], [Password], [RoleID]) VALUES (2, N'manager', N'Manager@1', 2)
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
ALTER TABLE [dbo].[Clients]  WITH CHECK ADD  CONSTRAINT [FK_Clients_ClientCategories] FOREIGN KEY([CategoryID])
REFERENCES [dbo].[ClientCategories] ([CategoryID])
GO
ALTER TABLE [dbo].[Clients] CHECK CONSTRAINT [FK_Clients_ClientCategories]
GO
ALTER TABLE [dbo].[Clients]  WITH CHECK ADD  CONSTRAINT [FK_Clients_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Clients] CHECK CONSTRAINT [FK_Clients_Users]
GO
ALTER TABLE [dbo].[Interactions]  WITH CHECK ADD  CONSTRAINT [FK_Interactions_Clients] FOREIGN KEY([ClientID])
REFERENCES [dbo].[Clients] ([ClientID])
GO
ALTER TABLE [dbo].[Interactions] CHECK CONSTRAINT [FK_Interactions_Clients]
GO
ALTER TABLE [dbo].[Interactions]  WITH CHECK ADD  CONSTRAINT [FK_Interactions_InteractionType] FOREIGN KEY([TypeID])
REFERENCES [dbo].[InteractionType] ([TypeID])
GO
ALTER TABLE [dbo].[Interactions] CHECK CONSTRAINT [FK_Interactions_InteractionType]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Roles] FOREIGN KEY([RoleID])
REFERENCES [dbo].[Roles] ([RoleID])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Roles]
GO
