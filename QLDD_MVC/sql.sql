USE [master]
GO
/****** Object:  Database [QLDD]    Script Date: 2/15/2023 8:16:09 PM ******/
CREATE DATABASE [QLDD]
GO
ALTER DATABASE [QLDD] SET QUERY_STORE = OFF
GO
USE [QLDD]
GO

/****** Object:  Table [dbo].[chitietdd]    Script Date: 2/15/2023 8:16:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[chitietdd](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[madd] [int] NOT NULL,
	[masv] [nchar](9) NOT NULL,
	[thoigiandd] [datetime] NOT NULL,
	[trangthai] [bit] NULL,

 CONSTRAINT [PK_chitietdd] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


/****** Object:  Table [dbo].[diemdanh]    Script Date: 2/15/2023 8:16:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[diemdanh](
	[madd] [int] IDENTITY(1,1) NOT NULL,
	[maloptc] [int] NOT NULL,
	[ngaydd] [date] NOT NULL,
	[diadiem] [nvarchar](20) NULL,
 CONSTRAINT [PK_diemdanh] PRIMARY KEY CLUSTERED 
(
	[madd] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[giangvien]    Script Date: 2/15/2023 8:16:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[giangvien](
	[magv] [int] IDENTITY(1,1) NOT NULL,
	[hoten] [nvarchar](50) NOT NULL,
	[gioitinh] [bit] NOT NULL,
	[diachi] [nvarchar](50) NULL,
	[email] [nchar](30) NULL,
	[sdt] [varchar](11) NULL,
	[username] [nchar](20) NULL,

 CONSTRAINT [PK_giangvien] PRIMARY KEY CLUSTERED 
(
	[magv] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GVCN]    Script Date: 2/15/2023 8:16:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GVCN](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[magv] [int] NULL,
	[malophc] [int] NULL,
	[username] [nchar](20) NOT NULL,

 CONSTRAINT [PK_GVCN] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GVTC]    Script Date: 2/15/2023 8:16:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GVTC](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[magv] [int] NULL,
	[maloptc] [int] NULL,
	[username] [nchar](20) NOT NULL,

 CONSTRAINT [PK_GVHP] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LopHC]    Script Date: 2/15/2023 8:16:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LopHC](
	[malophc] [int] IDENTITY(1,1) NOT NULL,
	[tenlophc] [nvarchar](50) NOT NULL,
	[magv] [int] NOT NULL,
	[khoa] [nvarchar](50) NULL,
 CONSTRAINT [PK_LopHC] PRIMARY KEY CLUSTERED 
(
	[malophc] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LopTC]    Script Date: 2/15/2023 8:16:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LopTC](
	[maloptc] [int] IDENTITY(1,1) NOT NULL,
	[mahp] [nchar](10) NOT NULL,
	[tenltc] [nvarchar](30) NULL,
	[magv] [int] NOT NULL,
	[trangthai] [bit] NOT NULL,
	[sttlop] [int] NULL,
 CONSTRAINT [PK_LopTC] PRIMARY KEY CLUSTERED 
(
	[maloptc] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Hocphan](
	[mahp] [nchar](10) NOT NULL,
	[tenhp] [nvarchar](50) NOT NULL,
	[sotc] [int] NOT NULL,
 CONSTRAINT [PK_Hocpha] PRIMARY KEY CLUSTERED 
(
	[mahp] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Sinhvien]    Script Date: 2/15/2023 8:16:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sinhvien](
	[masv] [int] IDENTITY(1,1) NOT NULL,
	[hoten] [nvarchar](50) NOT NULL,
	[gioitinh] [bit] NOT NULL,
	[malophc] [int] NULL,
	[khoa] [nvarchar](50) NULL,
	[EmbFace] [ntext] NULL,

 CONSTRAINT [PK_Sinhvien] PRIMARY KEY CLUSTERED 
(
	[masv] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TaiKhoan]    Script Date: 2/15/2023 8:16:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TaiKhoan](
	[username] [nchar](20) NOT NULL,
	[hoten] [nvarchar](50) NULL,
	[phanquyen] [nvarchar](50) NOT NULL,
	[password] [nchar](20) NOT NULL,
 CONSTRAINT [PK_TaiKhoan_1] PRIMARY KEY CLUSTERED 
(
	[username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE LopTC_SV
(
  id int IDENTITY(1,1) Primary key,
  masv   NCHAR(9) NOT NULL,
  maloptc  int NOT NULL,
  CONSTRAINT UC_LopTC_SV UNIQUE (masv,maloptc)
);

CREATE TABLE TempSV
(
  masv NCHAR(9) NOT NULL
  CONSTRAINT UC_TempSV UNIQUE (masv)
);

USE [QLDD]
GO

INSERT INTO TaiKhoan(username,hoten, phanquyen, password)
VALUES ('giangvien', N'Giảng viên 1', N'Giảng viên','123'),
		('giangvien2', N'Giảng viên 2', N'Giảng viên','123'),
		('giangvien3', N'Giảng viên 3', N'Giảng viên','123'),
		('admin', N'Quản trị viên 1', N'Quản trị viên','123'),
		('admin2', N'Quản trị viên 2', N'Quản trị viên','123'),
		('cbdt', N'Cán bộ quản lý đào tạo 1', N'Cán bộ quản lý đào tạo','123'),
		('cbdt2', N'Cán bộ quản lý đào tạo 2', N'Cán bộ quản lý đào tạo','123');


INSERT INTO giangvien(hoten, gioitinh, diachi, email, sdt, username)
VALUES (N'Giảng viên 1', 'True',N'Mạo Khê', 'giangvien1@gmail.com','0384627182','giangvien'),
	   (N'Giảng viên 2', 'True',N'Hà Nội', 'giangvien2@gmail.com','031237321','giangvien2'),
	   (N'Giảng viên 3', 'True',N'Hà Nội', 'giangvien3@gmail.com','031437321','giangvien3'),
	   	(N'Giảng viên 4', 'True',N'Hà Nội 4', 'giangvien4@gmail.com','034214321','giangvien4'),
		(N'Giảng viên 5', 'True',N'Hà Nội 5', 'giangvien5@gmail.com','032353321','giangvien5'),
		(N'Giảng viên 6', 'True',N'Hà Nội 6', 'giangvien6@gmail.com','033373221','giangvien6');
	   
INSERT INTO LopHC(tenlophc, magv)
VALUES (N'Khoa học máy tính K6A', '1'),
	   (N'Thiết kế đồ họa K1', '2'),
	   (N'Khoa học máy tính K7A', '3');

INSERT INTO GVCN(magv, malophc,username)
VALUES ('1', '1','giangvien'),
		('2', '2','giangvien2'),
		('3', '3','cbdt');






