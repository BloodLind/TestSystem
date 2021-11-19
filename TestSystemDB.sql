--create database TestSystemDB;
--go

use TestSystemDB;
go

create table Themes
(
	Id int primary key identity(1,1),
	[Name] nvarchar(500) not null
);

create table Disciplines
(
	Id int primary key identity(1,1),
	[Name] nvarchar(500) not null
);
create table TestLanguage
(
	Id int primary key identity(1,1),
	[Name] nvarchar(500) not null
);
create table Photos
(
	Id int primary key identity(1,1),
	Picture varbinary(max) not null
);

create table Users
(
	Id int primary key identity (1,1),
	Firstname nvarchar(500) not null,
	Secondname nvarchar(500) not null,
	Fathername nvarchar(500) not null,
	ProfilePictureId int foreign key references Photos(Id) null default(null),
);

create table Answers
(
	Id int primary key identity (1,1),
	AnswerData nvarchar(500) not null,
	ThemeId int foreign key references Themes(Id),
	DisciplineId int foreign key references Disciplines(Id)
);

create table Questions
(
	Id int primary key identity (1,1),
	QuestionData nvarchar(500) not null,
	ThemeId int foreign key references Themes(Id),
	DisciplineId int foreign key references Disciplines(Id)
);

create table Tests
(
	Id int primary key identity(1,1),
	IsArchive bit not null default ('FALSE'),
	ThemeId int foreign key references Themes(Id),
	DisciplineId int foreign key references Disciplines(Id),
	LanguageId int foreign key references TestLanguage(Id),
	[Name] nvarchar(500) not null
);
create table QuestionAnswer
(
	Testid int foreign key references Tests(Id) not null,
	QuestionId int foreign key references Questions(Id) not null,
	AnswerId int foreign key references Answers(Id) not null,
	IsAnswerCorrect bit not null default('True'),
	primary key (QuestionId, AnswerId, TestId)
);


create table Results
(
	Id int primary key identity(1,1),
	TestId int foreign key references Tests(Id),
	CorrectPercent int not null default (0) check (CorrectPercent >= 0 and CorrectPercent <= 100),
	Points int not null default(0) check (Points >= 0),
	UserId int foreign key references Users(Id) on delete set null default null
);


create table UserAuntifications
(
	Id int primary key identity(1,1),
	UserId int foreign key references Users(Id) on delete set null,
	UserLogin varchar(255) not null,
	UserPassword varchar(MAX) not null,
	IsAdmin bit not null default ('FALSE')
);

create table MaterialsInfos
(
	Id int primary key identity(1,1),
	PhotoId int foreign key references Photos(Id) null default(null),
	Information nvarchar(MAX) not null
);

create table TestMaterials
(
	Id int primary key identity(1,1),
	[Name] nvarchar (255) not null,
	ThemeId int foreign key references Themes(Id) on delete set null,
	DisciplineId int foreign key references Disciplines(Id) on delete set null,
	LanguageId int foreign key references TestLanguage(Id) on delete set null,
	InformationId int foreign key references MaterialsInfos on delete set null
);




--Проверка автоизации
--create function CheckAuthorization (@userLogin nvarchar(MAX), @userPassword nvarchar(max))
--returns bit
--as 
--begin 
--	declare @password varchar(255) = (select UserPassword from UserAuntifications where UserLogin like @userLogin);
--	if(@password like @userPassword)
--		return 'TRUE';
	
--	return 'FALSE'; 
--end;


-- Тригер на сооздание логина и пароля
--create trigger CreateUserAuntification 
--on UserAuntifications
--instead of insert
--as 
--begin
--	insert into UserAuntifications(UserId, UserLogin, UserPassword, IsAdmin)
--	(select UserId, UserLogin, UserPassword, IsAdmin from inserted where UserLogin not in (select UserLogin from UserAuntifications));
	
--end;

-- Получить всю информацию о пользователе
--create function GetUserByLogin(@userLogin varchar(255))
--returns @table table 
--(
--	Id int,
--	Firstname nvarchar(500),
--	Secondname nvarchar(500),
--	Fathername nvarchar(500),
--	ProfilePicture varbinary(MAX)
--)
--as
--begin 
--	declare @userId int = (select UserId from UserAuntifications where @userLogin like UserLogin);
--	insert into @table(Id,Firstname, Secondname, Fathername, ProfilePicture)
--	(select Users.Id, Firstname, Secondname, Fathername, Photos.Picture from Users left join Photos on Photos.Id = ProfilePictureId  where @userId = Users.Id);
--	return;
--end;

-- Триггер на удлаения пользователя если удалят его в атворизации
--create trigger DeleteUser
--on UserAuntifications
--after delete
--as 
--begin 
--	delete from Users
--	where Id in (select UserId from deleted);
--end;

--create function IsAdmin (@login varchar(255))
--returns bit
--as
--begin
--	return (select IsAdmin from UserAuntifications where UserLogin like @login);
--end;

-- Процедура для создания пользователя с кода, чтобы не иметь доступа к таблице вовсе
--create procedure CreateUserAuntifaction(@login varchar(255), @password varchar(255), @isAdmin bit)
--as
--begin 
--	insert into UserAuntifications(UserLogin, UserPassword, IsAdmin)
--	values(@login, @password, @isAdmin);
--end;

-- Процедура для редактирования пользователя
--create procedure SetUserInfoForAuntification(@login varchar(255), @userId int)
--as
--begin
--	update UserAuntifications
--	set UserId = @userId
--	where @login like UserLogin;
--end;

--Функция нужна для кода
--create function IsLoginFree(@login varchar(255))
--returns bit
--as
--begin
--	declare @count int = (select Count(Id) from UserAuntifications where UserLogin like @login);
--	if(@count = 0)
--		return 'TRUE';
--	return 'FALSE';
--end;

create procedure CreateUser(@Firstname nvarchar(500), @Secondname nvarchar(500), @Fathername nvarchar(500))
as
begin 
	insert into Users(Firstname, Secondname, Fathername)
	values (@Firstname, @Secondname, @Fathername);
end;


create procedure CreateUserAndGetId(@Firstname nvarchar(500), @Secondname nvarchar(500), @Fathername nvarchar(500), @Id int out)
as
begin
	exec dbo.CreateUser @Firstname, @Secondname, @Fathername;
	select @Id = (select top(1) Id from Users order by Id desc);
end;

create procedure EditUser(@UserId int, @FirstName nvarchar(500), @Secondname nvarchar(500), @Fathername nvarchar(500)) 
as
begin
	update Users
	set Firstname = @FirstName, Secondname = @Secondname, Fathername = @Fathername
	where Id = @UserId;
end;

create procedure SetUserPhotoId(@UserId int, @PhotoId int)
as 
begin
	update Users
	set ProfilePictureId = @PhotoId
	where @UserId = @PhotoId
end;

create procedure SetAdminForUser(@login varchar(255), @adminStatment bit)
as
begin
	update UserAuntifications
	set IsAdmin = @adminStatment
	where UserLogin like @login;
end;	



declare @id int;
exec dbo.CreateUserAndGetId 'test', 'test', 'test', @id out
select @id;

-- Тестирования возможностей и заполнение таблиц
insert into Users(Firstname, Fathername, Secondname)
values ('root', 'test', 'test');

insert into UserAuntifications(UserId, UserLogin, UserPassword, IsAdmin)
values (1,'root', 'root', 'TRUE');

insert into Disciplines([Name])
values ('Mathematic'), ('History'), ('Biology');

insert into Themes([Name])
values ('Proggresions'), ('WWII'), ('Atmosphere');

