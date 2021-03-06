/****** Object:  Table [dbo].[BaseParameters]    Script Date: 11/04/2021 16:35:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BaseParameters](
	[ColorR] [int] NOT NULL,
	[ColorG] [int] NOT NULL,
	[ColorB] [int] NOT NULL,
	[AdminLogin] [nvarchar](max) NOT NULL,
	[AdminPassword] [nvarchar](max) NOT NULL,
	[Name] [nvarchar](max) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Class]    Script Date: 11/04/2021 16:35:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Class](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Year] [int] NOT NULL,
	[Order] [int] NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[IdLevel] [int] NULL,
	[IdMainTeacher] [int] NULL,
 CONSTRAINT [PK_Class] PRIMARY KEY CLUSTERED 
(
	[Id] ASC,
	[Year] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Level]    Script Date: 11/04/2021 16:35:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Level](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Year] [int] NOT NULL,
	[Order] [int] NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Level] PRIMARY KEY CLUSTERED 
(
	[Id] ASC,
	[Year] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LevelSubject]    Script Date: 11/04/2021 16:35:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LevelSubject](
	[IdLevel] [int] NOT NULL,
	[IdSubject] [int] NOT NULL,
	[Year] [int] NOT NULL,
 CONSTRAINT [PK_LevelSubject] PRIMARY KEY CLUSTERED 
(
	[IdLevel] ASC,
	[IdSubject] ASC,
	[Year] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Mark]    Script Date: 11/04/2021 16:35:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Mark](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Year] [int] NOT NULL,
	[Mark] [decimal](5, 2) NOT NULL,
	[Coefficient] [decimal](5, 2) NOT NULL,
	[Order] [int] NOT NULL,
	[IdClass] [int] NOT NULL,
	[IdPeriod] [int] NOT NULL,
	[IdSubject] [int] NOT NULL,
	[IdStudent] [int] NOT NULL,
	[IdTeacher] [int] NOT NULL,
 CONSTRAINT [PK_Mark] PRIMARY KEY CLUSTERED 
(
	[Id] ASC,
	[Year] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Period]    Script Date: 11/04/2021 16:35:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Period](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Year] [int] NOT NULL,
	[Trimester] [int] NOT NULL,
	[Number] [int] NOT NULL,
	[FromDate] [datetime] NOT NULL,
	[ToDate] [datetime] NOT NULL,
 CONSTRAINT [PK_Period] PRIMARY KEY CLUSTERED 
(
	[Id] ASC,
	[Year] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PeriodComment]    Script Date: 11/04/2021 16:35:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PeriodComment](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Year] [int] NOT NULL,
	[StudiesReport] [int] NOT NULL,
	[DisciplineReport] [int] NOT NULL,
	[IdPeriod] [int] NOT NULL,
	[IdStudent] [int] NOT NULL,
 CONSTRAINT [PK_PeriodComment] PRIMARY KEY CLUSTERED 
(
	[Id] ASC,
	[Year] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SemiTrimester]    Script Date: 11/04/2021 16:35:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SemiTrimester](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Year] [int] NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[IdPeriod1] [int] NOT NULL,
	[IdPeriod2] [int] NULL,
 CONSTRAINT [PK_SemiTrimester] PRIMARY KEY CLUSTERED 
(
	[Id] ASC,
	[Year] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SemiTrimesterComment]    Script Date: 11/04/2021 16:35:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SemiTrimesterComment](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Year] [int] NOT NULL,
	[MainTeacherComment] [nvarchar](max) NOT NULL,
	[DivisionPrefectComment] [nvarchar](max) NOT NULL,
	[IdSemiTrimester] [int] NOT NULL,
	[IdStudent] [int] NOT NULL,
 CONSTRAINT [PK_SemiTrimesterComment] PRIMARY KEY CLUSTERED 
(
	[Id] ASC,
	[Year] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Student]    Script Date: 11/04/2021 16:35:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Student](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Year] [int] NOT NULL,
	[FirstName] [nvarchar](max) NOT NULL,
	[LastName] [nvarchar](max) NOT NULL,
	[BirthDate] [datetime] NOT NULL,
	[IdClass] [int] NULL,
 CONSTRAINT [PK_Student] PRIMARY KEY CLUSTERED 
(
	[Id] ASC,
	[Year] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Subject]    Script Date: 11/04/2021 16:35:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Subject](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Year] [int] NOT NULL,
	[Order] [int] NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Coefficient] [decimal](5, 2) NOT NULL,
	[Option] [bit] NOT NULL,
	[ParentSubjectId] [int] NULL,
 CONSTRAINT [PK_Subject] PRIMARY KEY CLUSTERED 
(
	[Id] ASC,
	[Year] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SubjectTeacher]    Script Date: 11/04/2021 16:35:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SubjectTeacher](
	[IdSubject] [int] NOT NULL,
	[IdTeacher] [int] NOT NULL,
	[Year] [int] NOT NULL,
 CONSTRAINT [PK_SubjectTeacher] PRIMARY KEY CLUSTERED 
(
	[IdSubject] ASC,
	[IdTeacher] ASC,
	[Year] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Teacher]    Script Date: 11/04/2021 16:35:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Teacher](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Year] [int] NOT NULL,
	[Title] [nvarchar](max) NOT NULL,
	[LastName] [nvarchar](max) NOT NULL,
	[FirstName] [nvarchar](max) NOT NULL,
	[Login] [nvarchar](max) NOT NULL,
	[Password] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Teacher] PRIMARY KEY CLUSTERED 
(
	[Id] ASC,
	[Year] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TeacherClass]    Script Date: 11/04/2021 16:35:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TeacherClass](
	[IdTeacher] [int] NOT NULL,
	[IdClass] [int] NOT NULL,
	[Year] [int] NOT NULL,
 CONSTRAINT [PK_TeacherClass] PRIMARY KEY CLUSTERED 
(
	[IdTeacher] ASC,
	[IdClass] ASC,
	[Year] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TrimesterComment]    Script Date: 11/04/2021 16:35:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TrimesterComment](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Year] [int] NOT NULL,
	[MainTeacherComment] [nvarchar](max) NOT NULL,
	[DivisionPrefectComment] [nvarchar](max) NOT NULL,
	[Trimester] [int] NOT NULL,
	[IdStudent] [int] NOT NULL,
 CONSTRAINT [PK_TrimesterComment] PRIMARY KEY CLUSTERED 
(
	[Id] ASC,
	[Year] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TrimesterSubjectComment]    Script Date: 11/04/2021 16:35:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TrimesterSubjectComment](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Year] [int] NOT NULL,
	[Comment] [nvarchar](max) NOT NULL,
	[Trimester] [int] NOT NULL,
	[IdSubject] [int] NOT NULL,
	[IdStudent] [int] NOT NULL,
 CONSTRAINT [PK_TrimesterSubjectComment] PRIMARY KEY CLUSTERED 
(
	[Id] ASC,
	[Year] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Year]    Script Date: 11/04/2021 16:35:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Year](
	[Year] [int] NOT NULL,
 CONSTRAINT [PK_Year] PRIMARY KEY CLUSTERED 
(
	[Year] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[YearParameters]    Script Date: 11/04/2021 16:35:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[YearParameters](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Year] [int] NOT NULL,
	[DivisionPrefect] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_YearParameters] PRIMARY KEY CLUSTERED 
(
	[Id] ASC,
	[Year] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Class]  WITH CHECK ADD  CONSTRAINT [FK_Class_Level] FOREIGN KEY([IdLevel], [Year])
REFERENCES [dbo].[Level] ([Id], [Year])
GO
ALTER TABLE [dbo].[Class] CHECK CONSTRAINT [FK_Class_Level]
GO
ALTER TABLE [dbo].[Class]  WITH CHECK ADD  CONSTRAINT [FK_Class_Teacher] FOREIGN KEY([IdMainTeacher], [Year])
REFERENCES [dbo].[Teacher] ([Id], [Year])
GO
ALTER TABLE [dbo].[Class] CHECK CONSTRAINT [FK_Class_Teacher]
GO
ALTER TABLE [dbo].[LevelSubject]  WITH CHECK ADD  CONSTRAINT [FK_LevelSubject_Level] FOREIGN KEY([IdLevel], [Year])
REFERENCES [dbo].[Level] ([Id], [Year])
GO
ALTER TABLE [dbo].[LevelSubject] CHECK CONSTRAINT [FK_LevelSubject_Level]
GO
ALTER TABLE [dbo].[LevelSubject]  WITH CHECK ADD  CONSTRAINT [FK_LevelSubject_LevelSubject] FOREIGN KEY([IdSubject], [Year])
REFERENCES [dbo].[Subject] ([Id], [Year])
GO
ALTER TABLE [dbo].[LevelSubject] CHECK CONSTRAINT [FK_LevelSubject_LevelSubject]
GO
ALTER TABLE [dbo].[Mark]  WITH CHECK ADD  CONSTRAINT [FK_Mark_Class] FOREIGN KEY([IdClass], [Year])
REFERENCES [dbo].[Class] ([Id], [Year])
GO
ALTER TABLE [dbo].[Mark] CHECK CONSTRAINT [FK_Mark_Class]
GO
ALTER TABLE [dbo].[Mark]  WITH CHECK ADD  CONSTRAINT [FK_Mark_Period] FOREIGN KEY([IdPeriod], [Year])
REFERENCES [dbo].[Period] ([Id], [Year])
GO
ALTER TABLE [dbo].[Mark] CHECK CONSTRAINT [FK_Mark_Period]
GO
ALTER TABLE [dbo].[Mark]  WITH CHECK ADD  CONSTRAINT [FK_Mark_Student] FOREIGN KEY([IdStudent], [Year])
REFERENCES [dbo].[Student] ([Id], [Year])
GO
ALTER TABLE [dbo].[Mark] CHECK CONSTRAINT [FK_Mark_Student]
GO
ALTER TABLE [dbo].[Mark]  WITH CHECK ADD  CONSTRAINT [FK_Mark_Subject] FOREIGN KEY([IdSubject], [Year])
REFERENCES [dbo].[Subject] ([Id], [Year])
GO
ALTER TABLE [dbo].[Mark] CHECK CONSTRAINT [FK_Mark_Subject]
GO
ALTER TABLE [dbo].[Mark]  WITH CHECK ADD  CONSTRAINT [FK_Mark_Teacher] FOREIGN KEY([IdTeacher], [Year])
REFERENCES [dbo].[Teacher] ([Id], [Year])
GO
ALTER TABLE [dbo].[Mark] CHECK CONSTRAINT [FK_Mark_Teacher]
GO
ALTER TABLE [dbo].[PeriodComment]  WITH CHECK ADD  CONSTRAINT [FK_PeriodComment_Period] FOREIGN KEY([IdPeriod], [Year])
REFERENCES [dbo].[Period] ([Id], [Year])
GO
ALTER TABLE [dbo].[PeriodComment] CHECK CONSTRAINT [FK_PeriodComment_Period]
GO
ALTER TABLE [dbo].[PeriodComment]  WITH CHECK ADD  CONSTRAINT [FK_PeriodComment_Student] FOREIGN KEY([IdStudent], [Year])
REFERENCES [dbo].[Student] ([Id], [Year])
GO
ALTER TABLE [dbo].[PeriodComment] CHECK CONSTRAINT [FK_PeriodComment_Student]
GO
ALTER TABLE [dbo].[SemiTrimester]  WITH CHECK ADD  CONSTRAINT [FK_SemiTrimester_Period1] FOREIGN KEY([IdPeriod1], [Year])
REFERENCES [dbo].[Period] ([Id], [Year])
GO
ALTER TABLE [dbo].[SemiTrimester] CHECK CONSTRAINT [FK_SemiTrimester_Period1]
GO
ALTER TABLE [dbo].[SemiTrimester]  WITH CHECK ADD  CONSTRAINT [FK_SemiTrimester_Period2] FOREIGN KEY([IdPeriod2], [Year])
REFERENCES [dbo].[Period] ([Id], [Year])
GO
ALTER TABLE [dbo].[SemiTrimester] CHECK CONSTRAINT [FK_SemiTrimester_Period2]
GO
ALTER TABLE [dbo].[SemiTrimesterComment]  WITH CHECK ADD  CONSTRAINT [FK_SemiTrimesterComment_SemiTrimester] FOREIGN KEY([IdSemiTrimester], [Year])
REFERENCES [dbo].[SemiTrimester] ([Id], [Year])
GO
ALTER TABLE [dbo].[SemiTrimesterComment] CHECK CONSTRAINT [FK_SemiTrimesterComment_SemiTrimester]
GO
ALTER TABLE [dbo].[SemiTrimesterComment]  WITH CHECK ADD  CONSTRAINT [FK_SemiTrimesterComment_Student] FOREIGN KEY([IdStudent], [Year])
REFERENCES [dbo].[Student] ([Id], [Year])
GO
ALTER TABLE [dbo].[SemiTrimesterComment] CHECK CONSTRAINT [FK_SemiTrimesterComment_Student]
GO
ALTER TABLE [dbo].[Student]  WITH CHECK ADD  CONSTRAINT [FK_Student_Class] FOREIGN KEY([IdClass], [Year])
REFERENCES [dbo].[Class] ([Id], [Year])
GO
ALTER TABLE [dbo].[Student] CHECK CONSTRAINT [FK_Student_Class]
GO
ALTER TABLE [dbo].[Subject]  WITH CHECK ADD  CONSTRAINT [FK_Subject_ParentSubject] FOREIGN KEY([ParentSubjectId], [Year])
REFERENCES [dbo].[Subject] ([Id], [Year])
GO
ALTER TABLE [dbo].[Subject] CHECK CONSTRAINT [FK_Subject_ParentSubject]
GO
ALTER TABLE [dbo].[SubjectTeacher]  WITH CHECK ADD  CONSTRAINT [FK_SubjectTeacher_Subject] FOREIGN KEY([IdSubject], [Year])
REFERENCES [dbo].[Subject] ([Id], [Year])
GO
ALTER TABLE [dbo].[SubjectTeacher] CHECK CONSTRAINT [FK_SubjectTeacher_Subject]
GO
ALTER TABLE [dbo].[SubjectTeacher]  WITH CHECK ADD  CONSTRAINT [FK_SubjectTeacher_Teacher] FOREIGN KEY([IdTeacher], [Year])
REFERENCES [dbo].[Teacher] ([Id], [Year])
GO
ALTER TABLE [dbo].[SubjectTeacher] CHECK CONSTRAINT [FK_SubjectTeacher_Teacher]
GO
ALTER TABLE [dbo].[TeacherClass]  WITH CHECK ADD  CONSTRAINT [FK_TeacherClass_Class] FOREIGN KEY([IdClass], [Year])
REFERENCES [dbo].[Class] ([Id], [Year])
GO
ALTER TABLE [dbo].[TeacherClass] CHECK CONSTRAINT [FK_TeacherClass_Class]
GO
ALTER TABLE [dbo].[TeacherClass]  WITH CHECK ADD  CONSTRAINT [FK_TeacherClass_Teacher] FOREIGN KEY([IdTeacher], [Year])
REFERENCES [dbo].[Teacher] ([Id], [Year])
GO
ALTER TABLE [dbo].[TeacherClass] CHECK CONSTRAINT [FK_TeacherClass_Teacher]
GO
ALTER TABLE [dbo].[TrimesterComment]  WITH CHECK ADD  CONSTRAINT [FK_TrimesterComment_Student] FOREIGN KEY([IdStudent], [Year])
REFERENCES [dbo].[Student] ([Id], [Year])
GO
ALTER TABLE [dbo].[TrimesterComment] CHECK CONSTRAINT [FK_TrimesterComment_Student]
GO
ALTER TABLE [dbo].[TrimesterSubjectComment]  WITH CHECK ADD  CONSTRAINT [FK_TrimesterSubjectComment_Student] FOREIGN KEY([IdStudent], [Year])
REFERENCES [dbo].[Student] ([Id], [Year])
GO
ALTER TABLE [dbo].[TrimesterSubjectComment] CHECK CONSTRAINT [FK_TrimesterSubjectComment_Student]
GO
ALTER TABLE [dbo].[TrimesterSubjectComment]  WITH CHECK ADD  CONSTRAINT [FK_TrimesterSubjectComment_Subject] FOREIGN KEY([IdSubject], [Year])
REFERENCES [dbo].[Subject] ([Id], [Year])
GO
ALTER TABLE [dbo].[TrimesterSubjectComment] CHECK CONSTRAINT [FK_TrimesterSubjectComment_Subject]
GO
