USE [Testing_DB]
GO
/****** Object:  Table [dbo].[Tbl_comment]    Script Date: 7/17/2024 8:59:32 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_comment](
	[id] [uniqueidentifier] NOT NULL,
	[post_id] [uniqueidentifier] NULL,
	[comment] [nvarchar](max) NULL,
	[created_at] [datetime] NULL,
	[updated_at] [datetime] NULL,
	[is_deleted] [bit] NULL,
	[comment_user_id] [uniqueidentifier] NULL,
 CONSTRAINT [PK_Tbl_comment] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tbl_filedetails]    Script Date: 7/17/2024 8:59:32 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_filedetails](
	[id] [uniqueidentifier] NOT NULL,
	[file_name] [nvarchar](300) NULL,
	[file_data] [varbinary](max) NOT NULL,
	[file_type] [int] NOT NULL,
	[file_size] [int] NULL,
	[uploaded_at] [datetime] NULL,
 CONSTRAINT [PK_Tbl_filedetails] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tbl_post]    Script Date: 7/17/2024 8:59:32 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_post](
	[id] [uniqueidentifier] NOT NULL,
	[user_id] [uniqueidentifier] NULL,
	[post_content] [nvarchar](max) NULL,
	[created_at] [datetime] NULL,
	[updated_at] [datetime] NULL,
	[is_deleted] [bit] NULL,
	[image_url] [varchar](max) NULL,
 CONSTRAINT [PK_Tbl_post] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tbl_user]    Script Date: 7/17/2024 8:59:32 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_user](
	[id] [uniqueidentifier] NOT NULL,
	[username] [varchar](50) NULL,
	[password] [varchar](max) NULL,
	[email] [varchar](100) NULL,
	[contact_no] [varchar](50) NULL,
	[created_at] [datetime] NULL,
	[updated_at] [datetime] NULL,
	[is_active] [bit] NULL,
 CONSTRAINT [PK_Tbl_user] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
