use master
if DB_ID('MVC_Restaurant') is not null
drop database MVC_Restaurant
go
Create Database MVC_Restaurant;
go
use MVC_Restaurant;
go
Create Table Item
(
	ID Int Identity Primary key,
	Item_Name Varchar(30),
	Unit_Price Money,
	Picture varchar(max),
);

Create Table Customer
(
	
	ID Int Identity Primary key,
	Cus_Name Varchar(30),
	Cus_Address varchar(50),
	Cus_Contact_NO Varchar(20),
	Cus_Email Varchar(20)
);


Create Table Food_Order
(
	ID Int Identity Primary key,
	Cus_Id Int References Customer(ID) ON DELETE CASCADE,
	Item_Id Int References Item(ID) ON DELETE CASCADE,
	OrderDate date,
	Quantity Int,
	Total_Price Money
);

Create Table Bill
(
	ID Int Identity Primary key,
	Order_Id Int References Food_Order(ID) ON DELETE CASCADE,
	Total_Price Money default 0,
	Payment_Method varchar(30)
);
go

create proc Sp_ins_order
@CusId int,@ItemId int,@Quantity int
AS
Begin
	declare @price money,@totalPrice money 
	select @price= Unit_Price  from Item where ID= @ItemId
	set @totalPrice= @price*@Quantity;
	Insert Food_Order (Cus_Id,Item_Id,Quantity,Total_Price)
	Values(@CusId,@ItemId,@Quantity,@totalPrice)
End
Go
create proc Sp_insBill
@ordId int,  @PayMath Varchar(30)
As
Begin
		Declare @tprice money
		select @tprice=Total_Price from Food_Order where ID=@ordId
		Insert Bill(Order_Id,Total_Price, Payment_Method) Values(@ordId,@tprice, @PayMath)
End
go

insert into Item(Item_Name,Unit_Price) 
	values	('Biriyani',200),
			('Kacchi',250),
			('Teheri',150),
			('Beef curry',180),
			('Mutton curry',220),
			('Chicken',150);
go
insert into Customer
	values	('Sohag','Dhanmondi','+8801736934574','sohag@Gmail.com'),
			('Mili','Mohammadpur','+8801747034474','mili@Gmail.com'),
			('Imran','Mohammadpur','+8801739724574','imran@Gmail.com'),
			('Mehedi','Mirpur','+880112344574','mehedi@Gmail.com'),
			('Rayhan','Mirpur','+8801736939876','rayhan@Gmail.com');
go
exec Sp_ins_order 1,1,2;
exec Sp_ins_order 1,2,3;
exec Sp_ins_order 2,3,4;
exec Sp_ins_order 3,4,3;
exec Sp_ins_order 4,5,3;
exec Sp_ins_order 5,6,4;
exec Sp_ins_order 5,1,3;
go
Exec Sp_insBill 1,'Cash On Delivery';
Exec Sp_insBill 2,'Cash On Delivery';
Exec Sp_insBill 3,'Cash On Delivery';
Exec Sp_insBill 4,'Cash On Delivery';
Exec Sp_insBill 5,'Cash On Delivery';
Exec Sp_insBill 6,'Cash On Delivery';
Exec Sp_insBill 7,'Cash On Delivery';