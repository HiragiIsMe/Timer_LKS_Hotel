create table RoomType(
	ID int identity(1,1) primary key,
	Name varchar(50) not null,
	Capacity int not null,
	RoomPrice int not null,
	Photo image not null
);

create table Room(
	ID int identity(1,1) primary key,
	RoomTypeID int foreign key references RoomType(ID) on update cascade on delete cascade not null,
	RoomNumber varchar(50) not null,
	RoomFloor varchar(50) not null,
	Description text
);

create table Job(
	ID int identity(1,1) primary key,
	Name varchar(50) not null
);

create table Employee(
	ID int identity(1,1) primary key,
	Username varchar(50) not null,
	Password varchar(50) not null,
	Name varchar(100) not null,
	Email varchar(50) not null,
	Address varchar(200) not null,
	DateOfBirth date not null,
	JobID int foreign key references Job(ID) on update cascade on delete cascade not null,
	Photo image not null
);

create table Customer(
	ID int identity(1,1) primary key,
	Name varchar(50) not null,
	NIK varchar(50),
	Email varchar(50),
	Gender char(1),
	PhoneNumber varchar(20),
	Age int
);

create table Reservation(
	ID int identity(1,1) primary key,
	Datetime  datetime not null,
	EmployeeID int foreign key references Employee(ID) on update cascade on delete cascade not null,
	CustomerID int foreign key references Customer(ID) on update cascade on delete cascade not null,
	BookingCode varchar(6) not null
);

create table ReservationRoom(
	ID int identity(1,1) primary key,
	ReservationID int foreign key references Reservation(ID) on update cascade on delete cascade not null,
	RoomID int foreign key references Room(ID) on update cascade on delete cascade not null,
	StartDateTime date not null,
	DurationNights int not null,
	RoomPrice int not null,
	CheckInDateTime datetime not null,
	CheckOutDateTime datetime not null
);

create table FoodsAndDrinks(
	ID int identity(1,1) primary key,
	Name varchar(50) not null,
	Type char(1) not null,
	Price int not null,
	Photo image not null
);

create table FDCheckOut(
	ID int identity(1,1) primary key,
	ReservationRoomID int foreign key references ReservationRoom(ID) on update cascade on delete cascade  not null,
	FDID int foreign key references FoodsAndDrinks(ID) on update cascade on delete cascade not null,
	Qty int,
	TotalPrice int,
	EmployeeID int foreign key references Employee(ID) on update cascade on delete cascade not null
);

create table ItemStatus(
	ID int identity(1,1) primary key,
	Name varchar(50) not null
);

create table Item(
	ID int identity(1,1) primary key,
	Name varchar(50) not null,
	RequestPrice int not null,
	CompensationFee int
);

create table ReservationCheckOut(
	ID int identity(1,1) primary key,
	ReservationRoomID int foreign key references ReservationRoom(ID) on update cascade on delete cascade not null,
	ItemID int foreign key references Item(ID) on update cascade on delete cascade not null,
	ItemStatus int foreign key references ItemStatus(ID) on update cascade on delete cascade not null,
	Qty int not null,
	TotalCharge int not null
);

create table ReservationRequestItem(
	ID int identity(1,1) primary key,
	ReservationRoomID int foreign key references ReservationRoom(ID) on update cascade on delete cascade not null,
	ItemID int foreign key references Item(ID) on update cascade on delete cascade not null,
	Qty int not null,
	TotalPrice int not null
);