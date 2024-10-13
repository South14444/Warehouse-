use Warehouse;
go
create table Product(
	ID int not null identity(1,1) constraint PK_ProductID primary key,
	Name nvarchar(30) not null,
	TypeId int not null,
	SupplierId int not null,
	Quantity int not null,
	Cost_price money not null,
	Date date
);
go

create table Type(
	ID int not null identity(1,1) constraint PK_TypeID primary key,
	Name nvarchar(30) not null
);
go

create table Supplier(
	ID int not null identity(1,1) constraint PK_SupplierID primary key,
	Name nvarchar(30) not null
);
go
alter table Product add foreign key ([TypeId]) references [Type](ID);
go
alter table Product add foreign key ([SupplierId]) references [Supplier](ID);
go



insert into Type([Name])
values (N'Дерево'),
(N'Метал'),
(N'Утеплитель');

insert into Supplier([Name])
values (N'ООО.ВАК'),
(N'ООО.БАН'),
(N'ООО.ВАЛВ');

insert into Product([Name],TypeId,SupplierId,Quantity,Cost_price)
values (N'Ель',1,1,100,1000),
(N'Сосна',1,2,200,2000),
(N'Дуб',1,3,300,3000),
(N'Сталь',2,1,100,1000),
(N'Медь',2,2,200,2000),
(N'Олово',2,3,300,3000),
(N'Стекловата',3,1,100,1000),
(N'Карамзит',3,2,200,2000),
(N'Пенополистерол',3,3,300,3000)
;

